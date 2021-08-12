using latcam.Models;
using latcam.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace latcam.Controllers
{
    [AllowAnonymous ]
    public class ForgotPasswordController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post([FromUri] string Email)
        {
            try
            {
                latincamEntities s = new latincamEntities();
                AspNetUsers user = (from c in new latincamEntities().AspNetUsers where c.Email == Email select c).FirstOrDefault();

                if (user == null)
                {
                    return NotFound();
                }


                //Set SmtpClient to send Email
                string stFromUserName = "noreply@mail.com";
                string stFromPassword = "CfimGdf6Kn3?";
                int inPort = Convert.ToInt32(587);
                string stHost = "mail.mail.com";
                bool btIsSSL = true;

                MailAddress to = new MailAddress(Email);
                MailAddress from = new MailAddress("noreply@mail.com");

                MailMessage objEmail = new MailMessage(from, to);
                objEmail.Subject = "Recuperación de contraseña de Latino Cams";
                objEmail.Body = "Para cambiar tu contraseña <a href='http://testing.mail.com/#!/restore?code=" + UsersManager.Encrypt(Email) + "'>haz click aqui</> ";
                objEmail.IsBodyHtml = true;
                objEmail.Priority = MailPriority.High;

                SmtpClient client = new SmtpClient();
                System.Net.NetworkCredential auth = new System.Net.NetworkCredential(stFromUserName, stFromPassword);
                client.Host = stHost;
                client.Port = inPort;
                client.UseDefaultCredentials = false;
                client.Credentials = auth;
                client.EnableSsl = btIsSSL;
                client.Send(objEmail);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPut]
        public async Task<IHttpActionResult> Put([FromBody] RestorePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string email = UsersManager.Decrypt(model.Email);
            AspNetUsers user = (from c in new latincamEntities().AspNetUsers where c.Email == email select c).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            string token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, token, model.Password);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No hay disponibles errores ModelState para enviar, por lo que simplemente devuelva un BadRequest vacío.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }

    public class RestorePasswordModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
