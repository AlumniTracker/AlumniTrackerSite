using System;
using System.Collections.Generic;

namespace AlumniTrackerSite.Models
{
    public partial class AdminUser
    {
        public int AdminId { get; set; }
        public string AdminType { get; set; } = null!;
        public string AdminName { get; set; } = null!;
        public string? Id { get; set; }

        public virtual AspNetUser? IdNavigation { get; set; }
    }
}
