using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Control
{
    public class Ent_Tienda
    {
        public string tda_codigo { get; set; }
        public Boolean tda_xstore { get; set; }
        public string cadena { get; set; }

    }

    public class Ent_Tienda_Proceso
    {
        public string Tienda { get; set; }
        public string Tipo { get; set; }
        public string Numdoc { get; set; }
        public DateTime? Fecha { get; set; }
        public string Cod_EntId { get; set; }
        public String Destino { get; set; }
    }
}
