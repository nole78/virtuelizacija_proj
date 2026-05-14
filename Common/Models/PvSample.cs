using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.PvDataContracts
{
    [DataContract]
    public class PvSample
    {
        [DataMember]
        public int RowIndex { get; set; }
        [DataMember]
        public DateTime Day { get; set; }
        [DataMember]
        public int Hour { get; set; }
        [DataMember]
        public double? AcPwrt { get; set; }
        [DataMember]
        public double? DcVolt { get; set; }
        [DataMember]
        public double? Temper { get; set; }
        [DataMember]
        public double? Vl1to2 { get; set; }
        [DataMember]
        public double? Vl2to3 { get; set; }
        [DataMember]
        public double? Vl3to1 { get; set; }
        [DataMember]
        public double? AcCur1 { get; set; }
        [DataMember]
        public double? AcVlt1 { get; set; }

        public PvSample(int rowIndex, DateTime day, int hour, double? acPwrt, double? dcVolt, double? temper, double? vl1to2, double? vl2to3, double? vl3to1, double? acCur1, double? acVlt1)
        {
            RowIndex = rowIndex;
            Day = day;
            Hour = hour;
            AcPwrt = acPwrt;
            DcVolt = dcVolt;
            Temper = temper;
            Vl1to2 = vl1to2;
            Vl2to3 = vl2to3;
            Vl3to1 = vl3to1;
            AcCur1 = acCur1;
            AcVlt1 = acVlt1;
        }

        public PvSample() {}
    }
}
