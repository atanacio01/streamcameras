using latcam.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace latcam.Controllers
{
    
    public class GetStreamingCamerasController : ApiController
    {
        latincamEntities context = new latincamEntities();


        [HttpGet]
        public IHttpActionResult Get([FromUri]string Id)
        {
            try
            {
                string UserId = new AccountController().User.Identity.GetUserId();
                List<CameraModel> listCam = new List<CameraModel>();
                var list = (from c in context.StreamingCamera where c.StreamingId == Id select c).ToList();

                foreach (var item in list)
                {
                    CameraModel cam = new CameraModel();
                    cam.AllowPublic = item.AllowPublic;
                    cam.Description = item.Description;
                    cam.Id = item.Id;
                    cam.IsActive = item.IsActive;
                    cam.StreamingId = item.StreamingId;
                    cam.StreamUrl = item.StreamUrl;
                    cam.Title = item.Title;
                    if (item.AllowPublic)
                    {
                        cam.Color = "Blue";
                    }
                    else
                    {
                        cam.Color = "Red";
                    }
                    if(UserId != null)
                    {
                        cam.Color = "#d5868d";
                    }           
                    cam.ScreenShoot = item.Description;
                    listCam.Add(cam);
                }
                return Ok(listCam);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
