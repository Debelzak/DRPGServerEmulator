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
        private int BaseSTR { get; set; }
        public int STR { get; set; }
        private int BaseAGI { get; set; }
        public int AGI { get; set; }
        private int BaseCON { get; set; }
        public int CON { get; set; }
        private int BaseINT { get; set; }
        public int INT { get; set; }
        public int MaxHP => (int)MathF.Round(STR * 0.90f + CON * 3.10f + 75f);
        public int MaxVP => (int)MathF.Round(CON * 1.4436f + INT * 2.5207f + 75f);
        public int ATK => (int)MathF.Round(STR * 4.3706f + AGI * 1.8705f + 60.0000f);
        public int DEF => (int)MathF.Round(CON * 3.0693f + INT * 0.6298f + 43.0000f);
        public int BR => (int)MathF.Round(AGI * 1.1758f + INT * 3.5702f + 85.0000f);
        public long EXP { get; private set; }
        public long NextLevelEXP { get; private set; }
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
            BaseSTR = digimonData.BaseSTR;
            BaseAGI = digimonData.BaseAGI;
            BaseCON = digimonData.BaseCON;
            BaseINT = digimonData.BaseINT;
            MaxActionGauge = digimonData.ActionGauge;
            NextLevelEXP = GetNextLevelExp(Level);
            Classification = digimonData.Classification;

            CurrentHP = MaxHP;
            CurrentVP = MaxVP;
            CurrentEVP = MaxEVP;
        }

        public static Digimon Empty { get; private set; } = new Digimon(digimonId: 0)
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

        public void LevelUp(ushort amount = 1)
        {
            Level += amount;
            STR += BaseSTR * amount;
            AGI += BaseAGI * amount;
            CON += BaseCON * amount;
            INT += BaseINT * amount;
            AbilityPoints += (ushort)(2 * amount);
            SkillPoints += (ushort)(1 * amount);

            EXP = 0;
            NextLevelEXP = GetNextLevelExp(Level);

            Heal(MaxHP, MaxVP, MaxEVP);

            var levelup = new LevelUpPacket(this);
            Owner?.Client.Send(levelup);
        }

        public bool AddAbilityPoint(byte statId, bool consumePoints = true)
        {
            if (statId < 1 && statId > 4) return false;

            if (consumePoints && AbilityPoints < 1)
                return false;
            else
                AbilityPoints--;
            
            switch (statId)
            {
                case 1: STR++; break;
                case 2: AGI++; break;
                case 3: CON++; break;
                case 4: INT++; break;
                default: return false;
            }

            return true;
        }

        public static long GetNextLevelExp(ushort level)
        {
            ExpTableManager.DigimonExpTable.TryGetValue(level, out long nextLevelExp);
            return (nextLevelExp <= 0) ? long.MaxValue : nextLevelExp;
        }

    }
}