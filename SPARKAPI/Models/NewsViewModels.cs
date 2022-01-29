using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARKAPI.Models
{
    public class NewsViewModels
    {
    }

    public class NewsCategoriesViewModel
    {

        public string Cat_Id { get; set; }
        public string Cat_Name { get; set; }
        public System.DateTime Cat_DateTime { get; set; }
        public bool Cat_Active { get; set; }


    }

    public class NewsViewModel {

        public string News_Id { get; set; }
        public string News_Title { get; set; }
        public string News_Description { get; set; }
        public string News_Content { get; set; }
        public System.DateTime News_DateTime { get; set; }
        public string News_Photo { get; set; }
        public bool News_Active { get; set; }
        public string News_Publisher { get; set; }
        public string Cat_Id { get; set; }

     
        public virtual AspNetNewsCategory AspNetNewsCategory { get; set; }

    }
}