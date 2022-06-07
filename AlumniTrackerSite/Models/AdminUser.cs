using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlumniTrackerSite.Models
{
    public partial class AdminUser
    {
        [Key]
        public int AdminId { get; set; }
        [MaxLength(50)]
        public string AdminType { get; set; } = null!;
        [PersonalData]
        [MaxLength(50)]
        public string AdminName { get; set; } = null!;
        public string? Id { get; set; }

        public virtual AspNetUsers? IdNavigation { get; set; }
    }
}
