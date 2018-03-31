using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralAtendimento.Models
{
    public class Messages
    {
        [JsonIgnore]
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string Sender { get; set; }

        public string Message { get; set; }
    }
}