using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebAppMVC.Models
{
    public class WriteFile
    {
        public static void WriteSQL(string data)
        {
            string path =@"c:\SQLtrace.txt";
            File.AppendAllText(path, data);
        }
    }


}
