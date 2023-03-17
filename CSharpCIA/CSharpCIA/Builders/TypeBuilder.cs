using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.CSharpCIA.Builders
{
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
    public enum CHANGE_TYPE
    {
        NONE,
        ADDED,
        REMOVED,
        MODIFIED,
    }
    public enum DEPENDENCY_TYPE
    {
        INVOKE,
        USE, // method,
        OWN, // root - namespace, namespace - class, class - field, property, method 
        INHERIT,
        IMPLEMENT,
        OVERRIDE,
        CALLBACK,
    }
    public struct IMPACT_WEIGHT
    {
        public const int DEPENDENCY_INVOKE = 1;
        public const int DEPENDENCY_USE = 1;
        public const int DEPENDENCY_OWN = 100;
        public const int DEPENDENCY_INHERIT = 1;
        public const int DEPENDENCY_IMPLEMENT = 1;
        public const int DEPENDENCY_OVERRIDE = 1;
        public const int DEPENDENCY_CALLBACK = 1;

        public const int CHANGE_ADDED = 1;
        public const int CHANGE_REMOVED = 1;
        public const int CHANGE_MODIFIED = 1;

        //public const int NODE_ROOT = 1;
        //public const int NODE_NAMESPACE = 1;
        //public const int NODE_INTERFACE = 1;
        //public const int NODE_CLASS = 1;
        //public const int NODE_FIELD = 1;
        //public const int NODE_PROPERTY = 1;
        //public const int NODE_METHOD = 1;
        //public const int NODE_STRUCT = 1;
        //public const int NODE_ENUM = 1;
        //public const int NODE_DELEGATE = 1;
    }
}