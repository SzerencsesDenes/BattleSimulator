using System.ComponentModel.DataAnnotations.Schema;
using static BattleSimulator.Support;

namespace BattleSimulator.Models
{
    
    public class Soldier
    {
        public int Id { get; set; }
        public SoldierType Type { get; set; }
        public float Life { get; set; }
        public int ArenaId { get; set; }
    }
}
