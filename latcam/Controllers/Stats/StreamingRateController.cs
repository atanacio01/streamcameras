using latcam.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace latcam.Controllers.Stats
{
    public class StreamingRateController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpGet]
        public IHttpActionResult Get([FromUri] string Id)
        {
            try
            {
                string UserId = new AccountController().User.Identity.GetUserId();
                StreamingRatings like = (from c in context.StreamingRatings where c.UserId == UserId && c.StreamingId == Id select c).FirstOrDefault();
                if (like == null)
                {
                    return NotFound();
                }
                return Ok(like.Rate);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPut]
        public IHttpActionResult Put([FromBody] RateModel model)
        {
            try
            {
                string UserId = new AccountController().User.Identity.GetUserId();
                StreamingRatings like = (from c in context.StreamingRatings where c.UserId == UserId && c.StreamingId == model.Id select c).FirstOrDefault();
                StreamingRatings newRate = new StreamingRatings();
                if (like == null)
                {
                    newRate.UserId = UserId;
                    newRate.StreamingId = model.Id;
                    newRate.Rate = model.Rate;
                    context.StreamingRatings.Add(newRate);
                }
                else
                {
                    like.Rate = model.Rate;
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
