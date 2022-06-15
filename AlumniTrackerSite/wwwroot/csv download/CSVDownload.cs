using System.Text;
using System.IO;

namespace AlumniTrackerSite.wwwroot.csv_download
{
    public class CSVDownload
    {
        

        public void MakeCSV(string filepath) 
        { 

            StringBuilder sb = new StringBuilder();

            // Add Information To A Thingy
            csvfile.AppendLine("Killer,App,Death,SlayQueen");
            csvfile.AppendLine("1,2,3,4");

            File.AppendAllText(filepath, sb.ToString());

        }
    }
}
