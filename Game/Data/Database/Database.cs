using MySqlConnector;
using System.Data;

namespace DRPGServer.Game.Data.Database
{
    public static class Database
    {
        private static string _connectionString = "";

        public static void Initialize(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}