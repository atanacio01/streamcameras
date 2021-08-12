using conekta;
using latcam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace latcam.Controllers.Conekta
{
    [Authorize]
    public class CreateClientController : ApiController
    {
        latincamEntities context = new latincamEntities();
        [HttpPost]
        public IHttpActionResult Post()
        {
            try
            {
                conekta.Api.apiKey = App.ConektaKey;
                conekta.Api.version = "2.0.0";
                conekta.Api.locale = "es";
                string UserID = new AccountController().User.Identity.GetUserId();
                var cUserExist = (from c in context.ConektaUsers where c.UserId == UserID select c).FirstOrDefault();
                if(cUserExist != null)
                {
                    return BadRequest("El usuario ya está registrado en conekta");
                }

                Profile profile = (from c in context.Profile where c.UserId == UserID select c).FirstOrDefault();
                if(profile == null)
                {
                    return BadRequest("No hay perfil registrado");
                }
                var cUserData = (from c in context.AspNetUsers where c.Id == UserID select c).FirstOrDefault();

                Customer customer = new conekta.Customer().create(@"{
                  ""name"": """ + profile.Name + " " + profile.LastName + @""",
                  ""email"": """ + cUserData.Email + @"""}");


                ConektaUsers cUser = new ConektaUsers();
                cUser.ConektaId = customer.id;
                cUser.UserId = UserID;
                context.ConektaUsers.Add(cUser);
                context.SaveChanges();
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public IHttpActionResult Put()
        {
            try
            {
                conekta.Api.apiKey = App.ConektaKey;
                conekta.Api.version = "2.0.0";
                conekta.Api.locale = "es";
                string UserID = new AccountController().User.Identity.GetUserId();
                Customer customer = new conekta.Customer().find(UserID);

                customer.update(@"{
                  ""name"":  ""Marios Perez""
                  ""email"": ""usuarsio@example.com"",
                }");
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete()
        {
            try
            {
                conekta.Api.apiKey = App.ConektaKey;
                conekta.Api.version = "2.0.0";
                conekta.Api.locale = "es";
                conekta.Api.apiKey = "key_UkFxpU2rTiR17x6c6foppw";
                conekta.Customer customer = new conekta.Customer().find("cus_2isbkbwccQS6WborM");
                customer.destroy();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
