using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad.ChatShop;

using System.Data.SqlClient;
using System.Data;
using CapaEntidad.Util;

namespace CapaDato.ChatShop
{
    public class Dat_ChatShop
    {
        public List<Ent_ChatShop> get_VentasChatShop(DateTime fdesde, DateTime fhasta, string noDocCli, string noDoc, string CodTda)
        {
            List<Ent_ChatShop> list = null;
            string sqlquery = "USP_CHATSHOP_LISTA_VENTAS_COURIER_2";
            //string _tienda = "";// (String)Session["Tienda"];
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FECHA_INI", fdesde.ToString("yyyyMMdd"));
                        cmd.Parameters.AddWithValue("@FECHA_FIN", fhasta.ToString("yyyyMMdd"));
                        cmd.Parameters.AddWithValue("@RUC", noDocCli);
                        cmd.Parameters.AddWithValue("@NRO_DOCUMENTO", noDoc);
                        cmd.Parameters.AddWithValue("@COD_TIENDA", CodTda);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_ChatShop>();

                            while (dr.Read())
                            {
                                Ent_ChatShop ven = new Ent_ChatShop();
                                ven.NroDocumento = dr["BOLETA"].ToString();
                                ven.Ruc = dr["RUC"].ToString();
                                ven.Cliente = dr["CLIENTE"].ToString();
                                ven.Tipo = dr["TIPO"].ToString();
                                ven.Importe = dr["IMPORTE"].ToString();
                                ven.Fecha = dr["FECHA"].ToString();
                                ven.CodSeguimiento = dr["COD_SEGUIMIENTO"].ToString();
                                ven.Tienda = dr["COD_TIENDA"].ToString();
                                ven.CodInterno = dr["COD_INTERNO"].ToString();
                                ven.Ubigeo = dr["UBIGEO"].ToString();
                                ven.Telefono = dr["TELEFONO"].ToString();
                                ven.Direccion = dr["DIR_CLIENTE"].ToString();
                                ven.Referencia = dr["REF_DIRECCION"].ToString();
                                ven.Estado = dr["ESTADO"].ToString();
                                ven.FlagCourier = dr["FLAG_COURIER"].ToString();
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
        public Ent_ChatShop get_Ventas_por_ChatShop(string Tienda, string CodInterno)
        {
            Ent_ChatShop ven = null;
            string sqlquery = "USP_CHATSHOP_CHASKI";
            try
            {
                //Ent_Conexion.conexion = "Server=192.168.1.242;Database=BDPOS;User ID=sa;Password=1;Trusted_Connection=False;";
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@serie_numero", noDoc);
                        cmd.Parameters.AddWithValue("@cod_entid", Tienda);
                        cmd.Parameters.AddWithValue("@fc_nint", CodInterno);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (ds.Tables.Count > 0)
                        {
                            //DataTable dtC = ds.Tables[0];
                            DataTable dtD = ds.Tables[0]; //data articulos
                            DataTable dtIC = ds.Tables[1]; //data chaski
                            DataTable dtID = ds.Tables[2]; //data direccion cliente 
                            ven = new Ent_ChatShop();
                            /*ARTICULOS*/
                            List<Ent_DetallesVentaCanal_2> listVenD = new List<Ent_DetallesVentaCanal_2>();
                            foreach (DataRow item in dtD.Rows)
                            {
                                Ent_DetallesVentaCanal_2 venD = new Ent_DetallesVentaCanal_2();
                                venD.codigoProducto = item["FD_ARTI"].ToString();
                                venD.nombreProducto = item["des_artic"].ToString();
                                venD.precioUnitario = item["FD_PREF"].ToString();
                                venD.descuento = item["FD_DREF"].ToString();
                                venD.total = item["FD_TOTAL"].ToString();
                                venD.cantidad = Convert.ToInt32(Convert.ToDouble(item["FD_QFAC"].ToString()));
                                venD.talla = item["FD_REGL"].ToString();
                                venD.fd_colo = item["FD_COLO"].ToString();
                                listVenD.Add(venD);
                            }
                            ven.detalles2 = listVenD;
                            /*TIENDA ORIGEN*/
                            if (dtIC.Rows.Count > 0)
                            {
                                Ent_Informacion_Tienda_envio_2 ic = new Ent_Informacion_Tienda_envio_2();
                                ic.id = Convert.ToInt32(dtIC.Rows[0]["id"]);
                                ic.cod_entid = dtIC.Rows[0]["cod_entid"].ToString();
                                ic.courier = dtIC.Rows[0]["courier"].ToString();
                                ic.cx_nroDocProveedor = dtIC.Rows[0]["cx_nroDocProveedor"].ToString();
                                ic.cx_codTipoDocProveedor = dtIC.Rows[0]["cx_codTipoDocProveedor"].ToString();
                                ic.cx_codDireccionProveedor = dtIC.Rows[0]["cx_codDireccionProveedor"].ToString();
                                ic.cx_codCliente = dtIC.Rows[0]["cx_codCliente"].ToString();
                                ic.cx_codCtaCliente = dtIC.Rows[0]["cx_codCtaCliente"].ToString();
                                ic.id_usuario = dtIC.Rows[0]["id_usuario"].ToString();
                                ic.de_terminal = dtIC.Rows[0]["de_terminal"].ToString();
                                ic.chaski_storeId = dtIC.Rows[0]["chaski_storeId"].ToString();
                                ic.chaski_branchId = dtIC.Rows[0]["chaski_branchId"].ToString();
                                ic.chaski_api_key = dtIC.Rows[0]["chaski_api_key"].ToString();
                                ven.informacionTiendaEnvio = ic;
                            }
                            /*CLENTE DESTINO*/
                            if (dtID.Rows.Count > 0) //des
                            {
                                Ent_Informacion_Tienda_Destinatario_2 id = new Ent_Informacion_Tienda_Destinatario_2();
                              
                                //id.id = Convert.ToInt32(dtID.Rows[0]["id"]);
                                //id.nroDocumento = dtID.Rows[0]["nroDocumento"].ToString();
                                //id.email = dtID.Rows[0]["email"].ToString();
                                id.referencia = dtID.Rows[0]["FC_REFERE"].ToString();
                                id.telefono = dtID.Rows[0]["fc_lcon"].ToString();
                                id.direccion_entrega = dtID.Rows[0]["FC_DCLI"].ToString();
                                id.cod_entid = dtID.Rows[0]["COD_ENTID"].ToString();
                                id.ubigeo = dtID.Rows[0]["FC_UBI"].ToString();

                                ven.informacionTiendaDestinatario = id;
                            }

                            //ven.historialEstados = listHist;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ven = null;
            }
            return ven;
        }

        public void insertar_ge_chatshop(string cod_entid, string fc_nint, string serie_numero, string ge)
        {
            string sqlquery = "USP_CHATSHOP_INSERT_UPD";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cod_entid", cod_entid);
                        cmd.Parameters.AddWithValue("@fc_nint", fc_nint);
                        cmd.Parameters.AddWithValue("@serie_numero", serie_numero);
                        cmd.Parameters.AddWithValue("@ge", ge);
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }


    }
}
