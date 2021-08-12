using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using latcam.Models;
using Microsoft.AspNet.Identity;
using PayPal.Api;
using PayPal.Sample;

namespace latcam.Controllers.Paypal
{
    [AllowAnonymous]
    public class executePaymentController : ApiController
    {
        latincamEntities context = new latincamEntities();
        [HttpGet]
        public IHttpActionResult Post([FromUri]string token)
        {
            try
            {
                string UserID = new AccountController().User.Identity.GetUserId();
                var res = (from c in context.Subscriptions where c.UserId == UserID select c).FirstOrDefault();

                if (res != null)
                {
                    if (res.SubscriptionType == 1)
                    {
                        return BadRequest("El usuario ya esta suscrito");
                    }
                }

                var apiContext = PayPal.Sample.Configuration.GetAPIContext();
                var agreement = new Agreement() { token = token };
                
                var executedAgreement = agreement.Execute(apiContext);

                if (res == null)
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

                return Ok(executedAgreement);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
    public class paypalmodel
    {
        public string paymentID { get; set; }
        public string payerID { get; set; }
    }
}
