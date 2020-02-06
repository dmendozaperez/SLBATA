using CapaEntidad.Orce;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Orce
{
    public class Dat_Promotion_Orce
    {

        public List<Ent_Promotion_Orce_Atributos> lista_prom_orce_atributos(string promotion_id, string deal_id,string tipo)
        {
            List<Ent_Promotion_Orce_Atributos> listar = null;
            string sqlquery = "USP_ORCE_GET_PROMOCIONES_ATRIBUTOS";
            DataTable dt = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {

                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PROMOTION_ID", promotion_id);
                            cmd.Parameters.AddWithValue("@DEAL_ID", deal_id);
                            cmd.Parameters.AddWithValue("@TIPO", tipo);
                            
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_Promotion_Orce_Atributos>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_Promotion_Orce_Atributos()
                                          {
                                              promotion_id = dr["PROMOTION_ID"].ToString(),
                                              deal_id = dr["DEAL_ID"].ToString(),
                                              tipo = dr["TIPO"].ToString(),
                                              attribute_name = dr["ATTRIBUTE_NAME"].ToString(),                                              
                                              attribute_value = dr["ATTRIBUTE_VALUE"].ToString(),                                                                                            
                                          }
                                        ).ToList();
                            }
                        }

                    }
                    catch (Exception exc)
                    {
                        listar = null;

                    }
                }
            }
            catch
            {
                listar = null;
            }
            return listar;
        }
        public List<Ent_Promotion_Orce> lista_prom_orce(string tip_estad,string promotion_type,
                                                        DateTime fec_ini,DateTime fec_fin,string prom_user)
        {
            List<Ent_Promotion_Orce> listar = null;
            string sqlquery = "USP_ORCE_GET_PROMOCIONES";
            DataTable dt = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {

                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@TIP_ESTAD", tip_estad);
                            cmd.Parameters.AddWithValue("@PROMOTION_TYPE", promotion_type);
                            cmd.Parameters.AddWithValue("@fec_ini", fec_ini);
                            cmd.Parameters.AddWithValue("@fec_FIN", fec_fin);
                            cmd.Parameters.AddWithValue("@PROM_USER", prom_user);
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_Promotion_Orce>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_Promotion_Orce()
                                          {
                                            CAMPAIGN_ID=dr["CAMPAIGN_ID"].ToString(),
                                            PROMOTION_ID = dr["PROMOTION_ID"].ToString(),
                                            PROMOTION_NAME = dr["PROMOTION_NAME"].ToString(),
                                            TIPO_PROMOCION = dr["TIPO_PROMOCION"].ToString(),
                                            FEC_INICIO = (dr["FEC_INICIO"] == null) ? null : Convert.ToDateTime(dr["FEC_INICIO"]).ToString("dd-MM-yyyy"),                                          
                                            FEC_FIN = (dr["FEC_FIN"] == null) ? null : Convert.ToDateTime(dr["FEC_FIN"]).ToString("dd-MM-yyyy"),                                              
                                            HORA_INICIO = dr["HORA_INICIO"].ToString(),
                                            HORA_FIN = dr["HORA_FIN"].ToString(),
                                            FEC_CREACION = (dr["FEC_CREACION"] == null) ? null : Convert.ToDateTime(dr["FEC_CREACION"]).ToString("dd-MM-yyyy"),
                                            USUARIO_CREACION = dr["USUARIO_CREACION"].ToString(),
                                            ESTADO = dr["ESTADO"].ToString(),
                                            ULTIMA_EXPORTACION = (dr["ULTIMA_EXPORTACION"] == null) ? null : Convert.ToDateTime(dr["ULTIMA_EXPORTACION"]).ToString("dd-MM-yyyy"),                                                                                        
                                            UBIC_INCLUIDAS = dr["UBIC_INCLUIDAS"].ToString(),
                                            UBIC_EXCLUIDAS = dr["UBIC_EXCLUIDAS"].ToString(),
                                            SEGMENTO_OBJETIVO = dr["SEGMENTO_OBJETIVO"].ToString(),
                                            PROMO_ACTIVADO = dr["PROMO_ACTIVADO"].ToString(),
                                            EXPORTS = dr["EXPORTS"].ToString(),
                                            ID_OFERTA = dr["ID_OFERTA"].ToString(),
                                            CODIGO_OFERTA = dr["CODIGO_OFERTA"].ToString(),
                                            CODIGO_CUPON = dr["CODIGO_CUPON"].ToString(),
                                            CANT_CUPON_GEN = (dr["CANT_CUPON_GEN"] is DBNull) ? (decimal?)null : Convert.ToDecimal(dr["CANT_CUPON_GEN"]),
                                            NAME_OFERTA = dr["NAME_OFERTA"].ToString(),
                                            NOMBRE_PDV = dr["NOMBRE_PDV"].ToString(),
                                            TIPO_OFERTA = dr["TIPO_OFERTA"].ToString(),
                                            CALIF_INCL = dr["CALIF_INCL"].ToString(),
                                            CALIF_EXCL = dr["CALIF_EXCL"].ToString(),
                                            AWARD_INCL = dr["AWARD_INCL"].ToString(),
                                            AWARD_EXCL = dr["AWARD_EXCL"].ToString(),
                                            TIPO_PROMO_PREVISTA = dr["TIPO_PROMO_PREVISTA"].ToString(),
                                            SUBTOTAL_MIN = (dr["SUBTOTAL_MIN"] is DBNull)? (decimal?)null:Convert.ToDecimal(dr["SUBTOTAL_MIN"]),
                                            SUBTOTAL_MAX = (dr["SUBTOTAL_MAX"] is DBNull) ? (decimal?)null : Convert.ToDecimal(dr["SUBTOTAL_MAX"]),                                          
                                            IMPORTE_MAX_PREMIO = (dr["IMPORTE_MAX_PREMIO"] is DBNull) ? (decimal?)null : Convert.ToDecimal(dr["IMPORTE_MAX_PREMIO"]),                                              
                                            DEAL_ACTIVADO = dr["DEAL_ACTIVADO"].ToString(),
                                            ESTILO_UMBRAL = dr["ESTILO_UMBRAL"].ToString(),
                                            TIPO_UMBRAL = dr["TIPO_UMBRAL"].ToString(),
                                            UMBRAL = (dr["UMBRAL"] is DBNull) ? (decimal?)null : Convert.ToDecimal(dr["UMBRAL"]),                                              
                                            TIPO_DESCUENTO = dr["TIPO_DESCUENTO"].ToString(),
                                            VALOR_DESCUENTO = (dr["VALOR_DESCUENTO"] is DBNull) ? (decimal?)null : Convert.ToDecimal(dr["VALOR_DESCUENTO"]),
                                            CANTIDAD_MAXIMA =  (dr["CANTIDAD_MAXIMA"] is DBNull) ? (decimal?)null : Convert.ToDecimal(dr["CANTIDAD_MAXIMA"]),
                                            LIMITE_OFERTA = (dr["LIMITE_OFERTA"] == null) ? (decimal?)null : Convert.ToDecimal(dr["LIMITE_OFERTA"]),
                                            DESCUENTO_PRORRATEADO = dr["DESCUENTO_PRORRATEADO"].ToString(),
                                          }
                                        ).ToList();
                            }
                        }

                    }
                    catch(Exception exc) 
                    {
                        listar = null;

                    }
                }
            }
            catch 
            {
                listar = null;            
            }
            return listar;
        }
        public Ent_Promotion_Orce_Lista lista_tipo_param()
        {
            Ent_Promotion_Orce_Lista lista_param = null;
            string sqlquery = "USP_ORCE_GET_FILTROS_PROMOS";
            DataTable dt = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                dt = new DataTable();
                                da.Fill(dt);
                                //List<Ent_Promotion_Orce_Filtros> lista_filtros = new List<Ent_Promotion_Orce_Filtros>();
                                //lista_filtros = (from DataRow fila in dt.Rows
                                //                 select new Ent_Promotion_Orce_Filtros()
                                //                 {
                                //                     PROMOTION_TYPE=fila["PROMOTION_TYPE"].ToString(),
                                //                     CREATE_USER = fila["CREATE_USER"].ToString(),
                                //                     STATUS = fila["STATUS"].ToString(),                                                  

                                //                 }
                                //               ).ToList();

                                if (dt != null)
                                {
                                    if (dt.Rows.Count>0)
                                    {
                                        lista_param = new Ent_Promotion_Orce_Lista();

                                        lista_param.Promotion_Orce_Status = (from item in dt.AsEnumerable()
                                                                            group item by
                                                                            new
                                                                            {
                                                                               CODIGO = item["STATUS"].ToString(),
                                                                               DESCRIPCION = item["STATUS"].ToString(),
                                                                            }
                                                                          into G
                                                                            select new Ent_Promotion_Orce_Status()
                                                                            {
                                                                                codigo=G.Key.CODIGO,
                                                                                descripcion=G.Key.DESCRIPCION,
                                                                            }).ToList();

                                        lista_param.Promotion_Orce_Type = (from item in dt.AsEnumerable()
                                                                             group item by
                                                                             new
                                                                             {
                                                                                 CODIGO = item["PROMOTION_TYPE"].ToString(),
                                                                                 DESCRIPCION = item["PROMOTION_TYPE"].ToString(),
                                                                             }
                                                                         into G
                                                                             select new Ent_Promotion_Orce_Type()
                                                                             {
                                                                                 codigo = G.Key.CODIGO,
                                                                                 descripcion = G.Key.DESCRIPCION,
                                                                             }).ToList();

                                        lista_param.Promotion_Orce_User = (from item in dt.AsEnumerable()
                                                                           group item by
                                                                           new
                                                                           {
                                                                               CODIGO = item["CREATE_USER"].ToString(),
                                                                               DESCRIPCION = item["CREATE_USER"].ToString(),
                                                                           }
                                                                       into G
                                                                           select new Ent_Promotion_Orce_User()
                                                                           {
                                                                               codigo = G.Key.CODIGO,
                                                                               descripcion = G.Key.DESCRIPCION,
                                                                           }).ToList();

                                    }
                                }

                            }
                        }
                    }
                    catch 
                    {

                        
                    }
                }
            }
            catch
            {

                
            }
            return lista_param;
        }
    }
}
