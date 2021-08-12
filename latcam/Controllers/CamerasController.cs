using latcam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace latcam.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CamerasController : ApiController
    {
        latincamEntities context = new latincamEntities();

 
        [HttpPost]
        public IHttpActionResult Post([FromBody] CameraModel model)
        {
            try
            {
                StreamingCamera camera = new StreamingCamera();
                camera.Id = Guid.NewGuid().ToString();
                camera.StreamingId = model.StreamingId;
                camera.Title = model.Title;
                camera.Description = model.Description;
                camera.StreamUrl = model.StreamUrl;
                camera.IsActive = model.IsActive;
                camera.AllowPublic = model.IsActive;
                context.StreamingCamera.Add(camera);
                context.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public IHttpActionResult Put([FromBody] CameraModel model)
        {
            try
            {
                StreamingCamera camera = (from c in context.StreamingCamera where c.Id == model.Id select c).FirstOrDefault();
                if(camera == null)
                {
                    return NotFound();
                }

                camera.Title = model.Title;
                camera.Description = model.Description;
                camera.AllowPublic = model.AllowPublic;
                camera.IsActive = model.IsActive;
                camera.StreamUrl = model.StreamUrl;
                context.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete([FromUri] String Id)
        {
            try
            {
                StreamingCamera camera = (from c in context.StreamingCamera where c.Id == Id select c).FirstOrDefault();
                if(camera == null)
                {
                    return NotFound();
                }

                context.StreamingCamera.Remove(camera);
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
