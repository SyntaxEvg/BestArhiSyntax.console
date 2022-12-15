using App.DDD.Domain.Base.EntityBase;
using Interfaces.Base;

namespace App.DDD.Domain.Models
{

    public class OrderItem : EntityName
    {

        public virtual Orders Orders { get; set; }

        public int Quantity { get; set; } = 0;
    }
}