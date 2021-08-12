using latcam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace latcam.Controllers.Stats
{
    [Authorize]
    public class CameraRateController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpGet]
        public IHttpActionResult Get([FromUri] string Id)
        {
            try
            {
                string UserId = new AccountController().User.Identity.GetUserId();
                CameraRatings rate = (from c in context.CameraRatings where c.UserId == UserId && c.CameraId == Id select c).FirstOrDefault();
                if (rate == null)
                {
                    return NotFound();
                }
                return Ok(rate.Rate);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        public IHttpActionResult POST([FromBody] RateModel model)
        {
            try
            {
                string UserId = new AccountController().User.Identity.GetUserId();
                CameraRatings rate = (from c in context.CameraRatings where c.UserId == UserId && c.CameraId == model.Id select c).FirstOrDefault();
                CameraRatings newRate = new CameraRatings();
                if (rate == null)
                {
                    newRate.Id = Guid.NewGuid().ToString();
                    newRate.UserId = UserId;
                    newRate.CameraId = model.Id;
                    newRate.Rate = model.Rate;
                    context.CameraRatings.Add(newRate);
                }
                else
                {
                    rate.Rate = model.Rate;
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
