using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ScanQRCodeLoginSample.Models
{
    public class ScanQRCodeDTO
    {
        [JsonProperty("connectionId")]
        public string ConnectionID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
