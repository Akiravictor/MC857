using CentralAtendimento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralAtendimento.Methods
{
	public class LogActions
	{
		private APIDbContext db;

		public LogActions()
		{
			db = new APIDbContext();
		}

		public void SaveLog(string HttpMethod, string siteId, string Message)
		{
			var internalLog = new InternalLog();

			internalLog.Timestamp = DateTime.Now;
			internalLog.HttpMethod = HttpMethod;
			internalLog.siteId = siteId;
			internalLog.Message = Message;

			db.InternalLogDb.Add(internalLog);
			db.SaveChanges();
		}
	}
}