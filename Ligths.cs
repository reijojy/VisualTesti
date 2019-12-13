using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlyTalo
{
    class Ligths
    {
        public int Level { get; set; }
        public Boolean Status { get; set; }

        public Ligths()
        {
            this.Status = false;
            this.Level = 0;
        }
    }
}
