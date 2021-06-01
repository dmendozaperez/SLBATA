using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Transitos
{
    public class Ent_Consulta_Transitos
    {
        public string Empre { get; set; }
        public string Caden { get; set; }
        public string Concepto { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string Guia { get; set; }
        public Decimal? Calz { get; set; }
        public Decimal? No_Calz { get; set; }
        public Decimal? Cajas { get; set; }
        public string Estado { get; set; }
        public DateTime? Tran_Ini { get; set; }
        public DateTime? Tran_Fin { get; set; }
        public DateTime Fecha { get; set; }
        public string Empresa { get; set; }
        public string Cadena { get; set; }
    }
    public class Ent_concepto_Transitos
    {
        public string Con_Id { get; set; }
        public string Con_Des { get; set; }
        public Boolean Con_Tran { get; set; }
    }
    public class Ent_Articulo_Transitos
    {
        public string Cod_Artic { get; set; }
        public string Des_Artic { get; set; }
    }

    public class Ent_Articulo_TransitosArt
    {
        public string Empresa { get; set; }
        public string Cadena { get; set; }
        public string Concepto { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string Guia { get; set; }
        public string Articulo { get; set; }
        public string Cal { get; set; }
        public string Talla { get; set; }
        public int? Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }
}
