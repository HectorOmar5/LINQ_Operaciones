using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ
{
    public class ItemISR
    {
        public ItemISR()
        {
        }

        public ItemISR(decimal limInf, decimal limSup, decimal cuotaFija, decimal porExced, decimal subsidio)
        {
            LimInf = limInf;
            LimSup = limSup;
            CuotaFija = cuotaFija;
            PorExced = porExced;
            Subsidio = subsidio;
        }

        public decimal LimInf { get; set; }
        public decimal LimSup { get; set; }
        public decimal CuotaFija { get; set; }
        public decimal PorExced { get; set; }
        public decimal Subsidio { get; set; }
    }
}
