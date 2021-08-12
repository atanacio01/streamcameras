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
    public class StreamingLikeController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpGet]
        public IHttpActionResult Get([FromUri] string Id)
        {
            try
            {
                string UserId = new AccountController().User.Identity.GetUserId();
                StreamingLikes like = (from c in context.StreamingLikes where c.UserId == UserId && c.StreamingId == Id select c).FirstOrDefault();
                if (like == null)
                {
                    return NotFound();
                }
                return Ok(like.StreamingId);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public IHttpActionResult Put([FromUri] string Id)
        {
            try
            {
                string UserId = new AccountController().User.Identity.GetUserId();
                StreamingLikes like = (from c in context.StreamingLikes where c.UserId == UserId && c.StreamingId == Id select c).FirstOrDefault();
                if (like == null)
                {
                    StreamingLikes newLike = new StreamingLikes();
                    newLike.UserId = UserId;
                    newLike.StreamingId = Id;
                    context.StreamingLikes.Add(newLike);
                }
                else
                {
                    context.StreamingLikes.Remove(like);
                }
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
