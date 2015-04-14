using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            DependencyGraph t = new DependencyGraph();
            t.ReplaceDependents("a", new HashSet<string>());
            Console.WriteLine(t.Size);
            Console.ReadLine();
        }
    }
}
