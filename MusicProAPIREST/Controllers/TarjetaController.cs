using Microsoft.AspNetCore.Mvc;
using MusicProAPIREST.Models;
using MusicProAPIREST.Services;

namespace MusicProAPIREST.Controllers
{
    [ApiController]
    [Route("Tarjeta")]
    public class TarjetaController
    {
        private readonly TarjetaService _ars;
        public TarjetaController(TarjetaService ars)
        {
            _ars = ars;
        }

        [HttpGet]
        [Produces("application/json")]
        public List<Tarjeta> getTarjeta()
        {
            return _ars.GetTarjeta();
        }


        [HttpGet("{id}")]
        [Produces("application/json")]
        public dynamic getTarjetaPorID(int id)
        {
            return _ars.GetTarjetaPorId(id);
        }


        [HttpPost]
        public dynamic PostTarjeta([FromBody] Tarjeta articulo)
        {
            _ars.AgregarTarjeta(articulo);
            return _ars.getLista();
        }


        [HttpPut("{id}")]
        public dynamic putTarjeta(int id, [FromBody] Tarjeta articulo)
        {
            return _ars.ModificarTarjeta(id, articulo);
        }

        [HttpDelete("{id}")]
        public dynamic deleteTarjeta(int id)
        {
            return _ars.EliminarTarjeta(id);
        }



        [HttpPost("{idProducto}/transaccion")]
        public IActionResult RealizarTransaccion(int idProducto, [FromBody] int montoTransaccion)
        {
            string resultado = _ars.RealizarTransaccion(idProducto, montoTransaccion);
            return new OkObjectResult(resultado);
        }
    }

}
