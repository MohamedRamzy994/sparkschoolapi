using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARKAPI.Models
{
    public class JobsViewModels
    {
    }

    public class JobsCategoriesViewModel
    {

        public string Cat_Id { get; set; }
        public string Cat_Name { get; set; }
        public System.DateTime Cat_DateTime { get; set; }
        public bool Cat_Active { get; set; }


    }

    public class JobsViewModel
    {

        public string Job_Id { get; set; }
        public string Job_Title { get; set; }
        public string Job_Description { get; set; }
        public System.DateTime Job_DateTime { get; set; }
        public string Job_Photo { get; set; }
        public string Job_City { get; set; }
        public string Job_Country { get; set; }
        public string Cat_Id { get; set; }
        public bool Job_Active { get; set; }
        public string Job_Publisher { get; set; }

        public virtual AspNetJobsCategory AspNetJobsCategory { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }

    }
}