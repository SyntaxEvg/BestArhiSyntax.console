using App.DAL.Repositories.Interfaces;
using App.DDD.Domain.Base.Identity;
using App.DDD.Domain.Models;
using Interfaces.Base.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Repositories.Base
{
    /// <summary>
    /// В менеджер ,желательно,складывать, интерфейсы,которые вы будите использовать в сервисах
    /// </summary>
    public interface IRepositoryManager
    {
        IEmployeesRepository employeesRepository { get; }
        IEmployeesRepository1 employeesRepository1 { get; }
        IEmployeesRepository2 employeesRepository2 { get; }

        public Task<int> SaveChangesAsync();

    }


}
