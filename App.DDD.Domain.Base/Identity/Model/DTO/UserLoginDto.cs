using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DDD.Domain.Base.Identity.Model.DTO
{
    public record UserLoginDto
    {
       // [Required(ErrorMessage = "Username is required")]

        public string? UserName { get; init; }
        public string? Email { get; init; }

      //  [Required(ErrorMessage = "Password is required")]
        public string? Password { get; init; }
    }

}
