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
    public class UpdateStreamJoinController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpPost]
        public IHttpActionResult Post([FromUri]string Id)
        {
            try
            {
                string UserID = new AccountController().User.Identity.GetUserId();
                CurrentUsers prof = (from c in context.CurrentUsers where c.UserId == UserID select c).FirstOrDefault();

                if (prof != null)
                {
                    prof.StreamingId = Id;
                }
                else
                {
                    CurrentUsers nuevo = new CurrentUsers();
                    nuevo.UserId = UserID;
                    nuevo.StreamingId = Id;
                    context.CurrentUsers.Add(nuevo);
                }

                context.SaveChanges();

                return Ok(UserID);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
