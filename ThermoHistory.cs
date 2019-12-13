using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace AlyTalo
{
    class ThermoHistory
    {

        private static CultureInfo cultureInfo = new CultureInfo("fi-FI");
        private DateTime asetusAika;
        public int Lampo { get; set; }
        
        public ThermoHistory(int lampo)
        {
            this.Lampo = lampo;
            asetusAika = DateTime.Now;
        }
        public String getAsetusAikaAsString()
        {
           return asetusAika.ToString(cultureInfo);
        }
    }
}
