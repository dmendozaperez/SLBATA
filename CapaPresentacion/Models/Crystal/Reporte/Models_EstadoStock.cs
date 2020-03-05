using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models.Crystal.Reporte
{
    public class Models_EstadoStock
    {
        public List<Estado_Stock_Cab> list_cab { get; set; }
        public List<Estado_Stock_Det> list_det { get; set; }
        public List<Estado_Stock_Fin> list_fin { get; set; }
    }
    public class Estado_Stock_Cab
    {
        public string tienda { get; set; }
        public Decimal uni_calzado { get; set; }
        public Decimal uni_no_calzado { get; set; }
        public Decimal sol_calzado { get; set; }
        public Decimal sol_no_calzado { get; set; }
        public string razon_social { get; set; }
        public string ruc { get; set; }
        public string rango_fecha { get; set; }

    }
    public class Estado_Stock_Det
    {
        public string tienda { get; set; }
        public string con_id { get; set; }
        public string concepto { get; set; }
        public string numdoc { get; set; }
        public DateTime fecha { get; set; }
        public Decimal ing_calzado { get; set; }
        public Decimal ing_no_calzado { get; set; }
        public Decimal sal_calzado { get; set; }
        public Decimal sal_no_calzado { get; set; }
        public Decimal ing_sol_calzado { get; set; }
        public Decimal ing_sol_no_calzado { get; set; }
        public Decimal sal_sol_calzado { get; set; }
        public Decimal sal_sol_no_calzado { get; set; }


    }
    public class Estado_Stock_Fin
    {
        public string tienda { get; set; }
        public string concepto { get; set; }
        public decimal calzado { get; set; }
        public Decimal no_calzado { get; set; }
        public Decimal sol_calzado { get; set; }
        public Decimal sol_no_calzado { get; set; }    
    }
}