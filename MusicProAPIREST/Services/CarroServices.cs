using MusicProAPIREST.Models;
using Microsoft.Data.SqlClient;

namespace MusicProAPIREST.Services
{
    public class CarroServices
    {
        string cs = "";
        public CarroServices(IConfiguration config)
        {
            IConfiguration configuration = config;
            cs = configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!;
        }

        public List<Carro> getCarros()
        {
            List<Carro> lista = new List<Carro>();
            using var conn = new SqlConnection(cs);
            conn.Open();

            var command = new SqlCommand(
                "select c.id_Carro, a.id, a.nombre, a.stock_disponible, a.precio, ac.cantidad "+
                "from carro c " +
                "join Articulo_carro ac ON c.id_Carro = ac.id_carro " +
                "join Articulo a on ac.id_articulo = a.id"
                , conn);

            using SqlDataReader reader = command.ExecuteReader();
            int carroActualID = 1;
            Carro carroActual = new Carro();

            while (reader.Read())
            {
                int carroID = reader.GetInt32(0);
                carroActual.ID = carroID;
                if(carroID != carroActualID)
                {
                    carroActual = new Carro();
                    carroActualID = carroID;
                    lista.Add(carroActual);
                }

                ArticuloCarro articulo = new ArticuloCarro();
                articulo.idProducto = reader.GetInt32(1);
                articulo.nombreProducto = reader.GetString(2);
                articulo.precio = reader.GetInt32(4);
                articulo.cantidad = reader.GetInt32(5);
                carroActual.articulos.Add(articulo);
                int cantidad = reader.GetInt32(5);
                carroActual.total_Carro += articulo.precio * cantidad;             
            }
            lista.Add(carroActual);
            return lista;
        }
    }
}
