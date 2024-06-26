using BattleSimulator.Models;

namespace BattleSimulator
{
    public static class Support
    {
        public enum SoldierType
        {
            Horseman,
            Knight,
            Archer
        }

        public static bool IsAlive(Soldier soldier)
        {
            if (soldier.Type == SoldierType.Horseman && soldier.Life >= (150 / 4))
                return true;
            else if (soldier.Type == SoldierType.Knight && soldier.Life >= (120 / 4))
                return true;
            else if (soldier.Type == SoldierType.Archer && soldier.Life >= (100 / 4))
                return true;
            else
                return false;
        }

        public static ArenaEvent CreateEvent(string desc, int id)
        {
            ArenaEvent arenaEvent = new ArenaEvent();
            arenaEvent.ArenaId = id;
            arenaEvent.Timestamp = DateTime.Now;
            arenaEvent.Description = desc;

            return arenaEvent;
        }
    }
}
