using CapaEntidad.Soporte;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDato.Reporte;

namespace CapaDato.Soporte
{
    public class Dat_Ticket_Retorno
    {
        public bool REIMPRESION_TR(string cupon, string log_upd, ref string mensaje)
        {
            string sqlquery = "USP_REIMPRESION_TR";
            bool ret = false;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@log_upd", log_upd);
                        cmd.Parameters.AddWithValue("@cupon", cupon);
                        ret = cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                ret = false;
            }
            return ret;
        }

        public List<Ent_Ticket_Retorno> get_lista_cupones_retorno(string tienda, string cupon, string fechaIni, string fechaFin)
        {
            string sqlquery = "USP_GET_CUPONES_RETORNO";
            List<Ent_Ticket_Retorno> listar = null;
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
                            cmd.Parameters.AddWithValue("@cod_tda", tienda);
                            cmd.Parameters.AddWithValue("@cupon", cupon);
                            cmd.Parameters.AddWithValue("@fechaini", Convert.ToDateTime(fechaIni));
                            cmd.Parameters.AddWithValue("@fechafin", Convert.ToDateTime(fechaFin));
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_Ticket_Retorno>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_Ticket_Retorno()
                                          {
                                              codigo = dr["CUP_RTN_BARRA"].ToString(),
                                              impreso = !Convert.ToBoolean(dr["CUP_RTN_REIM_TK"]),
                                              estado = dr["CUP_RTN_ESTADO"].ToString(),
                                              tiendaGen = dr["CUP_RTN_TDA_GEN"].ToString(),
                                              fechaGen = Convert.ToDateTime(dr["CUP_RTN_FECHA_GEN"]).ToString("dd-MM-yyyy"),
                                              montoGen = Convert.ToDecimal(dr["CUP_RTN_MONTO_GEN"]),
                                              serieGen = dr["CUP_RTN_SERIE_GEN"].ToString(),
                                              numeroGen = dr["CUP_RTN_NUMERO_GEN"].ToString(),
                                              tiendaUso = dr["CUP_RTN_TDA_USO"].ToString(),
                                              serieUso = dr["CUP_RTN_SERIE_USO"].ToString(),
                                              numeroUso = dr["CUP_RTN_NUMERO_USO"].ToString(),
                                              fechaUso = (dr["CUP_RTN_FECHA_USO"].ToString() == "" ? "" : Convert.ToDateTime(dr["CUP_RTN_FECHA_USO"]).ToString("dd-MM-yyyy")),
                                              montoUso = dr["CUP_RTN_TOTAL_USO"].ToString() == "" ? 0 : Convert.ToDecimal(dr["CUP_RTN_TOTAL_USO"]),
                                              montoDscto = Convert.ToDecimal(dr["MONTO_DSCTO"]),
                                              nroReimp = Convert.ToInt32(dr["CUP_RTN_NRO_REIM"]),
                                          }).ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var mensaje = ex.Message;
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
