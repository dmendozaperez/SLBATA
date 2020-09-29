using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.RuletaBata
{
    public class Ent_RuletaBata
    {
        public List<Premios> listPremios { get; set; }
    }
    public class Premios
    {
        public int id { get; set; }
        public int indice { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int tipo { get; set; }
        public string prom_id { get; set; }
        public string color { get; set; }
        public string imagen { get; set; }
    }
    public class Ent_Ruleta_Valida
    {
        public string prim_nom { get; set; }
        public string seg_nom { get; set; }
        public string pri_ape { get; set; }
        public string seg_ape { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public decimal compras { get; set; }
        public Boolean bataclub { get; set; }
    }
}
