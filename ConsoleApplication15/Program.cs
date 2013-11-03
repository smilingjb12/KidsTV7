using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication15
{
    public enum Fault
    {
        ConstOne, ConstZero, CfiByFront, 
        CfiByFall, CfidByFrontSet0, CfidByFrontSet1,
        CfidByFallSet0, CfidByFallSet1
    }

    public class Cell
    {
        private int val;
        private Fault? fault;
        private Cell victim;
        private bool isVictim;

        public Cell()
        {
            val = 0;
            fault = null;
            victim = null;
            isVictim = false;
        }

        public Fault? Fault
        {
            get { return fault; }
        }

        public bool IsVictim
        {
            get { return isVictim; }
        }

        public void Write(int val)
        {
            if (this.fault == null)
            {
                this.val = val;
                return;
            }
            if (this.fault == ConsoleApplication15.Fault.ConstOne || this.fault == ConsoleApplication15.Fault.ConstZero)
            {
                return;
            }
            int prevVal = this.val;
            if (this.fault == ConsoleApplication15.Fault.CfiByFront)
            {
                if (Rising(prevVal, val))
                {
                    victim.InvertValue();
                }
            }
            else if (this.fault == ConsoleApplication15.Fault.CfiByFall)
            {
                if (Falling(prevVal, val))
                {
                    victim.InvertValue();
                }
            }
            else if (this.fault == ConsoleApplication15.Fault.CfidByFrontSet1)
            {
                if (Rising(prevVal, val))
                {
                    victim.val = 1;
                }
            }
            else if (this.fault == ConsoleApplication15.Fault.CfidByFrontSet0)
            {
                if (Rising(prevVal, val))
                {
                    victim.val = 0;
                }
            }
            else if (this.fault == ConsoleApplication15.Fault.CfidByFallSet1)
            {
                if (Falling(prevVal, val))
                {
                    victim.val = 1;
                }
            }
            else if (this.fault == ConsoleApplication15.Fault.CfidByFallSet0)
            {
                if (Falling(prevVal, val))
                {
                    victim.val = 0;
                }
            }
            this.val = val;
        }

        public int Read()
        {
            if (fault == ConsoleApplication15.Fault.ConstOne) return 1;
            if (fault == ConsoleApplication15.Fault.ConstZero) return 0;
            return val;
        }

        public override string ToString()
        {
            if (isVictim) return "V";
            if (victim != null) return "A";
            if (fault == ConsoleApplication15.Fault.ConstOne) return "O";
            if (fault == ConsoleApplication15.Fault.ConstZero) return "Z";
            if (val == 0) return ".";
            return val.ToString();
        }

        public void InvertValue()
        {
            val = val == 1 ? 0 : 1;
        }

        private bool Rising(int val1, int val2)
        {
            return val1 - val2 == -1;
        }

        private bool Falling(int val1, int val2)
        {
            return val1 - val2 == 1;
        }

        public void AssignFault(Fault fault, Cell victim = null)
        {
            this.fault = fault;
            if (victim != null)
            {
                this.victim = victim;
                victim.isVictim = true;
            }
        }
    }

    public class Ram
    {
        public static readonly int Size; // should be 1 Mbit
        private readonly Cell[] cells;

        private int allFaultsCount = -1;
        public int AllFaultsCount
        {
            get
            {
                if (allFaultsCount != -1) return allFaultsCount;
                return allFaultsCount = cells.Count(c => c.Fault != null);
            }
        }

        static Ram()
        {
            Size = 1000;
        }

        public Ram()
        {
            cells = ConstructCells();
        }

        private Cell[] ConstructCells()
        {
            return Enumerable.Range(0, Size)
                .Select(i => new Cell())
                .ToArray();
        }

        public Cell[] Cells
        {
            get { return cells; }
        }

        public Cell this[int index]
        {
            get { return cells[index]; }
        }

        public override string ToString()
        {
            return string.Join(" ", cells.Select(c => c.ToString()));
        }
    }

    public static class IntExtensions
    {
        public static int Invert(this int x)
        {
            return x == 1 ? 0 : 1;
        }
    }

    public class RamTester
    {
        private readonly Ram ram;

        public RamTester(Ram ram)
        {
            this.ram = ram;
        }

        public void TestWalking(int what)
        {
            int val = what;
            var badAddresses = new List<int>();
            for (int i = 0; i < Ram.Size; ++i)
            {
                ram[i].Write(val);
                for (int j = 0; j < Ram.Size; ++j)
                {
                    if (i == j) continue;
                    ram[j].Write(val.Invert());
                }
                //for (int j = 0; j < Ram.Size; ++j)
                //{
                //    if (i == j) continue;
                //    ram[j].Read();
                //}
                int baseCell = ram[i].Read();
                if (baseCell != val) badAddresses.Add(i);
            }

            ReportErrors(badAddresses);
        }

        public void TestMarch() // C-
        {
            var badAddresses = new HashSet<int>();
            for (int i = 0; i < Ram.Size; ++i)
            {
                ram[i].Write(0);
            }

            for (int i = 0; i < Ram.Size; ++i)
            {
                if (ram[i].Read() != 0) badAddresses.Add(i);
                ram[i].Write(1);
            }

            for (int i = 0; i < Ram.Size; i++)
            {
                if (ram[i].Read() != 1) badAddresses.Add(i);
                ram[i].Write(0);
            }

            for (int i = Ram.Size - 1; i >= 0; --i)
            {
                if (ram[i].Read() != 0) badAddresses.Add(i);
                ram[i].Write(1);
            }

            for (int i = Ram.Size - 1; i >= 0; --i)
            {
                if (ram[i].Read() != 1) badAddresses.Add(i);
                ram[i].Write(0);
            }

            for (int i = 0; i < Ram.Size; i++)
            {
                if (ram[i].Read() != 0) badAddresses.Add(i);
            }

            ReportErrors(badAddresses);
        }

        private void ReportErrors(IEnumerable<int> badAddresses)
        {
            int foundFaults = badAddresses.Count();
            double percentage = (double)foundFaults / ram.AllFaultsCount * 100;
            Console.WriteLine("found {0}/{1} => {2:F2}%", foundFaults, ram.AllFaultsCount, percentage);
        }
    }

    class Program
    {
        private static readonly Random random = new Random(DateTime.Now.Millisecond);

        static void Corrupt(Ram ram)
        {
            for (int i = 0; i < Ram.Size / 4; i++) // const one, const zero
            {
                int index = random.Next(Ram.Size);
                if (ram[index].Fault == null)
                {
                    ram[index].AssignFault(new[] { Fault.ConstOne, Fault.ConstZero }[random.Next(2)]);
                }
            }
            var constFaultsCount = ram.Cells.Count(c => new[] {Fault.ConstOne, Fault.ConstZero}.Any(f => f == c.Fault));
            Console.WriteLine("constant 0/1 faults: {0}", constFaultsCount);

            for (int i = 0; i < Ram.Size; ++i) // other faults
            {
                Cell aggressor = ram[random.Next(Ram.Size)];
                Cell victim = ram[random.Next(Ram.Size)];
                if (new[] {aggressor, victim}.All(c => c.Fault == null && !c.IsVictim))
                {
                    var allFaults = Enum.GetValues(typeof (Fault)).Cast<Fault>();
                    var randomFault = allFaults
                        .Except(new[] {Fault.ConstOne, Fault.ConstZero})
                        .ElementAt(random.Next(allFaults.Count() - 2));
                    aggressor.AssignFault(randomFault, victim: victim);
                }
            }
            var otherFaultsCount = ram.Cells.Count(c => c.Fault != null && c.Fault != Fault.ConstOne && c.Fault != Fault.ConstZero);
            Console.WriteLine("other faults: {0}", otherFaultsCount);
        }

        static void Main(string[] args)
        {
            var ram = new Ram();
            Corrupt(ram);
            Console.WriteLine("total faults count: {0}", ram.AllFaultsCount);
            Console.WriteLine();

            var tester = new RamTester(ram);

            Console.WriteLine("testing march");
            tester.TestMarch();
            Console.WriteLine();

            Console.WriteLine("testing walking 0");
            tester.TestWalking(0);
            Console.WriteLine("testing walking 1");
            tester.TestWalking(1);
            Console.WriteLine();
        }
    }
}
