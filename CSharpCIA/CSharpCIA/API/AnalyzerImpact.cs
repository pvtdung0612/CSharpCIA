using CSharpCIA.CSharpCIA.Builders;
using CSharpCIA.CSharpCIA.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.CSharpCIA.API
{
    internal class AnalyzerImpact
    {
        public static Dictionary<string, string> AnalyzerChange(List<Node> listNodeVer1, List<Node> listNodeVer2)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Convert listNode1 to hashSet1 for increase program performance
            Dictionary<string, Node> diction1 = new Dictionary<string, Node>();
            Dictionary<string, Node> diction2 = new Dictionary<string, Node>();
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

                    if (item1.SyntaxNode.IsEquivalentTo(diction2[key].SyntaxNode))
                        result.Add(item1.BindingName, CHANGE_TYPE.NONE.ToString());
                    else
                        result.Add(item1.BindingName, CHANGE_TYPE.MODIFIED.ToString());

                    diction1.Remove(key);
                    diction2.Remove(key);
                }
                else
                {
                    result.Add(item1.BindingName, CHANGE_TYPE.REMOVE.ToString());
                    diction1.Remove(key);
                }
            }

            // < O(m)
            // Find node: CHANGE_TYPE is ADD
            foreach (var item2 in diction2)
            {
                result.Add(item2.Value.BindingName, CHANGE_TYPE.REMOVE.ToString());
            }

            return result;
        }
    }
}
