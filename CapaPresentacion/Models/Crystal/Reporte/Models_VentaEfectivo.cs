using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Crystal.Reporte
{
    public class Models_VentaEfectivo
    {
        public string Rango_Fecha { get; set; }
        public string Distrito { get; set; }
        public string Cadena { get; set; }
        public string Cod_Tienda { get; set; }
        public string Nombre_Tienda { get; set; }
        public string Tipo { get; set; }
        public string Documento { get; set; }
        public DateTime Fecha { get; set; }
        public Decimal Total { get; set; }
        public Decimal Redondeo { get; set; }
        //Campos Adicionales
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}