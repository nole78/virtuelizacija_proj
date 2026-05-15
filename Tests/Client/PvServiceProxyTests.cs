using Client.Proxy;
using NUnit.Framework;
using System;
using Moq;
using Common;
using System.ServiceModel;
using Common.PvDataContracts;

namespace Tests.Client
{
    [TestFixture]
    public class PvServiceProxyTests
    {
        private PvServiceProxy _pvServiceProxy;
        private Mock<IPvDataService> _ipvDataServiceMock;
        [SetUp]
        public void Setup()
        {
            _ipvDataServiceMock = new Mock<IPvDataService>();
            _ipvDataServiceMock.As<IClientChannel>();
            _pvServiceProxy = new PvServiceProxy(_ipvDataServiceMock.Object);
        }

        [Test]
        public void PvServiceProxy_ShouldThrowObjectDisposedException_WhenDisposed()
        {
            // Arrange
            _pvServiceProxy.Dispose();

            // Act
            TestDelegate act1 = () => _pvServiceProxy.StartSession(new PvMeta());
            TestDelegate act2 = () => _pvServiceProxy.PushSample(new PvSample());
            TestDelegate act3 = () => _pvServiceProxy.EndSession();

            // Assert
            Assert.Throws<ObjectDisposedException>(act1);
            Assert.Throws<ObjectDisposedException>(act2);
            Assert.Throws<ObjectDisposedException>(act3);
        }

        [Test]
        public void ServiceProxy_DisposeTwice_ShouldNotThrowObjectDisposedException()
        {
            // Arrange
            _pvServiceProxy.Dispose();

            // Act
            TestDelegate act = () => _pvServiceProxy.Dispose();

            // Assert
            Assert.DoesNotThrow(act);
        }

    }
}
