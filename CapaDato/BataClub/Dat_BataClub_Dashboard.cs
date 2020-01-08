﻿using CapaEntidad.BataClub;
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
        public List<Ent_Bataclub_Canales_Excel> get_canales_excel(Int32 informe,DateTime fecini_canal,DateTime fecfin_canal)
        {
            List<Ent_Bataclub_Canales_Excel> list=null;
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
            object fechaIni_canal = null, object fechaFin_canal = null, object fechaIni_com = null, object fechaFin_com = null, object fechaIni_com_cl = null, object fechaFin_com_cl = null,String opcion_data_in="FN") // 0 = TODO | 1 = GENERAL | 2 = REGISTRADOS | 3 = MIEMBROS | 4 = CANALES
        {
            string sqlquery = "USP_BATACLUB_DASHBOARD";
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

                        cmd.Parameters.AddWithValue("@prom", prom);//@prom
                        cmd.Parameters.AddWithValue("@opcion_data_in", opcion_data_in);//@prom
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);                            
                            //info = new Ent_BataClub_DashBoard();

                            if (new[] { 0 ,1 }.Contains(informe))
                                info.General = (from DataRow dr in ds.Tables[(0)].Rows
                                                select new Ent_BataClub_DashBoard_General()
                                                {
                                                    REGISTROS = Convert.ToDecimal(dr["REGISTROS"]),
                                                    MIEMBROS = Convert.ToDecimal(dr["MIEMBROS"]),                                                    
                                                    TRANSAC_CUPON = Convert.ToDecimal(dr["TRANSAC_CUPON"]),
                                                    PARES = Convert.ToDecimal(dr["PARES"]),
                                                    SOLES = Convert.ToDecimal(dr["SOLES"])
                                                }).FirstOrDefault();
                            if (new[] { 0, 2 }.Contains(informe))
                            {                                                           
                                info.listMesRegistros = (from DataRow dr in ds.Tables[(informe == 0 ? 1 : 0)].Rows
                                                    select new Ent_BataClub_DashBoard_Mensual()
                                                    {
                                                        ANIO = dr["ANIO"].ToString(),
                                                        MES  = Convert.ToInt32(dr["MES"].ToString()),
                                                        MES_STR = dr["MES_STR"].ToString(),
                                                        NUMERO = Convert.ToDecimal(dr["REGISTROS"])
                                                    }).ToList();
                                info.listMesMiembros = (from DataRow dr in ds.Tables[(informe == 0 ? 2 : 1) ].Rows
                                                     select new Ent_BataClub_DashBoard_Mensual()
                                                     {
                                                         ANIO = dr["ANIO"].ToString(),
                                                         MES = Convert.ToInt32(dr["MES"].ToString()),
                                                         MES_STR = dr["MES_STR"].ToString(),
                                                         NUMERO = Convert.ToDecimal(dr["MIEMBROS"])
                                                     }).ToList();
                                info.listMesGenero = (from DataRow dr in ds.Tables[(informe == 0 ? 3 : 2)].Rows
                                                      select new Ent_BataClub_Dashboard_Genero()
                                                      {
                                                          anio = dr["ANIO"].ToString(),
                                                          genero = dr["GENERO"].ToString(),
                                                          registros = Convert.ToDecimal(dr["REGISTROS"].ToString()),
                                                      }).ToList();
                            }
                            if (new[] { 0, 3 }.Contains(informe))
                            { 
                                info.listCanles = (from DataRow dr in ds.Tables[informe == 0 ? 4 : 0].Rows
                                                    select new Ent_BataClub_Dashboard_Canales()
                                                    {
                                                        CANAL = dr["CANAL"].ToString(),
                                                        REGISTROS = Convert.ToDecimal(dr["REGISTROS"]),
                                                        PORC = Convert.ToDecimal(dr["PORC"])
                                                    }).ToList();
                            }
                            DataTable dt_venta_bc = null;
                            if (new[] { 0, 4 }.Contains(informe))
                            { 
                                dt_venta_bc = ds.Tables[(informe == 0 ? 5 : 0)];
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
                            if (new[] { 0, 6 }.Contains(informe))
                            {
                                info.listSupervisorTot = (from DataRow dr in ds.Tables[(informe == 0 ? 6 :  0)].Rows
                                                          select new Ent_BataClub_DashBoard_Supervisor()
                                                          {
                                                              supervisor = dr["SUPERVISOR"].ToString(),
                                                              registros = Convert.ToInt32(dr["REGISTROS"].ToString()),
                                                              transac = Convert.ToInt32(dr["TRANSAC"].ToString()),
                                                              consumido = Convert.ToInt32(dr["CONSUMIDO"].ToString()),
                                                              bataclub = Convert.ToInt32(dr["MIEM_BATACLUB"].ToString()),
                                                          }).ToList();
                                info.listTiendasSupervTot = (from DataRow dr in ds.Tables[(informe == 0 ? 7 : 1)].Rows
                                                          select new Ent_BataClub_DashBoard_TiendasSupervisor()
                                                          {
                                                              supervisor = dr["SUPERVISOR"].ToString(),
                                                              tienda = dr["TIENDA"].ToString(),
                                                              registros = Convert.ToInt32(dr["REGISTROS"].ToString()),
                                                              transac = Convert.ToInt32(dr["TRANSAC"].ToString()),
                                                              consumido = Convert.ToInt32(dr["CONSUMIDO"].ToString()),
                                                              bataclub = Convert.ToInt32(dr["MIEM_BATACLUB"].ToString()),
                                                          }).ToList();
                            }
                            if (new[] { 0,7 }.Contains(informe))
                            {
                                info.listComprasTot = (from DataRow dr in ds.Tables[(informe == 0 ? 8 :0)].Rows
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

                            info.listTipoComprasTot= (from DataRow dr in ds.Tables[(informe == 0 ? 8 :  0)].Rows
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
                                info.listComprasCliTot = (from DataRow dr in ds.Tables[(informe == 0 ? 9 : 0)].Rows
                                                             select new Ent_BataClub_DashBoard_Compras_Cliente()
                                                             {                                                                 
                                                                 com_des = dr["COMP_DES"].ToString(),
                                                                 nclientes =Convert.ToInt32(dr["NCLIENTES"]),                                                                 
                                                             }).ToList();
                            }
                            if (new[] { 0, 13 }.Contains(informe))
                            {
                                info.listincompletos= (from DataRow dr in ds.Tables[(informe == 0 ? 10 : 0)].Rows
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

    }
}
