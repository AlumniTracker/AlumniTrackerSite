using AlumniTrackerSite.Models.AspNet;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlumniTrackerSite.Models
{
    public partial class AlumniUser
    {
        [Key]
        public int AlumniId { get; set; } // This is not their student ID

        [PersonalData]
        [MaxLength(10)]
        [Display(Name ="Student ID")]
        public string StudentId { get; set; } = null!;
        [PersonalData]
        [MaxLength(50)]
        [Display(Name ="Name")]
        public string Name { get; set; } = null!;
        [PersonalData]
        [MaxLength(100)]
        [Display(Name="Employer")]
        public string? EmployerName { get; set; }
        [PersonalData]
        [MaxLength(50)]
        [Display(Name ="Field of Employment")]
        public string? FieldofEmployment { get; set; }
        [PersonalData]
        [MaxLength(4)]
        [Display(Name ="Year Graduated")]
        public string? YearGraduated { get; set; }
        [PersonalData]
        [MaxLength(100)]
        public string? Degree { get; set; }
        [PersonalData]
        [MaxLength(500)]
        public string? Notes { get; set; }
        public DateTime? DateModified { get;  set; }
        [PersonalData]
        [MaxLength(100)]
        public string? Address { get; set; }
        [PersonalData]
        [MaxLength(50)]
        public string? City { get; set; }
        [PersonalData]
        [MaxLength(2)]
        public string? State { get; set; }
        [PersonalData]
        [DataType(DataType.PostalCode)]
        [MaxLength(10)]
        public string? Zip { get; set; }
        [PersonalData]
        [Phone]
        [MaxLength(15)]
        public string? Phone { get; set; }
        /// <summary>
        /// Foreign Key to AspNetUser account
        /// </summary>
        public string? Id { get; set; }

        public virtual AspNetUsers? IdNavigation { get; set; }

    }
}
