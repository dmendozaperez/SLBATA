using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Bata
{
    public class Ent_Transito
    {

    }
    public class Ent_Tran_Emp_Cad_Tda
    {
        public string cod_entid { get; set; }
        public string des_entid { get; set; }
        public string cod_empresa { get; set; }
        public string des_empresa { get; set; }
        public string cod_cadena { get; set; }
        public string des_cadena { get; set; }
        
    }
    public class Ent_Tran_concepto
    {
        public string con_id { get; set; }
        public string con_des { get; set; }
        public Boolean con_tran { get; set; }
    }
    public class Ent_Tran_Articulo
    {
        public string cod_artic { get; set; }
        public string des_artic { get; set; }
    }
}
