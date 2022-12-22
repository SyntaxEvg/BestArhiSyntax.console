using App.DDD.Domain.Models;
using Interfaces.Base.Base;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using System.Security.Cryptography;
using App.DAL.Repositories.Base;
using App.DAL;
using Microsoft.AspNetCore.Identity;
using App.DAL.Repositories.Interfaces;
using App.DDD.Domain.Base.Identity;

namespace App.DAL.Repositories
{
    /// <summary>
    /// Менеджер репозитория, Автоматически рег. в DI указав суфикс
    /// </summary>
    public class ManagerScopedRepository : IRepositoryManager
    {
        private readonly AppDBContext _db;
        private EmployeesRepository _employeesRepository;
        private EmployeesRepository _departmentRepository;
        private EmployeesRepository _studentRepository;
        //UserManager<User> _userManager;

        public ManagerScopedRepository(AppDBContext db)
        {
            _db = db;
        }
        public AppDBContext GetDb()
        {
            return _db;
        }
        public IEmployeesRepository employeesRepository
        {
            get
            {
                if (_employeesRepository == null)
                    _employeesRepository = new EmployeesRepository(_db);
                return _employeesRepository;
            }
        }

        public IEmployeesRepository1 employeesRepository1
        {
            get
            {
                if (_departmentRepository == null)
                    _departmentRepository = new EmployeesRepository(_db);
                return _departmentRepository;
            }
        }

        public IEmployeesRepository2 employeesRepository2
        {
            get
            {
                if (_studentRepository == null)
                    _studentRepository = new EmployeesRepository(_db);
                return _studentRepository;
            }
        }

        public Task<int> SaveChangesAsync()
        {

            return _db.SaveChangesAsync();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            _db.Dispose();
        }

      
    }

    public class EmployeesRepository : IEmployeesRepository, IEmployeesRepository1, IEmployeesRepository2
    {
        private readonly AppDBContext _db;

        public EmployeesRepository(AppDBContext db)
        {
            _db = db;
        }
        public async Task<Employees?> AddAsync(Employees? entity, CancellationToken token = default)
        {
            var test = new Employees() { LastName = "Hi" };
            return test;
            //throw new NotImplementedException();
        }

        public Task<bool> AddRangeAsync(IEnumerable<Employees?> entity, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<Employees?> DeleteAsync(Employees? entity, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task Execute(string command, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Employees?>> FillterAllAsync(Expression<Func<Employees, bool>> expression, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<Employees?> FillterSingleAsync(Expression<Func<Employees?, bool>> expression, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<Employees?> FindAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Employees>> GetAllAsync(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<Employees?> GetByIdAsync(Employees TEntity, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public AppDBContext GetDbContext(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<Employees?> UpdateAsync(Employees? entity, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}