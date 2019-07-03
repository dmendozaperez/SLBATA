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
        public ECommerce getGuia_EC(string venta_id)
        {
            string sqlquery = "USP_REP_GUIA_ECOMMERCE";
            ECommerce guia = null;
            List<DetallesECommerce> lista = null;
            DataTable dt = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
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
                                             codigoProducto = dr["stock"].ToString(),
                                             nombreProducto = dr["stock"].ToString(),
                                             cantidad = dr["stock"].ToString(),
                                             precioUnitario = dr["stock"].ToString(),
                                             descuento = dr["stock"].ToString(),
                                             total = dr["stock"].ToString(),
                                             talla = dr["stock"].ToString(),
                                         }).ToList();

                                guia = new ECommerce();
                                guia.idPedido = dt.Rows[0]["stock"].ToString();
                                guia.Referencia = dt.Rows[0]["stock"].ToString();
                                guia.fechaPedido = Convert.ToDateTime(dt.Rows[0]["stock"]);
                                guia.tipoComprobante = dt.Rows[0]["stock"].ToString();
                                guia.SerieDoc = dt.Rows[0]["stock"].ToString();
                                guia.GuiaNro = dt.Rows[0]["stock"].ToString();
                                guia.NroDoc = dt.Rows[0]["stock"].ToString();
                                guia.codSeguimiento = dt.Rows[0]["stock"].ToString();
                                guia.nom_courier = dt.Rows[0]["stock"].ToString();
                                guia.estado = dt.Rows[0]["stock"].ToString();
                                guia.cliente = dt.Rows[0]["stock"].ToString();
                                guia.direccionA = dt.Rows[0]["stock"].ToString();
                                guia.direccionB = dt.Rows[0]["stock"].ToString();
                                guia.direccionCliente = dt.Rows[0]["stock"].ToString();
                                guia.referenciaCliente = dt.Rows[0]["stock"].ToString();
                                guia.TpDocCli = dt.Rows[0]["stock"].ToString();
                                guia.noDocCli = dt.Rows[0]["stock"].ToString();
                                guia.nombreCliente = dt.Rows[0]["stock"].ToString();
                                guia.apePatCliente = dt.Rows[0]["stock"].ToString();
                                guia.apeMatCliente = dt.Rows[0]["stock"].ToString();
                                guia.nombreCompletoCliente = dt.Rows[0]["stock"].ToString();
                                guia.cod_entid = dt.Rows[0]["stock"].ToString();
                                guia.nombreEstado = dt.Rows[0]["stock"].ToString();
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
            return guia;
        }
        
    }
}