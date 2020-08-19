using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Bata
{
    public class Ent_Bata_Sac
    {
        
        public List<Ent_Bata_Sac_Cliente> Bata_Sac_Cliente { get; set; }
        public List<Ent_Bata_Sac_Cupones> Bata_Sac_Cupones { get; set; }
        public List<Ent_Bata_Sac_Venta>   Bata_Sac_Venta { get; set; }

    }
    public class Ent_Bata_Sac_Cliente
    {
        public string dni { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string ubicacion { get; set; }
        public string fec_nac { get; set; }
        public string bataclub { get; set; }
    }
    public class Ent_Bata_Sac_Cupones
    {
        public string barra { get; set; }
        public string promocion { get; set; }
        public string fecha_expiracion { get; set; }
        
    }
    public class Ent_Bata_Sac_Venta
    {
        public string cod_tda { get; set; }
        public string canal { get; set; }
        public string tienda { get; set; }
        public string tipodoc { get; set; }
        public string numdoc { get; set; }
        public string fecha { get; set; }
        public string estado { get; set; }
        public string pedido { get; set; }
        public string fc_suna { get; set; }
        public string fc_sfac { get; set; }
        public string fc_nfac { get; set; }
        public string fc_nint { get; set; }

    }

    public class Ent_Bata_Sac_Venta_Detalle
    {
        public string articulo { get; set; }
        public string talla { get; set; }
        public Int32 cantidad { get; set; }
        public Decimal precio { get; set; }
        public Decimal descuento { get; set; }
        public Decimal total_linea { get; set; }
        public string promocion { get; set; }

    }

}
