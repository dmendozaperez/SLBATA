using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Transac
{
    public class Ent_Analisis_Mov
    {
        public Int32 item { get; set; }
        public string fecha { get; set; }
        public string tipo_doc { get; set; }
        public string num_doc { get; set; }
        public string origen { get; set; }
        public string destino { get; set; }
        public Int32 inicial { get; set; }
        public Int32 ingreso { get; set; }
        public Int32 salida { get; set; }
        public Int32 saldo { get; set; }

    }
}
