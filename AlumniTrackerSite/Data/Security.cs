using System.Text.RegularExpressions;
using System.Web;

namespace AlumniTrackerSite.Data
{
    public static class Security
    {
        private static string BlackList = 
            @"<>/\'{};:`&|";
        private static string[] BlackListWords = { "DATABASE", "1:1", "TABLE", "TRUNCATE", "", "SELECT" };
        private static string NumberWhiteList = "0123456789";


        public static bool GeneralInput(string input)
        {
            // Null or Empty
            if(input == null || input == "")
            { return false; }
            // Is Clearly Injection
            input = input.ToUpper();
            string[] words = input.Split(' ');
            foreach (string word in words)
            {
                if(BlackListWords.Contains(word))
                { return false; }
            }
            // Coding Characters
            input = (string)input.Distinct();
            if (input.Contains(BlackList))
            { return false; }

            return true;
        }
        public static bool EmailInput(string input)
        {
            if (GeneralInput(input))
            {
                //HttpUtility.HtmlEncode(input); // 
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
            if (GeneralInput(input))
            {

            }
            return false;
        }
        public static bool PhoneInput(string input)
        {
            if (GeneralInput(input))
            {

            }
            return false;
        }
        
    }
}
