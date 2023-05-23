namespace MusicProAPIREST.Models
{
    public class Articulo
    {
        public int idProducto { get; set; }
        public string? nombreProducto { get; set; }
        public int stockDisponible { get; set; }
        public int precio { get; set; }

    }
}
