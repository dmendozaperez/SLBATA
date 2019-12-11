using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Soporte
{
    public class Ent_Ticket_Retorno
    {
        public string codigo { get; set; }
        public string tiendaGen { get; set; }
        public string fechaGen { get; set; }
        public decimal montoGen { get; set; }
        public string serieGen { get; set; }
        public string numeroGen { get; set; }
        public string tiendaUso { get; set; }
        public string fechaUso { get; set; }
        public decimal montoUso { get; set; }
        public string serieUso { get; set; }
        public string numeroUso { get; set; }
        public string estado { get; set; }
        public bool impreso { get; set; }
        public string mensaje { get; set; }
        public string imp_log { get; set; }
        public decimal montoDscto { get; set; }
        public int nroReimp { get; set; }
    }
}
