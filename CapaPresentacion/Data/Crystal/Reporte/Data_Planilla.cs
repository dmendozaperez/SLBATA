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
    public class Data_Planilla
    {
        public List<Models_ReglaCab> get_reglamed_cab()
        {
            string sqlquery = "[USP_Planilla_ReglaCab]";
            List<Models_ReglaCab> lista = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;                          
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                lista = new List<Models_ReglaCab>();
                                lista = (from DataRow dr in dt.Rows
                                         select new Models_ReglaCab()
                                         {
                                             med = dr["rmed"].ToString(),
                                             _00 = dr["00"].ToString(),
                                             _01 = dr["01"].ToString(),
                                             _02 = dr["02"].ToString(),
                                             _03 = dr["03"].ToString(),
                                             _04 = dr["04"].ToString(),
                                             _05 = dr["05"].ToString(),
                                             _06 = dr["06"].ToString(),
                                             _07 = dr["07"].ToString(),
                                             _08 = dr["08"].ToString(),
                                             _09 = dr["09"].ToString(),
                                             _10 = dr["10"].ToString(),
                                             _11 = dr["11"].ToString(),
                                         }).ToList();
                            }
                        }
                    }
                    catch (Exception exc)
                    {


                    }
                }
            }
            catch (Exception)
            {

            }
            return lista;
        }
    
        public List<Models_Planilla> get_planilla(string cod_tda, string grupo, string categoria, string subcategoria, string estado, string tipo,string tipo_rep="-1", string calidad="1")
        {
            string sqlquery = "[USP_ReportePlanilla]";
            List<Models_Planilla> lista = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@cod_tda", cod_tda);
                            cmd.Parameters.AddWithValue("@Grupo", grupo);
                            cmd.Parameters.AddWithValue("@Categoria", categoria);
                            cmd.Parameters.AddWithValue("@SubCategoria", subcategoria);
                            cmd.Parameters.AddWithValue("@Estado", estado);
                            cmd.Parameters.AddWithValue("@Tipo", tipo);
                            cmd.Parameters.AddWithValue("@tip_rep", tipo_rep);
                            cmd.Parameters.AddWithValue("@calidad", calidad);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                lista = new List<Models_Planilla>();
                                lista = (from DataRow dr in dt.Rows
                                         select new Models_Planilla()
                                         {
                                             semana_str = dr["semana_str"].ToString(),
                                             store_name = dr["store_name"].ToString(),
                                             des_entid = dr["des_entid"].ToString(),
                                             cate3_des = dr["cate3_des"].ToString(),
                                             subcat3_des = dr["subcat3_des"].ToString(),
                                             articulo = dr["articulo"].ToString(),                                             
                                             calidad = dr["calidad"].ToString(),
                                             grupo = dr["grupo"].ToString(),
                                             cate3 = dr["cate3"].ToString(),
                                             subcat3 = dr["subcat3"].ToString(),
                                             pventa =string.IsNullOrEmpty(dr["pventa"].ToString())?0:Convert.ToDecimal(dr["pventa"].ToString()),

                                             vta_acum = string.IsNullOrEmpty(dr["vta_acum"].ToString()) ? 0 : Convert.ToDecimal(dr["vta_acum"].ToString()),

                                             tcant = string.IsNullOrEmpty(dr["tcant"].ToString()) ? 0 : Convert.ToDecimal(dr["tcant"].ToString()), //Convert.ToDecimal(dr["tcant"]),

                                             valor = string.IsNullOrEmpty(dr["valor"].ToString()) ? 0 : Convert.ToDecimal(dr["valor"].ToString()), //Convert.ToDecimal(dr["valor"]),
                                             med = dr["rmed"].ToString(),
                                             reg_med= dr["reg_med"].ToString(),
                                             _00 =(tipo_rep == "-1") ?dr["00"].ToString():"",
                                             _01 = (tipo_rep == "-1") ? dr["01"].ToString() : "",
                                             _02 = (tipo_rep == "-1") ? dr["02"].ToString() : "",
                                             _03 = (tipo_rep == "-1") ? dr["03"].ToString() : "",
                                             _04 = (tipo_rep == "-1") ? dr["04"].ToString() : "",
                                             _05 = (tipo_rep == "-1") ? dr["05"].ToString() : "",
                                             _06 = (tipo_rep == "-1") ? dr["06"].ToString() : "",
                                             _07 = (tipo_rep == "-1") ? dr["07"].ToString() : "",
                                             _08 = (tipo_rep == "-1") ? dr["08"].ToString() : "",
                                             _09 = (tipo_rep == "-1") ? dr["09"].ToString() : "",
                                             _10 = (tipo_rep == "-1") ? dr["10"].ToString() : "",
                                             _11 = (tipo_rep == "-1") ? dr["11"].ToString() : "",
                                         }).ToList();
                            }
                        }
                    }
                    catch (Exception exc)
                    {


                    }
                }
            }
            catch (Exception)
            {

            }
            return lista;
        }

        public Reporte_Vendedor get_reporteVendedor(/*string coddis,*/string cod_tda, string fecIni, string fecFin, string calidad)
        {
            string sqlquery = "USP_XSTORE_REPORTE_VENDEDORES";
            Reporte_Vendedor lista = null;
            List<Models_Vendedor> lista1 = null;
            List<Models_Total2> lista2 = null;
            try
            {
             
                if (cod_tda.Substring(0, 1).ToString() == "0") cod_tda = "0";

                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@codtda", cod_tda);
                            cmd.Parameters.AddWithValue("@FEC_INI", fecIni);
                            cmd.Parameters.AddWithValue("@FEC_FIN", fecFin);
                            //cmd.Parameters.AddWithValue("@coddist", coddis);
                            cmd.Parameters.AddWithValue("@calidad", calidad);
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataSet ds = new DataSet();
                                da.Fill(ds);
                                lista1 = new List<Models_Vendedor>();
                                lista1 = (from DataRow dr in ds.Tables[0].Rows
                                         select new Models_Vendedor()
                                         {
                                             cod_distri=dr["DISTRITO"].ToString(),
                                             des_cadena = dr["DES_CADENA"].ToString(),
                                             cod_entid = dr["COD_ENTID"].ToString(),
                                             des_entid = dr["DES_ENTID"].ToString(),
                                             store_name = dr["STORE_NAME"].ToString(),
                                             semana_str = dr["SEMANA_STR"].ToString(),
                                             dni = dr["DNI"].ToString(),
                                             dni_nombre= dr["DNI_NOMBRE"].ToString(),
                                             pares = string.IsNullOrEmpty(dr["PARES"].ToString()) ? 0 : Convert.ToDecimal(dr["PARES"].ToString()),
                                             ropa = string.IsNullOrEmpty(dr["ROPA"].ToString()) ? 0 : Convert.ToDecimal(dr["ROPA"].ToString()), //Convert.ToDecimal(dr["tcant"]),
                                             acc = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["ACC"].ToString()), //Convert.ToDecimal(dr["valor"]),
                                             cant_total = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["CANT_TOTAL"].ToString()),
                                             neto = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["NETO"].ToString()),
                                             igv = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["IGV"].ToString()),
                                             total = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["TOTAL"].ToString()),
                                             upt = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["UPT"].ToString()),
                                             ntk = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["NTK"].ToString()),
                                             ntk2 = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["NTK2"].ToString()),
                                             mayor_1 = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["MAYOR_1"].ToString()),
                                             pormay1 = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["PORMAY1"].ToString()),
                                             ticket_prom = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["TICKET_PROM"].ToString()),
                                             upt_un= string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["UPT_UN"].ToString()),

                                         }).ToList();
                                lista2 = new List<Models_Total2>();
                                lista2 = (from DataRow dr in ds.Tables[1].Rows
                                          select new Models_Total2()
                                          {
                                              COD_ENTID_2 = dr["COD_ENTID_2"].ToString(),
                                              SUM_CANT_TOTAL = Convert.ToInt32(dr["SUM_CANT_TOTAL"].ToString()),
                                              SUM_NTK_TOTAL = Convert.ToInt32(dr["SUM_NTK_TOTAL"].ToString()),
                                              TOTAL_2 = Convert.ToDecimal(dr["TOTAL_2"].ToString()),
                                              UPT_2 = Convert.ToDecimal(dr["UPT_2"].ToString()),
                                              TICKET_PROM_2 = Convert.ToDecimal(dr["TICKET_PROM_2"].ToString()),
                                          }).ToList();
                                lista = new Reporte_Vendedor();
                                lista.listMV = lista1;
                                lista.listTotal2 = lista2;                                                          
                            }
                        }
                    }
                    catch (Exception exc)
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

        public DataTable get_reportePromociones(string cadena, string fecIni, string fecFin,string filtro, string tipo)
        {
            string sqlquery = (tipo == "RESUMIDO" ? "USP_REPORTE_PROMOCIONES" : "USP_REPORTE_PROMOCIONES_DETALLE");
            DataTable dt = new DataTable(); ;
            try
            {           

                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@cadena", cadena);
                            cmd.Parameters.AddWithValue("@fec_ini", fecIni);
                            cmd.Parameters.AddWithValue("@fec_fin", fecFin);
                            cmd.Parameters.AddWithValue("@filtro", filtro);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);                                
                            }
                        }
                    }
                    catch (Exception exc)
                    {


                    }
                }
            }
            catch (Exception)
            {

            }
            return dt;
        }
    }
}