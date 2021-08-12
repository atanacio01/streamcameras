using PayPal.Api;
using PayPal.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace latcam.Controllers.Paypal
{
    public class planmodel
    {
        private static Currency GetCurrency(string value)
        {
            return new Currency() { value = value, currency = "USD" };
        }

        public static Plan CreatePlanObject(HttpContext httpContext)
        {
            // ### Create the Billing Plan
            // Both the trial and standard plans will use the same shipping
            // charge for this example, so for simplicity we'll create a
            // single object to use with both payment definitions.
            var shippingChargeModel = new ChargeModel() 
            {
                type = "SHIPPING",
                amount = GetCurrency("9.99")
            };

            // Define the plan and attach the payment definitions and merchant preferences.
            // More Information: https://developer.paypal.com/webapps/developer/docs/api/#create-a-plan
            return new Plan
            {
                name = "Premium Plan",
                description = "LatinosCam Monthly Premium Plan",
                type = "INFINITE",
                // Define the merchant preferences.
                // More Information: https://developer.paypal.com/webapps/developer/docs/api/#merchantpreferences-object
                merchant_preferences = new MerchantPreferences()
                {
                    setup_fee = GetCurrency("9.99"),
                    //return_url = httpContext.Request.Url.ToString(),
                    return_url = "http://localhost/latweb/#!/membership",
                    cancel_url = httpContext.Request.Url.ToString() + "?cancel",
                    auto_bill_amount = "YES",
                    initial_fail_amount_action = "CONTINUE",
                    max_fail_attempts = "0"
                },
                payment_definitions = new List<PaymentDefinition>
                {
                    // Define a trial plan that will only charge $9.99 for the first
                    // month. After that, the standard plan will take over for the
                    // remaining 11 months of the year.
                    //new PaymentDefinition()
                    //{
                    //    name = "Trial Plan",
                    //    type = "TRIAL",
                    //    frequency = "DAY",
                    //    frequency_interval = "1",
                    //    amount = GetCurrency("9.99"),
                    //    cycles = "1"
                    //    //,
                    //    //charge_models = new List<ChargeModel>
                    //    //{
                    //    //    new ChargeModel()
                    //    //    {
                    //    //        type = "TAX",
                    //    //        amount = GetCurrency("1.65")
                    //    //    },
                    //    //    shippingChargeModel
                    //    //}
                    //},
                    // Define the standard payment plan. It will represent a monthly
                    // plan for $19.99 USD that charges once month for 11 months.
                    new PaymentDefinition
                    {
                        name = "Premmium Plan",
                        type = "REGULAR",
                        frequency = "DAY",
                        frequency_interval = "1",
                        amount = GetCurrency("9.99"),
                        // > NOTE: For `IFNINITE` type plans, `cycles` should be 0 for a `REGULAR` `PaymentDefinition` object.
                        cycles = "0"
                        //,
                        //charge_models = new List<ChargeModel>
                        //{
                        //    new ChargeModel
                        //    {
                        //        type = "TAX",
                        //        amount = GetCurrency("2.47")
                        //    },
                        //    shippingChargeModel
                        //}
                    }
                }
            };
        }

        public Plan  RunSample()
        {
            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            // See [Configuration.cs](/Source/Configuration.html) to know more about APIContext.
            var apiContext = Configuration.GetAPIContext();

            var plan = CreatePlanObject(HttpContext.Current);


            // Call `plan.Create()` to create the billing plan resource.
            var createdPlan = plan.Create(apiContext);

            return createdPlan;
            // For more information, please visit [PayPal Developer REST API Reference](https://developer.paypal.com/docs/api/).
        }
    }
}