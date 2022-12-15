using App.DDD.Domain.Models;
using Interfaces.Base.Base;
using MassTransit;
using MassTransit.Clients;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Services
{


    /// <summary>
    /// Отправляет сообщение, Указан суфикс, регистрация в DI не нужна!
    /// </summary>
    public class SendMessageScopedService : ISendMessageScopedService<CommandMessageRequest>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<CommandMessageResponse> _client;

        public string _culture { get; set; }

        public SendMessageScopedService(IPublishEndpoint publishEndpoint, IRequestClient<CommandMessageResponse> client)
        {
            _publishEndpoint = publishEndpoint;
            _client = client;
        }

        public async Task Send(CommandMessageRequest mes)
        {

           //var mes1 = await _client.GetResponse<CommandMessageRequest>(mes);
            
             await _publishEndpoint.Publish<CommandMessageRequest>(mes);
        }
       
        


}




    //public class TestService : EntityRepositoriesAll<Employees, Guid>
    //{
    //    public Test(AppDBContext DB) : base(DB)
    //    {
    //    }

    //    public override Employees? AddAsync(Employees? entity, CancellationToken token = default)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override Task<bool> AddRangeAsync(IEnumerable<Employees?> entity, CancellationToken token = default)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override Task<Employees?> DeleteAsync(Employees? entity, CancellationToken token = default)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override Task<IEnumerable<Employees?>> FillterAllAsync(Expression<Func<Employees, bool>> expression, CancellationToken token = default)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override Task<Employees?> FillterSingleAsync(Expression<Func<Employees?, bool>> expression, CancellationToken token = default)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async override Task<Employees?> FindAsync(Guid id, CancellationToken token = default)
    //    {
    //        return await _db.Set<Employees>().FirstOrDefaultAsync(x => x.ID == id);
    //    }

    //    public override Task<IEnumerable<Employees>> GetAllAsync(CancellationToken token = default)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override Task<Employees?> GetByIdAsync(Guid id, CancellationToken token = default)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override IQueryable<Employees?> QueryableAsync(CancellationToken token = default)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override Task<Employees?> UpdateAsync(Employees? entity, CancellationToken token = default)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
