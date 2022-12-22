using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DDD.Domain.Base.Security.JWT.Model
{
    /// <summary>
    /// Используется в главном проекте для Ioption
    /// </summary>
    public class JwtToken
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public TimeSpan TokenLifeTime { get; set; } //= "3600";
        /// <summary>
        /// Время жизни обновл.токена
        /// </summary>
        public int RefreshTokenValidityInDays { get; set; } = 7;
      //      "TokenValidityInMinutes": 1,
  //  "RefreshTokenValidityInDays": 7
    }
}
