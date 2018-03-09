using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScanQRCodeMobileApp
{
    public class ScanQRCodeDTO
    {
        [JsonProperty("connectionId")]
        public string ConnectionID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
