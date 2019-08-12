using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.XstoreTda
{
    public class Ent_ConfigConexion
    {
        public List<Ent_Central_Xst> list_central_xst { get; set; }
        public List<Ent_Cajas_Xst> list_cajas_xst { get; set; }
    }
    public class Ent_Central_Xst
    {
        public string IP_CENTRAL { get; set; }
        public string DES_CENTRAL { get; set; }
        public int ESTADO_CONEXION_CENTRAL_XST { get; set; }
    }
    public class Ent_Cajas_Xst
    {
        public string COD_ENTID { get; set; }
        public string TIENDA { get; set; }
        public string NCAJA { get; set; }
        public string IP { get; set; }
        public string VERSION_XST { get; set; }
        public int ESTADO_CONEXION_CAJA_XST { get; set; }
    }
}
