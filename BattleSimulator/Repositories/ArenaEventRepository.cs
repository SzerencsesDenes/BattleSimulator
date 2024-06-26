using BattleSimulator.Data;
using BattleSimulator.Interfaces;
using BattleSimulator.Models;
using Microsoft.EntityFrameworkCore;

namespace BattleSimulator.Repositories
{
    public class ArenaEventRepository : IArenaEventRepository
    {
        private readonly ApplicationDBContext _context;
        public ArenaEventRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<ArenaEvent>> GetArenaEventHistoryAsync(int arenaId)
        {
            return await _context.Events
                             .Where(e => e.ArenaId == arenaId)
                             .OrderBy(e => e.Timestamp)
                             .ToListAsync();
        }
    }
}
