using CapaDato.Maestros;
using CapaDato.Transac;
using CapaEntidad.General;
using CapaEntidad.Transac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using CapaEntidad.Util;
using CapaEntidad.Control;
using CapaDato.Reporte;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

using CapaDato.CanalVenta;
using CapaDato.ChatShop;
using CapaEntidad.ChatShop;
using CapaPresentacion.Models.ChatShop;
using System.Data;
using CapaEntidad.ValeCompra;



namespace CapaPresentacion.Controllers
{
    public class ConsultaController : Controller
    {
        private Dat_Concepto_Suna dat_concepto_suna = new Dat_Concepto_Suna();
        private Dat_Documentos_Tda dat_doc = new Dat_Documentos_Tda();
        private string _session_listdocumentoDetalle_private = "_session_listdocumentoDetalle_private";

        private string _session_listguia_private = "_session_listguia_private";
        //tienda
        private Dat_Combo tienda = new Dat_Combo();
        // private Dat_Combo datCbo = new Dat_Combo();

        private Dat_Consultar_Guia datGuia = new Dat_Consultar_Guia();
        // GET: Consulta
        public ActionResult ConDocTienda()
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
                if (Session["Tienda"] != null)
                {
                    //VLADIMIR
                    ViewBag.Tienda = tienda.get_ListaTiendaXstore(Session["PAIS"].ToString()).Where(a => a.cbo_codigo == Session["Tienda"].ToString());
                    //VLADIMIR END
                    ViewBag.concepto_suna = dat_concepto_suna.lista_concepto_suna().Where(d => d.con_sun_id != "07" && d.con_sun_id != "-1");
                }
                else
                {//VLADIMIR
                    ViewBag.Tienda = tienda.get_ListaTiendaXstore(Session["PAIS"].ToString(),true);
                    //VLADIMIR END
                    ViewBag.concepto_suna = dat_concepto_suna.lista_concepto_suna().Where(w => w.con_sun_id != "-1");
                }
                return View();
            }
        }
        public PartialViewResult ListaDocumento(string dwtipodoc, string numdoc, string fecini, string fecfinc, string dwtda, string txtArticulo)
        {
            if (fecini == "" || fecfinc == "")
            {
                TempData["Error"] = "Ingrese un rango de fechas por favor.";
                return PartialView();
            }
            else
            {
                return PartialView(lista(dwtipodoc, numdoc, fecini, fecfinc, dwtda, txtArticulo));
            }

        }

        public List<Ent_Documentos_Tda> lista(string tipo_doc, string num_doc, string fec_ini, string fec_fin, string cod_tda, string articulo)
        {
            //string gcodTda = (String)Session["Tienda"];
            string strParams = "";
            //if (gcodTda != "" && gcodTda != null)
            //{
            //    strParams = gcodTda;
            //}
            strParams = (cod_tda == "0" ? "" : cod_tda);
            List<Ent_Documentos_Tda> listdoc = dat_doc.get_lista(strParams, tipo_doc, num_doc, fec_ini, fec_fin, articulo);
            Session[_session_listdocumentoDetalle_private] = listdoc;
            return listdoc;
        }

        public ActionResult getDocumento(Ent_jQueryDataTableParams param)
        {

            /*verificar si esta null*/
            if (Session[_session_listdocumentoDetalle_private] == null)
            {
                List<Ent_Documentos_Tda> listdoc = new List<Ent_Documentos_Tda>();
                Session[_session_listdocumentoDetalle_private] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_Documentos_Tda> membercol = ((List<Ent_Documentos_Tda>)(Session[_session_listdocumentoDetalle_private])).AsQueryable();  //lista().AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<Ent_Documentos_Tda> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.tipo_doc.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.num_doc.ToUpper().Contains(param.sSearch.ToUpper()));
            }
            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Ent_Documentos_Tda, string> orderingFunction =
            (
            m => sortIdx == 0 ? m.tipo_doc :
             m.num_doc
            );
            var sortDirection = Request["sSortDir_0"];
            if (sortDirection == "asc")
                filteredMembers = filteredMembers.OrderBy(orderingFunction);
            else
                filteredMembers = filteredMembers.OrderByDescending(orderingFunction);
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);
            var result = from a in displayMembers
                         select new
                         {
                             a.tipo_doc,
                             a.num_doc,
                             a.fecha_doc,
                             a.sub_total,
                             a.igv,
                             a.total,
                             a.FE,
                             a.user_ws,
                             a.pass_ws,
                             a.ruc_ws,
                             a.tipodoc_ws,
                             a.num_doc_ws,
                             a.tcantidad
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

        [HttpPost]
        public JsonResult descargar_pdf(string user_ws, string pass_ws, string ruc_ws, string tipodoc_ws, string num_doc_ws)
        {
            string url_pdf = "";
            string consulta = "";
            if (tipodoc_ws == "9")
            {
                /*web servive backoficce*/
                FEBataBack.OnlinePortTypeClient gen_fe = new FEBataBack.OnlinePortTypeClient();
                consulta = gen_fe.OnlineRecovery(ruc_ws, user_ws, pass_ws, Convert.ToInt32(tipodoc_ws), num_doc_ws, 2);
            }
            else
            {
                /*web servive e-pos*/
                FEBataEpos.OnlinePortTypeClient gen_fe = new FEBataEpos.OnlinePortTypeClient();
                consulta = gen_fe.OnlineRecovery(ruc_ws, user_ws, pass_ws, Convert.ToInt32(tipodoc_ws), num_doc_ws, 2);
            }
            consulta = consulta.Replace("&", "amp;");
            var docpdf = XDocument.Parse(consulta);
            var resultpdf = from factura in docpdf.Descendants("Respuesta")
                            select new
                            {
                                Codigo = factura.Element("Codigo").Value,
                                Mensaje = factura.Element("Mensaje").Value.Replace("amp;", "&"),
                            };
            foreach (var itempdf in resultpdf)
            {
                url_pdf = itempdf.Mensaje;
            }

            JsonResult result = new JsonResult();
            result.Data = url_pdf;
            return result;
            //return  url_pdf;
        }

        public ActionResult Guia()
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
                Session[_session_listguia_private] = null;
                if (Session["Tienda"] != null)
                {//VLADIMIR
                    ViewBag.Tienda = tienda.get_ListaTiendaXstore(Session["PAIS"].ToString()).Where(t => t.cbo_codigo == Session["Tienda"].ToString()).ToList();
                }
                else
                {
                    if (Session["PAIS"].ToString() == "PE")
                    {
                        ViewBag.Tienda = tienda.get_ListaTiendaXstore(Session["PAIS"].ToString(),true);
                    }
                    else
                    {
                        ViewBag.Tienda = tienda.get_ListaTiendaXstore_EC(Session["PAIS"].ToString());
                    }


                }

                return View();
            }
        }
        public PartialViewResult _guiaTable(string dwtda, string numguia)
        {
            return PartialView(listaGuia(dwtda, numguia));
        }


        public string listarStr_DatosGuia(string tda_destino, string num_guia)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(datGuia.get_lista(tda_destino, num_guia, Session["PAIS"].ToString()));

            return jsonData;
        }

        public List<Ent_Consultar_Guia> listaGuia(string tda_destino, string num_guia)
        {
            List<Ent_Consultar_Guia> listguia = datGuia.get_lista(tda_destino, num_guia, Session["PAIS"].ToString());
            Session[_session_listguia_private] = listguia;
            return listguia;
        }


        public ActionResult getGuiaAjax(Ent_jQueryDataTableParams param)
        {

            /*verificar si esta null*/
            if (Session[_session_listguia_private] == null)
            {
                List<Ent_Consultar_Guia> listdoc = new List<Ent_Consultar_Guia>();
                Session[_session_listguia_private] = listdoc;
            }

            //Traer registros
            // string tda_destino = Request["tda_destino"];
            // string num_guia = Request["num_guia"];

            //  List<Ent_Consultar_Guia> mGuia = datGuia.get_lista(tda_destino, num_guia);

            //  IQueryable<Ent_Consultar_Guia> membercol = ((List<Ent_Consultar_Guia>)(mGuia)).AsQueryable();  //lista().AsQueryable();
            IQueryable<Ent_Consultar_Guia> membercol = ((List<Ent_Consultar_Guia>)(Session[_session_listguia_private])).AsQueryable();  //lista().AsQueryable();


            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Consultar_Guia> filteredMembers = membercol;

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.desc_almac,
                             a.num_gudis,
                             a.num_guia,
                             a.tienda_origen,
                             a.tienda_destino,
                             a.fecha_des,
                             a.desc_send_tda,
                             a.fec_env,
                             a.mc_id,
                             a.fec_recep,
                             a.cant_despd,
                             a.cant_fmd,
                             a.con_id,
                             a.con_des
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

        //ajax (envio de guia)
        public ActionResult SendGuide(string desc_almac, string num_gudis)
        {
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            Int32 respuesta = 0;
            respuesta = datGuia.send_guide(desc_almac, num_gudis, _usuario.usu_id);

            //var oJRespuesta = new JsonResponse();
            var oJRespuesta = new CapaEntidad.ValeCompra.JsonResponse();

            if (respuesta == 1)
            {
                oJRespuesta.Message = (respuesta).ToString();
                oJRespuesta.Data = true;
                oJRespuesta.Success = true;
            }
            else
            {

                oJRespuesta.Message = (respuesta).ToString();
                oJRespuesta.Data = false;
                oJRespuesta.Success = false;
            }

            return Json(oJRespuesta, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReprocesarGuide(string desc_almac, string num_gudis, string tda_destino, string num_guia)
        {
            try
            {
                Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
                int respuesta = 0;
                string mensaje = "";
                respuesta = datGuia.reprocesarGuia(desc_almac, num_gudis, _usuario.usu_id, ref mensaje);

                //var oJRespuesta = new JsonResponse();
                var oJRespuesta = new CapaEntidad.ValeCompra.JsonResponse();

                if (respuesta == 1)
                {
                    listaGuia(tda_destino, num_guia);
                    return Json(new { estado = 1, resultados = respuesta });

                }
                else
                {
                    return Json(new { estado = 0, resultados = mensaje });
                }
            }
            catch (Exception ex)
            {
                return Json(new { estado = 0, resultados = ex.Message });
            }
        }

        /*ChatShop*/

        public ActionResult ChatShop()
        {
            string fdesde = (Request.HttpMethod == "POST" ? Request.Params["fdesde"].ToString() : DateTime.Now.ToString("dd/MM/yyyy"));
            string fhasta = (Request.HttpMethod == "POST" ? Request.Params["fhasta"].ToString() : DateTime.Now.ToString("dd/MM/yyyy"));
            string noDocCli = (Request.HttpMethod == "POST" ? Request.Params["noDocCli"].ToString() : null);
            string noDoc = (Request.HttpMethod == "POST" ? Request.Params["noDoc"].ToString() : null);
            string CodTda = "";

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
                ViewBag._fdesde = fdesde;
                ViewBag._fhasta = fhasta;
                ViewBag._noDocCli = noDocCli;
                ViewBag._noDoc = noDoc;

                List<ChatShop> listPed = new List<ChatShop>();

                if (Session["Tienda"] != null)
                {
                    CodTda = Session["Tienda"].ToString();
                }

                listPed = selectVentas(Convert.ToDateTime(fdesde), Convert.ToDateTime(fhasta), noDocCli, noDoc, CodTda);
                return View(listPed);
            }
        }

        private List<ChatShop> selectVentas(DateTime fdesde, DateTime fhasta, string noDocCli, string noDoc, string CodTda)
        {
            List<ChatShop> ventas = new List<ChatShop>();
            Dat_ChatShop datos = new Dat_ChatShop();

            string _tienda = (Session["Tienda"] == null) ? "" : Session["Tienda"].ToString();

            List<Ent_ChatShop> ent_ventas = datos.get_VentasChatShop(fdesde, fhasta, noDocCli, noDoc, CodTda);
            if (ent_ventas != null)
            {
                foreach (var item in ent_ventas)
                {
                    ChatShop chsh = new ChatShop();
                    chsh.NroDocumento = item.NroDocumento;
                    chsh.Ruc = item.Ruc;
                    chsh.Cliente = item.Cliente;
                    chsh.Tipo = item.Tipo;
                    chsh.Importe = item.Importe;
                    chsh.Fecha = item.Fecha;
                    chsh.CodSeguimiento = item.CodSeguimiento;
                    chsh.Tienda = item.Tienda;
                    chsh.CodInterno = item.CodInterno;
                    chsh.Ubigeo = item.Ubigeo;
                    chsh.Direccion = item.Direccion;
                    chsh.Referencia = item.Referencia;
                    chsh.Estado = item.Estado;
                    chsh.FlagCourier = item.FlagCourier;
                    ventas.Add(chsh);
                }
            }
            return ventas;
        }

        public ActionResult Envia_Courier(string IdTienda, string CodInterno, string NroDocumento, string Ruc, string Cliente, string Flag)
        {
            Dat_ChatShop datos = new Dat_ChatShop();
            var oJRespuesta = new JsonResponse();

            if (Flag == "NO")
            {
                datos.insertar_ge_chatshop(IdTienda, CodInterno, NroDocumento, "");
                oJRespuesta.Message = ("3").ToString();
                oJRespuesta.Data = true;
                oJRespuesta.Success = true;

            }
            else
            {
                /*delivery CHASKI*/
                ChatShop cvCzk = selectVenta(IdTienda, CodInterno);
                List<Ent_Chazki> list_chazki = new List<Ent_Chazki>();

                Dat_CanalVenta datos2 = new Dat_CanalVenta();

                string[] desUbigeo = null;
                desUbigeo = datos2.get_des_ubigeo(cvCzk.informacionTiendaDestinatario.ubigeo);
                if (desUbigeo == null)
                {
                    oJRespuesta.Message = ("2").ToString();
                    oJRespuesta.Data = false;
                    oJRespuesta.Success = false;
                    return Json(oJRespuesta, JsonRequestBehavior.AllowGet);
                }

                if (cvCzk.informacionTiendaEnvio != null)
                {
                    /* DATA CHASKI : PRODUCCION*/

                    Ent_Chazki chazki = new Ent_Chazki();
                    chazki.storeId = cvCzk.informacionTiendaEnvio.chaski_storeId; // "10411"; // proporcionado por chazki
                    chazki.branchId = cvCzk.informacionTiendaEnvio.chaski_branchId; // proporcionado por chazki
                    chazki.deliveryTrackCode = NroDocumento.Replace("-", "");
                    chazki.proofPayment = "Ninguna"; // por definir la evindencia que será entregada al cliente
                    chazki.deliveryCost = 0;
                    chazki.mode = "Regular"; //pendiente definir el modo con el que se va a trabajar el canal de venta.
                    chazki.time = "";
                    chazki.paymentMethod = "Pagado";
                    chazki.country = "PE";

                    /* DATA CHASKI : TEST*/

                    //Ent_Chazki chazki = new Ent_Chazki();
                    //chazki.storeId = "10411";
                    //chazki.branchId = "CCSC-B187";
                    //chazki.deliveryTrackCode = NroDocumento;
                    //chazki.proofPayment = "Ninguna"; // por definir la evindencia que será entregada al cliente
                    //chazki.deliveryCost = 0;
                    //chazki.mode = "Regular"; //pendiente definir el modo con el que se va a trabajar el canal de venta.
                    //chazki.time = "";
                    //chazki.paymentMethod = "Pagado";
                    //chazki.country = "PE";

                    /* DATA ARTICULO*/

                    List<Ent_ItemSold_2> listItemSold = new List<Ent_ItemSold_2>();
                    foreach (var producto in cvCzk.detalles)
                    {
                        if (producto.codigoProducto != "9999997" && producto.fd_colo == "")
                        {
                            Ent_ItemSold_2 _item = new Ent_ItemSold_2();
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
                    chazki.listItemSold = listItemSold;
                    chazki.notes = "Entregar a Cliente";
                    chazki.documentNumber = Ruc;
                    chazki.lastName = "";
                    //chazki.email = "servicio.clientes.peru@bata.com";
                    if (chazki.email == "" || chazki.email == null)
                    {
                        chazki.email = "servicio.clientes.peru @bata.com";
                    }
                    else
                    {
                        chazki.email = cvCzk.informacionTiendaDestinatario.email;
                    }

                    chazki.phone = cvCzk.informacionTiendaDestinatario.telefono;
                    int CadRuc = Ruc.Length;

                    if (CadRuc > 8)
                    {
                        chazki.documentType = "RUC";
                        chazki.companyName = Cliente;
                        chazki.name_tmp = "";
                    }
                    else
                    {
                        chazki.documentType = "DNI";
                        chazki.companyName = "";
                        chazki.name_tmp = Cliente;
                    }
                    /* DATA DIRECCION*/

                    List<Ent_AddressClient_2> listAdressClient = new List<Ent_AddressClient_2>();
                    Ent_AddressClient_2 addressClient = new Ent_AddressClient_2();
                    addressClient.nivel_2 = desUbigeo[0]; //(cvCzk.tipo == "3" ? (cvCzk.ubigeoCliente.ToString() == "" ? cvCzk.ubigeoTienda.Substring(0, 2) : cvCzk.ubigeoCliente.Substring(0, 2)) : cvCzk.ubigeoTienda.Substring(0, 2));
                    addressClient.nivel_3 = desUbigeo[1]; //(cvCzk.tipo == "3" ? (cvCzk.ubigeoCliente.ToString() == "" ? cvCzk.ubigeoTienda.Substring(2, 2) : cvCzk.ubigeoCliente.Substring(2, 2)) : cvCzk.ubigeoTienda.Substring(2, 2));
                    addressClient.nivel_4 = desUbigeo[2]; //(cvCzk.tipo == "3" ? (cvCzk.ubigeoCliente.ToString() == "" ? cvCzk.ubigeoTienda.Substring(4) : cvCzk.ubigeoCliente.Substring(4)) : cvCzk.ubigeoTienda.Substring(4));
                    addressClient.name = cvCzk.informacionTiendaDestinatario.direccion_entrega;
                    addressClient.reference = cvCzk.informacionTiendaDestinatario.referencia;
                    addressClient.alias = "No Alias";
                    Ent_Position_2 position = new Ent_Position_2();
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
                    }
                    if (rpta.response == 1)
                    {
                        oJRespuesta.Message = (rpta.response).ToString();
                        oJRespuesta.Data = true;
                        oJRespuesta.Success = true;

                        Dat_CanalVenta objCanal = new Dat_CanalVenta();
                        //datos.insertar_ge_cv(IdTienda, CodInterno, NroDocumento, rpta.codeDelivery);
                        datos.insertar_ge_chatshop(IdTienda, CodInterno, NroDocumento, rpta.codeDelivery);

                        //TempData["Success"] = "Pedido generado correctamente: " + rpta.codeDelivery;
                    }
                    else if (rpta.response == 99)
                    {
                        oJRespuesta.Message = (rpta.response).ToString();
                        oJRespuesta.Data = false;
                        oJRespuesta.Success = false;

                        //TempData["Error"] = "Error al generar pedido. Error en el servidor" + " | " + rpta.descriptionResponse + " | " + rpta.codeDelivery + " | " + "Intentelo mas tarde.";
                    }
                    else
                    {
                        oJRespuesta.Message = (rpta.response).ToString();
                        oJRespuesta.Data = false;
                        oJRespuesta.Success = false;

                        TempData["Error"] = "Error al generar pedido. " + rpta.descriptionResponse + "|" + rpta.codeDelivery;
                    }
                }
                else
                {
                    TempData["Error"] = "Error al generar guia. No existe informacion de recogo para la tienda.";
                }
            }
            return Json(oJRespuesta, JsonRequestBehavior.AllowGet);


            //return RedirectToAction("ChatShop", "Consulta");
        }


        private ChatShop selectVenta(string Tienda, string CodInterno)
        {
            ChatShop ventas = new ChatShop();

            Dat_ChatShop datos = new Dat_ChatShop();

            //Chazki objModelo = new Chazki();

            Ent_ChatShop ent_ventas = datos.get_Ventas_por_ChatShop(Tienda, CodInterno);

            if (ent_ventas != null)
            {
                ChatShop _cnvta = new ChatShop();

                List<DetallesCanalVenta> list_cnvtaD = new List<DetallesCanalVenta>();

                foreach (Ent_DetallesVentaCanal_2 item in ent_ventas.detalles2)
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
                    _ic.id = ent_ventas.informacionTiendaEnvio.id;
                    _ic.cod_entid = ent_ventas.informacionTiendaEnvio.cod_entid;
                    _ic.courier = ent_ventas.informacionTiendaEnvio.courier;
                    _ic.cx_codTipoDocProveedor = ent_ventas.informacionTiendaEnvio.cx_codTipoDocProveedor;
                    _ic.cx_nroDocProveedor = ent_ventas.informacionTiendaEnvio.cx_nroDocProveedor;
                    _ic.cx_codDireccionProveedor = ent_ventas.informacionTiendaEnvio.cx_codDireccionProveedor;
                    _ic.cx_codCliente = ent_ventas.informacionTiendaEnvio.cx_codCliente;
                    _ic.cx_codCtaCliente = ent_ventas.informacionTiendaEnvio.cx_codCtaCliente;
                    _ic.id_usuario = ent_ventas.informacionTiendaEnvio.id_usuario;
                    _ic.de_terminal = ent_ventas.informacionTiendaEnvio.de_terminal;
                    _ic.chaski_storeId = ent_ventas.informacionTiendaEnvio.chaski_storeId;
                    _ic.chaski_branchId = ent_ventas.informacionTiendaEnvio.chaski_branchId;
                    _ic.chaski_api_key = ent_ventas.informacionTiendaEnvio.chaski_api_key;
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
                }
                _cnvta.informacionTiendaDestinatario = _id;
            }
            return ventas;
        }


        [HttpPost]
        public ActionResult ImprimirCodigo(string Cliente, string Tienda, string CodInterno, string CodSeguimiento)
        {
            try
            {
                List<GuiaElectronica> _ge = new List<GuiaElectronica>();
                GuiaElectronica ge = new GuiaElectronica();

                //CanalVenta _cv = selectVenta(serie_numero, cod_entid, fc_nint);
                //ge.guia = _cv.guia_electronica;
                //ge.cliente = (_cv.tipo == "3" ? _cv.cliente : _cv.tiendaOrigen);
                //ge.direccion = (_cv.tipo == "3" ? _cv.direccionCliente : _cv.direccionA);
                //ge.referencia = (_cv.tipo == "3" ? _cv.referenciaCliente : "Sin Referencia");
                //ge.ubigeo = (_cv.tipo == "3" ? _cv.ubigeoCliente : _cv.ubigeoTienda);
                //_ge.Add(ge);


                ChatShop cvCzk = selectVenta(Tienda, CodInterno);

                ge.guia = CodSeguimiento;
                ge.cliente = Cliente;
                ge.direccion = cvCzk.informacionTiendaDestinatario.direccion_entrega;
                ge.referencia = cvCzk.informacionTiendaDestinatario.referencia;
                ge.ubigeo = cvCzk.informacionTiendaDestinatario.ubigeo;
                ge.telefono = cvCzk.informacionTiendaDestinatario.telefono;
                _ge.Add(ge);

                return Json(new { estado = 1, guia = CodSeguimiento, cliente = Cliente, direccion = ge.direccion, referencia = ge.referencia, ubigeo = ge.ubigeo, telefono = ge.telefono });
            }
            catch (Exception ex)
            {
                return Json(new { estado = 0 });
            }
        }




    }


}

