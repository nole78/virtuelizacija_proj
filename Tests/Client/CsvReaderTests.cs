using NUnit.Framework;
using System;
using Client.Csv;
using System.IO;

namespace Tests.Client
{
    [TestFixture]
    public class CsvReaderTests
    {
        private CsvReader _csvReader;
        [SetUp]
        public void Setup()
        {
            string tempPath = Path.GetTempFileName();
            _csvReader = new CsvReader(tempPath);
        }

        [Test]
        public void PvServiceProxy_ShouldThrowObjectDisposedException_WhenDisposed()
        {
            // Arrange
            _csvReader.Dispose();

            // Act
            TestDelegate act = () => _csvReader.ReadLine();

            // Assert
            Assert.Throws<ObjectDisposedException>(act);
        }

        [Test]
        public void ServiceProxy_DisposeTwice_ShouldNotThrowObjectDisposedException()
        {
            // Arrange
            _csvReader.Dispose();

            // Act
            TestDelegate act = () => _csvReader.Dispose();

            // Assert
            Assert.DoesNotThrow(act);
        }
    }
}
