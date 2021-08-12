using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using latcam.Models;
using PayPal;
using PayPal.Api;

namespace latcam.Controllers.Paypal
{
    [AllowAnonymous]
    public class PaypalAuthController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post([FromBody]ProfileModel model)
        {
            latincamEntities context = new latincamEntities();

            try
            {
                var config = ConfigManager.Instance.GetProperties();
                var accessToken = new OAuthTokenCredential(config).GetAccessToken();

                var apiContext = new APIContext(accessToken);

                //create plan
                planmodel plan = new planmodel();
                var res = plan.RunSample();

                // Activate the plan
                var patchRequest = new PatchRequest()
                {
                    new Patch()
                    {
                        op = "replace",
                        path = "/",
                        value = new Plan() { state = "ACTIVE" }
                    }
                };

                res.Update(apiContext, patchRequest);

                // With the plan created and activated, we can now create the billing agreement.
                var payer = new Payer() { payment_method = "paypal" };
                var shippingAddress = new ShippingAddress()
                {
                    line1 = "111 First Street",
                    city = "Saratoga",
                    state = "CA",
                    postal_code = "95070",
                    country_code = "US"
                };

                string date = DateTime.UtcNow.AddDays(1).ToString("s", System.Globalization.CultureInfo.InvariantCulture) + "Z";
                var agreement = new Agreement()
                {
                    name = "Premium Plan",
                    description = "LatinosCam Monthly Premium Plan",
                    //start_date = "2018-08-11T00:00:00Z",
                    start_date = date,
                    payer = payer,
                    plan = new Plan() { id = res.id }
                    //,
                    //shipping_address = shippingAddress
                };

                // Create the billing agreement.
                var createdAgreement = agreement.Create(apiContext);
                PaypalPlans pplan = new PaypalPlans();
                pplan.Id = Guid.NewGuid().ToString();
                pplan.Name = createdAgreement.plan.name;
                pplan.Description = createdAgreement.description;
                pplan.PlanId = createdAgreement.plan.id;
                pplan.Created_time = DateTime.Now;
                pplan.Price = decimal.Parse("9.99");

                context.PaypalPlans.Add(pplan);
                context.SaveChanges();
                // Get the redirect URL to allow the user to be redirected to PayPal to accept the agreement.
                var links = createdAgreement.links.GetEnumerator();
                return Ok(createdAgreement);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
