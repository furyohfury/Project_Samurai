namespace Samurai
{
    public enum PhaseColor
    {
        Blue = 0,
        Red = 1,               
        Damaged = 2,
        Green = 10,
        Default = 500
    }
    public enum PlayerWeapon
    {
        Shotgun = 0,
        SniperRifle = 1,
        DefaultPistol = 10
    }    
    public enum AIStateType
    {
        Idle,
        Patrolling,
        Pursuit,
        Flee
    }
}