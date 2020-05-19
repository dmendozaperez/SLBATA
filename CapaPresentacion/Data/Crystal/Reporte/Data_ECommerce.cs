using BarcodeLib.Barcode;
using BarcodeLib.Barcode.CrystalReports;
using CapaEntidad.Util;
using CapaPresentacion.Models.ECommerce;
using Models.Crystal.Reporte;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace Data.Crystal.Reporte
{
    public class Data_Ecommerce
    {
        public List<ECommerce> getGuia_EC(string venta_id)
        {
            string sqlquery = "USP_REP_GUIA_ECOMMERCE";
            List<ECommerce> result = null;
            ECommerce guia = null;
            List<DetallesECommerce> lista = null;
            DataTable dt = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ven_id", venta_id);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                dt = new DataTable();
                                da.Fill(dt);
                                lista = new List<DetallesECommerce>();
                                lista = (from DataRow dr in dt.Rows
                                         select new DetallesECommerce()
                                         {
                                             codigoProducto = dr["codigoProducto"].ToString(),
                                             nombreProducto = dr["nombreProducto"].ToString(),
                                             cantidad = Convert.ToInt32(dr["cantidad"]),
                                             precioUnitario = Convert.ToDecimal(dr["precioUnitario"]),
                                             descuento = Convert.ToDecimal(dr["descuento"]),
                                             total = Convert.ToDecimal(dr["total"]),
                                             talla = dr["talla"].ToString(),
                                         }).ToList();

                                guia = new ECommerce();
                                guia.idPedido = dt.Rows[0]["idPedido"].ToString();
                                guia.Referencia = dt.Rows[0]["Referencia"].ToString();
                                guia.fechaPedido = Convert.ToDateTime(dt.Rows[0]["fechaPedido"]);
                                guia.tipoComprobante = dt.Rows[0]["tipoComprobante"].ToString();
                                guia.SerieDoc = dt.Rows[0]["SerieDoc"].ToString();
                                guia.GuiaNro = dt.Rows[0]["GuiaNro"].ToString();
                                guia.NroDoc = dt.Rows[0]["NroDoc"].ToString();
                                guia.codSeguimiento = dt.Rows[0]["codSeguimiento"].ToString();
                                guia.nom_courier = dt.Rows[0]["nom_courier"].ToString();
                                guia.estado = dt.Rows[0]["estado"].ToString();
                                guia.cliente = dt.Rows[0]["cliente"].ToString();
                                guia.direccionA = dt.Rows[0]["direccionA"].ToString();
                                guia.direccionB = dt.Rows[0]["direccionB"].ToString();
                                guia.direccionCliente = dt.Rows[0]["direccionCliente"].ToString();
                                guia.referenciaCliente = dt.Rows[0]["referenciaCliente"].ToString();
                                guia.TpDocCli = dt.Rows[0]["TpDocCli"].ToString();
                                guia.noDocCli = dt.Rows[0]["noDocCli"].ToString();
                                guia.nombreCliente = dt.Rows[0]["nombreCliente"].ToString();
                                guia.apePatCliente = dt.Rows[0]["apePatCliente"].ToString();
                                guia.apeMatCliente = dt.Rows[0]["apeMatCliente"].ToString();
                                guia.nombreCompletoCliente = dt.Rows[0]["nombreCompletoCliente"].ToString();
                                guia.cod_entid = dt.Rows[0]["cod_entid"].ToString();
                                guia.nombreEstado = dt.Rows[0]["nombreEstado"].ToString();
                                guia.detalles = lista;

                                /*****BarCode*****/
                                if (dt.Rows[0]["codSeguimiento"].ToString().Length > 0)
                                {
                                    //Create an instance of Linear Barcode
                                    //Use DataMatrixCrystal for Data Matrix
                                    //Use PDF417Crystal for PDF417
                                    //Use QRCodeCrystal for QR Code
                                    LinearCrystal barcode = new LinearCrystal();
                                    //Barcode settings
                                    barcode.Type = BarcodeType.CODE128;
                                    barcode.BarHeight = 100; //50 pixels
                                    barcode.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;

                                    barcode.Data = dt.Rows[0]["codSeguimiento"].ToString();
                                    byte[] imageData = barcode.drawBarcodeAsBytes();
                                    guia.BarCode = imageData;
                                }
 
                                /*****************/
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        
                    }
                }
            }
            catch 
            {
                
            }
            result = new List<ECommerce>();
            result.Add(guia);
            return result;
        }

        public List<DetallesECommerce> get_DetGuia_EC(string venta_id)
        {
            string sqlquery = "USP_REP_GUIA_ECOMMERCE";
            ECommerce guia = null;
            List<DetallesECommerce> lista = null;
            DataTable dt = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ven_id", venta_id);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                dt = new DataTable();
                                da.Fill(dt);
                                lista = new List<DetallesECommerce>();
                                lista = (from DataRow dr in dt.Rows
                                         select new DetallesECommerce()
                                         {
                                             codigoProducto = dr["codigoProducto"].ToString(),
                                             nombreProducto = dr["nombreProducto"].ToString(),
                                             cantidad = Convert.ToInt32(dr["cantidad"]),
                                             precioUnitario = Convert.ToDecimal(dr["precioUnitario"]),
                                             descuento = Convert.ToDecimal(dr["descuento"]),
                                             total = Convert.ToDecimal(dr["total"]),
                                             talla = dr["talla"].ToString(),
                                         }).ToList();

                                guia = new ECommerce();
                                guia.idPedido = dt.Rows[0]["idPedido"].ToString();
                                guia.Referencia = dt.Rows[0]["Referencia"].ToString();
                                guia.fechaPedido = Convert.ToDateTime(dt.Rows[0]["fechaPedido"]);
                                guia.tipoComprobante = dt.Rows[0]["tipoComprobante"].ToString();
                                guia.SerieDoc = dt.Rows[0]["SerieDoc"].ToString();
                                guia.GuiaNro = dt.Rows[0]["GuiaNro"].ToString();
                                guia.NroDoc = dt.Rows[0]["NroDoc"].ToString();
                                guia.codSeguimiento = dt.Rows[0]["codSeguimiento"].ToString();
                                guia.nom_courier = dt.Rows[0]["nom_courier"].ToString();
                                guia.estado = dt.Rows[0]["estado"].ToString();
                                guia.cliente = dt.Rows[0]["cliente"].ToString();
                                guia.direccionA = dt.Rows[0]["direccionA"].ToString();
                                guia.direccionB = dt.Rows[0]["direccionB"].ToString();
                                guia.direccionCliente = dt.Rows[0]["direccionCliente"].ToString();
                                guia.referenciaCliente = dt.Rows[0]["referenciaCliente"].ToString();
                                guia.TpDocCli = dt.Rows[0]["TpDocCli"].ToString();
                                guia.noDocCli = dt.Rows[0]["noDocCli"].ToString();
                                guia.nombreCliente = dt.Rows[0]["nombreCliente"].ToString();
                                guia.apePatCliente = dt.Rows[0]["apePatCliente"].ToString();
                                guia.apeMatCliente = dt.Rows[0]["apeMatCliente"].ToString();
                                guia.nombreCompletoCliente = dt.Rows[0]["nombreCompletoCliente"].ToString();
                                guia.cod_entid = dt.Rows[0]["cod_entid"].ToString();
                                guia.nombreEstado = dt.Rows[0]["nombreEstado"].ToString();
                                guia.detalles = lista;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch
            {

            }
            return lista;
        }
        #region<agregando vista adinson>
        public ReporteVentasEcommerce get_ecommerce_reporteventa(string cod_tda, string fecIni, string fecFin,string tipo)
        {
            ReporteVentasEcommerce lista = null;
            List<Models_VentasEcommerce> lista1 = null;
            var dt = new DataTable();
            var sqlquery = "USP_ECOMMERCE_REPORTE_VENTAS";

            try
            {
                using (var cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0)
                    {
                        cn.Open();
                    }
                    using (var cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fecha_inicio", fecIni);
                        cmd.Parameters.AddWithValue("@fecha_fin", fecFin);
                        cmd.Parameters.AddWithValue("@usuario", cod_tda);
                        cmd.Parameters.AddWithValue("@tipo_despacho", tipo);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            lista1 = new List<Models_VentasEcommerce>();
                            lista1 = (from DataRow dr in ds.Tables[0].Rows
                                      select new Models_VentasEcommerce()
                                      {
                                          BOL_FAC = dr["BOL_FAC"].ToString(),
                                          NRO_DOC = dr["NRO_DOC"].ToString(),
                                          CLIENTE = dr["CLIENTE"].ToString(),
                                          PARES = string.IsNullOrEmpty(dr["PARES"].ToString()) ? 0 : Convert.ToInt32(dr["PARES"].ToString()),
                                          ACCESORIOS = string.IsNullOrEmpty(dr["ACCESORIOS"].ToString()) ? 0 : Convert.ToInt32(dr["ACCESORIOS"].ToString()),
                                          ROPA = string.IsNullOrEmpty(dr["ROPA"].ToString()) ? 0 : Convert.ToInt32(dr["ROPA"].ToString()),
                                          TOT_ARTICULO = string.IsNullOrEmpty(dr["TOT_ARTICULO"].ToString()) ? 0 : Convert.ToInt32(dr["TOT_ARTICULO"].ToString()),
                                          PRE_NETO = string.IsNullOrEmpty(dr["PRE_NETO"].ToString()) ? 0 : Convert.ToDecimal(dr["PRE_NETO"].ToString()),
                                          IGV = string.IsNullOrEmpty(dr["IGV"].ToString()) ? 0 : Convert.ToDecimal(dr["IGV"].ToString()),
                                          TOTAL = string.IsNullOrEmpty(dr["TOTAL"].ToString()) ? 0 : Convert.ToDecimal(dr["TOTAL"].ToString()),
                                          NOM_TIENDA = dr["NOM_TIENDA"].ToString(),
                                          FECHA_INICIO = dr["FECHA_INICIO"].ToString(),
                                          FECHA_FIN = dr["FECHA_FIN"].ToString(),
                                          TIP_DESPACHO = dr["TIP_DESPACHO"].ToString(),
                                      }).ToList();
                            lista = new ReporteVentasEcommerce();

                            lista.ListVentaEcommerce = lista1;
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


        public List<Ent_Combo> get_ListaTienda(string codTienda,int ind_)
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

        public List<Ent_Combo> get_ListaAlmacen_Apoyo()
        {
            List<Ent_Combo> list = null;
            string sqlquery = "USP_ECOMMERCE_ALMACEN_APOYO";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@codTienda", codTienda);
                        //cmd.Parameters.AddWithValue("@ind_", ind_);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Combo>();
                            Ent_Combo combo = new Ent_Combo();

                            while (dr.Read())
                            {
                                combo = new Ent_Combo();
                                combo.cbo_codigo = dr["cod_almac"].ToString();
                                combo.cbo_descripcion = dr["des_cadena"].ToString();

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
        #endregion
    }
}