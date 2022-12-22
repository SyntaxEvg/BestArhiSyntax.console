using App.DDD.Domain.Models;
using MassTransit;

namespace App.Services.Services
{
    /// <summary>
    /// Сервис принимает сообщение 
    /// </summary>
    public class ReceiverMessageService : IConsumer<CommandMessageRequest>
    {
        int a = 0;

        public async Task Consume(ConsumeContext<CommandMessageRequest> context)
        {
            var message = context.Message;
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
