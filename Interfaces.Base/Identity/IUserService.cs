using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Base.Identity
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TLoginDTO"></typeparam>
    /// <typeparam name="TRegDTO"></typeparam>
    /// <typeparam name="TResponseMessage">ResponseMessage возрат операции из  сервиса в контроллер</typeparam>
    public interface IUserService<TLoginDTO, TRegDTO, TResponseMessage,TModelToken, TResponseTokens>
    {
        Task<TResponseMessage> RegisterUserAsync(TRegDTO userForRegistration);
        /// <summary>
        /// Аунтификация по токены или любая другая claim...
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AuthAndValidateUserAsync(TLoginDTO loginDto);
        /// <summary>
        /// JWT token
        /// </summary>
        /// <returns></returns>
        Task<TResponseTokens> CreateTokenAsync();


        /// <summary>
        /// возвращает Claim из маркера доступа JWT с истекшим сроком действия
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>


      //  TResponseTokens GetSavedRefreshTokens(string username, string refreshToken);

        IEnumerable<TResponseMessage> GetAll();
        TResponseMessage GetById(TLoginDTO loginDto);

        Task<TResponseMessage> Update(TLoginDTO loginDTO);
        Task<TResponseMessage> Delete(TLoginDTO loginDTO);

        Task<IList<string?>> GetRoleAsync(TLoginDTO loginDTO);

        Task<TResponseMessage> RefreshToken(TModelToken tokenModel);
        Task<TResponseMessage> CancelRefreshToken(string username);
        // await _signInManager.SignOutAsync();

    }

}
