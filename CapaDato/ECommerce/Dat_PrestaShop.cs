using CapaEntidad.ECommerce;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CapaDato.ECommerce
{
    public class Dat_PrestaShop
    {
        public  string[] Update_Pedido_Prestashop(decimal _usu, decimal _idCust, string _reference, decimal _discCommPctg,
                                             decimal _discCommValue, string _shipTo, string _specialInstr, List<Order_Dtl> _itemsDetail,
                                             decimal _varpercepcion, Int32 _estado, string _ped_id = "", string _liq = "", Int32 _liq_dir = 0,
                                             Int32 _PagPos = 0, string _PagoPostarjeta = "", string _PagoNumConsignacion = "", decimal _PagoTotal = 0, DataTable dtpago = null,
                                             Boolean _pago_credito = false, Decimal _porc_percepcion = 0,
                                             List<Order_Dtl_Temp> order_dtl_temp = null, decimal _Liq_Pst_Id = 0, string _Liq_Pst_Ref = "",
                                             Decimal _CostoE = 0, Cliente cl = null, Pagos pag = null,DateTime? _ped_fecha=null,decimal _liq_tot_cigv=0,
                                             string _ped_ubigeo_ent="",string _ped_dir_ent="",string _ped_ref_ent="", string _ped_nom_ent = "", string _ped_tel_ent = "", Decimal _det_peso=0, DataTable pagos = null,
                                             string _name_courier = null)
        {
            string[] resultDoc = new string[2];
            string sqlquery = "USP_Insertar_Modifica_Liquidacion";
            SqlConnection cn = null;
            SqlCommand cmd = null;
          
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Ped_Det_Id", typeof(string));
                dt.Columns.Add("Ped_Det_Items", typeof(Int32));
                dt.Columns.Add("Ped_Det_ArtId", typeof(string));
                dt.Columns.Add("Ped_Det_TalId", typeof(string));
                dt.Columns.Add("Ped_Det_Cantidad", typeof(Int32));
                dt.Columns.Add("Ped_Det_Costo", typeof(decimal));
                dt.Columns.Add("Ped_Det_Precio", typeof(decimal));
                dt.Columns.Add("Ped_Det_ComisionP", typeof(decimal));
                dt.Columns.Add("Ped_Det_ComisionM", typeof(decimal));

                dt.Columns.Add("Ped_Det_OfertaP", typeof(decimal));
                dt.Columns.Add("Ped_Det_OfertaM", typeof(decimal));
                dt.Columns.Add("Ped_Det_OfeID", typeof(decimal));

                dt.Columns.Add("Ped_Det_ArtDes", typeof(string));
                dt.Columns.Add("Ped_Det_Peso", typeof(decimal));


                int i = 1;
                // Recorrer todas las lineas adicionAQUARELLAs al detalle

                if (_itemsDetail != null)
                {
                    foreach (Order_Dtl item in _itemsDetail)
                    {
                        dt.Rows.Add(_ped_id, i, item._code, item._size, item._qty, 0, item._price, item._commissionPctg, Math.Round(item._commission, 2, MidpointRounding.AwayFromZero), item._ofe_porc, item._dscto, item._ofe_id,item._art_des,item._art_peso);
                        i++;
                    }
                }

                /*pedido original*/
                DataTable dtordertmp = new DataTable();
                dtordertmp.Columns.Add("items", typeof(Int32));
                dtordertmp.Columns.Add("articulo", typeof(string));
                dtordertmp.Columns.Add("talla", typeof(string));
                dtordertmp.Columns.Add("cantidad", typeof(Int32));




                if (order_dtl_temp != null)
                {
                    foreach (Order_Dtl_Temp item in order_dtl_temp)
                    {
                        dtordertmp.Rows.Add(item.items, item.articulo, item.talla, item.cantidad);
                    }
                }


                //grabar pedido
                cn = new SqlConnection(Ent_Conexion.conexionEcommerce);
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Estado", _estado);
                cmd.Parameters.AddWithValue("@Ped_Id", _ped_id);
                //cmd.Parameters.AddWithValue("@LiqId", _liq);
                cmd.Parameters.Add("@LiqId", SqlDbType.VarChar, 12);
                cmd.Parameters["@LiqId"].Value = _liq;
                cmd.Parameters["@LiqId"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters.AddWithValue("@Liq_BasId", _idCust);
                cmd.Parameters.AddWithValue("@Liq_ComisionP", _discCommPctg);
                cmd.Parameters.AddWithValue("@Liq_PercepcionM", _varpercepcion);
                cmd.Parameters.AddWithValue("@Liq_Usu", _usu);

                cmd.Parameters.AddWithValue("@Liq_Pst_Id", _Liq_Pst_Id);
                cmd.Parameters.AddWithValue("@Liq_Pst_Ref", _Liq_Pst_Ref);
                cmd.Parameters.AddWithValue("@liq_costoe", _CostoE);

                cmd.Parameters.AddWithValue("@liq_fecha", _ped_fecha);
                cmd.Parameters.AddWithValue("@liq_tot_cigv", _liq_tot_cigv);

                cmd.Parameters.AddWithValue("@liq_Ubigeo_ent", _ped_ubigeo_ent);
                cmd.Parameters.AddWithValue("@liq_dir_ent", _ped_dir_ent);
                cmd.Parameters.AddWithValue("@liq_dir_ref", _ped_ref_ent);
                // Modificado por : Henry Morales - 21/05/2018
                // Se agergaron los campos de nombre y telefono de referencia de entrega
                cmd.Parameters.AddWithValue("@liq_nom_ref", _ped_nom_ent);
                cmd.Parameters.AddWithValue("@liq_tel_ref", _ped_tel_ent);
                cmd.Parameters.AddWithValue("@liq_pes_tot", _det_peso);
   
                /*ingreso de clientes*/
                cmd.Parameters.AddWithValue("@bas_nombres", cl.cli_nombres);
                cmd.Parameters.AddWithValue("@bas_apellidos", cl.cli_apellidos);
                cmd.Parameters.AddWithValue("@bas_email", cl.cli_email);
                cmd.Parameters.AddWithValue("@bas_ubigeo", cl.cli_ubigeo);
                cmd.Parameters.AddWithValue("@bas_direccion", cl.cli_direc);
                cmd.Parameters.AddWithValue("@bas_telf", cl.cli_telf);
                cmd.Parameters.AddWithValue("@bas_cel", cl.cli_telf_mov);
                cmd.Parameters.AddWithValue("@bas_dni", cl.cli_dni);


                /****************************/
                /*METODO DE PAGOS*/
                // Modificado por : Henry Morales - 19/06/2018
                // Se agregó para mandar los diversos pagos hechos en la liquidación
                cmd.Parameters.AddWithValue("@Detalle_Pago_ps", pagos);
                /*cmd.Parameters.AddWithValue("@pag_metodo", pag.pag_metodo);
                cmd.Parameters.AddWithValue("@pag_metodo", pag.pag_metodo);
                cmd.Parameters.AddWithValue("@pag_nro_trans", pag.pag_nro_trans);
                cmd.Parameters.AddWithValue("@pag_fecha", pag.pag_fecha);
                cmd.Parameters.AddWithValue("@pag_monto", pag.pag_monto);*/

                /******************/

                cmd.Parameters.AddWithValue("@Detalle_Pedido", dt);
                cmd.Parameters.AddWithValue("@Liquidacion_Directa", _liq_dir);

                /*PEDIDO ORIGINAL*/
                cmd.Parameters.AddWithValue("@pedido_original", dtordertmp);

                //opcional pago por pos liquidacion directa
                cmd.Parameters.AddWithValue("@Pago_Pos", _PagPos);
                cmd.Parameters.AddWithValue("@Pago_PosTarjeta", _PagoPostarjeta);
                cmd.Parameters.AddWithValue("@Pago_numconsigacion", _PagoNumConsignacion);
                cmd.Parameters.AddWithValue("@Pago_Total", _PagoTotal);


                //pago directo de la liquidacion
                cmd.Parameters.AddWithValue("@DetallePago", dtpago);
                cmd.Parameters.AddWithValue("@Pago_Credito", _pago_credito);

                //porcentaje percepcion
                cmd.Parameters.AddWithValue("@Ped_Por_Perc", _porc_percepcion);

                cmd.Parameters.AddWithValue("@name_courier", _name_courier);
                //da = new SqlDataAdapter(cmd);
                //da.Fill(ds);

                cmd.ExecuteNonQuery();
                resultDoc[0] = cmd.Parameters["@LiqId"].Value.ToString();
            }
            catch (Exception ex)
            {
                if (cn != null)
                    if (cn.State == ConnectionState.Open) cn.Close();
                resultDoc[0] = "-1";
                resultDoc[1] = ex.Message;
            }
            if (cn != null)
                if (cn.State == ConnectionState.Open) cn.Close();
            return resultDoc;
        }

        public Boolean Existe_Pedido_Prestashop(string _nroped)
        {
            Boolean valida = false;
            string sqlquery = "USP_ExistePedido_Prestashop";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@liq_id", _nroped);
                        cmd.Parameters.Add("@existe", SqlDbType.Bit);
                        cmd.Parameters["@existe"].Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                        valida = (Boolean)cmd.Parameters["@existe"].Value;
                    }
                }
            }
            catch (Exception)
            {

                valida = false;
            }
            return valida;
        }

        /// <summary>
        /// retorna las guias de prestashop para cambiar a estado facturado
        /// </summary>
        /// <returns></returns>
        public DataTable getestadofac()
        {
            string sqlquery = "USP_Get_EstadoPresta";
            DataTable dt = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout =0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dt = new DataTable();
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception)
            {
                dt = null;
            }
            return dt;
        }
        /// <summary>
        /// validacion en la base sql para el estado sql
        /// </summary>
        /// <returns></returns>
        public Boolean updestafac_prestashop(string guia_prestashop)
        {
            string sqlquery = "USP_Upd_EstadoPresta";
            Boolean valida = false;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0) cn.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Ven_Pst_Ref", guia_prestashop);
                        cmd.ExecuteNonQuery();
                        valida = true;
                    }

                }
            }
            catch (Exception)
            {

                valida = false;
            }
            return valida;
        }



        /// <summary>
        /// retorna el numero de guia de prestashop y de urbano
        /// </summary>
        public void get_guia_presta_urba(string ven_id,ref string guia_presta,ref string guia_urb, ref string name_carrier)
        {
            string sqlquery = "[USP_Get_GuiaUrbXVentas_jvh]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ven_id", ven_id);

                        cmd.Parameters.Add("@cod_prestashop", SqlDbType.VarChar, 30);
                        cmd.Parameters.Add("@cod_urbano", SqlDbType.VarChar, 30);
                        cmd.Parameters.Add("@name_carrier", SqlDbType.VarChar, 15);

                        cmd.Parameters["@cod_prestashop"].Direction = ParameterDirection.Output;
                        cmd.Parameters["@cod_urbano"].Direction = ParameterDirection.Output;
                        cmd.Parameters["@name_carrier"].Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        guia_presta =(string)cmd.Parameters["@cod_prestashop"].Value;
                        guia_urb = (string)cmd.Parameters["@cod_urbano"].Value;
                        name_carrier = (string)cmd.Parameters["@name_carrier"].Value;
                    }
                }
            }
            catch (Exception)
            {
                guia_presta = "";
                guia_urb = "";
                throw;
            }
        }


        public void get_carrier(string ven_id, ref string guia_presta, ref string name_carrier)
        {
            string sqlquery = "[USP_Get_GuiaUrbXVentas_jvh]";
            ///string guia_presta = "";
            string guia_urb = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ven_id", ven_id);

                        cmd.Parameters.Add("@cod_prestashop", SqlDbType.VarChar, 30);
                        cmd.Parameters.Add("@cod_urbano", SqlDbType.VarChar, 30);
                        cmd.Parameters.Add("@name_carrier", SqlDbType.VarChar, 50);

                        cmd.Parameters["@cod_prestashop"].Direction = ParameterDirection.Output;
                        cmd.Parameters["@cod_urbano"].Direction = ParameterDirection.Output;
                        cmd.Parameters["@name_carrier"].Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        guia_presta = (string)cmd.Parameters["@cod_prestashop"].Value;
                        guia_urb = (string)cmd.Parameters["@cod_urbano"].Value;
                        name_carrier = (string)cmd.Parameters["@name_carrier"].Value;
                    }
                }
            }
            catch (Exception)
            {
                guia_presta = "";
                guia_urb = "";
                throw;
            }
        }

    }
}
