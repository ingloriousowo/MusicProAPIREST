using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MusicProAPIREST.Models;

namespace MusicProAPIREST.Services
{

    public class TarjetaService
    {
        string cs = "";

        public TarjetaService(IConfiguration config)
        {
            IConfiguration configuration = config;
            cs = configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!;
        }

        public List<Tarjeta> GetTarjeta()
        {
            using var conn = new SqlConnection(cs);
            conn.Open();
            List<Tarjeta> lista = new List<Tarjeta>();

            var command = new SqlCommand(
                "Select * from tarjeta", conn
                );
            using SqlDataReader reader = command.ExecuteReader();
            if (reader != null)
            {
                while (reader.Read())
                {
                    Tarjeta tarjeta = new Tarjeta();
                    tarjeta.idTarjeta = reader.GetInt32(0);
                    tarjeta.Nombre = reader.GetString(1);
                    tarjeta.Saldo = reader.GetInt32(2);

                    lista.Add(tarjeta);
                }
            }

            return lista;
        }

        public dynamic GetTarjetaPorId(int id)
        {
            Tarjeta tarjeta = new Tarjeta();
            using var conn = new SqlConnection(cs);
            conn.Open();

            var command = new SqlCommand(
                "Select * from tarjeta where id = " + id, conn
                );

            using SqlDataReader reader = command.ExecuteReader();
            if (reader != null)
            {
                while (reader.Read())
                {
                    tarjeta.idTarjeta = reader.GetInt32(0);
                    tarjeta.Nombre = reader.GetString(1);
                    tarjeta.Saldo = reader.GetInt32(2);
                }

                return tarjeta;
            }
            else
            {
                return "tarjeta con la id: " + id + ", no encontrado...";
            };
        }

        public string AgregarTarjeta(Tarjeta tarjeta)
        {
            using var conn = new SqlConnection(cs);
            conn.Open();

            var command = new SqlCommand(
                $"insert into Articulo(nombre, saldo) values('{tarjeta.Nombre}',{tarjeta.Saldo})", conn
                );
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                return "Articulo Guardado exitosamente.";

            }
            catch (Exception ex)
            {
                return "Error al guardar el articulo: " + ex.Message;
            }
        }

        public dynamic ModificarTarjeta(int id, Tarjeta tarjeta)
        {
            using var conn = new SqlConnection(cs);
            conn.Open();

            var command = new SqlCommand(
                $"update tarjeta set nombre ='{tarjeta.Nombre}', saldo = {tarjeta.Saldo}, where id = {id}", conn
                );
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.RecordsAffected != 0)
                {
                    return $"tarjeta {id} fue modificado con exito";
                }
                else
                {
                    return $"tarjeta con el ID: {id}, no existe";
                }

            }
            catch (Exception ex)
            {
                return "Error al Modificar el tarjeta: " + ex.Message;
            }
        }

        public dynamic EliminarTarjeta(int id)
        {
            using var conn = new SqlConnection(cs);
            conn.Open();

            var command = new SqlCommand(
                $"delete tarjeta where id = {id}", conn
                );

            try
            {
                using SqlDataReader reader = command.ExecuteReader();

                if (reader.RecordsAffected != 0)
                {
                    return $"tarjeta {id} Borrado exitosamente.";
                }
                else
                {
                    return $"tarjeta con el ID: {id}, no existe";
                }


            }
            catch (Exception ex)
            {
                return "Error al Modificar el articulo: " + ex.Message;
            }
        }


        public dynamic RealizarTransaccion(int cardNumber, int montoTransaccion)
        {
            List<Tarjeta> lista = new List<Tarjeta>();

            // Realizar la conexión a la base de datos
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();

                // Obtener la tarjeta específica por su id de la base de datos
                var command = new SqlCommand(
                    "SELECT * FROM tarjeta WHERE cardNumber = @cardNumber", conn
                );
                command.Parameters.AddWithValue("@cardNumber", cardNumber);

                using SqlDataReader reader = command.ExecuteReader();

                // Verificar si se encontró la tarjeta en la base de datos
                if (reader.HasRows)
                {
                    // Leer los datos de la tarjeta
                    reader.Read();
                    Tarjeta tarjeta = new Tarjeta();
                    tarjeta.idTarjeta = reader.GetInt32(0);
                    tarjeta.cardNumber = reader.GetInt32(1);
                    tarjeta.Nombre = reader.GetString(2);
                    tarjeta.Saldo = reader.GetInt32(3);

                    if (tarjeta == null)
                    {
                        return "El id no existe.";
                    }

                    if (montoTransaccion > tarjeta.Saldo)
                    {
                        return "No se pudo realizar la transacción de " + tarjeta.Nombre + ", Saldo insuficiente.";
                    }

                    // Realizar la transacción actualizando el saldo en la base de datos
                    tarjeta.Saldo -= montoTransaccion;
                    var updateCommand = new SqlCommand(
                        "UPDATE tarjeta SET Saldo = @saldo WHERE cardNumber = @cardNumber", conn
                    );
                    updateCommand.Parameters.AddWithValue("@saldo", tarjeta.Saldo);
                    updateCommand.Parameters.AddWithValue("@cardNumber", cardNumber);
                    updateCommand.ExecuteNonQuery();

                    return "Transacción realizada exitosamente.";
                }
                else
                {
                    return "El id no existe.";
                }
            }
        }
    }
}
