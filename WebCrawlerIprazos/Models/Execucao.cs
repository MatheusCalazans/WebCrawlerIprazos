using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawlerIprazos.Models
{
    public class Execucao
    {
        public int Id { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFinal { get; set; }
        public int PaginasProcessadas { get; set; }
        public int TotalRegistros { get; set; }
        public string CaminhoJson { get; set; } = "";
    }
}
