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
        private List<NumberRange> _myDB;


        internal AssignmentDb(MySqlConnection connection)
        {
            _connection = connection;
            _myDB = new ()
            {
                new NumberRange(0, 1000),
                new NumberRange(1800, 3000),
                new NumberRange(1097, 1701),
                new NumberRange(500, 1300),
            };
        }

        /// <summary>
        /// Returns a partial list of the ranges currently occupied in db
        /// </summary>
        /// <param name="skip">How many records to skip</param>
        /// <param name="length">How many records to take</param>
        /// <returns></returns>
        public NumberRange[] GetOccupiedRanges(int skip, int length)
        {
            return _myDB.ToArray();
        }

        public void OccupyRange(NumberRange numberRange)
        {
            _myDB.Add(numberRange);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
