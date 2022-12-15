﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    public interface IUserService<TLoginDTO, TRegDTO, TResponseMessage>
    {
        Task<TResponseMessage> RegisterUserAsync(TRegDTO userForRegistration);
        /// <summary>
        /// Аунтификация по токены или любая другая claim...
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AuthAndValidateUserAsync(TLoginDTO loginDto);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<TResponseMessage> CreateTokenAsync();
        IEnumerable<TResponseMessage> GetAll();
        TResponseMessage GetById(TLoginDTO loginDto);

        Task<TResponseMessage> Update(TLoginDTO loginDTO);
        Task<TResponseMessage> Delete(TLoginDTO loginDTO);

        Task<IList<string?>> GetRoleAsync(TLoginDTO loginDTO);

        // await _signInManager.SignOutAsync();

    }

}