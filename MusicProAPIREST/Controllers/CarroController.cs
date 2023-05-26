using Microsoft.AspNetCore.Mvc;
using MusicProAPIREST.Models;
using MusicProAPIREST.Services;

namespace MusicProAPIREST.Controllers
{
    [ApiController]
    [Route("Carros")]
    public class CarroController
    {
        private readonly CarroServices _crrs;
        public CarroController(CarroServices crrs)
        {
            _crrs = crrs;
        }

        [HttpGet]
        [Produces("application/json")]
        public List<Carro> getCarros()
        {
            return _crrs.getCarros();
        }
    }
}
