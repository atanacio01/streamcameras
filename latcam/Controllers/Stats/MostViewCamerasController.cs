using latcam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace latcam.Controllers.Stats
{
    [AllowAnonymous]
    public class MostViewCamerasController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpGet]
        public IHttpActionResult GET()
        {
            try
            {
                List<TopViewsCamerasView> top = (from c in context.TopViewsCamerasView select c).ToList();
                return Ok(top);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
