using CSharpCIA.CSharpCIA.Nodes;
using CSharpCIA.CSharpCIA.Nodes.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSharpCIA.CSharpCIA.API
{
    internal class JsonTree
    {
        public static bool ExportTreeToJson(RootNode rootNode, string fileOutput) {
            // Chuyển danh sách các đối tượng thành chuỗi JSON
            var json = JsonConvert.SerializeObject(rootNode);

            // Ghi chuỗi JSON vào tệp văn bản
            File.WriteAllText("C:\\Users\\dung3\\Desktop\\Temp\\ExportToJson.json", json);
            return true;
        }   
    }
}
