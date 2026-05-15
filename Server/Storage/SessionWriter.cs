using Common.PvDataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Storage
{
    public class SessionWriter : IDisposable
    {
        // TODO: Napraviti tako da radi upis u odgovarajuci folder
        private bool _disposed = false;
        private FileStream _fileStream;
        public SessionWriter(string path) 
        {
            _fileStream = new FileStream(path, FileMode.Append, FileAccess.Write);
        }
        public void WriteRow(PvSample sample)
        {
            // TODO: Napraviti tako da se upisuju i ostali podaci, ne samo AcPwrt
            if (_disposed)
                throw new ObjectDisposedException(nameof(SessionWriter), "Pokušaj pisanja u već zatvoreni SessionWriter.");
            string line = $"{sample.Day:yyyy-MM-dd},{sample.Hour},{sample.AcPwrt}\n";
            byte[] lineInBytes = new UTF8Encoding().GetBytes(line + Environment.NewLine);
            _fileStream.Write(lineInBytes, 0, lineInBytes.Length);
        }
        ~SessionWriter()
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
            if (disposing)
            {
                // Oslobodi managed resurse
                if (_fileStream != null)
                {
                    _fileStream.Flush();
                    _fileStream.Dispose();
                    _fileStream = null;
                }
            }
            // Oslobodi unmanaged resurse
            _disposed = true;
        }
    }
}
