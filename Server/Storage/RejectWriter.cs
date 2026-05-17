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
        public RejectWriter(string path) 
        {
            if (File.Exists(path))
            {
                _fileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read);
            }
            else
            {
                _fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                string header = "RowIndex,Reason,RawInput\n";
                _fileStream.Write(new UTF8Encoding().GetBytes(header), 0, new UTF8Encoding().GetByteCount(header));
            }
        }
        public void WriteRow(PvSample sample, string reason)
        {
            if(_disposed)
                throw new ObjectDisposedException(nameof(RejectWriter), "Pokušaj pisanja u već zatvoreni RejectWriter.");
            string line = $"{sample.RowIndex},{reason}," 
                        + $"Day={sample.Day}|Hour={sample.Hour}|AcPwrt={sample.AcPwrt}|DcVolt={sample.DcVolt}|" 
                        + $"Temper={sample.Temper}|Vl1to2={sample.Vl1to2}|Vl2to3={sample.Vl2to3}|" 
                        + $"Vl3to1={sample.Vl3to1}|AcCur1={sample.AcCur1}|AcVlt1={sample.AcVlt1}";

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
                    Console.WriteLine("[DISPOSE] Reject writer uspešno zatvoren.");
                }
            }
            // Dispose unmanaged resources here
            _disposed = true;
        }
    }
}
