using Microsoft.AspNetCore.Mvc;
using MusicProAPIREST.Models;

namespace MusicProAPIREST.Services
{
    public class TarjetaService
    {
        private List<Tarjeta> listaTarjeta = new List<Tarjeta>
    {
        new Tarjeta {idTarjeta = 1, Nombre = "Juan", Saldo = 200000},
        new Tarjeta {idTarjeta = 2, Nombre = "Martin", Saldo = 300000},
        new Tarjeta {idTarjeta = 3, Nombre = "Sebastian", Saldo = 100000}
    };

        public List<Tarjeta> GetTarjeta()
        {
            return listaTarjeta;
        }

        public dynamic GetTarjetaPorId(int id)
        {
            var Tarjeta = listaTarjeta.FirstOrDefault(a => a.idTarjeta == id);
            if (Tarjeta != null)
            {
                return Tarjeta;
            }
            return "Articulo con ID: " + id + ", no existe";
        }

        public void AgregarTarjeta(Tarjeta tarjeta)
        {
            tarjeta.idTarjeta = listaTarjeta.Last().idTarjeta + 1;
            listaTarjeta.Add(tarjeta);
        }

        public dynamic ModificarTarjeta(int id, Tarjeta tarjeta)
        {
            int index = listaTarjeta.FindIndex(a => a.idTarjeta == id);
            if (index != -1)
            {
                listaTarjeta[index].Nombre = tarjeta.Nombre;
                listaTarjeta[index].Saldo = tarjeta.Saldo;
                return listaTarjeta[index];
            }
            else
            {
                return "Articulo con ID: " + id + ", no existe";
            }
        }

        public dynamic EliminarTarjeta(int id)
        {
            int index = listaTarjeta.FindIndex(a => a.idTarjeta == id);
            if (index != -1)
            {
                listaTarjeta.RemoveAt(index);
                return listaTarjeta;
            }
            else
            {
                return "Articulo con ID: " + id + ", no existe";
            }
        }

        public List<Tarjeta> getLista()
        {
            return listaTarjeta;
        }

        public dynamic RealizarTransaccion(int idTarjeta, int montoTransaccion)
        {
            Tarjeta tarjeta = listaTarjeta.FirstOrDefault(a => a.idTarjeta == idTarjeta);

            if (tarjeta == null)
            {
                return "El id no existe.";
            }

            if (montoTransaccion > tarjeta.Saldo)
            {
                return "No se pudo realizar la transacción de " + tarjeta.Nombre + ", Saldo insuficiente.";
            }

            tarjeta.Saldo -= montoTransaccion;

            return "Transacción realizada exitosamente.";
        }
    }
}
