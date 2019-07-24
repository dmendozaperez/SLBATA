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
    }
}
