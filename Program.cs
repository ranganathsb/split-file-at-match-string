using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitFileAtMatchingString
{
    class Program
    {
        static void Main(string[] args)
        {
            var allowedFileExtensions = new List<string>() { ".txt", ".csv", ".sql" };
            var filepath = ConfigurationManager.AppSettings["FilePath"];
            var filename = string.Empty;
            var fileext = string.Empty;
            var directory = string.Empty;
            if (File.Exists(filepath))
            {
                var finfo = new FileInfo(filepath);
                filename = finfo.Name;
                fileext = finfo.Extension;
                directory = finfo.DirectoryName;
                if (!allowedFileExtensions.Contains(fileext.ToLower()))
                {
                    Console.WriteLine("FileType Not Supported! Only files of 'csv', 'txt' and 'sql' are supported.");
                    return;
                }                
            }          
            
            var matchString = ConfigurationManager.AppSettings["MatchString"];
            string line;
            int matches = 0;
            bool newFileFlag = true;
            string templine = string.Empty;
            StringBuilder sb = new StringBuilder();
            System.IO.StreamReader file = new System.IO.StreamReader(filepath);
            while ((line = file.ReadLine()) != null)
            {
                if (newFileFlag)
                {
                    if (sb != null)
                    {
                        sb.Clear();
                    }
                    newFileFlag = false;
                    if(!string.IsNullOrEmpty(templine))
                    sb.AppendLine(templine);
                }
                if (line.StartsWith(matchString))
                {
                    templine = line;
                    var partfilename = string.Format("{0}_part_{1}{2}", filename, matches, fileext);
                    System.IO.File.WriteAllText(string.Format("{0}\\{1}", directory, partfilename), sb.ToString());
                    matches++;
                    newFileFlag = true;
                }
                sb.AppendLine(line);
                System.Console.WriteLine(line);
                
            }

            file.Close();
            
            // Suspend the screen.  
            System.Console.ReadLine();
        }
    }
}
