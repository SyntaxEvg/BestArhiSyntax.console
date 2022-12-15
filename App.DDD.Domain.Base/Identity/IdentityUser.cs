
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DDD.Domain.Base.Identity
{
    /// <summary>
    /// Добавляем дополнительные сущности к бд Identity
    /// </summary>
    [Table("ApplicationUser")]
    public class User : IdentityUser
    {
        //public string? FirstName { get; set; }
        //public string? LastName { get; set; }
        [Column("FirstName")]
        public string? FirstName { get; set; }
        [Column("LastName")]
        public string? LastName { get; set; }
       // public string  Email { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    [Table("ApplicationRole")]
    public class Role : IdentityRole
    {
        [Column("admin")]
        public string admin { get; set; }
        [Column("user")]
        public string user { get; set; }
        [Column("hr")]
        public string hr { get; set; }
    }

}
