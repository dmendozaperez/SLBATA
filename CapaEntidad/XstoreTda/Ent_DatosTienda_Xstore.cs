using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.XstoreTda
{
    public class Ent_DatosTienda_Xstore
    {
        public string xs_rtl_loc_id { get; set; }
        public string xs_description { get; set; }
        public string xs_address1 { get; set; }
        public string xs_store_name { get; set; }
        public string xs_store_manager { get; set; }
        public Int32 xs_calzado { get; set; }
        public Int32 xs_no_calzado { get; set; }
        public Int32 xs_total { get; set; }
        public List<Ent_DatosTienda_Version> xs_version { get; set; }
        public List<Ent_DatosTienda_Doc> xs_doc { get; set; }
    }
    public class Ent_DatosTienda_Version
    {
        public string xs_wkstn_id { get; set; }
        public string xs_ip_address { get; set; }
        public string xs_xstore_version { get; set; }
    }
    public class Ent_DatosTienda_Doc
    {
        public string xs_tipo_doc { get; set; }
        public string xs_serie { get; set; }
        public string xs_correlativo { get; set; }
    }

}
