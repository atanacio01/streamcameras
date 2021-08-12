using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using latcam.Models;
using Microsoft.AspNet.Identity;

namespace latcam.Controllers.SubscriptionsControllers
{
    [AllowAnonymous]
    public class GetCurrentSubscriptionController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                string userid = new AccountController().User.Identity.GetUserId();
                Subscriptions sub = new Subscriptions();
                sub = (from c in context.Subscriptions where c.UserId == userid select c).FirstOrDefault();
                if(sub == null)
                {
                    return NotFound();
                }

                SuscriptionModel model = new SuscriptionModel()
                {
                    SubscriptionDate = sub.SubscriptionDate.ToShortDateString(),
                    SubscriptionEndDate = ((DateTime)sub.SubscriptionEndDate).ToShortDateString(),
                    SubscriptionType = sub.SubscriptionType,
                    isActive = (bool)sub.IsActive
                };
                return Ok(model);  
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
