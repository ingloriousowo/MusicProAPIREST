using Microsoft.Data.SqlClient;
using Moq;
using MusicProAPIREST.Models;
using MusicProAPIREST.Services;

namespace PruebasUnitarias
{
    public class TestArticulo
    {
        // Get Articulos
        [Test]
        public void GetArticulos_True()
        {
            var mockArticulosService = new Mock<ArticuloService>();

            var listaArticulos = new List<Articulo>()
            {
                new Articulo()
                {
                    idProducto = 1,
                    nombreProducto = "Producto 1",
                    stockDisponible = 10,
                    precio = 10
                },
                new Articulo()
                {
                    idProducto = 2,
                    nombreProducto = "Producto 2",
                    stockDisponible = 5,
                    precio = 15
                }
            };

            mockArticulosService.Setup(x => x.GetArticulos()).Returns(listaArticulos);

            var articuloService = mockArticulosService.Object;
            var resultado = articuloService.GetArticulos();

            Assert.NotNull(resultado);
            Assert.That(resultado.Count, Is.EqualTo(listaArticulos.Count));
            Assert.That(resultado[0].idProducto, Is.EqualTo(listaArticulos[0].idProducto));
            Assert.That(resultado[0].nombreProducto, Is.EqualTo(listaArticulos[0].nombreProducto));
            Assert.That(resultado[0].stockDisponible, Is.EqualTo(listaArticulos[0].stockDisponible));
            Assert.That(resultado[0].precio, Is.EqualTo(listaArticulos[0].precio));

            mockArticulosService.Verify(a => a.GetArticulos(), Times.Once);
        }

        // Get Articulos Por ID
        [Test]
        public void GetArticuloPorID_RetornaCorrectamente()
        {
            var mockArticulosService = new Mock<ArticuloService>();

            mockArticulosService.Setup(x => x.GetArticuloPorId(1)).Returns(new Articulo()
            {
                idProducto = 1,
                nombreProducto = "Producto de Prueba",
                stockDisponible = 10,
                precio = 10
            });

            var articuloService = mockArticulosService.Object;
            var resultado = articuloService.GetArticuloPorId(1);

            Assert.NotNull(resultado);
            Assert.AreEqual(1, resultado.idProducto);
            Assert.AreEqual("Producto de Prueba", resultado.nombreProducto);
            Assert.AreEqual(10, resultado.stockDisponible);
            Assert.AreEqual(10, resultado.precio);

            mockArticulosService.Verify(a => a.GetArticuloPorId(1), Times.Once);

        }

        [Test]
        public void GetArticuloPorID_RetornaInexistente()
        {
            var mockArticulosService = new Mock<ArticuloService>();

            int id = 10;
            string error = "Articulo con la id: " + id + ", no encontrado...";

            mockArticulosService.Setup(x => x.GetArticuloPorId(id)).Returns(error);

            var articuloService = mockArticulosService.Object;
            var resultado = articuloService.GetArticuloPorId(id);

            Assert.AreEqual(error, resultado);

            mockArticulosService.Verify(a => a.GetArticuloPorId(id), Times.Once);

        }

        // Agregar Articulo
        [Test]
        public void AgregarArticulo_RetornoCorrectamente()
        {
            var mockArticulosService = new Mock<ArticuloService>();
            string resultadoString = "Articulo Guardado exitosamente.";
            Articulo articulo = new Articulo()
            {
                idProducto = 1,
                nombreProducto = "Producto de Prueba",
                stockDisponible = 10,
                precio = 10
            };
            mockArticulosService.Setup(x => x.AgregarArticulo(articulo)).Returns(resultadoString);

            var articuloService = mockArticulosService.Object;
            var resultado = articuloService.AgregarArticulo(articulo);

            Assert.That(resultado, Is.EqualTo(resultadoString));
            mockArticulosService.Verify(a => a.AgregarArticulo(articulo), Times.Once);
        }

        
        [Test]
        public void AgregarArticulo_RetornoErroneo()
        {
            var mockArticulosService = new Mock<ArticuloService>();
            string errorMessage = "Error al guardar el articulo: Error de ejemplo";
            Articulo articulo = new Articulo()
            {
                idProducto = 1,
                nombreProducto = "Producto de Prueba",
                stockDisponible = 10,
                precio = 10
            };

            mockArticulosService.Setup(x => x.AgregarArticulo(articulo)).Throws(new Exception("Error de ejemplo"));

            var articuloService = mockArticulosService.Object;

            Assert.Throws<Exception>(() => articuloService.AgregarArticulo(articulo), errorMessage);
            mockArticulosService.Verify(a => a.AgregarArticulo(articulo), Times.Once);
        }

