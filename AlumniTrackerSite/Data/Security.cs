using AlumniTrackerSite.Models;
using System.Text.RegularExpressions;
using System.Web;

namespace AlumniTrackerSite.Data
{
    public static class Security
    {
        private static string BlackList =
            @"<>/\'{};:`&|^";
        private static string[] BlackListWords = { "DATABASE", "1:1", "1=1", "TABLE", "TRUNCATE", "", "SELECT", "UNION" };
        private static string NumberWhiteList = "0123456789";

        
        public static bool CheckInputs(AlumniUser alumniUser)
        {
            bool[] goodInput = new bool[12];
            goodInput[0] = NumericalInput(alumniUser.StudentId);
            goodInput[1] = GeneralInput(alumniUser.Name);
            goodInput[2] = GeneralInput(alumniUser.EmployerName);
            goodInput[3] = GeneralInput(alumniUser.FieldofEmployment);
            goodInput[4] = NumericalInput(alumniUser.YearGraduated);
            goodInput[5] = GeneralInput(alumniUser.Degree);
            goodInput[6] = GeneralInput(alumniUser.Notes);
            goodInput[7] = GeneralInput(alumniUser.Address);
            goodInput[8] = GeneralInput(alumniUser.City);
            goodInput[9] = GeneralInput(alumniUser.State);
            goodInput[10] = NumericalInput(alumniUser.Zip);
            goodInput[11] = GeneralInput(alumniUser.Phone);
            //PhoneInput(alumniUser.PhoneNumber); //ASP NET Identity Phone Number

            if (goodInput.Contains(false)) return false;

            return true;
        }

        public static bool GeneralInput(string input)
        {
            // Null or Empty
            if(input == null || input == "")
            { return true; } // changed from false
            // Is Clearly Injection
            input = input.ToUpper();
            string[] words = input.Split(' ');
            foreach (string word in words)
            {
                if(BlackListWords.Contains(word))
                { return false; }
            }
            // Coding Characters
            //input = (string)input.Distinct();
            if (input.Contains(BlackList))
            { return false; }

            return true;
        }
        public static bool EmailInput(string input)
        {
            if (input == null || input == "") return true;

            if (GeneralInput(input))
            {
                //HttpUtility.HtmlEncode(input); // This is a note to be used later

                Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
                Match match = regex.Match(input);
                if (!match.Success)
                { return false; }
                return true;
            }
            return false;
        } 
        public static bool NumericalInput(string input)
        {
            if (input == null || input == "") return true;
            if (GeneralInput(input))
            {
                foreach (char character in input)
                {
                    if (!NumberWhiteList.Contains(character))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        
    }
}
