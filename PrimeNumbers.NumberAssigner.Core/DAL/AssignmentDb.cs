using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeNumbers.NumberAssigner.Core.DAL
{

    public class AssignmentDb : IDisposable
    {
        private readonly MySqlConnection _connection;

        internal AssignmentDb(MySqlConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Returns a partial list of the ranges currently occupied in db
        /// </summary>
        /// <param name="skip">How many records to skip</param>
        /// <param name="length">How many records to take</param>
        /// <returns></returns>
        public List<NumberRange> GetOccupiedRanges(int skip, int length)
        {
            return new List<NumberRange>()
            {
                new NumberRange(0, 100),
                new NumberRange(150, 300),
                new NumberRange(50, 130),
            };
        }

        public void OccupyRange(int skip, int length)
        {

        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
