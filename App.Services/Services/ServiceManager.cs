using App.DAL.Repositories.Base;
using Interfaces.Base.Base;
using Interfaces.IServices;
using Microsoft.Extensions.Logging;

namespace App.Services.Services
{
    /// <summary>
    /// Менеджер Сервиса поможет  управлять эффективно памятью и отдавать только то что требуется из Менеджера
    /// </summary>
    public interface IServiceManager : IServiceManagerBase
    {
        ITestTransientService TestTransientService { get; }
        ITestScopedService TestScopedService { get; }
        ITestSingletonService TestSingletonService { get; }

    }

    //public sealed class ServiceManager : IServiceManager
    //{


    //    private readonly Lazy<ITestTransientService> _testTransientService;
    //    private readonly Lazy<ITestScopedService> _testScopedService;
    //    private readonly Lazy<ITestSingletonService> _testSingletonService;
    //    private readonly Lazy<IlangDictionaryScopedService> _ilangDictionaryScopedService;

    //    public ServiceManager(IRepositoryManager repositoryManager)
    //    {
    //        _testTransientService = new Lazy<ITestTransientService>(() => new
    //        TestTransientService(repositoryManager, logger));
    //        _testScopedService = new Lazy<ITestScopedService>(() => new
    //        TestScopedService(repositoryManager, logger));
    //        _testSingletonService = new Lazy<ITestSingletonService>(() => new
    //        TestSingletonService());
    //        _ilangDictionaryScopedService = new Lazy<IlangDictionaryScopedService>(() => new
    //        langDictionaryScopedService());
    //    }


    //    public ITestTransientService TestTransientService => _testTransientService.Value;

    //    public ITestScopedService TestScopedService => _testScopedService.Value;

    //    public ITestSingletonService TestSingletonService => _testSingletonService.Value;

    //    public IlangDictionaryScopedService IlangDictionary => _ilangDictionaryScopedService.Value;
    //}
}
