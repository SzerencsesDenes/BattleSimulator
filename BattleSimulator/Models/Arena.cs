using System.ComponentModel.DataAnnotations.Schema;

namespace BattleSimulator.Models
{
    
    public class Arena
    {
        public int Id { get; set; }
        public List<Soldier> Soldiers { get; set; } = new List<Soldier>();
        public List<ArenaEvent> Events { get; set; } = new List<ArenaEvent>();

    }
}
