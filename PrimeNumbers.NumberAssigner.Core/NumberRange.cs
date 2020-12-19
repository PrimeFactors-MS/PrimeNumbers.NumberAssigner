namespace PrimeNumbers.NumberAssigner.Core
{
    public struct NumberRange
    {
        public NumberRange(int start, int end)
        {
            Start = start;
            End = end;
        }
        public int Start { get; set; }
        public int End { get; set; }
    }
}
