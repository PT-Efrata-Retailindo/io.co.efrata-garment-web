using ExtCore.Data.Abstractions;
using Moq;
using System;

namespace Barebone.Tests
{
    public abstract class BaseCommandUnitTest : IDisposable
    {
        protected readonly MockRepository _MockRepository;

        protected readonly Mock<IStorage> _MockStorage;
        protected readonly Mock<IServiceProvider> _MockServiceProvider;

        public BaseCommandUnitTest()
        {
            this._MockRepository = new MockRepository(MockBehavior.Strict);
            _MockStorage = _MockRepository.Create<IStorage>();
            _MockServiceProvider = new Mock<IServiceProvider>();

        }

        public Mock<TAbstract> CreateMock<TAbstract>() where TAbstract : class
        {
            return _MockRepository.Create<TAbstract>();
        }

        public void Dispose()
        {
            this._MockRepository.VerifyAll();
        }
    }

    public static class MoqTestExtensions
    {
        public static void SetupStorage<TRepository>(this Mock<IStorage> mockStorage, Mock<TRepository> mockRepository) where TRepository : class, IRepository
        {
            mockStorage.Setup(x => x.GetRepository<TRepository>()).Returns(mockRepository.Object);

        }
    }
}