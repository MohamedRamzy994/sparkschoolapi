using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARKAPI.Models
{
    public class ProgrammesViewModel
    {
    }


    public class ProgrammeCategoriesViewModel
    {

        public string Cat_Id { get; set; }
        public string Cat_Name { get; set; }
        public System.DateTime Cat_DateTime { get; set; }
        public string Cat_Publisher { get; set; }
        public bool Cat_Active { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

    }

    public class ProgrammeViewModel
    {

        public string Programe_Id { get; set; }
        public string Programe_Name { get; set; }
        public string Programe_Description { get; set; }
        public int Programe_NumCourses { get; set; }
        public decimal Programe_Price { get; set; }
        public System.DateTime Programe_StartDateTime { get; set; }
        public System.DateTime Programe_DateTime { get; set; }
        public string Programe_Photo { get; set; }
        public string Cat_Id { get; set; }
        public string Programe_Publisher { get; set; }
        public string Programe_ErrorMessage { get; set; }
        public string Programe_Duration { get; set; }

        public Boolean Programe_Active { get; set; }

        public virtual ICollection<AspNetProgrameCours> AspNetProgrameCourses { get; set; }
        public virtual AspNetPrograme AspNetProgrames1 { get; set; }
        public virtual AspNetPrograme AspNetPrograme1 { get; set; }
        public virtual AspNetProgrammesCategory AspNetProgrammesCategory { get; set; }




    }
    public class ProgrameCoursesViewModel
    {

        public string Crs_Id { get; set; }
        public string Crs_Name { get; set; }
        public int Crs_Numlessons { get; set; }
        public string Crs_Level { get; set; }
        public string Crs_Description { get; set; }
        public string Crs_Photo { get; set; }
        public Nullable<int> Crs_Duration { get; set; }
        public bool Crs_Active { get; set; }
        public System.DateTime Crs_DateTime { get; set; }
        public string CrsCats_Id { get; set; }
        public decimal Crs_Price { get; set; }
        public string Crs_Video { get; set; }
        public string Crs_Publisher { get; set; }

        public string Crs_ErrorMessage { get; set; }
        public string Programe_Id { get; set; }




        public virtual ICollection<AspNetCourseLesson> AspNetCourseLessons { get; set; }
        public virtual AspNetCoursesCategory AspNetCoursesCategory { get; set; }


        public virtual AspNetCoursesCategory AspNetCoursesCategory1 { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }

    }
   





}