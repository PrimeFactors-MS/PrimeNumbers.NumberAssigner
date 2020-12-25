using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeNumbers.NumberAssigner.Core.DAL
{
    public class ConnectionFactory : IDisposable
    {
        private readonly ConnectionParameters _connectionParameters;

        public ConnectionFactory(ConnectionParameters connectionParameters)
        {
            _connectionParameters = connectionParameters;
        }

        public async Task<MySqlConnection> CreateConnection()
        {
            return await MySqlConnector.ConnectToDb(_connectionParameters);
        }

        public void Dispose()
        {
            MySqlConnection.ClearAllPools();
        }
    }
}
