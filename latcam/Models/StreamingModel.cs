using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace latcam.Models
{
    public class StreamingModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }
    }

    public class CameraModel
    {
        public string Id { get; set; }
        public string StreamingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StreamUrl { get; set; }
        public string ScreenShoot { get; set; }
        public bool AllowPublic { get; set; }
        public string Color { get; set; }
        public bool IsActive { get; set; }
    }

    public class RateModel
    {
        public string Id { get; set; }
        public int Rate { get; set; }
    }

    public class SuscriptionModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int SubscriptionType { get; set; }
        public string SubscriptionDate { get; set; }
        public string SubscriptionEndDate { get; set; }
        public bool isActive { get; set; }
    }

    public class CurrentUserModel
    {
        public string UserId { get; set; }
        public string StreamId { get; set; }
    }
}