using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaPresentacion.Models.ECommerce;
using CapaEntidad.ECommerce;
using CapaEntidad.Util;
using CapaEntidad.Control;
using CapaDato.ECommerce;
using CapaDato.ECommerce.Urbano;
using CapaDato.ECommerce.Chazki;
using CapaDato.ECommerce.Savar;
using CapaDato.CanalVenta;
using CapaDato.comercioxpress;
using Data.Crystal.Reporte;
using CapaPresentacion.Bll;
using CapaEntidad.Menu;
using CapaEntidad.General;
using Newtonsoft.Json;
using CapaEntidad.ValeCompra;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Cors;
using System.Net;
using System.Text.RegularExpressions;

namespace CapaPresentacion.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")] //CORS

    public class ECommerceController : Controller
    {

        Dat_ECommerce datos = new Dat_ECommerce();

        //List<Ent_TrazaPedido> listTraza = new List<Ent_TrazaPedido>();
        private string _session_listTraza_private = "_session_listTraza_private";
        // GET: Cnvta

        public ActionResult Index()
        {
            string fdesde = (Request.HttpMethod == "POST" ? Request.Params["fdesde"].ToString() : DateTime.Now.ToString("dd/MM/yyyy"));
            string fhasta = (Request.HttpMethod == "POST" ? Request.Params["fhasta"].ToString() : DateTime.Now.ToString("dd/MM/yyyy"));
            string noDocCli = (Request.HttpMethod == "POST" ? Request.Params["noDocCli"].ToString() : null);
            string noDoc = (Request.HttpMethod == "POST" ? Request.Params["noDoc"].ToString() : null);

            ViewBag._fdesde = fdesde;
            ViewBag._fhasta = fhasta;
            ViewBag._noDocCli = noDocCli;
            ViewBag._noDoc = noDoc;

            List<ECommerce> listPed = new List<ECommerce>();
            listPed = selectVentas(Convert.ToDateTime(fdesde), Convert.ToDateTime(fhasta), noDocCli, noDoc);
            /*Parametros para el reporte*/
            if (Request.HttpMethod == "POST")
            {
                this.HttpContext.Session["rptSource"] = listPed;
                this.HttpContext.Session["pDesde"] = fdesde;
                this.HttpContext.Session["pHasta"] = fhasta;
                //this.HttpContext.Session[""] = ;
                //this.HttpContext.Session[""] = ;
                //this.HttpContext.Session[""] = ;
            }

            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];

            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;
            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {
                return View(listPed);
            }
            //if (Request.HttpMethod == "POST")
            //{               

            //}
            //else
            //{
            //    ViewBag.selectOrigen = SelectOrigen();
            //    //ViewBag.ventas = selectVentas();
            //    return View(selectVentas());
            //}
        }

        public ActionResult GuiaEC(string ven_id)
        {
            return View();
            ; //RedirectToAction("GuiaEC", "ECommerce");
        }

        public ActionResult Envia_Courier(string ven_id)
        {
            //Basico.act_presta_urbano(grabar_numerodoc, ref _error, ref _cod_courier)
            Dat_PrestaShop action_presta = null;
            Dat_Urbano data_urbano = null;
            Dat_Cexpress data_Cexpress = null;
            var oJRespuesta = new JsonResponse();

            /*Datos para devolver*/
            //string error = ""; string cod_courier = "";
            try
            {
                string guia_presta = ""; string guia_courier = ""; string name_carrier = ""; //= "Chazki - Envíos Express";
                action_presta = new Dat_PrestaShop();
                data_urbano = new Dat_Urbano();
                //action_presta.get_guia_presta_urba(ven_id, ref guia_presta, ref guia_courier, ref name_carrier);

                action_presta.get_carrier(ven_id, ref guia_presta, ref name_carrier);
                string track_chazki;

                if (guia_presta.Trim().Length > 0)
                {
                    UpdaEstado updateestado = new UpdaEstado();
                    Boolean valida = updateestado.ActualizarReference(guia_presta);

                    //Boolean valida = true;
                    if (valida)
                    {
                        data_Cexpress = new Dat_Cexpress();
                        //action_presta.updestafac_prestashop(guia_presta);
                        EnviaPedidoCxpress envia2 = new EnviaPedidoCxpress();
                        string nroserv = "";

                        /*enviamos urbano la guia*/
                        EnviaPedido envia = new EnviaPedido();
                        /**/
                        if (name_carrier == "Comercio Xpress")
                        {
                            Ent_Cexpress ent_Cexpress = envia2.sendCexpress(ven_id, ref nroserv);
                        }
                        //intentando 3 veces
                        for (Int32 i = 0; i < 3; ++i)
                        {
                            /*Nuevo*/
                            if (name_carrier == "Comercio Xpress")
                            {
                                //Ent_Cexpress ent_Cexpress = envia2.sendCexpress(ven_id, ref nroserv);
                                action_presta.updestafac_prestashop(guia_presta);
                                data_Cexpress.update_guia(guia_presta, nroserv);
                                guia_courier = nroserv;
                                break;
                            }
                            else if (name_carrier.Contains("Chazki"))
                            {
                                string nrodelivery_chazki = Envia_Courier_chazki(ven_id); 
                                if (nrodelivery_chazki != "")
                                {
                                    action_presta.updestafac_prestashop(guia_presta);
                                    data_Cexpress.update_guia(guia_presta, ven_id);//se registra el  nro de BOL para codigo de seguimiento
                                    guia_courier = ven_id;
                                    break;
                                }
                            }
                            else if (name_carrier.Contains("Savar"))
                            {
                                string nrodelivery_savar = Envia_Courier_Savar(ven_id);
                                data_Cexpress.update_guia(guia_presta, nrodelivery_savar);
                                guia_courier = nrodelivery_savar;
                                break;

                            }
                            else
                            {
                                Ent_Urbano ent_urbano = envia.sendUrbano(ven_id);
                                if (ent_urbano.error == "1")
                                {
                                    if (ent_urbano.guia.Trim().Length > 0)
                                    {
                                        action_presta.updestafac_prestashop(guia_presta);
                                        data_urbano.update_guia(guia_presta, ent_urbano.guia);
                                        guia_courier = ent_urbano.guia;
                                        break;
                                    }
                                }
                            }
                        }
                        //guia_courier=
                        //action_presta.get_guia_presta_urba(ven_id, ref guia_presta, ref guia_courier);

                        ActTracking enviaguia_presta = new ActTracking();
                        string[] valida_prest;

                        valida_prest = enviaguia_presta.ActualizaTrackin(guia_presta, guia_courier);

                        /*el valor 1 quiere decir que actualizo prestashop*/
                        if (valida_prest[0] == "1" && guia_courier.ToString() != "")
                        {
                            data_urbano.updprestashopGuia(guia_presta, guia_courier);
                        }

                        //cod_courier = guia_courier;
                        oJRespuesta.Message = guia_courier;
                        //return Json(oJRespuesta, JsonRequestBehavior.AllowGet);
                        /************************/
                    }
                }
            }
            catch (Exception)
            {
                oJRespuesta.Message = "";
            }
            //return RedirectToAction("Index", "ECommerce");
            return Json(oJRespuesta, JsonRequestBehavior.AllowGet);
        }
        /*metodo para savar - ecommerce*/
        public string Envia_Courier_Savar(string ven_id)
        {
            DataTable dtApi_savar = new DataTable();
            EnviarPedidoSavar objD_savar = new EnviarPedidoSavar();
            List<Ent_Savar> objList_savar = new List<Ent_Savar>();
            Ent_Savar objE_savar = new Ent_Savar();

            dtApi_savar = objD_savar.get_Ventas_por_Savar(ven_id);
            objE_savar.CodPaquete = dtApi_savar.Rows[0]["CodPaquete"].ToString();
            objE_savar.NomRemitente = dtApi_savar.Rows[0]["NomRemitente"].ToString();
            objE_savar.DireccionRemitente = dtApi_savar.Rows[0]["DireccionRemitente"].ToString();
            objE_savar.DistritoRemitente = dtApi_savar.Rows[0]["DistritoRemitente"].ToString();
            objE_savar.TelefonoRemitente = dtApi_savar.Rows[0]["TelefonoRemitente"].ToString();
            objE_savar.CodigoProducto = dtApi_savar.Rows[0]["CodigoProducto"].ToString();
            objE_savar.MarcaProducto = dtApi_savar.Rows[0]["MarcaProducto"].ToString();
            objE_savar.ModeloProducto = dtApi_savar.Rows[0]["ModeloProducto"].ToString();
            objE_savar.ColorProducto = dtApi_savar.Rows[0]["ColorProducto"].ToString();
            objE_savar.TipoProducto = dtApi_savar.Rows[0]["TipoProducto"].ToString();
            objE_savar.DescProducto = dtApi_savar.Rows[0]["DescProducto"].ToString();
            objE_savar.cantidad = Convert.ToInt32(dtApi_savar.Rows[0]["cantidad"].ToString());
            objE_savar.NomConsignado = dtApi_savar.Rows[0]["NomConsignado"].ToString();
            objE_savar.NumDocConsignado = dtApi_savar.Rows[0]["NumDocConsignado"].ToString();
            objE_savar.DireccionConsignado = dtApi_savar.Rows[0]["DireccionConsignado"].ToString();
            objE_savar.DistritoConsignado = dtApi_savar.Rows[0]["DistritoConsignado"].ToString();
            objE_savar.Referencia = dtApi_savar.Rows[0]["Referencia"].ToString();
            objE_savar.TelefonoConsignado = dtApi_savar.Rows[0]["TelefonoConsignado"].ToString();
            objE_savar.CorreoConsignado = dtApi_savar.Rows[0]["CorreoConsignado"].ToString();
            objE_savar.Subservicio = dtApi_savar.Rows[0]["Subservicio"].ToString();
            objE_savar.TipoPago = dtApi_savar.Rows[0]["TipoPago"].ToString();
            objE_savar.MetodoPago = dtApi_savar.Rows[0]["MetodoPago"].ToString();
            objE_savar.Monto = Convert.ToDecimal(dtApi_savar.Rows[0]["Monto"].ToString());
            objE_savar.Largo = Convert.ToDecimal(dtApi_savar.Rows[0]["Largo"].ToString());
            objE_savar.Ancho = Convert.ToDecimal(dtApi_savar.Rows[0]["Ancho"].ToString());
            objE_savar.Peso = Convert.ToDecimal(dtApi_savar.Rows[0]["Peso"].ToString());
            objE_savar.ValorComercial = dtApi_savar.Rows[0]["ValorComercial"].ToString();
            objE_savar.HoraIni1 = dtApi_savar.Rows[0]["HoraIni1"].ToString();
            objE_savar.HoraFin1 = dtApi_savar.Rows[0]["HoraFin1"].ToString();
            objE_savar.HoraIni2 = dtApi_savar.Rows[0]["HoraIni2"].ToString();
            objE_savar.HoraFin2 = dtApi_savar.Rows[0]["HoraFin2"].ToString();
            objE_savar.Comentario = dtApi_savar.Rows[0]["Comentario"].ToString();
            objE_savar.Comentario2 = dtApi_savar.Rows[0]["Comentario2"].ToString();

            objList_savar.Add(objE_savar);
            string jsonSavar = JsonConvert.SerializeObject(objList_savar);
            string retorno = "";


            Dat_ECommerce objD_ecommerce = new Dat_ECommerce();
            DataTable dtConexion = new DataTable();

            dtConexion = objD_ecommerce.Ecommerce_getConexionesAPI("savar", 1); //conexion de savar

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", dtConexion.Rows[0]["Token"].ToString());
                    //client.DefaultRequestHeaders.Add("TOKEN", dtConexion.Rows[0]["Token"].ToString());

                    using (StringContent jsonContent = new StringContent(jsonSavar))
                    {
                        jsonContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        var request = client.PostAsync(dtConexion.Rows[0]["Url"].ToString(), jsonContent);

                        string response = request.Result.Content.ReadAsStringAsync().Result;
                        string codseguimiento = Regex.Replace(response, @"[^A-Za-z0-9ñÑ ]+", "");

                        if (objE_savar.CodPaquete == codseguimiento)
                        {
                            retorno = codseguimiento;
                        }
                        else
                        {
                            retorno = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                retorno = "";
            }

            return retorno;
        }


        /*metodo para chazki - ecommerce*/
        public string Envia_Courier_chazki(string ven_id)
        {
            string retorno = "";
            try
            {
                /*delivery CHASKI*/
                Ecommerce_Chazki cvCzk = selectVenta_Chazki(ven_id);
                List<Ent_Chazki_E> list_chazki = new List<Ent_Chazki_E>();
                if (cvCzk.informacionTiendaEnvio != null)
                {
                    /* DATA CHASKI : PRODUCCION*/
                    Ent_Chazki_E chazki = new Ent_Chazki_E();
                    chazki.storeId = cvCzk.informacionTiendaEnvio.chaski_storeId; // "10411"; // proporcionado por chazki
                    chazki.branchId = cvCzk.informacionTiendaEnvio.chaski_branchId; // proporcionado por chazki
                    chazki.deliveryTrackCode = cvCzk.informacionTiendaEnvio.nro_documento;
                    chazki.proofPayment = cvCzk.informacionTiendaEnvio.proofPayment; // por definir la evindencia que será entregada al cliente
                    chazki.deliveryCost = 0;
                    chazki.mode = cvCzk.informacionTiendaEnvio.mode; //pendiente definir el modo con el que se va a trabajar el canal de venta.
                    chazki.time = cvCzk.informacionTiendaEnvio.tiempo;
                    chazki.paymentMethod = cvCzk.informacionTiendaEnvio.payment_method;
                    chazki.country = "PE";

                    /* DATA CHASKI : TEST*/

                    //Ent_Chazki_E chazki = new Ent_Chazki_E();
                    //chazki.storeId = "10411";
                    //chazki.branchId = "CCSC-B187";
                    //chazki.deliveryTrackCode = cvCzk.informacionTiendaEnvio.nro_documento;
                    //chazki.proofPayment = "Ninguna"; // por definir la evindencia que será entregada al cliente
                    //chazki.deliveryCost = 0;
                    //chazki.mode = "Express"; //pendiente definir el modo con el que se va a trabajar el canal de venta.
                    //chazki.time = "";
                    //chazki.paymentMethod = "Pagado";
                    //chazki.country = "PE";

                    /* DATA ARTICULO*/

                    List<Ent_ItemSold_E> listItemSold = new List<Ent_ItemSold_E>();
                    foreach (var producto in cvCzk.detalles)
                    {
                        if (producto.codigoProducto != "9999997")
                        {
                            Ent_ItemSold_E _item = new Ent_ItemSold_E();
                            _item.name = producto.nombreProducto;
                            _item.currency = "PEN";
                            _item.price = Convert.ToDouble(producto.total);
                            _item.weight = 0.3;
                            _item.volumen = 0;
                            _item.quantity = producto.cantidad;
                            _item.unity = "Caja";
                            _item.size = "M";
                            listItemSold.Add(_item);
                        }
                    }

                    //CLIENTE
                    chazki.listItemSold = listItemSold;
                    chazki.notes = "Entregar a Cliente";
                    chazki.documentNumber = cvCzk.informacionTiendaDestinatario.nroDocumento;
                    //chazki.email = "servicio.clientes.peru@bata.com";
                    if (cvCzk.informacionTiendaDestinatario.email == "" || cvCzk.informacionTiendaDestinatario.email == null)
                    {
                        chazki.email = "servicio.clientes.peru @bata.com";
                    }
                    else
                    {
                        chazki.email = cvCzk.informacionTiendaDestinatario.email; //
                    }

                    chazki.phone = cvCzk.informacionTiendaDestinatario.telefono;
                    int CadRuc = cvCzk.informacionTiendaDestinatario.nroDocumento.Length;

                    if (CadRuc > 8)
                    {
                        chazki.documentType = "RUC";
                        chazki.lastName = "";
                        chazki.companyName = cvCzk.informacionTiendaDestinatario.cliente;
                        chazki.name_tmp = "";
                    }
                    else
                    {
                        chazki.documentType = "DNI";
                        chazki.companyName = "";
                        chazki.name_tmp = cvCzk.informacionTiendaDestinatario.cliente;
                        chazki.lastName = "";
                    }
                    /* DATA DIRECCION*/
                    List<Ent_AddressClient_E> listAdressClient = new List<Ent_AddressClient_E>();
                    Ent_AddressClient_E addressClient = new Ent_AddressClient_E();
                    Dat_CanalVenta datos = new Dat_CanalVenta();
                    string[] desUbigeo = null;

                    desUbigeo = datos.get_des_ubigeo(cvCzk.informacionTiendaDestinatario.ubigeo);
                    addressClient.nivel_2 = desUbigeo[0];
                    addressClient.nivel_3 = desUbigeo[1];
                    addressClient.nivel_4 = desUbigeo[2];
                    addressClient.name = cvCzk.informacionTiendaDestinatario.direccion_entrega;
                    addressClient.reference = cvCzk.informacionTiendaDestinatario.referencia;
                    addressClient.alias = "No Alias";
                    Ent_Position_E position = new Ent_Position_E();
                    position.latitude = 0;
                    position.longitude = 0;
                    addressClient.position = position;
                    listAdressClient.Add(addressClient);
                    chazki.addressClient = listAdressClient;

                    list_chazki.Add(chazki);

                    string jsonChazki = JsonConvert.SerializeObject(list_chazki);
                    Response_Registro rpta = new Response_Registro();
                    using (var http = new HttpClient())
                    {
                        http.DefaultRequestHeaders.Add("chazki-api-key", cvCzk.informacionTiendaEnvio.chaski_api_key); //PRODUCCION
                        //http.DefaultRequestHeaders.Add("chazki-api-key", "KfXfqgEBhfMK4T8Luw8ba91RynMtjzTY"); //TEST

                        HttpContent content = new StringContent(jsonChazki);
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        var request = http.PostAsync("https://integracion.chazki.com:8443/chazkiServices/delivery/create/deliveryService", content); //PRODUCCION
                        //var request = http.PostAsync("https://sandboxintegracion.chazki.com:8443/chazkiServices/delivery/create/deliveryService", content); //TEST

                        var response = request.Result.Content.ReadAsStringAsync().Result;
                        rpta = JsonConvert.DeserializeObject<Response_Registro>(response);

                        if (rpta.descriptionResponse == "SUCCESS")
                        {
                            retorno = rpta.codeDelivery;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                retorno = "";
            }
            return retorno;
        }

        private Ecommerce_Chazki selectVenta_Chazki(string ven_id)
        {
            Ecommerce_Chazki ventas = new Ecommerce_Chazki();

            EnviaPedidoChazki datos = new EnviaPedidoChazki();

            //Chazki objModelo = new Chazki();

            Ent_Ecommerce_Chazki ent_ventas = datos.get_Ventas_por_Chazki(ven_id);

            if (ent_ventas != null)
            {
                Ecommerce_Chazki _cnvta = new Ecommerce_Chazki();

                List<DetallesCanalVenta> list_cnvtaD = new List<DetallesCanalVenta>();

                foreach (Ent_DetallesVentaCanal_E item in ent_ventas.detalles2)
                {
                    DetallesCanalVenta _cnvtaD = new DetallesCanalVenta();

                    _cnvtaD.cantidad = Convert.ToInt32(item.cantidad);
                    _cnvtaD.codigoProducto = item.codigoProducto;
                    _cnvtaD.descuento = item.descuento;
                    _cnvtaD.precioUnitario = item.precioUnitario;
                    _cnvtaD.total = item.total;
                    _cnvtaD.talla = item.talla;
                    _cnvtaD.nombreProducto = item.nombreProducto;
                    _cnvtaD.fd_colo = item.fd_colo;
                    list_cnvtaD.Add(_cnvtaD);
                }
                _cnvta.detalles = list_cnvtaD;

                Informacion_Tienda_envio _ic = null;
                if (ent_ventas.informacionTiendaEnvio != null)
                {
                    _ic = new Informacion_Tienda_envio();
                    _ic.chaski_storeId = ent_ventas.informacionTiendaEnvio.chaski_storeId;
                    _ic.chaski_branchId = ent_ventas.informacionTiendaEnvio.chaski_branchId;
                    _ic.chaski_api_key = ent_ventas.informacionTiendaEnvio.chaski_api_key;
                    _ic.nro_documento = ent_ventas.informacionTiendaEnvio.deliveryTrack_Code;
                    _ic.mode = ent_ventas.informacionTiendaEnvio.mode;
                    _ic.tiempo = ent_ventas.informacionTiendaEnvio.tiempo;
                    _ic.payment_method = ent_ventas.informacionTiendaEnvio.payment_method;
                    _ic.proofPayment = ent_ventas.informacionTiendaEnvio.proofPayment;


                }
                _cnvta.informacionTiendaEnvio = _ic;
                ventas = _cnvta;

                Informacion_Tienda_Destinatario _id = null;
                if (ent_ventas.informacionTiendaDestinatario != null)
                {
                    _id = new Informacion_Tienda_Destinatario();
                    _id.id = ent_ventas.informacionTiendaDestinatario.id;
                    _id.nroDocumento = ent_ventas.informacionTiendaDestinatario.nroDocumento;
                    _id.email = ent_ventas.informacionTiendaDestinatario.email;
                    _id.referencia = ent_ventas.informacionTiendaDestinatario.referencia;
                    _id.telefono = ent_ventas.informacionTiendaDestinatario.telefono;
                    _id.direccion_entrega = ent_ventas.informacionTiendaDestinatario.direccion_entrega;
                    _id.cod_entid = ent_ventas.informacionTiendaDestinatario.cod_entid;
                    _id.ubigeo = ent_ventas.informacionTiendaDestinatario.ubigeo;
                    _id.cliente = ent_ventas.informacionTiendaDestinatario.cliente;
                }
                _cnvta.informacionTiendaDestinatario = _id;
            }
            return ventas;
        }

        public ActionResult ActualizarRechazado(string descripcion, string id, string cod_entid, string fc_nint, string vendedor)
        {
            /*Ent_Usuario user = null;
            user = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string _usu_id = user.usu_id.ToString();
            HistorialEstadosCV _historial = new HistorialEstadosCV();
            _historial.cod_usuario = _usu_id;
            _historial.cod_entid = cod_entid;
            _historial.fc_nint = fc_nint;
            _historial.descripcion = descripcion;
            _historial.id_estado = "003";
            _historial.cod_vendedor = vendedor;
            insertar_historial_estados_cv(_historial);*/
            return RedirectToAction("Ver", "ECommerce", new { id = id, fc_nint = fc_nint, cod_entid = cod_entid });
        }

        /*public void insertar_historial_estados_cv(HistorialEstadosCV historial)
        {
            Ent_HistorialEstadosCV _historial = new Ent_HistorialEstadosCV();
            _historial.cod_entid = historial.cod_entid;
            _historial.cod_usuario = historial.cod_usuario;
            _historial.fc_nint = historial.fc_nint;
            _historial.descripcion = historial.descripcion;
            _historial.id_estado = historial.id_estado;
            _historial.cod_vendedor = historial.cod_vendedor;
            datos.insertar_historial_estados_cv(_historial);
        }*/

        private bool _existe_en_array(string[] array, string match)
        {
            bool b = false;
            foreach (var item in array)
            {
                if (!b)
                    b = item == match;
            }
            return b;
        }
        private List<SelectListItem> SelectDestino(string tiendaOrigen, string value = null)
        {

            DataTable dt = datos.get_tiendas_destino(tiendaOrigen);
            List<SelectListItem> list = new List<SelectListItem>();
            if (dt.Rows.Count > 0)
            {
                list.Add(new SelectListItem() { Text = "TODOS", Value = "" });
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new SelectListItem()
                    {
                        Text = row["des_entid"].ToString(),
                        Value = row["cod_entid"].ToString(),
                        Selected = row["cod_entid"].ToString() == value
                    });
                }
            }
            else
            {
                list.Add(new SelectListItem() { Text = "Sin resultados", Value = "" });
            }
            return list;

        }
        private List<SelectListItem> SelectOrigen(string value = null)
        {

            DataTable dt = datos.get_tienda_origenes();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "TODOS", Value = "" });
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row["des_entid"].ToString(),
                    Value = row["cod_entid"].ToString(),
                    Selected = row["cod_entid"].ToString() == value

                });
            }
            return list;

        }
        private List<SelectListItem> SelectVendedor(string cod_tda, string value = null)
        {

            DataTable dt = datos.get_vendedores_tda(cod_tda);
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "SELECCIONE", Value = "" });
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row["v_codi"].ToString() + " - " + row["v_nomb"].ToString(),
                    Value = row["v_codi"].ToString(),
                    Selected = row["v_codi"].ToString() == value

                });
            }
            return list;
        }
        private List<ECommerce> selectVentas(DateTime fdesde, DateTime fhasta, string noDocCli, string noDoc)
        {
            List<ECommerce> ventas = new List<ECommerce>();
            string _tienda = (Session["Tienda"] == null) ? "" : Session["Tienda"].ToString();


            List<Ent_ECommerce> ent_ventas = datos.get_Ventas(fdesde, fhasta, noDocCli, noDoc, _tienda);
            if (ent_ventas != null)
            {
                foreach (var item in ent_ventas)
                {
                    ECommerce _cnvta = new ECommerce();
                    _cnvta.idPedido = item.idPedido;
                    _cnvta.Referencia = item.RefPedido;
                    _cnvta.fechaPedido = item.fechaPedido;
                    _cnvta.tipoComprobante = item.tipoComprobante;
                    _cnvta.SerieDoc = item.SerieDoc;
                    _cnvta.NroDoc = item.NroDoc;
                    _cnvta.codSeguimiento = item.CodSeguimiento;
                    _cnvta.nom_courier = item.nom_courier;
                    _cnvta.estado = item.estado;
                    _cnvta.cliente = item.cliente;
                    _cnvta.direccionA = item.direccionA;
                    _cnvta.direccionB = item.direccionB;
                    _cnvta.direccionCliente = item.direccionCliente;
                    _cnvta.referenciaCliente = item.referenciaCliente;
                    _cnvta.TpDocCli = item.TpDocCli;
                    _cnvta.noDocCli = item.noDocCli;
                    _cnvta.nombreCliente = item.nombreCliente;
                    _cnvta.apePatCliente = item.apePatCliente;
                    _cnvta.apeMatCliente = item.apeMatCliente;
                    _cnvta.nombreCompletoCliente = item.nombreCompletoCliente;
                    _cnvta.cod_entid = item.cod_entid;
                    _cnvta.nombreEstado = item.nombreEstado;
                    ventas.Add(_cnvta);
                }
            }
            return ventas;
        }
        private ECommerce selectVenta(string noDoc, string cod_entid)
        {
            ECommerce ventas = new ECommerce();
            /*Ent_ECommerce ent_ventas = datos.get_Ventas_por_sn(noDoc , cod_entid);
            if (ent_ventas != null)
            {
                ECommerce _cnvta = new ECommerce();
                    _cnvta.cliente = ent_ventas.cliente;
                    _cnvta.estado = ent_ventas.estado;
                    _cnvta.tipo = ent_ventas.tipo;
                    _cnvta.serieNumero = ent_ventas.serieNumero;
                    _cnvta.tiendaDestino = ent_ventas.tiendaDestino;
                    _cnvta.tiendaOrigen = ent_ventas.tiendaOrigen;
                    _cnvta.fechaVenta = ent_ventas.fechaVenta;
                _cnvta.direccionA = ent_ventas.direccionA;
                _cnvta.direccionB = ent_ventas.direccionB;
                _cnvta.direccionCliente = ent_ventas.direccionCliente;
                _cnvta.referenciaCliente = ent_ventas.referenciaCliente;
                _cnvta.hora = ent_ventas.hora;
                _cnvta.noDocCli = ent_ventas.noDocCli;
                _cnvta.nombreCliente = ent_ventas.nombreCliente;
                _cnvta.apePatCliente = ent_ventas.apePatCliente;
                _cnvta.apeMatCliente = ent_ventas.apeMatCliente;
                _cnvta.nombreCompletoCliente = ent_ventas.nombreCompletoCliente;
                _cnvta.tipoComprobante = ent_ventas.tipoComprobante;
                _cnvta.fc_nint = ent_ventas.fc_nint;
                _cnvta.idVendedor = ent_ventas.idVendedor;
                _cnvta.nomVendedor = ent_ventas.nomVendedor;
                _cnvta.nombreEstado = ent_ventas.nombreEstado;
                _cnvta.descripcionEstado = ent_ventas.descripcionEstado;
                _cnvta.colorEstado = ent_ventas.colorEstado;
                List<DetallesECommerce> list_cnvtaD = new List<DetallesECommerce>();
                foreach (Ent_DetallesVentaCanal item in ent_ventas.detalles)
                {
                    DetallesECommerce _cnvtaD = new DetallesECommerce();
                    _cnvtaD.cantidad = item.cantidad;
                    _cnvtaD.codigoProducto = item.codigoProducto;
                    _cnvtaD.descuento = item.descuento;
                    _cnvtaD.precioUnitario = item.precioUnitario;
                    _cnvtaD.total = item.total;
                    _cnvtaD.talla = item.talla;
                    _cnvtaD.nombreProducto = item.nombreProducto;
                    list_cnvtaD.Add(_cnvtaD);
                }
                _cnvta.detalles = list_cnvtaD;
                if (ent_ventas.historialEstados != null)
                {
                    List<HistorialEstadosCV> list_hist = new List<HistorialEstadosCV>();
                    foreach (Ent_HistorialEstadosCV item in ent_ventas.historialEstados)
                    {
                        HistorialEstadosCV _cnvtaH = new HistorialEstadosCV();
                        _cnvtaH.cod_entid = item.cod_entid;
                        _cnvtaH.cod_usuario = item.cod_usuario;
                        _cnvtaH.descripcion = item.descripcion;
                        _cnvtaH.usu_nombre = item.usu_nombre;
                        _cnvtaH.fecha = item.fecha;
                        _cnvtaH.id_estado = item.id_estado;
                        _cnvtaH.nombreEstado = item.nombreEstado;
                        _cnvtaH.colorEstado = item.colorEstado;
                        _cnvtaH.cod_vendedor = item.cod_vendedor;
                        _cnvtaH.nomVendedor = item.nomVendedor;
                        list_hist.Add(_cnvtaH);
                    }
                    _cnvta.historialEstados = list_hist;
                }                               
                ventas = _cnvta;         
            }*/
            return ventas;
        }

        #region<agregando vista adinson>
        [HttpPost] //VENTA_ECOMMERCE
        public ActionResult ShowGenericReportTiendasEcommerceInNewWin(string fecIni, string FecFin, string tipo, string tda)
        {

            try
            {

                string CodTda = "";
                var ec = new Data_Ecommerce();
                HttpContext.Session["ReportName"] = "VentasEcommerce.rpt";

                if (Session["Tienda"] != null)
                {
                    CodTda = Session["Tienda"].ToString();
                }
                else
                {
                    //CodTda = "-1";
                    CodTda = tda;
                }

                ReporteVentasEcommerce ModeloRepVentaEcommerce = ec.get_ecommerce_reporteventa(CodTda, fecIni, FecFin, tipo);

                HttpContext.Session["rptSource"] = ModeloRepVentaEcommerce.ListVentaEcommerce;

                var _estado = (ModeloRepVentaEcommerce == null) ? "0" : "1";

                if (ModeloRepVentaEcommerce != null)
                {
                    if (ModeloRepVentaEcommerce.ListVentaEcommerce.Count == 0)
                    {
                        _estado = "-1";
                        //ViewBag.Tienda = ec.get_ListaTienda();
                    }

                }

                return Json(new
                {
                    estado = _estado
                });

            }
            catch (Exception)
            {

                throw;
            }


        }

        public ActionResult ReporteVentasEcommerce()
        {

            string tipo = (Request.HttpMethod == "POST" ? Request.Params["tipo"] : "1,2,3");
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            //Dat_ECommerce ec = new Dat_ECommerce();
            var ec = new Data_Ecommerce();

            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;
            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {
                ViewBag._selectTipos = SelectTipos((tipo == null ? " '',R,E" : tipo));

                if (_usuario.usu_tip_id == "05") //INVITADO (TIENDAS)
                {
                    ViewBag.Tienda = ec.get_ListaTienda(_usuario.usu_login, 0);
                }
                else
                {
                    ViewBag.Tienda = ec.get_ListaTienda("", 1);
                }

                ViewBag.usu_tipo = _usuario.usu_tip_id;

            }
            return View();
        }

        private List<SelectListItem> SelectTipos(string value = null)
        {
            string[] _values = value.Split(',');
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "TODOS", Value = "" });
            list.Add(new SelectListItem() { Text = "RECOJO EN TIENDA", Value = "R" });
            list.Add(new SelectListItem() { Text = "ENVÍO A DOMICILIO", Value = "E" });

            //list.Add(new SelectListItem() { Text = "TODOS", Value = "", Selected = _existe_en_array(_values, "") });
            //list.Add(new SelectListItem() { Text = "Recojo en tienda", Value = "R", Selected = _existe_en_array(_values, "R") });
            //list.Add(new SelectListItem() { Text = "Envío a Domicilio", Value = "E", Selected = _existe_en_array(_values, "E") });
            return list;
        }

        //consulta stock almacen 15/05/2020

        public ActionResult StockAlmacen()
        {
            var ec = new Data_Ecommerce();
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;

            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {
                //ViewBag.Tienda = dat_lista_tienda.get_tienda("PE", "1");
                Session["Lista_stock_almacen"] = null;

                ViewBag.almacen = ec.get_ListaAlmacen_Apoyo();


                return View();
            }
        }


        public PartialViewResult ConsultaStockEcom(string codAlmacen, string numArticulo, string desArticulo, string talArticulo)
        {
            return PartialView(get_Lista_Stock_Almacen(codAlmacen, numArticulo, desArticulo, talArticulo));
        }

        public List<Ent_Stock_Almacen> get_Lista_Stock_Almacen(string codAlmacen, string numArticulo, string desArticulo, string talArticulo)
        {
            List<Ent_Stock_Almacen> lista = datos.get_Lista_Stock_Almacen(codAlmacen, numArticulo, desArticulo, talArticulo);
            Session["Lista_stock_almacen"] = lista;
            return lista;
        }

        public ActionResult ConsultaTablaStock(CapaEntidad.General.Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session["Lista_stock_almacen"] == null)
            {
                List<Ent_Stock_Almacen> listdoc = new List<Ent_Stock_Almacen>();
                Session["Lista_stock_almacen"] = listdoc;

            }

            //Traer registros
            IQueryable<Ent_Stock_Almacen> membercol = ((List<Ent_Stock_Almacen>)(Session["Lista_stock_almacen"])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Stock_Almacen> filteredMembers = membercol;

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.des_almacen,
                             a.cod_articulo,
                             a.descripcion,
                             a.talla,
                             a.stock


                         };

            //Se devuelven los resultados por json
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult StockArticulosAlmacen()
        {
            if (Session["Lista_stock_almacen"] == null)
            {
                List<Ent_Stock_Almacen> liststoreConf = new List<Ent_Stock_Almacen>();
                Session["Lista_stock_almacen"] = liststoreConf;
            }
            List<Ent_Stock_Almacen> lista = (List<Ent_Stock_Almacen>)Session["Lista_stock_almacen"];
            string[] columns = { "ALMACEN", "NUM_ARTICULO", "ARTICULO", "TALLA", "STOCK" };
            //string[] headers = { "ALMACEN", "NUM_ARTICULO", "ARTICULO", "TALLA", "STOCK"};
            byte[] filecontent = ExcelExportHelper.ExportExcelStock_Ecom1(lista, "LISTA GENERAL DE STOCK DE ARTICULOS POR ALMACEN", false, columns);
            //byte[] filecontent = ExcelExportHelper.CreateCSVFile(lista, "LISTA GENERAL DE STOCK DE ARTICULOS POR ALMACEN");
            return File(filecontent, ExcelExportHelper.ExcelContentType, "StockArticulosAlmacen.xlsx");
        }


        //CONSULTA PEDIDOS PRESTASHOP

        public ActionResult Prestashop()
        {
            var ec = new Data_Ecommerce();
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;

            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {
                //ViewBag.Tienda = dat_lista_tienda.get_tienda("PE", "1");
                Session["Lista_Pedidos_Prestashop"] = null;


                return View();
            }
        }

        public PartialViewResult _TablePrestashop(DateTime fecini, DateTime fecfin)
        {
            return PartialView(get_Lista_Prestashop(fecini, fecfin));
        }

        public List<Ent_Prestashop> get_Lista_Prestashop(DateTime fecini, DateTime fecfin)
        {
            List<Ent_Prestashop> lista = datos.get_Lista_Pedidos_Prestashop(fecini, fecfin);
            Session["Lista_Pedidos_Prestashop"] = lista;
            return lista;
        }

        public ActionResult ConsultaTabla_Prestashop(CapaEntidad.General.Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session["Lista_Pedidos_Prestashop"] == null)
            {
                List<Ent_Prestashop> listdoc = new List<Ent_Prestashop>();
                Session["Lista_Pedidos_Prestashop"] = listdoc;

            }

            //Traer registros
            IQueryable<Ent_Prestashop> membercol = ((List<Ent_Prestashop>)(Session["Lista_Pedidos_Prestashop"])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Prestashop> filteredMembers = membercol;

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.Id_Orden,
                             a.Fec_Pedido,
                             a.Est_Sis_Fact,
                             a.Presta_Estado,
                             a.Presta_Estado_Name,
                             a.Presta_Est_Ped_Tienda,
                             a.Presta_FecIng,
                             a.Fecha_Facturacion,
                             a.Comprobante,
                             a.Name_Carrier,
                             a.Almacen,
                             a.Ubigeo_Ent,
                             a.Ubicacion,
                             a.Semana,
                             a.ArticuloId,
                             a.Talla,
                             a.Cantidad,
                             a.Precio_Vta,
                             a.Precio_Original,
                             a.Cod_Linea3,
                             a.Des_Linea3,
                             a.Cod_Cate3,
                             a.Des_Cate3,
                             a.Cod_Subc3,
                             a.Des_Subc3,
                             a.Cod_Marc3,
                             a.Des_Marca,
                             a.Precio_Planilla,
                             a.Costo,
                             a.Alm_C,
                             a.Alm_5,
                             a.Alm_B,
                             a.Alm_W,
                             a.Alm_1
                         };

            //Se devuelven los resultados por json
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult DetallePrestashop()
        {
            if (Session["Lista_Pedidos_Prestashop"] == null)
            {
                List<Ent_Prestashop> liststoreConf = new List<Ent_Prestashop>();
                Session["Lista_Pedidos_Prestashop"] = liststoreConf;
            }
            List<Ent_Prestashop> lista = (List<Ent_Prestashop>)Session["Lista_Pedidos_Prestashop"];
            string[] columns = { "ID_ORDER", "FECHA_PED", "ESTADO_SIST_FACT", "PRESTA_ESTADO", "PRESTA_ESTADO_NAME", "PRESTA_FECING", "FECHA_FACTURACION", "COMPROBANTE", "NAME_CARRIER", "ALMACEN", "UBIGEO", "UBICACION", "SEMANA", "ARTICULO_ID", "TALLA", "CANTIDAD", "PRECIO_VTA", "PRECIO_ORIGINAL", "COD_LINE3", "DES_LINE3", "COD_CATE3", "DES_CATE3", "COD_SUBC3", "DES_SUBC3", "COD_MARC3", "DES_MARCA", "PRECIO_PLANILLA", "COSTO", "C", "5", "B", "W", "1" };
            //string[] headers = { "ALMACEN", "NUM_ARTICULO", "ARTICULO", "TALLA", "STOCK"};
            byte[] filecontent = ExcelExportHelper.ExportExcel_Prestashop(lista, "INFORME GENERAL DE PEDIDOS - PRESTASHOP", false, columns);
            //byte[] filecontent = ExcelExportHelper.CreateCSVFile(lista, "LISTA GENERAL DE STOCK DE ARTICULOS POR ALMACEN");
            return File(filecontent, ExcelExportHelper.ExcelContentType, "InformacionPrestashop.xlsx");
        }

        //MANTENIMIENTO Y TRAZA DE PEDIDOS ECOMMERCE
        public ActionResult TrazaPedido()
        {
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];

            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;

            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {
                return View();
            }
        }

        public PartialViewResult ListaTraza(string fecini, string fecfinc)
        {

            if (fecini != null && fecfinc != null)
            {
                Session["fecini"] = fecini;
                Session["fecfinc"] = fecfinc;

            }
            return PartialView(lista(Convert.ToDateTime(Session["fecini"]), Convert.ToDateTime(Session["fecfinc"])));
        }

        public List<Ent_TrazaPedido> lista(DateTime fechaini, DateTime fechafin)
        {

            List<Ent_TrazaPedido> listTraza = datos.get_lista(fechaini, fechafin);
            listTraza = datos.get_lista(fechaini, fechafin);
            Session[_session_listTraza_private] = listTraza;

            return listTraza;
        }

        public ActionResult getListaTraza(Ent_jQueryDataTableParams param, string ID_PEDIDO)
        {
            /*verificar si esta null*/
            if (Session[_session_listTraza_private] == null)
            {
                List<Ent_TrazaPedido> listTraza = new List<Ent_TrazaPedido>();
                Session[_session_listTraza_private] = listTraza;
            }

            if (ID_PEDIDO != null)
            {
                List<Ent_TrazaPedido> listPedido = (List<Ent_TrazaPedido>)Session[_session_listTraza_private];
                listPedido.Where(w => w.ID_PEDIDO == ID_PEDIDO).Select(a => { a.VALOR = !a.VALOR; return a; }).ToList();
                Session[_session_listTraza_private] = listPedido;

            }

            //Traer registros
            IQueryable<Ent_TrazaPedido> membercol = ((List<Ent_TrazaPedido>)(Session[_session_listTraza_private])).AsQueryable();  //lista().AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<Ent_TrazaPedido> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.ID_PEDIDO.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.CLIENTE.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.ESTADO.ToUpper().Contains(param.sSearch.ToUpper()));

            }
            //Manejador de orden
            //var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            //Func<Ent_TrazaPedido, string> orderingFunction =
            //(
            //m => sortIdx == 0 ? m.cod_tienda :
            //sortIdx == 1 ? m.des_tienda :
            //sortIdx == 2 ? m.semana :
            //sortIdx == 3 ? m.dni :
            //m.estado
            //);
            //var sortDirection = Request["sSortDir_0"];
            //if (sortDirection == "asc")
            //    filteredMembers = filteredMembers.OrderBy(orderingFunction);
            //else
            //    filteredMembers = filteredMembers.OrderByDescending(orderingFunction);
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);
            var result = from a in displayMembers
                         select new
                         {
                             a.ID_PEDIDO,
                             a.CLIENTE,
                             a.IMPORTE_PEDIDO,
                             a.DESPACHO,
                             a.TIPO_ENTREGA,
                             a.FECHA_PEDIDO,
                             a.FECHA_ING_FACTURACION,
                             a.FECHA_REG_VENTA,
                             a.FECHA_REG_COURIER,
                             //a.TRAZABILIDAD,
                             a.ESTADO,
                             a.COLOR,
                             a.VALOR

                         };
            //Se devuelven los resultados por json
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult getListaTraza_filtro(Ent_jQueryDataTableParams param, string ID_PEDIDO)
        {
            string _session_listTraza_private_filtro = "_session_listTraza_private_filtro";

            /*verificar si esta null*/
            if (Session[_session_listTraza_private] == null)
            {
                List<Ent_TrazaPedido> listTraza_filtro = new List<Ent_TrazaPedido>();
                Session[_session_listTraza_private] = listTraza_filtro;
            }

            if (ID_PEDIDO == null)
            {
                List<Ent_TrazaPedido> listPedido = (List<Ent_TrazaPedido>)Session[_session_listTraza_private];
                listPedido.Where(w => w.ID_PEDIDO == ID_PEDIDO).Select(a => { a.VALOR = !a.VALOR; return a; }).ToList();
                Session[_session_listTraza_private] = listPedido;

                //filtro
                List<Ent_TrazaPedido> listPedido_filtro = (List<Ent_TrazaPedido>)Session[_session_listTraza_private];

                Session[_session_listTraza_private_filtro] = listPedido_filtro;

                var newlist = listPedido_filtro.Where(sublista => sublista.VALOR == true).ToList();
                Session[_session_listTraza_private_filtro] = newlist;


            }

            //Traer registros
            IQueryable<Ent_TrazaPedido> membercol = ((List<Ent_TrazaPedido>)(Session[_session_listTraza_private_filtro])).AsQueryable();  //lista().AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<Ent_TrazaPedido> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.ID_PEDIDO.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     //m.CLIENTE.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.ESTADO.ToUpper().Contains(param.sSearch.ToUpper()));

            }
            //Manejador de orden
            //var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            //Func<Ent_TrazaPedido, string> orderingFunction =
            //(
            //m => sortIdx == 0 ? m.cod_tienda :
            //sortIdx == 1 ? m.des_tienda :
            //sortIdx == 2 ? m.semana :
            //sortIdx == 3 ? m.dni :
            //m.estado
            //);
            //var sortDirection = Request["sSortDir_0"];
            //if (sortDirection == "asc")
            //    filteredMembers = filteredMembers.OrderBy(orderingFunction);
            //else
            //    filteredMembers = filteredMembers.OrderByDescending(orderingFunction);
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);
            var result = from a in displayMembers
                         select new
                         {
                             a.ID_PEDIDO,
                             //a.FECHA_PEDIDO,
                             //a.DESPACHO,
                             //a.FECHA_ING_FACTURACION,
                             //a.FECHA_REG_VENTA,
                             //a.CLIENTE,
                             a.ESTADO,
                             a.COLOR,
                             a.VALOR

                         };
            //Se devuelven los resultados por json
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileContentResult ExportToExcel()
        {
            List<Ent_TrazaPedido> listTrazaPedido = (List<Ent_TrazaPedido>)Session[_session_listTraza_private];

            //List<Technology> technologies = StaticData.Technologies;
            string[] columns = { "ID_PEDIDO", "CLIENTE", "IMPORTE_PEDIDO", "DESPACHO", "TIPO_ENTREGA", "FECHA_PEDIDO", "FECHA_ING_FACTURACION", "FECHA_REG_VENTA", "FECHA_REG_COURIER", "ESTADO" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(listTrazaPedido, "Trazabilidad de Pedidos", true, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "TrazaPedidos.xlsx");
        }
        //[HttpPost]
        public ActionResult AgregarPedido(string IdPedido_)
        {
            List<Ent_TrazaPedido> listPedido = (List<Ent_TrazaPedido>)Session[_session_listTraza_private];

            listPedido.Where(w => w.ID_PEDIDO == IdPedido_).Select(a => { a.VALOR = !a.VALOR; return a; }).ToList();
            Session[_session_listTraza_private] = listPedido;

            return Json(new { estado = 1 });

        }

        //[HttpPost]
        public ActionResult AnularPedido(int FlagWMS)
        {
            var oJRespuesta = new JsonResponse();
            bool estado = false;
            List<Ent_TrazaPedido> listPedido = (List<Ent_TrazaPedido>)Session[_session_listTraza_private];
            //Dat_ECommerce datos = new Dat_ECommerce();

            var newlist = listPedido.Where(sublista => sublista.VALOR == true).ToList();
            int flagcorreo = 0;

            for (int i = 0; i < newlist.Count; i++)
            {
                if (i == newlist.Count - 1)
                {
                    flagcorreo = 1;
                }
                estado = datos.update_pedido_ecommerce(newlist[i].ID_PEDIDO, "A", FlagWMS, flagcorreo, 0);
            }
            //if (estado == true)
            //{
            //    oJRespuesta.Success = estado;
            //}
            //return Json(oJRespuesta, JsonRequestBehavior.AllowGet);
            return Json(new { estado = estado });
        }
        //[HttpPost]
        public ActionResult ActualizarPedido(int FlagWMS)
        {
            var oJRespuesta = new JsonResponse();
            bool estado = false;
            List<Ent_TrazaPedido> listPedido = (List<Ent_TrazaPedido>)Session[_session_listTraza_private];
            //Dat_ECommerce datos = new Dat_ECommerce();

            var newlist = listPedido.Where(sublista => sublista.VALOR == true).ToList();
            int flagcorreo = 0;

            for (int i = 0; i < newlist.Count; i++)
            {
                if (i == newlist.Count - 1)
                {
                    if (newlist[i].FLG_REASIGNA == 0)
                    {
                        flagcorreo = 1;
                    }
                    else
                    {
                        FlagWMS = 0;

                    }

                }
                estado = datos.update_pedido_ecommerce(newlist[i].ID_PEDIDO, "E", FlagWMS, flagcorreo, newlist[i].FLG_REASIGNA);
            }
            //if (estado == true)
            //{
            //    oJRespuesta.Success = estado;
            //}
            return Json(new { estado = estado, FlagWMS = FlagWMS });

        }
        #endregion
    }
}