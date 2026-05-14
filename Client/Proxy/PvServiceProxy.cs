using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client.Proxy
{
    // Veza sa serverom
    public class PvServiceProxy : IDisposable
    {
        // TODO: Implementirati
        private bool _disposed = false;
        private IPvDataService _channel;
        private ChannelFactory<IPvDataService> _channelFactory;
        public PvServiceProxy() 
        {
            // TODO: Promeniti bez hardcodovanog stringa vec is settingsa (Napraviti config folder s klasom na foru kao u serveru da parsira iz settingsa)
            // ili parametra konstruktora
            _channelFactory = new ChannelFactory<IPvDataService>("PvDataEndpoint");
            _channel = _channelFactory.CreateChannel();
        }
        ~PvServiceProxy()
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
                if (_channel != null)
                {
                    try
                    {
                        ((IClientChannel)_channel).Close();
                    }
                    catch
                    {
                        ((IClientChannel)_channel).Abort();
                    }
                    _channel = null;
                }
                if (_channelFactory != null)
                {
                    try
                    {
                        _channelFactory.Close();
                        _channelFactory = null;
                    }
                    catch
                    {
                        _channelFactory.Abort();
                        _channelFactory = null;
                    }
                }
            }
            // Oslobodi unmanaged resurse
            _disposed = true;
        }
    }
}
