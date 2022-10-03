using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ
{
    public class Estatus
    {
        public Estatus(int id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
        }

       public int id { get; set; }
       public string nombre { get; set; }
    }
}
