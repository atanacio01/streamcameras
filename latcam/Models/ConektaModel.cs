using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace latcam.Models
{
    public class ConektaModel
    {

    }

    public class PaymentDetails
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CardToken { get; set; }
        public string Last4 { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Brand { get; set; }
    }
}