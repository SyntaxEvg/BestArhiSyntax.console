using Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DDD.Domain.Base.EntityBase
{
    public abstract class Entity : IEntity<Guid>
    {
        public Guid ID { get; set; }

        public bool Equals(Entity? entity)
        {
            if (entity is  null) return false;
            if (ReferenceEquals(this, entity)) return true;
            return ID.Equals(entity.ID);
        }
    }
    /// <summary>
    /// Готовая сущность для скармливания классам 
    /// </summary>
    public abstract class EntityName: Entity, IEntityName
    {
        public string Name { get; set; } = null;
    }

}
