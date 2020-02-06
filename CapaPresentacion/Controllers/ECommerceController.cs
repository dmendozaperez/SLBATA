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
using CapaDato.comercioxpress;
using Data.Crystal.Reporte;

namespace CapaPresentacion.Controllers
{
    public class ECommerceController : Controller
    {
        Dat_ECommerce datos = new Dat_ECommerce();
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
            //Basico.act_presta_urbano(grabar_numerodoc, ref _error, ref _cod_urbano)
            Dat_PrestaShop action_presta = null;
            Dat_Urbano data_urbano = null;
            Dat_Cexpress data_Cexpress = null;

            /*Datos para devolver*/
            string error = ""; string cod_urbano = "";
            try
            {
                string guia_presta = ""; string guia_urb = ""; string name_carrier = "";
                action_presta = new Dat_PrestaShop();
                data_urbano = new Dat_Urbano();
                //action_presta.get_guia_presta_urba(ven_id, ref guia_presta, ref guia_urb, ref name_carrier);
                action_presta.get_carrier(ven_id, ref guia_presta, ref name_carrier);

                if (guia_presta.Trim().Length > 0)
                {
                    UpdaEstado updateestado = new UpdaEstado();
                    Boolean valida = updateestado.ActualizarReference(guia_presta);

                    if (valida)
                    {
                        data_Cexpress = new Dat_Cexpress();
                        //action_presta.updestafac_prestashop(guia_presta);
                        EnviaPedidoCxpress envia2 = new EnviaPedidoCxpress();
                        string nroserv = "";

                        /*enviamos urbano la guia*/
                        EnviaPedido envia = new EnviaPedido();

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
                                guia_urb = nroserv;

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
                                        guia_urb = ent_urbano.guia;
                                        break;
                                    }
                                }
                            }
                        }

                        //guia_urb=
                        //action_presta.get_guia_presta_urba(ven_id, ref guia_presta, ref guia_urb);

                        ActTracking enviaguia_presta = new ActTracking();
                        string[] valida_prest = enviaguia_presta.ActualizaTrackin(guia_presta, guia_urb);
                        /*el valor 1 quiere decir que actualizo prestashop*/
                        if (valida_prest[0] == "1" && guia_urb.ToString() != "")
                        {
                            data_urbano.updprestashopGuia(guia_presta, guia_urb);
                        }

                        cod_urbano = guia_urb;
                        /************************/
                    }
                }

            }
            catch (Exception exc)
            {
                cod_urbano = "";
                error = exc.Message;
            }
            return RedirectToAction("Index", "ECommerce");
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

        #endregion
    }
}