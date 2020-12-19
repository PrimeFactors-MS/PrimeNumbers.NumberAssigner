namespace PrimeNumbers.NumberAssigner.Core
{
    public struct NumberRange
    {
        public NumberRange(ulong start, ulong end)
        {
            Start = start;
            End = end;
        }
        public ulong Start { get; set; }
        public ulong End { get; set; }
    }
}
