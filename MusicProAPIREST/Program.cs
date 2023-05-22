using Microsoft.Data.SqlClient;
using MusicProAPIREST.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ArticuloService>();
builder.Services.AddScoped<TarjetaService>();
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
        while (reader.Read())
        {
            Console.WriteLine(reader.GetString(1));
        }
    }

}catch (Exception ex)
{
    Console.WriteLine(ex.Message);
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
