using App.DAL;
using App.DAL.Repositories;
using App.DAL.Repositories.Base;
using App.DDD.Domain.Models;
using Common.Localization;
using Common.Util;
using Interfaces.Base.Base;
using Interfaces.IServices;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Services
{


    public class langDictionaryScopedService : IlangDictionaryScopedService
    {
        public string _culture { get; set; }

        public langDictionaryScopedService()
        {
        }

        public void Create(string culture)
        {
            _culture = culture is not null && culture.Length > 1 ? culture : "ru-RU";
        }
        public string Get(string value)
        {
            _culture = _culture is null ? "" : _culture;
            var r = ConcurrentDictionaryLang._Lang.TryGetValue(_culture, out var dictLang);//.TryGetValue(culture, out var lang);
            var lang = "";
            dictLang?.TryGetValue(value, out lang);
            return lang ?? "";
        }

        //public IStringLocalizer edit(string baseName, string location)
        //{
        //    return new JsonStringLocalizer(_distributedCache);
        //}
    }


    /// <summary>
    /// Для Автоматической регистрации в DI укажите Суфикс, 
    /// Transient,Scoped,Singleton
    /// ,в нужном Методе scope,Tran, sing...
    /// Singleton</summary>
    public class TestTransientService : ITestTransientService
    {
        private readonly IRepositoryManager entityRepositoriesAll;

        //private readonly IRepositoryAsync<Employees, Guid> entityRepositoriesAll;
        private readonly ILogger<TestTransientService> logger;

        //public TestTransientService(IRepositoryAsync<Employees, Guid> entityRepositoriesAll, ILogger<TestTransientService> logger)
        public TestTransientService(IRepositoryManager entityRepositoriesAll, ILogger<TestTransientService> logger)
        {
            this.entityRepositoriesAll = entityRepositoriesAll;
            this.logger = logger;
        }
        public void Test()
        {
            entityRepositoriesAll.employeesRepository.AddAsync(new Employees());
            //Console.WriteLine("HI DI Service Transient");
        }
        //public void Test()
        //{
        //    throw new NotImplementedException();
        //}
    }
    public class TestScopedService : IBaseServiceAsync<Orders, ResponseMessage, Guid>
    {
        private readonly AppDBContext db;

        public TestScopedService(AppDBContext db)
        {
            this.db = db;
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            throw new NotImplementedException();
        }

        public Task Commit()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseMessage> CreateAsync(Orders dto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseMessage> DeleteAsync(Guid dto)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Orders>> FIndByFilterAsync(IEnumerable<Orders> dto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Orders>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Orders> GetByIdAsync(Orders dto)
        {
            throw new NotImplementedException();
        }

        public Task Rollback()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseMessage> UpdateAsync(Orders dto)
        {
            throw new NotImplementedException();
        }
    }

    public class TestSingletonService : ITestSingletonService
    {
        //private readonly IRepositoryAsync<Employees, Guid> entityRepositoriesAll;
        //private readonly Logger<TestSingletonService> logger;

        //public TestSingletonService(IRepositoryAsync<Employees, Guid> entityRepositoriesAll, Logger<TestSingletonService> logger)
        //{
        //    this.entityRepositoriesAll = entityRepositoriesAll;
        //    this.logger = logger;
        //}
        //public void Test()
        //{
        //    entityRepositoriesAll.AddAsync(new Employees());
        //    Console.WriteLine("HI DI Service Singleton");
        //}
        public void Test()
        {
            throw new NotImplementedException();
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
