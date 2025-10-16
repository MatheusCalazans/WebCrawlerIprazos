using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebCrawlerIprazos.Models;

namespace WebCrawlerIprazos.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Execucao> Execucoes => Set<Execucao>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=crawler.db");
    }
}
