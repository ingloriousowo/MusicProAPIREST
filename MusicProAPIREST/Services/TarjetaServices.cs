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
                    tarjeta.cardNumber = reader.GetString(1);
                    tarjeta.Nombre = reader.GetString(2);
                    tarjeta.Saldo = reader.GetInt32(3);

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
                    tarjeta.cardNumber = reader.GetString(1);
                    tarjeta.Nombre = reader.GetString(2);
                    tarjeta.Saldo = reader.GetInt32(3);
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


        public void RealizarCompra(string cardNumber, int carritoId)
        {
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();

                // Buscar la tarjeta por su número
                var tarjetaCommand = new SqlCommand("SELECT * FROM Tarjeta WHERE CardNumber = @CardNumber", conn);
                tarjetaCommand.Parameters.AddWithValue("@CardNumber", cardNumber);

                using (SqlDataReader tarjetaReader = tarjetaCommand.ExecuteReader())
                {
                    if (tarjetaReader.Read())
                    {
                        Int32 saldo = tarjetaReader.GetInt32(tarjetaReader.GetOrdinal("Saldo"));
                        tarjetaReader.Close();
                        // Buscar el carrito por su ID y cargar los artículos relacionados
                        var carritoCommand = new SqlCommand("SELECT * FROM carro WHERE id_Carro = @CarritoId", conn);
                        carritoCommand.Parameters.AddWithValue("@CarritoId", carritoId);

                        using (SqlDataReader carritoReader = carritoCommand.ExecuteReader())
                        {
                            if (carritoReader.Read())
                            {
                                Int32 total = carritoReader.GetInt32(carritoReader.GetOrdinal("total_Carro"));
                                carritoReader.Close();
                                // Verificar si el saldo es suficiente para realizar la compra
                                if (saldo >= total)
                                {
                                    // Descontar el saldo de la tarjeta
                                    saldo -= total;

                                    // Realizar otras operaciones relacionadas con la compra (por ejemplo, generar una factura)

                                    // Actualizar el saldo de la tarjeta en la base de datos
                                    var updateCommand = new SqlCommand("UPDATE Tarjeta SET Saldo = @Saldo WHERE CardNumber = @CardNumber", conn);
                                    updateCommand.Parameters.AddWithValue("@Saldo", saldo);
                                    updateCommand.Parameters.AddWithValue("@CardNumber", cardNumber);
                                    updateCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    throw new Exception("No tienes saldo suficiente para realizar la compra");
                                }
                            }
                            else
                            {
                                throw new Exception("El carrito no existe");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("La tarjeta no existe");
                    }
                }
            }
        }
        }
    }
