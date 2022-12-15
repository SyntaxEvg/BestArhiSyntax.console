using Interfaces.Base.Base;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Api.Controllers.Base
{
    public class BaseController<T> : ControllerBase
    {
        public readonly ILogger<T> logger;
        public readonly IlangDictionaryScopedService _lang;

        public BaseController(ILogger<T> logger, IlangDictionaryScopedService lang)
        {
            this.logger = logger;
            this._lang = lang;
        }
    }
}
