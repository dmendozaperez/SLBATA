using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.XstoreTda
{
    public class Ent_XcenterDocumento
    {
        public string rtl_loc_id { get; set; }
        public string tipo_doc { get; set; }
        public string serie{ get; set; }
        public string correlativo { get; set; }
    }

    public class Ent_XcenterMaestro
    {
        public object tienda { get; set; }
        public object xstore { get; set; }
        public object documento { get; set; }
    }

    public class Ent_XcenterTienda
    {
        public string rtl_loc_id { get; set; }
        public string description { get; set; }
        public string address1 { get; set; }
        public string store_name { get; set; }
        public string store_manager { get; set; }
        public string xs_calzado { get; set; }
        public string xs_no_calzado { get; set; }
        public string xs_total { get; set; }
    }

    public class Ent_XcenterXstore
    {
        public string rtl_loc_id { get; set; }
        public string wkstn_id { get; set; }
        public string ip_address { get; set; }
        public string xstore_version { get; set; }
    }
}
