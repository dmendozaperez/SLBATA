using CapaEntidad.Inventario;
using CapaEntidad.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Inventario
{
    public class Dat_Inventario_Consulta
    {
        // Combo de Tiendas (seleccionadas)
        public List<Ent_Inventario_Tienda> get_ListaTienda()
        {
            List<Ent_Inventario_Tienda> list = null;
            string sqlquery = "USP_XSTORE_INV_GET_TDA";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Inventario_Tienda>();
                            Ent_Inventario_Tienda combo = new Ent_Inventario_Tienda();
                            while (dr.Read())
                            {
                                combo = new Ent_Inventario_Tienda();
                                combo.cod_entid = dr["COD_ENTID"].ToString();
                                combo.des_entid = dr["DES_ENTID"].ToString();
                                list.Add(combo);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = null;
            }
            return list;
        }

        // Combo de Fechas (seleccionadas)
        public List<Ent_Inventario_Fecha> get_ListaFecha(string cod_entid)
        {
            List<Ent_Inventario_Fecha> list = null;
            string sqlquery = "USP_XSTORE_INV_GET_FEC";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@COD_TDA", cod_entid);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Inventario_Fecha>();
                            Ent_Inventario_Fecha combo = new Ent_Inventario_Fecha();
                            while (dr.Read())
                            {
                                combo = new Ent_Inventario_Fecha();
                               // combo.id = dr["XST_INV_FEC_AUD"].ToString();
                                combo.id = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dr["XST_INV_FEC_AUD"])); ;
                                combo.xst_inv_fec_aud = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["XST_INV_FEC_AUD"]));
                                list.Add(combo);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = null;
            }
            return list;
        }

        //Listado Tabla principal
        public List<Ent_Inventario_Consulta> get_ListaInv_Consulta(string cod_entid, string xst_inv_fec_aud, string articulo, string talla)
        {
            string sqlquery = "USP_XSTORE_INV_GET_CONSULTA";
            List<Ent_Inventario_Consulta> listar = null;
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
                            cmd.Parameters.AddWithValue("@COD_TDA", cod_entid);
                            cmd.Parameters.AddWithValue("@FEC_INV", xst_inv_fec_aud);
                            cmd.Parameters.AddWithValue("@COD_ART", articulo);
                            cmd.Parameters.AddWithValue("@TALLA", talla);
                            //cmd.Parameters.AddWithValue("@estado", dwest);
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_Inventario_Consulta>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_Inventario_Consulta()
                                          {
                                              des_entid = dr["DES_ENTID"].ToString(),
                                              articulo = dr["ARTICULO"].ToString(),
                                              calidad = dr["CALIDAD"].ToString(),
                                              talla = dr["TALLA"].ToString(),
                                              teorico = dr["TEORICO"].ToString(),
                                              fisico = dr["FISICO"].ToString(),
                                              diferencia = dr["DIFERENCIA"].ToString(),
                                              fecha_inv = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["FECHA_INV"]))
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

        public List<Ent_Consulta_Movimiento> get_Lista_Movimiento_Fecha(string fec, string dwtda)
        {
            string sqlquery = "USP_XSTORE_INV_MOV_XFECHA";
            DataTable dt = null;
            List<Ent_Consulta_Movimiento> listar = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {                                           
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@COD_TDA", dwtda);
                            cmd.Parameters.AddWithValue("@FECHA_INV", Convert.ToDateTime(fec));
                            //cmd.Parameters.AddWithValue("@estado", dwest);
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                dt= new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_Consulta_Movimiento>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_Consulta_Movimiento()
                                          {
                                              TIENDA = dr["TIENDA"].ToString(),
                                              FECHA = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["FECHA"])),
                                              INI_CALZADO = dr["INI_CALZADO"].ToString(),
                                              INI_NO_CALZADO = dr["INI_NO_CALZADO"].ToString(),
                                              VEN_CALZADO = dr["VEN_CALZADO"].ToString(),
                                              VEN_NO_CALZADO = dr["VEN_NO_CALZADO"].ToString(),
                                              ING_CALZADO = dr["ING_CALZADO"].ToString(),
                                              ING_NO_CALZADO = dr["ING_NO_CALZADO"].ToString(),
                                              SAL_CALZADO = dr["SAL_CALZADO"].ToString(),
                                              SAL_NO_CALZADO = dr["SAL_NO_CALZADO"].ToString(),
                                              SALDO_CALZADO = dr["SALDO_CALZADO"].ToString(),
                                              SALDO_NO_CALZADO = dr["SALDO_NO_CALZADO"].ToString(),
                                          }).ToList();
                            }
                    }       
                }
            }
            catch (Exception)
            {
                listar = null;
            }
            return listar ;
        }

        public List<Ent_Inv_Ajuste_Articulos> getListaTeorico(string tienda, DateTime fecha)
        {
            //[USP_XSTORE_INVENTARIO_CORTE]
            string sqlquery = "USP_XSTORE_INVENTARIO_CORTE";
            DataTable dt = null;
            List<Ent_Inv_Ajuste_Articulos> listar = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {

                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cod_tda", tienda);
                        cmd.Parameters.AddWithValue("@fec_inv", fecha);
                        //cmd.Parameters.AddWithValue("@estado", dwest);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dt = new DataTable();
                            da.Fill(dt);
                            listar = new List<Ent_Inv_Ajuste_Articulos>();
                            listar = (from DataRow dr in dt.Rows
                                      select new Ent_Inv_Ajuste_Articulos()
                                      {
                                          ARTICULO = dr["ARTICULO"].ToString(),
                                          CALIDAD = dr["CALIDAD"].ToString(),
                                          MEDIDA = dr["TALLA"].ToString(),
                                          STOCK = Convert.ToDecimal(dr["FISICO"].ToString()),
                                          TEORICO = Convert.ToDecimal(dr["TEORICO"].ToString()),
                                          DIFERENCIA = Convert.ToDecimal(dr["DIFERENCIA"].ToString()),
                                      }).ToList();
                        }
                    }
                }
            }
            catch (Exception)
            {
                listar = null;
            }
            return listar;
        }

        public string validarExcel(List<Ent_Inv_Ajuste_Articulos> listArtExcel)
        {
            string sqlquery = "USP_XSTORE_INVENTARIO_UPLOAD_VALIDA";
            DataTable dt = null;
            string mensaje = "";
            try
            {
                dt = new DataTable();
                dt = _toDTListInv(listArtExcel);
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TMP", dt);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return mensaje;
        }
        private DataTable _toDTListInv(List<Ent_Inv_Ajuste_Articulos> listArticulos)
        {
            DataTable dtRet = new DataTable();
            dtRet.Columns.Add("INV_COD_ART");
            dtRet.Columns.Add("INV_CAL");
            dtRet.Columns.Add("INV_MED_PER");
            dtRet.Columns.Add("INV_TEORICO");
            dtRet.Columns.Add("INV_FISICO");
            foreach (var item in listArticulos)
            {
                dtRet.Rows.Add(item.ARTICULO, item.CALIDAD , item.MEDIDA , item.TEORICO , item.STOCK );
            }
            return dtRet;
        }
        public int XSTORE_INSERTAR_INVENTARIO(string cod_tda , string inv_des , DateTime inv_fec_inv , decimal inv_usu , List<Ent_Inv_Ajuste_Articulos> lista , ref decimal tot_teorico , ref decimal tot_fisico , ref decimal stk_actual , ref string mensaje)
        {
            string sqlquery = "USP_XSTORE_INSERTAR_INVENTARIO";
            int f = 0;
            DataTable dt = null;
            try
            {
                dt = new DataTable();
                dt = _toDTListInv(lista);
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@COD_TDA", cod_tda);
                        cmd.Parameters.AddWithValue("@INV_DES", inv_des);
                        cmd.Parameters.AddWithValue("@INV_FEC_INV", inv_fec_inv);
                        cmd.Parameters.AddWithValue("@INV_USU", inv_usu);
                        cmd.Parameters.AddWithValue("@TMP", dt);
                        cmd.Parameters.Add("@TOT_TEORICO", SqlDbType.Decimal).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@TOT_FISICO", SqlDbType.Decimal).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@STK_ACTUAL", SqlDbType.Decimal).Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        tot_teorico = Convert.ToDecimal(cmd.Parameters["@TOT_TEORICO"].Value);
                        tot_fisico = Convert.ToDecimal(cmd.Parameters["@TOT_FISICO"].Value);
                        stk_actual = Convert.ToDecimal(cmd.Parameters["@STK_ACTUAL"].Value);
                        f = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                f = 0;
                mensaje = ex.Message;
            }
            return f;
        }

        public List<Ent_Inventario_Ajuste> getListaAjustesInv(string tienda)
        {
            string sqlquery = "USP_XSTORE_INVENTARIO_GET";
            DataTable dt = null;
            List<Ent_Inventario_Ajuste> listar = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {

                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@COD_TDA ", tienda);                        
                        //cmd.Parameters.AddWithValue("@estado", dwest);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dt = new DataTable();
                            da.Fill(dt);
                            listar = new List<Ent_Inventario_Ajuste>();
                            listar = (from DataRow dr in dt.Rows
                                      select new Ent_Inventario_Ajuste()
                                      {
                                          CODIGO = Convert.ToDecimal(dr["CODIGO"].ToString()),
                                          TIENDA = dr["TIENDA"].ToString(),
                                          DESCRIPCION = dr["DESCRIPCION"].ToString(),
                                          FECHA_INV =Convert.ToDateTime(dr["FECHA_INV"]).ToString("dd/MM/yyyy"),
                                          FISICO = Convert.ToDecimal(dr["FISICO"].ToString()),
                                          TEORICO = Convert.ToDecimal(dr["TEORICO"].ToString()),
                                          DIFERENCIA = Convert.ToDecimal(dr["DIFERENCIA"].ToString()),
                                      }).ToList();
                        }
                    }
                }
            }
            catch (Exception)
            {
                listar = null;
            }
            return listar;
        }

        public List<Ent_Inv_Ajuste_Articulos> get_list_arts_ajus_inv(string cod_ajus)
        {
            string sqlquery = "USP_XSTORE_INVENTARIO_GET";
            DataTable dt = null;
            List<Ent_Inv_Ajuste_Articulos> listar = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {

                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DET", 1);
                        cmd.Parameters.AddWithValue("@CODIGO", cod_ajus);
                        //cmd.Parameters.AddWithValue("@estado", dwest);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dt = new DataTable();
                            da.Fill(dt);
                            listar = new List<Ent_Inv_Ajuste_Articulos>();
                            listar = (from DataRow dr in dt.Rows
                                      select new Ent_Inv_Ajuste_Articulos()
                                      {
                                          ARTICULO = dr["ARTICULO"].ToString(),
                                          CALIDAD = dr["CALIDAD"].ToString(),
                                          MEDIDA = dr["MEDIDA"].ToString(),
                                          STOCK = Convert.ToDecimal(dr["FISICO"].ToString()),
                                          TEORICO = Convert.ToDecimal(dr["TEORICO"].ToString()),
                                          DIFERENCIA = Convert.ToDecimal(dr["DIFERENCIA"].ToString()),
                                      }).ToList();
                        }
                    }
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
