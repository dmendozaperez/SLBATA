using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Crystal.Reporte
{
    public class Models_Rendimiento_Categ
    {
        public string semana { get; set; }
        public string distrito { get; set; }
        public string tienda { get; set; }
        public string tipo { get; set; }
        public string linea { get; set; }
        public string categoria { get; set; }
        public Int32 stk_ant { get; set; }
        public Int32 stk_real { get; set; }
        public decimal saly_stk { get; set; }
        public Int32 pares_venta_ant { get; set; }
        public Int32 pares_venta_real { get; set; }
        public decimal saly_pares { get; set; }
        public decimal ratio { get; set; }
        public Int32 pares_acum_ant { get; set; }
        public Int32 pares_acum_real { get; set; }
        public decimal taly_pares_ant { get; set; }
        public decimal soles_ant { get; set; }
        public decimal soles_real { get; set; }
        public decimal saly_soles { get; set; }
        public decimal soles_acum_ant { get; set; }
        public decimal soles_acum_real { get; set; }
        public decimal saly_soles_acum { get; set; }
        public decimal nlineas_act { get; set; }
        public decimal nlineas_pas { get; set; }
        public string evalua { get; set; }
    }
}