using latcam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace latcam.Controllers.Stats
{
    public class RamndomActiveStreamingController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpGet]
        public IHttpActionResult GET()
        {
            try
            {
                List<TopActiveRandomStreamingView> top = (from c in context.TopActiveRandomStreamingView select c).ToList();
                return Ok(top);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
