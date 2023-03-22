using CSharpCIA.CSharpCIA.Builders;
using CSharpCIA.CSharpCIA.Nodes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Linq;

namespace CSharpCIA.CSharpCIA.API
{
    internal class AnalyzerImpact
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listNodeVer1"></param>
        /// <param name="listNodeVer2"></param>
        /// <returns>Dictionary<Node.BindingName, CHANGE_TYPE.ToString()></returns>
        public static Dictionary<string, string> AnalyzerChange(List<Node> listNodeVer1, List<Node> listNodeVer2)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // 3864: otimize performance increase - memory decrease
            // Convert listNode1 to hashSet1 for increase program performance
            Dictionary<string, Node> diction1 = new Dictionary<string, Node>(); // <BindingName, Node>
            Dictionary<string, Node> diction2 = new Dictionary<string, Node>(); // <BindingName, Node>
            // O(n)
            foreach (Node node in listNodeVer1)
                diction1.Add(node.BindingName, node);
            // O(m)
            foreach (Node node in listNodeVer2)
                diction2.Add(node.BindingName, node);

            // O(n)
            // Find node: CHANGE_TYPE is NONE, MODIFIED, REMOVE
            foreach (var item1 in listNodeVer1)
            {
                string key = item1.BindingName; // this is key for diction
                if (diction2.ContainsKey(item1.BindingName))
                {
                    // version 2 is have same Node in version 1:
                    // node is modified or node wasn't modified

                    // 3864: Obsolete
                    //var visitor = new RemoveCommentsVisitor();
                    //var declaration1 = visitor.Visit(item1.SyntaxNode);
                    //var declaration2 = visitor.Visit(diction2[key].SyntaxNode);

                    //if (declaration1.IsEquivalentTo(declaration2))
                    //    result.Add(item1.BindingName, CHANGE_TYPE.NONE.ToString());
                    //else
                    //    result.Add(item1.BindingName, CHANGE_TYPE.MODIFIED.ToString());

                    if (item1.isIdentical(diction2[key]))
                        result.Add(item1.BindingName, CHANGE_TYPE.NONE.ToString());
                    else
                        result.Add(item1.BindingName, CHANGE_TYPE.MODIFIED.ToString());

                    diction1.Remove(key);
                    diction2.Remove(key);
                }
                else
                {
                    result.Add(item1.BindingName, CHANGE_TYPE.REMOVED.ToString());
                    diction1.Remove(key);
                }
            }

