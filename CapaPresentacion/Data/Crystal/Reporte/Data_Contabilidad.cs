using CapaEntidad.Util;
using Models.Crystal.Reporte;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Data.Crystal.Reporte
{
    public class Data_Contabilidad
    {
        public List<Models_VentaEfectivo> get_reporteVentaEfectivo(Models_VentaEfectivo _Ent)
        {
            string sqlquery = "USP_XSTORE_REPORTE_REDONDEO_VENTAS_EFECTIVO";
            List<Models_VentaEfectivo> Lista = null;
            try
            {
                if (_Ent.Cod_Tienda.Substring(0, 1).ToString() == "0") _Ent.Cod_Tienda = "0";
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@FEC_INI", DbType.DateTime).Value = _Ent.FechaInicio;
                            cmd.Parameters.AddWithValue("@FEC_FIN", DbType.DateTime).Value = _Ent.FechaFin;
                            cmd.Parameters.AddWithValue("@codtda", DbType.String).Value = _Ent.Cod_Tienda;
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataSet ds = new DataSet();
                                da.Fill(ds);
                                Lista = new List<Models_VentaEfectivo>();
                                Lista = (from DataRow dr in ds.Tables[0].Rows
                                         select new Models_VentaEfectivo()
                                         {
                                             Rango_Fecha = dr["Rango_Fecha"].ToString(),
                                             Distrito = dr["Distrito"].ToString(),
                                             Cadena = dr["Cadena"].ToString(),
                                             Cod_Tienda = dr["Cod_Tienda"].ToString(),
                                             Nombre_Tienda = dr["Nombre_Tienda"].ToString(),
                                             Tipo = dr["Tipo"].ToString(),
                                             Documento = dr["Documento"].ToString(),
                                             Fecha = Convert.ToDateTime(dr["Fecha"]),
                                             Total = Convert.ToDecimal(dr["Total"]),
                                             Redondeo = Convert.ToDecimal(dr["Redondeo"]),
                                         }).ToList();
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        Lista = new List<Models_VentaEfectivo>();
                    }
                }
            }
            catch (Exception)
            {
                Lista = new List<Models_VentaEfectivo>();
            }
            return Lista;
        }
    }
}