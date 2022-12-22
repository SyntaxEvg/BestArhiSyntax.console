using System.ComponentModel.DataAnnotations.Schema;

namespace App.DDD.Domain.Models
{
    [Table("Employees")]
    public class Employees : EntityName
    {
        [Column("LastName")] public string LastName { get; set; }
        [Column("Patronymic")] public string Patronymic { get; set; }
        [Column("Surname")] public string Surname { get; set; }

    }
}
