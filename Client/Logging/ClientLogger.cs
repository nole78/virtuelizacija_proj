using System;
using System.IO;
using System.Text;

namespace Client.Logging
{
    public class ClientLogger : IDisposable
    {
        private bool _disposed = false;
        private FileStream _fileStream;
        public ClientLogger(string path) 
        {
            _fileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read);
        }
        public void Log(string message)
        {
            if(_disposed)
                throw new ObjectDisposedException(nameof(ClientLogger), "Pokušaj pisanja u već zatvoreni ClientLogger.");
            string line = message;
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
                    Console.WriteLine("[DISPOSE] Client Logger uspešno zatvoren.");
                }
            }
            // Dispose unmanaged resources here
            _disposed = true;
        }
    }
}
