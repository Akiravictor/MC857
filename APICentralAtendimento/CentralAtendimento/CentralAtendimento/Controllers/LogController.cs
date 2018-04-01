using CentralAtendimento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

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

            if (db.SiteResitersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
            {
                var tkts = db.TicketsDb.Include(n => n.MessagesList).Where(n => n.ClienteId == clienteId).ToList();

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

            if (db.SiteResitersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
            {
                var tkts = db.TicketsDb.Include(n => n.MessagesList).Where(n => n.ClienteId == clienteId).Where(a => a.CompraId == compraId).ToList();

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

            if (db.SiteResitersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
            {
                var tkts = db.TicketsDb.Include(n => n.MessagesList).Where(n => n.ClienteId == clienteId).Where(a => a.TicketId == ticketId).ToList();

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
        public HttpStatusCode CreateGenericTicket(string siteId, string clienteId, Messages msg)
        {
            if (db.SiteResitersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
            {
                if (ModelState.IsValid)
                {
                    Tickets tktToDb = new Tickets();
                    Messages msgToDb = new Messages();

                    tktToDb.TicketId = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                    tktToDb.ClienteId = clienteId;
                    tktToDb.SiteId = siteId;
                    tktToDb.StatusId = Status.Open;
                    tktToDb.MessagesList = new List<Messages>();
                    tktToDb.MessagesList.Add(msg);

                    db.TicketsDb.Add(tktToDb);
                    db.MessagesDb.Add(msg);

                    db.SaveChanges();

                    return HttpStatusCode.Created;
                }

                return HttpStatusCode.BadRequest;
            }
            else
            {
                return HttpStatusCode.BadRequest;
            }
        }

        [HttpPost]
        [Route("tickets/{siteId}/{clienteId}/compra/{compraID}")]
        public HttpStatusCode CreateTicketForPurchase(string siteId, string clienteId, string compraID, Messages msg)
        {
            if (db.SiteResitersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
            {
                if (ModelState.IsValid)
                {
                    Tickets tktToDb = new Tickets();
                    Messages msgToDb = new Messages();

                    tktToDb.TicketId = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                    tktToDb.ClienteId = clienteId;
                    tktToDb.CompraId = compraID;
                    tktToDb.SiteId = siteId;
                    tktToDb.StatusId = Status.Open;
                    tktToDb.MessagesList = new List<Messages>();
                    tktToDb.MessagesList.Add(msg);

                    db.TicketsDb.Add(tktToDb);
                    db.MessagesDb.Add(msg);

                    db.SaveChanges();

                    return HttpStatusCode.Created;
                }

                return HttpStatusCode.BadRequest;
            }
            else
            {
                return HttpStatusCode.BadRequest;
            }
        }

        [HttpPut]
        [Route("tickets/{siteId}/{clienteId}/id/{ticketID}")]
        public HttpStatusCode AddMsgToTicket(string siteId, string clienteId, string ticketId, Messages msg)
        {
            if (db.SiteResitersDb.FirstOrDefault(s => s.SiteKey == siteId) != null)
            {
                if (ModelState.IsValid)
                {
                    var tktToDb = db.TicketsDb.Include(n => n.MessagesList).Where(n => n.ClienteId == clienteId).Where(a => a.TicketId == ticketId);

                    tktToDb.MessagesList.Add(msg);
                    db.MessagesDb.Add(msg);

                    db.SaveChanges();

                    return HttpStatusCode.Accepted;
                }

                return HttpStatusCode.BadRequest;
            }
            else
            {
                return HttpStatusCode.BadRequest;
            }
        }



        /*

            {
	            sys_message: <string>
            }
            (DELETE)~/tickets/<CPF>/id/<ticket_ID> -> precisaria de uma mensagem ou item no link para saber se é CANCEL ou CLOSE
         */

    }
}
