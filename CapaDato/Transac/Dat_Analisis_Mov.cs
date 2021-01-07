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
    public class Dat_Analisis_Mov
    {
        public List<Ent_Analisis_Mov> get_lista(string cod_tda,DateTime fec_ini,DateTime fec_fin,
                                                string cod_art,string calidad,string talla)
        {
            string sqlquery = "USP_GET_CONSULTA_ANALISIS_MOV";
            List<Ent_Analisis_Mov> listar = null;
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
                            cmd.Parameters.AddWithValue("@COD_TDA", cod_tda);
                            cmd.Parameters.AddWithValue("@FEC_INI", fec_ini);
                            cmd.Parameters.AddWithValue("@FEC_FIN", fec_fin);
                            
                            cmd.Parameters.AddWithValue("@COD_ART", cod_art);
                            cmd.Parameters.AddWithValue("@CALIDAD", calidad);
                            cmd.Parameters.AddWithValue("@TALLA", talla);
                            
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_Analisis_Mov>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_Analisis_Mov()
                                          {
                                              item =Convert.ToInt32(dr["item"].ToString()),
                                              fecha = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["fecha"])),
                                              tipo_doc = dr["tipo_doc"].ToString(),
                                              num_doc = dr["num_doc"].ToString(),
                                              origen = dr["origen"].ToString(),
                                              destino = dr["destino"].ToString(),
                                              inicial =Convert.ToInt32(dr["inicial"]),
                                              ingreso =Convert.ToInt32(dr["ingreso"]),
                                              salida =Convert.ToInt32(dr["salida"]),
                                              saldo =Convert.ToInt32(dr["saldo"]),                                              
                                          }).ToList();
                            }

                        }
                    }
                    catch (Exception exc)
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
    }
}
