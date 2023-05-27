using Microsoft.AspNetCore.Mvc;
using MusicProAPIREST.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace tarjetaController
{
    [Route("RealizarPago")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly HttpClient httpClient;

        public ApiController()
        {
            httpClient = new HttpClient();
        }

        [HttpPost]
        public async Task<IActionResult> PostData([FromBody] TransaccionRequest datos)
        {
            string apiUrl = "http://localhost:5161/Tarjeta/transaccion";

            try
            {
                string json = JsonConvert.SerializeObject(datos);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    return Ok(responseData);
                }
                else if ((int)response.StatusCode == 400)
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    return BadRequest(errorResponse);
                }
                else
                {
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                // Puedes personalizar cómo deseas manejar la excepción aquí
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
