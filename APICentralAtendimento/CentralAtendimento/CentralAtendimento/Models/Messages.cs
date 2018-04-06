using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralAtendimento.Models
{
	/// <summary>
	/// Estrutura de Mensagens do Ticket
	/// </summary>
    public class Messages
    {
		/// <summary>
		/// ID interno de uso do Banco de Dados
		/// </summary>
        [JsonIgnore]
        public int Id { get; set; }

		/// <summary>
		/// Timestamp no formato yyyy-MM-ddTHH:mm
		/// </summary>
		[JsonConverter(typeof(CustomDateConverter))]
        public DateTime Timestamp { get; set; }

		/// <summary>
		/// Identificador de quem enviou a mensagem
		/// </summary>
        public string Sender { get; set; }

		/// <summary>
		/// Mensagem dentro do Ticket
		/// </summary>
        public string Message { get; set; }
    }
}