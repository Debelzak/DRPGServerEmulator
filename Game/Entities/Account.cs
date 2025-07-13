using DRPGServer.Game.Enum;
using DRPGServer.Network;

namespace DRPGServer.Game.Entities
{
    public class Account
    {
        public uint UID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string AuthKey { get; set; } = string.Empty;
        public IReadOnlyList<Character> Characters => _characters;
        public byte AuthorityLevel { get; set; } = (byte)AUTHORITY_ID.NONE;

        private readonly Character[] _characters = [new Character(), new Character(), new Character(), new Character()];

        public void SetCharacterSlot(byte slot, Character character)
        {
            if (slot < 0 || slot > 3) throw new ArgumentOutOfRangeException($"Trying to set an invalid character slot: {slot}");
            ArgumentNullException.ThrowIfNull(character);

            _characters[slot] = character;
        }

        public Character GetCharacterSlot(byte slot)
        {
            if (slot < 0 || slot > 3)
                throw new ArgumentOutOfRangeException(nameof(slot));

            return _characters[slot];
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
                if (_characters[i] != null && _characters[i].Name == nickname)
                {
                    return i;
                }
            }

            return null;
        }
    }
}