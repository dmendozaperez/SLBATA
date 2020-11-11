using CapaEntidad.Util;
using CapaPresentacion.Models.Crystal.Reporte;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Data.Crystal.Reporte
{
    public class Dat_Best_Seller
    {
        public List<Models_Best_Seller> get_best_seller(string cod_distri,string cadena,DateTime FEC_INI,DateTime FEC_FIN,string codtda,
	                                                    int top,string TIPO_CAT,string COD_LINEA,string COD_CATEG)
        {
            List<Models_Best_Seller> listar = null;
            string sqlquery = "[USP_XSTORE_REPORTE_BEST_SELLER]";
            DataTable dt = null;
            try
            {
                listar = new List<Models_Best_Seller>();
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cod_distri", cod_distri);
                        cmd.Parameters.AddWithValue("@cadena", cadena);
                        cmd.Parameters.AddWithValue("@FEC_INI", FEC_INI);
                        cmd.Parameters.AddWithValue("@FEC_FIN", FEC_FIN);
                        cmd.Parameters.AddWithValue("@codtda", codtda);
                        cmd.Parameters.AddWithValue("@top", top);
                        cmd.Parameters.AddWithValue("@TIPO_CAT", TIPO_CAT);
                        cmd.Parameters.AddWithValue("@COD_LINEA", COD_LINEA);
                        cmd.Parameters.AddWithValue("@COD_CATEG", COD_CATEG);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dt = new DataTable();
                            da.Fill(dt);
                            listar = (from DataRow fila in dt.Rows
                                      select new Models_Best_Seller()
                                      {
                                          cod_cadena=fila["cod_cadena"].ToString(),
                                          cod_entid = fila["cod_entid"].ToString(),
                                          des_entid = fila["des_entid"].ToString(),
                                          cod_distri = fila["cod_distri"].ToString(),
                                          cod_linea = fila["cod_linea"].ToString(),
                                          cod_categ = fila["cod_categ"].ToString(),
                                          cod_subcat = fila["cod_subcat"].ToString(),
                                          artic = fila["artic"].ToString(),
                                          can_total =Convert.ToDecimal(fila["cant_total"]),
                                          neto = Convert.ToDecimal(fila["neto"]),   
                                          fecha_rango= fila["fec_rango"].ToString(),
                                          id= Convert.ToInt32(fila["id"]),

                                          pplanilla= Convert.ToDecimal(fila["pplanilla"]),
                                          ppventa = Convert.ToDecimal(fila["pventa"]),
                                          stockactual = Convert.ToDecimal(fila["stkactual"]),

                                      }).ToList();
                        }
                                             
                    }
                }

            }
            catch (Exception exc)
            {
                
            }
            return listar;
        }
    }
}