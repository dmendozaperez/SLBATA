using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CapaPresentacion.Models.Crystal.Reporte;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad.Util;

namespace CapaPresentacion.Data.Crystal.Reporte
{
    public class Dat_InventarioPlanilla
    {

        public Models_InventarioPlanilla get_InventarioPlanilla(string cod_tda, string fecha)
        {
            Models_InventarioPlanilla lista = null;
            List<Lista_InventarioPlanilla> lista1 = null;

            var dt = new DataTable();
            var sqlquery = "USP_REPORTE_INVENTARIO_PLANILLA";

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
                        cmd.Parameters.AddWithValue("@FEC_EVAL", fecha);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            lista1 = new List<Lista_InventarioPlanilla>();
                            lista1 = (from DataRow dr in ds.Tables[0].Rows
                                      select new Lista_InventarioPlanilla()
                                      {
                                          COD_TIENDA = dr["COD_TIENDA"].ToString(),
                                          ARTICULO = dr["ARTICULO"].ToString(),
                                          CALIDAD = dr["CALIDAD"].ToString(),
                                          MEDIDA = dr["MEDIDA"].ToString(),
                                          STK_MED_LAT = dr["STK_MED_LAT"].ToString(),
                                          PPLANILLA = string.IsNullOrEmpty(dr["PPLANILLA"].ToString()) ? 0 : Convert.ToDecimal(dr["PPLANILLA"].ToString()),
                                          STOCK = string.IsNullOrEmpty(dr["STOCK"].ToString()) ? 0 : Convert.ToDecimal(dr["STOCK"].ToString()),
                                          VALOR = string.IsNullOrEmpty(dr["VALOR"].ToString()) ? 0 : Convert.ToDecimal(dr["VALOR"].ToString()),
                                          TALLA = dr["TALLA"].ToString(),
                                          FECHA = dr["FECHA"].ToString(),
                                          CATEG = dr["CATEG"].ToString(),
                                          SUBCATEG = dr["SUBCATEG"].ToString(),
                                          TIPO = dr["TIPO"].ToString(),
                                      }).ToList();
                            lista = new Models_InventarioPlanilla();

                            lista.ListInventarioPlanilla = lista1;
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

        public List<Ent_Combo> get_ListaTienda(string codTienda,int ind_, string pais)
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
                        cmd.Parameters.AddWithValue("@pais", pais);
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