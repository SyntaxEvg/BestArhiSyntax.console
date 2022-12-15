namespace Interfaces.Base.Identity
{
    ///Старайтесь, насколько это возможно, не включать какую-либо бизнес-логику проверки внутри ваших контроллеров, контроллер предназначен только для запуска правильного действия, пока вы обрабатываете, вы можете делать все, что хотите. 


    /// <summary>
    /// Интерфейс Авторизации,просто наследуемся  и реазализуем методы контроллера,согласно параметрам?,TIActionResult -стандартный метод контроллера
    /// </summary>
    public interface IIdentityAuthController<DTOLogin, DTOregistration, DTOforgotpass, DTOreset, TIActionResult>
                                                                            where DTOforgotpass : class
                                                                            where DTOregistration : class
                                                                            where DTOreset : class
                                                                            where DTOLogin : class
    {
        /// <summary>
        /// Get Login запрос для отображения страницы
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        Task<TIActionResult> Login(string returnUrl);

        /// <summary>
        /// Post Передача формы авторизации Claim and jwt
        /// </summary>
        /// <param name="dTOLogin"></param>
        /// <returns></returns>
        Task<TIActionResult> Authenticate(DTOLogin dTOLogin);
        /// <summary>
        /// Иногда нужен для выдачи токена, при этом в системе быть не нужно, подходит для Web.Api,
        /// метод опасный и требует контроля в реализации,если не понимаете, как с ним работать, лучше используйте Authenticate
        /// </summary>
        /// <param name="dTOLogin"></param>
        /// <returns></returns>
        Task<TIActionResult> CreateToken(DTOLogin dTOLogin);

        /// <summary>
        /// Get Register запрос для отображения страницы
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        Task<TIActionResult> Register(string returnUrl);

        /// <summary>
        /// Post Registration from system
        /// </summary>
        /// <param name="dTOLogin"></param>
        /// <returns></returns>
        Task<TIActionResult> Register(DTOregistration dTOregistration);

        /// <summary>
        /// Get Login запрос для отображения страницы
        /// </summary>
        Task<TIActionResult> Logout();

        /// <summary>
        /// Get страница где можно сбросить пароль, запрос для отображения страницы
        /// </summary>
        Task<TIActionResult> ForgotPassword(string emailOrLoginOrPhone);
        /// <summary>
        /// Добавить атрибут Post,Забыли пароль
        /// </summary>
        /// 
        Task<TIActionResult> ForgotPassword(DTOforgotpass DTOforgotpass);
        /// <summary>
        /// Добавить атрибут Post, сброса пароля
        /// </summary>
        Task<TIActionResult> ResetPassword(DTOforgotpass DTOforgotpass);

    }

}
