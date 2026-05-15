using Common.PvDataContracts;
using NUnit.Framework;
using Server.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Server
{
    [TestFixture]
    public class SessionWriterTests
    {
        private SessionWriter _sessionWriter;
        [SetUp]
        public void Setup()
        {
            string tempPath = Path.GetTempFileName();
            _sessionWriter = new SessionWriter(tempPath);
        }

        [Test]
        public void PvServiceProxy_ShouldThrowObjectDisposedException_WhenDisposed()
        {
            // Arrange
            _sessionWriter.Dispose();

            // Act
            TestDelegate act = () => _sessionWriter.WriteRow(new PvSample());

            // Assert
            Assert.Throws<ObjectDisposedException>(act);
        }

        [Test]
        public void ServiceProxy_DisposeTwice_ShouldNotThrowObjectDisposedException()
        {
            // Arrange
            _sessionWriter.Dispose();

            // Act
            TestDelegate act = () => _sessionWriter.Dispose();

            // Assert
            Assert.DoesNotThrow(act);
        }
    }
}
