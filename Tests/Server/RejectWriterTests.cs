using NUnit.Framework;
using Server.Storage;
using System;
using Common.PvDataContracts;
using System.IO;

namespace Tests.Server
{
    [TestFixture]
    public class RejectWriterTests
    {
        private RejectWriter _rejectWriter;
        [SetUp]
        public void Setup()
        {
            string tempPath = Path.GetTempFileName();
            _rejectWriter = new RejectWriter(tempPath);
        }

        [Test]
        public void PvServiceProxy_ShouldThrowObjectDisposedException_WhenDisposed()
        {
            // Arrange
            _rejectWriter.Dispose();

            // Act
            TestDelegate act = () => _rejectWriter.WriteRow(new PvSample(),"test");

            // Assert
            Assert.Throws<ObjectDisposedException>(act);
        }

        [Test]
        public void ServiceProxy_DisposeTwice_ShouldNotThrowObjectDisposedException()
        {
            // Arrange
            _rejectWriter.Dispose();

            // Act
            TestDelegate act = () => _rejectWriter.Dispose();

            // Assert
            Assert.DoesNotThrow(act);
        }
    }
}
