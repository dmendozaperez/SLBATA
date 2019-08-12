using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Transac
{
    public class Ent_Consultar_Guia
    {   
        public string desc_almac { get; set; }
        public string num_gudis { get; set; }
        public string num_guia { get; set; }
        public string tienda_origen { get; set; }
        public string tienda_destino { get; set; }
        public string fecha_des { get; set; }
        public string desc_send_tda { get; set; }
        public string fec_env { get; set; }
        public decimal mc_id { get; set; }
        public string fec_recep { get; set; }
        public int cant_despd { get; set; }
        public int cant_fmd { get; set; }
        public string con_id { get; set; }
        public string con_des { get; set; }
    }
}
