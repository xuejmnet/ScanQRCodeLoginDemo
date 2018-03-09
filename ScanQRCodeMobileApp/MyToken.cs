using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ScanQRCodeMobileApp
{
    public class MyToken
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
