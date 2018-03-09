using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanQRCodeLoginSample.Models.JWT
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
    }
}
