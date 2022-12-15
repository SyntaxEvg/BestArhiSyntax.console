using App.DDD.Domain.Base.EntityBase;
using Interfaces.Base.Base;

namespace App.DDD.Domain.Models
{
    public class EntityName : INamed, IEntity<Guid>
    {
        public string Name { get; set; }
        public Guid ID { get; set; }
    }
}
