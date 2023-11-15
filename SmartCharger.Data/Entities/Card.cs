using System.ComponentModel.DataAnnotations.Schema;

namespace SmartCharger.Data.Entities
{
    public class Card : BaseEntity
    {
        public string Value { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}