            // < O(m)
            // Find node: CHANGE_TYPE is ADD
            foreach (var item2 in diction2)
            {
                result.Add(item2.Value.BindingName, CHANGE_TYPE.REMOVED.ToString());
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeChanges">Dictionary<Node.BindingName, CHANGE_TYPE></param>
        /// <param name="dependencies">List<Dependency> of nodeChanges in a version (may be version 1 or version 2)</param>
        /// <returns>Dictionary<Node.BindingName, Impact score> is result of ChangeImpactAnalysis</returns>
        public static Dictionary<string, ulong> ChangeImpactAnalysis(List<Node> nodes, Dictionary<string, string> nodeChanges, List<Dependency> dependencies)
        {
            // Dictionary<Node.BindingName, Impact score>
            Dictionary<string, ulong> result = new Dictionary<string, ulong>();

            // Convert list to dictionary to increase performance
            Dictionary<string, Node> dicNodes = nodes.ToDictionary(keySelector: n => n.Id, elementSelector: n => n);
            
            // những thằng node nào vừa có sự thay đổi vừa có sự phụ thuộc từ thằng khác gọi đến thì tính là có ảnh hưởng
            foreach (var dependency in dependencies)
            {
                // Những đứa node nào gọi đến node hiện tại thì bị ảnh hưởng
                if (dicNodes.ContainsKey(dependency.Callee) && nodeChanges.ContainsKey(dicNodes[dependency.Callee].BindingName) 
                    && !nodeChanges[dicNodes[dependency.Callee].BindingName].Equals(CHANGE_TYPE.NONE.ToString()))
                {
                    ulong scoreImpactCaller = 0;
                    ulong scoreImpactCallee = 0;

                    // Set điểm số ảnh hưởng cho 2 thằng có quan hệ với nhau dựa vào kiểu phụ thuộc của 2 đứa
                    if (dependency.Type.Equals(DEPENDENCY_TYPE.INVOKE.ToString()))
                    {
                        scoreImpactCaller += IMPACT_WEIGHT.DEPENDENCY_INVOKE;
                        scoreImpactCallee += IMPACT_WEIGHT.DEPENDENCY_INVOKE;
                    }
                    if (dependency.Type.Equals(DEPENDENCY_TYPE.USE.ToString()))
                    {
                        scoreImpactCaller += IMPACT_WEIGHT.DEPENDENCY_USE;
                        scoreImpactCallee += IMPACT_WEIGHT.DEPENDENCY_USE;
                    }
                    if (dependency.Type.Equals(DEPENDENCY_TYPE.CONTAIN.ToString()))
                    {
                        scoreImpactCaller += IMPACT_WEIGHT.DEPENDENCY_OWN;
                        scoreImpactCallee += IMPACT_WEIGHT.DEPENDENCY_OWN;
                    }
                    if (dependency.Type.Equals(DEPENDENCY_TYPE.INHERIT.ToString()))
                    {
                        scoreImpactCaller += IMPACT_WEIGHT.DEPENDENCY_INHERIT;
                        scoreImpactCallee += IMPACT_WEIGHT.DEPENDENCY_INHERIT;
                    }
                    if (dependency.Type.Equals(DEPENDENCY_TYPE.IMPLEMENT.ToString()))
                    {
                        scoreImpactCaller += IMPACT_WEIGHT.DEPENDENCY_IMPLEMENT;
                        scoreImpactCallee += IMPACT_WEIGHT.DEPENDENCY_IMPLEMENT;
                    }
                    if (dependency.Type.Equals(DEPENDENCY_TYPE.OVERRIDE.ToString()))
                    {
                        scoreImpactCaller += IMPACT_WEIGHT.DEPENDENCY_OVERRIDE;
                        scoreImpactCallee += IMPACT_WEIGHT.DEPENDENCY_OVERRIDE;
                    }
                    if (dependency.Type.Equals(DEPENDENCY_TYPE.CALLBACK.ToString()))
                    {
                        scoreImpactCaller += IMPACT_WEIGHT.DEPENDENCY_CALLBACK;
                        scoreImpactCallee += IMPACT_WEIGHT.DEPENDENCY_CALLBACK;
                    }

                    // Set điểm số ảnh hưởng cho 2 thằng dựa vào kiểu dữ liệu của 2 thằng
                    if (nodeChanges[dicNodes[dependency.Callee].BindingName].Equals(CHANGE_TYPE.ADDED.ToString()))
                    {
                        scoreImpactCaller += IMPACT_WEIGHT.CHANGE_ADDED;
                        scoreImpactCallee += IMPACT_WEIGHT.CHANGE_ADDED;
                    }
                    if (nodeChanges[dicNodes[dependency.Callee].BindingName].Equals(CHANGE_TYPE.REMOVED.ToString()))
                    {
                        scoreImpactCaller += IMPACT_WEIGHT.CHANGE_REMOVED;
                        scoreImpactCallee += IMPACT_WEIGHT.CHANGE_REMOVED;
                    }
                    if (nodeChanges[dicNodes[dependency.Callee].BindingName].Equals(CHANGE_TYPE.MODIFIED.ToString()))
                    {
                        scoreImpactCaller += IMPACT_WEIGHT.CHANGE_MODIFIED;
                        scoreImpactCallee += IMPACT_WEIGHT.CHANGE_MODIFIED;
                    }

                    if (result.ContainsKey(dicNodes[dependency.Caller].BindingName))
                    {
                        result[dicNodes[dependency.Caller].BindingName] += scoreImpactCaller;
                    }
                    else
                    {
                        result.Add(dicNodes[dependency.Caller].BindingName, scoreImpactCaller);
                    }
                    if (result.ContainsKey(dicNodes[dependency.Callee].BindingName))
                    {
                        result[dicNodes[dependency.Callee].BindingName] += scoreImpactCallee;
                    }
                    else
                    {
                        result.Add(dicNodes[dependency.Callee].BindingName, scoreImpactCallee);
                    }
                }
            }

            // Set property impact for node in list nodes
            foreach (var node in nodes)
            {
                if (result.ContainsKey(node.BindingName))
                {
                    node.Impact = result[node.BindingName];
                }
            }

             return result;
        }
    }


    class RemoveCommentsVisitor : CSharpSyntaxRewriter
    {
        public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia)
        {
            // Loại bỏ các comment trên và trong một câu lệnh
            if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) ||
                trivia.IsKind(SyntaxKind.MultiLineCommentTrivia))
            {
                return default(SyntaxTrivia);
            }

            // Giữ lại các trivia khác
            return base.VisitTrivia(trivia);
        }
    }

}
