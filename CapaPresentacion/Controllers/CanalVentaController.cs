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

namespace CapaPresentacion.Controllers
{
    public class CanalVentaController : Controller
    {
        Dat_CanalVenta datos = new Dat_CanalVenta();
        // GET: Cnvta
        public ActionResult Index()
        {
            //_Urbano("F254-00000293" , "50254" , "D0431409");
            bool _reporte;
            string fdesde = (Request.HttpMethod == "POST" ? Request.Params["fdesde"].ToString() : DateTime.Now.ToString("dd/MM/yyyy"));
            string fhasta = (Request.HttpMethod == "POST" ? Request.Params["fhasta"].ToString() : DateTime.Now.ToString("dd/MM/yyyy"));
            string noDocCli = (Request.HttpMethod == "POST" ? Request.Params["noDocCli"].ToString() : null);
            string noDoc = (Request.HttpMethod == "POST" ? Request.Params["noDoc"].ToString() : null);
            string tiendaOrigen = (Request.HttpMethod == "POST" ? Request.Params["tiendaOrigen"].ToString() : (Session["Tienda"] != null ? Session["Tienda"].ToString() : null));
            string tiendaDestino = (Request.HttpMethod == "POST" ? Request.Params["tiendaDestino"].ToString() : null);
            string estado = (Request.HttpMethod == "POST" ? Request.Params["estado"] : "001,002,003,004,005");
            string tipo = (Request.HttpMethod == "POST" ? Request.Params["tipo"] : "1,2,3");

            ViewBag._selectOrigen = SelectOrigen(tiendaOrigen);
            ViewBag._selectDestino = SelectDestino((tiendaOrigen == null ? "" : tiendaOrigen), (tiendaDestino == null ? "" : tiendaDestino));
            ViewBag._fdesde = fdesde;
            ViewBag._fhasta = fhasta;
            ViewBag._noDocCli = noDocCli;
            ViewBag._noDoc = noDoc;
            ViewBag._tiendaOrigen = tiendaOrigen;
            ViewBag._tiendaDestino = tiendaDestino;
            ViewBag._selectTipos = SelectTipos((tipo == null ? "1,2,3" : tipo));
            ViewBag._selectEstados = SelectEstados((estado == null ? "001,002,003,004,005" : estado));

            List<CanalVenta> listCV = new List<CanalVenta>();
            listCV = selectVentas(Convert.ToDateTime(fdesde), Convert.ToDateTime(fhasta), noDocCli, noDoc, tiendaOrigen, tiendaDestino, tipo, estado);
            /*Parametros para el reporte*/
            _reporte = false;
            if (Request.HttpMethod == "POST")
            {
                _reporte = true;
                this.HttpContext.Session["rptSource"] = listCV;
                this.HttpContext.Session["pA"] = Request.Params["vtA"];
                this.HttpContext.Session["pB"] = Request.Params["vtB"];
                this.HttpContext.Session["pDesde"] = fdesde;
                this.HttpContext.Session["pHasta"] = fhasta;
                this.HttpContext.Session["pTipos"] = Request.Params["vtTipos"];
                this.HttpContext.Session["pEstado"] = Request.Params["vtEstados"];
                this.HttpContext.Session["pCliente"] = noDocCli;
                this.HttpContext.Session["pNoDocumento"] = noDoc;
                //this.HttpContext.Session[""] = ;
            }
            ViewBag._reporte = _reporte;
            return View(listCV);
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
            string[] _values = value.Split(',');
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "FACTURADO", Value = "001", Selected = _existe_en_array(_values, "001") });
            list.Add(new SelectListItem() { Text = "ENTREGADO", Value = "002", Selected = _existe_en_array(_values, "002") });
            list.Add(new SelectListItem() { Text = "RECHAZADO", Value = "003", Selected = _existe_en_array(_values, "003") });
            list.Add(new SelectListItem() { Text = "RECEPCIONADO", Value = "004", Selected = _existe_en_array(_values, "004") });
            list.Add(new SelectListItem() { Text = "DELIVERY", Value = "005", Selected = _existe_en_array(_values, "005") });
            return list;
        }
        public ActionResult Ver(string id, string cod_entid, string fc_nint, string ge = null)
        {
            ViewBag.id = id;
            ViewBag._SelectVendedor = SelectVendedor(cod_entid);
             return View(selectVenta(id, cod_entid, fc_nint));
        }
        public ActionResult ActualizarAcepado(string descripcion, string id, string cod_entid, string fc_nint, string vendedor)
        {
            ActualizarEstado("002", descripcion, id, cod_entid, fc_nint, vendedor);
            return RedirectToAction("Ver", "CanalVenta", new { id = id, fc_nint = fc_nint, cod_entid = cod_entid });
        }

        public ActionResult ActualizarRecepcionado(string descripcion, string id, string cod_entid, string fc_nint, string vendedor)
        {
            ActualizarEstado("004", descripcion, id, cod_entid, fc_nint, vendedor);
            return RedirectToAction("Ver", "CanalVenta", new { id = id, fc_nint = fc_nint, cod_entid = cod_entid });
        }
        public ActionResult ActualizarRechazado(string descripcion, string id, string cod_entid, string fc_nint, string vendedor)
        {
            ActualizarEstado("003", descripcion, id, cod_entid, fc_nint, vendedor);
            return RedirectToAction("Ver", "CanalVenta", new { id = id, fc_nint = fc_nint, cod_entid = cod_entid });
        }

        public ActionResult ImprimirCodigo(string id, string tienda, string fc_nint)
        {
            List<GuiaElectronica> _ge = new List<GuiaElectronica>();
            GuiaElectronica ge = new GuiaElectronica();

            CanalVenta _cv = selectVenta(id, tienda, fc_nint);
            ge.guia = _cv.guia_electronica;
            ge.cliente = (_cv.tipo == "3" ? _cv.cliente : _cv.tiendaOrigen);
            ge.direccion = (_cv.tipo == "3" ? _cv.direccionCliente : _cv.direccionA);
            ge.referencia = (_cv.tipo == "3" ? _cv.referenciaCliente : "Sin Referencia");
            ge.ubigeo = (_cv.tipo == "3" ? _cv.ubigeoCliente : _cv.ubigeoTienda);
            _ge.Add(ge);

            string strReportName = "GuiaElectronica.rpt";
            ReportDocument rd = new ReportDocument();
            string strRptPath = Server.MapPath("~/") + @"RptsCrystal\" + strReportName;
            rd.Load(strRptPath);
            rd.SetDataSource(_ge);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/pdf", "GuiaElectronica_" + ge.guia + ".pdf");
        }


        public ActionResult ActualizarDelivery(string descripcion, string id, string cod_entid, string fc_nint, string vendedor)
        {
            cxpress.WSOrdenServicioClient obj1 = new cxpress.WSOrdenServicioClient();
            cxpress.OrdenServicioReqParm objcla = new cxpress.OrdenServicioReqParm();
            cxpress.WSOrdenServicioClient dd = new cxpress.WSOrdenServicioClient();
            /*             
             <codCliente>141</codCliente>
             <codCtaCliente>142</codCtaCliente>
             <nroDocProveedor>20145556666</nroDocProveedor>
             <codDireccionProveedor>900055</codDireccionProveedor>
             */
            CanalVenta cvU = selectVenta(id, cod_entid, fc_nint);
            objcla.nroPedido = new String[] { cvU.serieNumero };// nroPedido;
            List<cxpress.item> lista = new List<cxpress.item>();

            foreach (var item in cvU.detalles)
            {
                cxpress.item objdet = new cxpress.item();
                objdet.descItem = new String[] { item.nombreProducto };
                objdet.cantItem = new int[] { item.cantidad };
                objdet.pesoMasa = new float[] { 1 };
                objdet.altoItem = new float[] { 1 };
                objdet.largoItem = new float[] { 1 };
                objdet.anchoItem = new float[] { 1 };
                objdet.valorItem = new float[] { 1 };
                lista.Add(objdet);
            }

            objcla.listaItems = lista.ToArray();

            objcla.volumen = new double[] { 10 };           //No hay 
            objcla.tipoServicio = new long[] { 101 };       // 
                                                            /*Codigos para prueba 141 y  142*/
            objcla.codCliente = new long[] { 141 };         //entregado por CX
            objcla.codCtaCliente = new long[] { 142 };      //entregado por CX
            objcla.cantPiezas = new int[] { cvU.detalles.Sum(cant => cant.cantidad) };
            objcla.codRef1 = new String[] { "0012071801" }; //opcional
            objcla.codRef2 = new String[] { "0012071801" }; //opcional
            objcla.valorProducto = new double[] { 1 };
            objcla.tipoOrigenRecojo = new int[] { 1 };
            objcla.codTipoDocProveedor = new long[] { 112 };    //entregado por CX
                                                                /*Para nroDocProveedor 20145556666*/
            objcla.nroDocProveedor = new String[] { "20145556666" };
            /*Para codDireccionProveedor 900055*/
            objcla.codDireccionProveedor = new long[] { 0900055 };  //entregado por CX
            objcla.indicadorGeneraRecojo = new int[] { 1 };
            objcla.tipoDestino = new int[] { 1 };
            objcla.direccEntrega = new String[] { (cvU.tipo == "3" ? cvU.direccionCliente : cvU.direccionA) };  // Dirección de entrega
                                                                                                                //Ubigeo dirección entrega  key.ubi_direc
            objcla.refDireccEntrega = new String[] { (cvU.tipo == "3" ? cvU.referenciaCliente : "sin referencia") }; //Referencia dirección entrega
            objcla.codDepartEntrega = new String[] { "15" }; //Departamento = Lima
            objcla.codProvEntrega = new String[] { "01" }; //Provincia = Lima
            objcla.codDistEntrega = new String[] { (cvU.tipo == "3" ? (cvU.ubigeoCliente.ToString() == "" ? cvU.ubigeoTienda.Substring(4) : cvU.ubigeoCliente.Substring(4)) : cvU.ubigeoTienda.Substring(4)) };
            objcla.nomDestEntrega = new String[] { (cvU.tipo == "3" ? cvU.nombreCliente : cvU.tiendaOrigen) };
            objcla.apellDestEntrega = new String[] { (cvU.tipo == "3" ? cvU.apePatCliente + ' ' + cvU.apeMatCliente : "BATA") };  //"Perez Luna"
            objcla.codTipoDocDestEntrega = new String[] { (cvU.noDocCli.Length == 11 ?  "112" : "109") };
            objcla.nroDocDestEntrega = new String[] { (cvU.tipo == "3" ? cvU.noDocCli: "20603000472") }; //supongo que si es para tienda debe ir el ruc de bata    //"12345678"
            objcla.telefDestEntrega = new String[] { (cvU.tipo == "3" ? cvU.telefonoCliente : "01777888") }; // telefono del cliente         //"991276768"
            objcla.emailDestEntrega = new String[] { "enriqueheredia.e@gmail.com" };     //"juanperez@gmail.com"
            objcla.idUsuario = new String[] { "EMPRESA  S.A.C." };
            objcla.deTerminal = new String[] { "LIMA" };
            
            var e= obj1.registrar(objcla);            

            if (e.nroOrdenServicio != null)
            {
                ActualizarEstado("005", descripcion, id, cod_entid, fc_nint, vendedor);
                datos.insertar_ge_cv(cod_entid, fc_nint, id, e.nroOrdenServicio);
                TempData["Success"] = "Guia generada correctamente.";
            }else
            {
                TempData["Error"] = "Errro al generar guia. " + e.msg; ;
            }
            return RedirectToAction("Ver", "CanalVenta", new { id = id, fc_nint = fc_nint, cod_entid = cod_entid });
            
            // Console.Write(e.ToString());
            
            #region
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

        }


        public void ActualizarEstado(string estado, string descripcion, string id, string cod_entid, string fc_nint, string vendedor)
        {
            Ent_Usuario user = null;
            user = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string _usu_id = user.usu_id.ToString();
            HistorialEstadosCV _historial = new HistorialEstadosCV();
            _historial.cod_usuario = _usu_id;
            _historial.cod_entid = cod_entid;
            _historial.fc_nint = fc_nint;
            _historial.descripcion = descripcion;
            _historial.id_estado = estado;
            _historial.cod_vendedor = vendedor;
            _historial.serieNumero = id;
            insertar_historial_estados_cv(_historial);
        }

        public void insertar_historial_estados_cv(HistorialEstadosCV historial)
        {
            Ent_HistorialEstadosCV _historial = new Ent_HistorialEstadosCV();
            _historial.cod_entid = historial.cod_entid;
            _historial.cod_usuario = historial.cod_usuario;
            _historial.fc_nint = historial.fc_nint;
            _historial.descripcion = historial.descripcion;
            _historial.id_estado = historial.id_estado;
            _historial.cod_vendedor = historial.cod_vendedor;
            _historial.serieNumero = historial.serieNumero;
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
                
                list.Add(new SelectListItem() { Text = "TODOS", Value = "" });
                foreach (DataRow row in dt.Rows)
                {

                    if (Session["Tienda"] != null)
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
                    Text = row["v_codi"].ToString() + " - "+  row["v_nomb"].ToString(),
                    Value = row["v_codi"].ToString(),
                    Selected = row["v_codi"].ToString() == value

                });
            }
            return list;
        }
        private List<CanalVenta> selectVentas(DateTime fdesde , DateTime fhasta, string noDocCli,string noDoc, string tiendaOrigen , string tiendaDestino, string tipo , string estado)
        {
            List<CanalVenta> ventas = new List<CanalVenta>();
            List<Ent_VentaCanal> ent_ventas =  datos.get_Ventas(fdesde,fhasta,noDocCli, noDoc , tiendaOrigen , tiendaDestino ,  tipo, estado);
            if (ent_ventas != null)
            {
                foreach (var item in ent_ventas)
                {
                    CanalVenta _cnvta = new CanalVenta();
                    _cnvta.cliente = item.cliente;
                    _cnvta.estado = item.estado;
                    _cnvta.tipo = item.tipo;
                    _cnvta.serieNumero = item.serieNumero;
                    _cnvta.tiendaDestino = item.tiendaDestino;
                    _cnvta.tiendaOrigen = item.tiendaOrigen;
                    _cnvta.fechaVenta = item.fechaVenta;
                    _cnvta.fc_nint = item.fc_nint;
                    _cnvta.cod_entid = item.cod_entid;
                    _cnvta.nombreEstado = item.nombreEstado;
                    _cnvta.descripcionEstado = item.descripcionEstado;
                    _cnvta.colorEstado = item.colorEstado;
                    _cnvta.nombreTipoCV = item.nombreTipoCV;
                    _cnvta.importeTotal = item.importeTotal;
                    ventas.Add(_cnvta);
                }
            }
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
            }
            return ventas;
        }
    }
}