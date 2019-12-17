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
    public class Ent_BataClub_Orce_Promotion
    {
        public string ORCE_COD_PROM { get; set; }
        public string ORCE_DES_PROM { get; set; }
        public int CAMPAIGN_ID { get;set;}
        public int PROMOTION_ID { get; set; }
        public int DEAL_ID { get; set; }
        public int COUPON_GEN_COUNT { get; set; }
        public string PROMOTION_NAME { get; set; }
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
        public string Coupon_Code { get; set; }
    }
    public class Ent_BataClub_ListTdasProm
    {
        public string prom_id { get; set; }
        public string cod_tda { get; set; }
        public string des_tda { get; set; }
        public string des_cadena_tda { get; set; }
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

    /*
     * Clase para comparar listas
     * https://docs.microsoft.com/es-es/dotnet/api/system.linq.enumerable.union?view=netframework-4.8
     */
    public class Ent_BataClub_CuponesComparer : IEqualityComparer<Ent_BataClub_Cupones>
    {
        public bool Equals(Ent_BataClub_Cupones x, Ent_BataClub_Cupones y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.promocion == y.promocion && x.estado == y.estado && x.fechaFin == y.fechaFin && x.nombresCliente == y.nombresCliente &&
                x.apellidosCliente == y.apellidosCliente && x.dniCliente == y.dniCliente && x.correo == y.correo && x.cupon == y.cupon &&
                x.porcDesc == y.porcDesc && x.genero == y.genero && x.mesCumple == y.mesCumple && x.miemBataClub == y.miemBataClub;
        }

        public int GetHashCode(Ent_BataClub_Cupones cupon)
        {
            if (Object.ReferenceEquals(cupon, null)) return 0;

            int hashCuponpromocion = cupon.promocion == null ? 0 : cupon.promocion.GetHashCode();
            int hashCuponestado = cupon.estado == null ? 0 : cupon.estado.GetHashCode();
            int hashCuponfechaFin = cupon.fechaFin == null ? 0 : cupon.fechaFin.GetHashCode();
            int hashCuponnombresCliente = cupon.nombresCliente == null ? 0 : cupon.nombresCliente.GetHashCode();
            int hashCuponapellidosCliente = cupon.apellidosCliente == null ? 0 : cupon.apellidosCliente.GetHashCode();
            int hashCupondniCliente = cupon.dniCliente == null ? 0 : cupon.dniCliente.GetHashCode();
            int hashCuponcorreo = cupon.correo == null ? 0 : cupon.correo.GetHashCode();
            int hashCuponcupon = cupon.cupon == null ? 0 : cupon.cupon.GetHashCode();
            int hashCuponporcDesc = cupon.porcDesc.GetHashCode();
            int hashCupongenero = cupon.genero == null ? 0 : cupon.genero.GetHashCode();
            int hashCuponmesCumple = cupon.mesCumple == null ? 0 : cupon.mesCumple.GetHashCode();
            int hashCuponmiemBataClub = cupon.miemBataClub.GetHashCode();

            return hashCuponpromocion ^ hashCuponestado ^ hashCuponfechaFin ^ hashCuponnombresCliente ^ hashCuponapellidosCliente ^ hashCupondniCliente ^ 
                hashCuponcorreo ^ hashCuponcupon ^ hashCuponporcDesc ^ hashCupongenero ^ hashCuponmesCumple ^ hashCuponmiemBataClub ;
        }

    }


}
