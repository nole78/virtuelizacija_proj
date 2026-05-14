using Common.PvDataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Storage
{
    public class RejectWriter : IDisposable
    {
        // TODO: Napraviti tako da radi upis u odgovarajuci folder
        private bool _disposed = false;
        private FileStream _fileStream;
        public RejectWriter() 
        {
            _fileStream = new FileStream("rejects.csv", FileMode.Append, FileAccess.Write);
        }
        public void WriteRow(PvSample sample, string reason)
        {
            // TODO: Napraviti tako da se upisuju i ostali podaci, ne samo AcPwrt
            string line = $"{reason},{sample.Day:yyyy-MM-dd},{sample.Hour},{sample.AcPwrt}\n";
            byte[] lineInBytes = new UTF8Encoding().GetBytes(line + Environment.NewLine);
            _fileStream.Write(lineInBytes, 0, lineInBytes.Length);
        }
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
                if (_fileStream != null)
                {
                    _fileStream.Flush();
                    _fileStream.Dispose();
                    _fileStream = null;
                }
            }
            // Dispose unmanaged resources here
            _disposed = true;
        }
    }
}
