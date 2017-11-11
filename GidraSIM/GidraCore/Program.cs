using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GidraCore
{
    class Program
    {
        static void Main(string[] args)
        {
            TestRootProcess rootProcess = new TestRootProcess();
            for( int i=0; i< 10000; i++)
            {
                rootProcess.Update(i);
                Console.WriteLine(i);
            }
            Console.ReadKey();
        }
    }
}
