using System;
using System.Collections.Generic;
using System.Linq;
using CapaPresentacion.Models.Crystal.Reporte;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad.Util; 

namespace CapaPresentacion.Data.Crystal.Reporte
{
    public class Dat_Inventario_Movimiento
    {
        public Models_InventarioMovimiento get_InventarioMovimiento(string cod_tda, DateTime fechaIni, DateTime FechaFin)
        {
            Models_InventarioMovimiento lista = null;
            List<Lista_InventarioMovimiento> lista1 = null;

            var dt = new DataTable();
            var sqlquery = "USP_XSTORE_REPORTE_INVENTARIO_MOVIMIENTOS";

            try
            {
                using (var cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    if (cn.State == 0)
                    {
                        cn.Open();
                    }
                    using (var cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CODTDA", cod_tda);
                        cmd.Parameters.AddWithValue("@FEC_INI", fechaIni);
                        cmd.Parameters.AddWithValue("@FEC_FIN", FechaFin);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            lista1 = new List<Lista_InventarioMovimiento>();
                            lista1 = (from DataRow dr in ds.Tables[0].Rows
                                      select new Lista_InventarioMovimiento()
                                      {
                                          CONCEPTO = dr["CONCEPTO"].ToString(),
                                          DESCRIPCION = dr["DESCRIPCION"].ToString(),
                                          TIENDA = dr["TIENDA"].ToString(),
                                          FECHA = dr["FECHA"].ToString(),
                                          HORA = dr["HORA"].ToString(),
                                          NUMDOC = dr["NUMDOC"].ToString(),
                                          ARTICULO = dr["ARTICULO"].ToString(),
                                          CALIDAD = dr["CALIDAD"].ToString(),
                                          TALLA = dr["TALLA"].ToString(),
                                          STK_MED_PER = dr["STK_MED_PER"].ToString(),
                                          STK_MED_LAT = dr["STK_MED_LAT"].ToString(),
                                          CALZADO = string.IsNullOrEmpty(dr["CALZADO"].ToString()) ? 0 : Convert.ToDecimal(dr["CALZADO"].ToString()),
                                          NOCALZADO = string.IsNullOrEmpty(dr["NOCALZADO"].ToString()) ? 0 : Convert.ToDecimal(dr["NOCALZADO"].ToString()),
                                          STOCK = string.IsNullOrEmpty(dr["STOCK"].ToString()) ? 0 : Convert.ToDecimal(dr["STOCK"].ToString()),
                                          VALOR = string.IsNullOrEmpty(dr["VALOR"].ToString()) ? 0 : Convert.ToDecimal(dr["VALOR"].ToString()),
                                          FEC_INI = dr["FEC_INI"].ToString(),
                                          FEC_FIN = dr["FEC_FIN"].ToString(),
                                          HORA_2 = dr["HORA_2"].ToString(),
                                          //update 29/07/2020

                                      }).ToList();
                            lista = new Models_InventarioMovimiento();

                            lista.ListInventarioMovimiento = lista1;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                lista = null;
            }
            return lista;
        }

        public List<Ent_Combo> get_ListaTienda(string codTienda, int ind_)
        {
            List<Ent_Combo> list = null;
            string sqlquery = "USP_LISTAR_TIENDA";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codTienda", codTienda);
                        cmd.Parameters.AddWithValue("@ind_", ind_);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Combo>();
                            Ent_Combo combo = new Ent_Combo();

                            while (dr.Read())
                            {
                                combo = new Ent_Combo();
                                combo.cbo_codigo = dr["cod_entid"].ToString();
                                combo.cbo_descripcion = dr["des_entid"].ToString();

                                list.Add(combo);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                list = null;
            }
            return list;
        }



    }
}