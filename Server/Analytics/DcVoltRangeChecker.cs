using Server.Config;
using Server.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Analytics
{
    public class DcVoltRangeChecker
    {
        private double _dcVoltMin = 0;
        private double _dcVoltMax = 0;
        private readonly TransferEventHub _transferEventHub;
        
        public DcVoltRangeChecker(TransferEventHub transferEventHub) 
        {
            _dcVoltMin = ThresholdConfig.DcVoltMin;
            _dcVoltMax = ThresholdConfig.DcVoltMax;
            _transferEventHub = transferEventHub;
        }
        public void RunCheck(double dcVolt)
        {
            /*DC napon van opsega: proveravati DCVOLT po svakom redu; ako vrednost padne ispod
            DcVoltMin ili poraste iznad DcVoltMax (iz konfiguracije) → podići
            DcVoltOutOfRangeWarning.*/
            //Ovo je razdvojeno samo kako bi poruke bile "tacnije"
            if(dcVolt < _dcVoltMin)
            {
                _transferEventHub.RaiseWarning(new TransferEventArgs(
                    WarningType.DcVoltOutOfRange,
                    $"DcVolt value: {Math.Round(dcVolt, 4)} | DcVoltMin: {Math.Round(_dcVoltMin, 2)}"));
            }
            else if (dcVolt > _dcVoltMax)
            {
                _transferEventHub.RaiseWarning(new TransferEventArgs(
                    WarningType.DcVoltOutOfRange,
                    $"DcVolt value: {Math.Round(dcVolt, 4)} | DcVoltMax: {Math.Round(_dcVoltMax, 2)}"));
            }
        }
    }
}
