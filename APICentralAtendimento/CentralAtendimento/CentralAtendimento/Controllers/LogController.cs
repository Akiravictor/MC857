using CentralAtendimento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Web.Http.Description;
using CentralAtendimento.Methods;

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

		/// <summary>
		/// Retorna todos os Tickets de um usuário
		/// </summary>
		/// <param name="request"></param>
		/// <param name="siteId">ID único para cada site/módulo (verificar contrato)</param>
		/// <param name="clienteId">ID do cliente</param>
		/// <returns></returns>
		[HttpGet]
		[Route("tickets/{siteId}/{clienteId}")]
		[ResponseType(typeof(Logs))]
		[ResponseType(typeof(SystemMessages))]
		public HttpResponseMessage GetAllTickets(HttpRequestMessage request, string siteId, string clienteId)
		{
			SystemMessages sysMsg = new SystemMessages();
			Logs logResponse = new Logs();
			LogActions la = new LogActions();

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

				la.SaveLog("GET", siteId, string.Format("Get all Tickets for {0}", clienteId));

			}
			else
			{
				la.SaveLog("GET", siteId, string.Format("Tried to get all Tickets for {0}", clienteId));
				sysMsg.SystemMessage = "Site key provided not recognized!";
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}

			return request.CreateResponse(HttpStatusCode.OK, logResponse);
		}

		/// <summary>
		/// Retorna todas as mensagens de um determinado Ticket de acordo com o ID da compra
		/// </summary>
		/// <param name="request"></param>
		/// <param name="siteId">ID único para cada site/módulo (verificar contrato)</param>
		/// <param name="clienteId">ID do cliente</param>
		/// <param name="compraId">ID da compra</param>
		/// <returns></returns>
		[HttpGet]
		[Route("tickets/{siteId}/{clienteId}/compra/{compraId}")]
		[ResponseType(typeof(Logs))]
		[ResponseType(typeof(SystemMessages))]
		public HttpResponseMessage GetTicketsByPurchase(HttpRequestMessage request, string siteId, string clienteId, string compraId)
		{
			SystemMessages sysMsg = new SystemMessages();
			Logs logResponse = new Logs();
			LogActions la = new LogActions();

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

				la.SaveLog("GET", siteId, string.Format("Get Tickets for {0} by Purchase: {1}", clienteId, compraId));

			}
			else
			{
				la.SaveLog("GET", siteId, string.Format("Tried to get Tickets for {0} by Purchase: {1}", clienteId, compraId));
				sysMsg.SystemMessage = "Site key provided not recognized!";
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}

			return request.CreateResponse(HttpStatusCode.OK, logResponse);
		}

		/// <summary>
		/// Retorna todas as mensagens de um determinado Ticket de acordo com o ID do ticket
		/// </summary>
		/// <param name="request"></param>
		/// <param name="siteId">ID único para cada site/módulo (verificar contrato)</param>
		/// <param name="clienteId">ID do cliente</param>
		/// <param name="ticketId">ID do ticket</param>
		/// <returns></returns>
		[HttpGet]
		[Route("tickets/{siteId}/{clienteId}/ticket/{ticketId}")]
		[ResponseType(typeof(Logs))]
		[ResponseType(typeof(SystemMessages))]
		public HttpResponseMessage GetTicketsByTicket(HttpRequestMessage request, string siteId, string clienteId, string ticketId)
		{
			SystemMessages sysMsg = new SystemMessages();
			Logs logResponse = new Logs();
			LogActions la = new LogActions();

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

				la.SaveLog("GET", siteId, string.Format("Get Tickets for {0} by Ticket: {1}", clienteId, ticketId));
			}
			else
			{
				la.SaveLog("GET", siteId, string.Format("Tried to get Tickets for {0} by Ticket: {1}", clienteId, ticketId));
				sysMsg.SystemMessage = "Site key provided not recognized!";
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}

			return request.CreateResponse(HttpStatusCode.OK, logResponse);
		}

		/// <summary>
		/// Cria um ticket genérico para o cliente
		/// </summary>
		/// <param name="request"></param>
		/// <param name="siteId">ID único para cada site/módulo (verificar contrato)</param>
		/// <param name="clienteId">ID do cliente</param>
		/// <param name="msg">Mensagem passada atráves do Body do Request</param>
		/// <returns></returns>
		[HttpPost]
		[Route("tickets/{siteId}/{clienteId}")]
		[ResponseType(typeof(SystemMessages))]
		public HttpResponseMessage CreateGenericTicket(HttpRequestMessage request, string siteId, string clienteId, Messages msg)
		{
			SystemMessages sysMsg = new SystemMessages();
			LogActions la = new LogActions();

			if (msg == null)
			{
				sysMsg.SystemMessage = "Message cannot be empty!";
				la.SaveLog("POST", siteId, sysMsg.SystemMessage);

				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
			else if (db.SiteRegistersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
			{
				if (ModelState.IsValid)
				{
					Tickets tktToDb = new Tickets();
					Messages msgToDb = new Messages();

					tktToDb.TicketId = DateTime.Now.ToString("yyyyMMddHHmmss");

					tktToDb.ClienteId = clienteId;
					tktToDb.SiteId = siteId;
					tktToDb.StatusId = Status.Open;
					tktToDb.MessagesList = new List<Messages>();
					tktToDb.MessagesList.Add(msg);

					db.MessagesDb.Add(msg);
					db.TicketsDb.Add(tktToDb);

					db.SaveChanges();

					la.SaveLog("POST", siteId, string.Format("Generated a ticket for {0}, TicketId: {1}", clienteId, tktToDb.TicketId));

					sysMsg.SystemMessage = tktToDb.TicketId;
					return request.CreateResponse(HttpStatusCode.Created, sysMsg);
				}

				sysMsg.SystemMessage = "Request malformed!";
				la.SaveLog("POST", siteId, sysMsg.SystemMessage);
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
			else
			{
				sysMsg.SystemMessage = "Site key provided not recognized!";
				la.SaveLog("POST", siteId, sysMsg.SystemMessage);
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
		}

		/// <summary>
		/// Cria um Ticket específico para uma compra
		/// </summary>
		/// <param name="request"></param>
		/// <param name="siteId">ID único para cada site/módulo (verificar contrato)</param>
		/// <param name="clienteId">ID do cliente</param>
		/// <param name="compraID">ID da compra</param>
		/// <param name="msg">Mensagem passada atráves do Body do Request</param>
		/// <returns></returns>
		[HttpPost]
		[Route("tickets/{siteId}/{clienteId}/compra/{compraID}")]
		[ResponseType(typeof(SystemMessages))]
		public HttpResponseMessage CreateTicketForPurchase(HttpRequestMessage request, string siteId, string clienteId, string compraID, Messages msg)
		{
			SystemMessages sysMsg = new SystemMessages();
			LogActions la = new LogActions();

			if (msg == null)
			{
				sysMsg.SystemMessage = "Message cannot be empty!";
				la.SaveLog("POST", siteId, sysMsg.SystemMessage);

				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
			else if (db.SiteRegistersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
			{
				if (ModelState.IsValid)
				{
					Tickets tktToDb = new Tickets();
					Messages msgToDb = new Messages();

					tktToDb.TicketId = DateTime.Now.ToString("yyyyMMddHHmmss");

					tktToDb.ClienteId = clienteId;
					tktToDb.CompraId = compraID;
					tktToDb.SiteId = siteId;
					tktToDb.StatusId = Status.Open;
					tktToDb.MessagesList = new List<Messages>();
					tktToDb.MessagesList.Add(msg);

					db.MessagesDb.Add(msg);
					db.TicketsDb.Add(tktToDb);

					db.SaveChanges();

					la.SaveLog("POST", siteId, string.Format("Generated a ticket for {0}, TicketId: {1}, CompraId: {2}", clienteId, tktToDb.TicketId, compraID));

					sysMsg.SystemMessage = tktToDb.TicketId;
					return request.CreateResponse(HttpStatusCode.Created, sysMsg);
				}

				sysMsg.SystemMessage = "Request malformed!";
				la.SaveLog("POST", siteId, sysMsg.SystemMessage);
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
			else
			{
				sysMsg.SystemMessage = "Site key provided not recognized!";
				la.SaveLog("POST", siteId, sysMsg.SystemMessage);
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
		}

		/// <summary>
		/// Adiciona uma mensagem em um Ticket
		/// </summary>
		/// <param name="request"></param>
		/// <param name="siteId">ID único para cada site/módulo (verificar contrato)</param>
		/// <param name="clienteId">ID do cliente</param>
		/// <param name="ticketId">ID do ticket</param>
		/// <param name="msg">Mensagem passada atráves do Body do Request</param>
		/// <returns></returns>
		[HttpPut]
		[Route("tickets/{siteId}/{clienteId}/ticket/{ticketId}")]
		[ResponseType(typeof(SystemMessages))]
		public HttpResponseMessage AddMsgToTicket(HttpRequestMessage request, string siteId, string clienteId, string ticketId, Messages msg)
		{
			SystemMessages sysMsg = new SystemMessages();
			LogActions la = new LogActions();

			if (msg == null)
			{
				sysMsg.SystemMessage = "Message cannot be empty!";
				la.SaveLog("PUT", siteId, sysMsg.SystemMessage);
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
			else if (db.SiteRegistersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
			{
				var tktInDb = db.TicketsDb.Include(m => m.MessagesList)
											.Where(t => t.TicketId == ticketId)
											.Where(t => t.SiteId == siteId)
											.FirstOrDefault(a => a.ClienteId == clienteId);

				if (tktInDb == null)
				{
					sysMsg.SystemMessage = "ClienteId or TicketId not found!";
					la.SaveLog("PUT", siteId, sysMsg.SystemMessage);
					return request.CreateResponse(HttpStatusCode.NotFound, sysMsg);
				}

				if (tktInDb.StatusId == Status.Closed || tktInDb.StatusId == Status.Canceled)
				{
					sysMsg.SystemMessage = "Cannot change a resolved ticket!";
					la.SaveLog("PUT", siteId, sysMsg.SystemMessage);
					return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
				}

				if (ModelState.IsValid)
				{
					tktInDb.MessagesList.Add(msg);

					db.MessagesDb.Add(msg);
					db.SaveChanges();

					la.SaveLog("POST", siteId, string.Format("Added a Message to ticket {0}, TicketId: {1}", clienteId, tktInDb.TicketId));

					sysMsg.SystemMessage = string.Format("Message added successfully to Ticket {0}!", tktInDb.TicketId);
					return request.CreateResponse(HttpStatusCode.OK, sysMsg);
				}

				sysMsg.SystemMessage = "Request is malformed!";
				la.SaveLog("PUT", siteId, sysMsg.SystemMessage);
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
			else
			{
				sysMsg.SystemMessage = "Site key provided not recognized!";
				la.SaveLog("POST", siteId, sysMsg.SystemMessage);
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
		}

		/// <summary>
		/// Encerra um Ticket
		/// </summary>
		/// <param name="request"></param>
		/// <param name="siteId">ID único para da site (verificar contrato)</param>
		/// <param name="clienteId">ID do cliente</param>
		/// <param name="ticketId">ID do Ticket</param>
		/// <param name="msg">Mensagem passada atráves do Body do Request</param>
		/// <param name="code">Código de estado do Ticket</param>
		/// <returns></returns>
		[HttpDelete]
		[Route("tickets/{siteId}/{clienteId}/ticket/{ticketId}")]
		[ResponseType(typeof(SystemMessages))]
		public HttpResponseMessage ResolveTicket(HttpRequestMessage request, string siteId, string clienteId, string ticketId, Messages msg, Status code)
		{
			SystemMessages sysMsg = new SystemMessages();
			LogActions la = new LogActions();

			if(msg == null)
			{
				sysMsg.SystemMessage = "Message cannot be empty!";
				la.SaveLog("DELETE", siteId, sysMsg.SystemMessage);
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
			else if (db.SiteRegistersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
			{
				if (!Enum.IsDefined(typeof(Status), code))
				{
					sysMsg.SystemMessage = "Invalid code!";
					la.SaveLog("DELETE", siteId, sysMsg.SystemMessage);
					return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
				}

				var tktInDb = db.TicketsDb.Include(m => m.MessagesList)
											.Where(t => t.TicketId == ticketId)
											.Where(t => t.SiteId == siteId)
											.FirstOrDefault(a => a.ClienteId == clienteId);

				if (tktInDb == null)
				{
					sysMsg.SystemMessage = "ClienteId or TicketId not found!";
					la.SaveLog("DELETE", siteId, sysMsg.SystemMessage);
					return request.CreateResponse(HttpStatusCode.NotFound, sysMsg);
				}

				if (tktInDb.StatusId == Status.Closed || tktInDb.StatusId == Status.Canceled)
				{
					sysMsg.SystemMessage = "Cannot change a resolved ticket!";
					la.SaveLog("DELETE", siteId, sysMsg.SystemMessage);
					return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
				}

				if (ModelState.IsValid)
				{
					tktInDb.MessagesList.Add(msg);
					tktInDb.StatusId = code;

					db.MessagesDb.Add(msg);
					db.SaveChanges();

					la.SaveLog("DELETE", siteId, string.Format("Ticket Status Code changed from {0} to {1}", tktInDb.StatusId, code));

					sysMsg.SystemMessage = string.Format("Message added successfully to Ticket {0}!", tktInDb.TicketId);
					return request.CreateResponse(HttpStatusCode.OK, sysMsg);
				}

				sysMsg.SystemMessage = "Request is malformed!";
				la.SaveLog("DELETE", siteId, sysMsg.SystemMessage);
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
			else
			{
				sysMsg.SystemMessage = "Site key provided not recognized!";
				la.SaveLog("DELETE", siteId, sysMsg.SystemMessage);
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
		}

#if DEBUG
		/// <summary>
		/// Metodo interno usado para DEBUG
		/// </summary>
		/// <param name="senha"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("tickets/all/{senha}")]
		public Logs AllTicketsDEBUG(string senha)
		{
			Logs logResponse = new Logs();

			if (senha.Equals("CentralClientesMC857"))
			{
				var tkts = db.TicketsDb.Include(n => n.MessagesList).ToList();

				logResponse.TicketSize = tkts.Count;
				logResponse.TicketsList = tkts;

				foreach (var tkt in tkts)
				{
					tkt.MessageSize = tkt.MessagesList.Count;
				}
			}
			return logResponse;
		}
#endif

	}
}
