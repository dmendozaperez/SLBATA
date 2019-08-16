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
    public class Dat_Canal
    {
        public List<Ent_Canal> get_lista()
        {
            List<Ent_Canal> list = null;
            string sqlquery = "USP_Leer_Canal";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            list = new List<Ent_Canal>();
                            Ent_Canal canal = new Ent_Canal();
                            canal.can_id = "0";
                            canal.can_des = "--Ninguno--";
                            list.Add(canal);

                            while (dr.Read())
                            {
                                canal = new Ent_Canal();
                                canal.can_id = dr["CAN_ID"].ToString();
                                canal.can_des = dr["CAN_DES"].ToString();
                                list.Add(canal);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = null;
            }
            return list;
        }

    }
}
