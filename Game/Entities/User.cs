using DRPGServer.Game.Enum;
using DRPGServer.Network;

namespace DRPGServer.Game.Entities
{
    public class User(string username)
    {
        public uint UID { get; set; } = 0;
        public string Username { get; private set; } = username;
        public IReadOnlyList<Character> Characters => _characters;
        public byte AuthorityLevel { get; set; } = (byte)AUTHORITY_ID.NONE;

        private readonly Character[] _characters = [new Character(Digimon.Empty), new Character(Digimon.Empty), new Character(Digimon.Empty), new Character(Digimon.Empty)];

        public void SetCharacterSlot(byte slot, Character character)
        {
            if (slot < 1 || slot > 4) throw new ArgumentOutOfRangeException($"Trying to set an invalid character slot: {slot}");
            ArgumentNullException.ThrowIfNull(character);

            _characters[slot - 1] = character;
        }

        public Character GetCharacterSlot(byte slot)
        {
            if (slot < 1 || slot > 4)
                throw new ArgumentOutOfRangeException(nameof(slot));

            return _characters[slot - 1];
        }

        public bool RemoveCharacter(uint uid)
        {
            for (int i = 0; i < _characters.Length; i++)
            {
                if (_characters[i] != null && _characters[i].UID == uid)
                {
                    _characters[i] = Character.Empty;
                    return true;
                }
            }

            return false;
        }

        public byte? GetCharacterSlotByNickname(string nickname)
        {
            for (byte i = 0; i < _characters.Length; i++)
            {
                if (_characters[i] != null && _characters[i].Nickname == nickname)
                {
                    return (byte?)(i + 1);
                }
            }

            return null;
        }
    }
}