using Client.Csv;
using Client.Logging;
using Client.Proxy;
using Common.PvDataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class TransferService
    {
        private readonly PvMeta _meta;
        private readonly CsvRowParser _parser;

        public TransferService(PvMeta meta)
        {
            _meta = meta;
            _parser = new CsvRowParser();
        }

        public void Run()
        {
            using (var proxy = new PvServiceProxy()) // pravi konstruktor bez parametra
            using (var csvReader = new CsvReader(_meta.FileName))
            using (var clientLogger = new ClientLogger("rejected_client.csv"))
            {
                try
                {
                    proxy.StartSession(_meta);

                    PvSample sample;
                    int currentRow = 0;
                    while (currentRow <= _meta.RowLimitN)
                    {
                        string line = csvReader.ReadLine();
                        if (line == null) break; // Kraj fajla

                        sample = _parser.MapLineToSample(line, currentRow);

                        if (sample != null)
                            proxy.PushSample(sample);
                        else
                            clientLogger.Log($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | Row {currentRow} | Data: {line}{Environment.NewLine}");

                        currentRow++;
                    }

                    proxy.EndSession();
                }
                catch (ObjectDisposedException ex)
                {
                    // programerska greška
                    Console.WriteLine($"Proxy je disposed: {ex.Message}");
                }
                catch (CommunicationException ex)
                {
                    // server pao usred prenosa
                    Console.WriteLine($"Greška u komunikaciji: {ex.Message}");
                }
            }
        }

        public void RunWithSimulatedFailure()
        {
            using (var proxy = new PvServiceProxy())
            using (var csvReader = new CsvReader(_meta.FileName))
            using (var clientLogger = new ClientLogger("rejected_client.csv"))
            {
                try
                {
                    proxy.StartSession(_meta);

                    PvSample sample;
                    int currentRow = 0;
                    while (currentRow < _meta.RowLimitN)
                    {
                        string line = csvReader.ReadLine();
                        if (line == null) break; // Kraj fajla

                        sample = _parser.MapLineToSample(line, currentRow);

                        if (sample != null)
                            proxy.PushSample(sample);
                        else
                            clientLogger.Log($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | Row {currentRow} | Data: {line}{Environment.NewLine}");

                        currentRow++;

                        if (currentRow == 5)
                            throw new Exception("Simuliran prekid na 5. redu!");
                    }

                    proxy.EndSession();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Greška: {ex.Message}");
                }
            }
        }
    }
}
