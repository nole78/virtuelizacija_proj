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

namespace Server
{
    //Dispose zbog SW-a, servicebehaviour se odnosi na zivotni vek ove klase, pravim jednu instancu, te ta instanca usluzuje sve klijente koji se ikada povezu
    //(u prevodi bez ovoga Program.cs nije hteo da radi kako sam ja zamislila)
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class PvDataService : IPvDataService, IDisposable
    {
        private StreamWriter _sessionWriter;
        private StreamWriter _rejectWriter;
        private string _sessionDir;
        private PvMeta _currentMeta;
        private bool _sessionActive = false;
        private bool _disposed = false;

        [OperationBehavior(AutoDisposeParameters = true)]
        public void EndSession()
        {
            if(!_sessionActive)
            {
                Console.WriteLine("[END_SESSION] Nema aktivne sesije");
                return;
            }

            CloseCurrentSession();
            Console.WriteLine("[END_SESSION] Sesija zatvorena, resursi su oslobodjeni");
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public void PushSample(PvSample sample)
        {
            throw new NotImplementedException();
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public void StartSession(PvMeta meta)
        {
            if (_sessionActive)
            {
                CloseCurrentSession();
            }

            _currentMeta = meta;

            //Kreiranje putanje
            string date = DateTime.Today.ToString("yyyy-MM-dd");
            string plantId = ConfigurationManager.AppSettings["PlantId"];
            _sessionDir = Path.Combine("Data", plantId, date);
            Directory.CreateDirectory(_sessionDir);

            string sessionPath = Path.Combine(_sessionDir, "session.csv");
            string rejectPath = Path.Combine(_sessionDir, "rejects.csv");

            _sessionWriter = new StreamWriter(new FileStream(sessionPath, FileMode.Append, FileAccess.Write, FileShare.Read), Encoding.UTF8);
            _rejectWriter = new StreamWriter(new FileStream(rejectPath, FileMode.Append, FileAccess.Write, FileShare.Read), Encoding.UTF8);
            //Ovo ovde je samo za ispis zaglavlja, ako nije nov fajl, vec ima zagavlje, ego ne pisemo ga
            bool rejectExists = File.Exists(rejectPath);
            if (!rejectExists)
            {
                _rejectWriter.WriteLine("RowIndex,Reason,RawInput");
            }

            bool sessionExists = File.Exists(sessionPath);
            if (!sessionExists)
            {
                _rejectWriter.WriteLine("RowIndex,Reason,RawInput");
            }

            _sessionActive = true;
            Console.WriteLine($"[START_SESSION] Sesija otvorena: {_sessionDir}");
            Console.WriteLine($"    Fajl: {meta.FileName}, Redovi: {meta.TotalRows}, Ucitani redovi: {meta.RowLimitN}");
        }

        private void CloseCurrentSession()
        {
            _sessionActive = false;

            _sessionWriter?.Close();
            _sessionWriter?.Dispose();
            _sessionWriter = null;

            _rejectWriter?.Close();
            _rejectWriter?.Dispose();
            _rejectWriter = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_sessionWriter != null)
                    {
                        _sessionWriter.Dispose();
                    }
                    if (_rejectWriter != null)
                    {
                        _rejectWriter.Dispose();
                    }
                }
                _disposed = true;
            }

        }
    }
}
