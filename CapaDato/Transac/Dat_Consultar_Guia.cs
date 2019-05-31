using CapaEntidad.Transac;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Transac
{
    public class Dat_Consultar_Guia
    {
        public List<Ent_Consultar_Guia> get_lista(string p_tda_destino, string p_num_guia)
        {
            string sqlquery = "USP_CONSULTAR_GUIAS_TIENDA";
            List<Ent_Consultar_Guia> listar = null;
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

                            cmd.Parameters.AddWithValue("@tda_destino", p_tda_destino);
                            cmd.Parameters.AddWithValue("@num_guia", p_num_guia);
                            
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_Consultar_Guia>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_Consultar_Guia()
                                          {
                                              desc_almac        = dr["desc_almac"].ToString(),
                                              num_gudis         = dr["num_gudis"].ToString(),
                                              num_guia          = dr["num_guia"].ToString(),
                                              tienda_origen     = dr["tienda_origen"].ToString(),
                                              tienda_destino    = dr["tienda_destino"].ToString(),
                                              fecha_des         = dr["fecha_des"].ToString(),
                                              desc_send_tda     = dr["desc_send_tda"].ToString(),
                                              fec_env           = dr["fec_env"].ToString(),
                                             
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
        
        public int send_guide(string desc_almac, string num_gudis, decimal usuario)
        {
            Int32 intRespuesta = 0;
            string sqlquery = "USP_UPDATE_GUIAS_ENVIO";         
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
                            cmd.Parameters.AddWithValue("@desc_almac", desc_almac);
                            cmd.Parameters.AddWithValue("@desc_gudis", num_gudis);
                            cmd.Parameters.AddWithValue("@usu_log", usuario);
                            cmd.ExecuteNonQuery();
                            intRespuesta = 1;
                        }
                    }
                    catch
                    {

                        intRespuesta = -1;
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }               

            }
            catch (Exception ex)
            {

                intRespuesta = -1;

            }

            return intRespuesta;
        }
    }
}
