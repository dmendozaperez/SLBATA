using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models.ECommerce
{
    public class ReporteVentasEcommerce
    {
        public List<Models_VentasEcommerce> ListVentaEcommerce { get; set; }
    }

    public class Models_VentasEcommerce
    {
        public string BOL_FAC { get; set; }
        public string NRO_DOC { get; set; }
        public string CLIENTE { get; set; }
        public int PARES { get; set; }
        public int ACCESORIOS { get; set; }
        public int ROPA { get; set; }
        public int TOT_ARTICULO { get; set; }
        public decimal PRE_NETO { get; set; }
        public decimal IGV { get; set; }
        public decimal TOTAL { get; set; }
        public string NOM_TIENDA { get; set; }
        public string FECHA_INICIO { get; set; }
        public string FECHA_FIN { get; set; }
        public string TIP_DESPACHO { get; set; }

    }
}