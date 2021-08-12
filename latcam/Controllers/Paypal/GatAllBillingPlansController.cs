using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace latcam.Controllers.Paypal
{
    public class GatAllBillingPlansController : ApiController
    {
        private string page;
        private string totalRequired;

        public IHttpActionResult GET()
        {
            var apiContext = PayPal.Sample.Configuration.GetAPIContext();

            var planList = Plan.List(apiContext,"2",null,null,"yes");

            

            return Ok(planList);
        }
    }
}
