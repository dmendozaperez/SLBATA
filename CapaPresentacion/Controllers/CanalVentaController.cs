using CapaDato.CanalVenta;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaPresentacion.Models.CanalVenta;
using CapaEntidad.CanalVenta;
using CapaEntidad.Util;
using CapaEntidad.Control;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CapaEntidad.General;

namespace CapaPresentacion.Controllers
{
    public class CanalVentaController : Controller
    {
        Dat_CanalVenta datos = new Dat_CanalVenta();
        // GET: Cnvta
        public PartialViewResult ListaVentas(string fdesde, string fhasta, string noDocCli, string noDoc, string tiendaOrigen, string tiendaDestino, string[] tipo, string[] estado)
        {
            return PartialView(selectVentas(Convert.ToDateTime(fdesde), Convert.ToDateTime(fhasta), noDocCli, noDoc, tiendaOrigen, tiendaDestino, tipo, estado));
        }
        [HttpPost]
        public ActionResult ReporteView(string fdesde, string fhasta, string noDocCli, string noDoc, string tiendaOrigen, string tiendaDestino, string[] tipo, string[] estado)
        {
            List<CanalVenta> cv = selectVentas(Convert.ToDateTime(fdesde), Convert.ToDateTime(fhasta), noDocCli, noDoc, tiendaOrigen, tiendaDestino, tipo, estado);
            this.HttpContext.Session["rptSource"] = cv;
            string _estado = (cv == null) ? "0" : "1";
            return Json(new { estado = _estado });
        }
        public ActionResult Index()
        {
            Session["_cv"] = null;
            string fdesde = (Request.HttpMethod == "POST" ? Request.Params["fdesde"].ToString() : DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy"));
            string fhasta = (Request.HttpMethod == "POST" ? Request.Params["fhasta"].ToString() : DateTime.Now.ToString("dd/MM/yyyy"));
            string noDocCli = (Request.HttpMethod == "POST" ? Request.Params["noDocCli"].ToString() : null);
            string noDoc = (Request.HttpMethod == "POST" ? Request.Params["noDoc"].ToString() : null);
            string tiendaOrigen = (Request.HttpMethod == "POST" ? Request.Params["tiendaOrigen"].ToString() :null);
            string tiendaDestino = (Request.HttpMethod == "POST" ? Request.Params["tiendaDestino"].ToString() : (Session["Tienda"] != null ? Session["Tienda"].ToString() : null));
            string estado = (Request.HttpMethod == "POST" ? Request.Params["estado"] : "001,004,005,006");
            string tipo = (Request.HttpMethod == "POST" ? Request.Params["tipo"] : "1,2,3");


            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;

            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control");
            }
            else
            {
                //Session["Tienda"] = null;
                ViewBag._selectOrigen = SelectOrigen(tiendaOrigen);
                ViewBag._selectDestino = SelectDestino((tiendaOrigen == null ? "" : tiendaOrigen), (tiendaDestino == null ? "" : tiendaDestino));
                ViewBag._fdesde = fdesde;
                ViewBag._fhasta = fhasta;
                ViewBag._noDocCli = noDocCli;
                ViewBag._noDoc = noDoc;
                ViewBag._tiendaOrigen = tiendaOrigen;
                ViewBag._tiendaDestino = tiendaDestino;
                ViewBag._selectTipos = SelectTipos((tipo == null ? "1,2,3" : tipo));
                ViewBag._selectEstados = SelectEstados((estado == null ? "001,002,003,004,005,006" : estado));

                List<CanalVenta> listCV = new List<CanalVenta>();

            }
            return View();
        }
        [HttpPost]
        public JsonResult BuscarNumAleatorio()    // el método debe ser de static
        {
            Random aleatorio = new Random();
            return Json("'num':" + "'" + aleatorio.Next(0, 1000).ToString() + "'");
        }
        [HttpPost]
        public JsonResult GetDestinosJson(string tiendaOrigen)
        {
            return Json(SelectDestino(tiendaOrigen));
        }
        private List<SelectListItem> SelectTipos(string value = null)
        {
            string[] _values = value.Split(',');
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "1.RECOGO EN ESTA TIENDA", Value = "1", Selected = _existe_en_array(_values, "1") });
            list.Add(new SelectListItem() { Text = "2.RECOGO EN OTRA TIENDA", Value = "2", Selected = _existe_en_array(_values, "2") });
            list.Add(new SelectListItem() { Text = "3.DELIVERY AL CLIENTE", Value = "3", Selected = _existe_en_array(_values, "3") });
            return list;
        }
        private List<SelectListItem> SelectEstados(string value = null)
        {
            DataTable dt = datos.get_estados_cv();
            string[] _values = value.Split(',');
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow item in dt.Rows)
            {
                list.Add(new SelectListItem() {
                    Text = item["nombreEstado"].ToString(),
                    Value = item["codigoEstado"].ToString(),
                    Selected = _existe_en_array(_values, item["codigoEstado"].ToString())
                });
            }
            return list;
        }
        public ActionResult Ver(string serie_numero, string cod_entid, string fc_nint, string ge = null)
        {
            ViewBag.id = serie_numero;
            CanalVenta cv = selectVenta(serie_numero, cod_entid, fc_nint);
            ViewBag._SelectVendedor = SelectVendedor((Session["Tienda"] == null ? cv.cod_entid + "," + cv.cod_entid_b : Session["Tienda"].ToString()));           

            
            return View(cv);
        }       
        public ActionResult ActualizarEstado(string descripcion, string serie_numero, string cod_entid, string fc_nint, string estado, string vendedor)
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
                vendedor = vendedor.Trim();
                string cod_vendedor = vendedor.Substring(0, vendedor.IndexOf('-'));
                string cod_tda = vendedor.Substring(vendedor.IndexOf('-') + 1);

                if (estado == "005")
                {
                    ActualizarDeliveryDespachado(descripcion, serie_numero, cod_entid, fc_nint, cod_vendedor, cod_tda);
                }
                else
                {
                    insertar_historial_estados_cv(cod_entid, fc_nint, descripcion, estado, cod_vendedor, cod_tda, serie_numero);
                }
                return RedirectToAction("Ver", "CanalVenta", new { serie_numero = serie_numero, fc_nint = fc_nint, cod_entid = cod_entid });
            }
            
        }        
        public ActionResult ListarVentasCV(Ent_jQueryDataTableParams param)
        {
            if (Session["_cv"] == null)
            {
                List<CanalVenta> liststoreConf = new List<CanalVenta>();
                Session["_cv"] = liststoreConf;
            }
            IQueryable<CanalVenta> membercol = ((List<CanalVenta>)(Session["_cv"])).AsQueryable();  //lista().AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<CanalVenta> filteredMembers = membercol;


            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.tiendaOrigen.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.tiendaOrigen.ToUpper().Contains(param.sSearch.ToUpper()));
            }

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<CanalVenta, DateTime> orderingFunction =
            (
            m => Convert.ToDateTime(m.fechaVenta));
            var sortDirection = Request["sSortDir_0"];
            if (sortDirection == "desc")
                filteredMembers = filteredMembers.OrderBy(orderingFunction);
            else
                filteredMembers = filteredMembers.OrderByDescending(orderingFunction);

            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);


            var result = from a in displayMembers
                         select new
                         {
                             a.serieNumero,
                             a.fechaVenta,
                             a.tiendaOrigen,
                             a.tiendaDestino,
                             a.tipo,
                             a.estado,
                             a.cliente,
                             a.importeTotal,
                             a.cod_entid,
                             a.fc_nint,
                             a.nombreTipoCV,
                             a.nombreEstado,
                             a.colorEstado,
                             a.nroPares,
                             a.totalSinIgv
                         };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = membercol.Count(),
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ImprimirCodigo(string cod_entid, string fc_nint, string serie_numero)
        {
            try
            {
                List<GuiaElectronica> _ge = new List<GuiaElectronica>();
                GuiaElectronica ge = new GuiaElectronica();

                CanalVenta _cv = selectVenta(serie_numero, cod_entid, fc_nint);
                ge.guia = _cv.guia_electronica;
                ge.cliente = (_cv.tipo == "3" ? _cv.cliente : _cv.tiendaOrigen);
                ge.direccion = (_cv.tipo == "3" ? _cv.direccionCliente : _cv.direccionA);
                ge.referencia = (_cv.tipo == "3" ? _cv.referenciaCliente : "Sin Referencia");
                ge.ubigeo = (_cv.tipo == "3" ? _cv.ubigeoCliente : _cv.ubigeoTienda);
                _ge.Add(ge);

                return Json(new { estado = 1, guia = ge.guia, cliente = ge.cliente, direccion = ge.direccion, referencia = ge.referencia, ubigeo = ge.ubigeo });
            }
            catch  (Exception ex)
            {
                return Json(new { estado = 0 });
            }
        }
        public void ActualizarDeliveryDespachado(string descripcion, string serieNumero, string cod_entid, string fc_nint, string vendedor, string cod_tda)
        {

            #region DELIVERY CON COMERCIO XPRESS
            //cxpress.WSOrdenServicioClient obj1 = new cxpress.WSOrdenServicioClient();
            //cxpress.OrdenServicioReqParm objcla = new cxpress.OrdenServicioReqParm();
            //cxpress.WSOrdenServicioClient dd = new cxpress.WSOrdenServicioClient();
            ///*             
            // <codCliente>141</codCliente>
            // <codCtaCliente>142</codCtaCliente>
            // <nroDocProveedor>20145556666</nroDocProveedor>
            // <codDireccionProveedor>900055</codDireccionProveedor>
            // */
            //CanalVenta cvU = selectVenta(serieNumero, cod_entid, fc_nint);

            //if (cvU.informacionTiendaEnvio != null)
            //{
            //    if (cvU.informacionTiendaEnvio.courier == "cxpress")
            //    {
            //        objcla.nroPedido = new String[] { cvU.serieNumero };// nroPedido;
            //        List<cxpress.item> lista = new List<cxpress.item>();

            //        foreach (var item in cvU.detalles)
            //        {
            //            if (item.codigoProducto != "9999997")
            //            {
            //                cxpress.item objdet = new cxpress.item();
            //                objdet.descItem = new String[] { item.nombreProducto };
            //                objdet.cantItem = new int[] { item.cantidad };
            //                objdet.pesoMasa = new float[] { 1 };
            //                objdet.altoItem = new float[] { 1 };
            //                objdet.largoItem = new float[] { 1 };
            //                objdet.anchoItem = new float[] { 1 };
            //                objdet.valorItem = new float[] { 1 };
            //                lista.Add(objdet);
            //            }
            //        }

            //        objcla.listaItems = lista.ToArray();

            //        objcla.volumen = new double[] { 10 };           //No hay 
            //        objcla.tipoServicio = new long[] { 101 };       // 

            //        /*Codigos para prueba 141 y  142*/
            //        objcla.codCliente = new long[] { Convert.ToInt32(cvU.informacionTiendaEnvio.cx_codCliente) };         //entregado por CX
            //        objcla.codCtaCliente = new long[] { Convert.ToInt32(cvU.informacionTiendaEnvio.cx_codCtaCliente) };      //entregado por CX

            //        objcla.cantPiezas = new int[] { cvU.detalles.Sum(cant => cant.cantidad) };
            //        objcla.codRef1 = new String[] { "0012071801" }; //opcional
            //        objcla.codRef2 = new String[] { "0012071801" }; //opcional
            //        objcla.valorProducto = new double[] { 1 };
            //        objcla.tipoOrigenRecojo = new int[] { 1 };
            //        objcla.nroDocProveedor = new String[] { cvU.informacionTiendaEnvio.cx_nroDocProveedor };/*Para nroDocProveedor 20145556666*/

            //        objcla.codTipoDocProveedor = new long[] { Convert.ToInt32(cvU.informacionTiendaEnvio.cx_codTipoDocProveedor) };    //entregado por CX
            //        objcla.codDireccionProveedor = new long[] { Convert.ToInt32(cvU.informacionTiendaEnvio.cx_codDireccionProveedor) };  //entregado por CX //prueba:0900055

            //        objcla.indicadorGeneraRecojo = new int[] { 1 };
            //        objcla.tipoDestino = new int[] { 1 };
            //        objcla.direccEntrega = new String[] { (cvU.tipo == "3" ? cvU.direccionCliente : cvU.informacionTiendaDestinatario.direccion_entrega) };  // Dirección de entrega
            //        //Ubigeo dirección entrega key.ubi_direc
            //        objcla.refDireccEntrega = new String[] { (cvU.tipo == "3" ? (String.IsNullOrEmpty(cvU.referenciaCliente) ? "Sin Referencia" : cvU.referenciaCliente) : cvU.informacionTiendaDestinatario.referencia) }; //Referencia dirección entrega
            //        objcla.codDepartEntrega = new String[] { (cvU.tipo == "3" ? (cvU.ubigeoCliente.ToString() == "" ? cvU.ubigeoTienda.Substring(0, 2) : cvU.ubigeoCliente.Substring(0, 2)) : cvU.ubigeoTienda.Substring(0, 2)) }; //Departamento = Lima
            //        objcla.codProvEntrega = new String[] { (cvU.tipo == "3" ? (cvU.ubigeoCliente.ToString() == "" ? cvU.ubigeoTienda.Substring(2, 2) : cvU.ubigeoCliente.Substring(2, 2)) : cvU.ubigeoTienda.Substring(2, 2)) }; //Provincia = Lima
            //        objcla.codDistEntrega = new String[] { (cvU.tipo == "3" ? (cvU.ubigeoCliente.ToString() == "" ? cvU.ubigeoTienda.Substring(4) : cvU.ubigeoCliente.Substring(4)) : cvU.ubigeoTienda.Substring(4)) };
            //        objcla.nomDestEntrega = new String[] { (cvU.tipo == "3" ? cvU.nombreCliente : cvU.tiendaOrigen) };
            //        objcla.apellDestEntrega = new String[] { (cvU.tipo == "3" ? cvU.apePatCliente + ' ' + cvU.apeMatCliente : "BATA") };  //"Perez Luna"
            //        objcla.codTipoDocDestEntrega = new String[] { (cvU.tipo == "3" ? (cvU.noDocCli.Length == 11 ? "112" : "109") : "112") };
            //        objcla.nroDocDestEntrega = new String[] { (cvU.tipo == "3" ? cvU.noDocCli : cvU.informacionTiendaDestinatario.nroDocumento) }; //supongo que si es para tienda debe ir el ruc de bata    //"12345678"
            //        objcla.telefDestEntrega = new String[] { (cvU.tipo == "3" ? (String.IsNullOrEmpty(cvU.telefonoCliente) ? "488-8300" : cvU.telefonoCliente) : cvU.informacionTiendaDestinatario.telefono) }; // telefono del cliente         //"991276768"
            //        objcla.emailDestEntrega = new String[] { (cvU.tipo == "3" ? "servicio.clientes.peru@bata.com" : cvU.informacionTiendaDestinatario.email) };     //"juanperez@gmail.com"
            //        objcla.idUsuario = new String[] { cvU.informacionTiendaEnvio.id_usuario };
            //        objcla.deTerminal = new String[] { cvU.informacionTiendaEnvio.de_terminal };

            //        var e = obj1.registrar(objcla);

            //        if (e.nroOrdenServicio != null)
            //        {
            //            insertar_historial_estados_cv(cod_entid, fc_nint, descripcion, "005", vendedor, cod_tda, serieNumero);
            //            datos.insertar_ge_cv(cod_entid, fc_nint, serieNumero, e.nroOrdenServicio);
            //            TempData["Success"] = "Guia generada correctamente.";
            //        }
            //        else
            //        {
            //            TempData["Error"] = "Error al generar guia. " + e.msg;
            //        }
            //    }
            //}
            //else
            //{
            //    TempData["Error"] = "Error al generar guia. No existe informacion de recogo para la tienda.";
            //}
            #endregion

            #region DELIVERY CON URBANO
            //Urbano urbano = new Urbano();
            ////CanalVenta cvU = selectVenta(id, cod_entid, fc_nint);
            //urbano.linea = "3";
            //urbano.id_contrato = "7182";
            //urbano.cod_rastreo = cvU.serieNumero;
            //urbano.cod_barra = cvU.serieNumero;
            //urbano.fech_emi_vent = DateTime.Now.ToString("dd/MM/yyyy");
            //urbano.nro_o_compra = cvU.serieNumero;
            //urbano.nro_guia_trans = "";
            //urbano.nro_factura = cvU.serieNumero;

            //urbano.cod_cliente = cvU.noDocCli;
            //urbano.nom_cliente = cvU.nombreCompletoCliente;
            //urbano.nro_telf = "";
            //urbano.nro_telf_mobil = "";
            //urbano.correo_elec = "";
            //urbano.dir_entrega = cvU.direccionCliente;
            //urbano.nro_via = 0;
            //urbano.ubi_direc = "150121"; //pendiente el ubigeo del cliente
            //urbano.ref_direc = cvU.referenciaCliente;
            //urbano.peso_total = "0.3";
            //urbano.pieza_total = "1";

            //urbano.venta_seller = "SI";
            //urbano.sell_codigo = cvU.cod_entid_b;
            //urbano.sell_nombre = cvU.tiendaDestino;
            //urbano.sell_direcc = cvU.direccionB;
            //urbano.sell_ubigeo = ""; //pendiente el ubigeo de la tienda B

            //List<Productos> productos = new List<Productos>();
            //foreach (var item in cvU.detalles)
            //{
            //    Productos pro = new Productos();
            //    pro.cod_sku = item.codigoProducto;
            //    pro.descr_sku = item.nombreProducto;
            //    pro.modelo_sku = "";
            //    pro.marca_sku = "";
            //    pro.peso_sku = "0.3";
            //    pro.cantidad_sku = item.cantidad;
            //    productos.Add(pro);
            //}

            //urbano.productos = productos;
            /*
            using (var http = new HttpClient())
            {
                // Define authorization headers here, if any
                http.DefaultRequestHeaders.Add("user", "B4T412");
                http.DefaultRequestHeaders.Add("pass", "597575f74bd17ed742ae989faafe8ef26f0d6235");
                HttpContent content = new StringContent("json=" + JsonConvert.SerializeObject(urbano));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var request = http.PostAsync("https://app.urbano.com.pe/ws/ue/ge", content);
                var response = request.Result.Content.ReadAsStringAsync().Result;
                var rpta =  JsonConvert.DeserializeObject<Respuesta>(response);
                Console.Write(rpta.guia);
            }
            */
            //Respuesta rpta = new Respuesta();
            //rpta.error = 1;
            //rpta.mensaje = "OK";
            //rpta.guia = "WYB16171360";

            //if (rpta.guia.Length == 11)
            //{
            //    GuiaElectronica ge = new GuiaElectronica();
            //    ge.guia = rpta.guia;
            //    ge.cliente = cvU.nombreCompletoCliente;
            //    ge.direccion = cvU.direccionCliente;
            //    ge.referencia = cvU.referenciaCliente;
            //    ge.ubigeo = urbano.ubi_direc;
            //    List<GuiaElectronica> _ge = new List<GuiaElectronica>();
            //    _ge.Add(ge);
            //    datos.insertar_ge_cv(cod_entid, fc_nint, id, ge.guia);

            //}
            //else
            //{
            //    return null;
            //}

            #endregion

            #region DELIVERY CON CHASKI
            CanalVenta cvCzk = selectVenta(serieNumero, cod_entid, fc_nint);
            List<Ent_Chaski> list_chazki = new List<Ent_Chaski>();
            string[] desUbigeo = null;
            if (cvCzk.informacionTiendaEnvio != null)
            {
                desUbigeo = datos.get_des_ubigeo(cvCzk.tipo == "3" ? cvCzk.ubigeoCliente : cvCzk.ubigeoTienda);
                if (desUbigeo == null)
                {
                    TempData["Error"] = "Error al solicitar el pedido de envio: No se encontró ubigeo.";
                    return;
                }
                Ent_Chaski chazki = new Ent_Chaski();
                chazki.storeId = cvCzk.informacionTiendaEnvio.chaski_storeId; // "10411"; // proporcionado por chazki
                chazki.branchId = cvCzk.informacionTiendaEnvio.chaski_branchId; // proporcionado por chazki
                chazki.deliveryTrackCode = cvCzk.serieNumero;
                chazki.proofPayment = "Ninguna"; // por definir la evindencia que será entregada al cliente
                chazki.deliveryCost = 0;
                chazki.mode = "Regular"; //pendiente definir el modo con el que se va a trabajar el canal de venta.
                chazki.time = "";
                chazki.paymentMethod = "Pagado";
                chazki.country = "PE";

                List<Ent_ItemSold> listItemSold = new List<Ent_ItemSold>();
                foreach (var producto in cvCzk.detalles)
                {
                    if (producto.codigoProducto != "9999997" && producto.fd_colo == "C")
                    {
                        Ent_ItemSold _item = new Ent_ItemSold();
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
                chazki.notes = (cvCzk.tipo == "3" ? "" : "Entregar en Tienda BATA");
                chazki.documentNumber = (cvCzk.tipo == "3" ? cvCzk.noDocCli : cvCzk.informacionTiendaDestinatario.nroDocumento);
                chazki.name_tmp = (cvCzk.tipo == "3" ? cvCzk.nombreCliente : "");
                chazki.lastName = cvCzk.tipo == "3" ? cvCzk.apePatCliente + " " + cvCzk.apeMatCliente : "";
                chazki.companyName = cvCzk.tipo == "3" ? "" : cvCzk.tiendaOrigen;
                chazki.email = (cvCzk.tipo == "3" ? "servicio.clientes.peru@bata.com" : cvCzk.informacionTiendaDestinatario.email);
                chazki.phone = (cvCzk.tipo == "3" ? (String.IsNullOrEmpty(cvCzk.telefonoCliente) ? "488-8300" : cvCzk.telefonoCliente) : cvCzk.informacionTiendaDestinatario.telefono);
                chazki.documentType = (cvCzk.tipo == "3" ? (cvCzk.noDocCli.Length == 11 ? "RUC" : "DNI") : "RUC");

                List<Ent_AddressClient> listAdressClient = new List<Ent_AddressClient>();
                Ent_AddressClient addressClient = new Ent_AddressClient();
                addressClient.nivel_2 = desUbigeo[0]; //(cvCzk.tipo == "3" ? (cvCzk.ubigeoCliente.ToString() == "" ? cvCzk.ubigeoTienda.Substring(0, 2) : cvCzk.ubigeoCliente.Substring(0, 2)) : cvCzk.ubigeoTienda.Substring(0, 2));
                addressClient.nivel_3 = desUbigeo[1]; //(cvCzk.tipo == "3" ? (cvCzk.ubigeoCliente.ToString() == "" ? cvCzk.ubigeoTienda.Substring(2, 2) : cvCzk.ubigeoCliente.Substring(2, 2)) : cvCzk.ubigeoTienda.Substring(2, 2));
                addressClient.nivel_4 = desUbigeo[2]; //(cvCzk.tipo == "3" ? (cvCzk.ubigeoCliente.ToString() == "" ? cvCzk.ubigeoTienda.Substring(4) : cvCzk.ubigeoCliente.Substring(4)) : cvCzk.ubigeoTienda.Substring(4));
                addressClient.name = (cvCzk.tipo == "3" ? cvCzk.direccionCliente : cvCzk.informacionTiendaDestinatario.direccion_entrega);
                addressClient.reference = (cvCzk.tipo == "3" ? (String.IsNullOrEmpty(cvCzk.referenciaCliente) ? "Sin Referencia" : cvCzk.referenciaCliente) : cvCzk.informacionTiendaDestinatario.referencia);
                addressClient.alias = "No Alias";
                Ent_Position position = new Ent_Position();
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
                    http.DefaultRequestHeaders.Add("chazki-api-key", cvCzk.informacionTiendaEnvio.chaski_api_key);
                    HttpContent content = new StringContent(jsonChazki);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var request = http.PostAsync("https://integracion.chazki.com:8443/chazkiServices/delivery/create/deliveryService", content);
                    var response = request.Result.Content.ReadAsStringAsync().Result;
                    rpta = JsonConvert.DeserializeObject<Response_Registro>(response);
                }
                if (rpta.response == 1)
                {
                    insertar_historial_estados_cv(cod_entid, fc_nint, descripcion, "005", vendedor, cod_tda, serieNumero);
                    datos.insertar_ge_cv(cod_entid, fc_nint, serieNumero, rpta.codeDelivery);
                    TempData["Success"] = "Pedido generado correctamente: " + rpta.codeDelivery;
                }
                else if (rpta.response == 99)
                {
                    TempData["Error"] = "Error al generar pedido. Error en el servidor" + " | " + rpta.descriptionResponse + " | " + rpta.codeDelivery + " | " + "Intentelo mas tarde.";
                }
                else
                {
                    TempData["Error"] = "Error al generar pedido. " + rpta.descriptionResponse + "|" + rpta.codeDelivery;
                }
            }
            else
            {
                TempData["Error"] = "Error al generar guia. No existe informacion de recogo para la tienda.";
            }
            #endregion
        }
        public void insertar_historial_estados_cv(string cod_entid, string fc_nint,string descripcion , string id_estado , string cod_vendedor, string cod_tda ,string serieNumero)
        {
            Ent_Usuario user = null;
            user = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string _usu_id = user.usu_id.ToString();

            Ent_HistorialEstadosCV _historial = new Ent_HistorialEstadosCV();
            _historial.cod_entid =cod_entid;
            _historial.cod_usuario = _usu_id;
            _historial.fc_nint =fc_nint;
            _historial.descripcion =descripcion;
            _historial.id_estado =id_estado;
            _historial.cod_vendedor =cod_vendedor;
            _historial.serieNumero =serieNumero;
            _historial.cod_tda = cod_tda;
            int f = datos.insertar_historial_estados_cv(_historial);
            if (f > 0)
            {
                TempData["Success"] = "Se actualizo el estado correctamente";
            }
            else
            {
                TempData["Error"] = "Error al actualizar el estado.";
            }
        }
        private bool _existe_en_array (string[] array , string match)
        {
            bool b = false;
            foreach (var item in array)
            {
                if (!b)
                b = item == match;                
            }
            return b;
        }
        private List<SelectListItem> SelectDestino(string tiendaOrigen, string value=null )
        {

            DataTable dt = datos.get_tiendas_destino(tiendaOrigen);
            List<SelectListItem> list = new List<SelectListItem>();
            if (dt.Rows.Count > 0)
            {
                if (Session["Tienda"] ==null || (Session["Tienda"] != null && ( tiendaOrigen == Session["Tienda"].ToString())))
                list.Add(new SelectListItem() { Text = "TODOS", Value = "" });
                foreach (DataRow row in dt.Rows)
                {

                    if (Session["Tienda"] != null)
                    {
                        if ( (row["cod_entid"].ToString() == Session["Tienda"].ToString()) || (tiendaOrigen != "" && (row["cod_entid"].ToString() != Session["Tienda"].ToString())))
                        {
                            list.Add(new SelectListItem()
                            {
                                Text = row["des_entid"].ToString(),
                                Value = row["cod_entid"].ToString(),
                                Selected = row["cod_entid"].ToString() == value
                            });
                        }
                    }else
                    {
                        list.Add(new SelectListItem()
                        {
                            Text = row["des_entid"].ToString(),
                            Value = row["cod_entid"].ToString(),
                            Selected = row["cod_entid"].ToString() == value
                        });
                    }
                  
                }
            }else
            {
                list.Add(new SelectListItem() { Text = "TODOS", Value = "" });
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
                if (Session["Tienda"] != null )
                {
                    if (row["cod_entid"].ToString() == Session["Tienda"].ToString())
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
                    list.Add(new SelectListItem()
                    {
                        Text = row["des_entid"].ToString(),
                        Value = row["cod_entid"].ToString(),
                        Selected = row["cod_entid"].ToString() == value

                    });
                }           
            }
            return list;
            
        }
        private List<SelectListItem> SelectVendedor(string cod_tda,string value = null)
        {

            DataTable dt = datos.get_vendedores_tda(cod_tda);
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "SELECCIONE", Value = "" });
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row["v_codi"].ToString() + " - " + row["v_nomb"].ToString(),
                    Value = row["v_codi"].ToString().Trim()+ "-" + row["cod_tda"].ToString().Trim(),
                    Selected = row["v_codi"].ToString() == value

                });
            }
            return list;
        }
        private List<CanalVenta> selectVentas(DateTime fdesde , DateTime fhasta, string noDocCli,string noDoc, string tiendaOrigen , string tiendaDestino, string[] tipo , string[] estado)
        {
            List<CanalVenta> ventas = new List<CanalVenta>();

            DataTable dt = datos.get_Ventas(fdesde, fhasta, noDocCli, noDoc, tiendaOrigen, tiendaDestino, String.Join(",", tipo), String.Join(",", estado));
            if (dt != null)
            {
                ventas = (from DataRow dr in dt.Rows
                          select new CanalVenta()
                          {
                              serieNumero = dr["FC_SFAC"].ToString() + "-" + dr["FC_NFAC"].ToString(),
                              tiendaOrigen = dr["COD_ENTID"].ToString() + " - " + dr["des_entida"].ToString(),
                              tiendaDestino = dr["FC_ID_TDACVTA"].ToString() + " - " + dr["des_entidb"].ToString(),
                              tipo = dr["FC_ID_TIP_CVTA"].ToString(),
                              estado = dr["FC_ID_ESTADO_CVTA"].ToString(),
                              cliente = (dr["FC_RUC"].ToString() + " - " + dr["FC_NOMB"].ToString() + " " + dr["FC_APEP"].ToString() + " " + dr["FC_APEM"].ToString()).Trim(),
                              fechaVenta = Convert.ToDateTime(dr["FC_FFAC"]).ToString("dd-MM-yyyy"),
                              fc_nint = dr["FC_NINT"].ToString(),
                              cod_entid = dr["COD_ENTID"].ToString(),
                              nombreEstado = dr["nombreEstado"].ToString(),
                              descripcionEstado = dr["descripcionEstado"].ToString(),
                              colorEstado = dr["colorEstado"].ToString(),
                              importeTotal = Convert.ToDecimal(dr["FC_TOTAL"].ToString()),
                              nombreTipoCV = dr["nombre_tipo_cv"].ToString(),
                              nroPares = Convert.ToDecimal(dr["pares"].ToString()),
                              totalSinIgv = Convert.ToDecimal(dr["soles"].ToString())
                          }).ToList();
            }
            Session["_cv"] = ventas;
            return ventas;
        }
        private CanalVenta selectVenta(string noDoc , string cod_entid, string fc_nint)
        {
            CanalVenta ventas = new CanalVenta();
            Ent_VentaCanal ent_ventas = datos.get_Ventas_por_sn(noDoc , cod_entid , fc_nint);
            if (ent_ventas != null)
            {
                CanalVenta _cnvta = new CanalVenta();
                _cnvta.cod_entid = ent_ventas.cod_entid;
                _cnvta.cliente = ent_ventas.cliente;
                _cnvta.estado = ent_ventas.estado;
                _cnvta.tipo = ent_ventas.tipo;
                _cnvta.serieNumero = ent_ventas.serieNumero;
                _cnvta.tiendaDestino = ent_ventas.tiendaDestino;
                _cnvta.tiendaOrigen = ent_ventas.tiendaOrigen;
                _cnvta.fechaVenta = ent_ventas.fechaVenta.ToString("dd/MM/yyyy");
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
                _cnvta.nombreTipoCV = ent_ventas.nombreTipoCV;
                _cnvta.guia_electronica = ent_ventas.guia_electronica;
                _cnvta.ubigeoCliente = ent_ventas.ubigeoCliente;
                _cnvta.ubigeoTienda = ent_ventas.ubigeoTienda;
                _cnvta.telefonoCliente = ent_ventas.telefonoCliente;
                _cnvta.cod_entid_b = ent_ventas.cod_entid_b;
                List<DetallesCanalVenta> list_cnvtaD = new List<DetallesCanalVenta>();
                foreach (Ent_DetallesVentaCanal item in ent_ventas.detalles)
                {
                    DetallesCanalVenta _cnvtaD = new DetallesCanalVenta();
                    _cnvtaD.cantidad = Convert.ToInt32( item.cantidad);
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
                }
                _cnvta.informacionTiendaDestinatario = _id;
            }
            return ventas;
        }
    }
}