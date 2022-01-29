using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using SPARKAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;



namespace SPARKAPI.Controllers
{

    [RoutePrefix("api/Programes")]
    //[Authorize]
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProgramesController : ApiController
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


        // GET: api/Programes/ProgramesCategories
        [HttpGet]

        [AllowAnonymous]
        [Route("ProgramesCategories")]
        public async Task<IHttpActionResult> ProgramesCategories()
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    IEnumerable<AspNetProgrammesCategory> ProgramesCategories = Context.AspNetProgrammesCategories;

                    ProgramesCategories.ToList().ForEach(GetPublisherName);
                    return Ok(ProgramesCategories);
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



        private void GetPublisherName(AspNetProgrammesCategory obj)
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


        // GET: api/Programes/ProgramesCategories
        [HttpGet]
        [AllowAnonymous]
        [Route("ActiveProgramesCategories")]
        public async Task<IHttpActionResult> ActiveProgramesCategories()
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    IEnumerable<AspNetProgrammesCategory> ActiveProgramesCategories = Context.AspNetProgrammesCategories.Where(m => m.Cat_Active == true).ToList();

                    return Ok(ActiveProgramesCategories);
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
        [Route("NewProgramesCategory")]
        public async Task<IHttpActionResult> NewProgramesCategory(ProgrammeCategoriesViewModel model)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    AspNetProgrammesCategory Category = new AspNetProgrammesCategory();

                    Category.Cat_Name = model.Cat_Name;
                    Category.Cat_Id = Guid.NewGuid().ToString();
                    Category.Cat_DateTime = DateTime.Now;
                    Category.Cat_Active = false;
                    Category.Cat_Publisher = Request.GetOwinContext().Request.User.Identity.GetUserId();



                    Context.AspNetProgrammesCategories.Add(Category);

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
        [Route("EditProgramesCategory")]
        public async Task<IHttpActionResult> EditProgramesCategory(ProgrammeCategoriesViewModel model)
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
                        AspNetProgrammesCategory Category = Context.AspNetProgrammesCategories.Find(model.Cat_Id);

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
        [Route("EditProgramesCategory")]
        public async Task<IHttpActionResult> EditProgramesCategory(string Cat_Id)
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
                        AspNetProgrammesCategory Category = Context.AspNetProgrammesCategories.Find(Cat_Id);

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


        // Delete: api/Programes/DeleteProgramesCategories

        [HttpDelete]
        [AllowAnonymous]
        [Route("DeleteProgrameCategory")]
        public async Task<IHttpActionResult> DeleteProgrameCategory(string Cat_Id)
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
                        AspNetProgrammesCategory Category = Context.AspNetProgrammesCategories.Find(Cat_Id);

