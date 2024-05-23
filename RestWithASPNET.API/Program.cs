using EvolveDb;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
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

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IBossService, BossService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<ProjetoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"
                ), b => b.MigrationsAssembly(typeof(ProjetoContext).Assembly.FullName)));

if (builder.Environment.IsDevelopment())
{
    var connection = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection"
                ));

    MigrateDatabase(connection);
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


void MigrateDatabase(SqlConnectionStringBuilder connection)
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute("DefaultApi", "v{version=apiVersion}/{controller=values}/{id?}");


// Garantir que o banco de dados seja criado
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProjetoContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
