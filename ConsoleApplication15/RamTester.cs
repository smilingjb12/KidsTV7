using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication15
{
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
                for (int j = 0; j < Ram.Size; ++j)
                {
                    if (i == j) continue;
                    ram[j].Read();
                }
                int baseCell = ram[i].Read();
                if (baseCell != val) badAddresses.Add(i);
            }

            ReportErrors(badAddresses);
        }

        public void TestMarchPS()
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
                if (ram[i].Read() != 1) badAddresses.Add(i);
                ram[i].Write(0);
                if (ram[i].Read() != 0) badAddresses.Add(i);
                ram[i].Write(1);
            }

            for (int i = 0; i < Ram.Size; i++)
            {
                if (ram[i].Read() != 1) badAddresses.Add(i);
                ram[i].Write(0);
                if (ram[i].Read() != 0) badAddresses.Add(i);
                ram[i].Write(1);
                if (ram[i].Read() != 1) badAddresses.Add(i);
            }

            for (int i = 0; i < Ram.Size; i++)
            {
                if (ram[i].Read() != 1) badAddresses.Add(i);
                ram[i].Write(0);
                if (ram[i].Read() != 0) badAddresses.Add(i);
                ram[i].Write(1);
                if (ram[i].Read() != 1) badAddresses.Add(i);
                ram[i].Write(0);
            }

            for (int i = Ram.Size - 1; i >= 0; --i)
            {
                if (ram[i].Read() != 0) badAddresses.Add(i);
                ram[i].Write(1);
                if (ram[i].Read() != 1) badAddresses.Add(i);
                ram[i].Write(0);
                if (ram[i].Read() != 0) badAddresses.Add(i);
            }

            ReportErrors(badAddresses);
        }

        public void TestMarchC()
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

            for (int i = 0; i < Ram.Size; i++)
            {
                if (ram[i].Read() != 0) badAddresses.Add(i);
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

        public void TestMarchCMinus() // C-
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
}