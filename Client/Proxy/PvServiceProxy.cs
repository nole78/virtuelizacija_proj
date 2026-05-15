using Common;
using Common.PvDataContracts;
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
        private bool _disposed = false;
        private IPvDataService _channel;
        private ChannelFactory<IPvDataService> _channelFactory;
        public PvServiceProxy() 
        {
            _channelFactory = new ChannelFactory<IPvDataService>("PvDataService");
            _channel = _channelFactory.CreateChannel();
        }

        public PvServiceProxy(IPvDataService channel, ChannelFactory<IPvDataService> factory = null)
        {
            _channelFactory = factory;
            _channel = channel;
        }

        // Dodati metode koje pozivaju server
        public void StartSession(PvMeta meta)
        {
            if (_disposed)
                throw new ObjectDisposedException("PvServiceProxy");
            _channel.StartSession(meta);
        }
        public void PushSample(PvSample sample)
        {
            if (_disposed)
                throw new ObjectDisposedException("PvServiceProxy");
            _channel.PushSample(sample);
        }
        public void EndSession()
        {
            if (_disposed)
                throw new ObjectDisposedException("PvServiceProxy");
            _channel.EndSession();
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
                    Console.WriteLine("[DISPOSE] Kanal uspešno zatvoren.");
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
