using CapaEntidad.Soporte;
using CapaEntidad.Util;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Soporte
{
    public class Dat_ConsultaGuia_Error
    {

        public List<Ent_Consulta_GuiaError> get_lista_guia_error(string estado, string Tk_soporte)
        {
            string sqlquery = "USP_XSTORE_SHIPPING_ERROR";
            List<Ent_Consulta_GuiaError> listar = null;
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
                            cmd.Parameters.AddWithValue("@ESTADO", estado);
                            cmd.Parameters.AddWithValue("@COD_TK_SOPORTE", Tk_soporte);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_Consulta_GuiaError>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_Consulta_GuiaError()
                                          {
                                              Codigo = Convert.ToInt32(dr["CODIGO"]),
                                              Caja = Convert.ToInt32(dr["CAJA"]),
                                              TK_xstore = Convert.ToInt32(dr["TK_XSTORE"]),
                                              Tienda = dr["TIENDA"].ToString(),
                                              Fec_Documento = dr["FECHA_DOC"].ToString(),
                                              Num_Documento = dr["NUM_DOC"].ToString(),
                                              Articulo = dr["ARTICULO"].ToString(),
                                              Calidad = dr["CALIDAD"].ToString(),
                                              Cantidad = Convert.ToInt32(dr["CANTIDAD"]),
                                              Fec_Ingreso = dr["FECHA_ING"].ToString(),
                                              Fec_Act = dr["FECHA_ACT"].ToString(),
                                              Mantis_Nro = dr["MANTIS_NRO"].ToString(),
                                              Estado = dr["ESTADO"].ToString(),
                                              TK_soporte_Bata = dr["TK_SOPORTE_BATA"].ToString(),


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

        public Int32 ActualizarGuiaError(int Id, string Tk_soporte, int Cod_mantis, string Estado)
        {
            string sqlquery = "USP_XSTORE_SHIPPING_MOD";
            int valida = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CODIGO", Id);
                        cmd.Parameters.AddWithValue("@MANTIS_NRO", Cod_mantis);
                        cmd.Parameters.AddWithValue("@ESTADO", Estado);
                        cmd.Parameters.AddWithValue("@TK_SOPORTE", Tk_soporte);
                        cmd.ExecuteNonQuery();
                        valida = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                valida = 0;
            }
            return valida;
        }


    }
}

