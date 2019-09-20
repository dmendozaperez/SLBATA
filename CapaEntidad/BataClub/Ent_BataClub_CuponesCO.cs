using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.BataClub
{
    public class Ent_BataClub_Cupones
    {
        public string promocion  { get; set; }
        public string estado { get; set; }
        public string fechaFin { get; set; }
        public string nombresCliente { get; set; }
        public string apellidosCliente { get; set; }
        public string dniCliente { get; set; }
        public string correo { get; set; }
        public string cupon { get; set; }
        public decimal porcDesc { get; set; }
        public string genero { get; set; }
        public string mesCumple { get; set; }
        public bool miemBataClub { get; set; }
    }
    public class Ent_BataClub_Promociones
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Porc_Dcto { get; set; }
        public int MaxPares { get; set; }
        public string FechaFin { get; set; }
        public bool PromActiva { get; set; }
        public int nroCupones { get; set; }
    }

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
