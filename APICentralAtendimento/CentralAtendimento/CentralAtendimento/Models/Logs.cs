using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralAtendimento.Models
{
    public class Logs
    {
        public int TicketSize { get; set; }

        public List<Tickets> TicketsList { get; set; }
    }
}