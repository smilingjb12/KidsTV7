namespace ConsoleApplication15
{
    public class Cell
    {
        private int val;
        private FaultType? fault;
        private Cell victim;
        private bool isVictim;

        public Cell()
        {
            val = 0;
            fault = null;
            victim = null;
            isVictim = false;
        }

        public FaultType? Fault
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
            if (this.fault == FaultType.ConstOne || this.fault == FaultType.ConstZero)
            {
                return;
            }
            int prevVal = this.val;
            if (this.fault == FaultType.CfiByFront)
            {
                if (Rising(prevVal, val))
                {
                    victim.InvertValue();
                }
            }
            else if (this.fault == FaultType.CfiByFall)
            {
                if (Falling(prevVal, val))
                {
                    victim.InvertValue();
                }
            }
            else if (this.fault == FaultType.CfidByFrontSet1)
            {
                if (Rising(prevVal, val))
                {
                    victim.val = 1;
                }
            }
            else if (this.fault == FaultType.CfidByFrontSet0)
            {
                if (Rising(prevVal, val))
                {
                    victim.val = 0;
                }
            }
            else if (this.fault == FaultType.CfidByFallSet1)
            {
                if (Falling(prevVal, val))
                {
                    victim.val = 1;
                }
            }
            else if (this.fault == FaultType.CfidByFallSet0)
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
            if (fault == FaultType.ConstOne) return 1;
            if (fault == FaultType.ConstZero) return 0;
            return val;
        }

        public override string ToString()
        {
            if (isVictim) return "V";
            if (victim != null) return "A";
            if (fault == FaultType.ConstOne) return "O";
            if (fault == FaultType.ConstZero) return "Z";
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

        public void AssignFault(FaultType faultType, Cell victim = null)
        {
            this.fault = faultType;
            if (victim != null)
            {
                this.victim = victim;
                victim.isVictim = true;
            }
        }
    }
}