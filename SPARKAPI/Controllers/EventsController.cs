using SPARKAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SPARKAPI.Controllers
{
    [RoutePrefix("api/Events")]
    public class EventsController : ApiController
    {

        private SPARKEntities Context = new SPARKEntities();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }






        // GET: api/Events/EventsCategories
        [HttpGet]

        [AllowAnonymous]
        [Route("EventsCategories")]
        public async Task<IHttpActionResult> EventsCategories()
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    IEnumerable<AspNetEventsCat> EventsCategories = Context.AspNetEventsCats;

                    EventsCategories.ToList().ForEach(GetPublisherName);

                    return Ok(EventsCategories);
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        private void GetPublisherName(AspNetEventsCat obj)
        {


            ApplicationUser UserInfo = UserManager.FindById(obj.Cat_Publisher);


            if (UserInfo != null)
            {



                int InstructorExistedFlag = Context.AspNetInstructors.Where(m => m.Usr_Id == UserInfo.Id).Count();
                int ManagerExistedFlag = Context.AspNetManagers.Where(m => m.Usr_Id == UserInfo.Id).Count();

                if (InstructorExistedFlag > 0)
                {
                    AspNetInstructor InstructorExisted = Context.AspNetInstructors.Where(m => m.Usr_Id == UserInfo.Id).SingleOrDefault();


                    string FirstName = InstructorExisted.Inst_Fname;
                    string LastName = InstructorExisted.Inst_Lname;
                    string FullName = FirstName + " " + LastName;
                    obj.Cat_Publisher = FullName;
                }
                else if (ManagerExistedFlag > 0)
                {
                    AspNetManager ManagerExisted = Context.AspNetManagers.Where(m => m.Usr_Id == UserInfo.Id).SingleOrDefault();


                    string FirstName = ManagerExisted.Manager_Fname;
                    string LastName = ManagerExisted.Manager_Lname;
                    string FullName = FirstName + " " + LastName;
                    obj.Cat_Publisher = FullName;
                }


            }



        }


        // GET: api/Events/EventsCategories
        [HttpGet]
        [AllowAnonymous]
        [Route("ActiveEventsCategories")]
        public async Task<IHttpActionResult> ActiveEventsCategories()
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    IEnumerable<AspNetEventsCat> ActiveEventsCategories = Context.AspNetEventsCats.Where(m => m.Cat_Active == true).ToList();

                    return Ok(ActiveEventsCategories);
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("NewEventsCategory")]
        public async Task<IHttpActionResult> NewEventsCategory(EventsCategoriesViewModel model)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    AspNetEventsCat Category = new AspNetEventsCat();

                    Category.Cat_Name = model.Cat_Name;
                    Category.Cat_Id = Guid.NewGuid().ToString();
                    Category.Cat_DateTime = DateTime.Now;
                    Category.Cat_Active = false;
                    Category.Cat_Publisher= Request.GetOwinContext().Request.User.Identity.GetUserId();



                    Context.AspNetEventsCats.Add(Category);

                    int Records = await Context.SaveChangesAsync();

                    if (Records > 0)
                    {

                        return Ok(Category);

                    }
                    else
                    {
                        return BadRequest();

                    }




                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [AllowAnonymous]
        [Route("EditEventsCategory")]
        public async Task<IHttpActionResult> EditEventsCategory(EventsCategoriesViewModel model)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(model.Cat_Id))
                    {
                        AspNetEventsCat Category = Context.AspNetEventsCats.Find(model.Cat_Id);

                        Category.Cat_Name = model.Cat_Name;

                        int Records = await Context.SaveChangesAsync();

                        if (Records > 0)
                        {

                            return Ok(Category);

                        }
                        else
                        {
                            return BadRequest();

                        }

                    }
                    else
                    {
                        return NotFound();
                    }


                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("EditEventsCategory")]
        public async Task<IHttpActionResult> EditEventsCategory(string Cat_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(Cat_Id))
                    {
                        AspNetEventsCat Category = Context.AspNetEventsCats.Find(Cat_Id);

                        if (Category != null)
                        {

                            return Ok(Category);

                        }
                        else
                        {
                            return NotFound();

                        }

                    }
                    else
                    {
                        return NotFound();
                    }


                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        // Delete: api/Events/DeleteEventsCategories

        [HttpDelete]
        [AllowAnonymous]
        [Route("DeleteEventsCategory")]
        public async Task<IHttpActionResult> DeleteEventsCategory(string Cat_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(Cat_Id))
                    {
                        AspNetEventsCat Category = Context.AspNetEventsCats.Find(Cat_Id);

                        if (Category != null)
                        {

                            Context.AspNetEventsCats.Remove(Category);

                            int record = await Context.SaveChangesAsync();

                            if (record > 0)
                            {

                                return Ok(Category);

                            }
                            else
                            {
                                return NotFound();
                            }




                        }
                        else
                        {
                            return NotFound();

                        }

                    }
                    else
                    {
                        return NotFound();
                    }


                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        // POST : api/Events/ActivateEventCategory
        [HttpPost]
        [AllowAnonymous]
        [Route("ActivateEventsCategory")]
        public async Task<IHttpActionResult> ActivateEventsCategory(string Cat_Id)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!string.IsNullOrEmpty(Cat_Id))
                    {

                        AspNetEventsCat Category = Context.AspNetEventsCats.Find(Cat_Id);

                        if (Category.Cat_Active == true)
                        {

                            Category.Cat_Active = false;
                        }
                        else if (Category.Cat_Active == false)
                        {

                            Category.Cat_Active = true;
                        }

                        int Records = await Context.SaveChangesAsync();

                        if (Records > 0)
                        {
                            return Ok(Category);

                        }
                        else
                        {
                            return BadRequest(ModelState);
                        }

                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }




                }
                else
                {
                    return Unauthorized();
                }


            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException.ToString());
            }


        }





        // POST: api/Events/ActivateEvents
        [HttpPost]
        [AllowAnonymous]
        [Route("ActivateEvents")]
        public async Task<IHttpActionResult> ActivateEvents(string Event_Id)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!string.IsNullOrEmpty(Event_Id))
                    {

                        AspNetEvent Events = Context.AspNetEvents.Find(Event_Id);

                        if (Events.Event_Active == true)
                        {

                            Events.Event_Active = false;
                        }
                        else if (Events.Event_Active == false)
                        {

                            Events.Event_Active = true;
                        }

                        int Records = await Context.SaveChangesAsync();

                        if (Records > 0)
                        {
                            return Ok(Events);

                        }
                        else
                        {
                            return BadRequest(ModelState);
                        }

                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }




                }
                else
                {
                    return Unauthorized();
                }


            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException.ToString());
            }


        }


        // DELETE: api/Events/DeleteEvents
        [HttpDelete]
        [AllowAnonymous]
        [Route("DeleteEvents")]
        public async Task<IHttpActionResult> DeleteEvents(string Event_Id)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!string.IsNullOrEmpty(Event_Id))
                    {

                        AspNetEvent Events = Context.AspNetEvents.Find(Event_Id);

                        Context.AspNetEvents.Remove(Events);


                        int Records = await Context.SaveChangesAsync();

                        if (Records > 0)
                        {
                            return Ok(Events);

                        }
                        else
                        {
                            return BadRequest(ModelState);
                        }

                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }




                }
                else
                {
                    return Unauthorized();
                }


            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException.ToString());
            }


        }

        [HttpPut]
        [AllowAnonymous]
        [Route("EditEvents")]
        public async Task<IHttpActionResult> EditEvents(EventsViewModel model)
        {
            try
            {
                HttpPostedFile Event_Photo = HttpContext.Current.Request.Files[0];

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(model.Event_Id))
                    {


                        if (Event_Photo != null && Event_Photo.ContentLength > 0)
                        {
                            var supportedTypes = new[] { ".jpg", ".jpeg", ".png" };

                            var fileextention = Path.GetExtension(Event_Photo.FileName);

                            //.........check file Extention

                            if (!supportedTypes.Contains(fileextention))
                            {

                                string ErrorMessage = "File Extension Is InValid - Only Upload (.jpg , .jpeg, .png)";


                                return BadRequest(ErrorMessage);


                            }
                            else
                            {

                                string fileName = Guid.NewGuid() + Path.GetExtension(Event_Photo.FileName);
                                string pathoriginal = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Eventsoriginal"), fileName).ToString();

                                string pathresized = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Eventsresized"), fileName).ToString();
                                Event_Photo.SaveAs(pathoriginal);
                                ResizeImage(350, pathoriginal, pathresized);

                                AspNetEvent Events = Context.AspNetEvents.Find(model.Event_Id);

                                Events.Event_Photo = model.Event_Photo;

                                Events.Event_Title = model.Event_Title;

                                Events.Event_DateTime = System.DateTime.Now;
                             
                                Events.Cat_Id = model.Cat_Id;



                                int Records = await Context.SaveChangesAsync();

                                if (Records > 0)
                                {
                                    return Ok(Events);

                                }
                                else
                                {
                                    return BadRequest(ModelState);
                                }


                            }
                        }

                        else
                        {


                            string fileName = Guid.NewGuid() + Path.GetExtension(Event_Photo.FileName);
                            string pathoriginal = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Eventssoriginal"), fileName).ToString();

                            string pathresized = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Eventssresized"), fileName).ToString();
                            Event_Photo.SaveAs(pathoriginal);
                            ResizeImage(350, pathoriginal, pathresized);

                            AspNetEvent Events = Context.AspNetEvents.Find(model.Event_Id);

                            Events.Event_Photo = model.Event_Photo;

                            Events.Event_Title = model.Event_Title;

                            Events.Event_DateTime = System.DateTime.Now;
                    
                            Events.Cat_Id = model.Cat_Id;



                            int Records = await Context.SaveChangesAsync();

                            if (Records > 0)
                            {
                                return Ok(Events);

                            }
                            else
                            {
                                return BadRequest(ModelState);
                            }


                        }


                    }

                    else
                    {
                        return NotFound();
                    }


                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("EditEvents")]
        public async Task<IHttpActionResult> EditEvents(string Event_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(Event_Id))
                    {
                        AspNetEvent Events = Context.AspNetEvents.Find(Event_Id);

                        if (Events != null)
                        {

                            return Ok(Events);

                        }
                        else
                        {
                            return NotFound();

                        }

                    }
                    else
                    {
                        return NotFound();
                    }


                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // POST: api/Events/NewEvents
        [HttpPost]
        [AllowAnonymous]
        [Route("NewEvents")]
        public async Task<IHttpActionResult> NewEvents(EventsViewModel model)
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }


                    AspNetEvent Events = new AspNetEvent();

                    Events.Event_Id = System.Guid.NewGuid().ToString();
                 
                    Events.Event_Photo = model.Event_Photo;
                
                    Events.Event_Title = model.Event_Title;
                
                    Events.Event_DateTime = System.DateTime.Now;
                    Events.Event_Active = false;
                    Events.Cat_Id = model.Cat_Id;
                    Events.Event_Publisher = Request.GetOwinContext().Request.User.Identity.GetUserId();



                    Context.AspNetEvents.Add(Events);
                    int Records = await Context.SaveChangesAsync();

                    if (Records > 0)
                    {
                        return Ok(Events);

                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }


                }

                else
                {
                    return Unauthorized();
                }


            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException.ToString());
            }


        }


        // GET: api/Dashboard/Events
        [HttpGet]
        [AllowAnonymous]
        [Route("Events")]
        [ActionName("Events")]
        public async Task<IHttpActionResult> Events()
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {

                    var header = Request.GetOwinContext().Response.Headers.SingleOrDefault(h => h.Key == "Access-Control-Allow-Origin");
                    if (header.Equals(default(KeyValuePair<string, string[]>)))
                    {
                        Request.GetOwinContext().Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });



                    }

                    IEnumerable<AspNetEvent> Events = Context.AspNetEvents.ToList();
                    Events.ToList().ForEach(GetEventCategoryByIdAsync);
                    return Ok(Events);



                }
                else
                {

                    var header = Request.GetOwinContext().Response.Headers.SingleOrDefault(h => h.Key == "Access-Control-Allow-Origin");
                    if (header.Equals(default(KeyValuePair<string, string[]>)))
                    {
                        Request.GetOwinContext().Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });



                    }
                    return Unauthorized();
                }




            }
            catch (Exception ex)
            {

                return BadRequest(ex.InnerException.ToString());
            }


        }

        // help action for display coureses customizable
        private void GetEventCategoryByIdAsync(AspNetEvent obj)
        {
            obj.Cat_Id = Context.AspNetEventsCats.Find(obj.Cat_Id).Cat_Name;

            if (obj.Event_Photo == "EventIMG-2.jpeg")
            {
                obj.Event_Photo = "http://localhost:50816/Events/Default_Images/" + obj.Event_Photo;


            }
            else
            {
                obj.Event_Photo = "http://localhost:50816/Events/Processing_Events/" + obj.Event_Photo;
            }





            ApplicationUser UserInfo = UserManager.FindById(obj.Event_Publisher);


            if (UserInfo != null)
            {



                int InstructorExistedFlag = Context.AspNetInstructors.Where(m => m.Usr_Id == UserInfo.Id).Count();
                int ManagerExistedFlag = Context.AspNetManagers.Where(m => m.Usr_Id == UserInfo.Id).Count();

                if (InstructorExistedFlag > 0)
                {
                    AspNetInstructor InstructorExisted = Context.AspNetInstructors.Where(m => m.Usr_Id == UserInfo.Id).SingleOrDefault();


                    string FirstName = InstructorExisted.Inst_Fname;
                    string LastName = InstructorExisted.Inst_Lname;
                    string FullName = FirstName + " " + LastName;
                    obj.Event_Publisher = FullName;
                }
                else if (ManagerExistedFlag > 0)
                {
                    AspNetManager ManagerExisted = Context.AspNetManagers.Where(m => m.Usr_Id == UserInfo.Id).SingleOrDefault();


                    string FirstName = ManagerExisted.Manager_Fname;
                    string LastName = ManagerExisted.Manager_Lname;
                    string FullName = FirstName + " " + LastName;
                    obj.Event_Publisher = FullName;
                }


            }




        }



        private static void ResizeImage(int size, string filePath, string saveFilePath)
        {
            //variables for image dimension/scale

            double newHeight = 0;
            double newWidth = 0;
            double scale = 0;

            //create new image object
            Bitmap curImage = new Bitmap(filePath);

            //Determine image scaling
            if (curImage.Height > curImage.Width)
            {
                scale = Convert.ToSingle(size) / curImage.Height;
            }
            else
            {
                scale = Convert.ToSingle(size) / curImage.Width;
            }
            if (scale < 0 || scale > 1) { scale = 1; }

            //New image dimension
            newHeight = Math.Floor(Convert.ToSingle(curImage.Height) * scale);
            newWidth = Math.Floor(Convert.ToSingle(curImage.Width) * scale);

            //Create new object image
            Bitmap newImage = new Bitmap(curImage, Convert.ToInt32(newWidth), Convert.ToInt32(newHeight));
            Graphics imgDest = Graphics.FromImage(newImage);
            imgDest.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            imgDest.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            imgDest.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            imgDest.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
            EncoderParameters param = new EncoderParameters(1);
            param.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);

            //Draw the object image
            imgDest.DrawImage(curImage, 0, 0, newImage.Width, newImage.Height);

            //Save image file
            newImage.Save(saveFilePath, info[1], param);

            //Dispose the image objects
            curImage.Dispose();
            newImage.Dispose();
            imgDest.Dispose();


        }


        // POST: api/Dashboard/UploadEventIamge
        [HttpPost]
        [AllowAnonymous]
        [Route("UploadEventsIamge")]
        public async Task<IHttpActionResult> UploadEventsIamge()
        {
            try
            {

                var httpRequest = HttpContext.Current.Request;
                HttpPostedFile httppostedfile = null;

                if (httpRequest.Files.Count > 0)

                {
                    httppostedfile = httpRequest.Files[0];


                }


                var supportedTypes = new[] { ".jpg", ".jpeg", ".png" };

                var fileextention = Path.GetExtension(httppostedfile.FileName);

                //.........check file Extention

                if (!supportedTypes.Contains(fileextention))
                {

                    string Event_ErrorMessage = "File Extension Is InValid - Only Upload (.jpg , .jpeg, .png)";



                    return BadRequest(Event_ErrorMessage);



                }

                string fileName = Guid.NewGuid().ToString() + httppostedfile.FileName;
                string pathoriginal = Path.Combine(HttpContext.Current.Request.MapPath("~/Events/Original_Events"), fileName).ToString();

                string pathresized = Path.Combine(HttpContext.Current.Request.MapPath("~/Events/Processing_Events"), fileName).ToString();
                httppostedfile.SaveAs(pathoriginal);

                ResizeImage(350, pathoriginal, pathresized);


                return Ok(fileName);




            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message.ToString());
            }


        }




    }
}
