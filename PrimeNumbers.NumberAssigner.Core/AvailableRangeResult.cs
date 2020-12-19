namespace PrimeNumbers.NumberAssigner.Core
{
    internal record AvailableRangeResult(
            bool FoundAvailableRange,
            NumberRange? TotalOccupiedRange,
            NumberRange? AvailableRange);
}
