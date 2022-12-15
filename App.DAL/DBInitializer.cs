using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.DAL
{
    /// <summary>
    /// Сервис который принимает contextBD
    /// </summary>
    public class DBInitializer
    {
        private readonly AppDBContext _db;
        private readonly ILogger<DBInitializer> logger;

        public DBInitializer(AppDBContext appDB,ILogger<DBInitializer> logger)
        {
            this._db = appDB;
            this.logger = logger;
        }


        public async Task<bool> RemoveAsync(CancellationToken token = default)
        {
            if (await _db.Database.EnsureDeletedAsync(token).ConfigureAwait(false))
            {
                logger.LogInformation("BD DELETE");
                return true;
            }
            logger.LogError("BD NO DELETE? Empty DB");
            return true;

        }
        /// <summary>
        /// removeBD -по умолчанию False(True полезно для отладки)
        /// </summary>
        /// <param name="removeBD">конфиг True ,если требуется удалить бд !осторожно с этим параметром</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task InitializationAsync(bool removeBD =false,CancellationToken token = default)
        {
            if (removeBD)
            {
                await RemoveAsync(token).ConfigureAwait(false);
            }
            //накатываем миграции Автоматом применит все настройки 
            await _db.Database.MigrateAsync();
            //await AddInitialData(); Если класс закрытый прописываем при инициализации если нет. создать интрефейс и работать где надо...
        }

        private async Task AddInitialData(CancellationToken token = default)
        {
            if (!await _db.Employees.AnyAsync(token).ConfigureAwait(false))
            {
                // Если данный нет, заполняем их начальными данными, также пройтись по всем нужным сущностям по образцу _db.Employees
                //await _db.SaveChangesAsync();
            }
        }


    }
}