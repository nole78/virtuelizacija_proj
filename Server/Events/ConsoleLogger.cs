using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Events
{
    public class ConsoleLogger
    {
        public void Subscribe(TransferEventHub hub)
        {
            hub.OnTransferStarted += TransferStarted;
            hub.OnSampleReceived += SampleReceived;
            hub.OnTransferCompleted += TransferCompleted;
            hub.OnWarningRaised += WarningRaised;
        }

        private void TransferStarted(object sender, TransferEventArgs e)
        {
            Console.WriteLine("[INFO] " + e.Message);
        }
        private void SampleReceived(object sender, TransferEventArgs e)
        {
            Console.WriteLine("[INFO]" + e.Message);
        }
        private void TransferCompleted(object sender, TransferEventArgs e)
        {
            Console.WriteLine("[INFO] " + e.Message);
        }
        private void WarningRaised(object sender, TransferEventArgs e)
        {
            Console.WriteLine($"[WARNING] [${e.WarningType}] ${e.Message}");
        }
    }
}
