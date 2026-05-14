using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Storage
{
    public class RejectWriter : IDisposable
    {
        // TODO: Implement writing in rejects.csv
        private bool _disposed = false;

        public RejectWriter() { }
        ~RejectWriter()
        {
            Dispose(false);
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
            if(disposing)
            {
                // Dispose managed resources here
            }
            // Dispose unmanaged resources here
            _disposed = true;
        }
    }
}
