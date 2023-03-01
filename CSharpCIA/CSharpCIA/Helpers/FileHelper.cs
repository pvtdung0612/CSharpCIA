using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.CSharpCIA.Helpers
{
    public class FileHelper
    {
        public static string GetContentFile(string path)
        {
            string fileContent = File.ReadAllText(path);
            return fileContent;
        }

        public static List<string> GetSourceFiles(string path)
        {
            List<string> files = new List<string>();

            if (Directory.Exists(path))
            {
                foreach (string itemDirectories in Directory.GetDirectories(path))
                {
                    //&& !itemDirectories.EndsWith(".git") && !itemDirectories.EndsWith(".vscode") && !itemDirectories.EndsWith(".vs")
                    //|| itemDirectories.EndsWith(".dll"
                    files.AddRange(GetSourceFiles(itemDirectories));
                }
                foreach (var itemFiles in Directory.GetFiles(path))
                {
                    if (itemFiles.EndsWith(".cs"))
                    {
                        files.AddRange(GetSourceFiles(itemFiles));
                    }
                }
            }
            else if (File.Exists(path))
            {
                if (path.EndsWith(".cs"))
                {
                    files.Add(path);
                }
            }

            return files;
        }

        public static List<string> GetAllDllFile(string path)
        {
            List<string> files = new List<string>();

            if (Directory.Exists(path))
            {
                foreach (string item in Directory.GetDirectories(path))
                {
                    files.AddRange(GetAllDllFile(item));
                }
                foreach (string item in Directory.GetFiles(path))
                {
                    if (item.EndsWith(".dll"))
                    {
                        files.Add(item);
                    }
                }
            }

            return files;
        }

        public static bool ExportObjectToJson(object obj, string filepath = "ExportObjectToJson.json")
        {
            try
            {
                string json = JsonConvert.SerializeObject(obj);

                // Lưu chuỗi JSON vào file
                File.WriteAllText(filepath, json);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}