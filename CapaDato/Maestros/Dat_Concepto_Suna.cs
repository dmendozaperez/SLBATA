using CapaEntidad.Maestros;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Maestros
{
    public class Dat_Concepto_Suna
    {
        public List<Ent_Concepto_Suna> lista_concepto_suna()
        {
            List<Ent_Concepto_Suna> listar = null;
            string sqlquery = "USP_GET_CONCEPTO_SUNA";
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
                            SqlDataReader dr = cmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                listar = new List<Ent_Concepto_Suna>();                                
                                while (dr.Read())
                                {
                                    Ent_Concepto_Suna con = new Ent_Concepto_Suna();
                                    con.con_sun_id = dr["con_sun_id"].ToString();
                                    con.con_sun_des = dr["con_sun_des"].ToString();
                                    listar.Add(con);
                                }
                            }
                        }
                    }
                    catch 
                    {
                        listar = null;
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch
            {
                listar = null;               
            }
            return listar;
        }
    }
}
