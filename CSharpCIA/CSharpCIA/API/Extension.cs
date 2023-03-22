using CSharpCIA.CSharpCIA.Builders;
using CSharpCIA.CSharpCIA.Helpers;
using CSharpCIA.CSharpCIA.Nodes;
using Newtonsoft.Json;
using System.IO;
using Gexf;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpCIA.CSharpCIA.API
{
    internal class Extension
    {
        public static Node? FindNodeById(List<Node> nodes, string id)
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
                Console.WriteLine(e.Message);
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
                Console.WriteLine(e.Message);
            }

            return dependencies;
        }

        public static bool ExportJsonToGexf(string jsonNodeFilePath, string jsonEdgeFilePath, string gexfFilePath)
        {
            try
            {
                // Create a new GEXF document
                var gexf = new GexfDocument();
                gexf.Meta.LastModified = DateTimeOffset.Now;
                gexf.Meta.Creator = "DungUET[" + Environment.UserName + "]";
                gexf.Meta.Description = "CSharpCIA";

                gexf.Graph.IdType = GexfIdType.String;

                // Add node to gexf
                // Set type Node
                gexf.Graph.NodeAttributes.AddRange(
                    // Node
                    new GexfAttribute(0, "Type", GexfDataType.String),
                    new GexfAttribute(1, "Impact", GexfDataType.String),
                    new GexfAttribute(2, "SimpleName", GexfDataType.String),
                    new GexfAttribute(3, "QualifiedName", GexfDataType.String),
                    new GexfAttribute(4, "OriginName", GexfDataType.String),
                    new GexfAttribute(5, "BindingName", GexfDataType.String),
                    new GexfAttribute(6, "SourcePath", GexfDataType.String),

                    // All Node except Root
                    new GexfAttribute(7, "Attributes", GexfDataType.ListString),
                    new GexfAttribute(8, "Modifiers", GexfDataType.ListString),

                    // Interface, Class, Enum, Struct
                    new GexfAttribute(9, "Bases", GexfDataType.ListString),

                    // Method
                    new GexfAttribute(10, "Parameters", GexfDataType.ListString),
                    new GexfAttribute(11, "Body", GexfDataType.String),
                    new GexfAttribute(12, "ReturnType", GexfDataType.String),

                    // Namespace
                    new GexfAttribute(13, "AllOriginNames", GexfDataType.ListString),
                    new GexfAttribute(14, "AllSourcePaths", GexfDataType.ListString),

                    // Update new property
                    new GexfAttribute(15, "isConstuctor", GexfDataType.Boolean),
                    new GexfAttribute(16, "Syntax", GexfDataType.String)
                    );

                List<Node>? nodes = ImportNodesFromJson(jsonNodeFilePath);
                if (nodes is not null)
                {
                    foreach (var item in nodes)
                    {
                        // Set for comon node
                        GexfNode gexfNode = new GexfNode(item.Id, item.SimpleName)
                        {
                            AttrValues = {
                                new GexfAttributeValue(0, item.Type),
                                new GexfAttributeValue(1, item.Impact),
                                new GexfAttributeValue(2, item.SimpleName),
                                new GexfAttributeValue(3, item.QualifiedName),
                                new GexfAttributeValue(4, item.OriginName),
                                new GexfAttributeValue(5, item.BindingName),
                                new GexfAttributeValue(6, item.SourcePath),
                                new GexfAttributeValue(16, item.Syntax)
                            }
                        };

                        switch (item.Type)
                        {
                            case nameof(NODE_TYPE.CLASS):
                                ClassNode classNode = (ClassNode)item;
                                gexfNode.AttrValues.AddRange(
                                    new GexfAttributeValue(7, classNode.Attributes),
                                    new GexfAttributeValue(8, classNode.Modifiers),
                                    new GexfAttributeValue(9, classNode.Bases),

                                    // "<null value>"
                                    new GexfAttributeValue(10, "<null value>"),
                                    new GexfAttributeValue(11, "<null value>"),
                                    new GexfAttributeValue(12, "<null value>"),
                                    new GexfAttributeValue(13, "<null value>"),
                                    new GexfAttributeValue(14, "<null value>")
                                    );
                                break;
                            case nameof(NODE_TYPE.DELEGATE):
                                DelegateNode delegateNode = (DelegateNode)item;
                                gexfNode.AttrValues.AddRange(
                                    new GexfAttributeValue(7, delegateNode.Attributes),
                                    new GexfAttributeValue(8, delegateNode.Modifiers),

                                    // "<null value>"
                                    new GexfAttributeValue(9, "<null value>"),
                                    new GexfAttributeValue(10, "<null value>"),
                                    new GexfAttributeValue(11, "<null value>"),
                                    new GexfAttributeValue(12, "<null value>"),
                                    new GexfAttributeValue(13, "<null value>"),
                                    new GexfAttributeValue(14, "<null value>")
                                    );
                                break;
                            case nameof(NODE_TYPE.ENUM):
                                EnumNode enumNode = (EnumNode)item;
                                gexfNode.AttrValues.AddRange(
                                    new GexfAttributeValue(7, enumNode.Attributes),
                                    new GexfAttributeValue(8, enumNode.Modifiers),
                                    new GexfAttributeValue(9, enumNode.Bases),

                                    // "<null value>"
                                    new GexfAttributeValue(10, "<null value>"),
                                    new GexfAttributeValue(11, "<null value>"),
                                    new GexfAttributeValue(12, "<null value>"),
                                    new GexfAttributeValue(13, "<null value>"),
                                    new GexfAttributeValue(14, "<null value>")
                                    );
                                break;
                            case nameof(NODE_TYPE.INTERFACE):
                                InterfaceNode interfaceNode = (InterfaceNode)item;
                                gexfNode.AttrValues.AddRange(
                                    new GexfAttributeValue(7, interfaceNode.Attributes),
                                    new GexfAttributeValue(8, interfaceNode.Modifiers),
                                    new GexfAttributeValue(9, interfaceNode.Bases),

                                    // "<null value>"
                                    new GexfAttributeValue(10, "<null value>"),
                                    new GexfAttributeValue(11, "<null value>"),
                                    new GexfAttributeValue(12, "<null value>"),
                                    new GexfAttributeValue(13, "<null value>"),
                                    new GexfAttributeValue(14, "<null value>")
                                    );
                                break;
                            case nameof(NODE_TYPE.METHOD):
                                MethodNode methodNode = (MethodNode)item;
                                gexfNode.AttrValues.AddRange(
                                    new GexfAttributeValue(7, methodNode.Attributes),
                                    new GexfAttributeValue(8, methodNode.Modifiers),
                                    new GexfAttributeValue(10, methodNode.Parameters),
                                    new GexfAttributeValue(11, methodNode.Body),
                                    new GexfAttributeValue(12, methodNode.ReturnType),
                                    new GexfAttributeValue(15, methodNode.IsContructor),

                                    // "<null value>"
                                    new GexfAttributeValue(9, "<null value>"),
                                    new GexfAttributeValue(13, "<null value>"),
                                    new GexfAttributeValue(14, "<null value>")

                                    );
                                break;
                            case nameof(NODE_TYPE.NAMESPACE):
                                NamespaceNode namespaceNode = (NamespaceNode)item;
                                gexfNode.AttrValues.AddRange(
                                    new GexfAttributeValue(13, namespaceNode.AllOriginNames),
                                    new GexfAttributeValue(14, namespaceNode.AllSourcePaths),
                                    new GexfAttributeValue(7, namespaceNode.Attributes),
                                    new GexfAttributeValue(8, namespaceNode.Modifiers),

                                    // "<null value>"
                                    new GexfAttributeValue(9, "<null value>"),
                                    new GexfAttributeValue(10, "<null value>"),
                                    new GexfAttributeValue(11, "<null value>"),
                                    new GexfAttributeValue(12, "<null value>")
                                    );
                                break;
                            case nameof(NODE_TYPE.ROOT):
                                RootNode rootNode = (RootNode)item;
                                gexfNode.AttrValues.AddRange(
                                // "<null value>"
                                new GexfAttributeValue(9, "<null value>"),
                                    new GexfAttributeValue(7, "<null value>"),
                                    new GexfAttributeValue(8, "<null value>"),
                                    new GexfAttributeValue(9, "<null value>"),
                                    new GexfAttributeValue(10, "<null value>"),
                                    new GexfAttributeValue(11, "<null value>"),
                                    new GexfAttributeValue(12, "<null value>"),
                                    new GexfAttributeValue(13, "<null value>"),
                                    new GexfAttributeValue(14, "<null value>")
                                    );
                                break;
                            case nameof(NODE_TYPE.STRUCT):
                                StructNode structNode = (StructNode)item;
                                gexfNode.AttrValues.AddRange(
                                    new GexfAttributeValue(7, structNode.Attributes),
                                    new GexfAttributeValue(8, structNode.Modifiers),
                                    new GexfAttributeValue(9, structNode.Bases),

                                    // "<null value>"
                                    new GexfAttributeValue(10, "<null value>"),
                                    new GexfAttributeValue(11, "<null value>"),
                                    new GexfAttributeValue(12, "<null value>"),
                                    new GexfAttributeValue(13, "<null value>"),
                                    new GexfAttributeValue(14, "<null value>"),
                                    new GexfAttributeValue(15, "<null value>"),
                                    new GexfAttributeValue(16, "<null value>")
                                    );
                                break;
                            default:
                                break;
                        }

                        gexf.Graph.Nodes.Add(gexfNode);
                    }
                }

                // Add edge to gexf
                // Set type Edge
                gexf.Graph.EdgeAttributes.AddRange(
                    new GexfAttribute(0, "Type", GexfDataType.String)
                    );

                List<Dependency>? dependencies = ImportDependencyFromJson(jsonEdgeFilePath);
                if (dependencies is not null)
                {
                    for (int i = 0; i < dependencies.Count; i++)
                    {
                        GexfEdge gexfEdge = new GexfEdge(i, dependencies[i].Caller, dependencies[i].Callee)
                        {
                            AttrValues = {
                                new GexfAttributeValue(0, dependencies[i].Type),
                            }
                        };
                        gexf.Graph.Edges.Add(gexfEdge);
                    }
                }

                gexf.Save(gexfFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

    }
}
