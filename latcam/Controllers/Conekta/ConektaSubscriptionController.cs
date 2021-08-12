using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using conekta;
using latcam.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace latcam.Controllers.Conekta
{
    [Authorize]
    public class ConektaSubscriptionController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpPost]
        public IHttpActionResult Post([FromBody] PaymentDetails details)
        {

            try
            {
                Api.apiKey = App.ConektaKey;
                Api.version = "2.0.0";
                Api.locale = "es";

                string UserID = new AccountController().User.Identity.GetUserId();
                var res = (from c in context.Subscriptions where c.UserId == UserID  select c).FirstOrDefault();

                if (res != null)
                {
                    if (res.SubscriptionType == 1)
                    {
                        return BadRequest("El usuario ya esta suscrito");
                    }
                }

                string conektaId = "";
                string conektaUserId = "";

                var cUserExist = (from c in context.ConektaUsers where c.UserId == UserID select c).FirstOrDefault();
                if (cUserExist == null)
                {



                    Customer customer = new conekta.Customer().create(@"{
                  ""name"": """ + details.Name + @""",
                  ""email"": """ + details.Email + @""",
                    ""payment_sources"": [{
                        ""token_id"": """ + details.CardToken + @""",
                        ""type"": ""card""
                      }],
                    ""shipping_contacts"": [{
                        ""phone"": ""+5215555555555"",
                        ""receiver"": """ + details.Name + @""",
                        ""address"": {
                          ""street1"": ""Mexico"",
                          ""country"": ""MX"",
                          ""postal_code"": ""06100""
                        }
                    }]}");

                    ConektaUsers cUser = new ConektaUsers();
                    cUser.ConektaId = customer.id;
                    cUser.UserId = UserID;
                    context.ConektaUsers.Add(cUser);
                    context.SaveChanges();
                    conektaId = cUser.ConektaId;
                    conektaUserId = UserID;
                }
                else
                {
                    conektaId = cUserExist.ConektaId;
                    conektaUserId = cUserExist.UserId;
                }

                string cardToken = String.Empty;
                var hasPayment = (from c in context.ConektaCards where c.UserID == UserID select c).FirstOrDefault();
                if (hasPayment == null)
                {
                    ConektaCards card = new ConektaCards
                    {
                        CardToken = details.CardToken,
                        Brand = details.Brand,
                        Exp_Month = details.Month,
                        Exp_Year = details.Year,
                        Last4 = details.Last4,
                        UserID = conektaUserId
                    };
                    
                    context.ConektaCards.Add(card);
                    context.SaveChanges();

                    cardToken = details.CardToken;
                }
                else
                {
                    cardToken = hasPayment.CardToken;
                }

                Order order = new conekta.Order().create(@"{
                  ""currency"":""MXN"",
                  ""customer_info"": {
                    ""customer_id"": """ + conektaId + @"""
                  },
                  ""line_items"": [{
                    ""name"": ""Premium"",
                    ""unit_price"": 9900,
                    ""quantity"": 1
                  }],
                  ""shipping_lines"":[{
                      ""amount"": 0,
                      ""tracking_number"": ""PREMIUM"",
                      ""carrier"": ""Online"",
                      ""method"": ""Online""
                    }],
                  ""charges"": [{
                    ""payment_method"": {
                      ""type"": ""card"",
                      ""token_id"": """ + cardToken + @"""
                    }
                  }]
                }");

                if(res == null)
                {
                        Subscriptions newsub = new Subscriptions();
                        newsub.Id = Guid.NewGuid().ToString();
                        newsub.SubscriptionType = 1;
                        newsub.UserId = UserID;
                        newsub.SubscriptionDate = DateTime.Now;
                        newsub.SubscriptionEndDate = DateTime.Now.AddDays(30);
                        newsub.IsActive = true;
                        context.Subscriptions.Add(newsub);
                        context.SaveChanges();
                }
                else
                {
                    if (res.SubscriptionType == 2)
                    {
                        res.SubscriptionType = 1;
                        res.IsActive = true;
                        //res.SubscriptionDate = DateTime.Now;
                        res.SubscriptionEndDate = DateTime.Now.AddDays(30);
                        context.SaveChanges();
                    }
                }
                

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
