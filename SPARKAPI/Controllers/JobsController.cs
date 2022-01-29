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
    [RoutePrefix("api/Jobs")]
    public class JobsController : ApiController
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
        // GET: api/Jobs/JobsCategories
        [HttpGet]
        [AllowAnonymous]
        [Route("JobsCategories")]
        public async Task<IHttpActionResult> JobsCategories()
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    IEnumerable<AspNetJobsCategory> JobsCategories = Context.AspNetJobsCategories;

                    JobsCategories.ToList().ForEach(GetPublisherName);

                    return Ok(JobsCategories);
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


        private void GetPublisherName(AspNetJobsCategory obj)
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


        // GET: api/Jobs/JobsCategories
        [HttpGet]
        [AllowAnonymous]
        [Route("ActiveJobsCategories")]
        public async Task<IHttpActionResult> ActiveJobsCategories()
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    IEnumerable<AspNetJobsCategory> ActiveJobsCategories = Context.AspNetJobsCategories.Where(m => m.Cat_Active == true).ToList();

                    return Ok(ActiveJobsCategories);
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
        [Route("NewJobsCategory")]
        public async Task<IHttpActionResult> NewJobsCategory(JobsCategoriesViewModel model)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    AspNetJobsCategory Category = new AspNetJobsCategory();

                    Category.Cat_Name = model.Cat_Name;
                    Category.Cat_Id = Guid.NewGuid().ToString();
                    Category.Cat_DateTime = DateTime.Now;
                    Category.Cat_Active = false;
                    Category.Cat_Publisher= Request.GetOwinContext().Request.User.Identity.GetUserId();



                    Context.AspNetJobsCategories.Add(Category);

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
        [Route("EditJobsCategory")]
        public async Task<IHttpActionResult> EditJobsCategory(JobsCategoriesViewModel model)
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
                        AspNetJobsCategory Category = Context.AspNetJobsCategories.Find(model.Cat_Id);

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
        [Route("EditJobsCategory")]
        public async Task<IHttpActionResult> EditJobsCategory(string Cat_Id)
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
                        AspNetJobsCategory Category = Context.AspNetJobsCategories.Find(Cat_Id);

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


        // Delete: api/Jobs/DeleteJobsCategories

        [HttpDelete]
        [AllowAnonymous]
        [Route("DeleteJobsCategory")]
        public async Task<IHttpActionResult> DeleteJobsCategory(string Cat_Id)
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
                        AspNetJobsCategory Category = Context.AspNetJobsCategories.Find(Cat_Id);

                        if (Category != null)
                        {

                            Context.AspNetJobsCategories.Remove(Category);

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


        // POST : api/Jobs/ActivateNewCategory
        [HttpPost]
        [AllowAnonymous]
        [Route("ActivateJobsCategory")]
        public async Task<IHttpActionResult> ActivateJobsCategory(string Cat_Id)
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

                        AspNetJobsCategory Category = Context.AspNetJobsCategories.Find(Cat_Id);

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


       // POST: api/Jobs/ActivateJobs
       [HttpPost]
       [AllowAnonymous]
       [Route("ActivateJobs")]
        public async Task<IHttpActionResult> ActivateJobs(string Job_Id)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!string.IsNullOrEmpty(Job_Id))
                    {

                        AspNetJob Jobs = Context.AspNetJobs.Find(Job_Id);

                        if (Jobs.Job_Active == true)
                        {

                            Jobs.Job_Active = false;
                        }
                        else if (Jobs.Job_Active == false)
                        {

                            Jobs.Job_Active = true;
                        }

                        int Records = await Context.SaveChangesAsync();

                        if (Records > 0)
                        {
                            return Ok(Jobs);

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


       // DELETE: api/Jobs/DeleteJobs
       [HttpDelete]
       [AllowAnonymous]
       [Route("DeleteJobs")]
        public async Task<IHttpActionResult> DeleteJobs(string Job_Id)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!string.IsNullOrEmpty(Job_Id))
                    {

                        AspNetJob Jobs = Context.AspNetJobs.Find(Job_Id);

                        Context.AspNetJobs.Remove(Jobs);


                        int Records = await Context.SaveChangesAsync();

                        if (Records > 0)
                        {
                            return Ok(Jobs);

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
        [Route("EditJobs")]
        public async Task<IHttpActionResult> EditJobs(JobsViewModel model)
        {
            try
            {
                HttpPostedFile New_Photo = HttpContext.Current.Request.Files[0];

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(model.Job_Id))
                    {


                        if (New_Photo != null && New_Photo.ContentLength > 0)
                        {
                            var supportedTypes = new[] { ".jpg", ".jpeg", ".png" };

                            var fileextention = Path.GetExtension(New_Photo.FileName);

                            //.........check file Extention

                            if (!supportedTypes.Contains(fileextention))
                            {

                                string ErrorMessage = "File Extension Is InValid - Only Upload (.jpg , .jpeg, .png)";


                                return BadRequest(ErrorMessage);


                            }
                            else
                            {

                                string fileName = Guid.NewGuid() + Path.GetExtension(New_Photo.FileName);
                                string pathoriginal = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Jobsoriginal"), fileName).ToString();

                                string pathresized = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Jobsresized"), fileName).ToString();
                                New_Photo.SaveAs(pathoriginal);
                                ResizeImage(350, pathoriginal, pathresized);

                                AspNetJob Jobs = Context.AspNetJobs.Find(model.Job_Id);

                                Jobs.Job_Photo = model.Job_Photo;

                                Jobs.Job_Title = model.Job_Title;

                                Jobs.Job_DateTime = System.DateTime.Now;

                                Jobs.Cat_Id = model.Cat_Id;
                                Jobs.Job_Description = model.Job_Description;



                                int Records = await Context.SaveChangesAsync();

                                if (Records > 0)
                                {
                                    return Ok(Jobs);

                                }
                                else
                                {
                                    return BadRequest(ModelState);
                                }


                            }
                        }

                        else
                        {


                            string fileName = Guid.NewGuid() + Path.GetExtension(New_Photo.FileName);
                            string pathoriginal = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Jobssoriginal"), fileName).ToString();

                            string pathresized = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Jobssresized"), fileName).ToString();
                            New_Photo.SaveAs(pathoriginal);
                            ResizeImage(350, pathoriginal, pathresized);

                            AspNetJob Jobs = Context.AspNetJobs.Find(model.Job_Id);

                            Jobs.Job_Photo = model.Job_Photo;

                            Jobs.Job_Title = model.Job_Title;

                            Jobs.Job_DateTime = System.DateTime.Now;

                            Jobs.Cat_Id = model.Cat_Id;

                            Jobs.Job_Description = model.Job_Description;



                            int Records = await Context.SaveChangesAsync();

                            if (Records > 0)
                            {
                                return Ok(Jobs);

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
        [Route("EditJobs")]
        public async Task<IHttpActionResult> EditJobs(string Job_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(Job_Id))
                    {
                        AspNetJob Jobs = Context.AspNetJobs.Find(Job_Id);

                        if (Jobs != null)
                        {

                            return Ok(Jobs);

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

        //POST: api/Jobs/NewJobs
       [HttpPost]
       [AllowAnonymous]
       [Route("NewJobs")]
        public async Task<IHttpActionResult> NewJobs(JobsViewModel model)
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    AspNetJob Jobs = new AspNetJob();

                    Jobs.Job_Id = System.Guid.NewGuid().ToString();

                    Jobs.Job_Photo = model.Job_Photo;

                    Jobs.Job_Title = model.Job_Title;
                    Jobs.Job_Country = model.Job_Country;
                    Jobs.Job_City = model.Job_City;

                    Jobs.Job_Description = model.Job_Description;
                   
                    Jobs.Job_DateTime = System.DateTime.Now;
                    Jobs.Job_Active = false;
                    Jobs.Cat_Id = model.Cat_Id;
                    Jobs.Job_Publisher = Request.GetOwinContext().Request.User.Identity.GetUserId();



                    Context.AspNetJobs.Add(Jobs);

                    int Records = await Context.SaveChangesAsync();

                    if (Records > 0)
                    {
                        return Ok(Jobs);

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

       // GET: api/Jobs/Jobs
       [HttpGet]
       [AllowAnonymous]
       [Route("Jobs")]
       [ActionName("Jobs")]
        public async Task<IHttpActionResult> Jobs()
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

                    IEnumerable<AspNetJob> Jobs = Context.AspNetJobs.ToList();
                    Jobs.ToList().ForEach(GetNewCategoryByIdAsync);
                    return Ok(Jobs);



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
        private void GetNewCategoryByIdAsync(AspNetJob obj)
        {
            obj.Cat_Id = Context.AspNetJobsCategories.Find(obj.Cat_Id).Cat_Name;

            if (obj.Job_Photo == "JobsIMG-2.jpeg")
            {
                obj.Job_Photo = "http://localhost:50816/Jobs/Default_Images/" + obj.Job_Photo;


            }
            else
            {
                obj.Job_Photo = "http://localhost:50816/Jobs/Processing_Jobs/" + obj.Job_Photo;
            }





            ApplicationUser UserInfo = UserManager.FindById(obj.Job_Publisher);


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
                    obj.Job_Publisher = FullName;
                }
                else if (ManagerExistedFlag > 0)
                {
                    AspNetManager ManagerExisted = Context.AspNetManagers.Where(m => m.Usr_Id == UserInfo.Id).SingleOrDefault();


                    string FirstName = ManagerExisted.Manager_Fname;
                    string LastName = ManagerExisted.Manager_Lname;
                    string FullName = FirstName + " " + LastName;
                    obj.Job_Publisher = FullName;
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


        // POST: api/Jobs/UploadNewIamge
        [HttpPost]
        [AllowAnonymous]
        [Route("UploadJobsIamge")]
        public async Task<IHttpActionResult> UploadJobsIamge()
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

                    string New_ErrorMessage = "File Extension Is InValid - Only Upload (.jpg , .jpeg, .png)";



                    return BadRequest(New_ErrorMessage);



                }

                string fileName = Guid.NewGuid().ToString() + httppostedfile.FileName;
                string pathoriginal = Path.Combine(HttpContext.Current.Request.MapPath("~/Jobs/Original_Jobs"), fileName).ToString();

                string pathresized = Path.Combine(HttpContext.Current.Request.MapPath("~/Jobs/Processing_Jobs"), fileName).ToString();
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
