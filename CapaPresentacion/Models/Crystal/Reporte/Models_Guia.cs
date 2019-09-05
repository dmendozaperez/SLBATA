using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Crystal.Reporte
{
    public class Models_Guia
    {
        public string NUMERO { get; set; }
        public string FECHA { get; set; }
        public string PARES { get; set; }
        public string VCALZADO { get; set; }
        public string NOCALZADO { get; set; }
        public string VNOCALZADO { get; set; }      
        public string ESTADO { get; set; }
        public string TIPO_CATE { get; set; }
        public string LINEA { get; set; }
        public string CATEGORIA { get; set; }
        public string ARTICULO { get; set; }
        public string CALIDAD { get; set; }
        public string TALLA { get; set; }
        public string CANTIDAD { get; set; }
        public string TIENDA_ORI { get; set; }
        public string TIENDA_DES { get; set; }
    }

    public class Models_GuiaConten
    {
        public List<Models_Guia> listGuia { get; set; }
        public string strDetalle { get; set; }
    }
   
}