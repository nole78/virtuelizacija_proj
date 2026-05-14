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
        SessionWriter() {
            _fileStream = new FileStream("sessions.csv", FileMode.Append, FileAccess.Write);
        }
        public void WriteRow(PvSample sample)
        {
            // TODO: Napraviti tako da se upisuju i ostali podaci, ne samo AcPwrt
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
            throw new NotImplementedException();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                // Oslobodi managed resurse
                _fileStream.Flush();
                _fileStream.Dispose();
                _fileStream = null;
            }
            // Oslobodi unmanaged resurse
            _disposed = true;
        }
    }
}
