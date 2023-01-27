using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.Test.Helpers
{
    public class FileHelper
    {
        public string GetContent(string path)
        {
            string fileContent = File.ReadAllText(path);
            return fileContent;
        }
    }
}
