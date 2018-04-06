using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralAtendimento.Models
{
	/// <summary>
	/// Status code para cada Ticket
	/// </summary>
    public enum Status
    {
		/// <summary>
		/// Representa um Ticket aberto
		/// </summary>
        Open = 0,

		/// <summary>
		/// Representa um Ticket fechado
		/// </summary>
		Closed = 1,

		/// <summary>
		/// Representa um Ticket cancelado
		/// </summary>
		Canceled = 2
    }
}