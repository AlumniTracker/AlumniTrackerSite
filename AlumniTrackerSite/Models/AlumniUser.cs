﻿using Microsoft.AspNetCore.Identity;
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
        public string StudentId { get; set; } = null!;
        [PersonalData]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [PersonalData]
        [MaxLength(100)]
        public string? EmployerName { get; set; }
        [PersonalData]
        [MaxLength(50)]
        public string? FieldofEmployment { get; set; }
        [PersonalData]
        [MaxLength(4)]
        public string? YearGraduated { get; set; }
        [PersonalData]
        [MaxLength(100)]
        public string? Degree { get; set; }
        [PersonalData]
        [MaxLength(500)]
        public string? Notes { get; set; }
        public DateTime? DateModified { get; set; }
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
        [MaxLength(10)]
        public string? Zip { get; set; }
        [PersonalData]
        [MaxLength(10)]
        [Phone]
        public string? Phone { get; set; }
        public string? Id { get; set; }

        public virtual AspNetUsers? IdNavigation { get; set; }
    }
}
