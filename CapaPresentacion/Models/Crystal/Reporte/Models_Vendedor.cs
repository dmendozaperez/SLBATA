using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Crystal.Reporte
{
    public class Reporte_Vendedor
    {
        public List<Models_Vendedor> listMV { get; set; }
        public List<Models_Total2> listTotal2 { get; set; }
    }
    public class Models_Vendedor
    {
        public string semana_str { get; set; }
        public string cod_distri { get; set; }
        public string des_cadena { get; set; }
        public string cod_entid { get; set; }
        public string des_entid { get; set; }
        public string store_name { get; set; }
        public string dni { get; set; }
        public string dni_nombre { get; set; }
        public decimal pares { get; set; }
        public decimal ropa { get; set; }
        public decimal acc { get; set; }
        public decimal cant_total { get; set; }
        public decimal neto { get; set; }
        public decimal igv { get; set; }
        public decimal total { get; set; }
        public decimal upt { get; set; }
        public decimal ntk { get; set; }
        public decimal ntk2 { get; set; }
        public decimal mayor_1 { get; set; }
        public decimal pormay1 { get; set; }
        public decimal ticket_prom { get; set; }
        public decimal upt_un { get; set; }

    }

    public class Models_Total2
    {
        public string COD_ENTID_2 { get; set; }
        public int SUM_CANT_TOTAL { get; set; }
        public int SUM_NTK_TOTAL { get; set; }
        public decimal TOTAL_2 { get; set; }
        public decimal UPT_2 { get; set; }
        public decimal TICKET_PROM_2 { get; set; }
    }
   
}