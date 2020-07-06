using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class Ent_Folder_Documento
    {
        public string cod_semana { get; set; }
        public string Fol_id { get; set; }
        public string Fol_Des { get; set; }
        public string Fol_Padre { get; set; }
        public Int32 Fol_Item { get; set; }        
    }
}
