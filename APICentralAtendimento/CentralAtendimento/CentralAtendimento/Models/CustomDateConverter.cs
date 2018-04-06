using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralAtendimento.Models
{
	public class CustomDateConverter : IsoDateTimeConverter
	{
		public CustomDateConverter()
		{
			base.DateTimeFormat = "yyyy-MM-ddTHH:mm";
		}
	}
}