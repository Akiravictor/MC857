using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CentralAtendimento.Models
{
	/// <summary>
	/// Estrutura dos Tickets
	/// </summary>
    public class Tickets
    {
		/// <summary>
		/// Id interno utilizado pelo Banco de Dados
		/// </summary>
        [JsonIgnore]
        public int Id{ get; set; }

		/// <summary>
		/// ID unico gerado pelo sistema
		/// </summary>
        public string TicketId { get; set; }

		/// <summary>
		/// ID representando o cliente
		/// </summary>
        public string ClienteId { get; set; }

		/// <summary>
		/// ID representando a compra
		/// </summary>
        public string CompraId { get; set; }

		/// <summary>
		/// ID do site que realizou a requisição (ID único para cada site)
		/// </summary>
        public string SiteId { get; set; }

		/// <summary>
		/// Código indicando o estado do Ticket
		/// </summary>
        public Status StatusId { get; set; }

		/// <summary>
		/// Tamanho do array de mensagens dentro do Ticket
		/// </summary>
        [NotMapped]
        public int MessageSize { get; set; }

		/// <summary>
		/// Lista de mensagens do Ticket
		/// </summary>
        public List<Messages> MessagesList { get; set; }
    }
}