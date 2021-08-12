using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace latcam.Controllers.Paypal
{
    public class GetPlanController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GET()
        {
            try
            {
                var planId = "P-78J580318U499250REELGLVA";
                var plan = Plan.Get(PayPal.Sample.Configuration.GetAPIContext(), planId);

                var payer = new Payer() { payment_method = "paypal" };
                var shippingAddress = new ShippingAddress()
                {
                    line1 = "111 First Street",
                    city = "Saratoga",
                    state = "CA",
                    postal_code = "95070",
                    country_code = "US"
                };

                var agreement = new Agreement()
                {
                    name = "Premium subscription",
                    description = "Total Acces to all cams",
                    start_date = "2018-08-11T00:00:00Z",
                    payer = payer,
                    plan = new Plan() { id = planId },
                };

                // Create the billing agreement.
                var createdAgreement = agreement.Create(PayPal.Sample.Configuration.GetAPIContext());

                return Ok(createdAgreement);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
