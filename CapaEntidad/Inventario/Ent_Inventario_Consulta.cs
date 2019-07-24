using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Inventario
{
    public class Ent_Inventario_Consulta
    {
        public string des_entid { get; set; }
        public string articulo { get; set; }
        public string calidad { get; set; }
        public string talla { get; set; }
        public string teorico { get; set; }
        public string fisico { get; set; }
        public string diferencia { get; set; }
        public string fecha_inv { get; set; }
    }

    public class Ent_Consulta_Movimiento
    {
        public string TIENDA { get; set; }
      public string FECHA{ get; set; }
      public string INI_CALZADO{ get; set; }
      public string INI_NO_CALZADO{ get; set; }
      public string VEN_CALZADO{ get; set; }
      public string VEN_NO_CALZADO{ get; set; }
      public string ING_CALZADO{ get; set; }
      public string ING_NO_CALZADO{ get; set; }
      public string SAL_CALZADO{ get; set; }
      public string SAL_NO_CALZADO{ get; set; }
      public string SALDO_CALZADO{ get; set; }
      public string SALDO_NO_CALZADO{ get; set; }
    }


}
