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
        public Ent_BataClub_DashBoard GET_INFO_DASHBOARD(string anio = "2019" , int informe = 0, int mes = 0,object fechaIni = null , object fechaFin = null) // 0 = TODO | 1 = GENERAL | 2 = REGISTRADOS | 3 = MIEMBROS | 4 = CANALES
        {
            string sqlquery = "USP_BATACLUB_DASHBOARD_3";
            Ent_BataClub_DashBoard info = null;
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
                        cmd.Parameters.AddWithValue("@INFORME", informe); 
                        cmd.Parameters.AddWithValue("@MES", mes);
                        if (fechaIni != null && fechaFin != null)
                        {
                            cmd.Parameters.AddWithValue("@fecha_ini", Convert.ToDateTime(fechaIni));
                            cmd.Parameters.AddWithValue("@fecha_fin", Convert.ToDateTime(fechaFin));
                        }
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);                            
                            info = new Ent_BataClub_DashBoard();

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
                                info.listCanles = (from DataRow dr in ds.Tables[informe == 0 ? 4 : 0].Rows
                                                    select new Ent_BataClub_Dashboard_Canales()
                                                    {
                                                        CANAL = dr["CANAL"].ToString(),
                                                        REGISTROS = Convert.ToDecimal(dr["REGISTROS"]),
                                                        PORC = Convert.ToDecimal(dr["PORC"])
                                                    }).ToList();               
                            if (new[] { 0,4}.Contains(informe))
                                info.listMesParesSoles = (from DataRow dr in ds.Tables[(informe == 0 ? 5 : 0)].Rows
                                                         select new Ent_BataClub_DashBoard_Mensual()
                                                         {
                                                             ANIO = dr["ANIO"].ToString(),
                                                             MES = Convert.ToInt32(dr["MES"].ToString()),
                                                             MES_STR = dr["MES_STR"].ToString(),
                                                             NUMERO = Convert.ToDecimal(dr["PARES"]),
                                                             NUMERO2 = Convert.ToDecimal(dr["SOLES"])
                                                         }).ToList();
                            if (new[] { 0,4,5}.Contains(informe))
                                info.listPromsPS = (from DataRow dr in ds.Tables[(informe == 0 ? 6 : informe == 4 ? 1 : 0)].Rows
                                                          select new Ent_BataClub_DashBoard_Proms()
                                                          {
                                                              promocion = dr["PROMOCION"].ToString(),
                                                              pares = Convert.ToInt32(dr["PARES"].ToString()),
                                                              soles = Convert.ToInt32(dr["SOLES"].ToString())
                                                          }).ToList();
                            if (new[] { 0, 6 }.Contains(informe))
                            {
                                info.listSupervisorTot = (from DataRow dr in ds.Tables[(informe == 0 ? 7 :  0)].Rows
                                                          select new Ent_BataClub_DashBoard_Supervisor()
                                                          {
                                                              supervisor = dr["SUPERVISOR"].ToString(),
                                                              registros = Convert.ToInt32(dr["REGISTROS"].ToString()),
                                                              transac = Convert.ToInt32(dr["TRANSAC"].ToString())
                                                          }).ToList();
                                info.listTiendasSupervTot = (from DataRow dr in ds.Tables[(informe == 0 ? 8 : 1)].Rows
                                                          select new Ent_BataClub_DashBoard_TiendasSupervisor()
                                                          {
                                                              supervisor = dr["SUPERVISOR"].ToString(),
                                                              tienda = dr["TIENDA"].ToString(),
                                                              registros = Convert.ToInt32(dr["REGISTROS"].ToString()),
                                                              transac = Convert.ToInt32(dr["TRANSAC"].ToString())
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
    }
}
