using EvolveDb;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Configuration;
using RestWithASPNET.API.Data;
using RestWithASPNET.API.Repositories;
using RestWithASPNET.API.Repositories.Interfaces;
using RestWithASPNET.API.Services;
using RestWithASPNET.API.Services.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

builder.Services.AddDbContext<ProjetoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"
                ), b => b.MigrationsAssembly(typeof(ProjetoContext).Assembly.FullName)));

if (builder.Environment.IsDevelopment())
{
    var connection = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection"
                ));

    MigrateDatabase(connection);
}


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

app.UseAuthorization();

app.MapControllers();


// Garantir que o banco de dados seja criado
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProjetoContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
