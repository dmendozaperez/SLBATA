using CapaEntidad.XstoreTda;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.NetworkInformation;



namespace CapaDato.Maestros
{
    public class Dat_XstoreTienda
    {
        public List<Ent_TiendaConf> List_Tienda_config(string pais)
        {
            string sqlquery = "USP_LISTAR_TIENDA";
            List<Ent_TiendaConf> list = null;
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

                            cmd.Parameters.AddWithValue("@pais", pais);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                list = new List<Ent_TiendaConf>();
                                list = (from DataRow dr in dt.Rows
                                        select new Ent_TiendaConf()
                                        {

                                            cod_Entid = dr["COD_ENTID"].ToString(),
                                            des_Entid = dr["DES_ENTID"].ToString(),
                                            cod_Emp = dr["COD_EMP"].ToString(),
                                            des_Emp = dr["DES_EMP"].ToString(),
                                            des_Cadena = dr["DES_CAD"].ToString(),
                                            direccion = dr["DIRECCION"].ToString(),
                                            cod_Jefe = dr["COD_JEFE"].ToString(),
                                            consecionario = dr["CONSECIONARIO"].ToString(),
                                            bol_xstore = dr["XSTORE"].ToString(),
                                            bol_gcorrelativo = dr["CORRE_GENERADO"].ToString(),
                                            outlet = dr["OUTLET"].ToString(),

                                        }).ToList();

                            }


                        }
                    }
                    catch (Exception)
                    {
                        list = null;
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch
            {
                list = null;
            }
            return list;
        }

        public int ActualizarTiendaXstore(string codTienda, Int32 Estado, decimal usuario)
        {

            Int32 intRespuesta = 0;

            string sqlquery = "USP_ACTIVAR_TIENDA_XSTORE";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {

                cn = new SqlConnection(Ent_Conexion.conexion);
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodTienda", codTienda);
                cmd.Parameters.AddWithValue("@Estado", Estado);
                cmd.Parameters.AddWithValue("@usuUpd", usuario);
                cmd.ExecuteNonQuery();
                intRespuesta = 1;

            }
            catch (Exception ex)
            {

                intRespuesta = -1;

            }

            return intRespuesta;
        }

        public int GenerarCorrelativoTiendaXstore(string codTienda, decimal usuario)
        {

            Int32 intRespuesta = 0;

            string sqlquery = "USP_SETEAR_CORRELATIVOS_TDA_WEB";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {

                cn = new SqlConnection(Ent_Conexion.conexion);
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COD_TDA", codTienda);
                cmd.Parameters.AddWithValue("@USU", usuario);
                cmd.ExecuteNonQuery();
                intRespuesta = 1;

            }
            catch (Exception ex)
            {

                intRespuesta = -1;

            }

            return intRespuesta;
        }

        public string listarStr_DatosTienda(string cod_tda)
        {
            string strJson = "";
            try
            {
                SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru);
                cn.Open();
                SqlCommand oComando = new SqlCommand("USP_OBTENER_DATOS_TIENDA", cn);
                oComando.CommandType = CommandType.StoredProcedure;

                SqlParameter ocod_tda = oComando.Parameters.Add("@TIENDA", SqlDbType.VarChar);
                ocod_tda.Direction = ParameterDirection.Input;
                ocod_tda.Value = cod_tda;

                SqlDataReader oReader = oComando.ExecuteReader(CommandBehavior.SingleResult);
                DataTable dataTable = new DataTable("row");
                dataTable.Load(oReader);

                strJson = JsonConvert.SerializeObject(dataTable, Newtonsoft.Json.Formatting.Indented);
                strJson = strJson.Replace(Environment.NewLine, "");
                //strJson = strJson.Replace(" ", "");
                cn.Close();
            }
            catch (Exception ex)
            {

                return strJson;
            }

            //return oLista;
            return strJson;
        }

        public string listarStr_InterfacexDefecto(String cod_tda)
        {
            string strJson = "";
            try
            {
                SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru);
                cn.Open();
                SqlCommand oComando = new SqlCommand("USP_OBTENER_INTERFACE_XDEFECTO", cn);
                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@cod_tda", cod_tda);
                SqlDataReader oReader = oComando.ExecuteReader(CommandBehavior.SingleResult);
                DataTable dataTable = new DataTable("row");
                dataTable.Load(oReader);

                strJson = JsonConvert.SerializeObject(dataTable, Newtonsoft.Json.Formatting.Indented);
                strJson = strJson.Replace(Environment.NewLine, "");
                //strJson = strJson.Replace(" ", "");
                cn.Close();
            }
            catch (Exception ex)
            {

                return strJson;
            }

            //return oLista;
            return strJson;
        }
        public string listarStr_DatosXcenterTienda(string cod_tda)
        {
            string sqlquery = "USP_XSTORE_XCENTER_TIENDA";

            List<Ent_XcenterDocumento> documento = null;
            List<Ent_XcenterTienda> tienda = null;
            List<Ent_XcenterXstore> store = null;
            Ent_XcenterMaestro maestro = new Ent_XcenterMaestro();

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
                            cmd.Parameters.AddWithValue("@COD_TDA", cod_tda);

                            DataSet dsReturn = new DataSet();

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dsReturn);

                                tienda = new List<Ent_XcenterTienda>();
                                tienda = (from DataRow dr in dsReturn.Tables[0].Rows
                                          select new Ent_XcenterTienda()
                                          {
                                              rtl_loc_id = dr["RTL_LOC_ID"].ToString(),
                                              description = dr["DESCRIPTION"].ToString(),
                                              address1 = dr["ADDRESS1"].ToString(),
                                              store_name = dr["STORE_NAME"].ToString(),
                                              store_manager = dr["STORE_MANAGER"].ToString(),
                                              xs_calzado = dr["XS_CALZADO"].ToString(),
                                              xs_no_calzado = dr["XS_NO_CALZADO"].ToString(),
                                              xs_total = dr["XS_TOTAL"].ToString()
                                          }).ToList();

                                store = new List<Ent_XcenterXstore>();
                                store = (from DataRow dr in dsReturn.Tables[1].Rows
                                         select new Ent_XcenterXstore()
                                         {
                                             rtl_loc_id = dr["RTL_LOC_ID"].ToString(),
                                             wkstn_id = dr["WKSTN_ID"].ToString(),
                                             ip_address = dr["IP_ADDRESS"].ToString(),
                                             xstore_version = dr["XSTORE_VERSION"].ToString()
                                         }).ToList();

                                documento = new List<Ent_XcenterDocumento>();
                                documento = (from DataRow dr in dsReturn.Tables[2].Rows
                                             select new Ent_XcenterDocumento()
                                             {
                                                 rtl_loc_id = dr["RTL_LOC_ID"].ToString(),
                                                 tipo_doc = dr["TIPO_DOC"].ToString(),
                                                 serie = dr["SERIE"].ToString(),
                                                 correlativo = dr["CORRELATIVO"].ToString()
                                             }).ToList();
                            }

                        }
                    }
                    catch (Exception)
                    {
                        maestro = null;
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }

                maestro.tienda = tienda;
                maestro.xstore = store;
                maestro.documento = documento;

            }
            catch (Exception)
            {
                maestro = null;
            }

            string strJson = JsonConvert.SerializeObject(maestro);

            return strJson;
        }

        #region **Config Conexion**
        public Ent_ConfigConexion XSTORE_GET_CONEXION_GLOBAL(string pais)
        {
            string sql = "USP_XSTORE_GET_CONEXION_GLOBAL";
            DataSet dsResponse = null;
            Ent_ConfigConexion configConexion = null;
            List<Ent_Central_Xst> listCentralXst = null;
            List<Ent_Cajas_Xst> listCajasXst = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    dsResponse = new DataSet();
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pais", pais);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dsResponse);
                    listCentralXst = new List<Ent_Central_Xst>();
                    listCentralXst = (from DataRow dr in dsResponse.Tables[0].Rows
                                      select new Ent_Central_Xst()
                                      {
                                          IP_CENTRAL = dr["IP_CENTRAL"].ToString(),
                                          DES_CENTRAL = dr["DES_CENTRAL"].ToString(),
                                          ESTADO_CONEXION_CENTRAL_XST = PingHost(dr["IP_CENTRAL"].ToString())
                                      }).ToList();
                    listCajasXst = new List<Ent_Cajas_Xst>();
                    listCajasXst = (from DataRow dr in dsResponse.Tables[1].Rows
                                    select new Ent_Cajas_Xst()
                                    {
                                        COD_ENTID = dr["COD_ENTID"].ToString(),
                                        TIENDA = dr["TIENDA"].ToString(),
                                        NCAJA = dr["NCAJA"].ToString(),
                                        IP = dr["IP"].ToString(),
                                        VERSION_XST = dr["VERSION_XST"].ToString(),
                                        ESTADO_CONEXION_CAJA_XST = PingHost(dr["IP"].ToString())
                                    }).ToList();
                    configConexion = new Ent_ConfigConexion();
                    configConexion.list_cajas_xst = listCajasXst;
                    configConexion.list_central_xst = listCentralXst;
                }
            }
            catch (Exception ex)
            {
                configConexion = null;
            }
            return configConexion;
        }
        public int PingHost(string nameOrAddress)
        {
            try
            {
                Ping Pings = new Ping();
                int timeout = 1000;

                if (Pings.Send(nameOrAddress, timeout).Status == IPStatus.Success)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

            //bool pingable = false;
            //Ping pinger = null;

            //try
            //{
            //    pinger = new Ping();
            //    PingReply reply = pinger.Send(nameOrAddress);
            //    pingable = reply.Status == IPStatus.Success;
            //}
            //catch (PingException)
            //{
            //    // Discard PingExceptions and return false;
            //    pingable = false;
            //}
            //finally
            //{
            //    if (pinger != null)
            //    {
            //        pinger.Dispose();
            //    }
            //}

            //return pingable ? 1 : 0;
        }
        #endregion

    }

}
