using App.DDD.Domain.Base.EntityBase;

namespace App.DDD.Domain.Models
{
    public class Orders : EntityName
    {
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();

        public DateTimeOffset? Time { get; set; }
    }
}