        // Modificar Articulo
        [Test]
        public void ModificarArticulo_ArticuloModificadoCorrectamente()
        {
            var mockArticulosService = new Mock<ArticuloService>();
            int id = 1;
            string successMessage = $"Articulo {id} fue modificado con exito";
            Articulo articulo = new Articulo()
            {
                nombreProducto = "Producto Modificado",
                stockDisponible = 20,
                precio = 20
            };

            mockArticulosService.Setup(x => x.ModificarArticulo(id, articulo)).Returns(successMessage);

            var articuloService = mockArticulosService.Object;
            var resultado = articuloService.ModificarArticulo(id, articulo);

            Assert.AreEqual(successMessage, resultado);
            mockArticulosService.Verify(a => a.ModificarArticulo(id, articulo), Times.Once);
        }

        [Test]
        public void ModificarArticulo_ArticuloNoExiste()
        {
            var mockArticulosService = new Mock<ArticuloService>();
            int id = 1;
            string notFoundMessage = $"Articulo con el ID: {id}, no existe";
            Articulo articulo = new Articulo()
            {
                nombreProducto = "Producto Modificado",
                stockDisponible = 20,
                precio = 20
            };

            mockArticulosService.Setup(x => x.ModificarArticulo(id, articulo)).Returns(notFoundMessage);

            var articuloService = mockArticulosService.Object;
            var resultado = articuloService.ModificarArticulo(id, articulo);

            Assert.AreEqual(notFoundMessage, resultado);
            mockArticulosService.Verify(a => a.ModificarArticulo(id, articulo), Times.Once);
        }

        [Test]
        public void ModificarArticulo_ErrorAlModificar()
        {
            var mockArticulosService = new Mock<ArticuloService>();
            int id = 1;
            string errorMessage = "Error al Modificar el articulo: Error de ejemplo";
            Articulo articulo = new Articulo()
            {
                nombreProducto = "Producto Modificado",
                stockDisponible = 20,
                precio = 20
            };

            mockArticulosService.Setup(x => x.ModificarArticulo(id, articulo)).Throws(new Exception("Error de ejemplo"));

            var articuloService = mockArticulosService.Object;

            Assert.Throws<Exception>(() => articuloService.ModificarArticulo(id, articulo), errorMessage);
            mockArticulosService.Verify(a => a.ModificarArticulo(id, articulo), Times.Once);
        }

        // Eliminar Articulo
        [Test]
        public void EliminarArticulo_ArticuloEliminadoCorrectamente()
        {
            var mockArticulosService = new Mock<ArticuloService>();
            int id = 1;
            string successMessage = $"Articulo {id} Borrado exitosamente.";

            mockArticulosService.Setup(x => x.EliminarArticulo(id)).Returns(successMessage);

            var articuloService = mockArticulosService.Object;
            var resultado = articuloService.EliminarArticulo(id);

            Assert.AreEqual(successMessage, resultado);
            mockArticulosService.Verify(a => a.EliminarArticulo(id), Times.Once);
        }

        [Test]
        public void EliminarArticulo_ArticuloNoExiste()
        {
            var mockArticulosService = new Mock<ArticuloService>();
            int id = 1;
            string notFoundMessage = $"Articulo con el ID: {id}, no existe";

            mockArticulosService.Setup(x => x.EliminarArticulo(id)).Returns(notFoundMessage);

            var articuloService = mockArticulosService.Object;
            var resultado = articuloService.EliminarArticulo(id);

            Assert.AreEqual(notFoundMessage, resultado);
            mockArticulosService.Verify(a => a.EliminarArticulo(id), Times.Once);
        }

        [Test]
        public void EliminarArticulo_ErrorAlEliminar()
        {
            var mockArticulosService = new Mock<ArticuloService>();
            int id = 1;
            string errorMessage = "Error al Eliminar el articulo: Error de ejemplo";

            mockArticulosService.Setup(x => x.EliminarArticulo(id)).Throws(new Exception("Error de ejemplo"));

            var articuloService = mockArticulosService.Object;

            Assert.Throws<Exception>(() => articuloService.EliminarArticulo(id), errorMessage);
            mockArticulosService.Verify(a => a.EliminarArticulo(id), Times.Once);
        }

    }
}