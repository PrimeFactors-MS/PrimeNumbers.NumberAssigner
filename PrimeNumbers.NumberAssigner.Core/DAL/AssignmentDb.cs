using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeNumbers.NumberAssigner.Core.DAL
{

    public class AssignmentDb : IDisposable
    {
        private readonly MySqlConnection _connection;
        private readonly List<NumberRange> _myDB;


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
        public async Task<NumberRange[]> GetOccupiedRanges(int skip, int length)
        {
            using MySqlCommand command = new("GetOccupiedRanges", _connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add(new MySqlParameter("skipRecords", MySqlDbType.UInt32));
            command.Parameters["skipRecords"].Value = skip;
            command.Parameters.Add(new MySqlParameter("maxLength", MySqlDbType.UInt32));
            command.Parameters["maxLength"].Value = length;

            List<NumberRange> numberRangeList = new();
            using (MySqlDataReader resultReader = (MySqlDataReader)await command.ExecuteReaderAsync())
            {
                while (await resultReader.ReadAsync())
                {
                    numberRangeList.Add(new NumberRange
                    {
                        Start = (ulong)resultReader["rangeStart"],
                        End = (ulong)resultReader["rangeEnd"],
                    });
                }
            }

            return numberRangeList.ToArray();
        }

        public async Task<uint> OccupyRange(NumberRange numberRange)
        {
            using MySqlCommand command = new("occupyRange", _connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add("rangeStart", MySqlDbType.UInt64);
            command.Parameters.Add("rangeEnd", MySqlDbType.UInt64);
            command.Parameters.Add("workerId", MySqlDbType.UInt32);
            command.Parameters["rangeStart"].Value = numberRange.Start;
            command.Parameters["rangeEnd"].Value = numberRange.End;
            command.Parameters["workerId"].Direction = ParameterDirection.Output;

            await command.ExecuteNonQueryAsync();

            return (uint)command.Parameters["workerId"].Value;            
        }



        public void Dispose()
        {
            _connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
