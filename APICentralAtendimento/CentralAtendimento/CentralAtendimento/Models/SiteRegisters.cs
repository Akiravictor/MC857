using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralAtendimento.Models
{
	/// <summary>
	/// Registro de cada Site com contrato firmado com a API
	/// </summary>
    public class SiteRegisters
    {
		/// <summary>
		/// Id interno ao Banco de dados
		/// </summary>
        public int Id { get; set; }

		/// <summary>
		/// Identificador da chave do site
		/// </summary>
        public string SiteKey { get; set; }

		/// <summary>
		/// Nome amigável do site registrado
		/// </summary>
		public string SiteName { get; set; }
	}
}