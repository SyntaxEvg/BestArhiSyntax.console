using Interfaces.Base.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Localization
{
    /// <summary>
    /// Extension метод для LocalizerMiddleware
    /// </summary>
    public static class LocalizationMiddlewareExtensions
    {
        public static IApplicationBuilder LocalizationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LocalizerMiddleware>();
        }
    }
    public class LocalizerMiddleware : IMiddleware
    {
        private readonly IlangDictionaryScopedService _lang;
        public LocalizerMiddleware(IlangDictionaryScopedService lang)
        {
            this._lang = lang;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //var cultureKey = context.Request.Headers["Accept-Language"];
            var req =context.Request;
            string cultureKey = null;
            //var token = await HttpContext.GetTokenAsync("access_token");//получить  jwt token
            if (req.Method == "GET")
            {
                cultureKey = context.Request.Query["lang"];
                _lang.Create(cultureKey);
               //context.Items.Add(cultureKey);//.Append(cultureKey);
            }
            else if (req.Method == "POST")
            {
                cultureKey = context.Request.Query["lang"];
                _lang.Create(cultureKey);
            }
           // var t = Thread.CurrentThread.CurrentCulture;
            _lang.Create("ru-RU");
            //if (!string.IsNullOrEmpty(cultureKey))
            //{
            //    if (DoesCultureExist(cultureKey))
            //    {
            //        var culture = new CultureInfo(cultureKey);
            //        // Set the culture in the current thread responsible for that request
            //        //Thread.CurrentThread.CurrentCulture = culture;
            //        //Thread.CurrentThread.CurrentUICulture = culture;
            //    }
            //}

            // Await the next request
            await next(context);
        }

        private static bool DoesCultureExist(string cultureName)
        {
            // Return the culture where the culture equals the culture name set
            return CultureInfo.GetCultures(CultureTypes.AllCultures).
                Any(culture => string.Equals(culture.Name, cultureName,
                                             StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
