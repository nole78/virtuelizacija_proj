using Common.PvDataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Validation
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Reason { get; set; }
    }
    public class SampleValidator
    {
        private int _lastRowIndex = -1;

        public ValidationResult ValidateRow(PvSample sample)
        {
            // 1. Monoton RowIndex
            if (sample.RowIndex <= _lastRowIndex)
                return Fail($"RowIndex nije monoton: primljeno {sample.RowIndex}, prethodni {_lastRowIndex}");
            _lastRowIndex = sample.RowIndex;

            // 2. DAY i HOUR – nenegativni
            if (sample.Day < 0)
                return Fail($"DAY je negativan: {sample.Day}");
            if (sample.Hour < 0 || sample.Hour > 23)
                return Fail($"HOUR van opsega: {sample.Hour}");

            // 3. Struje >= 0 (kad nije null/sentinel)
            if (sample.AcPwrt.HasValue && sample.AcPwrt.Value < 0)
                return Fail($"AcPwrt je negativan: {sample.AcPwrt}");

            // TODO: Dodati ako treba i za AcCur1

            // 4. Naponi > 0 kad nisu null
            if (sample.DcVolt.HasValue && sample.DcVolt.Value <= 0)
                return Fail($"DcVolt mora biti > 0: {sample.DcVolt}");
            if (sample.Vl1to2.HasValue && sample.Vl1to2.Value <= 0)
                return Fail($"Vl1to2 mora biti > 0: {sample.Vl1to2}");
            if (sample.Vl2to3.HasValue && sample.Vl2to3.Value <= 0)
                return Fail($"Vl2to3 mora biti > 0: {sample.Vl2to3}");
            if (sample.Vl3to1.HasValue && sample.Vl3to1.Value <= 0)
                return Fail($"Vl3to1 mora biti > 0: {sample.Vl3to1}");
            if (sample.AcVlt1.HasValue && sample.AcVlt1.Value <= 0)
                return Fail($"AcVlt1 mora biti > 0: {sample.AcVlt1}");

            // 6. Temperatura – realna vrednost (nema posebnog ograničenja osim treshholda koji je u analitici)
            // Ovde samo proveravamo da nije sentinel (to je već null ako je pravilno parsirano)

            return new ValidationResult { IsValid = true };
        }

        private static ValidationResult Fail(string reason) =>
            new ValidationResult { IsValid = false, Reason = reason };
    }
}
