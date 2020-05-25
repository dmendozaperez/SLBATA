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
        public List<Variacion_Precio> list_var { get; set; }
        public List<Insumos> list_ins { get; set; }

        public List<Estado_Stock_Saldos> list_saldos { get; set; }
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
        public String fecha { get; set; }
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
    public class Variacion_Precio
    {
        public string tienda { get; set; }
        public string fecha { get; set; }
        public Decimal vp_ing_sol_calzado { get; set; }
        public Decimal vp_ing_sol_no_calzado { get; set; }
        public Decimal vp_sal_sol_calzado { get; set; }
        public Decimal vp_sal_sol_no_calzado { get; set; }


    }

    public class Insumos
    {
        public string tienda { get; set; }
        public Decimal tcan_in { get; set; }
        public Decimal tsol_in { get; set; }
    }
    public class Estado_Stock_Saldos
    {
        public string tienda { get; set; }
        public decimal SF_SOLES_CALZADO { get; set; }
        public decimal SF_SOLES_NO_CALZADO { get; set; }
    }
}