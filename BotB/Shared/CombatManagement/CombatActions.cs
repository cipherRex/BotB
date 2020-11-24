
namespace BotB.Shared.CombatManagement
{

    public enum CombatActions
    {
        UNASSIGNED = 0,
        SWING = 1,
        BLOCK = 2,
        REST = 3
    }

    public enum AnimationCommands
    {
        AC_SWING = 0,
        AC_PARRY = 1,
        AC_COUNTERPARRY = 2,
        AC_KICK = 3,
        AC_CLEAVE = 4,
        AC_BLOCK = 5,
        AC_HEAL = 6,
        AC_GROINED = 7,
        AC_CLEAVED = 8,
        AC_DIE = 9,
        AC_CELEBRATE = 10,
        AC_LAUGH = 11,
        AC_RUN = 12
    }

    public enum CombatVictoryConditions
    {
        VICTORY_KILL = 0,
        VICTORY_RUNAWAY = 1
    }
}
