using latcam.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace latcam.Controllers.SubscriptionsControllers
{
    [AllowAnonymous]
    public class JointoStreamController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpGet]
        public IHttpActionResult get([FromUri]string url)
        {
            try
            {
                string UserID = new AccountController().User.Identity.GetUserId();
                var prof = (from c in context.CurrentUsers where c.StreamingId == url select c).ToList();

                List<CurrentUserModel> model = new List<CurrentUserModel>();
                foreach (var item in prof)
                {
                    CurrentUserModel newModel = new CurrentUserModel();
                    newModel.UserId = (from c in context.Profile where c.UserId == item.UserId select c).FirstOrDefault().Name;
                    newModel.StreamId = item.StreamingId;
                    model.Add(newModel);
                }
                
                return Ok(model.Take(10));
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }


    }
}
