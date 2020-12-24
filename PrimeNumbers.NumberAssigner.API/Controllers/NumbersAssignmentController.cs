using Microsoft.AspNetCore.Mvc;
using PrimeNumbers.NumberAssigner.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeNumbers.NumberAssigner.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class NumbersAssignmentController : Controller
    {
        private readonly AvailableRangeFinder _availableRangeFinder;

        public NumbersAssignmentController(AvailableRangeFinder availableRangeFinder)
        {
            _availableRangeFinder = availableRangeFinder;
        }

        [HttpGet]
        public async Task<RangeAssignment> GetNumberAssignment()
        {
            return await _availableRangeFinder.GetRangeAssignment();
        }
    }
}
