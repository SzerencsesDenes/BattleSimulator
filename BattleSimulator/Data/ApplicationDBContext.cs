using Microsoft.EntityFrameworkCore;
using BattleSimulator.Models;

namespace BattleSimulator.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Arena> Arena { get; set; }
        public DbSet<Soldier> Soldier { get; set; }
        public DbSet<ArenaEvent> Events { get; set; }
    }
}
