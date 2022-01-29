using SPARKAPI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SPARKAPI.Controllers
{
    [RoutePrefix("api/Teacher")]
    [Authorize]
    public class TeacherController : ApiController
    {

        private SPARKEntities Context = new SPARKEntities();


        #region TeacherArea
        // GET: api/Teacher/InstructorInfo
        [HttpGet]
        [AllowAnonymous]
        [Route("InstructorInfo")]
        public async Task<IHttpActionResult> InstructorInfo()
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

                    string usr_Id = (Request.GetOwinContext().Request.User.Identity.GetUserId()).ToString();

                    AspNetInstructor InstructorInfo = Context.AspNetInstructors.Where(m => m.Usr_Id ==usr_Id ).SingleOrDefault();

                    return Ok(InstructorInfo);



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

                return BadRequest(ex.Message);
            }


        }



        // GET : api/Teacher/GetInstructorCourses

        [HttpGet]
        [AllowAnonymous]
        [Route("GetInstructorCourses")]
        public async Task<IHttpActionResult> GetInstructorCourses()
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

                    string usr_Id = (Request.GetOwinContext().Request.User.Identity.GetUserId()).ToString();

                    IEnumerable<AspNetCours> InstructorCourses = Context.AspNetCourses.Where(m => m.Crs_Publisher == usr_Id).ToList();

                    return Ok(InstructorCourses);



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

                return BadRequest(ex.Message);
            }


        }




    }



    #endregion
}