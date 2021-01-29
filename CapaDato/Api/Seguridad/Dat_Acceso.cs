using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Api.Seguridad
{
    public class Dat_Acceso
    {
        public Boolean ExisteTokenApi(string cod,string token)
        {
            Boolean valida = false;
            string sqlquery = "USP_VALIDA_ACCESO_WS";
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
                            cmd.Parameters.AddWithValue("@user", "");
                            cmd.Parameters.AddWithValue("@password", "");                            
                            cmd.Parameters.AddWithValue("@acceso_cod", cod);
                            cmd.Parameters.AddWithValue("@token", token);

                            cmd.Parameters.Add("@existe",SqlDbType.Bit);
                            cmd.Parameters["@existe"].Direction = ParameterDirection.Output;

                            cmd.ExecuteNonQuery();

                            valida = Convert.ToBoolean(cmd.Parameters["@existe"].Value);

                        }
                    }
                    catch 
                    {                        


                    }
                    if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch 
            {
                
            }
            return valida;
        }
    }
}
