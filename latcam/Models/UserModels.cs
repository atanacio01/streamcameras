using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace latcam.Models
{
    public class UserModels
    {
    }

    public class ProfileModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string ProfileUrl { get; set; }
        public byte[] arrayImg { get; set; }
    }
}