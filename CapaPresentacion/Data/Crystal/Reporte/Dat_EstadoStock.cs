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
    public class Dat_EstadoStock
    {
        public Models_EstadoStock listar(string cod_tda,DateTime fec_ini,DateTime fec_fin)
        {
            string sqlquery = "USP_XSTORE_REPORTE_ESTADO_STOCK";
            Models_EstadoStock listar = null;
            DataSet ds = null;
            try
            {
                listar = new Models_EstadoStock();
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cod_tda", cod_tda);
                        cmd.Parameters.AddWithValue("@fec_ini", fec_ini);
                        cmd.Parameters.AddWithValue("@fec_fin", fec_fin);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            ds = new DataSet();
                            da.Fill(ds);

                            listar.list_cab = (from DataRow fila in ds.Tables[0].Rows
                                               select new Estado_Stock_Cab()
                                               {
                                                   tienda = fila["tienda"].ToString(),
                                                   uni_calzado = Convert.ToDecimal(fila["uni_calzado"]),
                                                   uni_no_calzado = Convert.ToDecimal(fila["uni_no_calzado"]),
                                                   sol_calzado = Convert.ToDecimal(fila["sol_calzado"]),
                                                   sol_no_calzado = Convert.ToDecimal(fila["sol_no_calzado"]),
                                                   razon_social = fila["razon_social"].ToString(),
                                                   ruc = fila["ruc"].ToString(),
                                                   rango_fecha = fila["rango_fecha"].ToString(),
                                               }
                                             ).ToList();

                            listar.list_det = (from DataRow fila in ds.Tables[1].Rows
                                               select new Estado_Stock_Det()
                                               {
                                                   tienda = fila["tienda"].ToString(),
                                                   con_id = fila["con_id"].ToString(),
                                                   concepto = fila["concepto"].ToString(),
                                                   numdoc = fila["numdoc"].ToString(),
                                                   fecha = fila["fecha"].ToString(),//(fila["fecha"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(fila["fecha"]),
                                                   ing_calzado = Convert.ToDecimal(fila["ing_calzado"]),
                                                   ing_no_calzado = Convert.ToDecimal(fila["ing_no_calzado"]),
                                                   sal_calzado = Convert.ToDecimal(fila["sal_calzado"]),
                                                   sal_no_calzado = Convert.ToDecimal(fila["sal_no_calzado"]),
                                                   ing_sol_calzado = Convert.ToDecimal(fila["ing_sol_calzado"]),
                                                   ing_sol_no_calzado = Convert.ToDecimal(fila["ing_sol_no_calzado"]),
                                                   sal_sol_calzado = Convert.ToDecimal(fila["sal_sol_calzado"]),
                                                   sal_sol_no_calzado = Convert.ToDecimal(fila["sal_sol_no_calzado"]),
                                               }
                                             ).ToList();

                            listar.list_fin = (from DataRow fila in ds.Tables[2].Rows
                                               select new Estado_Stock_Fin()
                                               {
                                                   tienda = fila["tienda"].ToString(),
                                                   concepto = fila["concepto"].ToString(),
                                                   calzado = Convert.ToDecimal(fila["calzado"]),
                                                   no_calzado = Convert.ToDecimal(fila["no_calzado"]),
                                                   sol_calzado = Convert.ToDecimal(fila["sol_calzado"]),
                                                   sol_no_calzado = Convert.ToDecimal(fila["sol_no_calzado"]),
                                               }
                                            ).ToList();

                            listar.list_var= (from DataRow fila in ds.Tables[3].Rows
                                              select new Variacion_Precio()
                                              {
                                                  tienda = fila["tienda"].ToString(),
                                                  fecha = fila["fecha"].ToString(),
                                                  vp_ing_sol_calzado = Convert.ToDecimal(fila["vp_ing_sol_calzado"]),
                                                  vp_ing_sol_no_calzado = Convert.ToDecimal(fila["vp_ing_sol_no_calzado"]),
                                                  vp_sal_sol_calzado = Convert.ToDecimal(fila["vp_sal_sol_calzado"]),
                                                  vp_sal_sol_no_calzado = Convert.ToDecimal(fila["vp_sal_sol_no_calzado"]),
                                              }
                                            ).ToList();

                            listar.list_ins = (from DataRow fila in ds.Tables[4].Rows
                                               select new Insumos()
                                               {
                                                   tienda = fila["tienda"].ToString(),
                                                   tcan_in = Convert.ToDecimal(fila["tcan_in"]),
                                                   tsol_in = Convert.ToDecimal(fila["tsol_in"]),
                                               }
                                            ).ToList();

                            listar.list_saldos = (from DataRow fila in ds.Tables[5].Rows
                                               select new Estado_Stock_Saldos()
                                               {
                                                   tienda = fila["tienda"].ToString(),
                                                   SF_SOLES_CALZADO = Convert.ToDecimal(fila["SF_SOLES_CALZADO"]),
                                                   SF_SOLES_NO_CALZADO = Convert.ToDecimal(fila["SF_SOLES_NO_CALZADO"]),
                                               }
                                          ).ToList();
                        }
                    }
                }
            }
            catch( Exception exc)
            {
                
            }
            return listar;
        }
    }
}