using CSharpCIA.CSharpCIA.Builders;
using CSharpCIA.CSharpCIA.Helpers;
using CSharpCIA.CSharpCIA.Nodes;
using Newtonsoft.Json;

namespace CSharpCIA.CSharpCIA.API
{
    internal class Extension
    {
        public static Node? FindNodeById(RootNode root,Guid id)
        {
            if (root is not null)
            {
                return root.childrens.Find(n => n.Id.Equals(id));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="root"></param>
        /// <param name="position">Type is string. This is position of Node found: FIRST, LAST</param>
        /// <returns></returns>
        public static Node? FindNodeById(RootNode root, Guid id, string position)
        {
            if (root is not null && root.childrens != null)
            {
                if (position.Equals("FIRST"))
                {
                    return root.childrens.Find(n => n.Id.Equals(id));
                }
                else
                {
                    return root.childrens.FindLast(n => n.Id.Equals(id));
                }
            }
            else
            {
                return null;
            }
        }

        public static List<Dependency> FindAllDependencyCallerById(List<Dependency> dependencies, Guid idCaller)
        {
            return dependencies.FindAll(d => d.Caller.Id.Equals(idCaller));
        }
        public static List<Dependency> FindAllDependencyCalleeById(List<Dependency> dependencies, Guid idCallee)
        {
            return dependencies.FindAll(d => d.Callee.Id.Equals(idCallee));
        }
        public static List<Dependency> FindAllDependencyById(List<Dependency> dependencies, Guid id)
        {
            List<Dependency> result = dependencies.FindAll(d => d.Callee.Id.Equals(id));
            result.AddRange(dependencies.FindAll(d => d.Caller.Id.Equals(id)));
            return result;
        }

        /// <summary>
        /// Export list Dependency to json file
        /// </summary>
        /// <param name="dependencies"></param>
        /// <returns></returns>
        public static bool ExportDependencyToJson(List<Dependency> dependencies, string filepath = "ExportDependencyToJson.json")
        {
            try
            {
                string json = "[";
                for (int i = 0; i < dependencies.Count; i++)
                {
                    var obj = new
                    {
                        dependencies[i].Type,
                        Caller = dependencies[i].Caller.ToString(),
                        Callee = dependencies[i].Callee.ToString()
                    };
                    json += JsonConvert.SerializeObject(obj);
                    if (i != dependencies.Count - 1)
                    {
                        json += ",";
                    }
                }
                json += "]";

                // Lưu chuỗi JSON vào file
                File.WriteAllText("C:\\Users\\dung3\\Desktop\\Temp\\ExportDependencyToJson.json", json);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Export list nodes of root to json file
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static bool ExportRootToJson(RootNode root, string filepath = "ExportRootToJson.json")
        {
            return FileHelper.ExportObjectToJson(root, "C:\\Users\\dung3\\Desktop\\Temp\\ExportRootToJson.json");
        }
        /// <summary>
        /// Export Change of ver1 and ver2 to json file
        /// </summary>
        /// <param name="nodeChanges">Dictionary<Node.BindingName, CHANGE_TYPE></param>
        /// <returns></returns>
        public static bool ExportChangeToJson(Dictionary<string, string> nodeChanges, string filepath = "ExportChangeToJson.json")
        {
            return FileHelper.ExportObjectToJson(nodeChanges, "C:\\Users\\dung3\\Desktop\\Temp\\ExportChangeToJson.json");
        }
        /// <summary>
        /// Export Impact of change from ver1 and ver2 to json file
        /// </summary>
        /// <param name="impacts">Dictionary<Node.BindingName, ulong> </param>
        /// <returns></returns>
        public static bool ExportImpactToJson(Dictionary<string, ulong> impacts, string filepath = "ExportImpactToJson.json")
        {
            return FileHelper.ExportObjectToJson(impacts, "C:\\Users\\dung3\\Desktop\\Temp\\ExportImpactToJson.json");
        }
    }
}
