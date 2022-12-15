using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Util
{
    public record ResponseMessage
    {
        public ResponseMessage(bool success, string message =null)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; init; } = false;
        public string Message { get; init; }
        public override string ToString() => JsonSerializer.Serialize(this);

    }
}
