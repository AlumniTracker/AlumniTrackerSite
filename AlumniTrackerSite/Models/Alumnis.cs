

using System.ComponentModel.DataAnnotations;

namespace AlumniTrackerSite.Models
{
    public class Alumnis
    {
        public int AlumniId { get; set; } // This is not their student ID
    
        [MaxLength(10)]
        [Display(Name = "Student ID")]
        public string StudentId { get; set; } = null!;
         
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; } = null!;
         
        [MaxLength(100)]
        [Display(Name = "Employer")]
        public string? EmployerName { get; set; }
         
        [MaxLength(50)]
        [Display(Name = "Field of Employment")]
        public string? FieldofEmployment { get; set; }
         
        [MaxLength(4)]
        [Display(Name = "Year Graduated")]
        public string? YearGraduated { get; set; }
         
        [MaxLength(100)]
        public string? Degree { get; set; }
         
        [MaxLength(500)]
        public string? Notes { get; set; }
        public DateTime? DateModified { get; set; }
         
        [MaxLength(100)]
        public string? Address { get; set; }
         
        [MaxLength(50)]
        public string? City { get; set; }
         
        [MaxLength(2)]
        public string? State { get; set; }
         
        [DataType(DataType.PostalCode)]
        [MaxLength(10)]
        public string? Zip { get; set; }
         
        public string? Email { get; set; }
        public string? Id { get; set; }

    }
}
