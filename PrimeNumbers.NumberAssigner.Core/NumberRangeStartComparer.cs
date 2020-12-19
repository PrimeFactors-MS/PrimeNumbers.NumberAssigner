using System.Collections.Generic;

namespace PrimeNumbers.NumberAssigner.Core
{
    internal class NumberRangeStartComparer : IComparer<NumberRange>
    {
        int IComparer<NumberRange>.Compare(NumberRange x, NumberRange y)
        {
            ulong xStart = x.Start;
            ulong yStart = y.Start;

            if (yStart > xStart) { return -1; }
            if (yStart < xStart) { return 1; }
            return 0; // y == x
        }
    }
}
