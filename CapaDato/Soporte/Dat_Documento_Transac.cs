using CapaEntidad.Soporte;
using CapaEntidad.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Soporte
{
    public class Dat_Documento_Transac
    {
        //Listado Tabla principal
        public List<Ent_Documento_Transac> get_lista(string cod_entid, DateTime fec_ini, DateTime fec_fin)
        {
            string sqlquery = "USP_XSTORE_GET_DOCUMENTO";
            List<Ent_Documento_Transac> listar = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    try
                    {
                        if (cn.State == 0) cn.Open();
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@COD_ENTID", cod_entid);
                            cmd.Parameters.AddWithValue("@FEC_INI", fec_ini);
                            cmd.Parameters.AddWithValue("@FEC_FIN", fec_fin);
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_Documento_Transac>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_Documento_Transac()
                                          {
                                              cod_entid = dr["COD_ENTID"].ToString(),
                                              tienda_origen = dr["TIENDA"].ToString(),
                                             // fecha_des = dr["FECHA"].ToString(),
                                              fecha_des = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["FECHA"])),
                                              tran_id = dr["TRAN_ID"].ToString(),
                                              flg_novell = Convert.ToBoolean( dr["NOVEL"]),
                                              fec_env = dr["NOVEL_FEC_ENVIO"].ToString()
                                          }).ToList();
                            }

                        }
                    }
                    catch (Exception)
                    {
                        listar = null;
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception)
            {
                listar = null;
            }
            return listar;
        }

        ///Listado Tabla de Detalle - PopUp
        public List<Ent_Documento_TransacDoc> listarStr_Detalle_Pop(string cod_tda, DateTime fecha_des)
        {
            string sqlquery = "USP_XSTORE_GET_DOCUMENTO_DET";
            List<Ent_Documento_TransacDoc> listar = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    try
                    {
                        if (cn.State == 0) cn.Open();
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@COD_ENTID", cod_tda);
                            cmd.Parameters.AddWithValue("@FECHA", fecha_des);                  
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_Documento_TransacDoc>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_Documento_TransacDoc()
                                          {
                                              TIPO_DOC = dr["TIPO_DOC"].ToString(),
                                              NUM_FAC = dr["NUM_FAC"].ToString(),
                                              SERIE = dr["SERIE"].ToString(),
                                              TOTAL = dr["TOTAL"].ToString()
                                          }).ToList();
                            }

                        }
                    }
                    catch (Exception)
                    {
                        listar = null;
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception)
            {
                listar = null;
            }
            return listar;
        }

        //Envío de paquetes de checkbox seleccionados
        public string Envio_chk(string cadena, decimal usu_id)
        {
            string strJson = "";
            try
            {
                SqlConnection cn = new SqlConnection(Ent_Conexion.conexion);
                cn.Open();
                SqlCommand oComando = new SqlCommand("USP_XSTORE_UPD_SYSTEM_STORE_ENVIO", cn);
                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@TRAN_ID", cadena);
                oComando.Parameters.AddWithValue("@USU", usu_id);
                SqlDataReader oReader = oComando.ExecuteReader(CommandBehavior.SingleResult);
                strJson = "1";
                cn.Close();
            }
            catch (Exception ex)
            {
                return strJson= ex.Message;
            }
            return strJson;
        }

        ///Listado de tabla detalle anterior
        //public string listarStr_Detalle(string cod_tda,DateTime fecha_des)
        //{
        //    string strJson = "";
        //    try
        //    {
        //        SqlConnection cn = new SqlConnection(Ent_Conexion.conexion);
        //        cn.Open();
        //        SqlCommand oComando = new SqlCommand("USP_XSTORE_GET_DOCUMENTO_DET", cn);
        //        oComando.CommandType = CommandType.StoredProcedure;

        //        oComando.Parameters.AddWithValue("@COD_ENTID", cod_tda);
        //        oComando.Parameters.AddWithValue("@FECHA", fecha_des);

        //        SqlDataReader oReader = oComando.ExecuteReader(CommandBehavior.SingleResult);
        //        DataTable dataTable = new DataTable("row");
        //        dataTable.Load(oReader);

        //        strJson = JsonConvert.SerializeObject(dataTable, Formatting.Indented);
        //        strJson = strJson.Replace(Environment.NewLine, "");
        //        cn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        return strJson;
        //    }
        //    return strJson;
        //}


        //gft
    }
}
