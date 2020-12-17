using CapaEntidad.Util;
using CapaEntidad.ECommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace CapaDato.ECommerce
{
    public class Dat_ECommerce
    {
        public DataTable get_tienda_origenes()
        {
            string sqlquery = "[sp_get_tiendas_origen]";
            DataTable origenes = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    // if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(origenes);
                    }
                }
            }
            catch (Exception ex)
            {
                origenes = null;
            }
            return origenes;
        }

        //public void insertar_historial_estados_cv(Ent_HistorialEstadosCV historial)
        //{
        //    string sqlquery = "usp_insertar_historial_estados_cv";
        //    try
        //    {
        //        using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
        //        {
        //            if (cn.State == 0) cn.Open();
        //            using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@cod_entid", historial.cod_entid );
        //                cmd.Parameters.AddWithValue("@fc_nint", historial.fc_nint );
        //                cmd.Parameters.AddWithValue("@id_estado", historial.id_estado );
        //                cmd.Parameters.AddWithValue("@descripcion", historial.descripcion );
        //                cmd.Parameters.AddWithValue("@cod_usuario", historial.cod_usuario );
        //                cmd.Parameters.AddWithValue("@cod_vendedor", historial.cod_vendedor);
        //                cmd.ExecuteNonQuery();
        //            }

        //        }
        //    }catch (Exception ex)
        //    {

        //    }
        //}

        public List<Ent_ECommerce> get_Ventas(DateTime fdesde, DateTime fhasta, string noDocCli, string noDoc, string _tienda)
        {
            List<Ent_ECommerce> list = null;
            string sqlquery = "USP_ECOM_Lista_Ventas";
            //string _tienda = "";// (String)Session["Tienda"];
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Fec_Ini", fdesde.ToString("yyyyMMdd"));
                        cmd.Parameters.AddWithValue("@Fec_Fin", fhasta.ToString("yyyyMMdd"));
                        cmd.Parameters.AddWithValue("@NDoc_Cli", noDocCli);
                        cmd.Parameters.AddWithValue("@Nro_Doc", noDoc);
                        cmd.Parameters.AddWithValue("@Tda_Id", _tienda);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_ECommerce>();

                            while (dr.Read())
                            {
                                Ent_ECommerce ven = new Ent_ECommerce();
                                ven.idPedido = dr["Pedido_Id"].ToString();
                                ven.RefPedido = dr["Pedido_Ref"].ToString();
                                ven.fechaPedido = Convert.ToDateTime(dr["Pedido_Fecha"]);
                                ven.tipoComprobante = dr["Doc_Tipo"].ToString();
                                ven.SerieDoc = dr["Doc_Serie"].ToString();
                                ven.NroDoc = dr["Doc_Nro"].ToString();
                                ven.CodSeguimiento = dr["Doc_Seguimiento"].ToString();
                                ven.nom_courier = dr["Nom_courier"].ToString();
                                ven.estado = dr["Estado"].ToString();
                                ven.cliente = dr["Cliente_Id"].ToString();
                                ven.direccionCliente = dr["Cliente_Direccion"].ToString();
                                ven.referenciaCliente = dr["Cliente_Direccion"].ToString();
                                ven.TpDocCli = dr["Cliente_Tp_Doc"].ToString();
                                ven.noDocCli = dr["Cliente_No_Doc"].ToString();
                                ven.nombreCliente = dr["Cliente_Nombre"].ToString();
                                ven.apePatCliente = dr["Cliente_ApePat"].ToString();
                                ven.apeMatCliente = dr["Cliente_ApeMat"].ToString();
                                ven.nombreCompletoCliente = dr["Cliente_NombreCompleto"].ToString();
                                ven.cod_entid = dr["Entidad_Id"].ToString();
                                ven.nombreEstado = dr["Nombre_Estado"].ToString();
                                list.Add(ven);
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


        public Ent_ECommerce get_Ventas_por_sn(string noDoc, string cod_entid)
        {
            Ent_ECommerce ven = null;
            string sqlquery = "USP_Select_Ventas_x_Tda";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NroDoc", noDoc);
                        cmd.Parameters.AddWithValue("@Tda_Id", cod_entid);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        if (ds.Tables.Count > 0)
                        {
                            DataTable dtC = ds.Tables[0];
                            DataTable dtD = ds.Tables[1];
                            //DataTable dtH = ds.Tables[2];
                            ven = new Ent_ECommerce();
                            ven.idPedido = dtC.Rows[0]["Pedido_Id"].ToString();
                            //ven.tiendaOrigen = dtC.Rows[0]["COD_ENTID"].ToString() + " - " + dtC.Rows[0]["des_entida"].ToString();
                            //ven.tiendaDestino = dtC.Rows[0]["FC_ID_TDACVTA"].ToString() + " - " + dtC.Rows[0]["des_entidb"].ToString();
                            ven.fechaPedido = Convert.ToDateTime(dtC.Rows[0]["Pedido_Fecha"]);
                            ven.tipoComprobante = dtC.Rows[0]["Doc_Tipo"].ToString();
                            ven.SerieDoc = dtC.Rows[0]["Doc_Serie"].ToString();
                            ven.NroDoc = dtC.Rows[0]["Doc_Nro"].ToString();
                            ven.CodSeguimiento = dtC.Rows[0]["Doc_Seguimiento"].ToString();
                            ven.CodSeguimiento = dtC.Rows[0]["Nom_Courier"].ToString();
                            ven.estado = dtC.Rows[0]["Estado"].ToString();
                            ven.cliente = dtC.Rows[0]["Cliente_Id"].ToString();
                            ven.direccionA = "";//dtC.Rows[0]["direccion_a"].ToString();
                            ven.direccionB = "";//dtC.Rows[0]["direccion_b"].ToString();
                            ven.direccionCliente = dtC.Rows[0]["Cliente_Direccion"].ToString();
                            ven.referenciaCliente = "";//dtC.Rows[0]["FC_REFERE"].ToString();
                            //ven.hora = dtC.Rows[0]["FC_HORA"].ToString();
                            ven.TpDocCli = dtC.Rows[0]["Cliente_Tp_Doc"].ToString();
                            ven.noDocCli = dtC.Rows[0]["Cliente_No_Doc"].ToString();
                            ven.nombreCliente = dtC.Rows[0]["Cliente_Nombre"].ToString();
                            ven.apePatCliente = dtC.Rows[0]["Cliente_ApePat"].ToString();
                            ven.apeMatCliente = dtC.Rows[0]["Cliente_ApeMat"].ToString();
                            ven.nombreCompletoCliente = dtC.Rows[0]["Cliente_NombreCompleto"].ToString();
                            ven.cod_entid = dtC.Rows[0]["Entidad_Id"].ToString();
                            //ven.idVendedor = dtC.Rows[0]["FC_VEND"].ToString();
                            //ven.nomVendedor = dtC.Rows[0]["V_NOMB"].ToString();
                            ven.nombreEstado = dtC.Rows[0]["nombreEstado"].ToString();
                            //ven.descripcionEstado = dtC.Rows[0]["descripcionEstado"].ToString();
                            //ven.colorEstado = dtC.Rows[0]["colorEstado"].ToString();
                            //ven.importeTotal = Convert.ToDecimal(dtC.Rows[0]["FC_TOTAL"].ToString());
                            //ven.nombreTipoCV = dtC.Rows[0]["nombre_tipo_cv"].ToString();

                            List<Ent_DetallesECommerce> listVenD = new List<Ent_DetallesECommerce>();
                            foreach (DataRow item in dtD.Rows)
                            {
                                Ent_DetallesECommerce venD = new Ent_DetallesECommerce();
                                venD.codigoProducto = item["Producto_Id"].ToString();
                                venD.nombreProducto = item["Producto_Desc"].ToString();
                                venD.cantidad = item["Producto_Cant"].ToString();
                                venD.precioUnitario = item["Producto_Prec_Unit"].ToString();
                                venD.descuento = item["Producto_Dcto"].ToString();
                                venD.total = item["Producto_Total"].ToString();
                                venD.talla = item["Producto_Talla"].ToString();
                                listVenD.Add(venD);
                            }
                            ven.detalles = listVenD;
                            //List<Ent_HistorialEstadosCV> listHist = new List<Ent_HistorialEstadosCV>();
                            //foreach (DataRow item in dtH.Rows)
                            //{
                            //    Ent_HistorialEstadosCV _hist = new Ent_HistorialEstadosCV();
                            //    _hist.cod_entid = item["cod_entid"].ToString();
                            //    _hist.fc_nint = item["fc_nint"].ToString();
                            //    _hist.id_estado = item["id_estado"].ToString();
                            //    _hist.fecha =(DateTime)item["fecha"];
                            //    _hist.cod_usuario = item["usu_id"].ToString();
                            //    _hist.descripcion = item["descripcion"].ToString();
                            //    _hist.usu_nombre = item["usu_nombre"].ToString();
                            //    _hist.nombreEstado = item["nombreEstado"].ToString();
                            //    _hist.colorEstado = item["colorEstado"].ToString();
                            //    _hist.descripcionEstado = item["descripcionEstado"].ToString();
                            //    _hist.cod_vendedor = item["cod_vendedor"].ToString();
                            //    _hist.nomVendedor = item["v_nomb"].ToString();
                            //    listHist.Add(_hist);
                            //}
                            //ven.historialEstados = listHist;
                        }
                    }
                }
            }
            catch (Exception)
            {
                ven = null;
            }
            return ven;
        }

        public DataTable get_vendedores_tda(string cod_tda)
        {
            string sqlquery = "[sp_get_vendedores_tda_cv]";
            DataTable dt_vendedores = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    // if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cod_tda", cod_tda);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt_vendedores);
                    }
                }
            }
            catch (Exception ex)
            {
                dt_vendedores = null;
            }
            return dt_vendedores;
        }

        public DataTable get_tiendas_destino(string tiendaOrigen)
        {
            string sqlquery = "[sp_get_tiendas_destino]";
            DataTable destinos = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    // if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codTdaOri", tiendaOrigen);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(destinos);
                    }
                }
            }
            catch (Exception ex)
            {
                destinos = null;
            }
            return destinos;
        }

        //STOCK ALMACEN 15-05-2020
        public List<Ent_Stock_Almacen> get_Lista_Stock_Almacen(string cod_almacen, string cod_articulo, string descripcion, string talla)
        {
            string sqlquery = "USP_ECOMMERCE_STOCK_ALMACEN";
            DataTable dt = null;
            List<Ent_Stock_Almacen> listar = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@COD_ALMACEN", cod_almacen);
                        cmd.Parameters.AddWithValue("@COD_ARTICULO", cod_articulo);
                        cmd.Parameters.AddWithValue("@DESCRIPCION", descripcion);
                        cmd.Parameters.AddWithValue("@TALLA", talla);
                        //cmd.Parameters.AddWithValue("@estado", dwest);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dt = new DataTable();
                            da.Fill(dt);
                            listar = new List<Ent_Stock_Almacen>();
                            listar = (from DataRow dr in dt.Rows
                                      select new Ent_Stock_Almacen()
                                      {
                                          des_almacen = dr["DES_ALMACEN"].ToString(),
                                          cod_articulo = dr["COD_ARTICULO"].ToString(),
                                          descripcion = dr["DESCRIPCION"].ToString(),
                                          talla = dr["TALLA"].ToString(),
                                          stock = dr["STOCK"].ToString(),

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


        //LISTA PEDIDOS PRESTASHOP

        public List<Ent_Prestashop> get_Lista_Pedidos_Prestashop(DateTime Fecha_Ini, DateTime Fecha_Fin)
        {
            string sqlquery = "USP_GET_PEDIDOS_CARRITO";
            DataTable dt = null;
            List<Ent_Prestashop> listar = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FECHAINI", Fecha_Ini);
                        cmd.Parameters.AddWithValue("@FECHAFIN", Fecha_Fin);
                        cmd.Parameters.AddWithValue("@ESTADO", "TODO");
                        //cmd.Parameters.AddWithValue("@estado", dwest);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dt = new DataTable();
                            da.Fill(dt);
                            listar = new List<Ent_Prestashop>();
                            listar = (from DataRow dr in dt.Rows
                                      select new Ent_Prestashop()
                                      {
                                          Id_Orden = dr["ID_ORDEN"].ToString(),
                                          Fec_Pedido = dr["FECHA_PED"] == null || dr["FECHA_PED"].ToString() == "" ? "" : Convert.ToDateTime(dr["FECHA_PED"]).ToString("dd/MM/yyyy"),
                                          Est_Sis_Fact = dr["ESTADO_SIST_FACT"].ToString(),
                                          Presta_Estado = dr["PRESTA_ESTADO"] == null || dr["PRESTA_ESTADO"].ToString() == "" ? "" : (dr["PRESTA_ESTADO"]).ToString(),
                                          Presta_Estado_Name = dr["PRESTA_ESTADO_NAME"].ToString(),
                                          Presta_Est_Ped_Tienda = dr["PRESTA_EST_PED_TIENDA"].ToString(),

                                          Presta_FecIng = dr["PRESTA_FECING"] == null || dr["PRESTA_FECING"].ToString() == "" ? "" : Convert.ToDateTime(dr["PRESTA_FECING"]).ToString("dd/MM/yyyy"),
                                          Fecha_Facturacion = dr["fecha_facturacion"] == null || dr["fecha_facturacion"].ToString() == "" ? "" : Convert.ToDateTime(dr["fecha_facturacion"]).ToString("dd/MM/yyyy"),

                                          Comprobante = dr["comprobante"].ToString(),
                                          Name_Carrier = dr["name_carrier"].ToString(),
                                          Almacen = dr["almacen"].ToString(),
                                          Ubigeo_Ent = dr["ubigeo_ent"].ToString(),
                                          Ubicacion = dr["ubicacion"].ToString(),
                                          Semana = dr["semana"].ToString(),
                                          ArticuloId = dr["ARTICULOID"].ToString(),

                                          Talla = dr["TALLA"].ToString(),
                                          Cantidad = Convert.ToInt32(dr["CANTIDAD"]),

                                          Precio_Vta = Math.Round(Convert.ToDecimal(dr["PrecioVta"]), 2),
                                          Precio_Original = Math.Round(Convert.ToDecimal(dr["PrecioOriginal"]), 2),

                                          Cod_Linea3 = dr["cod_line3"].ToString(),
                                          Des_Linea3 = dr["des_line3"].ToString(),
                                          Cod_Cate3 = dr["cod_cate3"].ToString(),
                                          Des_Cate3 = dr["des_cate3"].ToString(),
                                          Cod_Subc3 = dr["cod_subc3"].ToString(),
                                          Des_Subc3 = dr["des_subc3"].ToString(),
                                          Cod_Marc3 = dr["cod_marc3"].ToString(),
                                          Des_Marca = dr["des_marca"].ToString(),

                                          Precio_Planilla = Math.Round(Convert.ToDecimal(dr["PrecioPlanilla"]), 2),
                                          Costo = Math.Round(Convert.ToDecimal(dr["Costo"]), 2),

                                          Alm_C = Convert.ToInt32(dr["C"]),
                                          Alm_5 = Convert.ToInt32(dr["5"]),
                                          Alm_B = Convert.ToInt32(dr["B"]),
                                          Alm_W = Convert.ToInt32(dr["W"]),
                                          Alm_1 = Convert.ToInt32(dr["1"]),
                                      }).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listar = null;
            }
            return listar;
        }

        //LISTA PEDIDOS DE ALMACEN - TRAZA
        public List<Ent_TrazaPedido> get_lista(DateTime fechaini, DateTime fechafin)
        {
            string sqlquery = "USP_GetTrazabilidadPedido";
            List<Ent_TrazaPedido> listar = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FECHA_INICIO", fechaini);
                        cmd.Parameters.AddWithValue("@FECHA_FIN", fechafin);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            listar = new List<Ent_TrazaPedido>();
                            listar = (from DataRow dr in dt.Rows
                                      select new Ent_TrazaPedido()
                                      {
                                          ID_PEDIDO = dr["ID_PEDIDO"].ToString(),
                                          IMPORTE_PEDIDO = dr["IMPORTE_PEDIDO"].ToString(),
                                          FECHA_PEDIDO = dr["FECHA_PEDIDO"].ToString(),
                                          DESPACHO = dr["DESPACHO"].ToString(),
                                          TIPO_ENTREGA = dr["TIPO_ENTREGA"].ToString(),
                                          FECHA_ING_FACTURACION = dr["FECHA_ING_FACTURACION"].ToString(),
                                          FECHA_REG_VENTA = dr["FECHA_REG_VENTA"].ToString(),
                                          FECHA_REG_COURIER = dr["FECHA_REG_COURIER"].ToString(),
                                          CLIENTE = dr["CLIENTE"].ToString(),
                                          //TRAZABILIDAD = dr["TRAZABILIDAD"].ToString(),
                                          ESTADO = dr["ESTADO"].ToString(),
                                          COLOR = dr["COLOR"].ToString(),
                                          FLG_REASIGNA = Convert.ToInt32( dr["FLG_REASIGNA"]),
                                      }).ToList();

                        }
                    }
                }
            }
            catch (Exception exc)
            {

                listar = null;
            }
            return listar;
        }

        public Boolean update_pedido_ecommerce(string liq_id, string accion,int flagWMS,int flagcorreo,int flagreasignar)
        {
            Boolean valida = false;
            string sqlquery = "USP_Anular_Liquidacion2";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Liq_Id", liq_id);
                        cmd.Parameters.AddWithValue("@Accion", accion);
                        cmd.Parameters.AddWithValue("@flagWMS", flagWMS);
                        cmd.Parameters.AddWithValue("@flag_correo", flagcorreo);
                        cmd.Parameters.AddWithValue("@flag_reasignar", flagreasignar);
                        cmd.ExecuteNonQuery();
                        valida = true;
                    }
                }
            }
            catch (Exception)
            {
                valida = false;
                throw;
            }
            return valida;
        }

        public DataTable Ecommerce_getConexionesAPI(string nombre, int tipo)
        {
            DataTable dt = null;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            string sqlcommand = "USP_Lista_APICourier";
            try
            {
                cn = new SqlConnection(Ent_Conexion.conexionEcommerce);
                cmd = new SqlCommand(sqlcommand, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Tipo", tipo);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
            }
            catch
            {
                dt = null;
                throw;
            }
            return dt;
        }


    }

}
