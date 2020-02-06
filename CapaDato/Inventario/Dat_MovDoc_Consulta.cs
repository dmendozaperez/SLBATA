using CapaEntidad.Inventario;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Inventario
{
    public class Dat_MovDoc_Consulta
    {
        public List<Ent_MovDoc_Consulta> lista_movdoc(string id,string cod_tda,DateTime fecha_ini,DateTime fecha_fin,string numdoc)
        {
            string sqlquery = "USP_XSTORE_CONSULTA_MOVIMIENTO";
            List<Ent_MovDoc_Consulta> listar = null;
            DataTable dt = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery,cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MD_ID", id);
                        cmd.Parameters.AddWithValue("@COD_TDA", cod_tda);
                        cmd.Parameters.AddWithValue("@FECHA_INI", fecha_ini);
                        cmd.Parameters.AddWithValue("@FECHA_FIN", fecha_fin);
                        cmd.Parameters.AddWithValue("@NUMDOC", numdoc);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dt = new DataTable();
                            da.Fill(dt);
                            listar = new List<Ent_MovDoc_Consulta>();
                            listar = (from DataRow fila in dt.Rows
                                      select new Ent_MovDoc_Consulta
                                      {
                                          mc_id = fila["mc_id"].ToString(),
                                          tienda= fila["tienda"].ToString(),
                                          tipo_transac = fila["tipo_transac"].ToString(),
                                          tipo_doc = fila["tipo_doc"].ToString(),
                                          num_doc = fila["num_doc"].ToString(),
                                          fecha_doc = fila["fecha_doc"].ToString(),
                                          cant =Convert.ToInt32(fila["cant"]),
                                          tda_des = fila["tda_des"].ToString(),
                                          tda_ori= fila["tda_ori"].ToString(),
                                      }
                                    ).ToList();
                        }

                    }
                }
            }
            catch (Exception)
            {

                
            }
            return listar;
        }
        public List<Ent_MovDoc_Consulta_Detalle> lista_movdoc_det(string id)
        {
            string sqlquery = "USP_XSTORE_CONSULTA_MOVIMIENTO";
            List<Ent_MovDoc_Consulta_Detalle> listar = null;
            DataTable dt = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MD_ID", id);                     

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dt = new DataTable();
                            da.Fill(dt);
                            listar = new List<Ent_MovDoc_Consulta_Detalle>();
                            listar = (from DataRow fila in dt.Rows
                                      select new Ent_MovDoc_Consulta_Detalle
                                      {
                                          articulo = fila["articulo"].ToString(),
                                          calidad = fila["calidad"].ToString(),
                                          linea = fila["linea"].ToString(),
                                          categoria = fila["categoria"].ToString(),
                                          talla = fila["talla"].ToString(),
                                          cantidad = Convert.ToInt32(fila["cantidad"]),
                                        
                                      }
                                    ).ToList();
                        }

                    }
                }
            }
            catch (Exception)
            {


            }
            return listar;
        }
    }
}
