using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.CSharpCIA.Nodes.Builders
{
    public enum DEPENDENCY_TYPE
    {
        INVOKE,
        USE,
        //MEMBER,
        //INHERIT,
        //OVERRIDE,
    }
    public enum NODE_TYPE
    {
        ROOT,
        NAMESPACE,
        CLASS,
        FIELD,
        PROPERTY,
        METHOD,
    }
}