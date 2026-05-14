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
        PvServiceProxy() 
        {
            // TODO: Implementirati
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
                    _channelFactory.Close();
                    _channelFactory = null;
                }
            }
            // Oslobodi unmanaged resurse
            _disposed = true;
        }
    }
}
