using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AlumniTrackerSite.Areas.Identity;
using Microsoft.AspNetCore.Identity;

namespace AlumniTrackerSite.Models
{
    public partial class AlumniUser
    {
        [Key]
        [Required]
        [MaxLength(10)]
        public string StudentId { get; set; } = null!;
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [MaxLength(100)]
        public string? EmployerName { get; set; }
        [MaxLength(50)]
        public string? FieldofEmployment { get; set; }
        [MaxLength(4)]
        public string? YearGraduated { get; set; }
        [MaxLength(100)]
        public string? Degree { get; set; }
        [MaxLength(500)]
        public string? Notes { get; set; }
        [MaxLength(150)]
        public string? AdminType { get; set; }
        public DateTime? DateModified { get; set; }
        [MaxLength(100)]
        public string? Address { get; set; }
        [MaxLength(50)]
        public string? City { get; set; }
        [MaxLength(2)]
        public string? State { get; set; }
        [MaxLength(10)]
        public string? Zip { get; set; }
        [MaxLength(10)]
        public string? Phone { get; set; }
        public bool IsAdmin { get; set; }

        //public string? Id { get; set; }
        //public virtual IdentityUser? User { get; set; }


    }
}
