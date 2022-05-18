﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlumniTrackerSite.Models
{
    public partial class AlumniUser
    {
        [Key]
        [Required]
        //public int UserId { get; set; }
        public string StudentId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? EmployerName { get; set; }
        public string? FieldofEmployment { get; set; }
        public string? YearGraduated { get; set; }
        public string? Degree { get; set; }
        public string? Notes { get; set; }
        public string? AdminType { get; set; }
        public DateTime? DateModified { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Phone { get; set; }
        public bool IsAdmin { get; set; }
    }
}
