using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeNumbers.NumberAssigner.API.Models
{
    public record KeepAliveRequest(uint WorkerId);
}
