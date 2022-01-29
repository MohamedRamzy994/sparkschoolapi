using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SPARKAPI.Models
{
    public class CoursesViewModel
    {

        public string Crs_Id { get; set; }
        public string Crs_Name { get; set; }
        public int Crs_Numlessons { get; set; }
        public string Crs_Level { get; set; }
        public string Crs_Description { get; set; }
        public string  Crs_Photo { get; set; }
        public Nullable<int> Crs_Duration { get; set; }
        public bool Crs_Active { get; set; }
        public System.DateTime Crs_DateTime { get; set; }
        public string CrsCats_Id { get; set; }
        public decimal Crs_Price { get; set; }
        public string Crs_Video { get; set; }
        public string Crs_Publisher { get; set; }

        public string Crs_ErrorMessage { get; set; }
    



        public virtual ICollection<AspNetCourseLesson> AspNetCourseLessons { get; set; }
        public virtual AspNetCoursesCategory AspNetCoursesCategory { get; set; }

      
        public virtual AspNetCoursesCategory AspNetCoursesCategory1 { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }

    }
    public class CoursesCategoriesViewModel
    {

        public string CrsCats_Id { get; set; }
        public string CrsCats_Name { get; set; }
        public System.DateTime CrsCats_DateTime { get; set; }
        public string CrsCats_Publisher { get; set; }
        public bool CrsCats_Active { get; set; }

        public virtual AspNetCours AspNetCours { get; set; }

        public virtual ICollection<AspNetCours> AspNetCourses { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }

    }

    public  class CourseLessonViewModel
    {
        public string Lesson_Id { get; set; }
        public string Lesson_Name { get; set; }
        public System.TimeSpan Lesson_Duration { get; set; }
        public string Lesson_Typescript { get; set; }

        public Nullable<System.DateTime> Lesson_SessionTime { get; set; }
        public bool Lesson_Active { get; set; }
        public string Lesson_Instructor { get; set; }
        public string Lesson_Photo { get; set; }
        public string Crs_Id { get; set; }
        public System.DateTime Lesson_DateTime { get; set; }
        public string Lesson_Publisher { get; set; }

        public string CrsInst_Id { get; set; }
        public string CrsInst_Name { get; set; }
        public string CrsInst_Cat { get; set; }
        public System.DateTime CrsInst_DateTime { get; set; }

        public virtual AspNetCours AspNetCours { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }

    public class CourseInstructoriesViewModel {

     
            public string CrsInst_Id { get; set; }
            public string CrsInst_Name { get; set; }
            public string CrsInst_Cat { get; set; }
            public System.DateTime CrsInst_DateTime { get; set; }
            public string Crs_Id { get; set; }

            public virtual AspNetCours AspNetCours { get; set; }
   





    }


    public class CourseGroupsViewModel
    { 
        public string Group_Name { get; set; }
        public List<DateTime> Group_Times { get; set; }
        public List<string> Group_Instructors { get; set; }
        public string Crs_Id { get; set; }
        public bool Lesson_Active { get; set; }






    }

}