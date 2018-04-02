using CentralAtendimento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Web.Http.Description;

namespace CentralAtendimento.Controllers
{
	public class LogController : ApiController
	{
		private APIDbContext db;

		public LogController()
		{
			db = new APIDbContext();
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
		}

		[HttpGet]
		[Route("tickets/{siteId}/{clienteId}")]
		public Logs GetAllTickets(string siteId, string clienteId)
		{
			Logs logResponse = new Logs();

			if (db.SiteRegistersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
			{
				var tkts = db.TicketsDb.Include(n => n.MessagesList)
											.Where(t => t.ClienteId == clienteId)
											.Where(t => t.SiteId == siteId).ToList();

				logResponse.TicketSize = tkts.Count;
				logResponse.TicketsList = tkts;

				foreach (var tkt in tkts)
				{
					tkt.MessageSize = tkt.MessagesList.Count;
				}
			}

			return logResponse;
		}

		[HttpGet]
		[Route("tickets/{siteId}/{clienteId}/compra/{compraId}")]
		public Logs GetTicketsByPurchase(string siteId, string clienteId, string compraId)
		{
			Logs logResponse = new Logs();

			if (db.SiteRegistersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
			{
				var tkts = db.TicketsDb.Include(n => n.MessagesList)
											.Where(t => t.ClienteId == clienteId)
											.Where(t => t.CompraId == compraId)
											.Where(t => t.SiteId == siteId).ToList();

				logResponse.TicketSize = tkts.Count;
				logResponse.TicketsList = tkts;

				foreach (var tkt in tkts)
				{
					tkt.MessageSize = tkt.MessagesList.Count;
				}
			}

			return logResponse;
		}

		[HttpGet]
		[Route("tickets/{siteId}/{clienteId}/ticket/{ticketId}")]
		public Logs GetTicketsByTicket(string siteId, string clienteId, string ticketId)
		{
			Logs logResponse = new Logs();

			if (db.SiteRegistersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
			{
				var tkts = db.TicketsDb.Include(n => n.MessagesList)
										   .Where(t => t.ClienteId == clienteId)
										   .Where(t => t.TicketId == ticketId)
										   .Where(t => t.SiteId == siteId).ToList();

				logResponse.TicketSize = tkts.Count;
				logResponse.TicketsList = tkts;

				foreach (var tkt in tkts)
				{
					tkt.MessageSize = tkt.MessagesList.Count;
				}
			}

			return logResponse;
		}

		[HttpPost]
		[Route("tickets/{siteId}/{clienteId}")]
		[ResponseType(typeof(SystemMessages))]
		public HttpResponseMessage CreateGenericTicket(HttpRequestMessage request, string siteId, string clienteId, Messages msg)
		{
			SystemMessages sysMsg = new SystemMessages();

			if (db.SiteRegistersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
			{
				if (ModelState.IsValid)
				{
					Tickets tktToDb = new Tickets();
					Messages msgToDb = new Messages();

					tktToDb.TicketId = DateTime.Now.Year.ToString("yyyy")
										+ DateTime.Now.Month.ToString("MM")
										+ DateTime.Now.Day.ToString("dd")
										+ DateTime.Now.Hour.ToString("HH")
										+ DateTime.Now.Minute.ToString("mm")
										+ DateTime.Now.Second.ToString("ss");

					tktToDb.ClienteId = clienteId;
					tktToDb.SiteId = siteId;
					tktToDb.StatusId = Status.Open;
					tktToDb.MessagesList = new List<Messages>();
					tktToDb.MessagesList.Add(msg);

					db.MessagesDb.Add(msg);
					db.TicketsDb.Add(tktToDb);

					db.SaveChanges();

					sysMsg.SystemMessage = "Ticket created Successfully!";
					return request.CreateResponse(HttpStatusCode.Created, sysMsg);
				}

				sysMsg.SystemMessage = "Request malformed!";
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
			else
			{
				sysMsg.SystemMessage = "Site key provided not recognized!";
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
		}

		[HttpPost]
		[Route("tickets/{siteId}/{clienteId}/compra/{compraID}")]
		[ResponseType(typeof(SystemMessages))]
		public HttpResponseMessage CreateTicketForPurchase(HttpRequestMessage request, string siteId, string clienteId, string compraID, Messages msg)
		{
			SystemMessages sysMsg = new SystemMessages();

			if (db.SiteRegistersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
			{
				if (ModelState.IsValid)
				{
					Tickets tktToDb = new Tickets();
					Messages msgToDb = new Messages();

					tktToDb.TicketId = DateTime.Now.Year.ToString("yyyy")
										+ DateTime.Now.Month.ToString("MM")
										+ DateTime.Now.Day.ToString("dd")
										+ DateTime.Now.Hour.ToString("HH")
										+ DateTime.Now.Minute.ToString("mm")
										+ DateTime.Now.Second.ToString("ss");

					tktToDb.ClienteId = clienteId;
					tktToDb.CompraId = compraID;
					tktToDb.SiteId = siteId;
					tktToDb.StatusId = Status.Open;
					tktToDb.MessagesList = new List<Messages>();
					tktToDb.MessagesList.Add(msg);

					db.MessagesDb.Add(msg);
					db.TicketsDb.Add(tktToDb);

					db.SaveChanges();

					sysMsg.SystemMessage = "Ticket created Successfully!";
					return request.CreateResponse(HttpStatusCode.Created, sysMsg);
				}

				sysMsg.SystemMessage = "Request malformed!";
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
			else
			{
				sysMsg.SystemMessage = "Site key provided not recognized!";
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
		}

		[HttpPut]
		[Route("tickets/{siteId}/{clienteId}/ticket/{ticketId}")]
		[ResponseType(typeof(SystemMessages))]
		public HttpResponseMessage AddMsgToTicket(HttpRequestMessage request, string siteId, string clienteId, string ticketId, Messages msg)
		{
			SystemMessages sysMsg = new SystemMessages();

			if (db.SiteRegistersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
			{
				var tktInDb = db.TicketsDb.Include(m => m.MessagesList)
											.Where(t => t.TicketId == ticketId)
											.Where(t => t.SiteId == siteId)
											.FirstOrDefault(a => a.ClienteId == clienteId);

				if (tktInDb == null)
				{
					sysMsg.SystemMessage = "ClienteId or TicketId not found!";
					return request.CreateResponse(HttpStatusCode.NotFound, sysMsg);
				}

				if (tktInDb.StatusId == Status.Closed || tktInDb.StatusId == Status.Canceled)
				{
					sysMsg.SystemMessage = "Cannot change a resolved ticket!";
					return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
				}

				if (ModelState.IsValid)
				{
					tktInDb.MessagesList.Add(msg);

					db.MessagesDb.Add(msg);
					db.SaveChanges();

					sysMsg.SystemMessage = "Message added successfully!";

					return request.CreateResponse(HttpStatusCode.OK, sysMsg);
				}

				sysMsg.SystemMessage = "Request is malformed!";

				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
			else
			{
				sysMsg.SystemMessage = "Site key provided not recognized!";
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
		}

		[HttpDelete]
		[Route("tickets/{siteId}/{clienteId}/ticket/{ticketId}")]
		[ResponseType(typeof(SystemMessages))]
		public HttpResponseMessage ResolveTicket(HttpRequestMessage request, string siteId, string clienteId, string ticketId, Messages msg, Status code)
		{
			SystemMessages sysMsg = new SystemMessages();

			if (db.SiteRegistersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
			{
				if (Enum.IsDefined(typeof(Status), code))
				{
					sysMsg.SystemMessage = "Invalid code!";
					return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
				}

				var tktInDb = db.TicketsDb.Include(m => m.MessagesList)
											.Where(t => t.TicketId == ticketId)
											.Where(t => t.SiteId == siteId)
											.FirstOrDefault(a => a.ClienteId == clienteId);

				if (tktInDb == null)
				{
					sysMsg.SystemMessage = "ClienteId or TicketId not found!";
					return request.CreateResponse(HttpStatusCode.NotFound, sysMsg);
				}

				if (tktInDb.StatusId == Status.Closed || tktInDb.StatusId == Status.Canceled)
				{
					sysMsg.SystemMessage = "Cannot change a resolved ticket!";
					return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
				}

				if (ModelState.IsValid)
				{
					tktInDb.MessagesList.Add(msg);
					tktInDb.StatusId = code;

					db.MessagesDb.Add(msg);
					db.SaveChanges();

					sysMsg.SystemMessage = "Message added successfully!";

					return request.CreateResponse(HttpStatusCode.OK, sysMsg);
				}

				sysMsg.SystemMessage = "Request is malformed!";
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
			else
			{
				sysMsg.SystemMessage = "Site key provided not recognized!";
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
		}
	}
}
