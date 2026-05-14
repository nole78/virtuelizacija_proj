using Common.PvDataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logging
{
    public class ClientLogger : IDisposable
    {
        // TODO: Imlementirati logger za rejected_client.csv (pise u fajl)
        private bool _disposed = false;
        private FileStream _fileStream;
        public ClientLogger() 
        {
            // TODO: Nem pojma u koji folder treba ovaj fajl da se smesti, pa sam ga stavio u root folder projekta
            _fileStream = new FileStream("rejected_client.csv", FileMode.Append, FileAccess.Write, FileShare.Read);
        }
        public void WriteRow(PvSample sample)
        {
            // TODO: upisi sve kolone u fajl, trenutno se upisuje samo 3 kolone (Day, Hour, AcPwrt) zbog testiranja
            string line = $"{sample.Day:yyyy-MM-dd},{sample.Hour},{sample.AcPwrt}";
            byte[] lineInBytes = new UTF8Encoding(true).GetBytes(line + Environment.NewLine);
            _fileStream.Write(lineInBytes, 0, lineInBytes.Length);
        }
        ~ClientLogger()
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
                // Dispose managed resources here
                if(_fileStream != null)
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
