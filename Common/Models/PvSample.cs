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
        public string Day { get; set; }
        [DataMember]
        public string Hour { get; set; }
        [DataMember]
        public string AcPwrt { get; set; }
        [DataMember]
        public string DcVolt { get; set; }
        [DataMember]
        public string Temper { get; set; }
        [DataMember]
        public string Vl1to2 { get; set; }
        [DataMember]
        public string Vl2to3 { get; set; }
        [DataMember]
        public string Vl3to1 { get; set; }
        [DataMember]
        public string AcCur1 { get; set; }
        [DataMember]
        public string AcVlt1 { get; set; }

        public PvSample(int rowIndex, string day, string hour, string acPwrt, string dcVolt, string temper, string vl1to2, string vl2to3, string vl3to1, string acCur1, string acVlt1)
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
