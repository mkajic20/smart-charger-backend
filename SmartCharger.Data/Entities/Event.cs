using System.ComponentModel.DataAnnotations.Schema;

namespace SmartCharger.Data.Entities
{
    public class Event : BaseEntity
    {
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double? Volume { get; set; }

        [ForeignKey(nameof(Charger))]
        public int ChargerId { get; set; }
        public virtual Charger Charger { get; set; }

        [ForeignKey(nameof(Card))]
        public int CardId { get; set; }
        public virtual Card Card { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId{ get; set; }
        public virtual User User { get; set; }
    }
}