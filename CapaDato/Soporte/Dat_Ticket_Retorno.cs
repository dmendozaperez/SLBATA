using CapaEntidad.Soporte;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Soporte
{
    public class Dat_Ticket_Retorno
    {
        public Ent_Ticket_Retorno PRUEBA_TK(Ent_Ticket_Retorno tr , bool consulta , ref string _mensaje)
        {
            Ent_Ticket_Retorno _cup = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    try
                    {
                        if (cn.State == 0) cn.Open();
                        using (SqlCommand cmd = new SqlCommand("USP_PRUEBA_TK", cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@cod_tda", tr.tiendaGen);
                            cmd.Parameters.AddWithValue("@cupon", tr.codigo);
                            cmd.Parameters.AddWithValue("@consulta", consulta);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                
                                if (dt.Rows.Count > 0)
                                {
                                    _cup = new Ent_Ticket_Retorno();
                                    _cup.codigo = dt.Rows[0]["CUP_RTN_BARRA"].ToString();
                                    _cup.impreso = Convert.ToBoolean(dt.Rows[0]["CUP_RTN_REIM_TK"]);
                                    _cup.estado = dt.Rows[0]["CUP_RTN_ESTADO"].ToString();
                                    _cup.tiendaGen = dt.Rows[0]["CUP_RTN_TDA_GEN"].ToString();
                                    _cup.fechaGen = dt.Rows[0]["CUP_RTN_FECHA_GEN"].ToString();
                                    _cup.montoGen = dt.Rows[0]["CUP_RTN_MONTO_GEN"].ToString();
                                    _cup.serieGen = dt.Rows[0]["CUP_RTN_SERIE_GEN"].ToString();
                                    _cup.numeroGen = dt.Rows[0]["CUP_RTN_NUMERO_GEN"].ToString();
                                    _cup.tiendaUso = dt.Rows[0]["CUP_RTN_TDA_USO"].ToString();
                                    _cup.serieUso = dt.Rows[0]["CUP_RTN_SERIE_USO"].ToString();
                                    _cup.numeroUso = dt.Rows[0]["CUP_RTN_NUMERO_USO"].ToString();
                                    _cup.fechaUso = dt.Rows[0]["CUP_RTN_FECHA_USO"].ToString();
                                    _cup.montoUso = dt.Rows[0]["CUP_RTN_TOTAL_USO"].ToString();
                                    _cup.montoDscto = dt.Rows[0]["MONTO_DSCTO"].ToString();
                                }
                            }
                            _mensaje = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        _mensaje = ex.Message;
                        _cup = null;
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.Message;
                _cup = null;
            }
            return _cup;
        }
    }
}
