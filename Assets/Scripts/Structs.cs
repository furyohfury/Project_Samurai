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
    public struct ProjectileStatsStruct
    {
        public int Damage;
        public float ProjectileSpeed;
        public float ProjectileScale;
    }
}