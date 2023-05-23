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
        /*
        public dynamic GetArticuloPorId(int id)
        {
            var articulo = listaArticulos.FirstOrDefault(a => a.idProducto == id);
            if(articulo != null)
            {
                return articulo;
            }
            return "Articulo con ID: " + id + ", no existe";
        }

        public void AgregarArticulo(Articulo articulo)
        {
            articulo.idProducto = listaArticulos.Last().idProducto + 1;
            listaArticulos.Add(articulo);
        }

        public dynamic ModificarArticulo(int id, Articulo articulo)
        {
            int index = listaArticulos.FindIndex(a => a.idProducto == id);
            if(index != -1)
            {
                listaArticulos[index].nombreProducto = articulo.nombreProducto;
                listaArticulos[index].categoria = articulo.categoria;
                return listaArticulos[index];
            }
            else
            {
                return "Articulo con ID: " + id + ", no existe";
            }
        }

        public dynamic EliminarArticulo(int id)
        {
            int index = listaArticulos.FindIndex(a => a.idProducto == id);
            if (index != -1)
            {
                listaArticulos.RemoveAt(index);
                return listaArticulos;
            }
            else
            {
                return "Articulo con ID: " + id + ", no existe";
            }
        }

        public List<Articulo> getLista()
        {
            return listaArticulos;
        }
        */
    }

}
