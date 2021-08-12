using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using latcam.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace latcam.Controllers.Stats
{
    [Authorize]
    public class CameraLikesController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpGet]
        public IHttpActionResult Get([FromUri] String Id)
        {
            try
            {
                string UserId = new AccountController().User.Identity.GetUserId();
                CameraLikes like = (from c in context.CameraLikes where c.UserId == UserId && c.CameraId == Id select c).FirstOrDefault();
                if (like == null)
                {
                    return Ok(false);
                    
                }
                return Ok(true);
            }
            catch(Exception ex)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromUri] string Id)
        {
            try
            {
                string UserId = new AccountController().User.Identity.GetUserId();
                CameraLikes like = (from c in context.CameraLikes where c.UserId == UserId && c.CameraId == Id select c).FirstOrDefault();
                bool res = true;
                if (like == null)
                {
                    CameraLikes newLike = new CameraLikes();
                    newLike.UserId = UserId;
                    newLike.CameraId = Id;
                    context.CameraLikes.Add(newLike);
                    res = true;
                }
                else
                {
                    context.CameraLikes.Remove(like);
                    res = false;
                }
                context.SaveChanges();
                return Ok(res);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
