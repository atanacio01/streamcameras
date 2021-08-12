using latcam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace latcam.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class StreamingController : ApiController
    {
        public latincamEntities context = new latincamEntities();

        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                List<Streaming> streams = (from c in context.Streaming select c).ToList();
                List<StreamingModel> list = new List<StreamingModel>();
                foreach (var item in streams)
                {
                    StreamingModel model = new StreamingModel();
                    model.Id = item.Id;
                    model.Title = item.Title;
                    model.Description = item.Description;
                    model.IsActive = item.IsActive;
                    model.IsPublic = item.IsPublic;
                    list.Add(model);
                }
                return Ok(list);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] StreamingModel model)
        {
            try
            {
                Streaming stream = new Streaming();
                stream.Id = Guid.NewGuid().ToString();
                stream.Title = model.Title;
                stream.Description = model.Description;
                stream.IsActive = model.IsActive;
                stream.IsPublic = model.IsPublic;

                context.Streaming.Add(stream);
                context.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public IHttpActionResult Put([FromBody] StreamingModel model)
        {
            try
            {
                Streaming streaming = (from c in context.Streaming where c.Id == model.Id select c).FirstOrDefault();
                if (streaming == null)
                {
                    return NotFound();
                }

                streaming.Title = model.Title;
                streaming.Description = model.Description;
                streaming.IsActive = model.IsActive;
                streaming.IsPublic = model.IsPublic;
                context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete([FromUri] String Id)
        {
            try
            {
                Streaming streaming = (from c in context.Streaming where c.Id == Id select c).FirstOrDefault();
                if (streaming == null)
                {
                    return NotFound();
                }

                context.Streaming.Remove(streaming);
                context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
