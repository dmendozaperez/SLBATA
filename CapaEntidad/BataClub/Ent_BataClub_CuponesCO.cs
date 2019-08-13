using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.BataClub
{
    public class Ent_BataClub_CuponesCO
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string dni { get; set; }
        public string correo { get; set; }
        public string cupon { get; set; }
        public string tienda { get; set; }
      /*  public string dni_venta { get; set; }*/
        public string nombres_venta { get; set; }
      //  public string correo_venta { get; set; }
      //  public string telefono_venta { get; set; }
        public string tickets { get; set; }
        public string soles { get; set; }
        public string id_grupo { get; set; }
       /* public string grupo { get; set; }*/
        public string porc_desc { get; set; }
        public string fec_doc { get; set; }
        public string prom_id { get; set; }
        public string prom_des { get; set; }
        public string est_des { get; set; }
        public string cup_fecha_fin { get; set; }
        public string max_pares { get; set; }
    }

    public class Ent_BataClub_ListaCliente
    {
        public string dni { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string email { get; set; }
        public string barra { get; set; }
        public string error { get; set; }
    }

    public class Ent_BataClub_ListaItems
    {
        public Ent_BataClub_ListaCliente[] Lista { get; set; }
    }

}
