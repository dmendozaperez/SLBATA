using CapaEntidad.Transac;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Transac
{
    public class Dat_Documentos_Tda
    {
        public List<Ent_Documentos_Tda> get_lista(string tienda, string tipo_doc,string num_doc,string fecha_ini,string fecha_fin , string articulo)
        {
            string sqlquery = "[USP_GET_DOCUMENTOS_TIENDA]";
            List<Ent_Documentos_Tda> listar = null;
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
                            cmd.Parameters.AddWithValue("@TIP_DOC", tipo_doc);
                            cmd.Parameters.AddWithValue("@NUM_DOC", num_doc);  
                            cmd.Parameters.AddWithValue("@COD_TDA", tienda);
                            if (fecha_ini.Length>0 && fecha_fin.Length>0)
                            { 
                                cmd.Parameters.AddWithValue("@FEC_INI", Convert.ToDateTime(fecha_ini));
                                cmd.Parameters.AddWithValue("@FEC_FIN",Convert.ToDateTime(fecha_fin));
                            }
                            cmd.Parameters.AddWithValue("@COD_PRO", articulo);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_Documentos_Tda>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_Documentos_Tda()
                                          {
                                              tipo_doc = dr["TipoDoc"].ToString(),
                                              num_doc = dr["NumDoc"].ToString(),
                                              fecha_doc = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["FechaDoc"])),                                              
                                              sub_total = String.Format(new CultureInfo("es-PE"), "{0:C}", Convert.ToDecimal(dr["SubTotal"]) ),
                                              igv = String.Format(new CultureInfo("es-PE"), "{0:C}", Convert.ToDecimal(dr["Igv"])),                                              
                                              total = String.Format(new CultureInfo("es-PE"), "{0:C}", Convert.ToDecimal(dr["Total"])),
                                              FE = dr["FE"].ToString(),
                                              user_ws = dr["user_ws"].ToString(),
                                              pass_ws = dr["pass_ws"].ToString(),
                                              ruc_ws = dr["ruc_ws"].ToString(),
                                              tipodoc_ws = dr["tipodoc_ws"].ToString(),
                                              num_doc_ws = dr["num_doc_ws"].ToString(),
                                              tcantidad= dr["TCANT"].ToString(),
                                          }).ToList();
                            }

                        }
                    }
                    catch (Exception EXC)
                    {
                        listar = null;
                    }
                    if (cn!=null)
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
