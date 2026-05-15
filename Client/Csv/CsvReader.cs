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
        public CsvReader(string path) 
        {
            // TODO: premini fajl, stavi da se uzima iz parametra konstuktora
            _reader = new StreamReader(path);
        }

        public string ReadLine()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(CsvReader), "Pokušaj citanje iz već zatvorenog CsvReader.");
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
