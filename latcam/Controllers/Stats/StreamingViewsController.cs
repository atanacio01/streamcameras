using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using latcam.Models;
using Microsoft.AspNet.Identity;

namespace latcam.Controllers.Stats
{
    [Authorize]
    public class StreamingViewsController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpPut]
        public IHttpActionResult Put([FromUri] String Id)
        {
            try
            {
                string UserId = new AccountController().User.Identity.GetUserId();
                Streaming stream = (from c in context.Streaming where c.Id == Id select c).FirstOrDefault();
                if(stream == null)
                {
                    return NotFound();
                }
                stream.Views = stream.Views == null ? stream.Views = 1 : stream.Views + 1;
                context.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
