# Solarni Panel

Analiza i razmena podataka solarnog panela korišćenjem WCF servisa i događajnog modela

---

## Opis projekta

Projekat predstavlja klijent-server sistem za obradu i razmenu podataka sa solarnog panela korišćenjem WCF servisa.

Klijentska aplikacija:

* čita CSV fajl
* parsira podatke
* validira vrednosti
* šalje podatke serveru sekvencijalno

Serverska aplikacija:

* prima podatke preko WCF servisa
* vrši dodatnu validaciju
* skladišti podatke u fajl sistem
* pokreće događaje i upozorenja
* vrši analitiku nad podacima

Tehnologije:

* .NET Framework 4.7.2
* WCF (netTcpBinding)
* Console Application
* Class Library
* CSV obrada
* Event/Delegate model
* File System Storage
* Unit i Integration testovi

---

# Rešenje projekta

```text
SolarniPanelSolution/
│
├── Client/                 → Console App (.NET Framework 4.7.2)
├── Server/                 → Console App (.NET Framework 4.7.2)
├── Common/                 → Class Library (.NET Framework 4.7.2)
└── SolarniPanel.sln
```

---

# Uloga projekata

## Client

Zadužen za:

* čitanje CSV fajla
* validaciju podataka
* slanje podataka serveru
* logovanje grešaka
* upravljanje WCF konekcijom

---

## Server

Zadužen za:

* hostovanje WCF servisa
* prijem podataka
* obradu i validaciju
* skladištenje podataka
* analitiku
* događaje i upozorenja

---

## Common

Zajednička biblioteka za:

* modele
* WCF ugovore
* helper klase
* exceptions
* validatore
* event argumente

---

# Struktura foldera i fajlova – Projekat „Solarni panel”

```text
SolarniPanelSolution/
│
├── SolarniPanel.sln
│
├── Shared/
│   ├── Shared.csproj
│   │
│   ├── Contracts/
│   │   ├── IPvDataService.cs
│   │   └── ServiceFault.cs
│   │
│   ├── Models/
│   │   ├── PvMeta.cs
│   │   ├── PvSample.cs
│   │   ├── WarningEventArgs.cs
│   │   └── TransferStatus.cs
│   │
│   ├── Exceptions/
│   │   ├── InvalidSampleException.cs
│   │   ├── CsvParseException.cs
│   │   └── SessionException.cs
│   │
│   ├── Enums/
│   │   ├── WarningType.cs
│   │   └── TransferState.cs
│   │
│   ├── Helpers/
│   │   ├── CsvConstants.cs
│   │   ├── ValidationHelper.cs
│   │   ├── SentinelHelper.cs
│   │   └── DateTimeHelper.cs
│   │
│   └── Configuration/
│       └── AppSettings.cs
│
├── Server/
│   ├── Server.csproj
│   ├── App.config
│   ├── Program.cs
│   │
│   ├── Services/
│   │   ├── PvDataService.cs
│   │   └── SessionManager.cs
│   │
│   ├── Storage/
│   │   ├── CsvStorageManager.cs
│   │   ├── FileSessionWriter.cs
│   │   └── RejectWriter.cs
│   │
│   ├── Analytics/
│   │   ├── CurrentAnalytics.cs
│   │   ├── VoltageAnalytics.cs
│   │   ├── TemperatureAnalytics.cs
│   │   ├── PowerAnalytics.cs
│   │   └── AnalyticsEngine.cs
│   │
│   ├── Events/
│   │   ├── EventPublisher.cs
│   │   ├── TransferEvents.cs
│   │   └── WarningEvents.cs
│   │
│   ├── Logging/
│   │   ├── Logger.cs
│   │   ├── FileLogger.cs
│   │   └── LogFormatter.cs
│   │
│   ├── Validation/
│   │   ├── SampleValidator.cs
│   │   └── RowSequenceValidator.cs
│   │
│   ├── Resources/
│   │   └── Templates/
│   │       └── session_template.csv
│   │
│   ├── Data/
│   │   └── Plant01/
│   │       └── YYYY-MM-DD/
│   │           ├── session.csv
│   │           └── rejects.csv
│   │
│   └── Tests/
│       ├── Integration/
│       │   ├── StreamingTests.cs
│       │   └── DisposePatternTests.cs
│       │
│       └── Unit/
│           ├── ValidationTests.cs
│           ├── AnalyticsTests.cs
│           └── StorageTests.cs
│
├── Client/
│   ├── Client.csproj
│   ├── App.config
│   ├── Program.cs
│   │
│   ├── Services/
│   │   ├── WcfClientFactory.cs
│   │   ├── PvDataClient.cs
│   │   └── StreamingClient.cs
│   │
│   ├── Csv/
│   │   ├── CsvReaderService.cs
│   │   ├── CsvParser.cs
│   │   ├── CsvMapper.cs
│   │   └── CsvColumnSelector.cs
│   │
│   ├── Validation/
│   │   ├── ClientValidator.cs
│   │   └── SentinelValueHandler.cs
│   │
│   ├── Logging/
│   │   ├── ClientLogger.cs
│   │   └── RejectedRowsWriter.cs
│   │
│   ├── Input/
│   │   └── FloatingPV_Altamonte_FL_Data.csv
│   │
│   ├── Output/
│   │   └── rejected_client.csv
│   │
│   └── Tests/
│       ├── CsvParserTests.cs
│       ├── ClientValidationTests.cs
│       └── WcfConnectionTests.cs
│
├── Documentation/
│   ├── ArchitectureDiagram.png
│   ├── ProtocolFlowDiagram.png
│   ├── SequenceDiagram.png
│   ├── UseCases.md
│   ├── README.md
│   └── ProjectRequirements.md
│
├── Scripts/
│   ├── clean_data.ps1
│   ├── generate_test_data.ps1
│   └── start_server.ps1
│
├── Logs/
│   ├── client.log
│   ├── server.log
│   ├── warnings.log
│   └── errors.log
│
└── .gitignore
```

