using App.DDD.Domain.Base.Identity;
using App.DDD.Domain.Base.Identity.Model.DTO;
using App.DDD.Domain.Models;
using App.DDD.Domain.Models.Authentication;
using App.Web.Api.Controllers.Base;
using Common.Util;
using Interfaces.Base.Base;
using Interfaces.Base.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[RoutePrefix]
    //[Authorize]
    [AllowAnonymous]
    public class AuthController : ControllerBase, IIdentityAuthController<UserLoginDto, UserRegistrationDto, UserLoginDto, UserLoginDto, IActionResult>
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService<UserLoginDto, UserRegistrationDto, ResponseMessage, TokenModel,ResponseTokens> _userService;
        private readonly IlangDictionaryScopedService _lang;

        public AuthController(ILogger<AuthController> logger, 
            IUserService<UserLoginDto, UserRegistrationDto, ResponseMessage, TokenModel, ResponseTokens> userService,
            IlangDictionaryScopedService lang)
        {
            this._logger = logger;
            this._userService = userService;
            this._lang = lang;
        }
        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> Authenticate(UserLoginDto dTOLogin)
        {
            return !await _userService.AuthAndValidateUserAsync(dTOLogin)
           ? Unauthorized()
           : Ok(new { Token = await _userService.CreateTokenAsync() });
        }
    
        [HttpGet]
        [Route("Register")]
        public Task<IActionResult> Register(string returnUrl)
        {
            throw new NotImplementedException();
        }
       
        /// <summary>
        /// После регистрация необходимо авторизоваться
        /// </summary>
        /// <param name="dTOregistration"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
      //  [ValidateModelAttribute]
        public async Task<IActionResult> Register(UserRegistrationDto dTOregistration)
        {
            var userResult = await _userService.RegisterUserAsync(dTOregistration);
            return !userResult.Success ? new BadRequestObjectResult(userResult) : StatusCode(201);
        }
        [HttpGet]
        [Route("ForgotPassword")]
        public Task<IActionResult> ForgotPassword(string returnUrl)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        [Route("ForgotPassword")]
        public Task<IActionResult> ForgotPassword(UserLoginDto DTOforgotpass)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        [Route("Login")]
        public Task<IActionResult> Login(string returnUrl)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        [Route("Logout")]
        public Task<IActionResult> Logout()
        {
            throw new NotImplementedException();
        }


        [HttpPost]
        [Route("ResetPassword")]
        public Task<IActionResult> ResetPassword(UserLoginDto DTOforgotpass)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Метод испоьзуется когда JWT токен устарел,и нужна получить новый для использования Авторизованного Api
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(TokenModel token)
        {
            var userResult = await _userService.RefreshToken(token);// tokenModel)(dTOregistration);

            return Ok(userResult.Body);
        }


        /// <summary>
        /// Админский параметр, сбрасывает один или все ТокеныОбновления
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost]
        [Route("cancelRefreshToken")]
        public async Task<IActionResult> CancelRefreshToken(string username = "null")
        {
            if (username == "null") //костыль для свагера, который не умеет рабоать с опцинальным параметром
            {
                username = null;
            }
            var userResult = await _userService.CancelRefreshToken(username);// tokenModel)(dTOregistration);
            return Ok(userResult);
          

            return NoContent();
        }
    }



   
}
