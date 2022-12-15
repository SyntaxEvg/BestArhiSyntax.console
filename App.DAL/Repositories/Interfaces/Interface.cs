using App.DDD.Domain.Models;
using Interfaces.Base.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Что это?  это сделно для того чтобы не писать много передаваемые параметры,generic,
/// тут  в примере их 3 а может  быть больше, поэтому создается абстракция New Interface,
/// который в будещем уже реализуется без передачи параметров
/// </summary>
namespace App.DAL.Repositories.Interfaces
{

    public interface ICountryRepository : IRepositoryBaseAsync<Employees, Guid, AppDBContext> { } //через запятую  можно еще без труда внедрить интерфейсы
    public interface IEmployeesRepository : IRepositoryBaseAsync<Employees, Guid, AppDBContext> { }
    public interface IEmployeesRepository1 : IRepositoryBaseAsync<Employees, Guid, AppDBContext> { }
    public interface IEmployeesRepository2 : IRepositoryBaseAsync<Employees, Guid, AppDBContext> { }
}
