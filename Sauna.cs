using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlyTalo
{
    class Sauna
    {
        public int Lampo { get; set; }
        public int TavoiteLampo { get; set; }
        public Boolean isLammitys { get; set; }
        public Sauna(int lampo)
        {
            this.Lampo = lampo;
            isLammitys = false; ;
        }
    }
    
}
