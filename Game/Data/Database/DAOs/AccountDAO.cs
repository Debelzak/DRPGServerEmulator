using DRPGServer.Game.Entities;
using MySqlConnector;

namespace DRPGServer.Game.Data.Database.DAOs
{
    public static class AccountDAO
    {
        public static Account? GetAccountByUsername(string username)
        {
            using var conn = Database.GetConnection();
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM account WHERE username = @username";
            cmd.Parameters.AddWithValue("@username", username);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            var account = new Account
            {
                UID = reader.GetUInt32("uid"),
                Username = reader.GetString("username"),
                AuthKey = reader.GetString("auth_key"),
                AuthorityLevel = reader.GetByte("authority"),
            };

            return account;
        }
    }
}