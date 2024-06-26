using EvolveDb;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using RestWithASPNET.API.Configurations;
using RestWithASPNET.API.Data;
using RestWithASPNET.API.Hypermedia.Enricher;
using RestWithASPNET.API.Hypermedia.Filters;
using RestWithASPNET.API.Mappings;
using RestWithASPNET.API.Repositories;
using RestWithASPNET.API.Repositories.Generic;
using RestWithASPNET.API.Repositories.Interfaces;
using RestWithASPNET.API.Services;
using RestWithASPNET.API.Services.Interfaces;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IBossService, BossService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<ProjetoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"
                ), b => b.MigrationsAssembly(typeof(ProjetoContext).Assembly.FullName)));

if (builder.Environment.IsDevelopment())
{
    MigrateDatabase();
}

builder.Services.AddMvc(options =>
{
    options.RespectBrowserAcceptHeader = true;

    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
    options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
})
.AddXmlSerializerFormatters();

var filterOptions = new HyperMediaFilterOptions();
filterOptions.ContentRespondeEnricherList.Add(new PersonEnricher());

builder.Services.AddSingleton(filterOptions);


void MigrateDatabase()
{
    try
    {
        var evolveConnection = new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
        var evolve = new Evolve(evolveConnection, msg => Log.Information(msg))
        {
            Locations = new[] { "db/migrations" },
            IsEraseDisabled = true,
        };

        evolve.Migrate();
    }
    catch (Exception ex)
    {
        Log.Error("Database migration failed.", ex);
        throw;
    }
}

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

//Versioning API
builder.Services.AddApiVersioning();


//Autenticação
var tokenConfiguration = new TokenConfiguration();

new ConfigureFromConfigurationOptions<TokenConfiguration>(
    builder.Configuration.GetSection("TokenConfiguration")
).Configure(tokenConfiguration);

builder.Services.AddSingleton(tokenConfiguration);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = tokenConfiguration.Issuer,
            ValidAudience = tokenConfiguration.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.Secret))
        };
    });

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser().Build());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();


app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute("DefaultApi", "v{version=apiVersion}/{controller=values}/{id?}");


//Garantir que o banco de dados seja criado
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProjetoContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
