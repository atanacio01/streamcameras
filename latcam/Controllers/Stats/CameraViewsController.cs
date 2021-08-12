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
    public class CameraViewsController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpGet]
        public IHttpActionResult GET([FromUri]String Id)
        {
            try
            {
                string UserId = new AccountController().User.Identity.GetUserId();
                StreamingCamera camera = (from c in context.StreamingCamera where c.Id == Id select c).FirstOrDefault();
                if (camera == null)
                {
                    return NotFound();
                }
                return Ok(camera.Views);
            }
            catch(Exception ex)
            {
                return InternalServerError();
            }

        }

        [HttpPost]
        public IHttpActionResult POST([FromUri] String Id)
        {
            try
            {
                string UserId = new AccountController().User.Identity.GetUserId();
                StreamingCamera camera = (from c in context.StreamingCamera where c.Id == Id select c).FirstOrDefault();
                if (camera == null)
                {
                    return NotFound();
                }
                camera.Views = camera.Views == null ? camera.Views = 1: camera.Views + 1;
                context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
