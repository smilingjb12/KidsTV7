using System.Linq;

namespace ConsoleApplication15
{
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
}