using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlyTalo
{
    class AsuntoLampo
    {
        private int asetettuLampo;
        private int edellinenLampo;
        private static int maksimiLampo = 30;
        private static int minimiLampo = 14;
        public List<ThermoHistory> thermoHistories { get; set; }
        public AsuntoLampo()
        {
            this.asetettuLampo = 18;
        }
        public void setAsetettuLampo(int lampo)
        {
            this.edellinenLampo = this.asetettuLampo;
            this.asetettuLampo = lampo;
            // Historia merkintä
            ThermoHistory thermoHistory = new ThermoHistory(lampo);
        }
        public int getAsetettuLampo()
        {
            return asetettuLampo;
        }
        public int getEdellinenLampo()
        {
            return edellinenLampo;
        }
        private void addThermoHistory(ThermoHistory th)
        {
            if (thermoHistories == null) thermoHistories = new List<ThermoHistory>();
            thermoHistories.Add(th);
        }
        public Boolean tarkistaTavoiteLammpo(int lampo)
        {
            if (lampo > maksimiLampo || lampo < minimiLampo) return false;
            return true;
        }

    }
}
