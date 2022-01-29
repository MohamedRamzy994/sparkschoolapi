using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPARKAPI.Models
{
    public class EmployeesViewModels
    {
    }
 
    public class EmployeeViewModel
    {

        public string Employee_Id { get; set; }
        public string Employee_Name { get; set; }
        public string Employee_Phone { get; set; }
        public string Employee_Photo { get; set; }
        public decimal Employee_Salary { get; set; }
        public bool Employee_Active { get; set; }
        public string Employee_Publisher { get; set; }
        public System.DateTime Employee_DateTime { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

    }
}