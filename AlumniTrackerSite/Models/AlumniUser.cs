using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace AlumniTrackerSite.Models
{
    public partial class AlumniUser
    {
        
        public string StudentId { get; set; } = null!;
        
        public string Name { get; set; } = null!;
        public string? EmployerName { get; set; }
        public string? FieldofEmployment { get; set; }
        public string? YearGraduated { get; set; }
        public string? Degree { get; set; }
        public string? Notes { get; set; }
        public DateTime? DateModified { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Phone { get; set; }
        public int AlumniId { get; set; }
        public string? Id { get; set; }

        public virtual AspNetUsers? IdNavigation { get; set; }
    }
}
