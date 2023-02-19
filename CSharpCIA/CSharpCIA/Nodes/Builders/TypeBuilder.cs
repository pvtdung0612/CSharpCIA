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
        OWN,
        INHERIT,
        IMPLEMENT,
        OVERRIDE,
        CALLBACK,
    }
    public enum NODE_TYPE
    {
        ROOT,
        NAMESPACE,
        INTERFACE,
        CLASS,
        FIELD,
        PROPERTY,
        METHOD,
        STRUCT,
        ENUM,
        DELEGATE,
    }
}