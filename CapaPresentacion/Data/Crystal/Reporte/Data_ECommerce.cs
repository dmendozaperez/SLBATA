using CapaEntidad.Util;
using CapaPresentacion.Models.ECommerce;
using Models.Crystal.Reporte;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    }
}