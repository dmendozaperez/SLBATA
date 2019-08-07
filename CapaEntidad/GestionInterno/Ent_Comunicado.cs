using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.GestionInterno
{
    public class Ent_Comunicado
    {
        public Decimal id { get; set; }
        public String tienda { get; set; }
        public string archivo { get; set; }
        public string descripcion { get; set; }
        public string url { get; set; }
        public string fecha_hora_crea { get; set; }
        public string fecha_hora_mod { get; set; }
        public string file_leido_fecha { get; set; }
        public Boolean file_leido { get; set; }    
        public DateTime fecha { get; set; }    
    }
}
