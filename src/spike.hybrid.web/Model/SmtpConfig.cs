using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace spike.hybrid.web
{
    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public string FromAddress { get; set; }

        public int Port { get; set; }

        public bool EnableSSL { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}