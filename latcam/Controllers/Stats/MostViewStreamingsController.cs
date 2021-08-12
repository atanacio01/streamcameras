using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using latcam.Models;

namespace latcam.Controllers.Stats
{
    [AllowAnonymous]
    public class MostViewStreamingsController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpGet]
        public IHttpActionResult GET()
        {
            try
            {
                List<TopViewsStreamingView> top = (from c in context.TopViewsStreamingView select c).ToList();
                return Ok(top);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
