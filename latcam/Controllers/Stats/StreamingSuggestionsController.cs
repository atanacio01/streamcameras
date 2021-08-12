using latcam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace latcam.Controllers.Stats
{
    [Authorize]
    public class StreamingSuggestionsController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpGet]
        public IHttpActionResult Get([FromUri] string Id)
        {
            try
            {
                string UserId = new AccountController().User.Identity.GetUserId();
                StreamingSuggestions suggest = (from c in context.StreamingSuggestions where c.UserId == UserId && c.StreamingId == Id select c).FirstOrDefault();
                if (suggest == null)
                {
                    return NotFound();
                }
                return Ok(suggest.StreamingId);
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
                StreamingSuggestions suggest = (from c in context.StreamingSuggestions where c.UserId == UserId && c.StreamingId == Id select c).FirstOrDefault();
                if (suggest == null)
                {
                    StreamingSuggestions newSuggest = new StreamingSuggestions();
                    newSuggest.UserId = UserId;
                    newSuggest.StreamingId = Id;
                    context.StreamingSuggestions.Add(newSuggest);
                }
                else
                {
                    context.StreamingSuggestions.Remove(suggest);
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
