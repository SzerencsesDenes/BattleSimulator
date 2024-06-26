using BattleSimulator.Data;
using BattleSimulator.Interfaces;
using BattleSimulator.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BattleSimulator.Repositories
{
    public class ArenaRepository : IArenaRepository
    {
        private readonly ApplicationDBContext _context;

        public ArenaRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Arena> CreateAsync()
        {
            Arena arena = new Arena();

            await _context.Arena.AddAsync(arena);
            await _context.SaveChangesAsync();
            return arena;
        }

        public async Task DeleteAsync(int id)
        {
            var arena = await _context.Arena.Include(x => x.Soldiers).Include(y => y.Events).FirstOrDefaultAsync(a => a.Id == id);

            if (arena == null)
                return;
            _context.Arena.Remove(arena);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task<Arena?> DoBattleAsync(int id)
        {
            var arena = await _context.Arena
                                        .Include(x => x.Soldiers)
                                        .Include(y => y.Events)
                                        .FirstOrDefaultAsync(a => a.Id == id);

            if (arena == null)
                return null;

            Random random = new Random();
            int roundCounter = 0;
            List <string> eventQueue = new List<string>();

            while (arena.Soldiers.Count > 1)
            {
                Soldier attacker = arena.Soldiers.ElementAt(random.Next(0, arena.Soldiers.Count));
                Soldier defender = arena.Soldiers.ElementAt(random.Next(0, arena.Soldiers.Count));

                if(attacker.Id == defender.Id) //if we randomly chose the same soldier twice
                    continue;

                roundCounter++;
                eventQueue.Add($"Round {roundCounter}");
                eventQueue.Add($"Soldier No. {attacker.Id} {attacker.Type.ToString()} attacks soldier No. {defender.Id} {defender.Type.ToString()}.");

                attacker.Life = attacker.Life / 2;
                defender.Life = defender.Life / 2;
                if (!Support.IsAlive(attacker)) //Case attacker dead and possibly defender too
                {
                    arena.Soldiers.Remove(attacker);
                    eventQueue.Add($"Soldier No. {attacker.Id} dies due to health loss.");
                    if (!Support.IsAlive(defender))
                    {
                        arena.Soldiers.Remove(defender);
                        eventQueue.Add($"Soldier No. {defender.Id} dies due to health loss.");
                    }   
                    continue;
                }
                if (!Support.IsAlive(defender)) //Case attacker is alive and defender is possibly dead
                {
                    arena.Soldiers.Remove(defender);
                    eventQueue.Add($"Soldier No. {defender.Id} dies due to health loss.");
                    continue;
                }

                //Healing the resting soldiers
                foreach (var soldier in arena.Soldiers)
                {
                    if (soldier.Id == attacker.Id || soldier.Id == defender.Id)
                        continue;
                    if(soldier.Type is Support.SoldierType.Horseman)
                    {
                        soldier.Life = soldier.Life + 10;
                        if (soldier.Life > 150)
                            soldier.Life = 150;
                    }
                    else if (soldier.Type is Support.SoldierType.Knight)
                    {
                        soldier.Life = soldier.Life + 10;
                        if (soldier.Life > 120)
                            soldier.Life = 120;
                    }
                    else if (soldier.Type is Support.SoldierType.Archer)
                    {
                        soldier.Life = soldier.Life + 10;
                        if (soldier.Life > 100)
                            soldier.Life = 100;
                    }
                }

                //The two participants fight
                switch (attacker.Type)
                {
                    case Support.SoldierType.Horseman:
                        if (defender.Type is Support.SoldierType.Horseman)
                            arena.Soldiers.Remove(defender);
                        else if (defender.Type is Support.SoldierType.Knight)
                            arena.Soldiers.Remove(attacker);
                        else if (defender.Type is Support.SoldierType.Archer)
                            arena.Soldiers.Remove(defender);
                        break;
                    case Support.SoldierType.Knight:
                        if (defender.Type is Support.SoldierType.Knight)
                            arena.Soldiers.Remove(defender);
                        else if(defender.Type is Support.SoldierType.Archer)
                            arena.Soldiers.Remove(defender);
                        break;
                    case Support.SoldierType.Archer:
                        if (defender.Type is Support.SoldierType.Horseman)
                        {
                            if (random.Next(0, 100) <= 40)
                                arena.Soldiers.Remove(defender);
                        }
                        else if (defender.Type is Support.SoldierType.Knight)
                            arena.Soldiers.Remove(defender);
                        else if (defender.Type is Support.SoldierType.Archer)
                            arena.Soldiers.Remove(defender);
                        break;
                    default:
                        break;
                }

                if (!arena.Soldiers.Contains(attacker))
                    eventQueue.Add($"Attacking soldier No.{attacker.Id} has been killed by defender soldier No.{defender.Id}");
                else
                    eventQueue.Add($"Attacking soldier No.{attacker.Id} has survived with {attacker.Life} health.");
                if (!arena.Soldiers.Contains(defender))
                    eventQueue.Add($"Defender soldier No.{defender.Id} has been killed by attacker soldier No.{attacker.Id}");
                else
                    eventQueue.Add($"Defender soldier No.{defender.Id} has survived with {defender.Life} health.");
            }

            Soldier winner = arena.Soldiers.First();
            if (winner != null)
                eventQueue.Add($"Soldier No.{winner.Id} {winner.Type.ToString()} has won the battle with {winner.Life} health remaining.");
            else
                eventQueue.Add("Nobody survived the battle.");
            

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Save changes to remove soldiers
                    await _context.SaveChangesAsync();

                    
                    // Add events
                    foreach (var desc in eventQueue)
                    {
                        arena.Events.Add(Support.CreateEvent(desc, id));
                    }

                    // Save changes to add events
                    await _context.SaveChangesAsync();

                    // Commit transaction
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    // Rollback transaction in case of an error
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return arena;
        }

        public async Task<List<Arena>> GetAllAsync()
        {
            var Arenas = _context.Arena.Include(x => x.Soldiers);
            return await Arenas.ToListAsync();
        }

    }
}
