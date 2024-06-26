using BattleSimulator.Data;
using BattleSimulator.Interfaces;
using BattleSimulator.Models;
using static BattleSimulator.Support;

namespace BattleSimulator.Repositories
{
    public class SoldierRepository : ISoldierRepository
    {
        private readonly ApplicationDBContext _context;

        public SoldierRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<Soldier>> CreateMultipleAsync(int ArenaId, int n)
        {
            Random random = new Random();

            for (int i = 0; i < n; i++)
            {
                Soldier soldier = new Soldier();
                soldier.ArenaId = ArenaId;
                soldier.Type = (SoldierType)random.Next(0,3);

                switch (soldier.Type)
                {
                    case SoldierType.Horseman:
                        soldier.Life = 150;
                        break;
                    case SoldierType.Knight:
                        soldier.Life = 120;
                        break;
                    case SoldierType.Archer:
                        soldier.Life = 100;
                        break;
                    default:
                        soldier.Life = 0;
                        break;
                }
                await _context.Soldier.AddAsync(soldier);
            }
            await _context.SaveChangesAsync();

            return _context.Soldier.ToList();
        }
    }
}
