using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Crystal.Reporte
{
    public class Models_CuponRedimidos
    {
        public string Rango_Fecha { get; set; }
        public string Distrito { get; set; }
        public string Cadena { get; set; }
        public string Cod_Tienda { get; set; }
        public string Nombre_Tienda { get; set; }
        public string Documento { get; set; }
        public DateTime Fecha { get; set; }
        public string Dni { get; set; }
        public string Cliente { get; set; }
        public Decimal Porcentaje { get; set; }
        public Decimal Monto { get; set; }
        public string Cupon { get; set; }
        public string Campanna { get; set; }
        public string Descripcion { get; set; }
        // Campos Adicionales
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Scan_Code { get; set; }
    }

    public class Models_Prefijos
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
    }
}