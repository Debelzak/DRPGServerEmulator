using DRPGServer.Common;
using DRPGServer.Game.Data.Managers;
using DRPGServer.Network.Packets.Map;

namespace DRPGServer.Game.Entities
{
    public class Digimon
    {
        public Player? Owner { get; private set; }
        public uint UID { get; set; }
        public Serial Serial { get; set; } = new();
        public ushort DigimonID { get; private set; }
        public string Name { get; set; } = string.Empty;

        // Stats
        public ushort Level { get; set; } = 1;
        public int CurrentHP { get; private set; }
        public int CurrentVP { get; private set; }
        public int CurrentEVP { get; private set; }
        public int MaxEVP { get; private set; }
        public int STR { get; set; }
        public int AGI { get; set; }
        public int CON { get; set; }
        public int INT { get; set; }
        public int MaxHP => (int)MathF.Round(STR * 1.2880f + CON * 2.9809f + 75.9363f);
        public int MaxVP => (int)MathF.Round(CON * 1.8277f + INT * 2.5518f + 72.8321f);
        public int ATK => (int)MathF.Round(STR * 2.8199f + AGI * 2.8323f + 93.5256f);
        public int DEF => (int)MathF.Round(CON * 2.9539f + INT * 0.2177f + 56.9070f);
        public int BR => (int)MathF.Round(AGI * 1.7256f + INT * 3.2886f + 82.0384f);
        public long EXP { get; private set; }
        public long NextLevelEXP { get; private set; } = 24;
        public int TotalWins { get; private set; }
        public int TotalBattles { get; private set; }
        public ushort AbilityPoints { get; private set; }
        public ushort SkillPoints { get; private set; }

        public byte Classification { get; private set; }
        public int CurrentActionGauge { get; set; }
        public int MaxActionGauge { get; set; }

        // Flags
        public bool IsKnockedOut => CurrentHP <= 0;
        public bool IsActionGaugeFull => CurrentActionGauge >= MaxActionGauge;

        public Digimon(ushort digimonId, Player? owner = null)
        {
            DigimonID = digimonId;
            if (!DigimonDataManager.DigimonTable.TryGetValue(digimonId, out var digimonData))
                return;

            Owner ??= owner;

            Name = digimonData.Name;

            CurrentHP = MaxHP;
            CurrentVP = MaxVP;
            //EVP?

            MaxActionGauge = digimonData.ActionGauge;
            Classification = digimonData.Classification;
        }

        public static Digimon Empty { get; private set; } = new Digimon(0)
        {
            UID = 0,
            Serial = new(new byte[16]),
            Level = 0,
            Name = string.Empty,
        };

        public void SetOwner(Player player)
        {
            Owner = player;
        }

        public void Heal(int hpAmount, int vpAmount, int evpAmount)
        {
            int newHp = CurrentHP + hpAmount;
            CurrentHP = (newHp > MaxHP) ? MaxHP : newHp;

            int newVp = CurrentVP + vpAmount;
            CurrentVP = (newVp > MaxVP) ? MaxVP : newVp;

            int newEvp = CurrentEVP + evpAmount;
            CurrentEVP = (newEvp > MaxEVP) ? MaxEVP : newEvp;
        }

        public void TakeDamage(int incomingDamage)
        {
            var newHp = CurrentHP - incomingDamage;
            CurrentHP = (newHp < 0) ? 0 : newHp;
        }

        public void AddExp(long incomingExp)
        {
            long newExp = EXP + incomingExp;
            EXP = (newExp > NextLevelEXP) ? NextLevelEXP : newExp;
            if (EXP < 0) EXP = 0;

            if (EXP >= NextLevelEXP)
            {
                LevelUp();
            }
        }

        public void LevelUp()
        {
            Level += 1;
            STR += 3;
            AGI += 2;
            CON += 2;
            INT += 1;
            AbilityPoints += 2;
            SkillPoints += 1;

            EXP = 0;
            NextLevelEXP = GetTargetExp(Level);

            Heal(MaxHP, MaxVP, MaxEVP);

            var levelup = new LevelUpPacket(this);
            Owner?.Client.Send(levelup);
        }

        public static long GetTargetExp(ushort level)
        {
            double exp = 0.977 * Math.Pow(level, 3)
                        - 0.770 * Math.Pow(level, 2)
                        + 30.29 * level
                        - 7.14;

            return (long)Math.Round(exp);
        }

    }
}