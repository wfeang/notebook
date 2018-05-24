using RDemo.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var route = new Routes();
            var defaultContext = new RContexts("Home", "Index");
            GlobalCU.Invoke(route).Begin(defaultContext);            
        }

    }
}
