using CapaEntidad.BataClub;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.BataClub
{
    public class Dat_BataClub_Dashboard
    {

        public Ent_BataClub_DashBoard get_info_distritos(DateTime fecini, DateTime fecfin)
        {
            string sqlquery = "[USP_BATACLUB_DASHBOARD_DISTRITOS_INFO]";
            Ent_BataClub_DashBoard data = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fecha_ini", fecini);
                        cmd.Parameters.AddWithValue("@fecha_fin", fecfin);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            if (ds != null)
                            {
                                data = new Ent_BataClub_DashBoard();
                                data.listDistritos = new List<Ent_BataClub_DashBoard_Distritos>();
                                data.listDistritos = (from DataRow dr in ds.Tables[0].Rows
                                        select new Ent_BataClub_DashBoard_Distritos()
                                        {
                                            supervisor = dr["SUPERVISOR"].ToString(),
                                            distrito = dr["DISTRITO"].ToString(),
                                            registros = Convert.ToInt32(dr["REGISTROS"].ToString()),
                                            transac = Convert.ToInt32(dr["TRANSAC"].ToString()),
                                            consumido = Convert.ToInt32(dr["CONSUMIDO"].ToString()),
                                            bataclub = Convert.ToInt32(dr["MIEM_BATACLUB"].ToString()),
                                        }
                                      ).ToList();

                                data.listDistritosTiendas = new List<Ent_BataClub_DashBoard_Tiendas_Distritos>();
                                data.listDistritosTiendas = (from DataRow dr in ds.Tables[1].Rows
                                           select new Ent_BataClub_DashBoard_Tiendas_Distritos()
                                           {
                                               supervisor = dr["SUPERVISOR"].ToString(),
                                               distrito = dr["DISTRITO"].ToString(),
                                               tienda = dr["TIENDA"].ToString(),
                                               registros = Convert.ToInt32(dr["REGISTROS"].ToString()),
                                               transac = Convert.ToInt32(dr["TRANSAC"].ToString()),
                                               consumido = Convert.ToInt32(dr["CONSUMIDO"].ToString()),
                                               bataclub = Convert.ToInt32(dr["MIEM_BATACLUB"].ToString()),
                                           }
                                      ).ToList();                                
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                data = null;
            }
            return data;
        }

        public List<Ent_BC_Venta_Categoria> get_info_venta_categoria(DateTime fecini, DateTime fecfin)
        {
            List<Ent_BC_Venta_Categoria> list = null;
            string sqlquery = "[USP_BATACLUB_DASHBOARD_VENTA_CATEGORIA]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fecha_ini", fecini);
                        cmd.Parameters.AddWithValue("@fecha_fin", fecfin);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt != null)
                            {
                                list = new List<Ent_BC_Venta_Categoria>();
                                list = (from DataRow dr in dt.Rows
                                        select new Ent_BC_Venta_Categoria()
                                        {
                                            CATEGORIA = Convert.ToString(dr["CATEGORIA"]),
                                            TOTAL_BATACLUB = Convert.ToDecimal(dr["TOTAL_BATACLUB"]),
                                            TOTAL_BATA = Convert.ToDecimal(dr["TOTAL_BATA"]),
                                        }
                                      ).ToList();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                list = null;
            }
            return list;
        }

        public List<Ent_BC_Dashboard_CVB> get_info_cump_venta(string cod_semana)
        {
            List<Ent_BC_Dashboard_CVB> list = null;
            string sqlquery = "[USP_BATACLUB_DASHBOARD_CUMPLIMIENTO_VENTA_BATA]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cod_semana", cod_semana);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt != null)
                            {
                                list = new List<Ent_BC_Dashboard_CVB>();
                                list = (from DataRow dr in dt.Rows
                                        select new Ent_BC_Dashboard_CVB()
                                        {
                                            n_semana = Convert.ToString(dr["n_semana"]),
                                            cod_entid = Convert.ToString(dr["cod_entid"]),
                                            des_entid = Convert.ToString(dr["des_entid"]),
                                            anterior = Convert.ToDecimal(dr["anterior"]),
                                            actual = Convert.ToDecimal(dr["actual"]),
                                            porc = Convert.ToDecimal(dr["porc"]),
                                            semana_ant = Convert.ToString(dr["semana_ant"]),
                                            semana_act = Convert.ToString(dr["semana_act"]),
                                        }
                                      ).ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                list = null;
            }
            return list;
        }
        public List<Ent_BC_Dashboard_Ticket_Promedio> get_info_ticket_promedio (DateTime fecini, DateTime fecfin)
        {
            List<Ent_BC_Dashboard_Ticket_Promedio> list = null;
            string sqlquery = "[USP_BATACLUB_DASHBOARD_TICKET_PROMEDIO]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FEC_INI", fecini);
                        cmd.Parameters.AddWithValue("@FEC_FIN", fecfin);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt != null)
                            {
                                list = new List<Ent_BC_Dashboard_Ticket_Promedio>();
                                list = (from DataRow dr in dt.Rows
                                        select new Ent_BC_Dashboard_Ticket_Promedio()
                                        {
                                            GRUPO = Convert.ToString (dr["GRUPO"]),
                                            TRANSAC = Convert.ToDecimal(dr["TRANSAC"]),
                                            TOTAL = Convert.ToDecimal(dr["TOTAL"]),
                                            TICKETPROM = Convert.ToDecimal(dr["TICKETPROM"]),
                                        }
                                      ).ToList();
                            }
                        }

                    }
                }
            }
            catch (Exception ex )
            {
                list = null;
            }
            return list;
        }
        public List<Ent_Bataclub_Canales_Excel> get_canales_excel(Int32 informe,DateTime fecini_canal,DateTime fecfin_canal)
        {
            List<Ent_Bataclub_Canales_Excel> list=null;
            string sqlquery = "[USP_BATACLUB_DASHBOARD_CANALES_FECHA]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fecha_ini_canal", fecini_canal);
                        cmd.Parameters.AddWithValue("@fecha_fin_canal", fecfin_canal);
                        cmd.Parameters.AddWithValue("@canal_excel", true);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt!=null)
                            {
                                list = new List<Ent_Bataclub_Canales_Excel>();
                                list = (from DataRow dr in dt.Rows
                                        select new Ent_Bataclub_Canales_Excel()
                                        {
                                            Canal = dr["canal"].ToString(),
                                            Tienda = dr["tienda"].ToString(),
                                            Dni = dr["dni"].ToString(),
                                            Nombres = dr["nombres"].ToString(),
                                            Correo = dr["correo"].ToString(),
                                            Miem_Bataclub = dr["miem_bataclub"].ToString(),
                                            Fec_Registro = dr["fec_registro"].ToString(),// (dr["fec_registro"] ==DBNull.Value)? (string?)null : Convert.ToString(dr["fec_registro"]),
                                            Fec_Miembro = dr["fec_miembro"].ToString(),//(dr["fec_miembro"] == DBNull.Value) ? (string?)null : Convert.ToDateTime(dr["fec_miembro"]),
                                        }
                                      ).ToList();
                            }
                        }

                    }
                }
            }
            catch
            {
                
            }
            return list;
        }
        public List<Ent_BataClub_Datos_Incompletos_Excel> get_datos_incompletos_excel(Int32 informe,string opcion_data_in)
        {
            List<Ent_BataClub_Datos_Incompletos_Excel> list = null;
            string sqlquery = "[USP_BATACLUB_DASHBOARD]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@INFORME", informe);
                        cmd.Parameters.AddWithValue("@opcion_data_in", opcion_data_in);                        
                        cmd.Parameters.AddWithValue("@opcion_data_in_excel", true);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt != null)
                            {
                                list = new List<Ent_BataClub_Datos_Incompletos_Excel>();
                                list = (from DataRow dr in dt.Rows
                                        select new Ent_BataClub_Datos_Incompletos_Excel()
                                        {
                                            dni = dr["DNI"].ToString(),
                                            nombres = dr["NOMBRES"].ToString(),
                                            correo = dr["CORREO"].ToString(),                                            
                                        }
                                      ).ToList();
                            }
                        }

                    }
                }
            }
            catch
            {

            }
            return list;
        }
        public List<Ent_BataClub_Compras_CL_Excel> get_compras_excel(Int32 informe, DateTime fecini_com_cl, DateTime fecfin_com_cl)
        {
            List<Ent_BataClub_Compras_CL_Excel> list = null;
            string sqlquery = "[USP_BATACLUB_DASHBOARD]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@INFORME", informe);
                        cmd.Parameters.AddWithValue("@fecha_ini_compras", fecini_com_cl);
                        cmd.Parameters.AddWithValue("@fecha_fin_compras", fecfin_com_cl);
                        cmd.Parameters.AddWithValue("@compra_excel", true);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt != null)
                            {
                                list = new List<Ent_BataClub_Compras_CL_Excel>();
                                list = (from DataRow dr in dt.Rows
                                        select new Ent_BataClub_Compras_CL_Excel()
                                        {
                                            dni = dr["dni"].ToString(),
                                            correo = dr["correo"].ToString(),
                                            compras =Convert.ToDecimal(dr["compras"]),                                            
                                        }
                                      ).ToList();
                            }
                        }

                    }
                }
            }
            catch
            {

            }
            return list;
        }
        public Ent_BataClub_DashBoard GET_INFO_DASHBOARD(ref Ent_BataClub_DashBoard dashboard_session, string anio = "2020" , int informe = 0, int mes = 0,object fechaIni = null , object fechaFin = null , string prom = "", 
            object fechaIni_canal = null, object fechaFin_canal = null, object fechaIni_com = null, object fechaFin_com = null, object fechaIni_com_cl = null, object fechaFin_com_cl = null,String opcion_data_in="FN",
            object fechaIni_ps = null, object fechaFin_ps = null,String canal_venta="TD,EC") // 0 = TODO | 1 = GENERAL | 2 = REGISTRADOS | 3 = MIEMBROS | 4 = CANALES
        {
            string sqlquery = "[USP_BATACLUB_DASHBOARD]";
            Ent_BataClub_DashBoard info = null;
            try
            {

                info= (dashboard_session != null) ?  dashboard_session : new Ent_BataClub_DashBoard();

              

                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ANIO", anio);
                        cmd.Parameters.AddWithValue("@INFORME", informe); 
                        cmd.Parameters.AddWithValue("@MES", mes);
                        if (fechaIni != null && fechaFin != null)
                        {
                            cmd.Parameters.AddWithValue("@fecha_ini", Convert.ToDateTime(fechaIni));
                            cmd.Parameters.AddWithValue("@fecha_fin", Convert.ToDateTime(fechaFin));
                        }

                        if (fechaIni_canal != null && fechaFin_canal != null)
                        {
                            cmd.Parameters.AddWithValue("@fecha_ini_canal", Convert.ToDateTime(fechaIni_canal));
                            cmd.Parameters.AddWithValue("@fecha_fin_canal", Convert.ToDateTime(fechaFin_canal));
                        }

                        if (fechaIni_com != null && fechaFin_com != null)
                        {
                            cmd.Parameters.AddWithValue("@fecha_ini_com", Convert.ToDateTime(fechaIni_com));
                            cmd.Parameters.AddWithValue("@fecha_fin_com", Convert.ToDateTime(fechaFin_com));
                        }
                        if (fechaIni_com_cl != null && fechaFin_com_cl != null)
                        {
                            cmd.Parameters.AddWithValue("@fecha_ini_compras", Convert.ToDateTime(fechaIni_com_cl));
                            cmd.Parameters.AddWithValue("@fecha_fin_compras", Convert.ToDateTime(fechaFin_com_cl));
                        }

                        if (fechaIni_ps != null && fechaFin_ps != null)
                        {
                            cmd.Parameters.AddWithValue("@fecha_ini_ps", Convert.ToDateTime(fechaIni_ps));
                            cmd.Parameters.AddWithValue("@fecha_fin_ps", Convert.ToDateTime(fechaFin_ps));
                        }

                        cmd.Parameters.AddWithValue("@prom", prom);//@prom
                        cmd.Parameters.AddWithValue("@opcion_data_in", opcion_data_in);//@prom

                        cmd.Parameters.AddWithValue("@canal_venta_ps", canal_venta);//@prom

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);                            
                            //info = new Ent_BataClub_DashBoard();

                            //if (new[] { 0 ,1 }.Contains(informe))
                            //    info.General = (from DataRow dr in ds.Tables[(0)].Rows
                            //                    select new Ent_BataClub_DashBoard_General()
                            //                    {
                            //                        REGISTROS = Convert.ToDecimal(dr["REGISTROS"]),
                            //                        MIEMBROS = Convert.ToDecimal(dr["MIEMBROS"]),                                                    
                            //                        TRANSAC_CUPON = Convert.ToDecimal(dr["TRANSAC_CUPON"]),
                            //                        PARES = Convert.ToDecimal(dr["PARES"]),
                            //                        SOLES = Convert.ToDecimal(dr["SOLES"])
                            //                    }).FirstOrDefault();
                            //if (new[] { 0, 2 }.Contains(informe))
                            //{                                                           
                            //    info.listMesRegistros = (from DataRow dr in ds.Tables[(informe == 0 ? 1 : 0)].Rows
                            //                        select new Ent_BataClub_DashBoard_Mensual()
                            //                        {
                            //                            ANIO = dr["ANIO"].ToString(),
                            //                            MES  = Convert.ToInt32(dr["MES"].ToString()),
                            //                            MES_STR = dr["MES_STR"].ToString(),
                            //                            NUMERO = Convert.ToDecimal(dr["REGISTROS"])
                            //                        }).ToList();
                            //    info.listMesMiembros = (from DataRow dr in ds.Tables[(informe == 0 ? 2 : 1) ].Rows
                            //                         select new Ent_BataClub_DashBoard_Mensual()
                            //                         {
                            //                             ANIO = dr["ANIO"].ToString(),
                            //                             MES = Convert.ToInt32(dr["MES"].ToString()),
                            //                             MES_STR = dr["MES_STR"].ToString(),
                            //                             NUMERO = Convert.ToDecimal(dr["MIEMBROS"])
                            //                         }).ToList();
                            //    info.listMesGenero = (from DataRow dr in ds.Tables[(informe == 0 ? 3 : 2)].Rows
                            //                          select new Ent_BataClub_Dashboard_Genero()
                            //                          {
                            //                              anio = dr["ANIO"].ToString(),
                            //                              genero = dr["GENERO"].ToString(),
                            //                              registros = Convert.ToDecimal(dr["REGISTROS"].ToString()),
                            //                          }).ToList();
                            //}
                                //if (new[] { 0, 3 }.Contains(informe))
                                //{ 
                                //    info.listCanles = (from DataRow dr in ds.Tables[informe == 0 ? 0 : 0].Rows
                                //                        select new Ent_BataClub_Dashboard_Canales()
                                //                        {
                                //                            CANAL = dr["CANAL"].ToString(),
                                //                            REGISTROS = Convert.ToDecimal(dr["REGISTROS"]),
                                //                            PORC = Convert.ToDecimal(dr["PORC"])
                                //                        }).ToList();
                                //}
                            DataTable dt_venta_bc = null;
                            if (new[] { 0, 4 }.Contains(informe))
                            { 
                                dt_venta_bc = ds.Tables[(informe == 0 ? 0 : 0)];
                                info.dtventa_bataclub = dt_venta_bc;
                                info.listMesParesSoles = (from dr in dt_venta_bc.AsEnumerable()// ds.Tables[(informe == 0 ? 5 : 0)].AsEnumerable()
                                                          group dr by
                                                          new
                                                          {
                                                              ANIO = dr["ANIO"].ToString(),
                                                              MES = Convert.ToInt32(dr["MES"].ToString()),
                                                              MES_STR = dr["MES_STR"].ToString(),
                                                              //NUMERO = Convert.ToDecimal(dr["PARES"]),
                                                              //NUMERO2 = Convert.ToDecimal(dr["SOLES"])
                                                          }
                                                          into G
                                                          select new Ent_BataClub_DashBoard_Mensual()
                                                          {
                                                              ANIO = G.Key.ANIO,
                                                              MES=G.Key.MES,
                                                              MES_STR=G.Key.MES_STR,
                                                              NUMERO=G.Sum(r=>Convert.ToInt32(r["PARES"])),
                                                              NUMERO2 =G.Sum(r => Convert.ToInt32(r["SOLES"])),
                                                          }).OrderBy(r=>r.MES).ToList();

                                //info.listMesParesSoles = (from DataRow dr in ds.Tables[(informe == 0 ? 5 : 0)].Rows
                                //                         select new Ent_BataClub_DashBoard_Mensual()
                                //                         {
                                //                             ANIO = dr["ANIO"].ToString(),
                                //                             MES = Convert.ToInt32(dr["MES"].ToString()),
                                //                             MES_STR = dr["MES_STR"].ToString(),
                                //                             NUMERO = Convert.ToDecimal(dr["PARES"]),
                                //                             NUMERO2 = Convert.ToDecimal(dr["SOLES"])
                                //                         }).ToList();
                            }
                            if (new[] { 0, 4, 5 }.Contains(informe))
                            { 

                                info.listPromsPS = (from dr in dt_venta_bc.AsEnumerable()
                                                    group dr by
                                                    new
                                                    {
                                                        promocion = dr["PROMOCION"].ToString()
                                                    }
                                                    into G
                                                    select new Ent_BataClub_DashBoard_Proms()
                                                    {
                                                        promocion = G.Key.promocion,
                                                        pares = G.Sum(r => Convert.ToInt32(r["PARES"])),
                                                        soles = G.Sum(r => Convert.ToInt32(r["SOLES"])),
                                                    }).OrderByDescending(r => r.soles).ToList();

                                //select new Ent_BataClub_DashBoard_Proms()
                                //{
                                //    promocion = dr["PROMOCION"].ToString(),
                                //    pares = Convert.ToInt32(dr["PARES"].ToString()),
                                //    soles = Convert.ToInt32(dr["SOLES"].ToString())
                                //}).ToList();

                                //info.listPromsPS = (from DataRow dr in ds.Tables[(informe == 0 ? 6 : informe == 4 ? 1 : 0)].Rows
                                //                              select new Ent_BataClub_DashBoard_Proms()
                                //                              {
                                //                                  promocion = dr["PROMOCION"].ToString(),
                                //                                  pares = Convert.ToInt32(dr["PARES"].ToString()),
                                //                                  soles = Convert.ToInt32(dr["SOLES"].ToString())
                                //                              }).ToList();
                            }
                            //if (new[] { 0, 6 }.Contains(informe))
                            //{
                            //    info.listSupervisorTot = (from DataRow dr in ds.Tables[(informe == 0 ? 1 :  0)].Rows
                            //                              select new Ent_BataClub_DashBoard_Supervisor()
                            //                              {
                            //                                  supervisor = dr["SUPERVISOR"].ToString(),
                            //                                  registros = Convert.ToInt32(dr["REGISTROS"].ToString()),
                            //                                  transac = Convert.ToInt32(dr["TRANSAC"].ToString()),
                            //                                  consumido = Convert.ToInt32(dr["CONSUMIDO"].ToString()),
                            //                                  bataclub = Convert.ToInt32(dr["MIEM_BATACLUB"].ToString()),
                            //                              }).ToList();
                            //    info.listTiendasSupervTot = (from DataRow dr in ds.Tables[(informe == 0 ? 2 : 1)].Rows
                            //                              select new Ent_BataClub_DashBoard_TiendasSupervisor()
                            //                              {
                            //                                  supervisor = dr["SUPERVISOR"].ToString(),
                            //                                  tienda = dr["TIENDA"].ToString(),
                            //                                  registros = Convert.ToInt32(dr["REGISTROS"].ToString()),
                            //                                  transac = Convert.ToInt32(dr["TRANSAC"].ToString()),
                            //                                  consumido = Convert.ToInt32(dr["CONSUMIDO"].ToString()),
                            //                                  bataclub = Convert.ToInt32(dr["MIEM_BATACLUB"].ToString()),
                            //                              }).ToList();
                            //}
                            if (new[] { 0,7 }.Contains(informe))
                            {
                                info.listComprasTot = (from DataRow dr in ds.Tables[(informe == 0 ? 1 :0)].Rows
                                                    group dr by
                                                    new
                                                    {
                                                        tipo = dr["TIPO"].ToString()
                                                    }
                                                    into G                                                    
                                                    select new Ent_BataClub_DashBoard_Compras()
                                                    {
                                                        tipo =G.Key.tipo,                                                      
                                                        monto =G.Sum(r=>Convert.ToDecimal(r["MONTO"])),
                                                        transac = G.Sum(r => Convert.ToDecimal(r["TRANSAC"])),

                                                    }).ToList();

                            info.listTipoComprasTot= (from DataRow dr in ds.Tables[(informe == 0 ? 1 :  0)].Rows
                                                      select new Ent_BataClub_DashBoard_Tipo_Compras()
                                                      {
                                                          transac = Convert.ToInt32(dr["TRANSAC"]),
                                                          prom = dr["PROM"].ToString(),
                                                          monto = Convert.ToInt32(dr["MONTO"]),
                                                          tipo = dr["TIPO"].ToString()
                                                      }).ToList();
                             }

                            if (new[] { 0, 12 }.Contains(informe))
                            {
                                info.listComprasCliTot = (from DataRow dr in ds.Tables[(informe == 0 ? 2 : 0)].Rows
                                                             select new Ent_BataClub_DashBoard_Compras_Cliente()
                                                             {                                                                 
                                                                 com_des = dr["COMP_DES"].ToString(),
                                                                 nclientes =Convert.ToInt32(dr["NCLIENTES"]),                                                                 
                                                             }).ToList();
                            }
                            if (new[] { 0, 13 }.Contains(informe))
                            {
                                info.listincompletos= (from DataRow dr in ds.Tables[(informe == 0 ? 3 : 0)].Rows
                                                          select new Ent_BataClub_Dashboard_Datos_Incompletos()
                                                          {
                                                              campo = dr["CAMPO"].ToString(),
                                                              porc = Convert.ToInt32(dr["PORC"]),
                                                          }).ToList();
                            }

                        }
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception ex)
            {
                info = null;
            }
            return info;

        }

        public List<Ent_BataClub_Dashboard_PSM> GetChartPSM(string fechaini = null, string fechafin = null)
        {
            string sqlquery = "USP_BATACLUB_DASHBOARD_PSM";
            List<Ent_BataClub_Dashboard_PSM> info = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (fechaini != null && fechafin != null)
                        {
                            cmd.Parameters.AddWithValue("@fecha_ini", Convert.ToDateTime(fechaini));
                            cmd.Parameters.AddWithValue("@fecha_fin", Convert.ToDateTime(fechafin));
                        }
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            info = (from DataRow dr in dt.Rows
                                                   select new Ent_BataClub_Dashboard_PSM()
                                                   {
                                                       pares = Convert.ToDecimal(dr["PARES"].ToString()),
                                                       soles = Convert.ToDecimal(dr["SOLES"].ToString()),
                                                       marca = Convert.ToString(dr["MARCA"])
                                                   }).ToList();
                        }
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception ex)
            {
                info = null;
            }
            return info;
        }
        public Ent_BataClub_DashBoard_General BATACLUB_DASHBOARD_GENERAL()
        {
            string sqlquery = "USP_BATACLUB_DASHBOARD_GENERAL";
            Ent_BataClub_DashBoard_General info = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;                       
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            info = (from DataRow dr in dt.Rows
                                    select new Ent_BataClub_DashBoard_General()
                                    {
                                        REGISTROS = Convert.ToDecimal(dr["REGISTROS"]),
                                        MIEMBROS = Convert.ToDecimal(dr["MIEMBROS"]),
                                        TRANSAC_CUPON = Convert.ToDecimal(dr["TRANSAC_CUPON"]),
                                        PARES = Convert.ToDecimal(dr["PARES"]),
                                        SOLES = Convert.ToDecimal(dr["SOLES"])
                                    }).FirstOrDefault();
                        }
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception ex)
            {
                info = null;
            }
            return info;
        }
        public Ent_BataClub_Dashboard_CxM BATACLUB_DASHBOARD_CLIENTES_MES(string anio)
        {
            string sqlquery = "USP_BATACLUB_DASHBOARD_CLIENTES_MES";
            Ent_BataClub_Dashboard_CxM info = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ANIO", anio);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            info = new Ent_BataClub_Dashboard_CxM();
                            info.meses = (from DataRow dr in ds.Tables[0].Rows
                                          select new Ent_BataClub_DashBoard_Mensual()
                                          {
                                              ANIO = dr["ANIO"].ToString(),
                                              MES = Convert.ToInt32(dr["MES"].ToString()),
                                              MES_STR = dr["MES_STR"].ToString(),
                                              NUMERO = Convert.ToDecimal(dr["REGISTROS"]),
                                              NUMERO2 = Convert.ToDecimal(dr["MIEMBROS"])
                                          }).ToList();

                            info.genero = (from DataRow dr in ds.Tables[1].Rows
                                                  select new Ent_BataClub_Dashboard_Genero()
                                                  {
                                                      anio = dr["ANIO"].ToString(),
                                                      genero = dr["GENERO"].ToString(),
                                                      registros = Convert.ToDecimal(dr["REGISTROS"].ToString()),
                                                  }).ToList();
                        }
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception ex)
            {
                info = null;
            }
            return info;
        }
        public Ent_BataClub_Dashboard_PPS BATACLUB_DASHBOARD_PPS(string fechaini = null, string fechafin = null,String canal_venta_ps= "TD,EC")
        {
            string sqlquery = "USP_BATACLUB_DASHBOARD_PORC_PARES_SOLES";
            Ent_BataClub_Dashboard_PPS info = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (fechaini != null && fechafin != null)
                        {
                            cmd.Parameters.AddWithValue("@fecha_ini", Convert.ToDateTime(fechaini));
                            cmd.Parameters.AddWithValue("@fecha_fin", Convert.ToDateTime(fechafin));
                            cmd.Parameters.AddWithValue("@canal_venta_ps", canal_venta_ps);
                        }
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            info = (from DataRow dr in dt.Rows
                                    select new Ent_BataClub_Dashboard_PPS()
                                    {
                                        PORC_PARES_BATACLUB = Convert.ToDecimal(dr["PORC_PARES_BATACLUB"]),
                                        PORC_SOLES_BATACLUB = Convert.ToDecimal(dr["PORC_SOLES_BATACLUB"]),
                                        PORC_SOLES_BATA = Convert.ToDecimal(dr["PORC_SOLES_BATA"]),
                                        PORC_PARES_BATA = Convert.ToDecimal(dr["PORC_PARES_BATA"]),
                                    }).FirstOrDefault();
                        }
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception ex)
            {
                info = null;
            }
            return info;
        }
        public List<Ent_BataClub_Dashboard_Canales> BATACLUB_DASHBOARD_CANALES_FECHA(string fechaini = null, string fechafin = null , bool excel = false)
        {
            string sqlquery = "USP_BATACLUB_DASHBOARD_CANALES_FECHA";
            List<Ent_BataClub_Dashboard_Canales> info = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (fechaini != null && fechafin != null)
                        {
                            cmd.Parameters.AddWithValue("@fecha_ini_canal", Convert.ToDateTime(fechaini));
                            cmd.Parameters.AddWithValue("@fecha_fin_canal", Convert.ToDateTime(fechafin));
                        }
                        cmd.Parameters.AddWithValue("@canal_excel", excel);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            info = new List<Ent_BataClub_Dashboard_Canales>();
                            info = (from DataRow dr in dt.Rows
                                    select new Ent_BataClub_Dashboard_Canales()
                                    {
                                        CANAL = Convert.ToString(dr["CANAL"]),
                                        REGISTROS = Convert.ToDecimal(dr["REGISTROS"]),
                                        PORC = Convert.ToDecimal(dr["PORC"]),
                                    }).ToList();
                        }
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception ex)
            {
                info = null;
            }
            return info;
        }
        public List<Ent_BataClub_Dashboard_PSPM> BATACLUB_DASHBOARD_PARES_SOLES_MES(string anio)
        {
            string sqlquery = "USP_BATACLUB_DASHBOARD_PARES_SOLES_MES";
            List<Ent_BataClub_Dashboard_PSPM> info = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ANIO", anio);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            info = new List<Ent_BataClub_Dashboard_PSPM>();
                            info = (from DataRow dr in dt.Rows
                                    select new Ent_BataClub_Dashboard_PSPM()
                                    {
                                        COD_ENTID = Convert.ToString(dr["COD_ENTID"]),
                                        TIENDA = Convert.ToString(dr["TIENDA"]),
                                        ANIO = Convert.ToString(dr["ANIO"]),
                                        MES = Convert.ToString(dr["MES"]),
                                        MES_STR = Convert.ToString(dr["MES_STR"]),
                                        PARES = Convert.ToDecimal(dr["PARES"]),
                                        SOLES = Convert.ToDecimal(dr["SOLES"]),
                                        PROMOCION = Convert.ToString(dr["PROMOCION"]),
                                    }).ToList();                           
                            
                        }
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception ex)
            {
                info = null;
            }
            return info;
        }
    }
}
