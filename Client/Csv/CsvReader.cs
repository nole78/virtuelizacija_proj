using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Csv
{
    public class CsvReader : IDisposable
    {
        private bool _disposed = false;
        private StreamReader _reader;
        public CsvReader(string filePath) 
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException("File path must be filled in", nameof(filePath));

            _reader = new StreamReader(filePath);
        }

        public string ReadLine()
        {
            return _reader.ReadLine();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                // Oslobodi managed resurse
                if (_reader != null)
                {
                    _reader.Dispose();
                    _reader = null;
                }
            }
            // Oslobodi unmanaged resurse
            _disposed = true;
        }
        ~CsvReader()
        {
            Dispose(false);
        }
    }
}
