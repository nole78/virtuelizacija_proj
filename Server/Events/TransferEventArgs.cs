using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Events
{
    public enum WarningType
    {
        None,
        CurrentSpike,
        CurrentOutOfBand,
        DcVoltOutOfRange,
        OverTemp
    }
    public class TransferEventArgs : EventArgs
    {
        public WarningType WarningType { get; set; }
        public string Message { get; set; }

        public TransferEventArgs(WarningType warningType, string message)
        {
            WarningType = warningType;
            Message = message;
        }
    }
}
