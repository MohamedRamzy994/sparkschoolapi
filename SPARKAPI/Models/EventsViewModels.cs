using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARKAPI.Models
{
    public class EventsViewModels
    {
    }

    public class EventsCategoriesViewModel
    {

        public string Cat_Id { get; set; }
        public string Cat_Name { get; set; }
        public System.DateTime Cat_DateTime { get; set; }
        public bool Cat_Active { get; set; }


    }

    public class EventsViewModel {
 
        public string Event_Id { get; set; }
        public string Event_Title { get; set; }
        public string Event_Photo { get; set; }
        public string Cat_Id { get; set; }
        public System.DateTime Event_DateTime { get; set; }
    }
}