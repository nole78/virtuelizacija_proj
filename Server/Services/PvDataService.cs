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
    //Dispose zbog SW-a, servicebehaviour se odnosi na zivotni vek ove klase,
    //pravim jednu instancu, te ta instanca usluzuje sve klijente koji se ikada povezu
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
        private int _receivedSamplesCount = 0;
        private double _procenat = 0;

        [OperationBehavior(AutoDisposeParameters = true)]
        public void EndSession()
        {
            if(!_sessionActive)
            {
                Console.WriteLine("[END_SESSION] Nema aktivne sesije");
                return;
            }

            CloseCurrentSession();
            Console.WriteLine($"[STATUS] Prenos u završen... Primljeno redova: {_receivedSamplesCount} ({_procenat:F2}%)");
            Console.WriteLine("[END_SESSION] Sesija zatvorena, resursi su oslobodjeni");
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public void PushSample(PvSample sample)
        {
            string rejectReason = Validate(sample);

            if (rejectReason != null)
            {
                string raw = SampleToRaw(sample);
                _rejectWriter.WriteLine($"{sample.RowIndex},{EscapeCsv(rejectReason)},{EscapeCsv(raw)}");

                Console.WriteLine($"[PUSH_SAMPLE] Odbijen red {sample.RowIndex}: {rejectReason}");
            }
            else
            {
                _sessionWriter.WriteLine(
                    $"{sample.RowIndex}," +
                    $"{sample.Day}," +
                    $"{sample.Hour}," +
                    $"{sample.AcPwrt}," +
                    $"{sample.DcVolt}," +
                    $"{sample.Temper}," +
                    $"{sample.Vl1to2}," +
                    $"{sample.Vl2to3}," +
                    $"{sample.Vl3to1}," +
                    $"{sample.AcCur1}," +
                    $"{sample.AcVlt1}");

                _receivedSamplesCount++;
                _procenat = ((double)_receivedSamplesCount / _currentMeta.RowLimitN) * 100;
                Console.WriteLine($"[STATUS] prenos u toku... Primljeno redova: {_receivedSamplesCount} ({_procenat:F2}%)");
            }
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

            _sessionWriter = new StreamWriter(new FileStream(sessionPath, FileMode.Create, FileAccess.Write, FileShare.Read), Encoding.UTF8);
            _rejectWriter = new StreamWriter(new FileStream(rejectPath, FileMode.Create, FileAccess.Write, FileShare.Read), Encoding.UTF8);
            //Ovo ovde je samo za ispis zaglavlja, ako nije nov fajl, vec ima zagavlje, ergo ne pisemo ga
            bool rejectExists = File.Exists(rejectPath);
            if (!rejectExists)
            {
                _rejectWriter.WriteLine("RowIndex,Reason,RawInput");
            }

            bool sessionExists = File.Exists(sessionPath);
            if (!sessionExists)
            {
                _sessionWriter.WriteLine("RowIndex,Day,Hour,AcPwrt,DcVolt,Temper,Vl1to2,Vl2to3,Vl3to1,AcCur1,AcVlt1");
            }

            _sessionActive = true;
            Console.WriteLine($"[START_SESSION] Sesija otvorena: {_sessionDir}");
            Console.WriteLine($"    Fajl: {meta.FileName}, Redovi: {meta.TotalRows}, Ucitani redovi: {meta.RowLimitN}");
        }


        //Pomocne metode |
        //               V
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

        //* za vracanje razloga za upis u rejects.csv "nevalidne pisati u rejects.csv
        //(sa razlogom i sirovim inputom)"
        private string Validate(PvSample sample)
        {
            //Uslovi 3. zadatka:
            //AcPwrt >= 0
            if (sample.AcPwrt.HasValue && sample.AcPwrt.Value < 0)
            {
                return $"AcPwrt negativan: {sample.AcPwrt}";
            }

            //naponi/frekvencije > 0 
            if (sample.DcVolt.HasValue && sample.DcVolt <= 0)
            {
                return $"DcVolt nije pozitivan: {sample.DcVolt}";
            }
            if(sample.Vl1to2.HasValue && sample.Vl1to2 <= 0)
            {
                return $"Vl1to2 nije pozitivan: {sample.Vl1to2}";
            }
            if(sample.Vl2to3.HasValue && sample.Vl2to3 <= 0)
            {
                return $"Vl2to3 nije pozitivan: {sample.Vl2to3}";
            }
            if(sample.Vl3to1.HasValue && sample.Vl3to1 <= 0)
            {
                return $"Vl3to1 nije pozitivan: {sample.Vl3to1}";
            }
            if(sample.AcVlt1.HasValue && sample.AcVlt1 <= 0)
            {
                return $"AcVlt1 nije pozitivan: {sample.AcVlt1}";
            }

            return null;
        }

        private string SampleToRaw(PvSample s)
        {
           return $"Day={s.Day}|Hour={s.Hour}|AcPwrt={s.AcPwrt}|DcVolt={s.DcVolt}|" +
            $"Temper={s.Temper}|Vl1to2={s.Vl1to2}|Vl2to3={s.Vl2to3}|" +
            $"Vl3to1={s.Vl3to1}|AcCur1={s.AcCur1}|AcVlt1={s.AcVlt1}";
        }

        //
        private string EscapeCsv(string s) {
            return s.Contains(",") || s.Contains("\"") || s.Contains("\n") ? $"\"{s.Replace("\"", "\"\"")}\"" : s;
        }

        //Dispose
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
