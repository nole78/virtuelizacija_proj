using Client.Logging;
using NUnit.Framework;
using System;
using System.IO;

namespace Tests.Client
{
    [TestFixture]
    public class ClientLoggerTests
    {
        private ClientLogger _clientLogger;
        [SetUp]
        public void Setup()
        {
            string tempPath = Path.GetTempFileName();
            _clientLogger = new ClientLogger(tempPath);
        }

        [Test]
        public void PvServiceProxy_ShouldThrowObjectDisposedException_WhenDisposed()
        {
            // Arrange
            _clientLogger.Dispose();

            // Act
            TestDelegate act = () => _clientLogger.Log("Test");

            // Assert
            Assert.Throws<ObjectDisposedException>(act);
        }

        [Test]
        public void ServiceProxy_DisposeTwice_ShouldNotThrowObjectDisposedException()
        {
            // Arrange
            _clientLogger.Dispose();

            // Act
            TestDelegate act = () => _clientLogger.Dispose();

            // Assert
            Assert.DoesNotThrow(act);
        }
    }
}
