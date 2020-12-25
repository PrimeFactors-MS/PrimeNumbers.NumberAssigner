using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeNumbers.NumberAssigner.Core.DAL
{

    public class AssignmentDb
    {
        private readonly ConnectionFactory _connectionFactory;

        public AssignmentDb(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Returns a partial list of the ranges currently occupied in db
        /// </summary>
        /// <param name="skip">How many records to skip</param>
        /// <param name="length">How many records to take</param>
        /// <returns></returns>
        public async Task<NumberRange[]> GetOccupiedRanges(int skip, int length)
        {
            using MySqlConnection connection = await _connectionFactory.CreateConnection();
            using MySqlCommand command = new("GetOccupiedRanges", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add(new MySqlParameter("skipRecords", MySqlDbType.UInt32));
            command.Parameters["skipRecords"].Value = skip;
            command.Parameters.Add(new MySqlParameter("maxLength", MySqlDbType.UInt32));
            command.Parameters["maxLength"].Value = length;

            List<NumberRange> numberRangeList = new();
            {
                using MySqlDataReader resultReader = (MySqlDataReader)await command.ExecuteReaderAsync();
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
            using MySqlConnection connection = await _connectionFactory.CreateConnection();
            using MySqlCommand command = new("occupyRange", connection)
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

        public async Task UpdateKeepAlive(uint workerId)
        {
            using MySqlConnection connection = await _connectionFactory.CreateConnection();
            using MySqlCommand command = new("updateKeepAlive", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("workerId", MySqlDbType.UInt32);
            command.Parameters["workerId"].Value = workerId;

            await command.ExecuteNonQueryAsync();
        }
    }
}
