using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralAtendimento.Models
{
	public class InternalLog
	{
		public int Id { get; set; }
		public DateTime Timestamp { get; set; }
		public string HttpMethod { get; set; }
		public string siteId { get; set; }
		public string Message { get; set; }
	}
}