using AlumniTrackerSite.Models;
using System.Text.RegularExpressions;
using System.Web;

namespace AlumniTrackerSite.Data
{
    public static class Security
    {
        private static string BlackList =
            @",<>/\'{};:`&|^";
        private static string[] BlackListWords = { "DATABASE", "1:1", "1=1", "TABLE", "TRUNCATE", "SELECT", "UNION" };
        private static string NumberWhiteList = "0123456789";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="alumniUser"></param>
        /// <returns></returns>
        public static bool CheckInputs(ILogger log, AlumniUser alumniUser)
        {
            //Checks every input individually for bad data
            bool[] goodInput = new bool[12];
            goodInput[0] = NumericalInput(log, alumniUser.StudentId);
            goodInput[1] = GeneralInput(log, alumniUser.Name);
            goodInput[2] = GeneralInput(log, alumniUser.EmployerName);
            goodInput[3] = GeneralInput(log, alumniUser.FieldofEmployment);
            goodInput[4] = NumericalInput(log, alumniUser.YearGraduated);
            goodInput[5] = GeneralInput(log, alumniUser.Degree);
            goodInput[6] = GeneralInput(log, alumniUser.Notes);
            goodInput[7] = GeneralInput(log, alumniUser.Address);
            goodInput[8] = GeneralInput(log, alumniUser.City);
            goodInput[9] = GeneralInput(log, alumniUser.State);
            goodInput[10] = NumericalInput(log, alumniUser.Zip);
            goodInput[11] = GeneralInput(log, alumniUser.Phone); // is not a possible input, but just in case.

            // if any input is bad, return false
            if (goodInput.Contains(false)) return false;
            return true;
        }

        public static bool GeneralInput(ILogger log, string input)
        {
            // Null or Empty returns as good input
            if(input == null || input == "")
            { return true; }
            
            // Is Clearly Injection
            input = input.ToUpper();
            string[] words = input.Split(' ');
            foreach (string word in words)
            {
                if(BlackListWords.Contains(word))
                {
                    log.LogWarning("Possible Sql Injection '{input}' at {date}", input, DateTime.Now);
                    return false; 
                }
            }

            // checks for Coding Characters
            if (input.Contains(BlackList))
            {
                log.LogWarning("Bad Inputs '{input}' at {date}", input, DateTime.Now);
                return false; 
            }

            return true;
        }
        public static bool EmailInput(ILogger log, string input)
        {
            if (input == null || input == "") return true;

            if (GeneralInput(log, input))
            {
                //Regex that matches that the input has words@something.thing, and makes sure the end is between 2-4 characters
                Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
                Match match = regex.Match(input);
                if (!match.Success)
                { return false; }
                return true;
            }
            return false;
        } 
        public static bool NumericalInput(ILogger log, string input)
        {
            if (input == null || input == "") return true;
            if (GeneralInput(log, input))
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
