# Solarni panel — Folder struktura projekta

Projekat implementira klijent–server sistem za prikupljanje, razmenu i skladištenje podataka o radu solarnog panela korišćenjem WCF servisa i događajnog modela.

**Solution sadrži 3 projekta:** `Common`, `Client`, `Server`

---

## Common
> Deljeni tipovi i ugovori — referencuju ga i klijent i server

```
Common/
├── Contracts/
│   └── IPvService.cs           # [ServiceContract] WCF ugovor (StartSession, PushSample, EndSession)
├── Models/
│   ├── PvMeta.cs               # [DataContract] metapodaci sesije (FileName, TotalRows, SchemaVersion, RowLimitN)
│   └── PvSample.cs             # [DataContract] jedan red merenja (Day, Hour, AcPwrt, DcVolt, Temper, ...)
├── Exceptions/
│   ├── PvValidationFault.cs    # FaultContract za nevalidne uzorke
│   └── PvTransferException.cs  # Prilagođeni izuzetak za greške pri prenosu
└── Constants/
    ├── ChannelNames.cs          # Nazivi odabranih 10 kanala iz CSV-a
    └── SentinelValues.cs        # Sentinel vrednost 32767.0 → null
```

---

## Client
> Čitanje CSV-a, parsiranje i slanje podataka serveru

```
Client/
├── Csv/
│   ├── CsvReader.cs            # Čita CSV fajl, implementira IDisposable nad StreamReader-om
│   └── CsvRowParser.cs         # Parsira redove, mapira sentinel 32767.0 → null
├── Proxy/
│   └── PvServiceProxy.cs       # WCF proxy ka serveru, implementira IDisposable
├── Logging/
│   └── ClientLogger.cs         # Upisuje odbačene redove u rejected_client.csv
├── TransferService.cs           # Orchestracija: otvara sesiju, šalje redove, zatvara sesiju
├── Program.cs                   # Entry point
├── App.config                   # netTcpBinding konfiguracija, RowLimitN (npr. 100–200)
└── Tests/
    ├── CsvRowParserTests.cs     # [Unit] parsiranje redova, edge case-ovi
    └── SentinelMappingTests.cs  # [Unit] mapiranje sentinel vrednosti na null
```

---

## Server
> Prijem podataka, skladištenje na disk, analitika i događajni model

```
Server/
├── Service/
│   └── PvService.cs            # [ServiceBehavior] implementacija IPvService WCF ugovora
├── Storage/
│   ├── SessionWriter.cs        # Piše validne redove u Data/<PlantId>/<YYYY-MM-DD>/session.csv, IDisposable
│   └── RejectWriter.cs         # Piše nevalidne redove u rejects.csv (sa razlogom), IDisposable
├── Events/
│   ├── TransferEventArgs.cs    # EventArgs klase za sve događaje
│   └── TransferEventHub.cs     # Svi događaji: OnTransferStarted, OnSampleReceived,
│                               #   OnTransferCompleted, OnWarningRaised
├── Analytics/
│   ├── CurrentSpikeDetector.cs # Zadatak 9 — nagli skok AcCur1, praćenje proseka ±20%
│   ├── DcVoltRangeChecker.cs   # Zadatak 10 — DcVolt van [DcVoltMin, DcVoltMax]
│   └── ThermalAlarmChecker.cs  # Zadatak 10 — Temper > OverTempThreshold
├── Validation/
│   └── SampleValidator.cs      # Provera vrednosti uzorka (nenegativnost, monoton RowIndex, ...)
├── Program.cs                   # Entry point, host WCF servisa
├── App.config                   # Pragovi: OverTempThreshold, VoltageImbalancePct,
│                               #   PowerFlatlineWindow, AcCur1SpikeThreshold, DcVoltMin/Max
└── Tests/
    ├── SampleValidatorTests.cs          # [Unit] validacija uzoraka
    ├── CurrentSpikeDetectorTests.cs     # [Unit] detekcija skoka struje
    ├── DcVoltRangeCheckerTests.cs       # [Unit] provera DC napona
    ├── ThermalAlarmCheckerTests.cs      # [Unit] termalni alarm
    └── SessionWriterDisposeTests.cs     # [Unit] dokazuje pravilno zatvaranje resursa (Dispose pattern)
```

---

## Runtime — Data folder

Server automatski kreira sledeću strukturu pri prvoj sesiji (nije deo solution-a):

```
Data/
└── <PlantId>/
    └── <YYYY-MM-DD>/
        ├── session.csv     # Validni primljeni redovi
        └── rejects.csv     # Odbačeni redovi sa razlogom i sirovim inputom
```

---

## Tehnologije i koncepti

| Oblast | Gde se primenjuje |
|---|---|
| WCF (netTcpBinding, streaming) | `Common/Contracts`, `Server/Service`, `Client/Proxy` |
| DataContract / FaultContract | `Common/Models`, `Common/Exceptions` |
| IDisposable / Dispose pattern | `Client/Csv`, `Client/Proxy`, `Server/Storage` |
| Fajl i memorijski tokovi | `Server/Storage/SessionWriter`, `RejectWriter` |
| Delegati i događaji | `Server/Events/TransferEventHub` |
| Analitika (spike, opseg, temp) | `Server/Analytics/` |
| Unit testovi | `Client/Tests`, `Server/Tests` |

---

## Korisni linkovi

- **GitHub repozitorijum:** https://github.com/nole78/virtuelizacija_proj.git
- **Dataset:** https://data.openei.org/submissions/6179 (`Floating PV Altamonte FL Data.csv`)
