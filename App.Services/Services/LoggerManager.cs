using Interfaces.Base.Base;
using Microsoft.Extensions.Logging;

namespace App.Services.Services
{
    ///
    ///Обертка для nlog
    ///
    //public class LoggerManager : ILoggerManager
    //{
    //    private static ILogger logger = LogManager.GetCurrentClassLogger();
    //    public LoggerManager() { }
    //    public void LogDebug(string message)
    //    {
    //        logger.Debug(message);
    //    }
    //    public void LogError(string message)
    //    {
    //        logger.Error(message);
    //    }
    //    public void LogInfo(string message)
    //    {
    //        logger.Info(message);
    //    }
    //    public void LogWarn(string message)
    //    {
    //        logger.Warn(message);
    //    }
    //}




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
