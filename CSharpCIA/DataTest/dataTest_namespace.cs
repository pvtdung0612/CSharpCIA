using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.DataTest
{
    internal class dataTest_namespace
    {
    }
}

namespace Test
{
    using Test.test1;
    public class test
    {
        public static string name = "dung";
    }
    namespace Test.test1
    {
        public class test1
        {
            public static void info()
            {
                Console.WriteLine(test.name);
            }
        }
    }

    public class test2
    {
        public static void info()
        {
            test1.info();
        }
    }
}


namespace Test
{
    using Test.test1;
    public class test3
    {
        public static void info()
        {
            test1.info();
        }

    }
}