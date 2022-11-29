using DanLiris.Admin.Web;
using ExtCore.Data.Abstractions;
using Infrastructure;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.AzureUtility;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;

namespace Barebone.Tests
{
    public abstract class BaseControllerUnitTest : IDisposable
    {
        protected readonly MockRepository _MockRepository;
        protected readonly Mock<IServiceProvider> _MockServiceProvider;
        protected readonly Mock<IStorage> _MockStorage;
        protected readonly Mock<IMediator> _MockMediator;
        protected readonly Mock<IAzureImage> _MockAzureImage;
        protected readonly Mock<IAzureDocument> _MockAzureDocument;

        protected BaseControllerUnitTest()
        {
            this._MockRepository = new MockRepository(MockBehavior.Strict);
            _MockMediator = _MockRepository.Create<IMediator>();
            _MockStorage = _MockRepository.Create<IStorage>();
            this._MockServiceProvider = this._MockRepository.Create<IServiceProvider>();
            this._MockAzureDocument = this._MockRepository.Create<IAzureDocument>();
            this._MockAzureImage = this._MockRepository.Create<IAzureImage>();
            _MockServiceProvider.Setup(s => s.GetService(typeof(IHttpClientService))).Returns(new HttpClientTestService());
            _MockServiceProvider.Setup(s => s.GetService(typeof(IStorage))).Returns(_MockStorage.Object);
            _MockServiceProvider.Setup(s => s.GetService(typeof(IWebApiContext))).Returns(new WorkContext());
            _MockServiceProvider.Setup(s => s.GetService(typeof(IMediator))).Returns(_MockMediator.Object);

        }

        public Mock<TAbstract> CreateMock<TAbstract>() where TAbstract : class
        {
            return _MockRepository.Create<TAbstract>();
        }

        protected int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        public void Dispose()
        {
            this._MockRepository.VerifyAll();
        }
    }
}