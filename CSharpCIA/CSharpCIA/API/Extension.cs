using CSharpCIA.CSharpCIA.Builders;
using CSharpCIA.CSharpCIA.Helpers;
using CSharpCIA.CSharpCIA.Nodes;
using Newtonsoft.Json;
using System.IO;
using Gexf;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;


namespace CSharpCIA.CSharpCIA.API
{
    internal class Extension
    {
        public static Node? FindNodeById(List<Node> nodes,Guid id)
        {
            if (nodes is not null)
            {
                return nodes.Find(n => n.Id.Equals(id));
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
        public static Node? FindNodeById(List<Node> nodes, Guid id, string position)
        {
            if (nodes is not null)
            {
                if (position.Equals("FIRST"))
                {
                    return nodes.Find(n => n.Id.Equals(id));
                }
                else
                {
                    return nodes.FindLast(n => n.Id.Equals(id));
                }
            }
            else
            {
                return null;
            }
        }

        public static List<Dependency> FindAllDependencyCallerById(List<Dependency> dependencies, Guid idCaller)
        {
            return dependencies.FindAll(d => d.Caller.Equals(idCaller));
        }
        public static List<Dependency> FindAllDependencyCalleeById(List<Dependency> dependencies, Guid idCallee)
        {
            return dependencies.FindAll(d => d.Callee.Equals(idCallee));
        }
        public static List<Dependency> FindAllDependencyById(List<Dependency> dependencies, Guid id)
        {
            List<Dependency> result = dependencies.FindAll(d => d.Callee.Equals(id));
            result.AddRange(dependencies.FindAll(d => d.Caller.Equals(id)));
            return result;
        }

        /// <summary>
        /// Export list Dependency to json file
        /// </summary>
        /// <param name="dependencies"></param>
        /// <returns></returns>
        public static bool ExportDependencyToJson(List<Dependency> dependencies, string filepath = "ExportDependencyToJson.json")
        {
            return FileHelper.ExportObjectToJson(dependencies, filepath);
        }
        /// <summary>
        /// Export list nodes of root to json file
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static bool ExportNodesToJson(List<Node> nodes, string filepath = "ExportRootToJson.json")
        {
            return FileHelper.ExportObjectToJson(nodes, filepath);
        }
        /// <summary>
        /// Export Change of ver1 and ver2 to json file
        /// </summary>
        /// <param name="nodeChanges">Dictionary<Node.BindingName, CHANGE_TYPE></param>
        /// <returns></returns>
        public static bool ExportChangeToJson(Dictionary<string, string> nodeChanges, string filepath = "ExportChangeToJson.json")
        {
            return FileHelper.ExportObjectToJson(nodeChanges, filepath);
        }
        /// <summary>
        /// Export Impact of change from ver1 and ver2 to json file
        /// </summary>
        /// <param name="impacts">Dictionary<Node.BindingName, ulong> </param>
        /// <returns></returns>
        public static bool ExportImpactToJson(Dictionary<string, ulong> impacts, string filepath = "ExportImpactToJson.json")
        {
            return FileHelper.ExportObjectToJson(impacts, filepath);
        }

        public static List<Node>? ImportNodesFromJson(string filePath)
        {
            List<Node>? nodes = new List<Node>();

            try
            {
                if (filePath is not null && !String.IsNullOrEmpty(filePath) && filePath.EndsWith(".json"))
                {
                    using (StreamReader stream = new StreamReader(filePath))
                    {
                        string json = stream.ReadToEnd();
                        dynamic array = JsonConvert.DeserializeObject(json);
                        foreach (var item in array)
                        {
                            // parse node from json maybe null. solution is still add null to list nodes
                            switch (item.Type.Value)
                            {
                                case nameof(NODE_TYPE.ROOT):
                                    nodes.Add(((JObject)item).ToObject<RootNode>());
                                    //RootNode rootNode = new RootNode(
                                    //    item.SimpleName.Value, item.QualifiedName.Value, item.OriginName.Value, item.SourcePath.Value, null, null , item.Id.Value);
                                    //nodes.Add(rootNode);
                                    break;
                                case nameof(NODE_TYPE.NAMESPACE):
                                    nodes.Add(((JObject)item).ToObject<NamespaceNode>());
                                    //NamespaceNode namespaceNode = new NamespaceNode(
                                    //    item.SimpleName.Value, item.QualifiedName.Value, item.OriginName.Value, item.SourcePath.Value, null, null, ((JObject)item.Attributes).ToObject<List<string>>(), ((JObject)item.Modifiers).ToObject<List<string>>(), item.Id.Value);
                                    //nodes.Add(namespaceNode);
                                    break;
                                case nameof(NODE_TYPE.INTERFACE):
                                    nodes.Add(((JObject)item).ToObject<InterfaceNode>());
                                    //InterfaceNode interfaceNode = new InterfaceNode(
                                    //    item.SimpleName.Value, item.QualifiedName.Value, item.OriginName.Value, item.SourcePath.Value, null, null, item.Attributes.Value, item.Modifiers.Value, item.Bases.Value, item.Id.Value);
                                    //nodes.Add(interfaceNode);
                                    break;
                                case nameof(NODE_TYPE.CLASS):
                                    nodes.Add(((JObject)item).ToObject<ClassNode>());
                                    //ClassNode classNode = new ClassNode(
                                    //    item.SimpleName.Value, item.QualifiedName.Value, item.OriginName.Value, item.SourcePath.Value, null, null, item.Attributes.Value, item.Modifiers.Value, item.Bases.Value, item.Id.Value);
                                    //nodes.Add(classNode);
                                    break;
                                case nameof(NODE_TYPE.FIELD):
                                    nodes.Add(((JObject)item).ToObject<FieldNode>());
                                    //FieldNode fieldNode = new FieldNode(
                                    //    item.SimpleName.Value, item.QualifiedName.Value, item.OriginName.Value, item.SourcePath.Value, null, null, item.Attributes.Value, item.Modifiers.Value, item.VariableType.Value, item.variableValue.Value, item.Id.Value);
                                    //nodes.Add(fieldNode);
                                    break;
                                case nameof(NODE_TYPE.PROPERTY):
                                    nodes.Add(((JObject)item).ToObject<PropertyNode>());
                                    //PropertyNode propertyNode = new PropertyNode(
                                    //    item.SimpleName.Value, item.QualifiedName.Value, item.OriginName.Value, item.SourcePath.Value, null, null, item.Attributes.Value, item.Modifiers.Value, item.Id.Value);
                                    //nodes.Add(propertyNode);
                                    break;
                                case nameof(NODE_TYPE.METHOD):
                                    nodes.Add(((JObject)item).ToObject<MethodNode>());
                                    //MethodNode methodNode = new MethodNode(
                                    //    item.SimpleName.Value, item.QualifiedName.Value, item.OriginName.Value, item.SourcePath.Value, null, null, item.Attributes.Value, item.Modifiers.Value, item.Parameters.Value, item.Body.Value, item.ReturnType.Value, item.Id.Value);
                                    //nodes.Add(methodNode);
                                    break;
                                case nameof(NODE_TYPE.STRUCT):
                                    nodes.Add(((JObject)item).ToObject<StructNode>());
                                    //StructNode structNode = new StructNode(
                                    //    item.SimpleName.Value, item.QualifiedName.Value, item.OriginName.Value, item.SourcePath.Value, null, null, item.Attributes.Value, item.Modifiers.Value, item.Bases.Value, item.Id.Value);
                                    //nodes.Add(structNode);
                                    break;
                                case nameof(NODE_TYPE.ENUM):
                                    nodes.Add(((JObject)item).ToObject<EnumNode>());
                                    //EnumNode enumNode = new EnumNode(
                                    //    item.SimpleName.Value, item.QualifiedName.Value, item.OriginName.Value, item.SourcePath.Value, null, null, item.Attributes.Value, item.Modifiers.Value, item.Bases.Value, item.Id.Value);
                                    //nodes.Add(enumNode);
                                    break;
                                case nameof(NODE_TYPE.DELEGATE):
                                    nodes.Add(((JObject)item).ToObject<DelegateNode>());
                                    //DelegateNode delegateNode = new DelegateNode(
                                    //   item.SimpleName.Value, item.QualifiedName.Value, item.OriginName.Value, item.SourcePath.Value, null, null, item.Attributes.Value, item.Modifiers.Value, item.Id.Value);
                                    //nodes.Add(delegateNode);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return nodes;
        }

        public static List<Dependency>? ImportDependencyFromJson(string filePath)
        {
            List<Dependency>? dependencies = new List<Dependency>();

            try
            {
                if (filePath is not null && !String.IsNullOrEmpty(filePath) && filePath.EndsWith(".json"))
                {
                    using (StreamReader stream = new StreamReader(filePath))
                    {
                        string json = stream.ReadToEnd();
                        dynamic array = JsonConvert.DeserializeObject(json);
                        foreach (var item in array)
                        {
                            dependencies.Add(((JObject)item).ToObject<Dependency>());
                            //Dependency dependency = new Dependency()
                            //{
                            //    Type = item.Type.Value,
                            //    Caller = Guid.Parse(item.Caller.Value),
                            //    Callee = Guid.Parse(item.Callee.Value),
                            //};
                            //dependencies.Add(dependency);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return dependencies;
        }

        public static bool ExportJsonToGexf(string jsonFileName, string gexfFileName) {
            try {
                //// Định nghĩa các thuộc tính cho nút
                //GexfNode node = graph.AddNode("node1", "Node 1");
                //node.SetAttribute("size", "10");
                //node.SetAttribute("color", "#FF0000");

                //// Định nghĩa các thuộc tính cho cạnh
                //GexfEdge edge = graph.AddEdge("node1", "node2");
                //edge.SetAttribute("weight", "2");
                //edge.SetAttribute("color", "#0000FF");


                //// Load JSON file as a JObject
                //JObject jsonData = JObject.Parse(File.ReadAllText(jsonFileName));

                //// Create a new GEXF document
                //GexfGraph gexfGraph = new GexfGraph();
                //gexfGraph.Meta.LastModifiedDate = DateTime.Now;
                //gexfGraph.Meta.Creator = "Your Name";
                //gexfGraph.Meta.Description = "Your description";

                //// Add nodes to the GEXF document
                //foreach (JToken node in jsonData["nodes"])
                //{
                //    GexfNode gexfNode = gexfGraph.AddNode(node["id"].ToString());
                //    gexfNode.Label = node["label"].ToString();
                //}

                //// Add edges to the GEXF document
                //foreach (JToken edge in jsonData["edges"])
                //{
                //    GexfNode sourceNode = gexfGraph.GetNode(edge["source"].ToString());
                //    GexfNode targetNode = gexfGraph.GetNode(edge["target"].ToString());
                //    GexfEdge gexfEdge = sourceNode.ConnectTo(targetNode);
                //    gexfEdge.Id = edge["id"].ToString();
                //}

                //// Save the GEXF document to a file
                //using (FileStream stream = new FileStream(gexfFileName, FileMode.Create))
                //{
                //    gexfGraph.Save(stream);
                //}

            } catch (Exception e) {
                return false;
            }

            return true;
        }

    }
}
