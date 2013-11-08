namespace ConsoleApplication15
{
    public static class IntExtensions
    {
        public static int Invert(this int x)
        {
            return x == 1 ? 0 : 1;
        }
    }
}