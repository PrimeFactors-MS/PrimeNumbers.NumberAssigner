using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PrimeNumbers.NumberAssigner.Core.DAL
{
    static class MySqlConnector
    {
        public static async Task<MySqlConnection> ConnectToDb(ConnectionParameters connectionParameters)
        {
            string connStr = GetConnectionString(connectionParameters);

            MySqlConnection dbConnection = new MySqlConnection(connStr);
            await dbConnection.OpenAsync();

            return dbConnection;
        }

        private static string GetConnectionString(ConnectionParameters connectionParameters)
        {
            return string.Format("server={0};user={1};database={2};port={3};password={4}",
                                 connectionParameters.Server,
                                 connectionParameters.Username,
                                 connectionParameters.Database,
                                 connectionParameters.Port,
                                 connectionParameters.Password);
        }
    }
}
