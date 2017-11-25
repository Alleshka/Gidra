using GidraSIM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            TokensCollector tokensCollector = new TokensCollector();
            TestTpprProcedure testTpprProcedure = new TestTpprProcedure(tokensCollector);

            int prevCount = 0;
            int currentCount = -1;
            while (prevCount != currentCount && currentCount <100)
            {
                testTpprProcedure.Update(1);

                prevCount = currentCount;
                currentCount = tokensCollector.GetHistory().Count;
            }
            Console.WriteLine("Test passed");
        }
    }
}
