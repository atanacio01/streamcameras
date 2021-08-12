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
    public class CamerasSuggestionsController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpGet]
        public IHttpActionResult Get([FromUri] string Id)
        {
            try
            {
                string UserId = new AccountController().User.Identity.GetUserId();
                CamerasSuggestions rate = (from c in context.CamerasSuggestions where c.UserId == UserId && c.CameraId == Id select c).FirstOrDefault();
                if (rate == null)
                {
                    return NotFound();
                }
                return Ok(rate.CameraId);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPut]
        public IHttpActionResult Put([FromUri] string Id)
        {
            try
            {
                string UserId = new AccountController().User.Identity.GetUserId();
                CamerasSuggestions suggest = (from c in context.CamerasSuggestions where c.UserId == UserId && c.CameraId == Id select c).FirstOrDefault();
                if (suggest == null)
                {
                    CamerasSuggestions newSuggest = new CamerasSuggestions();
                    newSuggest.UserId = UserId;
                    newSuggest.CameraId = Id;
                    context.CamerasSuggestions.Add(newSuggest);
                }
                else
                {
                    context.CamerasSuggestions.Remove(suggest);
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
