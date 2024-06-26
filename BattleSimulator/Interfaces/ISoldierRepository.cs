using BattleSimulator.Models;

namespace BattleSimulator.Interfaces
{
    public interface ISoldierRepository
    {
        public Task<List<Soldier>> CreateMultipleAsync(int ArenaId, int n);
    }
}
