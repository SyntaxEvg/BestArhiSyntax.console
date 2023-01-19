using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Util
{
    /// <summary>
    /// Испольуется в качестве возрата из сервера или Контроллера 
    /// ToString помогает возращать на фронт сразу json return IAction
    /// </summary>
    public record ResponseMessage
    {
        public ResponseMessage(bool success, string message = null, object? body =null)
        {
            Success = success;
            Message = message;
            Body = body;
        }
        public ResponseMessage()
        {

        }


        public bool Success { get; init; } = false;
        public string? Message { get; init; }
        /// <summary>
        /// Исользую чтобы вернуть с сервиса любой типна контроллер и вывести ответ
        /// </summary>
        public object? Body { get; init; } 
        public override string ToString() => JsonSerializer.Serialize(this);

    }
}
