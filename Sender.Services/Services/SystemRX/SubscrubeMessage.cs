using App.DDD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Sender.Services.Services.SystemRX
{
    /// <summary>
    /// Тут находяться все подписки
    /// </summary>
    public static class SubscrubeMessage
    {
        public readonly static Subject<CommandMessageRequest> subjectMail = new();
    }
}
