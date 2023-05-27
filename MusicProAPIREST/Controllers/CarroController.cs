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

        [HttpGet("{id}")]
        [Produces("application/json")]
        public Carro getCarroPorID(int id)
        {
            return _crrs.getCarroPorID(id);
        }

        [HttpPut]
        public string postCarro()
        {
            return _crrs.crearCarro();
        }

        [HttpPut]
        [Route("/agregarArticulo")]
        public string AgregarArticulo(int id_articulo, int id_Carro, int cantidad)
        {
            return _crrs.agregarArticuloCarro(id_articulo, id_Carro, cantidad);
        }
    }
}
