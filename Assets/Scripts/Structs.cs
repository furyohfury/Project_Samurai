namespace Samurai
{
    [System.Serializable]
    public struct UnitStatsStruct
    {
        public int MaxHP;
        public int HP;
        public float MoveSpeed;        
    }

    [System.Serializable]
    public struct UnitBuffsStruct
    {
        public int HPBuff;
        public int MoveSpeedBuff;

        public int RangeWeaponDamageBuff;
        public int MeleeWeaponDamageBuff;
    }

    [System.Serializable]
    public struct ProjectileStatsStruct
    {
        public int Damage;
        public float ProjectileSpeed;
        public float ProjectileScale;
    }

    [System.Serializable]
    public struct PlayerBuffsStruct
    {
        public int PickableWeaponDamageBuff;
        public float SlomoDurationBuff;
    }
}