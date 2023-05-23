using MusicProAPIREST.Models;
using Microsoft.Data.SqlClient;

namespace MusicProAPIREST.Services
{
    public class ArticuloService
    {
        string cs = "";
        
        public ArticuloService(IConfiguration config)
        {
            IConfiguration configuration = config;
            cs = configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!;
        }

        public List<Articulo> GetArticulos()
        {
            using var conn = new SqlConnection(cs);
            conn.Open();
            List<Articulo> lista = new List<Articulo>();

            var command = new SqlCommand(
                "Select * from articulo", conn
                );

            using SqlDataReader reader = command.ExecuteReader();
            if( reader != null )
            {
                while( reader.Read())
                {
                    Articulo articulo = new Articulo();
                    articulo.idProducto = reader.GetInt32(0);
                    articulo.nombreProducto = reader.GetString(1);
                    articulo.stockDisponible = reader.GetInt32(2);
                    articulo.precio = reader.GetInt32(3);

                    lista.Add(articulo);
                }
            }

            return lista;
        }
        
        public dynamic GetArticuloPorId(int id)
        {
            Articulo articulo = new Articulo();
            using var conn = new SqlConnection(cs);
            conn.Open();

            var command = new SqlCommand(
                "Select * from articulo where id = "+id, conn
                );

            using SqlDataReader reader = command.ExecuteReader();
            if(reader != null)
            {
                while (reader.Read())
                {                   
                    articulo.idProducto = reader.GetInt32(0);
                    articulo.nombreProducto = reader.GetString(1);
                    articulo.stockDisponible = reader.GetInt32(2);
                    articulo.precio = reader.GetInt32(3);
                }

                return articulo;
            }
            else
            {
                return "Articulo con la id: "+id+", no encontrado...";
            }
        }


        public string AgregarArticulo(Articulo articulo)
        {
            using var conn = new SqlConnection(cs);
            conn.Open();

            var command = new SqlCommand(
                $"insert into Articulo(nombre, stock_disponible, precio) values('{articulo.nombreProducto}',{articulo.stockDisponible},{articulo.precio})", conn
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
        
        
        public dynamic ModificarArticulo(int id, Articulo articulo)
        {
            using var conn = new SqlConnection(cs);
            conn.Open();

            var command = new SqlCommand(
                $"update articulo set nombre ='{articulo.nombreProducto}', stock_disponible = {articulo.stockDisponible}, precio = {articulo.precio} where id = {id}", conn
                );
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                if(reader.RecordsAffected != 0)
                {
                    return $"Articulo {id} fue modificado con exito";
                }
                else
                {
                    return $"Articulo con el ID: {id}, no existe";
                }

            }
            catch (Exception ex)
            {
                return "Error al Modificar el articulo: " + ex.Message;
            }

        }
               
        public dynamic EliminarArticulo(int id)
        {
            using var conn = new SqlConnection(cs);
            conn.Open();

            var command = new SqlCommand(
                $"delete articulo where id = {id}", conn
                );

            try
            {
                using SqlDataReader reader = command.ExecuteReader();

                if(reader.RecordsAffected != 0)
                {
                    return $"Articulo {id} Borrado exitosamente.";
                }
                else
                {
                    return $"Articulo con el ID: {id}, no existe";
                }
                

            }
            catch (Exception ex)
            {
                return "Error al Modificar el articulo: " + ex.Message;
            }
        }
        
    }

}
