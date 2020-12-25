using Microsoft.AspNetCore.Mvc;
using PrimeNumbers.NumberAssigner.API.Models;
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
        private readonly AssignmentManager _assignmentManager;

        public NumbersAssignmentController(AssignmentManager assignmentManager)
        {
            _assignmentManager = assignmentManager;
        }

        [HttpGet]
        public async Task<ActionResult<RangeAssignment>> GetNumberAssignment()
        {
            return Ok(await _assignmentManager.GetRangeAssignment());
        }

        [HttpPost]
        public async Task<ActionResult> SendKeepAlive([FromBody] KeepAliveRequest keepAliveRequest)
        {
            await _assignmentManager.UpdateKeepAlive(keepAliveRequest.WorkerId);
            return Ok();
        }
    }
}
