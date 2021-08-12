using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using latcam.Models;
using latcam.Results;
using Microsoft.AspNet.Identity;


namespace latcam.Controllers
{
    [AllowAnonymous]
    
    public class UploadImageController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                    
                    var postedFile = httpRequest.Files[file];
                    var postedFileStream = httpRequest.Files[file].InputStream;
                    

                    string UserID = new AccountController().User.Identity.GetUserId();
                    latincamEntities context = new latincamEntities();
                    Profile prof = (from c in context.Profile where c.UserId == UserID select c).FirstOrDefault();
                    if (prof == null)
                    {
                        return BadRequest("Update your profile first");
                    }
                    await AzureStorage.DeleteFileAsync(ContainerType.Image, prof.ProfileUrl);
                    string imageToLoad = await AzureStorage.UploadFileAsync(ContainerType.Image, postedFileStream);
                    prof.ProfileUrl = imageToLoad;
                    context.SaveChanges();
                    var message1 = string.Format("Image Updated Successfully.");
                    return Ok(message1);
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return BadRequest(dict.ToString());
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", ex);
                return InternalServerError(ex);
            }
        }
    }
}
