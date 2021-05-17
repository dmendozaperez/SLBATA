using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad.Util;
using CapaEntidad.Reports;

namespace CapaDato.Reporte
{
    public class Dat_XstoreReporte
    {
        public List<Ent_Xstore_Vendedor> ListarIncentivoVendedor(Ent_Xstore_Vendedor _Ent)
        {
            List<Ent_Xstore_Vendedor> Listar = new List<Ent_Xstore_Vendedor>();
            string sqlquery = "[USP_XSTORE_REPORTE_INCENTIVO_ESCOLAR_VENDEDORES]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FEC_INI", DbType.DateTime).Value = _Ent.FechaInicio;
                        cmd.Parameters.AddWithValue("@FEC_FIN", DbType.DateTime).Value = _Ent.FechaFin;
                        cmd.Parameters.AddWithValue("@codtda", DbType.String).Value = _Ent.Cod_Tda;
                        cmd.Parameters.AddWithValue("@tip_rep", DbType.String).Value = _Ent.Tip_Rep;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            Listar = new List<Ent_Xstore_Vendedor>();
                            if (_Ent.Tip_Rep=="1")
                            {
                                Listar = (from DataRow fila in dt.Rows
                                          select new Ent_Xstore_Vendedor()
                                          {
                                              Semana_Str = (fila["Semana_Str"] is DBNull) ? string.Empty : (string)(fila["Semana_Str"]),
                                              Distrito = (fila["Distrito"] is DBNull) ? string.Empty : (string)(fila["Distrito"]),
                                              Des_Cadena = (fila["Des_Cadena"] is DBNull) ? string.Empty : (string)(fila["Des_Cadena"]),
                                              Cod_Entid = (fila["Cod_Entid"] is DBNull) ? string.Empty : (string)(fila["Cod_Entid"]),
                                              Des_Entid = (fila["Des_Entid"] is DBNull) ? string.Empty : (string)(fila["Des_Entid"]),
                                              Dni = (fila["Dni"] is DBNull) ? string.Empty : (string)(fila["Dni"]),
                                              Dni_Nombre = (fila["Dni_Nombre"] is DBNull) ? string.Empty : (string)(fila["Dni_Nombre"]),
                                              Fecha = (fila["Fecha"] is DBNull) ? (DateTime?)null : Convert.ToDateTime(fila["Fecha"]),
                                              Documento = (fila["Documento"] is DBNull) ? string.Empty : (string)(fila["Documento"]),
                                              Articulo = (fila["Articulo"] is DBNull) ? string.Empty : (string)(fila["Articulo"]),
                                              Cod_Categ = (fila["Cod_Categ"] is DBNull) ? string.Empty : (string)(fila["Cod_Categ"]),
                                              Cod_Subca = (fila["Cod_Subca"] is DBNull) ? string.Empty : (string)(fila["Cod_Subca"]),
                                              Cantidad = (fila["Cantidad"] is DBNull) ?  (int?)null : Convert.ToInt32(fila["Cantidad"]),
                                              Total = (fila["Total"] is DBNull) ? (int?)null : Convert.ToInt32(fila["Total"]),
                                              Tot_Transacc = (fila["Tot_Transacc"] is DBNull) ? (int?)null : Convert.ToInt32(fila["Tot_Transacc"]),
                                              Esc_Negro_2x50 = (fila["Esc_Negro_2x50"] is DBNull) ? (int?)null : Convert.ToInt32(fila["Esc_Negro_2x50"]),
                                              Incentivo = (fila["Incentivo"] is DBNull) ? (Decimal?)null : Convert.ToDecimal(fila["Incentivo"])
                                          }).ToList();
                            }
                            else
                            {
                                Listar = (from DataRow fila in dt.Rows
                                          select new Ent_Xstore_Vendedor()
                                          {
                                              Semana_Str = (fila["Semana_Str"] is DBNull) ? string.Empty : (string)(fila["Semana_Str"]),
                                              Distrito = (fila["Distrito"] is DBNull) ? string.Empty : (string)(fila["Distrito"]),
                                              Des_Cadena = (fila["Des_Cadena"] is DBNull) ? string.Empty : (string)(fila["Des_Cadena"]),
                                              Cod_Entid = (fila["Cod_Entid"] is DBNull) ? string.Empty : (string)(fila["Cod_Entid"]),
                                              Des_Entid = (fila["Des_Entid"] is DBNull) ? string.Empty : (string)(fila["Des_Entid"]),
                                              Dni = (fila["Dni"] is DBNull) ? string.Empty : (string)(fila["Dni"]),
                                              Dni_Nombre = (fila["Dni_Nombre"] is DBNull) ? string.Empty : (string)(fila["Dni_Nombre"]),
                                              Pares_Esc_Negro = (fila["Pares_Esc_Negro"] is DBNull) ? (int?)null : Convert.ToInt32(fila["Pares_Esc_Negro"]),
                                              Incentivo1 = (fila["Incentivo1"] is DBNull) ? (int?)null : Convert.ToInt32(fila["Incentivo1"]),
                                              Incentivo2 = (fila["Incentivo2"] is DBNull) ? (int?)null : Convert.ToInt32(fila["Incentivo2"]),
                                              Total_Incentivo = (fila["Total_Incentivo"] is DBNull) ? (Decimal?)null : Convert.ToDecimal(fila["Total_Incentivo"])
                                          }
                                        ).ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Listar;
        }
    }
}
