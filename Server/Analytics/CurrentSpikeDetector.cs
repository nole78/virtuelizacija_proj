using Server.Config;
using Server.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Analytics
{
    public class CurrentSpikeDetector
    {
        private double _acCur1Mean = 0;
        private int _cnt = 0;
        private double _acCur1SpikeThreshold = 0;
        private readonly TransferEventHub _eventHub;
        private double _acCur1Last = 0;

        public CurrentSpikeDetector(TransferEventHub eventHub)
        {
            _acCur1SpikeThreshold = ThresholdConfig.AcCur1SpikeThreshold;
            _eventHub = eventHub;
        }

        public void RunCheck(double acCur1)
        {
            double acCur = Math.Abs(acCur1 - _acCur1Last);
            // Current Spike provera
            if (_cnt != 0 && (acCur > _acCur1SpikeThreshold))
            {
                double delta = acCur1 - _acCur1Last;
                string smer = delta > 0 ? "increase" : "decrease";
                _eventHub.RaiseWarning(new TransferEventArgs(
                    WarningType.CurrentSpike,
                    $"AcCur1 value: {Math.Round(acCur1, 4)} | Spike: {smer} {Math.Round(Math.Abs(delta), 4)}"));
            }

            // Ako je struja 0 (odnosno panel iskljucen) ne utice na proveru out of Band ili u sam prosek struje
            // Jer bi onda kad dodju posle normalne vred struje one bile stalno OutOfBand
            // (OVO SAM SAM DODAO, NEMAU SPEC)
            if (acCur1 == 0)
                return;

            // Current Out Of Band provera
            double acCurAbs = Math.Abs(acCur1);
            if (_cnt != 0 && (acCurAbs < 0.8*_acCur1Mean || acCurAbs > 1.2*_acCur1Mean))
            {
                _eventHub.RaiseWarning(new TransferEventArgs(
                    WarningType.CurrentOutOfBand,
                    $"AcCur1 value: {Math.Round(acCur1, 4)} | AcCur1Mean: {Math.Round(_acCur1Mean,4)}"));
            }

            // Update proseka struje i countera
            _acCur1Mean = (_acCur1Mean * _cnt + acCurAbs) / (++_cnt);
            _acCur1Last = acCur1;
        }
    }
}
