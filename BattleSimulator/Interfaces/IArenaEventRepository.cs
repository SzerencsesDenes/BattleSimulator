using BattleSimulator.Models;

namespace BattleSimulator.Interfaces
{
    public interface IArenaEventRepository
    {
        public Task<List<ArenaEvent>> GetArenaEventHistoryAsync(int arenaId);
    }
}
