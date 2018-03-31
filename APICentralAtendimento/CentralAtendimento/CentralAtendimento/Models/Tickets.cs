using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CentralAtendimento.Models
{
    public class Tickets
    {
        [JsonIgnore]
        public int Id{ get; set; }

        public string TicketId { get; set; }

        public string ClienteId { get; set; }

        public string CompraId { get; set; }

        public string SiteId { get; set; }

        public Status StatusId { get; set; }

        [NotMapped]
        public int MessageSize { get; set; }

        public List<Messages> MessagesList { get; set; }
    }
}