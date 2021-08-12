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
    public class DefaultController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpGet]
        public IHttpActionResult get([FromUri]string url)
        {
            try
            {
                string UserID = new AccountController().User.Identity.GetUserId();
                return Ok(UserID);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
