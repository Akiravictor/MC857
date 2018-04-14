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
		/// Método para obter todos os Tickets de um site/módulo criados por um determinado Cliente.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="siteId">ID único para cada site/módulo (verificar contrato)</param>
		/// <param name="clienteId">ID do cliente</param>
		/// <returns>Retorna um objeto JSON contendo uma lista de Tickets ou uma mensagem de erro do sistema. HTTP Response codes:<br/>
		/// HTTP 200 (OK) caso o Ticket seja encontrado;<br/>
		/// HTTP 400 (Bad Request) caso haja erro;<br/>
		/// HTTP 404 (Not Found) caso não encontre nenhum Ticket.<br/>
		/// </returns>
		[HttpGet]
		[Route("tickets/{siteId}/{clienteId}")]
		[ResponseType(typeof(Logs))]
		[ResponseType(typeof(SystemMessages))]
		public HttpResponseMessage GetAllTickets(HttpRequestMessage request, string siteId, string clienteId)
		{
			SystemMessages sysMsg = new SystemMessages();
			Logs logResponse = new Logs();
			LogActions la = new LogActions();

			var tkts = db.TicketsDb.Include(n => n.MessagesList)
											.Where(t => t.ClienteId == clienteId)
											.Where(t => t.SiteId == siteId).ToList();

			if (db.SiteRegistersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
			{
				if(tkts.Count == 0)
				{
					sysMsg.SystemMessage = "ClienteId not found!";
					la.SaveLog("GET", siteId, string.Format("ClienteId: {0} System Message: {1}", clienteId, sysMsg.SystemMessage));
					return request.CreateResponse(HttpStatusCode.NotFound, sysMsg);
				}
				else
				{
					logResponse.TicketSize = tkts.Count;
					logResponse.TicketsList = tkts;

					foreach (var tkt in tkts)
					{
						tkt.MessageSize = tkt.MessagesList.Count;
					}

					la.SaveLog("GET", siteId, string.Format("ClienteId: {0} System Message: Get all Tickets from ClienteId", clienteId));
					return request.CreateResponse(HttpStatusCode.OK, logResponse);
				}

			}
			else
			{
				sysMsg.SystemMessage = "Site key provided not recognized!";
				la.SaveLog("GET", siteId, sysMsg.SystemMessage);
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
		}

		/// <summary>
		/// Método para obter o Ticket de uma Compra criado por um determinado Cliente.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="siteId">ID único para cada site/módulo (verificar contrato)</param>
		/// <param name="clienteId">ID do cliente</param>
		/// <param name="compraId">ID da compra</param>
		/// <returns>Retorna um objeto JSON contendo uma lista de Tickets ou uma Mensagem de Erro do Sistema. HTTP Response codes:<br/>
		/// HTTP 200 (OK) caso o Ticket seja encontrado;<br/>
		/// HTTP 400 (Bad Request) caso haja erro;<br/>
		/// HTTP 404 (Not Found) caso não encontre nenhum Ticket.<br/>
		/// </returns>
		[HttpGet]
		[Route("tickets/{siteId}/{clienteId}/compra/{compraId}")]
		[ResponseType(typeof(Logs))]
		[ResponseType(typeof(SystemMessages))]
		public HttpResponseMessage GetTicketsByPurchase(HttpRequestMessage request, string siteId, string clienteId, string compraId)
		{
			SystemMessages sysMsg = new SystemMessages();
			Logs logResponse = new Logs();
			LogActions la = new LogActions();

			var tkts = db.TicketsDb.Include(n => n.MessagesList)
											.Where(t => t.ClienteId == clienteId)
											.Where(t => t.CompraId == compraId)
											.Where(t => t.SiteId == siteId).ToList();

			if (db.SiteRegistersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
			{
				if (tkts.Count == 0)
				{
					sysMsg.SystemMessage = "ClienteId or CompraId not found!";
					la.SaveLog("GET", siteId, string.Format("ClienteId: {0} CompraId: {1} System Message: {2}", clienteId, compraId, sysMsg.SystemMessage));
					return request.CreateResponse(HttpStatusCode.NotFound, sysMsg);
				}
				else
				{
					logResponse.TicketSize = tkts.Count;
					logResponse.TicketsList = tkts;

					foreach (var tkt in tkts)
					{
						tkt.MessageSize = tkt.MessagesList.Count;
					}

					la.SaveLog("GET", siteId, string.Format("ClienteId: {0} CompraId: {1} System Message: Get ticket by CompraId", clienteId, compraId));
					return request.CreateResponse(HttpStatusCode.OK, logResponse);
				}
			}
			else
			{
				sysMsg.SystemMessage = "Site key provided not recognized!";
				la.SaveLog("GET", siteId, sysMsg.SystemMessage);
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
		}

		/// <summary>
		/// Método para obter o Ticket específico gerado por um Cliente.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="siteId">ID único para cada site/módulo (verificar contrato)</param>
		/// <param name="clienteId">ID do cliente</param>
		/// <param name="ticketId">ID do ticket</param>
		/// <returns>Retorna um objeto JSON contendo uma lista de Tickets ou uma Mensagem de Erro do Sistema. HTTP Response codes:<br/>
		/// HTTP 200 (OK) caso o Ticket seja encontrado;<br/>
		/// HTTP 400 (Bad Request) caso haja erro;<br/>
		/// HTTP 404 (Not Found) caso não encontre nenhum Ticket.<br/>
		/// </returns>
		[HttpGet]
		[Route("tickets/{siteId}/{clienteId}/ticket/{ticketId}")]
		[ResponseType(typeof(Logs))]
		[ResponseType(typeof(SystemMessages))]
		public HttpResponseMessage GetTicketsByTicket(HttpRequestMessage request, string siteId, string clienteId, string ticketId)
		{
			SystemMessages sysMsg = new SystemMessages();
			Logs logResponse = new Logs();
			LogActions la = new LogActions();

			var tkts = db.TicketsDb.Include(n => n.MessagesList)
										   .Where(t => t.ClienteId == clienteId)
										   .Where(t => t.TicketId == ticketId)
										   .Where(t => t.SiteId == siteId).ToList();

			if (db.SiteRegistersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
			{
				if (tkts.Count == 0)
				{
					sysMsg.SystemMessage = "ClienteId or TicketId not found!";
					la.SaveLog("GET", siteId, string.Format("TicketId: {0} ClienteId: {1} System Message: {1}", ticketId, clienteId, sysMsg.SystemMessage));
					return request.CreateResponse(HttpStatusCode.NotFound, sysMsg);
				}
				else
				{

					logResponse.TicketSize = tkts.Count;
					logResponse.TicketsList = tkts;

					foreach (var tkt in tkts)
					{
						tkt.MessageSize = tkt.MessagesList.Count;
					}

					la.SaveLog("GET", siteId, string.Format("TicketId: {0} ClienteId: {1} System Message: Get ticket by Id", ticketId, clienteId));
					return request.CreateResponse(HttpStatusCode.OK, logResponse);
				}
			}
			else
			{
				sysMsg.SystemMessage = "Site key provided not recognized!";
				la.SaveLog("GET", siteId, sysMsg.SystemMessage);
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
		}

		/// <summary>
		/// Método para gerar um Ticket para um determinado Cliente não atrelado à nenhuma compra.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="siteId">ID único para cada site/módulo (verificar contrato)</param>
		/// <param name="clienteId">ID do cliente</param>
		/// <param name="msg">Mensagem passada atráves do Body do Request</param>
		/// <returns>Retorna um objeto JSON contendo uma Mensagem do Sistema. Podendo ser uma mensagem de erro ou o número do Ticket gerado. HTTP Response codes:<br/>
		/// HTTP 201 (Created) caso o Ticket seja criado;<br/>
		/// HTTP 400 (Bad Request) caso haja um erro.<br/>
		/// </returns>
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

					la.SaveLog("POST", siteId, string.Format("TicketId: {0} ClienteId: {1} System Message: Ticket Created", tktToDb.TicketId, clienteId));

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
		/// Método para gerar um Ticket para um determinado Cliente atrelado à um Compra.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="siteId">ID único para cada site/módulo (verificar contrato)</param>
		/// <param name="clienteId">ID do cliente</param>
		/// <param name="compraID">ID da compra</param>
		/// <param name="msg">Mensagem passada atráves do Body do Request</param>
		/// <returns>Retorna um objeto JSON contendo uma Mensagem do Sistema. Podendo ser uma mensagem de erro ou o número do Ticket gerado.HTTP Response codes:<br/>
		/// HTTP 201 (Created) caso o Ticket seja criado;<br/>
		/// HTTP 400 (Bad Request) caso haja um erro.<br/>
		/// </returns>
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

					la.SaveLog("POST", siteId, string.Format("TicketId: {0} ClienteId: {1} CompraId: {2} System Message: Ticket Created", tktToDb.TicketId, clienteId, compraID));

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
		/// Método para adicionar uma mensagem à um determinado Ticket de um Cliente.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="siteId">ID único para cada site/módulo (verificar contrato)</param>
		/// <param name="clienteId">ID do cliente</param>
		/// <param name="ticketId">ID do ticket</param>
		/// <param name="msg">Mensagem passada atráves do Body do Request</param>
		/// <returns>Retorna um objeto JSON contendo uma Mensagem do Sistema. Podendo ser uma mensagem de erro ou uma confirmação de mensagem adicionada. HTTP Response codes:<br/>
		/// HTTP 200 (OK) caso a mensagem seja adicionada corretamente;<br/>
		/// HTTP 400 (Bad Request) caso haja um erro;<br/>
		/// HTTP 404 (Not Found) caso o Ticket não seja encontrado.<br/>
		/// </returns>
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
					la.SaveLog("PUT", siteId, string.Format("TicketId: {0} ClientId: {1} System Message: {2}", ticketId, clienteId, sysMsg.SystemMessage));
					return request.CreateResponse(HttpStatusCode.NotFound, sysMsg);
				}

				else if (tktInDb.StatusId == Status.Closed || tktInDb.StatusId == Status.Canceled)
				{
					sysMsg.SystemMessage = "Cannot change a resolved ticket!";
					la.SaveLog("PUT", siteId, string.Format("TicketId: {0} ClienteId: {1} Code: {2} System Message: {3}", ticketId, clienteId, tktInDb.StatusId, sysMsg.SystemMessage));
					return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
				}

				else if (ModelState.IsValid)
				{
					tktInDb.MessagesList.Add(msg);

					db.MessagesDb.Add(msg);
					db.SaveChanges();

					la.SaveLog("PUT", siteId, string.Format("TicketId: {0} ClienteId: {1} System Message: Added Message", ticketId, clienteId));

					sysMsg.SystemMessage = string.Format("Message added successfully to Ticket {0}!", tktInDb.TicketId);
					return request.CreateResponse(HttpStatusCode.OK, sysMsg);
				}

				else
				{
					sysMsg.SystemMessage = "Request is malformed!";
					la.SaveLog("PUT", siteId, sysMsg.SystemMessage);
					return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
				}
			}
			else
			{
				sysMsg.SystemMessage = "Site key provided not recognized!";
				la.SaveLog("POST", siteId, sysMsg.SystemMessage);
				return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
			}
		}

		/// <summary>
		/// Método para alterar o Estado de um determinado Ticket de um Cliente.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="siteId">ID único para da site (verificar contrato)</param>
		/// <param name="clienteId">ID do cliente</param>
		/// <param name="ticketId">ID do Ticket</param>
		/// <param name="msg">Mensagem passada atráves do Body do Request</param>
		/// <param name="code">Código de estado do Ticket</param>
		/// <returns>Retorna um objeto JSON contendo uma Mensagem do Sistema. Podendo ser uma mensagem de erro ou uma confirmação de alteração de Status do Ticket.HTTP Response codes:<br/>
		/// HTTP 200 (OK) caso a mensagem seja adicionada corretamente e o status alterado;<br/>
		/// HTTP 400 (Bad Request) caso haja um erro;<br/>
		/// HTTP 404 (Not Found) caso o Ticket não seja encontrado.<br/>
		/// </returns>
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
					la.SaveLog("DELETE", siteId, string.Format("TicketId: {0} ClienteId: {1} Code: {2} System Message: {3}", ticketId, clienteId, code, sysMsg.SystemMessage));
					return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
				}

				var tktInDb = db.TicketsDb.Include(m => m.MessagesList)
											.Where(t => t.TicketId == ticketId)
											.Where(t => t.SiteId == siteId)
											.FirstOrDefault(a => a.ClienteId == clienteId);

				if (tktInDb == null)
				{
					sysMsg.SystemMessage = "ClienteId or TicketId not found!";
					la.SaveLog("DELETE", siteId, string.Format("TicketId: {0} ClienteId: {1} Code: {2} System Message: {3}", ticketId, clienteId, code, sysMsg.SystemMessage));
					return request.CreateResponse(HttpStatusCode.NotFound, sysMsg);
				}

				if (tktInDb.StatusId == Status.Closed || tktInDb.StatusId == Status.Canceled)
				{
					sysMsg.SystemMessage = "Cannot change a resolved ticket!";
					la.SaveLog("DELETE", siteId, string.Format("TicketId: {0} ClienteId: {1} CodeInDb: {2} CodeFromRequest: {3} System Message: {4}", ticketId, clienteId, tktInDb.StatusId, code, sysMsg.SystemMessage));
					return request.CreateResponse(HttpStatusCode.BadRequest, sysMsg);
				}

				if (ModelState.IsValid)
				{
					tktInDb.MessagesList.Add(msg);
					tktInDb.StatusId = code;

					db.MessagesDb.Add(msg);
					db.SaveChanges();

					la.SaveLog("DELETE", siteId, string.Format("TicketId: {0} ClienteId: {1} System Message: Status changed from {2} to {3}", tktInDb.TicketId, clienteId, tktInDb.StatusId, code));

					sysMsg.SystemMessage = string.Format("Message added successfully to Ticket {0} and Status changed from {1} to {2}", tktInDb.TicketId, tktInDb.StatusId, code);
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

		/// <summary>
		/// Metodo interno usado para DEBUG
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("tickets/all/")]
		public Logs AllTicketsDEBUG()
		{
			Logs logResponse = new Logs();

		
			var tkts = db.TicketsDb.Include(n => n.MessagesList).ToList();

			logResponse.TicketSize = tkts.Count;
			logResponse.TicketsList = tkts;

			foreach (var tkt in tkts)
			{
				tkt.MessageSize = tkt.MessagesList.Count;
			}
			return logResponse;
		}

	}
}