                        if (Category != null)
                        {

                            Context.AspNetProgrammesCategories.Remove(Category);

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


        // POST : api/Programes/ActivateProgrameCategory
        [HttpPost]
        [AllowAnonymous]
        [Route("ActivateProgrameCategory")]
        public async Task<IHttpActionResult> ActivateProgrameCategory(string Cat_Id)
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

                        AspNetProgrammesCategory Category = Context.AspNetProgrammesCategories.Find(Cat_Id);

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


        // GET:  api/Programes/GetCourseName
        [HttpGet]
        [AllowAnonymous]
        [Route("GetProgrameName")]
        public async Task<IHttpActionResult> GetProgrameName(string Programe_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(Programe_Id))
                    {
                        AspNetPrograme Programe = Context.AspNetProgrames.Find(Programe_Id);

                        if (Programe != null)
                        {

                            return Ok(Programe);

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



        // GET: api/Programes/Courses
        [HttpGet]
        [AllowAnonymous]
        [Route("GetProgrameCourses")]
        [ActionName("GetProgrameCourses")]
        public async Task<IHttpActionResult> GetProgrameCourses(string Programe_Id)
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

                    IEnumerable<AspNetProgrameCours> Courses = Context.AspNetProgrameCourses.ToList().Where(p=>p.Programe_Id==Programe_Id);
                    Courses.ToList().ForEach(GetCourseCategoryByIdAsync);
                    return Ok(Courses);



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

        //help action for display coureses customizable
        private void GetCourseCategoryByIdAsync(AspNetProgrameCours obj)
        {
            obj.CrsCats_Id = Context.AspNetCoursesCategories.Find(obj.CrsCats_Id).CrsCats_Name;

            if (obj.Crs_Photo == "CRSIMG-2.jpeg")
            {
                obj.Crs_Photo = "http://localhost:50816/Courses/Default_Images/" + obj.Crs_Photo;


            }
            else
            {
                obj.Crs_Photo = "http://localhost:50816/Courses/Processing_Courses/" + obj.Crs_Photo;
            }
            if (obj.Crs_Video == "CrsVideo.mp4")
            {
                obj.Crs_Video = "http://localhost:50816/Courses/Default_Images/" + obj.Crs_Video;


            }
            else
            {
                obj.Crs_Video = "http://localhost:50816/Courses/Courses_Videos/" + obj.Crs_Video;
            }





            ApplicationUser UserInfo = UserManager.FindById(obj.Crs_Publisher);


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
                    obj.Crs_Publisher = FullName;
                }
                else if (ManagerExistedFlag > 0)
                {
                    AspNetManager ManagerExisted = Context.AspNetManagers.Where(m => m.Usr_Id == UserInfo.Id).SingleOrDefault();


                    string FirstName = ManagerExisted.Manager_Fname;
                    string LastName = ManagerExisted.Manager_Lname;
                    string FullName = FirstName + " " + LastName;
                    obj.Crs_Publisher = FullName;
                }


            }


            int NumLessons = Context.AspNetCourseLessons.Where(m => m.Crs_Id == obj.Crs_Id).Count();

            if (NumLessons > obj.Crs_Numlessons)
            {

                obj.Crs_Numlessons = NumLessons;

            }





        }


        // POST: api/Dashboard/NewCourse
        [HttpPost]
        [AllowAnonymous]
        [Route("NewProgrameCourse")]
        public async Task<IHttpActionResult> NewProgrameCourse(ProgrameCoursesViewModel model)
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }




                    AspNetProgrameCours Course = new AspNetProgrameCours();

                    Course.Crs_Id = System.Guid.NewGuid().ToString();
                    Course.Crs_Price = Convert.ToDecimal(model.Crs_Price);
                    Course.Crs_Photo = model.Crs_Photo;
                    Course.Crs_Numlessons = model.Crs_Numlessons;
                    Course.Crs_Video = model.Crs_Video;
                    Course.Crs_Name = model.Crs_Name;
                    Course.Crs_Level = model.Crs_Level;
                    Course.Crs_Duration = model.Crs_Duration;
                    Course.Crs_Description = model.Crs_Description;
                    Course.Crs_DateTime = System.DateTime.Now;
                    Course.Crs_Active = false;
                    Course.CrsCats_Id = model.CrsCats_Id;
                    Course.Crs_Publisher = Request.GetOwinContext().Request.User.Identity.GetUserId();
                    Course.Programe_Id = model.Programe_Id;



                    Context.AspNetProgrameCourses.Add(Course);
                    int Records = await Context.SaveChangesAsync();

