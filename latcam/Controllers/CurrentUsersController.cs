using latcam.Models;
using Microsoft.AspNet.Identity;
using PusherServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace latcam.Controllers
{
    
    public class CurrentUsersController : ApiController
    {
        latincamEntities context = new latincamEntities();
        [HttpPost]
        public async Task<IHttpActionResult> POST()
        {

            string UserID = new AccountController().User.Identity.GetUserId();
            Profile prof = (from c in context.Profile where c.UserId == UserID select c).FirstOrDefault();
            if (prof == null)
            {
                return NotFound();
            }

            var options = new PusherOptions
            {
                Cluster = "us2",
                Encrypted = true
            };

            var pusher = new Pusher(
              "576360",
              "afaf917859fe6f85436c",
              "ef43d49c4e68fcc45e48",
              options);

            var result = await pusher.TriggerAsync(
              "my-channel",
              "my-event",
              new { message = prof.Name });

            return Ok();
        }
    }
}
