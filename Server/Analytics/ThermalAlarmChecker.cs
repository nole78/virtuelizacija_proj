using Server.Config;
using Server.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Analytics
{
    public class ThermalAlarmChecker
    {
        private readonly TransferEventHub _transferEventHub;
        private readonly double _overTempThreshlod = 0;

        public ThermalAlarmChecker(TransferEventHub transferEventHub)
        {
            _transferEventHub = transferEventHub;
            _overTempThreshlod = ThresholdConfig.OverTempThreshold;
        }

        public void RunCheck(double temper)
        {
            if (temper > _overTempThreshlod)
            {
                _transferEventHub.RaiseWarning(new TransferEventArgs(
                    WarningType.OverTemp,
                    $"Temper value: {Math.Round(temper, 4)} | OverTempThreshold: {Math.Round(_overTempThreshlod, 4)}"));
            }
        }
    }
}
