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
    
    public partial class AspNetCoursesCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetCoursesCategory()
        {
            this.AspNetCourses = new HashSet<AspNetCours>();
            this.AspNetProgrameCourses = new HashSet<AspNetProgrameCours>();
        }
    
        public string CrsCats_Id { get; set; }
        public string CrsCats_Name { get; set; }
        public System.DateTime CrsCats_DateTime { get; set; }
        public string CrsCats_Publisher { get; set; }
        public bool CrsCats_Active { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetCours> AspNetCourses { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetProgrameCours> AspNetProgrameCourses { get; set; }
    }
}
