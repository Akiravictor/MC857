using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralAtendimento.Models
{
	/// <summary>
	/// Objeto para retorno dos Tickets
	/// </summary>
    public class Logs
    {
		/// <summary>
		/// Tamanho do array de Tickets
		/// </summary>
        public int TicketSize { get; set; }

		/// <summary>
		/// Lista de Tickets
		/// </summary>
        public List<Tickets> TicketsList { get; set; }
    }
}