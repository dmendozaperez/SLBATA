using CapaEntidad.GestionInterno;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.GestionInterno
{
    public class Dat_Comunicado
    {
        public List<Ent_Comunicado> lista_comunicado(string cod_tda)
        {
            string sqlquery = "USP_COMUNICADO_GET_TIENDA";
            List<Ent_Comunicado> listar = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    try
                    {
                        //if (cn.State == 0) cn.Open();
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@COD_TDA", cod_tda);                      
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_Comunicado>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_Comunicado()
                                          {
                                              tienda = dr["TIENDA"].ToString(),
                                              archivo = dr["ARCHIVO"].ToString(),
                                              descripcion = dr["DESCRIPCION"].ToString(),
                                              url = dr["URL"].ToString(),
                                              fecha_hora_crea = dr["FECHA_HORA_CREA"].ToString(),
                                              fecha_hora_mod = dr["FECHA_HORA_MOD"].ToString(),
                                              file_leido_fecha = dr["FILE_LEIDO_FECHA"].ToString(),
                                              file_leido =Convert.ToBoolean(dr["FILE_LEIDO"]),                                              
                                              id= Convert.ToDecimal(dr["ID"]),
                                              fecha=Convert.ToDateTime(dr["FECHA"]),
                                          }).ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        listar = null;
                    }
                    //if (cn != null)
                    //    if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception)
            {
                listar = null;
            }
            return listar;
        }

        public int leer_comunicado(string id_comunicado, decimal usu_id)
        {
            string sqlquery = "USP_COMUNICADO_UPD_LEIDO";
            int actualizo = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FILE_ID", id_comunicado);
                        cmd.Parameters.AddWithValue("@USU_ID", usu_id);
                        actualizo = cmd.ExecuteNonQuery();
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception ex)
            {
                actualizo = 0;
            }
            return actualizo;
        }
        public int NotificacionesComunicado(string tienda)
        {
            string sqlquery = "USP_COMUNICADO_GET_PEN_X_TIENDA";
            int nroNoti = 0;
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@COD_TDA", tienda);
                        cmd.Parameters.Add("@N_MENSAJE",SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        nroNoti = Convert.ToInt32(cmd.Parameters["@N_MENSAJE"].Value);
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception ex)
            {
                nroNoti = 0;
            }
            return nroNoti;
        }
    }
}
