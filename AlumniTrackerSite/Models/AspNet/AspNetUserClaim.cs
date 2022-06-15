using AlumniTrackerSite.Models.AspNet;
using System;
using System.Collections.Generic;

namespace AlumniTrackerSite.Models
{
    public partial class AspNetUserClaim
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string? ClaimType { get; set; }
        public string? ClaimValue { get; set; }

        public virtual AspNetUsers User { get; set; } = null!;
    }
}
