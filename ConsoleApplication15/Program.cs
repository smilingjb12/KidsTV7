using System;
using System.Linq;

namespace ConsoleApplication15
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Test();
                if (Console.ReadLine() == "exit") break;
            }
        }

        static void Test()
        {
            var ram = new Ram();
            var corrupter = new RamCorrupter();
            corrupter.Corrupt(ram);
            Console.WriteLine("total faults count: {0}", ram.AllFaultsCount);
            Console.WriteLine(ram);
            Console.WriteLine();

            var tester = new RamTester(ram);

            Console.WriteLine("testing march");
            //tester.TestMarchCMinus();
            tester.TestMarchC();
            //tester.TestMarchPS();
            Console.WriteLine();

            Console.WriteLine("testing walking 0");
            tester.TestWalking(0);
            Console.WriteLine("testing walking 1");
            tester.TestWalking(1);
            Console.WriteLine();
        }
    }
}
