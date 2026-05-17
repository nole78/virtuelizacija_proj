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
            if (File.Exists(path))
            {
                _fileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read);
            }
            else
            {
                _fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                string header = "RowIndex,Day,Hour,AcPwrt,DcVolt,Temper,Vl1to2,Vl2to3,Vl3to1,AcCur1,AcVlt1\n";
                _fileStream.Write(new UTF8Encoding().GetBytes(header), 0, new UTF8Encoding().GetByteCount(header));
            }
        }
        public void WriteRow(PvSample sample)
        {
            // TODO: Napraviti tako da se upisuju i ostali podaci, ne samo AcPwrt
            if (_disposed)
                throw new ObjectDisposedException(nameof(SessionWriter), "Pokušaj pisanja u već zatvoreni SessionWriter.");
            string line =   $"{sample.RowIndex},{sample.Day},{sample.Hour},{sample.AcPwrt},{sample.DcVolt},"
                          + $"{sample.Temper},{sample.Vl1to2},{sample.Vl2to3},{sample.Vl3to1},{sample.AcCur1},{sample.AcVlt1}";

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
                    Console.WriteLine("[DISPOSE] Session writer uspešno zatvoren.");
                }
            }
            // Oslobodi unmanaged resurse
            _disposed = true;
        }
    }
}
