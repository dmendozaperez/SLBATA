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
        public Ent_BataClub_DashBoard GET_INFO_DASHBOARD(string anio = "2019")
        {
            string sqlquery = "USP_BATACLUB_DASHBOARD_2";
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
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            
                            info = new Ent_BataClub_DashBoard();
                            info.General = (from DataRow dr in ds.Tables[0].Rows
                                            select new Ent_BataClub_DashBoard_General()
                                            {
                                                REGISTROS = Convert.ToDecimal(dr["REGISTROS"]),
                                                MIEMBROS = Convert.ToDecimal(dr["MIEMBROS"]),
                                                RATIO = Convert.ToDecimal(dr["RATIO"]),
                                                TRANSAC_CUPON = Convert.ToDecimal(dr["TRANSAC_CUPON"])
                                            }).FirstOrDefault();
                            info.listMesRegistros = (from DataRow dr in ds.Tables[1].Rows
                                                    select new Ent_BataClub_DashBoard_Mensual()
                                                    {
                                                        ANIO = dr["ANIO"].ToString(),
                                                        MES  = Convert.ToInt32(dr["MES"].ToString()),
                                                        MES_STR = dr["MES_STR"].ToString(),
                                                        NUMERO = Convert.ToDecimal(dr["REGISTROS"])
                                                    }).ToList();
                            info.listMesMiembros = (from DataRow dr in ds.Tables[2].Rows
                                                     select new Ent_BataClub_DashBoard_Mensual()
                                                     {
                                                         ANIO = dr["ANIO"].ToString(),
                                                         MES = Convert.ToInt32(dr["MES"].ToString()),
                                                         MES_STR = dr["MES_STR"].ToString(),
                                                         NUMERO = Convert.ToDecimal(dr["MIEMBROS"])
                                                     }).ToList();
                            info.listCanles = (from DataRow dr in ds.Tables[3].Rows
                                                    select new Ent_BataClub_Dashboard_Canales()
                                                    {
                                                        CANAL = dr["CANAL"].ToString(),
                                                        REGISTROS = Convert.ToDecimal(dr["REGISTROS"]),
                                                        PORC = Convert.ToDecimal(dr["PORC"])
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
