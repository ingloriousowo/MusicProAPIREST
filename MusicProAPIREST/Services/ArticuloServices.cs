using MusicProAPIREST.Models;

namespace MusicProAPIREST.Services
{
    public class ArticuloService
    {
        private List<Articulo> listaArticulos = new List<Articulo>
    {
        new Articulo {idProducto = 1, nombreProducto = "Guitarra", categoria = "Intrumentos de Cuerdas"},
        new Articulo {idProducto = 2, nombreProducto = "Bateria", categoria = "Percusion"},
        new Articulo {idProducto = 3, nombreProducto = "Cajas", categoria = "Amplificadores"}
    };

        public List<Articulo> GetArticulos()
        {
            return listaArticulos;
        }

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
    }
}
