using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Crystal.Reporte
{
    public class Models_Obs
    {
        public string rango_fecha { get; set; }        
        public string distrito { get; set; }        
        public string tienda { get; set; }    
        public string tipo_cat { get; set; }    
        public string cod_linea { get; set; }
        public string cod_categ { get; set; }
        public string artic { get; set; }
        public string calid { get; set; }
        public decimal pplanilla { get; set; }
        public string tip_obsol { get; set; }
        public string des_obsol { get; set; }
        public decimal stk { get; set; }
        public Decimal vtas4sem { get; set; }
        public decimal pventa { get; set; }       
    }
}