using App.DDD.Domain.Base.Identity.Model.DTO;
using App.DDD.Domain.Models.Authentication;
using App.Web.Api.Controllers.Base;
using Common.Util;
using Interfaces.Base.Base;
using Interfaces.Base.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserService<UserLoginDto, UserRegistrationDto, ResponseMessage> _userService;
        private readonly IlangDictionaryScopedService _lang;

        public AuthController(ILogger<AuthController> logger, 
            IUserService<UserLoginDto, UserRegistrationDto, ResponseMessage> userService,
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
        [HttpPost]
        [Route("CreateToken")]
        public async Task<IActionResult> CreateToken(UserLoginDto dTOLogin)
        {
            return Ok(new { Token = await _userService.CreateTokenAsync() });
        }
        [HttpGet]
        [Route("Register")]
        public Task<IActionResult> Register(string returnUrl)
        {
            throw new NotImplementedException();
        }
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
    }



    //public class SecurityJWTController : BaseController<SecurityJWTController>
    //{
    //    public SecurityJWTController(ILogger<SecurityJWTController> logger, IlangDictionaryScopedService lang) : base(logger, lang) { }

    //    [AllowAnonymous]
    //    //[HttpPost(Name = "createToken")]
    //    [HttpPost]
    //    [Route("api/createToken")]
    //    public IActionResult CreateToken([FromServices] IUserService<AccountDTO, ResponseMessage> userServiceJWT, AccountDTO account )
    //    {
    //        var res = userServiceJWT.Authenticate(account);
    //        if (res.Success)
    //        {
    //            return Ok(res);
    //        }
    //        return Unauthorized();
    //    } 
        


        
    //    [HttpGet]
    //    [Route("api/get")]        
    //    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //    [Authorize(Roles = "Admin,User")]//User
    //    public IActionResult get()
    //    {
    //        return Ok("hi");
    //        //var res = userServiceJWT.Authenticate(account);
    //        //if (res.Success)
    //        //{
    //        //    return Ok(res);
    //        //}
    //        //return Unauthorized();
    //    }

    //}
}
