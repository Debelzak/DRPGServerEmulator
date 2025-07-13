using DRPGServer.Game.Entities;
using MySqlConnector;
using DRPGServer.Common;

namespace DRPGServer.Game.Data.DAOs
{
    public static class CharacterDAO
    {
        public static Character[] GetAccountCharacters(uint accountUid)
        {
            var characterArray = new Character[4] { Character.Empty, Character.Empty, Character.Empty, Character.Empty };

            using var conn = Database.GetConnection();
            conn.Open();

            string query = """
                SELECT 
                    c.*, 
                    d.uid AS digimon_uid,
                    d.name AS digimon_name,
                    d.digimon_id, 
                    d.level AS digimon_level
                FROM `character` c
                INNER JOIN digimon d ON c.uid = d.character_uid
                WHERE c.account_uid = @account_uid AND d.is_leader = 1;
            """;

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@account_uid", accountUid);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                byte slot = reader.GetByte("slot");
                if (slot < 0 || slot > 3) continue;

                var character = new Character
                {
                    UID = reader.GetUInt32("uid"),
                    Name = reader.GetString("name"),
                    TamerID = reader.GetByte("tamer_id"),
                    Level = reader.GetUInt16("level"),
                    MapID = reader.GetByte("map_id"),
                    PositionX = reader.GetInt16("pos_x"),
                    PositionY = reader.GetInt16("pos_y"),
                };

                var mainDigimon = new Digimon(reader.GetUInt16("digimon_id"))
                {
                    UID = reader.GetUInt32("digimon_uid"),
                    Name = reader.GetString("digimon_name"),
                    Level = reader.GetUInt16("digimon_level"),
                };

                character.MainDigimon = mainDigimon;
                characterArray[slot] = character;
            }

            return characterArray;
        }

        public static Character? GetCharacterByUID(uint characterUid)
        {
            using var conn = Database.GetConnection();
            conn.Open();

            string query = """
                SELECT 
                    c.*, 
                    d.uid AS digimon_uid,
                    d.name AS digimon_name,
                    d.digimon_id, 
                    d.level AS digimon_level
                FROM `character` c
                INNER JOIN digimon d ON c.uid = d.character_uid
                WHERE c.uid = @character_uid AND d.is_leader = 1;
            """;

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@character_uid", characterUid);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            var character = new Character
            {
                UID = reader.GetUInt32("uid"),
                Name = reader.GetString("name"),
                TamerID = reader.GetByte("tamer_id"),
                Level = reader.GetUInt16("level"),
                MapID = reader.GetByte("map_id"),
                PositionX = reader.GetInt16("pos_x"),
                PositionY = reader.GetInt16("pos_y"),
            };

            var mainDigimon = new Digimon(reader.GetUInt16("digimon_id"))
            {
                UID = reader.GetUInt32("digimon_uid"),
                Name = reader.GetString("digimon_name"),
                Level = reader.GetUInt16("digimon_level"),
            };

            character.MainDigimon = mainDigimon;
            return character;
        }

        public static uint? CreateCharacter(uint accountUid, byte slot, string characterName, byte tamerId, string digimonName, ushort digimonId)
        {
            if (slot < 0 || slot > 3)
                throw new ArgumentOutOfRangeException(nameof(slot), "Character slot must be between 0 and 3.");

            using var conn = Database.GetConnection();
            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                // 1. Insert Character
                string insertCharacter = """
                    INSERT INTO `character` 
                        (account_uid, slot, name, tamer_id, map_id, pos_x, pos_y)
                    VALUES 
                        (@account_uid, @slot, @name, @tamer_id, @map_id, @pos_x, @pos_y);
                    SELECT LAST_INSERT_ID();
                """;

                using var charCmd = new MySqlCommand(insertCharacter, conn, transaction);
                charCmd.Parameters.AddWithValue("@account_uid", accountUid);
                charCmd.Parameters.AddWithValue("@slot", slot);
                charCmd.Parameters.AddWithValue("@name", characterName);
                charCmd.Parameters.AddWithValue("@tamer_id", tamerId);
                charCmd.Parameters.AddWithValue("@map_id", 79);
                charCmd.Parameters.AddWithValue("@pos_x", 56);
                charCmd.Parameters.AddWithValue("@pos_y", 56);

                uint characterUid = Convert.ToUInt32(charCmd.ExecuteScalar());

                // 2. Insert main digimon
                string insertDigimon = """
                    INSERT INTO digimon (character_uid, digimon_id, name, level, is_leader)
                    VALUES (@character_uid, @digimon_id, @name, @level, 1);
                    SELECT LAST_INSERT_ID();
                """;

                using var digimonCmd = new MySqlCommand(insertDigimon, conn, transaction);
                digimonCmd.Parameters.AddWithValue("@character_uid", characterUid);
                digimonCmd.Parameters.AddWithValue("@digimon_id", digimonId);
                digimonCmd.Parameters.AddWithValue("@name", digimonName);
                digimonCmd.Parameters.AddWithValue("@level", 1);

                uint digimonUid = Convert.ToUInt32(digimonCmd.ExecuteScalar());

                transaction.Commit();
                return characterUid;
            }
            catch (Exception ex)
            {
                Logger.Error($"[CharacterDAO] Failed to create character: {ex.Message}");
                transaction.Rollback();
                return null;
            }
        }

        public static bool IsNicknameAvailable(string nickname)
        {
            using var conn = Database.GetConnection();
            conn.Open();

            string query = "SELECT 1 FROM `character` WHERE `name` = @name LIMIT 1";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", nickname);

            using var reader = cmd.ExecuteReader();
            return !reader.HasRows;
        }
        
        public static bool DeleteCharacter(uint characterUid, uint account_uid)
        {
            using var conn = Database.GetConnection();
            conn.Open();

            string query = "DELETE FROM `character` WHERE `uid` = @uid AND `account_uid` = @account_uid";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@uid", characterUid);
            cmd.Parameters.AddWithValue("@account_uid", account_uid);

            int affectedRows = cmd.ExecuteNonQuery();
            return affectedRows > 0;
        }
    }
}