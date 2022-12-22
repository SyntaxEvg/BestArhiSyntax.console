using App.DAL.Repositories.Base;
using App.DDD.Domain.Base.Identity;
using App.DDD.Domain.Base.Identity.Model.DTO;
using App.DDD.Domain.Base.Security.JWT.Model;
using App.DDD.Domain.Models;
using App.DDD.Domain.Models.Authentication;
using Common.Util;
using Interfaces.Base.Base;
using Interfaces.Base.Identity;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite.Operation.Buffer;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace App.Services.Services
{
    public class UserManagerTransientService : IUserService<UserLoginDto, UserRegistrationDto, ResponseMessage, TokenModel, ResponseTokens>
    {
        private readonly JwtToken _appSettings;
        private readonly ILogger<UserManagerTransientService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IlangDictionaryScopedService _lang;
        private readonly IRepositoryManager _manager;
        private User? _user;

        public UserManagerTransientService(ILogger<UserManagerTransientService> logg,
                                           IOptions<JwtToken> options, UserManager<User> userManager,
                                           IlangDictionaryScopedService lang,
                                           IRepositoryManager manager)
        {
            _appSettings = options.Value;
            _logger = logg;
            _userManager = userManager;
            _lang = lang;
            _manager = manager;
        }


        public async Task<bool> AuthAndValidateUserAsync(UserLoginDto loginDto)
        {
            // тут подключение к бд и проверка юзера в бд дальше логика 
            _user = await _userManager.FindByEmailAsync(loginDto.Email);
            var result = _user != null && await _userManager.CheckPasswordAsync(_user, loginDto.Password);
            return result;
        }
        public async Task<ResponseMessage> RegisterUserAsync(UserRegistrationDto userForRegistration)
        {
            var user = new User
            {
                FirstName = userForRegistration.FirstName,
                LastName = userForRegistration.LastName,
                UserName = userForRegistration.UserName,
                Email = userForRegistration.Email,
                PhoneNumber = userForRegistration.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (result.Succeeded)
            {
                return new ResponseMessage(result.Succeeded, _lang.Get("welcome"));
            }
            var mess = string.Join(';', result.Errors.Select(x => x.Description));
            _logger.LogError(string.Join(';', result.Errors));
            return new ResponseMessage(result.Succeeded, _lang.Get("welcome") + ";" + mess);


        }
       

        public async Task<ResponseTokens> CreateTokenAsync()
        {
            return await generateJwtToken();
        }

        public Task<ResponseMessage> Delete(UserLoginDto loginDTO)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ResponseMessage> GetAll()
        {
            throw new NotImplementedException();
        }

        public ResponseMessage GetById(UserLoginDto loginDto)
        {
            throw new NotImplementedException();
        }
        public Task<ResponseMessage> Update(UserLoginDto loginDTO)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Если потребуется запросить с контроллера роль
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IList<string?>> GetRoleAsync(UserLoginDto user = null)
        {
            if (user is not null)
            {
                _user = new User
                {
                    UserName = user.UserName,
                    Email = user.Email,
                };

            }
            var roles = await _userManager.GetRolesAsync(_user);//поиск этого  человека в системе и добавление роли в токен
            return roles;
        }
        private async Task<ClaimsIdentity> CreateClaims(IEnumerable<Claim> claims = null)
        {
            List<Claim> claimsRole = new();

            foreach (var role in await GetRoleAsync())
            {
                claimsRole.Add(new Claim(ClaimTypes.Role, role));
            }

            if (claimsRole.Count == 0)
            {
                claimsRole.Add(new Claim(ClaimTypes.Role, "User"));
            }

            var ClaimsIdentity = new ClaimsIdentity(new[]
            {
                            new Claim("Id", Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Name, _user.UserName),
                            new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            });
            ClaimsIdentity.AddClaims(claimsRole);
            return ClaimsIdentity;
        }


        /// <summary>
        /// IsRefreshToken- обновление токена
        /// </summary>
        /// <param name="user"></param>
        /// <param name="IsRefreshToken"></param>
        /// <returns></returns>
        private async Task<ResponseTokens> generateJwtToken(User user = null,bool IsRefreshToken =true)
        {
            var issuer = _appSettings.Issuer;
            var audience = _appSettings.Audience;
            var key = Encoding.ASCII.GetBytes(_appSettings.Key);
            var TokenLifeTime = _appSettings.TokenLifeTime;
            if (user is not null)
            {
                _user = user;
            }
            
            // TimeSpan.TryParse(_appSettings.TokenLifeTime, out var TokenLifeTime = _appSettings.TokenLifeTime);
            ClaimsIdentity claimsIdentity = await CreateClaims();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.Add(TokenLifeTime),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                                    (new SymmetricSecurityKey(key),
                                    SecurityAlgorithms.HmacSha512)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = GenerateRefreshToken();
            var userT = await _userManager.FindByEmailAsync(user is null ? _user.Email :  user.Email);
            if (IsRefreshToken)
            {
                userT.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_appSettings.RefreshTokenValidityInDays);
            }
           
            userT.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(userT);

            var tokenAccessRefresh = new ResponseTokens()
            {
                Access_Token = tokenHandler.WriteToken(token),
                Refresh_Token = refreshToken,
                Expiration = token.ValidTo,
            };
            return tokenAccessRefresh;
           
        }



        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }


        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(_appSettings.Key);
            var TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = _appSettings.Issuer,
                ValidAudience = _appSettings.Audience,// то чем проверяем ...
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ValidateIssuer = true, //указываем что мы проверяем издателя
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,//это всегда TRUE
                ValidateLifetime = false,//при сбросе токена,нужно отключить валидацию по времени,иначе будет  ошибка и замена токена не произойдет
                //ValidateLifetime = true,//проверяется время существования во время проверки токена
                //LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken,
                //                        TokenValidationParameters validationParameters) =>
                //                         {
                //                             return notBefore <= DateTime.UtcNow &&
                //                                    expires > DateTime.UtcNow;
                //                         }
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, TokenValidationParameters, out SecurityToken securityToken);
                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch (Exception)
            {

                throw;
            }
            return null;
            
        }

        //.....................................................................................
        public async Task<ResponseMessage> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                //return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;
            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
               // return BadRequest("Invalid access token or refresh token");
            }
            string username = principal!.Identity!.Name!;
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                //return BadRequest("Invalid access token or refresh token");
                return null;
            }
            var tok = await generateJwtToken(user, false);
            return new ResponseMessage()
            {
                Success = true,
                Message =null,
                Body = tok,
            };
           


        }

        public async Task<ResponseMessage> CancelRefreshToken(string username)
        {
            if (username is null)
            {
                //если имя не указана отзывает  все токены у юзеров (админский параметр) потом только вход через емейл и пароль также на такие методы нужен семафор
                try
                {
                    var users = _userManager.Users.Where(u=> u.RefreshToken != null).ToList();
                    foreach (var user in users)
                    {
                        user.RefreshToken = null;
                        await _userManager.UpdateAsync(user);
                    }
                    return new ResponseMessage(true, "All users RefreshToken = null");
                }
                catch (Exception ex)
                {
                    return new ResponseMessage(true, ex.Message);
                }

            }
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                    return new ResponseMessage(false, "Invalid user name");
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
                return new ResponseMessage(false, "User user RefreshToken");
            }
            
        }
    }
    }