---

# Objašnjenje glavnih delova projekta

## Common

Zajednička biblioteka koju koriste i Client i Server.

Sadrži:

* WCF ugovore
* modele podataka
* custom exceptions
* helper klase
* validacije
* enum tipove

---

## Client

Klijentska aplikacija:

* čita CSV
* validira podatke
* mapira sentinel vrednosti
* šalje podatke preko WCF servisa
* loguje problematične redove

Glavni tok:

1. Učitavanje CSV
2. Parsiranje prvih N redova
3. Validacija
4. StartSession()
5. PushSample()
6. EndSession()

---

## Server

Serverska aplikacija:

* prima podatke
* validira podatke
* skladišti podatke
* pokreće događaje i alarme
* vrši analitiku

Glavni moduli:

* WCF servis
* analytics engine
* event sistem
* logging
* storage manager

---

# Ključni fajlovi

## IPvDataService.cs

Definiše WCF ServiceContract:

```csharp
StartSession(PvMeta meta)
PushSample(PvSample sample)
EndSession()
```

---

## PvMeta.cs

Sadrži:

```csharp
FileName
TotalRows
SchemaVersion
RowLimitN
PlantId
```

---

## PvSample.cs

Sadrži:

```csharp
Day
Hour
AcPwrt
DcVolt
Temper
Vl1to2
Vl2to3
Vl3to1
AcCur1
AcVlt1
RowIndex
```

---

# Konfiguracije (App.config)

## Client App.config

```xml
<appSettings>
  <add key="RowLimitN" value="200"/>
</appSettings>
```

## Server App.config

```xml
<appSettings>
  <add key="OverTempThreshold" value="80"/>
  <add key="VoltageImbalancePct" value="15"/>
  <add key="PowerFlatlineWindow" value="10"/>
  <add key="AcCur1SpikeThreshold" value="20"/>
  <add key="DcVoltMin" value="400"/>
  <add key="DcVoltMax" value="900"/>
</appSettings>
```

---

# Predlog arhitekture

```text
CSV FILE
   │
   ▼
CLIENT
(CSV Parser + Validation)
   │
   ▼
WCF SERVICE
(netTcpBinding + Streaming)
   │
   ▼
SERVER
(Storage + Analytics + Events)
   │
   ▼
FILESYSTEM STORAGE
(session.csv / rejects.csv)
```

---

# Događaji koje treba implementirati

```csharp
OnTransferStarted
OnSampleReceived
OnTransferCompleted
OnWarningRaised
```

---

# Warning tipovi

```csharp
CurrentSpikeWarning
CurrentOutOfBandWarning
DcVoltOutOfRangeWarning
OverTempWarning
VoltageImbalanceWarning
```

---

# Testovi koje obavezno treba imati

## Unit testovi

* CSV parsiranje
* validacija podataka
* sentinel mapiranje
* analytics logika
* događaji

## Integration testovi

* WCF konekcija
* streaming podataka
* dispose pattern
* simulacija prekida prenosa

---

# Preporučene tehnologije

* WCF netTcpBinding
* CSVHelper biblioteka
* MSTest ili xUnit
* Serilog ili custom logger
