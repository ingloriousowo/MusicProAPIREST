using Microsoft.Data.SqlClient;
using MusicProAPIREST.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ArticuloService>();
builder.Services.AddScoped<CarroServices>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

string connectionString = app.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!;

try
{
    using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand(
        "Select * from Articulo", conn
        );

    using SqlDataReader reader = command.ExecuteReader();
    if (reader.HasRows)
    {
        Console.WriteLine("Conexion a Base de datos Exitosa!");
    }
    else
    {
        Console.WriteLine("Sin valores en la Base de datos");
    }

}catch (Exception ex)
{
    Console.WriteLine("Fallo la conexion con la base de datos: "+ ex.Message);
}

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
