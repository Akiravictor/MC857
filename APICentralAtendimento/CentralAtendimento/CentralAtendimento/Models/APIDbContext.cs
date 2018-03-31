using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CentralAtendimento.Models
{
    public class APIDbContext : DbContext
    {
        public APIDbContext() : base("name=APIDbContext")
        {
        }

        public DbSet<Tickets> TicketsDb { get; set; }

        public DbSet<Messages> MessagesDb { get; set; }

        public DbSet<SiteRegisters> SiteResitersDb { get; set; }
    }
}