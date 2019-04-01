using CapaEntidad.Util;
using Models.Crystal.Reporte;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Data.Crystal.Reporte
{
    public class Data_Bata
    {
        public List<Models_Art_Sin_Mov> list_art_sin_mov(string cadena,string tienda,Int32 nsemana,Int32 maxpares, string estado)
        {
            string sqlquery = "USP_XSTORE_REPORTE_ART_SIN_MOVIMIENTOS";
            List<Models_Art_Sin_Mov> lista = null;
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
                            cmd.Parameters.AddWithValue("@cadena", cadena);
                            cmd.Parameters.AddWithValue("@codtda", tienda);
                            cmd.Parameters.AddWithValue("@nsemanas", nsemana);
                            cmd.Parameters.AddWithValue("@nstock", maxpares);
                            cmd.Parameters.AddWithValue("@estado", estado);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                dt = new DataTable();
                                da.Fill(dt);
                                lista = new List<Models_Art_Sin_Mov>();
                                lista = (from DataRow dr in dt.Rows
                                         select new Models_Art_Sin_Mov()
                                         {
                                             tiend=dr["tiend"].ToString(),
                                             des_entid= dr["des_entid"].ToString(),
                                             store_name = dr["storename"].ToString(),
                                             semana_str = dr["semana_str"].ToString(),
                                             cate3 = dr["cate3"].ToString(),
                                             subc3 = dr["subc3"].ToString(),
                                             artic = dr["artic"].ToString(),
                                             pplan =Convert.ToDecimal(dr["pplan"]),
                                             pares =Convert.ToInt32(dr["pares"]),
                                             stock =Convert.ToInt32(dr["stock"]),
                                         }).ToList();
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
            return lista;
        }

        public List<Models_Comparativo_Venta> list_comparativo_venta(string codEntid, string fecIni_1, string fecFin_1, string fecIni_2, string fecFin_2, string idcomparativo)
        {
            string sqlquery = "USP_XSTORE_COMPARATIVO_VENTAS_NEW";
            List<Models_Comparativo_Venta> lista = null;
            DataTable dt = null;
            string[] arrFi1 = fecIni_1.Split('-');
            string[] arrFf1 = fecFin_1.Split('-');

            string[] arrFi2 = fecIni_2.Split('-');
            string[] arrFf2 = fecFin_2.Split('-');

            string strFi1 = arrFi1[2] + arrFi1[1] + arrFi1[0];
            string strFf1 = arrFf1[2] + arrFf1[1] + arrFf1[0];

            string strFi2 = arrFi2[2] + arrFi2[1] + arrFi2[0];
            string strFf2 = arrFf2[2] + arrFf2[1] + arrFf2[0];

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
                            cmd.Parameters.AddWithValue("@FEC_INI1", strFi1);
                            cmd.Parameters.AddWithValue("@FEC_FIN1", strFf1);
                            cmd.Parameters.AddWithValue("@FEC_INI2", strFi2);
                            cmd.Parameters.AddWithValue("@FEC_FIN2", strFf2);
                            cmd.Parameters.AddWithValue("@codtda", codEntid);
                            cmd.Parameters.AddWithValue("@comparativo", idcomparativo);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                dt = new DataTable();
                                da.Fill(dt);
                                lista = new List<Models_Comparativo_Venta>();
                                lista = (from DataRow dr in dt.Rows
                                         select new Models_Comparativo_Venta()
                                         {
                                             orden = dr["ORDEN"].ToString(),
                                             cod_entid = dr["COD_ENTID"].ToString(),
                                             des_entid = dr["DES_ENTID"].ToString(),
                                             rango = dr["RANGO"].ToString(),
                                             pares = dr["PAR1"].ToString(),
                                             ropa = dr["ROP1"].ToString(),
                                             acc = dr["ACC1"].ToString(),
                                             cant_total = dr["CANT_TOTAL1"].ToString(),
                                             neto = dr["NETO1"].ToString()
                                         }).ToList();
                            }
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lista;
        }
        public List<Models_Obs> list_obs (string cod_distri,string codtda,string tipo_cat,string cod_linea,string cod_categ,
                                          string calidad,Decimal rprecio1,Decimal rprecio2,string tipo_obs,string rango_obs  )
        {
            List<Models_Obs> lista = null;
            string sqlquery = "USP_XSTORE_REPORTE_OBSOLESCENCIA";
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
                            cmd.Parameters.AddWithValue("@cod_distri", cod_distri);
                            cmd.Parameters.AddWithValue("@codtda", codtda);
                            cmd.Parameters.AddWithValue("@tipo_cat", tipo_cat);
                            cmd.Parameters.AddWithValue("@cod_linea", cod_linea);
                            cmd.Parameters.AddWithValue("@cod_categ", cod_categ);
                            cmd.Parameters.AddWithValue("@calidad", calidad);
                            cmd.Parameters.AddWithValue("@rprecio1", rprecio1);
                            cmd.Parameters.AddWithValue("@rprecio2", rprecio2);
                            cmd.Parameters.AddWithValue("@tipo_obs", tipo_obs);
                            cmd.Parameters.AddWithValue("@rango_obs", rango_obs);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                dt = new DataTable();
                                da.Fill(dt);
                                lista = new List<Models_Obs>();
                                lista = (from DataRow dr in dt.Rows
                                         select new Models_Obs()
                                         {
                                             rango_fecha =dr["RANGO_FECHA"].ToString(),
                                             distrito = dr["DISTRITO"].ToString(),
                                             tienda = dr["TIENDA"].ToString(),   
                                             tipo_cat= dr["TIPO_CAT"].ToString(),
                                             cod_linea = dr["COD_LINEA"].ToString(),
                                             cod_categ = dr["COD_CATEG"].ToString(),
                                             artic = dr["ARTIC"].ToString(),
                                             calid = dr["CALID"].ToString(),
                                             pplanilla = Convert.ToDecimal(dr["PPLANILLA"]),
                                             tip_obsol = dr["TIP_OBSOL"].ToString(),
                                             des_obsol = dr["DES_OBSOL"].ToString(),
                                             stk = Convert.ToDecimal(dr["STK"]),
                                             vtas4sem = Convert.ToInt32(dr["VTAS4SEM"]),
                                         }).ToList();
                            }
                                                    
                        }
                    }
                    catch 
                    {
                        lista = null;

                    }
                }
            }
            catch (Exception)
            {

                lista = null;
            }
            return lista;
        }
    }
}