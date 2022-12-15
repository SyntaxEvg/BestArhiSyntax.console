using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.IServices
{
    public interface ITestTransientService
    {
        void Test();
    }
    public interface ITestScopedService
    {
        void Test();
    }
    public interface ITestSingletonService
    {
        void Test();
    }
}