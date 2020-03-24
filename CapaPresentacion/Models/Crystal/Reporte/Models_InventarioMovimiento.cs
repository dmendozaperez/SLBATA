using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models.Crystal.Reporte
{
    public class Models_InventarioMovimiento
    {
       public List<Lista_InventarioMovimiento> ListInventarioMovimiento { get; set; }
    }

    public class Lista_InventarioMovimiento
    {
        public string CONCEPTO { get; set; }
        public string DESCRIPCION { get; set; }
        public string TIENDA { get; set; }
        public string FECHA { get; set; }
        public string HORA { get; set; }
        public string NUMDOC { get; set; }
        public string ARTICULO { get; set; }
        public string CALIDAD { get; set; }
        public string TALLA { get; set; }
        public string STK_MED_PER { get; set; }
        public string STK_MED_LAT { get; set; }
        public decimal CALZADO { get; set; }
        public decimal NOCALZADO { get; set; }
        public decimal STOCK { get; set; }
        public decimal VALOR { get; set; }
        public string FEC_INI { get; set; }
        public string FEC_FIN { get; set; }
        public string HORA_2 { get; set; }


    }


}