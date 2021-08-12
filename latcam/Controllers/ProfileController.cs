using latcam.Models;
using latcam.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace latcam.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProfileController : ApiController
    {
        latincamEntities context = new latincamEntities();


        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                string UserID = new AccountController().User.Identity.GetUserId();
                Profile prof = (from c in context.Profile where c.UserId == UserID select c).FirstOrDefault();
                if (prof == null)
                {
                    return NotFound();
                }

                ProfileModel model = new ProfileModel();
                model.Id = prof.Id;
                model.UserId = prof.UserId;
                model.Name = prof.Name;
                model.LastName = prof.LastName;
                model.Age = (int)prof.Age;
                //model.ProfileUrl = prof.ProfileUrl;
                if (prof.ProfileUrl != null)
                {
                    var img = await AzureStorage.GetFileAsync(ContainerType.Image, prof.ProfileUrl);
                    model.arrayImg = img;
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        public IHttpActionResult Post([FromBody]Profile profile)
        {
            try
            {
                string UserID = new AccountController().User.Identity.GetUserId();

                if (UserID == null)
                {
                    return NotFound();
                }

                var ProfileExists = (from c in context.Profile where c.UserId == UserID select c).FirstOrDefault();
                if (ProfileExists != null)
                {
                    return BadRequest("El perfil del usuario ya ha sido creado");
                }

                Profile prof = new Profile();
                prof.Id = Guid.NewGuid().ToString();
                prof.UserId = UserID;
                prof.Name = profile.Name;
                prof.LastName = profile.LastName;
                prof.Age = profile.Age;
                context.Profile.Add(prof);
                context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPut]
        public IHttpActionResult Put([FromBody]Profile profile)
        {
            try
            {
                string UserID = new AccountController().User.Identity.GetUserId();
                Profile prof = (from c in context.Profile where c.UserId == UserID select c).FirstOrDefault();
                if (prof == null)
                {
                    return NotFound();
                }

                prof.Name = profile.Name;
                prof.LastName = profile.LastName;
                prof.Age = profile.Age;
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
