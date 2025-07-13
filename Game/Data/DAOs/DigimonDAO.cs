using DRPGServer.Game.Entities;
using DRPGServer.Common;

namespace DRPGServer.Game.Data.DAOs
{
    public static class DigimonDAO
    {
        public static Digimon? GetByUID(uint digimonUid)
        {
            using var conn = Database.GetConnection();
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM digimon WHERE uid = @digimon_uid";
            cmd.Parameters.AddWithValue("@digimon_uid", digimonUid);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            var digimon = new Digimon(reader.GetUInt16("digimon_id"))
            {
                UID = reader.GetUInt32("uid"),
                Name = reader.GetString("name"),
                Level = reader.GetUInt16("level"),
            };

            return digimon;
        }
    }
}