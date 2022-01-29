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

    [RoutePrefix("api/Dashboard")]
    //[Authorize]
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DashboardController : ApiController
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
        // GET: api/Dashboard/Courses
        [HttpGet]
        [AllowAnonymous]
        [Route("Courses")]
        [ActionName("Courses")]
        public async Task<IHttpActionResult> Courses()
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

                    IEnumerable<AspNetCours> Courses = Context.AspNetCourses.ToList();
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

        // help action for display coureses customizable
        private void GetCourseCategoryByIdAsync(AspNetCours obj)
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

        // POST: api/Dashboard/ActivateCourse
        [HttpPost]
        [AllowAnonymous]
        [Route("ActivateCourse")]
        public async Task<IHttpActionResult> ActivateCourse(string Crs_Id)
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

                        AspNetCours Course = Context.AspNetCourses.Find(Crs_Id);

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
        [Route("DeleteCourse")]
        public async Task<IHttpActionResult> DeleteCourse(string Crs_Id)
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

                        AspNetCours Course = Context.AspNetCourses.Find(Crs_Id);

                        Context.AspNetCourses.Remove(Course);


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

        [HttpPut]
        [AllowAnonymous]
        [Route("EditCourse")]
        public async Task<IHttpActionResult> EditCourse(CoursesViewModel model)
        {
            try
            {
                HttpPostedFile Crs_Photo = HttpContext.Current.Request.Files[0];

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(model.Crs_Id))
                    {


                        if (Crs_Photo != null && Crs_Photo.ContentLength > 0)
                        {
                            var supportedTypes = new[] { ".jpg", ".jpeg", ".png" };

                            var fileextention = Path.GetExtension(Crs_Photo.FileName);

                            //.........check file Extention

                            if (!supportedTypes.Contains(fileextention))
                            {

                                model.Crs_ErrorMessage = "File Extension Is InValid - Only Upload (.jpg , .jpeg, .png)";


                                return BadRequest(model.Crs_ErrorMessage);


                            }
                            else
                            {

                                string fileName = Guid.NewGuid() + Path.GetExtension(Crs_Photo.FileName);
                                string pathoriginal = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_coursesoriginal"), fileName).ToString();

                                string pathresized = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_coursesresized"), fileName).ToString();
                                Crs_Photo.SaveAs(pathoriginal);
                                ResizeImage(350, pathoriginal, pathresized);

                                AspNetCours Course = Context.AspNetCourses.Find(model.Crs_Id);

                                Course.Crs_Name = model.Crs_Name;
                                Course.Crs_Level = model.Crs_Level;
                                Course.Crs_Numlessons = model.Crs_Numlessons;
                                Course.Crs_Price = model.Crs_Price;
                                Course.Crs_Description = model.Crs_Description;
                                Course.Crs_Duration = model.Crs_Duration;
                                Course.Crs_Photo = fileName;
                                Course.CrsCats_Id = model.CrsCats_Id;



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
                        }

                        else
                        {


                            string fileName = Guid.NewGuid() + Path.GetExtension(Crs_Photo.FileName);
                            string pathoriginal = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_coursesoriginal"), fileName).ToString();

                            string pathresized = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/app/appimages/appimages_coursesresized"), fileName).ToString();
                            Crs_Photo.SaveAs(pathoriginal);
                            ResizeImage(350, pathoriginal, pathresized);

                            AspNetCours Course = Context.AspNetCourses.Find(model.Crs_Id);

                            Course.Crs_Name = model.Crs_Name;
                            Course.Crs_Level = model.Crs_Level;
                            Course.Crs_Numlessons = model.Crs_Numlessons;
                            Course.Crs_Price = model.Crs_Price;
                            Course.Crs_Description = model.Crs_Description;
                            Course.Crs_Duration = model.Crs_Duration;
                            Course.Crs_Photo = fileName;
                            Course.CrsCats_Id = model.CrsCats_Id;



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
        [Route("EditCourse")]
        public async Task<IHttpActionResult> EditCourse(string Crs_Id)
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
                        AspNetCours Course = Context.AspNetCourses.Find(Crs_Id);

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

        // POST: api/Dashboard/NewCourse
        [HttpPost]
        [AllowAnonymous]
        [Route("NewCourse")]
        public async Task<IHttpActionResult> NewCourse(CoursesViewModel model)
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }




                    AspNetCours Course = new AspNetCours();

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



                    Context.AspNetCourses.Add(Course);
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


        // GET: api/Dashboard/CoursesCategories
        [HttpGet]

        [AllowAnonymous]
        [Route("CoursesCategories")]
        public async Task<IHttpActionResult> CoursesCategories()
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    IEnumerable<AspNetCoursesCategory> CoursesCategories = Context.AspNetCoursesCategories;
                    CoursesCategories.ToList().ForEach(GetPublisherName);

                    return Ok(CoursesCategories);
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

        private void GetPublisherName(AspNetCoursesCategory obj)
        {


            ApplicationUser UserInfo = UserManager.FindById(obj.CrsCats_Publisher);


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
                    obj.CrsCats_Publisher = FullName;
                }
                else if (ManagerExistedFlag > 0)
                {
                    AspNetManager ManagerExisted = Context.AspNetManagers.Where(m => m.Usr_Id == UserInfo.Id).SingleOrDefault();


                    string FirstName = ManagerExisted.Manager_Fname;
                    string LastName = ManagerExisted.Manager_Lname;
                    string FullName = FirstName + " " + LastName;
                    obj.CrsCats_Publisher = FullName;
                }


            }



        }


        // GET: api/Dashboard/CoursesCategories
        [HttpGet]
        [AllowAnonymous]
        [Route("ActiveCoursesCategories")]
        public async Task<IHttpActionResult> ActiveCoursesCategories()
        {
            try
            {
                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    IEnumerable<AspNetCoursesCategory> ActiveCoursesCategories = Context.AspNetCoursesCategories.Where(m => m.CrsCats_Active == true).ToList();

                    return Ok(ActiveCoursesCategories);
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
        [Route("NewCourseCategory")]
        public async Task<IHttpActionResult> NewCourseCategory(CoursesCategoriesViewModel model)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    AspNetCoursesCategory Category = new AspNetCoursesCategory();

                    Category.CrsCats_Name = model.CrsCats_Name;
                    Category.CrsCats_Id = Guid.NewGuid().ToString();
                    Category.CrsCats_DateTime = DateTime.Now;
                    Category.CrsCats_Active = true;
                    Category.CrsCats_Publisher = Request.GetOwinContext().Request.User.Identity.GetUserId();



                    Context.AspNetCoursesCategories.Add(Category);

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
        [Route("EditCourseCategory")]
        public async Task<IHttpActionResult> EditCourseCategory(CoursesCategoriesViewModel model)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(model.CrsCats_Id))
                    {
                        AspNetCoursesCategory Category = Context.AspNetCoursesCategories.Find(model.CrsCats_Id);

                        Category.CrsCats_Name = model.CrsCats_Name;

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
        [Route("EditCourseCategory")]
        public async Task<IHttpActionResult> EditCourseCategory(string CrsCats_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(CrsCats_Id))
                    {
                        AspNetCoursesCategory Category = Context.AspNetCoursesCategories.Find(CrsCats_Id);

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


        // Delete: api/Dashboard/DeleteCoursesCategories

        [HttpDelete]
        [AllowAnonymous]
        [Route("DeleteCourseCategory")]
        public async Task<IHttpActionResult> DeleteCourseCategory(string CrsCats_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(CrsCats_Id))
                    {
                        AspNetCoursesCategory Category = Context.AspNetCoursesCategories.Find(CrsCats_Id);

                        if (Category != null)
                        {

                            Context.AspNetCoursesCategories.Remove(Category);

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


        // POST : api/Dashboard/ActivateCourseCategory
        [HttpPost]
        [AllowAnonymous]
        [Route("ActivateCourseCategory")]
        public async Task<IHttpActionResult> ActivateCourseCategory(string CrsCats_Id)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!string.IsNullOrEmpty(CrsCats_Id))
                    {

                        AspNetCoursesCategory Category = Context.AspNetCoursesCategories.Find(CrsCats_Id);

                        if (Category.CrsCats_Active == true)
                        {

                            Category.CrsCats_Active = false;
                        }
                        else if (Category.CrsCats_Active == false)
                        {

                            Category.CrsCats_Active = true;
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
        // GET:  api/Dashboard/GetCourseName
        [HttpGet]
        [AllowAnonymous]
        [Route("GetCourseName")]
        public async Task<IHttpActionResult> GetCourseName(string Crs_Id)
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
                        AspNetCours Course = Context.AspNetCourses.Find(Crs_Id);

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



        // POST: api/Dashboard/NewCourseLesson
        [HttpPost]
        [AllowAnonymous]
        [Route("NewCourseLesson")]
        public async Task<IHttpActionResult> NewCourseLesson(CourseLessonViewModel model)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    AspNetCourseLesson CourseLesson = new AspNetCourseLesson();

                    CourseLesson.Lesson_Id = System.Guid.NewGuid().ToString();
                    CourseLesson.Lesson_Name = model.Lesson_Name;
                    CourseLesson.Lesson_Instructor = model.Lesson_Instructor;
                    CourseLesson.Lesson_SessionTime = model.Lesson_SessionTime;
                    CourseLesson.Lesson_Typescript = model.Lesson_Typescript;
          
                    CourseLesson.Lesson_Photo = model.Lesson_Photo;
                    CourseLesson.Lesson_Duration = model.Lesson_Duration;
                    CourseLesson.Lesson_DateTime = System.DateTime.Now;
                    CourseLesson.Lesson_Active = false;
                    CourseLesson.Crs_Id = model.Crs_Id;
                    CourseLesson.Lesson_Publisher = User.Identity.GetUserId();



                    Context.AspNetCourseLessons.Add(CourseLesson);



                    int Records = await Context.SaveChangesAsync();

                    if (Records > 0)
                    {
                        return Ok(CourseLesson);

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


                                                           

        // GET:  api/Dashboard/GetALLInstructors
        [HttpGet]
        [AllowAnonymous]
        [Route("GetALLInstructors")]
        public async Task<IHttpActionResult> GetALLInstructors()
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    IEnumerable<AspNetInstructor> Instructors = Context.AspNetInstructors.ToList();


                    return Ok(Instructors);

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



        // GET: api/Dashboard/Courses
        [HttpGet]
        [AllowAnonymous]
        [Route("GetCourseLessons")]
        public async Task<IHttpActionResult> GetCourseLessons(string Crs_Id)
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


                    IEnumerable<AspNetCourseLesson> CourseLessons = Context.AspNetCourseLessons.Where(m => m.Crs_Id == Crs_Id);
                    CourseLessons.ToList().ForEach(GetLessonInstructorById);
                    return Ok(CourseLessons);


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


        //DELETE:/api/Dashboard/DeleteCourseLessons

        [HttpDelete]
        [AllowAnonymous]
        [Route("DeleteCourseLessons")]
        public async Task<IHttpActionResult> DeleteCourseLessons(string Lesson_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(Lesson_Id))
                    {
                        AspNetCourseLesson Lesson = Context.AspNetCourseLessons.Find(Lesson_Id);

                        if (Lesson != null)
                        {

                            Context.AspNetCourseLessons.Remove(Lesson);

                            int record = await Context.SaveChangesAsync();

                            if (record > 0)
                            {

                                return Ok(Lesson);

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

        // POST : api/Dashboard/ActivateCourseLesson
        [HttpPost]
        [AllowAnonymous]
        [Route("ActivateCourseLesson")]
        public async Task<IHttpActionResult> ActivateCourseLesson(string Lesson_Id)
        {
            try
            {

                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    if (!string.IsNullOrEmpty(Lesson_Id))
                    {

                        AspNetCourseLesson Lesson = Context.AspNetCourseLessons.Find(Lesson_Id);

                        if (Lesson.Lesson_Active == true)
                        {

                            Lesson.Lesson_Active = false;
                        }
                        else if (Lesson.Lesson_Active == false)
                        {

                            Lesson.Lesson_Active = true;
                        }

                        int Records = await Context.SaveChangesAsync();

                        if (Records > 0)
                        {
                            return Ok(Lesson);

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

        [HttpGet]
        [AllowAnonymous]
        [Route("GetCourseLessonName")]
        public async Task<IHttpActionResult> GetCourseLessonName(string Lesson_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(Lesson_Id))
                    {
                        AspNetCourseLesson Lesson = Context.AspNetCourseLessons.Find(Lesson_Id);

                        if (Lesson != null)
                        {

                            return Ok(Lesson);

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



        [HttpPut]
        [AllowAnonymous]
        [Route("EditCourseLesson")]
        public async Task<IHttpActionResult> EditCourseLesson(CourseLessonViewModel model)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(model.Lesson_Id))
                    {
                        AspNetCourseLesson CourseLesson = Context.AspNetCourseLessons.Find(model.Lesson_Id);



                        CourseLesson.Lesson_Name = model.Lesson_Name;
                        CourseLesson.Lesson_Instructor = model.Lesson_Instructor;
                        CourseLesson.Lesson_SessionTime = model.Lesson_SessionTime;
                        CourseLesson.Lesson_Typescript = model.Lesson_Typescript;
                   
                        CourseLesson.Lesson_Photo = model.Lesson_Photo;
                        CourseLesson.Lesson_Duration = model.Lesson_Duration;




                        int Records = await Context.SaveChangesAsync();

                        if (Records > 0)
                        {

                            return Ok(CourseLesson);

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
        [Route("EditCourseLesson")]
        public async Task<IHttpActionResult> EditCourseLesson(string Lesson_Id)
        {
            try
            {


                if (Request.GetOwinContext().Request.User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!string.IsNullOrEmpty(Lesson_Id))
                    {
                        AspNetCourseLesson CourseLesson = Context.AspNetCourseLessons.Find(Lesson_Id);

                        if (CourseLesson != null)
                        {

                            return Ok(CourseLesson);

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


        [HttpPost]
        [AllowAnonymous]
        [Route("NewCourseGroup")]
        public async Task<IHttpActionResult> NewCourseGroup(CourseGroupsViewModel Model)
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
                        AspNetCourseGroup Group = new AspNetCourseGroup();

                        Group.Group_Id = Guid.NewGuid().ToString();
                        Group.Group_Active = false;
                        Group.Group_Name = Model.Group_Name;
                        Group.Crs_Id = Model.Crs_Id;

                        Context.AspNetCourseGroups.Add(Group);

                        foreach (DateTime item in Model.Group_Times)
                        {
                            AspNetCourseGroupsTime GroupTime = new AspNetCourseGroupsTime();
                            GroupTime.Group_Time_Id = Guid.NewGuid().ToString();
                            GroupTime.Group_Time = item;
                            GroupTime.Group_Id = Group.Group_Id;
                            Context.AspNetCourseGroupsTimes.Add(GroupTime);



                        }

                        foreach (string item in Model.Group_Instructors)
                        {
                            AspNetCourseGroupsInstructor GroupInstructor = new AspNetCourseGroupsInstructor();
                            GroupInstructor.Group_Instructors_Id = Guid.NewGuid().ToString();
                            GroupInstructor.Group_Instructor_Name = item;
                            GroupInstructor.Group_Id = Group.Group_Id;
                            Context.AspNetCourseGroupsInstructors.Add(GroupInstructor);


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
        [Route("GetCourseGroups")]
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

                        var Groups = Context.AspNetCourseGroups.GroupJoin(Context.AspNetCourseGroupsInstructors, g => g.Group_Id, d => d.Group_Id, (g, d) => new { g, d }).Select(m=>m.g);
                           


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
        [Route("GetCourseGroupsInstructorName")]
        public async  Task<IHttpActionResult> GetCourseGroupsInstructorName(string Instructor_Id)
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
        [Route("ActiveCoursesGroupsFactory")]
        public async Task<IHttpActionResult> ActiveCoursesGroupsFactory(string Group_Id)
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

                        AspNetCourseGroup Group = Context.AspNetCourseGroups.Find(Group_Id);

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
        [Route("DeleteCourseGroup")]
        public async Task<IHttpActionResult> DeleteCourseGroup(string Group_Id)
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
                        AspNetCourseGroup Group = Context.AspNetCourseGroups.Find(Group_Id);
                       IEnumerable<AspNetCourseGroupsInstructor> Instructors = Context.AspNetCourseGroupsInstructors.Where(i => i.Group_Id == Group.Group_Id).ToList();
                        IEnumerable<AspNetCourseGroupsTime> Times = Context.AspNetCourseGroupsTimes.Where(t => t.Group_Id == Group.Group_Id).ToList();
                        if (Group != null)
                        {

                            Context.AspNetCourseGroupsInstructors.RemoveRange(Instructors);
                            Context.AspNetCourseGroupsTimes.RemoveRange(Times);

                            Context.AspNetCourseGroups.Remove(Group);

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
        [Route("EditCourseGroup")]
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
                            .Select(m => m.g).Where(m=>m.Group_Id==Group_Id).SingleOrDefault();

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




        #region Help Methods DashboardController

        //--------------------------------- Help Methods--------------------------------
        // POST: api/Dashboard/UploadCourseIamge
        [HttpPost]
        [AllowAnonymous]
        [Route("UploadCourseIamge")]
        public async Task<IHttpActionResult> UploadCourseIamge()
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
                string pathoriginal = Path.Combine(HttpContext.Current.Request.MapPath("~/Courses/Original_Courses"), fileName).ToString();

                string pathresized = Path.Combine(HttpContext.Current.Request.MapPath("~/Courses/Processing_Courses"), fileName).ToString();
                httppostedfile.SaveAs(pathoriginal);

                ResizeImage(350, pathoriginal, pathresized);


                return Ok(fileName);




            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message.ToString());
            }


        }

        // POST: api/Dashboard/UploadCourseIamge
        [HttpPost]
        [AllowAnonymous]
        [Route("UploadLessonIamge")]
        public async Task<IHttpActionResult> UploadLessonIamge()
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
                string pathoriginal = Path.Combine(HttpContext.Current.Request.MapPath("~/Courses/Original_Lessons"), fileName).ToString();

                string pathresized = Path.Combine(HttpContext.Current.Request.MapPath("~/Courses/Processing_Lessons"), fileName).ToString();
                httppostedfile.SaveAs(pathoriginal);

                ResizeImage(350, pathoriginal, pathresized);


                return Ok(fileName);




            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message.ToString());
            }


        }


        // POST: api/Dashboard/CourseLessonVideoUpload
        [HttpPost]
        [AllowAnonymous]
        [Route("CourseVideoUpload")]
        public async Task<IHttpActionResult> CourseVideoUpload()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;


                if (httpRequest.Files.Count > 0)

                {

                 


                    HttpPostedFile httppostedfile = httpRequest.Files[0];


                    var supportedTypes = new[] { ".mpeg ,.ogg", ".mp4" };

                    var fileextention = Path.GetExtension(httppostedfile.FileName);

                    //.........check file Extention

                    if (!supportedTypes.Contains(fileextention))
                    {

                        string Crs_ErrorMessage = "File Extension Is InValid - Only Upload (.mpeg , .ogg, .mp4 )";



                        return BadRequest(Crs_ErrorMessage);



                    }



                    string fileName = Guid.NewGuid().ToString() + httppostedfile.FileName;
                    string pathoriginal = Path.Combine(HttpContext.Current.Request.MapPath("~/Courses/Courses_Videos"), fileName).ToString();

                    httppostedfile.SaveAs(pathoriginal);



                    return Ok(fileName);

                }
                else
                {
                    return BadRequest("something wrong happening try again!");
                }


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }


        // help action for display coureses customizable
        private void GetLessonInstructorById(AspNetCourseLesson obj)
        {

            if (obj.Lesson_Photo == "LessonIMG-2.jpeg")
            {
                obj.Lesson_Photo = "http://localhost:50816/Courses/Default_Images/" + obj.Lesson_Photo;


            }
            else
            {
                obj.Lesson_Photo = "http://localhost:50816/Courses/Processing_Lessons/" + obj.Lesson_Photo;
            }
           
           


            string FirstName = Context.AspNetInstructors.Where(m => m.Inst_Id == obj.Lesson_Instructor).SingleOrDefault().Inst_Fname;
            string LastName = Context.AspNetInstructors.Where(m => m.Inst_Id == obj.Lesson_Instructor).SingleOrDefault().Inst_Lname;
            string FullName = FirstName + " " + LastName;
            obj.Lesson_Instructor = FullName;


            string PublisherFirstName = Context.AspNetManagers.Where(m => m.Usr_Id == obj.Lesson_Publisher).SingleOrDefault().Manager_Fname;
            string PublisherLastName = Context.AspNetManagers.Where(m => m.Usr_Id == obj.Lesson_Publisher).SingleOrDefault().Manager_Lname;
            string PublisherFullName = PublisherFirstName + " " + PublisherLastName;
            obj.Lesson_Publisher = PublisherFullName;



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
    



        #endregion APPLETCoursesArea


    }
}
