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
    public enum RangeWeaponEnum
    {
        Shotgun = 0,
        Rifle = 1,
        DefaultPlayerWeapon = 10
    }    
    public enum AIStateType
    {
        Idle,
        Patrolling,
        Pursuit,
        Flee,
        Attack
    }
    public enum Languages
    {

    }
    public enum ArenaEndAction
    {
        OpenDoor = 0,
        SwitchScene = 1,
        None = 10
    }
    public enum LoadingType
    {
        SwitchBetweenLevels,
        CheckpointReload,
        ContinueFromMainMenu,
        NewGameFromMainMenu,
        ToMainMenu,
        None
    }
}