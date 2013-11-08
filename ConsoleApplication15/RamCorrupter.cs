using System;
using System.Linq;

namespace ConsoleApplication15
{
    public class RamCorrupter
    {
        private static readonly Random random = new Random(DateTime.Now.Millisecond);

        public void Corrupt(Ram ram)
        {
            AddConstantFaultsTo(ram);
            AddOtherFaultsTo(ram);
        }

        private void AddOtherFaultsTo(Ram ram)
        {
            for (int i = 0; i < Ram.Size; ++i) // other faults
            {
                Cell aggressor = ram[random.Next(Ram.Size)];
                Cell victim = ram[random.Next(Ram.Size)];
                if (new[] { aggressor, victim }.All(c => c.Fault == null && !c.IsVictim))
                {
                    var allFaults = Enum.GetValues(typeof(FaultType)).Cast<FaultType>();
                    var randomFault = allFaults
                        .Except(new[] { FaultType.ConstOne, FaultType.ConstZero })
                        .ElementAt(random.Next(allFaults.Count() - 2));
                    aggressor.AssignFault(randomFault, victim: victim);
                }
            }
            var otherFaultsCount = ram.Cells.Count(c => c.Fault != null && c.Fault != FaultType.ConstOne && c.Fault != FaultType.ConstZero);
            Console.WriteLine("other faults: {0}", otherFaultsCount);
        }

        private void AddConstantFaultsTo(Ram ram)
        {
            for (int i = 0; i < Ram.Size / 4; i++)
            {
                int index = random.Next(Ram.Size);
                if (ram[index].Fault == null)
                {
                    ram[index].AssignFault(new[] { FaultType.ConstOne, FaultType.ConstZero }[random.Next(2)]);
                }
            }
            var constFaultsCount = ram.Cells.Count(c => new[] { FaultType.ConstOne, FaultType.ConstZero }.Any(f => f == c.Fault));
            Console.WriteLine("constant 0/1 faults: {0}", constFaultsCount);
        }
    }
}