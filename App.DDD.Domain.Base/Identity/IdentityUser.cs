
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
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

        [Column("RefreshToken")]
        public string? RefreshToken { get; set; }

      //  [Column(TypeName = "datetime2")]
        public DateTime RefreshTokenExpiryTime { get; set; }

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

    //[Table("ApplicationSessions")]
    //public class UsersSession
    //{
    //    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public Guid SessionId { get; set; }

    //    [Required]
    //    [StringLength(384)]
    //    public string SessionToken { get; set; }

    //    [ForeignKey(nameof(User))]
    //    public int UserId { get; set; }

    //    [Column(TypeName = "datetime2")]
    //    public DateTime TimeCreated { get; set; }

    //    [Column(TypeName = "datetime2")]
    //    public DateTime TimeLastRequest { get; set; }

    //    public bool IsClosed { get; set; }

    //    [Column(TypeName = "datetime2")]
    //    public DateTime? TimeClosed { get; set; }

    //    public virtual User User { get; set; }
    //}
    //[Table("ApplicationTokens")]
    //public class Tokens
    //{
    //    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public Guid TokensId { get; set; }
    //    public string Access_Token { get; set; }
    //    public string Refresh_Token { get; set; }
    //}
    //[Table("UserRefreshTokens")]
    //public class UserRefreshTokens //сохраняет токены обновления для действительных пользователей. 
    //{
    //    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public Guid Id { get; set; }
    //    [Required]
    //    public string UserName { get; set; }
    //    [Required]
    //    public string RefreshToken { get; set; }
    //    public bool IsActive { get; set; } = true;
    //}
}
