//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SPARKAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AspNetProgrameCoursesLesson
    {
        public string ProgrameCoursesLesson_Id { get; set; }
        public string ProgrameCoursesLesson_Name { get; set; }
        public int ProgrameCoursesLesson_Duration { get; set; }
        public string ProgrameCoursesLesson_Typescript { get; set; }
        public string ProgrameCoursesLesson_Vintro { get; set; }
        public string ProgrameCoursesLesson_VideoFull { get; set; }
        public System.DateTime ProgrameCoursesLesson_StartDate { get; set; }
        public Nullable<System.TimeSpan> ProgrameCoursesLesson_SessionTime { get; set; }
        public bool ProgrameCoursesLesson_Active { get; set; }
        public string ProgrameCoursesLesson_Instructor { get; set; }
        public string ProgrameCoursesLesson_Photo { get; set; }
        public string ProgrameCourses_Id { get; set; }
    }
}