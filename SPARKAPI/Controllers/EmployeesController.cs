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
    [RoutePrefix("api/Employees")]
    public class EmployeesController : ApiController
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

        // POST: api/News/ActivateNews
        [HttpPost]
        [AllowAnonymous]
        [Route("ActivateEmployee")]
        public async Task<IHttpActionResult> ActivateEmployee(string Employee_Id)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!string.IsNullOrEmpty(Employee_Id))
                    {

                        AspNetEmployee News = Context.AspNetEmployees.Find(Employee_Id);

                        if (News.Employee_Active == true)
                        {

                            News.Employee_Active = false;
                        }
                        else if (News.Employee_Active == false)
                        {

                            News.Employee_Active = true;
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
        [Route("DeleteEmployee")]
        public async Task<IHttpActionResult> DeleteEmployee(string Employee_Id)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!string.IsNullOrEmpty(Employee_Id))
                    {

                        AspNetEmployee Employee = Context.AspNetEmployees.Find(Employee_Id);

                        Context.AspNetEmployees.Remove(Employee);


                        int Records = await Context.SaveChangesAsync();

                        if (Records > 0)
                        {
                            return Ok(Employee);

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

        //[HttpPut]
        //[AllowAnonymous]
        //[Route("EditNews")]
        //public async Task<IHttpActionResult> EditNews(NewsViewModel model)
        //{
        //    try
        //    {
        //        HttpPostedFile New_Photo = HttpContext.Current.Request.Files[0];

        //        if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                return BadRequest(ModelState);
        //            }

        //            if (!string.IsNullOrEmpty(model.News_Id))
        //            {


        //                if (New_Photo != null && New_Photo.ContentLength > 0)
        //                {
        //                    var supportedTypes = new[] { ".jpg", ".jpeg", ".png" };

        //                    var fileextention = Path.GetExtension(New_Photo.FileName);

        //                    //.........check file Extention

        //                    if (!supportedTypes.Contains(fileextention))
        //                    {

        //                        string ErrorMessage = "File Extension Is InValid - Only Upload (.jpg , .jpeg, .png)";


        //                        return BadRequest(ErrorMessage);


        //                    }
        //                    else
        //                    {

        //                        string fileName = Guid.NewGuid() + Path.GetExtension(New_Photo.FileName);
        //                        string pathoriginal = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Newsoriginal"), fileName).ToString();

        //                        string pathresized = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Newsresized"), fileName).ToString();
        //                        New_Photo.SaveAs(pathoriginal);
        //                        ResizeImage(350, pathoriginal, pathresized);

        //                        AspNetNew News = Context.AspNetNews.Find(model.News_Id);

        //                        News.News_Photo = model.News_Photo;

        //                        News.News_Title = model.News_Title;

        //                        News.News_DateTime = System.DateTime.Now;

        //                        News.Cat_Id = model.Cat_Id;



        //                        int Records = await Context.SaveChangesAsync();

        //                        if (Records > 0)
        //                        {
        //                            return Ok(News);

        //                        }
        //                        else
        //                        {
        //                            return BadRequest(ModelState);
        //                        }


        //                    }
        //                }

        //                else
        //                {


        //                    string fileName = Guid.NewGuid() + Path.GetExtension(New_Photo.FileName);
        //                    string pathoriginal = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Newssoriginal"), fileName).ToString();

        //                    string pathresized = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Newssresized"), fileName).ToString();
        //                    New_Photo.SaveAs(pathoriginal);
        //                    ResizeImage(350, pathoriginal, pathresized);

        //                    AspNetNew News = Context.AspNetNews.Find(model.News_Id);

        //                    News.News_Photo = model.News_Photo;

        //                    News.News_Title = model.News_Title;

        //                    News.News_DateTime = System.DateTime.Now;

        //                    News.Cat_Id = model.Cat_Id;



        //                    int Records = await Context.SaveChangesAsync();

        //                    if (Records > 0)
        //                    {
        //                        return Ok(News);

        //                    }
        //                    else
        //                    {
        //                        return BadRequest(ModelState);
        //                    }


        //                }


        //            }

        //            else
        //            {
        //                return NotFound();
        //            }


        //        }
        //        else
        //        {
        //            return Unauthorized();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(ex.Message);
        //    }
        //}


        [HttpGet]
        [AllowAnonymous]
        [Route("EditEmployee")]
        public async Task<IHttpActionResult> EditEmployee(string Employee_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(Employee_Id))
                    {
                        AspNetEmployee Employee = Context.AspNetEmployees.Find(Employee_Id);

                        if (Employee != null)
                        {

                            return Ok(Employee);

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
        [Route("NewEmployee")]
        public async Task<IHttpActionResult> NewEmployee(EmployeeViewModel model)
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    AspNetEmployee Employee = new AspNetEmployee();

                    Employee.Employee_Id = System.Guid.NewGuid().ToString();
                 
                    Employee.Employee_Salary = model.Employee_Salary;
                
                    Employee.Employee_Name = model.Employee_Name;
                    
                    Employee.Employee_Phone = model.Employee_Phone;
                    Employee.Employee_Photo = model.Employee_Photo;

                    Employee.Employee_DateTime = System.DateTime.Now;
                    Employee.Employee_Active = false;
                    
                    Employee.Employee_Publisher = Request.GetOwinContext().Request.User.Identity.GetUserId();



                    Context.AspNetEmployees.Add(Employee);

                    int Records = await Context.SaveChangesAsync();

                    if (Records > 0)
                    {
                        return Ok(Employee);

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
        [Route("Employees")]
        [ActionName("Employees")]
        public async Task<IHttpActionResult> Employees()
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

                    IEnumerable<AspNetEmployee> News = Context.AspNetEmployees.ToList();
                    News.ToList().ForEach(Customization);
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
        private void Customization(AspNetEmployee obj)
        {


            if (obj.Employee_Photo == "avatar.jpg")
            {
                obj.Employee_Photo = "http://localhost:50816/Employees/Default_Images/" + obj.Employee_Photo;


            }
            else
            {
                obj.Employee_Photo = "http://localhost:50816/Employees/Processing_Employees/" + obj.Employee_Photo;
            }


            ApplicationUser UserInfo = UserManager.FindById(obj.Employee_Publisher);


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
                    obj.Employee_Publisher = FullName;
                }
                else if (ManagerExistedFlag > 0)
                {
                    AspNetManager ManagerExisted = Context.AspNetManagers.Where(m => m.Usr_Id == UserInfo.Id).SingleOrDefault();


                    string FirstName = ManagerExisted.Manager_Fname;
                    string LastName = ManagerExisted.Manager_Lname;
                    string FullName = FirstName + " " + LastName;
                    obj.Employee_Publisher = FullName;
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
        [Route("UploadEmployeesImage")]
        public async Task<IHttpActionResult> UploadEmployeesImage()
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
                string pathoriginal = Path.Combine(HttpContext.Current.Request.MapPath("~/Employees/Original_Employees"), fileName).ToString();

                string pathresized = Path.Combine(HttpContext.Current.Request.MapPath("~/Employees/Processing_Employees"), fileName).ToString();
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
