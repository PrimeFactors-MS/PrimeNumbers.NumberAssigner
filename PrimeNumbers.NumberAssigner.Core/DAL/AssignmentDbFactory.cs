using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;

namespace PrimeNumbers.NumberAssigner.Core.DAL
{
    public class AssignmentDbFactory
    {
        private readonly ConnectionParameters _connectionParameters;

        public AssignmentDbFactory(ConnectionParameters connectionParameters)
        {
            _connectionParameters = connectionParameters;
        }

        public AssignmentDb CreateConnection()
        {
            //var connection = MySqlConnector.ConnectToDb(_connectionParameters);
            return new AssignmentDb(null);
        }
    }
}
