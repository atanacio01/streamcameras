using latcam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace latcam.Controllers.Stats
{
    [Authorize]
    public class MostLikeStreamingController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpGet]
        public IHttpActionResult GET()
        {
            try
            {
                List<TopMostLikeStreamingView> top = (from c in context.TopMostLikeStreamingView select c).ToList();
                return Ok(top);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
