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
    public class Dat_RRHH
    {
        public List<Models_CuponRedimidos> get_reporteCuponRedimidos(Models_CuponRedimidos _Ent)
        {
            string sqlquery = "USP_XSTORE_REPORTE_CUPONES_REDIMIDOS";
            List<Models_CuponRedimidos> Lista = null;
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
                            cmd.Parameters.AddWithValue("@scancode", DbType.String).Value = _Ent.Scan_Code;
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataSet ds = new DataSet();
                                da.Fill(ds);
                                Lista = new List<Models_CuponRedimidos>();
                                Lista = (from DataRow dr in ds.Tables[0].Rows
                                         select new Models_CuponRedimidos()
                                         {
                                             Rango_Fecha = dr["Rango_Fecha"].ToString(),
                                             Distrito = dr["Distrito"].ToString(),
                                             Cadena = dr["Cadena"].ToString(),
                                             Cod_Tienda = dr["Cod_Tienda"].ToString(),
                                             Nombre_Tienda = dr["Nombre_Tienda"].ToString(),
                                             Documento = dr["Documento"].ToString(),
                                             Fecha = Convert.ToDateTime(dr["Fecha"]),
                                             Dni = dr["Dni"].ToString(),
                                             Cliente = dr["Cliente"].ToString(),
                                             Porcentaje = Convert.ToDecimal(dr["Porcentaje"]),
                                             Monto = Convert.ToDecimal(dr["Monto"]),
                                             Cupon = dr["Cupon"].ToString(),
                                             Campanna = dr["Campaña"].ToString(),
                                             Descripcion = dr["Descripcion"].ToString(),

                                         }).ToList();
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        Lista = new List<Models_CuponRedimidos>();
                    }
                }
            }
            catch (Exception)
            {
                Lista = new List<Models_CuponRedimidos>();
            }
            return Lista;
        }

        public List<Models_Prefijos> get_Prefijos()
        {
            string sqlquery = "USP_ORCE_GET_PREFIX";
            List<Models_Prefijos> Lista = null;
            try
            {             
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataSet ds = new DataSet();
                                da.Fill(ds);
                                Lista = new List<Models_Prefijos>();
                                Lista = (from DataRow dr in ds.Tables[0].Rows
                                         select new Models_Prefijos()
                                         {
                                             Codigo = dr["CODIGO"].ToString(),
                                             Descripcion = dr["NOMBRE"].ToString()
                                         }).ToList();
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        Lista = new List<Models_Prefijos>();
                    }
                }
            }
            catch (Exception)
            {
                Lista = new List<Models_Prefijos>();
            }
            return Lista;
        }
    }
}