using App.DDD.Domain.Base.EntityBase;
using Interfaces.Base.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DDD.Domain.Models.Authentication
{
    public class Account :IEntity<Guid>
    {
        public Guid ID { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        //[PasswordPropertyText]
        public string Password { get; set; }
        public RoleFromJWT[] Role { get; set; }
    }
    public enum RoleFromJWT
    {
        User,
        Admin,
        hr,
    }
}
