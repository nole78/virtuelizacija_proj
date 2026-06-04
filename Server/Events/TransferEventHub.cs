using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Events
{
    public class TransferEventHub
    {
        public delegate void TransferHanlder(object sender, TransferEventArgs e);
        public event TransferHanlder OnTransferStarted;
        public event TransferHanlder OnSampleReceived;
        public event TransferHanlder OnTransferCompleted;
        public event TransferHanlder OnWarningRaised;

        public void Start()
        {
            OnTransferStarted?.Invoke(this, new TransferEventArgs(WarningType.None, "Transfer Started."));
        }
        public void Recived()
        {
            OnSampleReceived?.Invoke(this, new TransferEventArgs(WarningType.None, "Sample Received."));
        }
        public void Finish()
        {
            OnTransferCompleted?.Invoke(this, new TransferEventArgs(WarningType.None, "Transfer Completed."));
        }
        public void RaiseWarning(TransferEventArgs info)
        {
            OnWarningRaised?.Invoke(this, info);
        }
    }
}
