using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Base.Base
{
    /// <summary>
    /// Базавый интерфейс, содержит типичные методы для реализации в сервисах,
    /// TDTO - модуль которая пришла с контроллера,Tout выходной формат на контроллер,
    /// Tid - id для поиска элемента,
    /// чаще будет Guid, но иногда и Int
    /// </summary>
    public interface IBaseServiceAsync<TDTO,Tout,Tid>:IDisposable
    {
        Task<Tout> UpdateAsync(TDTO dto);
        Task<Tout> DeleteAsync(Tid dto);
        Task<Tout> CreateAsync(TDTO dto);
        Task<TDTO> GetByIdAsync(TDTO dto);
        Task<IEnumerable<TDTO>> GetAllAsync();
        Task<IEnumerable<TDTO>> FIndByFilterAsync(IEnumerable<TDTO> dto); //решить вопрос с переводом на бекенде 

        //Единая работа, прямо в репозитори 
        Task<IDbContextTransaction> BeginTransactionAsync();
        //{
        //return await _context.Database.BeginTransactionAsync(); //пример использования
        //}

        Task Commit();
        Task Rollback();
        Task<int> SaveChangesAsync();

    }
}
