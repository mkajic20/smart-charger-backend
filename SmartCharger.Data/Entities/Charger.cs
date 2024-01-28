
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartCharger.Data.Entities
{
    public class Charger : BaseEntity
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public  DateTime CreationTime { get; set; }
        public  DateTime? LastSync { get; set; }
        public bool Active { get; set; }

        [ForeignKey(nameof(User))]
        public int CreatorId { get; set; }
        public virtual User User { get; set; }
        public bool IsDeleted { get; set; }
    }
}