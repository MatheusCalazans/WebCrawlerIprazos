using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebCrawlerIprazos.Models
{
    public class Proxy
    {
        public string IpAddress { get; set; } = "";
        public string Port { get; set; } = "";
        public string Country { get; set; } = "";
        public string Protocol { get; set; } = "";

        [JsonIgnore] 
        public int PaginaOrigem { get; set; }
    }
}
