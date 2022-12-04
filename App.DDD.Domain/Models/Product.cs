using App.DDD.Domain.Base.EntityBase;

namespace App.DDD.Domain.Models
{
    public class Product : EntityName
    {
        public string Description { get; set; }
    }
}