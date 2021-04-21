using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Reports
{
    public class Ent_Xstore_Vendedor
    {
        public string Semana_Str { get; set; }
        public string Distrito { get; set; }
        public string Des_Cadena { get; set; }
        public string Cod_Entid { get; set; }
        public string Des_Entid { get; set; }
        public string Dni { get; set; }
        public string Dni_Nombre { get; set; }
        public DateTime? Fecha { get; set; }
        public string Documento { get; set; }
        public string Articulo { get; set; }
        public string Cod_Categ { get; set; }
        public string Cod_Subca { get; set; }
        public int? Cantidad { get; set; }
        public int? Total { get; set; }
        public int? Tot_Transacc { get; set; }
        public int? Esc_Negro_2x50 { get; set; }
        public Decimal? Incentivo { get; set; }
        public int? Incentivo1 { get; set; }
        public int? Incentivo2 { get; set; }
        public Decimal? Total_Incentivo { get; set; }
        public int? Pares_Esc_Negro { get; set; }
        //Campos adicionales
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Cod_Tda { get; set; }
        public string Tip_Rep { get; set; }
    }
}
