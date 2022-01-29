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
    [RoutePrefix("api/News")]
    public class NewsController : ApiController
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






        // GET: api/News/NewsCategories
        [HttpGet]

        [AllowAnonymous]
        [Route("NewsCategories")]
        public async Task<IHttpActionResult> NewsCategories()
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    IEnumerable<AspNetNewsCategory> NewsCategories = Context.AspNetNewsCategories;

                    NewsCategories.ToList().ForEach(GetPublisherName);

                    return Ok(NewsCategories);
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


        private void GetPublisherName(AspNetNewsCategory obj)
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

        // GET: api/News/NewsCategories
        [HttpGet]
        [AllowAnonymous]
        [Route("ActiveNewsCategories")]
        public async Task<IHttpActionResult> ActiveNewsCategories()
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    IEnumerable<AspNetNewsCategory> ActiveNewsCategories = Context.AspNetNewsCategories.Where(m => m.Cat_Active == true).ToList();

                    return Ok(ActiveNewsCategories);
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
        [Route("NewNewsCategory")]
        public async Task<IHttpActionResult> NewNewsCategory(NewsCategoriesViewModel model)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    AspNetNewsCategory Category = new AspNetNewsCategory();

                    Category.Cat_Name = model.Cat_Name;
                    Category.Cat_Id = Guid.NewGuid().ToString();
                    Category.Cat_DateTime = DateTime.Now;
                    Category.Cat_Active = false;
                    Category.Cat_Publisher= Request.GetOwinContext().Request.User.Identity.GetUserId();



                    Context.AspNetNewsCategories.Add(Category);

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
        [Route("EditNewsCategory")]
        public async Task<IHttpActionResult> EditNewsCategory(NewsCategoriesViewModel model)
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
                        AspNetNewsCategory Category = Context.AspNetNewsCategories.Find(model.Cat_Id);

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
        [Route("EditNewsCategory")]
        public async Task<IHttpActionResult> EditNewsCategory(string Cat_Id)
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
                        AspNetNewsCategory Category = Context.AspNetNewsCategories.Find(Cat_Id);

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


        // Delete: api/News/DeleteNewsCategories

        [HttpDelete]
        [AllowAnonymous]
        [Route("DeleteNewsCategory")]
        public async Task<IHttpActionResult> DeleteNewsCategory(string Cat_Id)
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
                        AspNetNewsCategory Category = Context.AspNetNewsCategories.Find(Cat_Id);

                        if (Category != null)
                        {

                            Context.AspNetNewsCategories.Remove(Category);

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


        // POST : api/News/ActivateNewCategory
        [HttpPost]
        [AllowAnonymous]
        [Route("ActivateNewsCategory")]
        public async Task<IHttpActionResult> ActivateNewsCategory(string Cat_Id)
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

                        AspNetNewsCategory Category = Context.AspNetNewsCategories.Find(Cat_Id);

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
     

        // POST: api/News/ActivateNews
        [HttpPost]
        [AllowAnonymous]
        [Route("ActivateNews")]
        public async Task<IHttpActionResult> ActivateNews(string New_Id)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!string.IsNullOrEmpty(New_Id))
                    {

                        AspNetNew News = Context.AspNetNews.Find(New_Id);

                        if (News.News_Active == true)
                        {

                            News.News_Active = false;
                        }
                        else if (News.News_Active == false)
                        {

                            News.News_Active = true;
                        }

                        int Records = await Context.SaveChangesAsync();

                        if (Records > 0)
                        {
                            return Ok(News);

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


        // DELETE: api/News/DeleteNews
        [HttpDelete]
        [AllowAnonymous]
        [Route("DeleteNews")]
        public async Task<IHttpActionResult> DeleteNews(string New_Id)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!string.IsNullOrEmpty(New_Id))
                    {

                        AspNetNew News = Context.AspNetNews.Find(New_Id);

                        Context.AspNetNews.Remove(News);


                        int Records = await Context.SaveChangesAsync();

                        if (Records > 0)
                        {
                            return Ok(News);

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
        [Route("EditNews")]
        public async Task<IHttpActionResult> EditNews(NewsViewModel model)
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

                    if (!string.IsNullOrEmpty(model.News_Id))
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
                                string pathoriginal = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Newsoriginal"), fileName).ToString();

                                string pathresized = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Newsresized"), fileName).ToString();
                                New_Photo.SaveAs(pathoriginal);
                                ResizeImage(350, pathoriginal, pathresized);

                                AspNetNew News = Context.AspNetNews.Find(model.News_Id);

                                News.News_Photo = model.News_Photo;

                                News.News_Title = model.News_Title;

                                News.News_DateTime = System.DateTime.Now;
                             
                                News.Cat_Id = model.Cat_Id;



                                int Records = await Context.SaveChangesAsync();

                                if (Records > 0)
                                {
                                    return Ok(News);

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
                            string pathoriginal = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Newssoriginal"), fileName).ToString();

                            string pathresized = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Newssresized"), fileName).ToString();
                            New_Photo.SaveAs(pathoriginal);
                            ResizeImage(350, pathoriginal, pathresized);

                            AspNetNew News = Context.AspNetNews.Find(model.News_Id);

                            News.News_Photo = model.News_Photo;

                            News.News_Title = model.News_Title;

                            News.News_DateTime = System.DateTime.Now;
                    
                            News.Cat_Id = model.Cat_Id;



                            int Records = await Context.SaveChangesAsync();

                            if (Records > 0)
                            {
                                return Ok(News);

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
        [Route("EditNews")]
        public async Task<IHttpActionResult> EditNews(string News_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(News_Id))
                    {
                        AspNetNew News = Context.AspNetNews.Find(News_Id);

                        if (News != null)
                        {

                            return Ok(News);

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

        // POST: api/News/NewNews
        [HttpPost]
        [AllowAnonymous]
        [Route("NewNews")]
        public async Task<IHttpActionResult> NewNews(NewsViewModel model)
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    AspNetNew News = new AspNetNew();

                    News.News_Id = System.Guid.NewGuid().ToString();
                 
                    News.News_Photo = model.News_Photo;
                
                    News.News_Title = model.News_Title;

                    News.News_Description = model.News_Description;
                    News.News_Content = model.News_Content;
                    News.News_DateTime = System.DateTime.Now;
                    News.News_Active = false;
                    News.Cat_Id = model.Cat_Id;
                    News.News_Publisher = Request.GetOwinContext().Request.User.Identity.GetUserId();



                    Context.AspNetNews.Add(News);

                    int Records = await Context.SaveChangesAsync();

                    if (Records > 0)
                    {
                        return Ok(News);

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


        // GET: api/News/News
        [HttpGet]
        [AllowAnonymous]
        [Route("News")]
        [ActionName("News")]
        public async Task<IHttpActionResult> News()
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

                    IEnumerable<AspNetNew> News = Context.AspNetNews.ToList();
                    News.ToList().ForEach(GetNewCategoryByIdAsync);
                    return Ok(News);



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
        private void GetNewCategoryByIdAsync(AspNetNew obj)
        {
            obj.Cat_Id = Context.AspNetNewsCategories.Find(obj.Cat_Id).Cat_Name;

            if (obj.News_Photo == "CRSIMG-2.jpeg")
            {
                obj.News_Photo = "http://localhost:50816/Courses/Default_Images/" + obj.News_Photo;


            }
            else
            {
                obj.News_Photo = "http://localhost:50816/News/Processing_News/" + obj.News_Photo;
            }





            ApplicationUser UserInfo = UserManager.FindById(obj.News_Publisher);


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
                    obj.News_Publisher = FullName;
                }
                else if (ManagerExistedFlag > 0)
                {
                    AspNetManager ManagerExisted = Context.AspNetManagers.Where(m => m.Usr_Id == UserInfo.Id).SingleOrDefault();


                    string FirstName = ManagerExisted.Manager_Fname;
                    string LastName = ManagerExisted.Manager_Lname;
                    string FullName = FirstName + " " + LastName;
                    obj.News_Publisher = FullName;
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


        // POST: api/News/UploadNewIamge
        [HttpPost]
        [AllowAnonymous]
        [Route("UploadNewsIamge")]
        public async Task<IHttpActionResult> UploadNewsIamge()
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
                string pathoriginal = Path.Combine(HttpContext.Current.Request.MapPath("~/News/Original_News"), fileName).ToString();

                string pathresized = Path.Combine(HttpContext.Current.Request.MapPath("~/News/Processing_News"), fileName).ToString();
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
