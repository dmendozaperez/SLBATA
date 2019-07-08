using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class Ent_Contabilidad_EstadoDocumento
    {
        public string tienda { get; set; }
        public string fecha { get; set; }
        public string tipo_doc { get; set; }
        public string serie { get; set; }
        public string numero { get; set; }
        public string total { get; set; }
        public string estado { get; set; }
        public string ruc { get; set; }
        public string login_ws { get; set; }
        public string clave_ws { get; set; }
        public string tipodoc { get; set; }
        public string folio { get; set; }

    }

    public class Ent_Contabilidad_EstadoDocumento_Det
    {
        public string PDF { get; set; }
        public string ESTADO { get; set; }
    }


}







