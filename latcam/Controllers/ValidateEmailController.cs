using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Net.Mail;
using latcam.Models;
using latcam.Results;

namespace latcam.Controllers
{
    public class ValidateEmailController : ApiController
    {

        [HttpPost]
        public IHttpActionResult post([FromUri] string Email)
        {
            try
            {
                AuxUser.getUser(Email);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        public IHttpActionResult put([FromUri] string Email, [FromUri] string Code)
        {
            try
            {
                latincamEntities s = new latincamEntities();
                AspNetUsers user = (from c in s.AspNetUsers where c.Email == Email && c.ActivationCode == Code select c).FirstOrDefault();

                if (user == null)
                {
                    return NotFound();
                }

                user.EmailConfirmed = true;
                s.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
    }
}
