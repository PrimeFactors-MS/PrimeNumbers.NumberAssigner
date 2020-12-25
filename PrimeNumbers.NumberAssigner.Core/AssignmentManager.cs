using PrimeNumbers.NumberAssigner.Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeNumbers.NumberAssigner.Core
{
    public class AssignmentManager
    {
        private readonly AssignmentDb _assignmentDb;
        private readonly AvailableRangeAssigner _availableRangeAssigner;

        public AssignmentManager(AssignmentDb assignmentDb, AvailableRangeAssigner availableRangeAssigner)
        {
            availableRangeAssigner.SetDb(assignmentDb);
            _assignmentDb = assignmentDb;
            _availableRangeAssigner = availableRangeAssigner;
        }

        public async Task<RangeAssignment> GetRangeAssignment() => await _availableRangeAssigner.GetRangeAssignment();

        public async Task UpdateKeepAlive(uint workerId) => await _assignmentDb.UpdateKeepAlive(workerId);
    }
}
