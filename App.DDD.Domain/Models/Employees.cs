using App.DDD.Domain.Base.EntityBase;

namespace App.DDD.Domain.Models
{
    public class Employees : EntityName
    {
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Surname { get; set; }

    }
}