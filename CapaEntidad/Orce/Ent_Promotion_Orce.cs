using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Orce
{
    public class Ent_Promotion_Orce
    {
        /*CAMPAIGN_ID, PROMOTION_ID, PROMOTION_NAME, TIPO_PROMOCION, FEC_INICIO, FEC_FIN, 
          HORA_INICIO, HORA_FIN, FEC_CREACION, USUARIO_CREACION, ESTADO, ULTIMA_EXPORTACION, UBIC_INCLUIDAS, 
          UBIC_EXCLUIDAS, SEGMENTO_OBJETIVO, PROMO_ACTIVADO, EXPORTS, ID_OFERTA, CODIGO_OFERTA, CODIGO_CUPON, CANT_CUPON_GEN, 
          NAME_OFERTA, NOMBRE_PDV, TIPO_OFERTA, CALIF_INCL, CALIF_EXCL, AWARD_INCL, AWARD_EXCL, 
          TIPO_PROMO_PREVISTA, SUBTOTAL_MIN, SUBTOTAL_MAX, IMPORTE_MAX_PREMIO, DEAL_ACTIVADO,
          ESTILO_UMBRAL, TIPO_UMBRAL, UMBRAL, TIPO_DESCUENTO, VALOR_DESCUENTO, 
          CANTIDAD_MAXIMA, LIMITE_OFERTA, DESCUENTO_PRORRATEADO*/
        public string CAMPAIGN_ID { get; set; }
        public string PROMOTION_ID { get; set; }
        public string PROMOTION_NAME { get; set; }
        public string TIPO_PROMOCION { get; set; }
        public string FEC_INICIO { get; set; }
        public string FEC_FIN { get; set; }
        public string HORA_INICIO { get; set; }
        public string HORA_FIN { get; set; }
        public string FEC_CREACION { get; set; }
        public string USUARIO_CREACION { get; set; }
        public string ESTADO { get; set; }
        public string ULTIMA_EXPORTACION { get; set; }
        public string UBIC_INCLUIDAS { get; set; }
        public string UBIC_EXCLUIDAS { get; set; }
        public string SEGMENTO_OBJETIVO { get; set; }
        public string PROMO_ACTIVADO { get; set; }
        public string EXPORTS { get; set; }
        public string ID_OFERTA { get; set; }
        public string CODIGO_OFERTA { get; set; }
        public string CODIGO_CUPON { get; set; }
        public Decimal? CANT_CUPON_GEN { get; set; }
        public string NAME_OFERTA { get; set; }
        public string NOMBRE_PDV { get; set; }
        public string TIPO_OFERTA { get; set; }
        public string CALIF_INCL { get; set; }
        public string CALIF_EXCL { get; set; }
        public string AWARD_INCL  { get; set; }
        public string AWARD_EXCL { get; set; }
        public string TIPO_PROMO_PREVISTA { get; set; }
        public decimal? SUBTOTAL_MIN { get; set; }
        public decimal? SUBTOTAL_MAX { get; set; }
        public decimal? IMPORTE_MAX_PREMIO { get; set; }
        public string DEAL_ACTIVADO { get; set; }
        public string ESTILO_UMBRAL { get; set; }
        public string TIPO_UMBRAL { get; set; }
        public decimal? UMBRAL { get; set; }
        public string TIPO_DESCUENTO { get; set; }
        public decimal? VALOR_DESCUENTO { get; set; }
        public decimal? CANTIDAD_MAXIMA { get; set; }
        public decimal? LIMITE_OFERTA { get; set; }
        public string DESCUENTO_PRORRATEADO { get; set; }          
    }
    //public class Ent_Promotion_Orce_Filtros
    //{
    //    public string PROMOTION_TYPE { get; set; }
    //    public string CREATE_DATE { get; set; }
    //    public string CREATE_USER { get; set; }
    //    public string STATUS { get; set; }
    //}
    public class Ent_Promotion_Orce_Lista
    {
        public List<Ent_Promotion_Orce_Status> Promotion_Orce_Status { get; set; }
        public List<Ent_Promotion_Orce_Type> Promotion_Orce_Type { get; set; }
        public List<Ent_Promotion_Orce_User> Promotion_Orce_User { get; set; }
    }
    public class Ent_Promotion_Orce_Status
    {
        public string codigo { get; set; }
        public string descripcion { get; set; }
    }
    public class Ent_Promotion_Orce_Type
    {
        public string codigo { get; set; }
        public string descripcion { get; set; }
    }
    public class Ent_Promotion_Orce_User
    {
        public string codigo { get; set; }
        public string descripcion { get; set; }
    }
    public class Ent_Promotion_Orce_Atributos
    {
        public string promotion_id { get; set; }
        public string deal_id { get; set; }
        public string tipo { get; set; }
        public string attribute_name { get; set; }
        public string attribute_value { get; set; }

    }
}
