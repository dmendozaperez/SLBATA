using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models.RuletaBata
{
    public class Ruleta
    {
        
    }
    public class PremioRuleta
    {
        public string id  {get;set;}
        public string indice {get;set;}
        public string nombre  {get;set;}
        public string descripcion {get;set;}
        public string tipo {get;set;}
        public string prom_id {get;set;}
        public string color { get; set; }
        public string codigo_cupon { get; set; }
    }
    public class GanadorRuleta
    {
        public string dni { get; set; }
        public string nombre { get; set; }
        public string ape_pat { get; set; }
        public string ape_mat { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
    }

}