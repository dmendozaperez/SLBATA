using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ReportsValeCompra
{
    public class Reporte_Resultado
    {
        public string rep_dni { get; set; }
        public string rep_nombre { get; set; }
        public string rep_apellidoPater { get; set; }
        public string rep_apellidoMater { get; set; }
        public string rep_email { get; set; }


        public string rep_CupBarra { get; set; }
        public string rep_CupNumero { get; set; }
        public string rep_CupMonto { get; set; }
        public string rep_CupEstado { get; set; }
        


        public string rep_docTipo { get; set; }
        public string rep_docSerie { get; set; }
        public string rep_docNro { get; set; }
        public string rep_docfecha { get; set; }


        public string rep_tdaCodigo { get; set; }
        public string rep_tdaDesc { get; set; }
        public string rep_tdaDirec { get; set; }
      

    }
}
