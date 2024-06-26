using BattleSimulator.Models;

namespace BattleSimulator.Interfaces
{
    public interface IArenaRepository
    {
        public Task<List<Arena>> GetAllAsync();
        public Task<Arena> CreateAsync();
        public Task DeleteAsync(int id);

        public Task<Arena?> DoBattleAsync(int id);
    }
}
