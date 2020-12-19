using PrimeNumbers.NumberAssigner.Core.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrimeNumbers.NumberAssigner.Core
{
    public class AvailableRangeFinder
    {
        private readonly AssignmentDb _assignmentDb;

        public AvailableRangeFinder(AssignmentDb assignmentDb)
        {
            _assignmentDb = assignmentDb;
        }

        public NumberRange GetRangeAssignment()
        {
            List<NumberRange> occupiedRanges = _assignmentDb.GetOccupiedRanges(0, 100);

            return new NumberRange(5, 16);
        }
    }
}
