using Interfaces.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.DDD.Domain.Models.Authentication
{
    public class AccountDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        //[PasswordPropertyText]
        public string Password { get; set; }

       //// [Required]
       //// [EnumDataType(typeof(Role))]
       // [JsonIgnore]
       // public string Role { get; set; }
    }
   
}