                    if (Records > 0)
                    {
                        return Ok(Course);

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


        // POST : api/Programes/ActivateProgrameCourse
        [HttpPost]
        [AllowAnonymous]
        [Route("ActivateProgrameCourse")]
        public async Task<IHttpActionResult> ActivateProgrameCourse(string Crs_Id)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!string.IsNullOrEmpty(Crs_Id))
                    {

                        AspNetProgrameCours Course = Context.AspNetProgrameCourses.Find(Crs_Id);

                        if (Course.Crs_Active == true)
                        {

                            Course.Crs_Active = false;
                        }
                        else if (Course.Crs_Active == false)
                        {

                            Course.Crs_Active = true;
                        }

                        int Records = await Context.SaveChangesAsync();

                        if (Records > 0)
                        {
                            return Ok(Course);

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



        // DELETE: api/Dashboard/DeleteCourse
        [HttpDelete]
        [AllowAnonymous]
        [Route("DeleteProgrameCourses")]
        public async Task<IHttpActionResult> DeleteProgrameCourses(string Crs_Id)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!string.IsNullOrEmpty(Crs_Id))
                    {

                        AspNetProgrameCours Course = Context.AspNetProgrameCourses.Find(Crs_Id);

                        Context.AspNetProgrameCourses.Remove(Course);


                        int Records = await Context.SaveChangesAsync();

                        if (Records > 0)
                        {
                            return Ok(Course);

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



       

        // GET: api/Programes/Programes
        [HttpGet]
        [AllowAnonymous]
        [Route("Programes")]
        [ActionName("Programes")]
        public async Task<IHttpActionResult> Programes()
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

                    IEnumerable<AspNetPrograme> Programes = Context.AspNetProgrames.ToList();
                    Programes.ToList().ForEach(GetProgrameCategoryByIdAsync);
                    return Ok(Programes);



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
        private void GetProgrameCategoryByIdAsync(AspNetPrograme obj)
        {
            obj.Cat_Id = Context.AspNetProgrammesCategories.Find(obj.Cat_Id).Cat_Name;

            if (obj.Programe_Photo == "ProgrameIMG-2.jpeg")
            {
                obj.Programe_Photo = "http://localhost:50816/Programes/Default_Images/" + obj.Programe_Photo;


            }
            else
            {
                obj.Programe_Photo = "http://localhost:50816/Programes/Processing_Programes/" + obj.Programe_Photo;
            }





            ApplicationUser UserInfo = UserManager.FindById(obj.Programe_Publisher);


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
                    obj.Programe_Publisher = FullName;
                }
                else if (ManagerExistedFlag > 0)
                {
                    AspNetManager ManagerExisted = Context.AspNetManagers.Where(m => m.Usr_Id == UserInfo.Id).SingleOrDefault();


                    string FirstName = ManagerExisted.Manager_Fname;
                    string LastName = ManagerExisted.Manager_Lname;
                    string FullName = FirstName + " " + LastName;
                    obj.Programe_Publisher = FullName;
                }


            }


            int NumLessons = Context.AspNetProgrameCourses.Where(m => m.Programe_Id == obj.Programe_Id).Count();

            if (NumLessons > obj.Programe_NumCourses)
            {

                obj.Programe_NumCourses = NumLessons;

            }





        }

        // POST: api/Programes/ActivatePrograme
        [HttpPost]
        [AllowAnonymous]
        [Route("ActivatePrograme")]
        public async Task<IHttpActionResult> ActivatePrograme(string Programe_Id)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!string.IsNullOrEmpty(Programe_Id))
                    {

                        AspNetPrograme Programe = Context.AspNetProgrames.Find(Programe_Id);

                        if (Programe.Programe_Active == true)
                        {

                            Programe.Programe_Active = false;
                        }
                        else if (Programe.Programe_Active == false)
                        {

                            Programe.Programe_Active = true;
                        }

                        int Records = await Context.SaveChangesAsync();

                        if (Records > 0)
                        {
                            return Ok(Programe);

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



   

      // DELETE: api/Programes/DeletePrograme
      [HttpDelete]
        [AllowAnonymous]
        [Route("DeletePrograme")]
        public async Task<IHttpActionResult> DeletePrograme(string Programe_Id)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!string.IsNullOrEmpty(Programe_Id))
                    {

                        AspNetPrograme Programe = Context.AspNetProgrames.Find(Programe_Id);

                        Context.AspNetProgrames.Remove(Programe);


                        int Records = await Context.SaveChangesAsync();

                        if (Records > 0)
                        {
                            return Ok(Programe);

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
        [Route("EditPrograme")]
        public async Task<IHttpActionResult> EditPrograme(ProgrammeViewModel model)
        {
            try
            {
                HttpPostedFile Programe_Photo = HttpContext.Current.Request.Files[0];

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(model.Programe_Id))
                    {


                        if (Programe_Photo != null && Programe_Photo.ContentLength > 0)
                        {
                            var supportedTypes = new[] { ".jpg", ".jpeg", ".png" };

                            var fileextention = Path.GetExtension(Programe_Photo.FileName);

                            //.........check file Extention

                            if (!supportedTypes.Contains(fileextention))
                            {

                                model.Programe_ErrorMessage = "File Extension Is InValid - Only Upload (.jpg , .jpeg, .png)";


                                return BadRequest(model.Programe_ErrorMessage);


                            }
                            else
                            {

                                string fileName = Guid.NewGuid() + Path.GetExtension(Programe_Photo.FileName);
                                string pathoriginal = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Programesoriginal"), fileName).ToString();

                                string pathresized = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Programesresized"), fileName).ToString();
                                Programe_Photo.SaveAs(pathoriginal);
                                ResizeImage(350, pathoriginal, pathresized);

                                AspNetPrograme Programe = Context.AspNetProgrames.Find(model.Programe_Id);

                                Programe.Programe_Name = model.Programe_Name;
                              
                                Programe.Programe_NumCourses = model.Programe_NumCourses;
                                Programe.Programe_Price = model.Programe_Price;
                                Programe.Programe_Description = model.Programe_Description;
                                Programe.Programe_StartDateTime = model.Programe_StartDateTime;
                                Programe.Programe_Photo = fileName;
                                Programe.Cat_Id = model.Cat_Id;



                                int Records = await Context.SaveChangesAsync();

                                if (Records > 0)
                                {
                                    return Ok(Programe);

                                }
                                else
                                {
                                    return BadRequest(ModelState);
                                }


                            }
                        }

                        else
                        {


                            string fileName = Guid.NewGuid() + Path.GetExtension(Programe_Photo.FileName);
                            string pathoriginal = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Programesoriginal"), fileName).ToString();

                            string pathresized = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_Programesresized"), fileName).ToString();
                            Programe_Photo.SaveAs(pathoriginal);
                            ResizeImage(350, pathoriginal, pathresized);

                            AspNetPrograme Programe = Context.AspNetProgrames.Find(model.Programe_Id);

                            Programe.Programe_Name = model.Programe_Name;
                
                            Programe.Programe_NumCourses = model.Programe_NumCourses;
                            Programe.Programe_Price = model.Programe_Price;
                            Programe.Programe_Description = model.Programe_Description;
                         
                            Programe.Programe_Photo = fileName;
                            Programe.Cat_Id = model.Cat_Id;



                            int Records = await Context.SaveChangesAsync();

                            if (Records > 0)
                            {
                                return Ok(Programe);

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
        [Route("EditPrograme")]
        public async Task<IHttpActionResult> EditPrograme(string Programe_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(Programe_Id))
                    {
                        AspNetPrograme Programe = Context.AspNetProgrames.Find(Programe_Id);

                        if (Programe != null)
                        {

                            return Ok(Programe);

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

        // POST: api/Programes/NewPrograme
        [HttpPost]
        [AllowAnonymous]
        [Route("NewPrograme")]
        public async Task<IHttpActionResult> NewPrograme(ProgrammeViewModel model)
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }




                    AspNetPrograme Programe = new AspNetPrograme();

                    Programe.Programe_Id = System.Guid.NewGuid().ToString();
                    Programe.Programe_Price = Convert.ToDecimal(model.Programe_Price);
                    Programe.Programe_Photo = model.Programe_Photo;
                    Programe.Programe_NumCourses = model.Programe_NumCourses;
                    Programe.Programe_Name = model.Programe_Name;
                    Programe.Programe_Description = model.Programe_Description;
                    Programe.Programe_Duration = model.Programe_Duration;
                    Programe.Programe_DateTime = System.DateTime.Now;
                    Programe.Programe_StartDateTime = (DateTime)model.Programe_StartDateTime;
                    Programe.Programe_Active = false;
                    Programe.Cat_Id = model.Cat_Id;
                    Programe.Programe_Publisher = Request.GetOwinContext().Request.User.Identity.GetUserId();



                    Context.AspNetProgrames.Add(Programe);
                    int Records = await Context.SaveChangesAsync();

                    if (Records > 0)
                    {
                        return Ok(Programe);

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



        [HttpPost]
        [AllowAnonymous]
        [Route("NewProgrameCourseGroup")]
        public async Task<IHttpActionResult> NewProgrameCourseGroup(CourseGroupsViewModel Model)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(Model.Crs_Id))
                    {
                        AspNetProgrameCourseGroup Group = new AspNetProgrameCourseGroup();

                        Group.Group_Id = Guid.NewGuid().ToString();
                        Group.Group_Active = false;
                        Group.Group_Name = Model.Group_Name;
                        Group.Crs_Id = Model.Crs_Id;

                        Context.AspNetProgrameCourseGroups.Add(Group);

                        foreach (DateTime item in Model.Group_Times)
                        {
                            AspNetProgrameCourseGroupsTime GroupTime = new AspNetProgrameCourseGroupsTime();
                            GroupTime.Group_Time_Id = Guid.NewGuid().ToString();
                            GroupTime.Group_Time = item;
                            GroupTime.Group_Id = Group.Group_Id;
                            Context.AspNetProgrameCourseGroupsTimes.Add(GroupTime);



                        }

                        foreach (string item in Model.Group_Instructors)
                        {
                            AspNetProgrameCourseGroupsInstructor GroupInstructor = new AspNetProgrameCourseGroupsInstructor();
                            GroupInstructor.Group_Instructors_Id = Guid.NewGuid().ToString();
                            GroupInstructor.Group_Instructor_Name = item;
                            GroupInstructor.Group_Id = Group.Group_Id;
                            Context.AspNetProgrameCourseGroupsInstructors.Add(GroupInstructor);


                        }

                        await Context.SaveChangesAsync();

                        if (Context.GetValidationErrors().Count() == 0)
                        {

                            return Ok(Group);

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
        [Route("GetProgrameCourseGroups")]
        public async Task<IHttpActionResult> GetCourseGroups(string Crs_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(Crs_Id))
                    {

                        var Groups = Context.AspNetProgrameCourseGroups.GroupJoin(Context.AspNetProgrameCourseGroupsInstructors, g => g.Group_Id, d => d.Group_Id, (g, d) => new { g, d }).Where(g=>g.g.Crs_Id==Crs_Id).Select(m => m.g);



                        return Ok(Groups);


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
        [Route("GetProgrameCourseGroupsInstructorName")]
        public async Task<IHttpActionResult> GetCourseGroupsInstructorName(string Instructor_Id)
        {

            if (!string.IsNullOrEmpty(Instructor_Id))
            {
                string FName = Context.AspNetInstructors.Where(m => m.Inst_Id == Instructor_Id).SingleOrDefault().Inst_Fname;

                string LName = Context.AspNetInstructors.Where(m => m.Inst_Id == Instructor_Id).SingleOrDefault().Inst_Lname;

                Instructor_Id = FName + " " + LName;


                return Ok(Instructor_Id);


            }
            else
            {
                return BadRequest();
            }




        }


        [HttpPost]
        [AllowAnonymous]
        [Route("ActiveProgrameCoursesGroupsFactory")]
        public async Task<IHttpActionResult> ActiveProgrameCoursesGroupsFactory(string Group_Id)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!string.IsNullOrEmpty(Group_Id))
                    {

                        AspNetProgrameCourseGroup Group = Context.AspNetProgrameCourseGroups.Find(Group_Id);

                        if (Group.Group_Active == true)
                        {

                            Group.Group_Active = false;
                        }
                        else if (Group.Group_Active == false)
                        {

                            Group.Group_Active = true;
                        }

                        int Records = await Context.SaveChangesAsync();

                        if (Records > 0)
                        {
                            return Ok(Group);

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



        [HttpDelete]
        [AllowAnonymous]
        [Route("DeleteProgrameCourseGroup")]
        public async Task<IHttpActionResult> DeleteProgrameCourseGroup(string Group_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(Group_Id))
                    {
                        AspNetProgrameCourseGroup Group = Context.AspNetProgrameCourseGroups.Find(Group_Id);
                        IEnumerable<AspNetProgrameCourseGroupsInstructor> Instructors = Context.AspNetProgrameCourseGroupsInstructors.Where(i => i.Group_Id == Group.Group_Id).ToList();
                        IEnumerable<AspNetProgrameCourseGroupsTime> Times = Context.AspNetProgrameCourseGroupsTimes.Where(t => t.Group_Id == Group.Group_Id).ToList();
                        if (Group != null)
                        {

                            Context.AspNetProgrameCourseGroupsInstructors.RemoveRange(Instructors);
                            Context.AspNetProgrameCourseGroupsTimes.RemoveRange(Times);

                            Context.AspNetProgrameCourseGroups.Remove(Group);

                            int record = await Context.SaveChangesAsync();

                            if (record > 0)
                            {

                                return Ok(Group);

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



        [HttpGet]
        [AllowAnonymous]
        [Route("EditProgrameCourseGroup")]
        public async Task<IHttpActionResult> EditCourseGroup(string Group_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(Group_Id))
                    {
                        var Group = Context.AspNetCourseGroups.GroupJoin(Context.AspNetCourseGroupsInstructors, g => g.Group_Id, d => d.Group_Id, (g, d) => new { g, d })
                            .Select(m => m.g).Where(m => m.Group_Id == Group_Id).SingleOrDefault();

                        if (Group != null)
                        {

                            return Ok(Group);

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


        [HttpGet]
        [AllowAnonymous]
        [Route("GetProgrameCourseName")]
        public async Task<IHttpActionResult> GetProgrameCourseName(string Crs_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(Crs_Id))
                    {
                        AspNetProgrameCours Course = Context.AspNetProgrameCourses.Find(Crs_Id);

                        if (Course != null)
                        {

                            return Ok(Course);

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



        #region Help Methods DashboardController
        //--------------------------------- Help Methods--------------------------------

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

    
        // POST: api/Dashboard/UploadProgrameIamge
        [HttpPost]
        [AllowAnonymous]
        [Route("UploadProgrameIamge")]
        public async Task<IHttpActionResult> UploadProgrameIamge()
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

                    string Crs_ErrorMessage = "File Extension Is InValid - Only Upload (.jpg , .jpeg, .png)";



                    return BadRequest(Crs_ErrorMessage);



                }

                string fileName = Guid.NewGuid().ToString() + httppostedfile.FileName;
                string pathoriginal = Path.Combine(HttpContext.Current.Request.MapPath("~/Programes/Original_Programes"), fileName).ToString();

                string pathresized = Path.Combine(HttpContext.Current.Request.MapPath("~/Programes/Processing_Programes"), fileName).ToString();
                httppostedfile.SaveAs(pathoriginal);

                ResizeImage(350, pathoriginal, pathresized);


                return Ok(fileName);




            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message.ToString());
            }


        }




        #endregion 

    }
}





