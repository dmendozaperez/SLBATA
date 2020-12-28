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
                                              TOTAL = dr["TOTAL"].ToString(),
                                              ESTADO = dr["ESTADO"].ToString()
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
            string rpta = "";
            try
            {
                SqlConnection cn = new SqlConnection(Ent_Conexion.conexion);
                cn.Open();
                SqlCommand oComando = new SqlCommand("USP_XSTORE_UPD_SYSTEM_STORE_ENVIO", cn);
                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@TRAN_ID", cadena);
                oComando.Parameters.AddWithValue("@USU", usu_id);
                SqlDataReader oReader = oComando.ExecuteReader(CommandBehavior.SingleResult);
                rpta = "1";
                cn.Close();
            }
            catch (Exception ex)
            {
                return rpta = ex.Message;
            }
            return rpta;
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
        public List<Ent_Extender_NC> LisXCenter_NC(Ent_Extender_NC ent)
        {
            List<Ent_Extender_NC> Listar = new List<Ent_Extender_NC>();
            string sqlquery = "[USP_XCENTER_GET_NOTAS_CREDITO]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TIENDA", DbType.String).Value = ent.Tienda;
                        cmd.Parameters.AddWithValue("@NUM_DOC", DbType.String).Value = ent.Num_Doc;
                        cmd.Parameters.AddWithValue("@FECHA_INI", DbType.DateTime).Value = ent.FechaInicio;
                        cmd.Parameters.AddWithValue("@FECHA_FIN", DbType.DateTime).Value = ent.FechaFin;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            Listar = new List<Ent_Extender_NC>();
                            Listar = (from DataRow fila in dt.Rows
                                      select new Ent_Extender_NC()
                                      {
                                          Serial_Nbr = (fila["Serial_Nbr"] is DBNull) ? string.Empty : (string)(fila["Serial_Nbr"]),
                                          Organization_Id = (fila["Organization_Id"] is DBNull) ? (int?)null : Convert.ToInt32(fila["Organization_Id"]),
                                          Rtl_Loc_Id = (fila["Rtl_Loc_Id"] is DBNull) ? (int?)null : Convert.ToInt32(fila["Rtl_Loc_Id"]),
                                          Wkstn_Id = (fila["Wkstn_Id"] is DBNull) ? (int?)null : Convert.ToInt32(fila["Wkstn_Id"]),
                                          Trans_Seq = (fila["Trans_Seq"] is DBNull) ? (int?)null : Convert.ToInt32(fila["Trans_Seq"]),
                                          String_Value = (fila["String_Value"] is DBNull) ? string.Empty : (string)(fila["String_Value"]),
                                          Business_Date = (fila["Business_Date"] is DBNull) ? (DateTime?)null : Convert.ToDateTime(fila["Business_Date"]),
                                          Expr_Date = (fila["Expr_Date"] is DBNull) ? (DateTime?)null : Convert.ToDateTime(fila["Expr_Date"])
                                      }
                                    ).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Listar;
        }
        
        public bool UpExtender_NC(Ent_Extender_NC ent, ref string Estado)
        {
            bool result= false;
            string sqlquery = "USP_XCENTER_EXTENDER_NOTAS_CREDITO";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Ent_Conexion.conexion);
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TIENDA", ent.Rtl_Loc_Id);
                cmd.Parameters.AddWithValue("@NUM_NC", ent.String_Value);
                cmd.Parameters.AddWithValue("@FEC_NC", ent.Business_Date);
                cmd.Parameters.AddWithValue("@NEW_FECHA", ent.New_Expr_Date);
                cmd.Parameters.Add("@ESTADO", SqlDbType.VarChar, 30);

                cmd.Parameters["@ESTADO"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                Estado = cmd.Parameters["@ESTADO"].Value.ToString();
                result = true;
            }
            catch (Exception exc)
            {
                result = false;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return result;
        }

    }
}
