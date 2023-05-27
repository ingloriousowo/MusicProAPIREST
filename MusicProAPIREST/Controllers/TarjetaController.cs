using Azure.Core;
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
            try
            {
                _ars.RealizarCompra(request.CardNumber, request.carritoId);
                return new OkObjectResult("La compra se realizó correctamente");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public class TransaccionRequest
        {
            public string CardNumber { get; set; }
            public int carritoId { get; set; }
        }
    }
}
