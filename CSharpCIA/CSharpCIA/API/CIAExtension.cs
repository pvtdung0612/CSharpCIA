using CSharpCIA.CSharpCIA.Nodes;
using CSharpCIA.CSharpCIA.Nodes.Builders;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.CSharpCIA.API
{
    internal class CIAExtension
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

        public static void ExportDependencyToJson(List<Dependency> dependencies)
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

        public static void ExportRootToJson(RootNode root)
        {
            string json = JsonConvert.SerializeObject(root);

            // Lưu chuỗi JSON vào file
            File.WriteAllText("C:\\Users\\dung3\\Desktop\\Temp\\ExportRootToJson.json", json);
        }
    }
}
