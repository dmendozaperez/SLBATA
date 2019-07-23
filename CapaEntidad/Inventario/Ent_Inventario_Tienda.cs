using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Inventario
{
    public class Ent_Inventario_Tienda
    {
        public string cod_entid { get; set; }
        public string des_entid { get; set; }
        public List<Ent_Inventario_Fecha> List_TiendaFecha { get; set; }
    }

    public class Ent_Inventario_Fecha
    {
        public string id { get; set; }
        public string xst_inv_fec_aud { get; set; }
    }

}
