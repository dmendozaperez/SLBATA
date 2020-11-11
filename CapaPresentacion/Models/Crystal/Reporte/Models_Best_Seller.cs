using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models.Crystal.Reporte
{
    public class Models_Best_Seller
    {
        public string cod_cadena { get; set; }
        public string cod_entid { get; set; }
        public string des_entid { get; set; }
        public string cod_distri { get; set; }
        public string cod_linea { get; set; }
        public string cod_categ { get; set; }
        public string cod_subcat { get; set; }
        public string artic { get; set; }
        public decimal can_total { get; set; }
        public decimal neto { get; set; }

        public string fecha_rango { get; set; }
        public int id { get; set; }

        public decimal pplanilla { get; set; }
        public decimal ppventa { get; set; }
        public decimal stockactual { get; set; }

    }
}