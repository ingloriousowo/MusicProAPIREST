using Microsoft.AspNetCore.Mvc;
using MusicProAPIREST.Models;
using MusicProAPIREST.Services;
namespace MusicProAPIREST.Controllers
{
    [ApiController]
    [Route("articulos")]
    public class ArticuloController
    {
        private readonly ArticuloService _ars;
        public ArticuloController(ArticuloService ars)
        {
            _ars = ars;
        }

        [HttpGet]
        [Produces("application/json")]
        public List<Articulo> getArticulos()
        {
            return _ars.GetArticulos();
        }

        

        [HttpGet("{id}")]
        [Produces("application/json")]
        public dynamic getArticuloPorID(int id)
        {
            return _ars.GetArticuloPorId(id);
        }

        
        [HttpPost]
        public dynamic PostArticulo([FromBody] Articulo articulo)
        {
            return _ars.AgregarArticulo(articulo);
        }
        

        [HttpPut("{id}")]
        public dynamic putArticulo(int id, [FromBody] Articulo articulo)
        {
            return _ars.ModificarArticulo(id, articulo);
        }


        [HttpDelete("{id}")]
        public dynamic deleteArticulo(int id)
        {
            return _ars.EliminarArticulo(id);
        }

    }
}
