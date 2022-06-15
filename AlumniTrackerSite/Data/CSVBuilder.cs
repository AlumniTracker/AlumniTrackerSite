using System.Text;
using System.IO;
using AlumniTrackerSite.Contexts;
using AlumniTrackerSite.Models;
using System.Collections.Generic;

namespace AlumniTrackerSite.Data
{
    public static class CSVBuilder
    {

        public static bool MakeCSV(TrackerContext _context, string filepath)
        {
            StringBuilder sb = new StringBuilder();
            List<Alumnis> Alums = _context.GetAlumnis();
            if(Alums == null) { return false; }
            sb.AppendLine(
            "AlumniId" + ", " +
            "StudentId" + ", " +
            "Id" + ", " +
            "Name" + ", " +
            "Degree" + ", " +
            "FieldofEmployment" + ", " +
            "YearGraduated" + ", " +
            "Email" + ", " +
            "Address" + ", " +
            "City" + ", " +
            "State" + ", " +
            "Zip" + ", " +
            "EmployerName" + ", " +
            "DateModified" + ", "
                );
            foreach (Alumnis alumni in Alums)
            {
                sb.AppendLine(
                    alumni.AlumniId + ", " +
                    alumni.StudentId + ", " +
                    alumni.Id + ", " +
                    alumni.Name+ ", " +
                    alumni.Degree + ", " +
                    alumni.FieldofEmployment + ", " +
                    alumni.YearGraduated + ", " +
                    alumni.Email + ", " +
                    alumni.Address + ", " +
                    alumni.City + ", " +
                    alumni.State + ", " +
                    alumni.Zip + ", "  +
                    alumni.EmployerName + ", "+
                    alumni.DateModified + ", "
                    );
            }
            File.WriteAllText(filepath, sb.ToString());
            return true;

        }
    }
}
