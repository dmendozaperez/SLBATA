using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Inventario
{
    public class Ent_MovDoc_Consulta
    {
        public string mc_id { get; set; }
        public string tienda { get; set; }
        public string tipo_transac { get; set; }
        public string tipo_doc { get; set; }
        public string num_doc { get; set; }
        public string fecha_doc { get; set; }
        public Int32 cant { get; set; }
        public string tda_des { get; set; }
        public string tda_ori { get; set; }
    }
    public class Ent_MovDoc_Consulta_Detalle
    {
        public string articulo { get; set; }
        public string calidad { get; set; }
        public string linea { get; set; }
        public string categoria { get; set; }
        public string talla { get; set; }
        public Int32 cantidad { get; set; }
    }
    public class Ent_MovDoc_Consulta_Detalle_Articulo
    {
        public string articulo { get; set; }
        public string calidad { get; set; }
        public string linea { get; set; }
        public string categoria { get; set; }
        public Decimal total { get; set; }
        public List<Ent_MovDoc_Consulta_Detalle_Articulo_Talla> list_talla { get; set; }
    }
    public class Ent_MovDoc_Consulta_Detalle_Articulo_Talla
    {       
        public string talla { get; set; }
        public Int32 cantidad { get; set; }
    }
}
