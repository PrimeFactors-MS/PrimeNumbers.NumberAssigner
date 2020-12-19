using PrimeNumbers.NumberAssigner.Core.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimeNumbers.NumberAssigner.Core
{
    public class AvailableRangeFinder
    {
        const int RECORDS_PER_DRAW = 100;
        const int MAX_RANGE_ASSIGNMENT = 100;
        private readonly AssignmentDb _assignmentDb;

        public AvailableRangeFinder(AssignmentDb assignmentDb)
        {
            _assignmentDb = assignmentDb;
        }

        public NumberRange GetRangeAssignment()
        {
            NumberRange availableRange;
            AvailableRangeResult searchResult;

            searchResult = FindHoleRangeInDb();

            if (searchResult.FoundAvailableRange)
            {
                availableRange = searchResult.AvailableRange.Value;
                if (availableRange.End - availableRange.Start + 1 > MAX_RANGE_ASSIGNMENT)
                {
                    availableRange.End = availableRange.Start + MAX_RANGE_ASSIGNMENT - 1;
                }
            }
            else
            {
                var endOfRange = searchResult.TotalOccupiedRange.Value.End;
                availableRange = new NumberRange(endOfRange, endOfRange + MAX_RANGE_ASSIGNMENT - 1);
            }

            _assignmentDb.OccupyRange(availableRange);

            return availableRange;
        }


        /// <summary>
        /// Tries finding free range *between* occupied ranges in db
        /// </summary>
        private AvailableRangeResult FindHoleRangeInDb()
        {
            int iterations = 0;

            NumberRange previousOccupiedRange = new NumberRange(0, 0);
            AvailableRangeResult sliceResult;

            NumberRange[] occupiedRanges;
            do
            {
                occupiedRanges = _assignmentDb.GetOccupiedRanges(iterations * RECORDS_PER_DRAW, RECORDS_PER_DRAW);

                sliceResult = FindFreeRangeBetweenOccupied(occupiedRanges, previousOccupiedRange);

                if (sliceResult.FoundAvailableRange)
                {
                    return sliceResult;
                }

                previousOccupiedRange = sliceResult.TotalOccupiedRange.Value;

                iterations++;
                // last slice will be smaller than number requested
            } while (occupiedRanges.Length == RECORDS_PER_DRAW);

            return sliceResult;
        }

        /// <summary>
        /// Tries finding a free range *between* occopied ones.
        /// <para>NOTE: given preOccupiedRange start must be smaller than slice's ranges</para>
        /// </summary>
        /// <param name="occupiedRanges">List of occopied ranges to search between</param>
        /// <param name="preOccupiedRange">Total range occupied from previous slices</param>
        /// <returns></returns>
        private AvailableRangeResult FindFreeRangeBetweenOccupied(NumberRange[] occupiedRanges, NumberRange preOccupiedRange)
        {
            if (occupiedRanges.Length == 0) { return new AvailableRangeResult(false, preOccupiedRange, null); }

            Array.Sort(occupiedRanges, new NumberRangeStartComparer());
            NumberRange totalOccupiedRange = new NumberRange(occupiedRanges[0].Start, occupiedRanges[0].End);

            foreach (NumberRange curRange in occupiedRanges)
            {
                if (curRange.Start > totalOccupiedRange.End + 1) // not overlapping
                {
                    NumberRange availableRange = new(totalOccupiedRange.End + 1, curRange.Start - 1);
                    return new AvailableRangeResult(FoundAvailableRange: true, null, availableRange);
                }
                totalOccupiedRange.End = curRange.End;
            }

            if (totalOccupiedRange.Start > preOccupiedRange.End + 1)
            {
                NumberRange availableRange = new(preOccupiedRange.End + 1, totalOccupiedRange.Start - 1);
                return new AvailableRangeResult(FoundAvailableRange: true, null, availableRange);
            }

            return new AvailableRangeResult(FoundAvailableRange: false, totalOccupiedRange, null);
        }
    }
}
