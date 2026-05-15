using Client.Csv;
using Client.Proxy;
using Common.PvDataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class TransferService
    {
        private readonly PvMeta _meta;

        public TransferService(PvMeta meta)
        {
            _meta = meta;
        }

        public void Run()
        {
            using (var proxy = new PvServiceProxy()) // pravi konstruktor bez parametra
            using (var csvReader = new CsvReader(_meta.FileName))
            {
                try
                {
                    proxy.StartSession(_meta);

                    // TODO: Dodati parser i onda pustiti ovo da radi
                    /*PvSample sample;
                    while ((sample = csvReader.ReadLine()) != null)
                    {
                        proxy.PushSample(sample);
                    }*/

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
            {
                try
                {
                    proxy.StartSession(_meta);

                    /*
                    PvSample sample;
                    int count = 0;
                    while ((sample = csvReader.ReadNext()) != null)
                    {
                        proxy.PushSample(sample);
                        count++;

                        if (count == 5)
                            throw new Exception("Simuliran prekid na 5. redu!");
                    }*/

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
