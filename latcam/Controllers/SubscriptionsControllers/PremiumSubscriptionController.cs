using latcam.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace latcam.Controllers.SubscriptionsControllers
{
    [Authorize]
    public class PremiumSubscriptionController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                string UserID = new AccountController().User.Identity.GetUserId();

                var currentSub = (from c in context.Subscriptions where c.UserId == UserID select c).FirstOrDefault();
                if(currentSub == null)
                {
                    return Ok(false);
                }

                if(currentSub.SubscriptionType == 2)
                {
                    return Ok(false);
                }

                return Ok(true);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
