using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alquran.model
{
    class DataQuran
    {
        [JsonProperty(PropertyName = "nomor")]
        public string Nomor { get; set; }

        [JsonProperty(PropertyName = "nama")]
        public string Nama { get; set; }

        [JsonProperty(PropertyName = "nama_latin")]
        public string Latin { get; set; }

        [JsonProperty(PropertyName = "jumlah_ayat")]
        public string Jumlah { get; set; }

        [JsonProperty(PropertyName = "tempat_turun")]
        public string Tempat { get; set; }

        [JsonProperty(PropertyName = "arti")]
        public string Arti { get; set; }

        [JsonProperty(PropertyName = "deskripsi")]
        public string Deskripsi { get; set; }

        [JsonProperty(PropertyName = "audio")]
        public string Audio { get; set; }

    }
}
