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
                if(carroID != carroActualID)
                {
                    lista.Add(carroActual);
                    carroActual = new Carro();
                    carroActualID = carroID;                   
                }
                carroActual.ID = carroID;
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

        public Carro getCarroPorID(int id)
        {
            using var conn = new SqlConnection(cs);
            conn.Open();

            var command = new SqlCommand(
                "select c.id_Carro, a.id, a.nombre, a.stock_disponible, a.precio, ac.cantidad " +
                "from carro c " +
                "join Articulo_carro ac ON c.id_Carro = ac.id_carro " +
                "join Articulo a on ac.id_articulo = a.id " +
                $"where c.id_Carro = {id}"
                , conn);

            using SqlDataReader reader = command.ExecuteReader();
            Carro carro = new Carro();
            if(reader != null)
            {
                while (reader.Read())
                {
                    carro.ID = reader.GetInt32(0);

                    ArticuloCarro articulo = new ArticuloCarro();
                    articulo.idProducto = reader.GetInt32(1);
                    articulo.nombreProducto = reader.GetString(2);
                    articulo.precio = reader.GetInt32(4);
                    articulo.cantidad = reader.GetInt32(5);
                    carro.articulos.Add(articulo);
                    int cantidad = reader.GetInt32(5);
                    carro.total_Carro += articulo.precio * cantidad;
                }
                return carro;
            }
            else
            {
                return carro;
            }
            

        }

        public string crearCarro()
        {
            using var conn = new SqlConnection(cs);
            conn.Open();
            var command = new SqlCommand("insert into Carro(total_carro) values (0)",conn);
            using SqlDataReader reader = command.ExecuteReader();

            if(reader.RecordsAffected != 0)
            {
                return "Nuevo Carro Creado!";
            }
            else
            {
                return "Hubo un error al crear el Carro";
            }
        }

        public string agregarArticuloCarro( int id_producto, int id_Carro, int cantidad)
        {
            using var conn = new SqlConnection(cs); 
            conn.Open();

            var command = new SqlCommand($"select * from carro where id_carro = {id_Carro}",conn);
            using SqlDataReader reader_carro = command.ExecuteReader();
            bool existeCarro = reader_carro.Read();
            reader_carro.Close();
            command = new SqlCommand($"select * from articulo where id = {id_producto}", conn);
            using SqlDataReader reader_articulo = command.ExecuteReader();
            bool existeArticulo = reader_articulo.Read();
            reader_articulo.Close();

            if (existeArticulo && existeCarro)
            {
                var command_inside = new SqlCommand($"insert into Articulo_Carro values ({id_producto},{id_Carro},{cantidad})", conn);
                command_inside.BeginExecuteReader();
                int cantidadActual = getTotalCarro(id_Carro);
                int precioArticulo = getArticuloPrecio(id_producto);                
                cantidadActual += precioArticulo * cantidad;
                actualizarTotalCarro(cantidadActual, id_Carro);


                return "Articulo Agregado con exito!";
            }
            else if (existeCarro && !existeArticulo) 
            {
                return "El Articulo seleccionado No Existe";
            }
            else if(existeArticulo && !existeCarro)
            {
                return "El Carro seleccionado No Existe";
            }
            else
            {
                return "Ocurrio un error agregando el Articulo";
            }
        }

        private int getArticuloPrecio(int id_producto)
        {
            using var conn = new SqlConnection(cs); conn.Open();
            using var command = new SqlCommand($"select precio from articulo where id={id_producto}", conn);
            using SqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                int precioArticulo = reader.GetInt32(0);
                reader.Close();
                return precioArticulo;
            }
            reader.Close();
            return 0;
        }

        public int getTotalCarro(int id_Carro)
        {
            using var conn = new SqlConnection(cs); conn.Open();
            using var command = new SqlCommand($"select total_Carro from carro where id_Carro = {id_Carro}", conn);
            using SqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                int cantidadActual = reader.GetInt32(0);
                reader.Close();
                return cantidadActual;
            }
            reader.Close();
            return 0;
        }

        public void actualizarTotalCarro(int total, int id_Carro)
        {
            using var conn = new SqlConnection(cs); conn.Open();
            using var command = new SqlCommand($"update carro set total_carro = {total} where id_Carro = {id_Carro}", conn);
            command.BeginExecuteReader();
        }
    }
}
