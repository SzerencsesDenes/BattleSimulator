namespace BattleSimulator.Models
{
    public class ArenaEvent
    {
        public int Id { get; set; }
        public int ArenaId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
