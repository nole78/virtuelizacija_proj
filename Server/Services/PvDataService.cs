using Common;
using Common.PvDataContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Server.Storage;
using Server.Validation;
using System.Threading;
using Server.Events;
using Server.Analytics;
using Common.Exceptions;

namespace Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class PvDataService : IPvDataService
    {
        private SessionWriter _sessionWriter;
        private RejectWriter _rejectWriter;
        private SampleValidator _sampleValidator;
        private string _sessionDir;
        private PvMeta _currentMeta;
        private bool _sessionActive = false;
        private int _receivedSamplesCount = 0;
        private double _procenat = 0;
        private CurrentSpikeDetector _spikeDetector;
        private DcVoltRangeChecker _dcVoltChecker;
        private ThermalAlarmChecker _thermalAlarmChecker;

        private DateTime _lastActivity;
        private Timer _sessionWatchdog;

        private readonly TransferEventHub _eventHub;
        public PvDataService(TransferEventHub eventHub)
        {
            _eventHub = eventHub;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public void EndSession()
        {
            if(!_sessionActive)
            {
                throw new FaultException<PvTransferException>(new PvTransferException("Nema aktivne sesije!"));
            }
            CloseCurrentSession();
            _eventHub.Finish();
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public void PushSample(PvSample sample)
        {
            _eventHub.Recived();
            if (!_sessionActive)
            {
                throw new FaultException<PvTransferException>(new PvTransferException("Nema aktivne sesije!"));
            }

            _lastActivity = DateTime.Now;

            var validationResult = _sampleValidator.ValidateRow(sample);

            if (!validationResult.IsValid)
            {
                _rejectWriter.WriteRow(sample, validationResult.Reason);
                Console.WriteLine($"[PUSH_SAMPLE] Odbijen red {sample.RowIndex}: {validationResult.Reason}");
            }
            else
            {
                _sessionWriter.WriteRow(sample);

                _receivedSamplesCount++;
                _procenat = _currentMeta.RowLimitN > 0 ? ((double)_receivedSamplesCount / _currentMeta.RowLimitN) * 100 : 0;
                Console.WriteLine($"[STATUS] prenos u toku... Primljeno redova: {_receivedSamplesCount} ({_procenat:F2}%)");

                // PROVERE
                _spikeDetector.RunCheck((double)sample.AcCur1);
                _dcVoltChecker.RunCheck((double)sample.DcVolt);
                _thermalAlarmChecker.RunCheck((double)sample.Temper);
            }
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public void StartSession(PvMeta meta)
        {
            if (meta == null || meta.RowLimitN <= 0 || meta.TotalRows <= 0 || meta.FileName.Equals(string.Empty) || meta.SchemaVersion.Equals(string.Empty))
            {
                throw new FaultException<PvTransferException>(new PvTransferException("Nevalidni meta podaci!"));
            }

            if (_sessionActive)
            {
                throw new FaultException<PvTransferException>(new PvTransferException("Sesija je vec aktivna!"));
            }

            _currentMeta = meta;

            //Kreiranje putanje
            string date = DateTime.Today.ToString("yyyy-MM-dd");
            string plantId = ConfigurationManager.AppSettings["PlantId"];
            _sessionDir = Path.Combine("Data", plantId, date);
            Directory.CreateDirectory(_sessionDir);

            string _sessionPath = Path.Combine(_sessionDir, "session.csv");
            string _rejectPath = Path.Combine(_sessionDir, "rejects.csv");

            _sessionWriter = new SessionWriter(_sessionPath);
            _rejectWriter = new RejectWriter(_rejectPath);

            _sampleValidator = new SampleValidator();
            _spikeDetector = new CurrentSpikeDetector(_eventHub);
            _dcVoltChecker = new DcVoltRangeChecker(_eventHub);
            _thermalAlarmChecker = new ThermalAlarmChecker(_eventHub);

            _lastActivity = DateTime.Now;

            _sessionWatchdog = new Timer(CheckSessionTimeout, null,
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(10));

            _sessionActive = true;
            _eventHub.Start();
        }


        //Pomocne metode |
        //               V
        private void CloseCurrentSession()
        {
            _sessionActive = false;
            _receivedSamplesCount = 0;
            _procenat = 0;
            _currentMeta = null;
            _sampleValidator = null;
            _sessionWatchdog?.Dispose();
            _sessionWatchdog = null;

            try
            {
                _sessionWriter?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _sessionWriter = null;
            }

            try
            {
                _rejectWriter?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _rejectWriter = null;
            }
        }

        private void CheckSessionTimeout(object state)
        {
            if (!_sessionActive)
                return;

            TimeSpan inactiveTime = DateTime.Now - _lastActivity;

            if (inactiveTime > TimeSpan.FromSeconds(5))
            {
                Console.WriteLine("[WATCHDOG] Sesija timeoutovana.");

                CloseCurrentSession();
            }
        }
    }
}
