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
        // TODO: Implementirati 
        private bool _disposed = false;
        private StreamReader _reader;
        CsvReader() 
        {
            // TODO: premini fajl, stavi da se uzima iz parametra konstuktora
            _reader = new StreamReader("data.csv");
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
                _reader.Dispose();
                _reader = null;
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
