using App.DDD.Domain.Base.Identity;
using App.DDD.Domain.Base.Identity.Model.DTO;
using App.DDD.Domain.Base.Security.JWT.Model;
using App.DDD.Domain.Models.Authentication;
using Common.Util;
using Interfaces.Base.Base;
using Interfaces.Base.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite.Operation.Buffer;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.Services.Services
{
    public class UserManagerTransientService : IUserService<UserLoginDto, UserRegistrationDto, ResponseMessage>
    {
        private readonly JwtToken _appSettings;
        private readonly ILogger<UserManagerTransientService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IlangDictionaryScopedService _lang;
        private User? _user;

        public UserManagerTransientService(ILogger<UserManagerTransientService> logg,
                                           IOptions<JwtToken> options, UserManager<User> userManager,
                                           IlangDictionaryScopedService lang)
        {
            _appSettings = options.Value;
            _logger = logg;
            _userManager = userManager;
            _lang = lang;
        }
        public async Task<bool> AuthAndValidateUserAsync(UserLoginDto loginDto)
        {
            // тут подключение к бд и проверка юзера в бд дальше логика 
            // authentication successful so generate jwt token
            // var token = generateJwtToken(model);
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
            var mess = string.Join(';', result.Errors.Select(x=>x.Description));
            _logger.LogError(string.Join(';', result.Errors));
            return new ResponseMessage(result.Succeeded, _lang.Get("welcome") +";" +mess);


        }
        public async Task<ResponseMessage> CreateTokenAsync()
        {
            return new ResponseMessage(true, await generateJwtToken());
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


        //public AuthenticateResponse Authenticate(AuthenticateRequest model)
        //{

        //    var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

        //    // return null if user not found
        //    if (user == null) return null;

        //    // authentication successful so generate jwt token
        //    var token = generateJwtToken(user);

        //    return new AuthenticateResponse(user, token);
        //}
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
                            new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            });
            ClaimsIdentity.AddClaims(claimsRole);
            return ClaimsIdentity;
        }

        private async Task<string> generateJwtToken(User user = null)
        {

            //if (user.Email == "joydip" && user.Password == "joydip123")
            //{
            var issuer = _appSettings.Issuer;
            var audience = _appSettings.Audience;
            var key = Encoding.ASCII.GetBytes(_appSettings.Key);
            int Time = 100;
            int.TryParse(_appSettings.TokenLifeTime, out Time);
            ClaimsIdentity claimsIdentity = await CreateClaims();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(Time),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
            // return Ok(stringToken);
            //}
            //return Unauthorized();
        }
    }
}
