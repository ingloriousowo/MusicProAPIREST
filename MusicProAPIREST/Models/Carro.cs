using MusicProAPIREST.Models;
using Newtonsoft.Json;

namespace MusicProAPIREST.Models
{
    public class Carro
    {
        public int ID { get; set; }

        [JsonIgnore]
        public List<ArticuloCarro> articulos = new List<ArticuloCarro>();

        [JsonProperty("articulos")]
        public List<ArticuloCarro> articulos_carro => articulos;
        public int total_Carro { get; set; } = 0;

    }
}
