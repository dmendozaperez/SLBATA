using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models.Crystal.Reporte
{
    public class Models_Tab_Pros
    {
        public int ID { get; set; }
        public string TIENDA          {get;set;}
        public string SEMANA          {get;set;}
        public string TIPO_VALOR_1    {get;set;}
        public int STK_ACTUAL      {get;set;}
        public int STK_TALY_ACT    {get;set;}
        public int CWS             {get;set;}
        public string TIPO_VALOR_2    {get;set;}
        public int PARES_VENTA_ANT {get;set;}
        public int PARES_PRESU_ACT {get;set;}
        public int PARES_VENTA_ACT {get;set;}
        public int PARES_TEST_ACT  {get;set;}
        public int PARES_TALY_ACT  {get;set;}
        public int SOLES_VENTA_ANT {get;set;}
        public int SOLES_PRESU_ACT {get;set;}
        public int SOLES_VENTA_ACT {get;set;}
        public int SOLES_TEST_ACT  {get;set;}
        public int SOLES_TALY_ACT  {get;set;}
        public int PRECIO_PROM_ANT {get;set;}
        public int PRECIO_PROM_ACT {get;set;}

    }
}