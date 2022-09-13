using System;

namespace Assets.CodeBase.Datas
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public PlayerLootData PlayerLoot;
        public Stats PlayerGranmaStats;
        public Stats PlayerBullyStats;

        public PlayerProgress(int defaultLevelNumber)
        {
            WorldData = new WorldData(defaultLevelNumber);
            PlayerLoot = new PlayerLootData();
            PlayerGranmaStats = new Stats();
            PlayerBullyStats = new Stats();
        }

        public override string ToString()
        {
            return $"LevelNumber = {WorldData.LevelNumber}, Money = {PlayerLoot.Collected}, Granma Stats = {PlayerGranmaStats}, Bully Stats = {PlayerBullyStats}";
        }
    }

    [Serializable]
    public class Stats
    {
        public Stats()
        {
            MoveSpeedMultiplier = new MoveSpeedMultiplier(this, "Move Speed");
            ParkourSpeedMultiplier = new ParkourSpeedMultiplier(this, "Parkour Speed");
            SneakerRecharge = new SneakerRecharge(this, "Trow Recharge");
        }

        public Ability MoveSpeedMultiplier;
        public Ability ParkourSpeedMultiplier;
        public Ability SneakerRecharge;
        public int AbilityCost;

        public override string ToString()
        {
            return 
                $"MoveSpeedMultiplier.Level = {MoveSpeedMultiplier.Level}, " +
                $"ParkourSpeedMultiplier.Level = {ParkourSpeedMultiplier.Level}, " +
                $"SneakerRecharge.Level = {SneakerRecharge.Level}";
        }
    }

    /// <summary>
    /// WRNING: do not create object of this class. This class have to de absctract, can not be because of Unity JsonUtility do not serialize it!
    /// </summary>
    [Serializable]
    public class Ability
    {
        private readonly Stats _playerStats;

        public string Name;
        public int Level;

        public Ability(Stats playerStats, string name)
        {
            _playerStats = playerStats;
            Name = name;
        }

        public bool MaxLevelReached => Level == MaxLevel;
        public int NextLevel => Level + 1;
        public int NextLevelCost => _playerStats.AbilityCost * NextLevel;
        public int MaxLevel => 10;
        public virtual float Value { get; }
    }

    [Serializable]
    public class ParkourSpeedMultiplier : Ability
    {
        // стоимость 1 ур = 30 монет, каждый след ур + 30 от прошлого
        // 10 уровней всего, стартовый ур = 0, начальное значение = 1, +10% от стартового каждый уровень (0ур = 1, 10 ур = 2)
        //public float ParkourSpeedMultiplier => 1 + 0.1f * ParkourSpeedLevel;//1;
        //public int ParkourSpeedLevel = 0;

        public ParkourSpeedMultiplier(Stats playerStats, string name) : base(playerStats, name)
        { }

        public override float Value => 1 + 0.1f * Level;
    }

    [Serializable]
    public class MoveSpeedMultiplier : Ability
    {
        // стоимость 1 ур = 30 монет, каждый след ур + 30 от прошлого
        // 10 уровней всего, стартовый ур = 0, начальное значение = 1, +10% от стартового каждый уровень (0ур = 1, 10 ур = 2)
        //public float MoveSpeedMultiplier => 1 + 0.1f * MoveSpeedLevel;//1;
        //public int MoveSpeedLevel = 0;

        public MoveSpeedMultiplier(Stats playerStats, string name) : base(playerStats, name)
        { }

        public override float Value => 1 + 0.1f * Level;
    }

    [Serializable]
    public class SneakerRecharge : Ability
    {
        // 10 уровней всего, стартовый ур = 0, начальное значение = 6, уменьшается на 0,5 каждый ур (0ур = 6, 10 ур = 1)
        //public float SneakerRecharge_ => 6 - 0.5f * SneakerRechargeLevel;//6;
        //public int SneakerRechargeLevel = 0;

        public SneakerRecharge(Stats playerStats, string name) : base(playerStats, name)
        { }

        public override float Value => 6 - 0.5f * Level;
    }
}
