using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CapaPresentacion.Models.Crystal.Reporte;

namespace CapaPresentacion.Models.Crystal.Reporte
{
    public class Models_InventarioPlanilla //atributos inventario planilla
    {
        public List<Lista_InventarioPlanilla> ListInventarioPlanilla { get; set; }
    }
    public class Lista_InventarioPlanilla
    {
        public string COD_TIENDA { get; set; }
        public string ARTICULO { get; set; }
        public string CALIDAD { get; set; }
        public string MEDIDA { get; set; }
        public string STK_MED_LAT { get; set; }
        public decimal PPLANILLA { get; set; }
        public decimal STOCK { get; set; }
        public decimal VALOR { get; set; }
        public string TALLA { get; set; }
        public string FECHA { get; set; }
        public string CATEG { get; set; }
        public string SUBCATEG { get; set; }
        public string TIPO { get; set; }
    }
}