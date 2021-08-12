using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using latcam.Models;
namespace latcam.Controllers.Posts
{
    [Authorize]
    public class FavoritesController : ApiController
    {
        latincamEntities context = new latincamEntities();

        [HttpPost]
        public IHttpActionResult Post([FromUri]string Id)
        {
            try
            {
                var UserId = new AccountController().User.Identity.GetUserId();
                Favorites fav = new Favorites();
                fav.UserId = UserId;
                fav.StreamingId = Id;
                context.Favorites.Add(fav);
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
