using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Csv
{
    public class CsvReader : IDisposable
    {
        // TODO: Implementirati 
        private bool _disposed = false;

        CsvReader() {}
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
