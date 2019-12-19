using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.BataClub
{
    public class Ent_BataClub_Cliente
    {
        public string canal { get; set; }
        public string dni { get; set; }
        public string nombres { get; set; }
        public string primer_nombre { get; set; }
        //public string segundo_nombre { get; set; }
        public string apellido_pat { get; set; }
        public string apellido_mat { get; set; }
        public string genero { get; set; }
        public string correo { get; set; }
        public string fec_nac { get; set; }
        public string telefono { get; set; }
        public string ubigeo { get; set; }
        public string ubigeo_distrito { get; set; }
        public string fec_ingreso { get; set; }
        public string user_ingreso { get; set; }
        public string fec_modif { get; set; }
        public string user_modif { get; set; }
        public string canal_modif { get; set; }
        public string fec_registro { get; set; }
        public string fec_miembro { get; set; }
        public string env_nue_miembro { get; set; }
        public string cod_tda { get; set; }
        public string envio_correo { get; set; }
        public string fec_envio_correo { get; set; }
        public string gene_cupon { get; set; }
        public string gene_fec_cupon { get; set; }
        public string miem_bataclub { get; set; }
        public string miem_bataclub_fecha { get; set; }
        public string dni_barra { get; set; }

        public string can_des { get; set; }
        public string des_entid { get; set; }
        public string cod_cadena { get; set; }
    }

    public class Ent_Cliente_Promocion
    {
        public string Promocion { get; set; }
        public string Barra { get; set; }
        public string Estado { get; set;}
        public string cup_fecha_ini { get; set; }
        public string cup_fecha_fin { get; set; }
        public string Tienda { get; set; }
        public string Doc { get; set; }
        public string Ndoc { get; set; }
        public string FecDoc { get; set; }
    }
}
