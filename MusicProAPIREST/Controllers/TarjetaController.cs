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
        public dynamic PostTarjeta([FromBody] Tarjeta tarjeta)
        {
            return _ars.AgregarTarjeta(tarjeta);
        }


        [HttpPut("{id}")]
        public dynamic putTarjeta(int id, [FromBody] Tarjeta tarjeta)
        {
            return _ars.ModificarTarjeta(id, tarjeta);
        }

        [HttpDelete("{id}")]
        public dynamic deleteTarjeta(int id)
        {
            return _ars.EliminarTarjeta(id);
        }



        [HttpPost("transaccion")]
        public IActionResult RealizarTransaccion([FromBody] TransaccionRequest request)
        {
            string resultado = _ars.RealizarTransaccion(request.CardNumber, request.MontoTransaccion);
            return new OkObjectResult(resultado);
        }
        public class TransaccionRequest
        {
            public int CardNumber { get; set; }
            public int MontoTransaccion { get; set; }
        }
    }

}
