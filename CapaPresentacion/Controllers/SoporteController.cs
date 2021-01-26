using CapaDato.Maestros;
using CapaDato.Reporte;
using CapaDato.Soporte;
using CapaEntidad.Control;
using CapaEntidad.General;
using CapaEntidad.Maestros;
using CapaEntidad.Soporte;
using CapaEntidad.Util;
using CapaOraDato.XSTORE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad.ValeCompra;
using Newtonsoft.Json;

namespace CapaPresentacion.Controllers
{
    public class SoporteController : Controller
    {
        //private Dat_ListaTienda dat_lista_tienda = new Dat_ListaTienda();
        Dat_Tienda_Config tiendaconfig = new Dat_Tienda_Config();

        private Dat_Documento_Transac datGuia = new Dat_Documento_Transac(); //gft
        private string _session_doc_transac_private = "_session_doc_transac_private"; //gft
        private string _session_doc_transac_doc_private = "_session_doc_transac_doc_private"; //gft
        private string _session_soporte_tienda_peru = "_session_soporte_tienda_peru";//gft
        private string _session_cupones_retorno = "_session_cupones_retorno";
        private string _session_consulta_guiaerrores = "_session_consulta_guiaerrores"; //aga
        private string _session_LisXCenter_NC = "_session_LisXCenter_NC";

        private Dat_ListaTienda dat_lista_tienda = new Dat_ListaTienda();
        private Dat_Combo tienda = new Dat_Combo();//gft
        Dat_Ticket_Retorno datTR = new Dat_Ticket_Retorno();
        // GET: Soporte
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ConfigTda()
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
                ViewBag.tienda = tiendaconfig.Listar();
                return View();
            }
        }
        [HttpPost]
        public ActionResult getconfigtda(string codtda)
        {

            var config = tiendaconfig.get_config(codtda);

            if (config != null)
            {
                string _codigo_interno = config.CODIGO_INTERNO;
                string _boleta = config.BOLETA;
                string _factura = config.FACTURA;
                string _nc_boleta = config.NCBOLETA;
                string _nc_factura = config.NCFACTURA;

                return Json(new
                {
                    estado = "1",
                    interno = _codigo_interno,
                    boleta = _boleta,
                    factura = _factura,
                    ncboleta = _nc_boleta,
                    ncfactura = _nc_factura
                });
            }
            else
            {
                return Json(new
                {
                    estado = "-1",
                    desmsg = "Error con el servidor"

                });
            }
        }

        #region Documento_Transac

        //Index

        public ActionResult Envio()
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
                if (Session["PAIS"].ToString() == "PE")
                {

                    if (Session[_session_soporte_tienda_peru] != null)
                    {
                        ViewBag.Tienda = Session["_session_soporte_tienda_peru"];
                    }
                    else
                    {
                        ViewBag.Tienda = dat_lista_tienda.get_tienda("PE", "1");
                        List<Ent_ListaTienda> listienda = ViewBag.Tienda;
                        Session[_session_soporte_tienda_peru] = listienda;
                    }
                }
                else
                {
                    //ViewBag.Tienda = tienda.get_ListaTiendaXstore_EC(Session["PAIS"].ToString());
                    ViewBag.Tienda = dat_lista_tienda.get_tienda("EC", "1");
                }
            }
            return View();

        }


        public List<Ent_Documento_Transac> listaGuia(string cod_entid, DateTime fec_ini, DateTime fec_fin)
        {
            List<Ent_Documento_Transac> listguia = datGuia.get_lista(cod_entid, fec_ini, fec_fin);
            Session[_session_doc_transac_private] = listguia;
            return listguia;
        }

        public PartialViewResult _envioTable(string hidden, string fec_ini, string fec_fin, string dwtda)
        {
            if (dwtda == null)
            { return PartialView(); }
            else
            {    //string dwtda--> se reemplaza por hidden - para agarrar varios id de tiendas por el combo multiselect
                return PartialView(listaGuia(hidden, Convert.ToDateTime(fec_ini), Convert.ToDateTime(fec_fin)));
            }
        }

        public ActionResult getGuiaAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_doc_transac_private] == null)
            {
                List<Ent_Documento_Transac> listdoc = new List<Ent_Documento_Transac>();
                Session[_session_doc_transac_private] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_Documento_Transac> membercol = ((List<Ent_Documento_Transac>)(Session[_session_doc_transac_private])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Documento_Transac> filteredMembers = membercol;

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.cod_entid,
                             a.tienda_origen,
                             a.tran_id,
                             a.fecha_des,
                             a.flg_novell,
                             a.fec_env
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

        //PopUp Detalle 

        public List<Ent_Documento_TransacDoc> listarStr_Detalle(string codTienda, string fecha_des)
        {
            List<Ent_Documento_TransacDoc> listguia = datGuia.listarStr_Detalle_Pop(codTienda, Convert.ToDateTime(fecha_des));
            Session[_session_doc_transac_doc_private] = listguia;
            return listguia;
        }

        public PartialViewResult _popUpDetalle(string codTienda, string fecha_des)
        {
            return PartialView(listarStr_Detalle(codTienda, fecha_des));
        }

        public ActionResult getDetalleAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_doc_transac_doc_private] == null)
            {
                List<Ent_Documento_TransacDoc> listdoc = new List<Ent_Documento_TransacDoc>();
                Session[_session_doc_transac_doc_private] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_Documento_TransacDoc> membercol = ((List<Ent_Documento_TransacDoc>)(Session[_session_doc_transac_doc_private])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Documento_TransacDoc> filteredMembers = membercol;

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Ent_Documento_TransacDoc, string> orderingFunction =
          (
          m => sortIdx == 0 ? m.NUM_FAC :
           m.NUM_FAC
          );
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.NUM_FAC.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.TIPO_DOC.ToUpper().Contains(param.sSearch.ToUpper()));
            }
            var sortDirection = Request["sSortDir_0"];
            if (sortDirection == "desc")
                filteredMembers = filteredMembers.OrderBy(orderingFunction);
            else
                filteredMembers = filteredMembers.OrderByDescending(orderingFunction);
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            //var displayMembers = filteredMembers
            //    .Skip(param.iDisplayStart)
            //    .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.TIPO_DOC,
                             a.NUM_FAC,
                             a.SERIE,
                             a.TOTAL,
                             a.ESTADO
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

        //Envío de paquetes
        public string Envio_chk(string cadena)
        {
            string str = "";
            //  JsonResult jRespuesta = null;
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            str = datGuia.Envio_chk(cadena, _usuario.usu_id);
            // var serializer = new JavaScriptSerializer();
            //jRespuesta = Json(serializer.Deserialize<List<Ent_Documento_TransacDoc>>(strJson), JsonRequestBehavior.AllowGet);

            return str;
        }

        #endregion
        #region Prueba de conexion oracle

        public ActionResult PruebaOracle()
        {
            string tienda = "50143";
            Dat_Ora_Conexion dcon = new Dat_Ora_Conexion();
            Ent_Ora_Conexion ora_conexion = dcon.get_conexion_ora(tienda);
            string _mensaje = "";

            if (ora_conexion != null)
            {
                Ent_Acceso_BD.user = ora_conexion.user_ora;
                Ent_Acceso_BD.password = ora_conexion.pas_ora;
                Ent_Acceso_BD.server = ora_conexion.server_ora;
                Ent_Acceso_BD.port = ora_conexion.port_ora;
                Ent_Acceso_BD.sid = ora_conexion.sid_ora;//B143-00062168
                Dat_Ora_Data dat_ora = new Dat_Ora_Data();
                DataTable dt = dat_ora.get_documento("B143-62168", ref _mensaje);

                ViewBag.Mensaje = _mensaje;
                if (dt == null)
                {
                    ViewBag.TranSeq = 0;
                    ViewBag.Total = 0;
                }
                else
                {
                    ViewBag.TranSeq = dt.Rows[0]["TRANS_SEQ"].ToString();
                    ViewBag.Total = dt.Rows[0]["TOTAL"].ToString();
                }
            }
            return View();

        }
        #endregion

        #region Ticket Retorno
        public ActionResult TicketRetorno()
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
                Session[_session_cupones_retorno] = null;
                //VLADIMIR
                ViewBag.Tienda = tienda.get_ListaTiendaXstore(Session["PAIS"].ToString(),true);
                //VLADIMIR END
                return View();
            }
        }
        public PartialViewResult _DataCupones(string dwtda, string cupon, string fechaIni, string fechaFin)
        {
            if (!String.IsNullOrEmpty(cupon))
            {
                dwtda = "";
            }

            return PartialView(listaTablaCupones(dwtda, cupon, fechaIni, fechaFin));
        }
        public List<Ent_Ticket_Retorno> listaTablaCupones(string dwtda, string cupon, string fechaIni, string fechaFin)
        {
            List<Ent_Ticket_Retorno> listguia = datTR.get_lista_cupones_retorno(dwtda, cupon, fechaIni, fechaFin);
            Session[_session_cupones_retorno] = listguia;
            return listguia;
        }

        public ActionResult getTablaCuponesRetorno(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_cupones_retorno] == null)
            {
                List<Ent_Ticket_Retorno> listdoc = new List<Ent_Ticket_Retorno>();
                Session[_session_cupones_retorno] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_Ticket_Retorno> membercol = ((List<Ent_Ticket_Retorno>)(Session[_session_cupones_retorno])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Ticket_Retorno> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m =>
                    m.codigo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.estado.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.tiendaGen.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.fechaGen.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.tiendaUso.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.fechaUso.ToUpper().Contains(param.sSearch.ToUpper()));
            }

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            var sortDirection = Request["sSortDir_0"];
            if (param.iSortingCols > 0)
            {
                if (sortDirection == "asc")
                {
                    if (sortIdx == 1)
                    {
                        filteredMembers = filteredMembers.OrderBy(o => o.estado);
                    }
                    else if (sortIdx == 5)
                    {
                        filteredMembers = filteredMembers.OrderBy(o => Convert.ToDateTime(o.fechaGen));
                    }
                    else if (sortIdx == 9)
                    {
                        filteredMembers = filteredMembers.OrderBy(o => Convert.ToDateTime(o.fechaUso));
                    }


                }
                else
                {
                    if (sortIdx == 1)
                    {
                        filteredMembers = filteredMembers.OrderByDescending(o => o.estado);
                    }
                    else if (sortIdx == 5)
                    {
                        filteredMembers = filteredMembers.OrderByDescending(o => Convert.ToDateTime(o.fechaGen));
                    }
                    else if (sortIdx == 9)
                    {
                        filteredMembers = filteredMembers.OrderByDescending(o => Convert.ToDateTime(o.fechaUso));
                    }
                }
            }
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.codigo,
                             a.estado,
                             a.montoDscto,
                             a.impreso,
                             a.tiendaGen,
                             a.fechaGen,
                             a.montoGen,
                             a.serieGen,
                             a.numeroGen,
                             a.tiendaUso,
                             a.fechaUso,
                             a.montoUso,
                             a.serieUso,
                             a.numeroUso,
                             a.nroReimp,
                         };
            var numvariable1 = filteredMembers.Count(n => n.estado.ToUpper() == "CONSUMIDO");
            var numvariable2 = filteredMembers.Count(n => n.estado.ToUpper() == "DISPONIBLE");
            var numvariable3 = filteredMembers.Count(n => n.estado.ToUpper() == "CADUCADO");
            // param.variable1 = lblConsumidos;
            param.variable1 = numvariable1.ToString();
            param.variable2 = numvariable2.ToString();
            param.variable3 = numvariable3.ToString();
            //Se devuelven los resultados por json
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = result,
                variable1 = param.variable1,
                variable2 = param.variable2,
                variable3 = param.variable3
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult REIMPRESION_TR(string cupon)
        {

            string _mensaje = "";
            bool _upd = false;
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];


            if (String.IsNullOrEmpty(cupon))
            {
                _mensaje = "Cupon inválido.";
            }
            if (_usuario == null)
            {
                _mensaje = "No hay un usuario logeado.";
            }


            if (_mensaje == "")
            {
                _upd = datTR.REIMPRESION_TR(cupon, DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss") + " Usuario: " + _usuario.usu_nombre, ref _mensaje);
                if (_upd)
                {
                    return Json(new { estado = 1, mensaje = "Reimpresión enviada" });
                }
                else
                {
                    return Json(new { estado = 0, mensaje = "Error" });
                }
            }
            else
            {
                return Json(new { estado = 0, mensaje = _mensaje });
            }
        }
        #endregion

        #region Consulta guia errores
        public ActionResult ConsultaGuiaError() // Inicio.Lista Combo con estados de GUIAS
        {
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;
            //string nroTK_Soporte = (Request.HttpMethod == "POST" ? Request.Params["nroTK_Soporte"].ToString() : null);

            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }

            else
            {
                Session[_session_consulta_guiaerrores] = null;

                if (_usuario != null) //verifica session diferente de usuario de tienda
                {
                    var ListEstado = new List<SelectListItem>() {
                    new SelectListItem() { Value = "S", Text = "--SELECCIONE--" },
                    new SelectListItem() { Value = "", Text = "TODOS LOS ESTADOS" },
                    new SelectListItem() { Value = "A", Text = "ABIERTO" },
                    new SelectListItem() { Value = "E", Text = "EN PROCESO" },
                    new SelectListItem() { Value = "C", Text = "CERRADO" },
                    };
                    ViewBag.Estado = ListEstado;
                }
            }
            return View();
        }

        public PartialViewResult _guia_tableError(string dwestado, string tk_soporte) //Lista grid con todo los casos de las GUIAS ,BUSQUEDA
        {
            if (dwestado == null)
            {
                dwestado = "";
            }
            if (tk_soporte == null)
            {
                tk_soporte = "";
            }

            return PartialView(listaGuiaErr(dwestado, tk_soporte));
        }

        public List<Ent_Consulta_GuiaError> listaGuiaErr(string dwGuiaErr, string nroTK_Soporte)
        {
            Dat_ConsultaGuia_Error datGuiaErr = new Dat_ConsultaGuia_Error();

            List<Ent_Consulta_GuiaError> listguiaErr = datGuiaErr.get_lista_guia_error(dwGuiaErr, nroTK_Soporte);
            Session[_session_consulta_guiaerrores] = listguiaErr;
            return listguiaErr;
        }

        public ActionResult getGuiaErrorAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_consulta_guiaerrores] == null)
            {
                List<Ent_Consulta_GuiaError> listdoc = new List<Ent_Consulta_GuiaError>();
                Session[_session_consulta_guiaerrores] = listdoc;

            }

            //Traer registros
            IQueryable<Ent_Consulta_GuiaError> membercol = ((List<Ent_Consulta_GuiaError>)(Session[_session_consulta_guiaerrores])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Consulta_GuiaError> filteredMembers = membercol;

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.Codigo,
                             a.Caja,
                             a.TK_xstore,
                             a.Tienda,
                             a.Fec_Documento,
                             a.Num_Documento,
                             a.Articulo,
                             a.Calidad,
                             a.Cantidad,
                             a.Fec_Ingreso,
                             a.Fec_Act,
                             a.Mantis_Nro,
                             a.Estado,
                             a.TK_soporte_Bata

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

        //[HttpPost]
        public ActionResult ActualizarEstadoGuia(int Id, string Tk_soporte, string Estado)
        {
            Dat_ConsultaGuia_Error datGuiaErr = new Dat_ConsultaGuia_Error();
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            Int32 respuesta = 0;
            var oJRespuesta = new JsonResponse();


            if (Estado == "1" && Tk_soporte != "")
            {
                Estado = "E"; // EN PROCESO
            }
            if (Estado == "2" && Tk_soporte != "")
            {
                Estado = "C"; // CERRADO
            }


            if (Tk_soporte == "")
            {
                oJRespuesta.Message = (respuesta).ToString();
                oJRespuesta.Data = false;
                oJRespuesta.Success = false;
            }
            else

                respuesta = datGuiaErr.ActualizarGuiaError(Id, Tk_soporte, 0, Estado); //Finalizar

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
        #endregion

        #region <Extender Nota de Credito Center>
        public ActionResult Extender_NC_XCenter()
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
                Ent_Extender_NC entExtender_NC = new Ent_Extender_NC();
                ViewBag.entExtender_NC = entExtender_NC;

                List<Ent_ListaTienda> tiendas = new List<Ent_ListaTienda>();
                tiendas.Add(new Ent_ListaTienda() { cod_entid = "-1", des_entid = "SELECCIONE" });
                ViewBag.tienda = tiendas.Concat(dat_lista_tienda.get_tienda("PE", "1"));
                return View();
            }

        }

        public JsonResult getLisXCenter_NC(Ent_jQueryDataTableParams param, string FechaInicio, string FechaFin,string Tienda, string Num_Doc, bool isOkUpdate)
        {
            Ent_Extender_NC _ent = new Ent_Extender_NC();
            _ent.Tienda = Tienda;
            _ent.Num_Doc = Num_Doc;
            _ent.FechaInicio = DateTime.Parse(FechaInicio);
            _ent.FechaFin = DateTime.Parse(FechaFin);

            if (isOkUpdate)
            {
                Session[_session_LisXCenter_NC] = datGuia.LisXCenter_NC(_ent).ToList();
            }

            /*verificar si esta null*/
            if (Session[_session_LisXCenter_NC] == null)
            {
                List<Ent_Extender_NC> ListarXCenter_NC = new List<Ent_Extender_NC>();
                Session[_session_LisXCenter_NC] = ListarXCenter_NC;
            }

            IQueryable<Ent_Extender_NC> entDocTrans = ((List<Ent_Extender_NC>)(Session[_session_LisXCenter_NC])).AsQueryable();

            //Manejador de filtros
            int totalCount = entDocTrans.Count();
            IEnumerable<Ent_Extender_NC> filteredMembers = entDocTrans;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = entDocTrans
                    .Where(m =>
                                m.Serial_Nbr.ToUpper().Contains(param.sSearch.ToUpper()) ||
                                m.String_Value.ToUpper().Contains(param.sSearch.ToUpper())
                            );
            }

            //Manejador de ordene
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            if (param.iSortingCols > 0)
            {
                if (Request["sSortDir_0"].ToString() == "asc")
                {
                    switch (sortIdx)
                    {
                        case 0: filteredMembers = filteredMembers.OrderBy(o => o.Serial_Nbr); break;
                        case 1: filteredMembers = filteredMembers.OrderBy(o => o.Organization_Id); break;
                        case 2: filteredMembers = filteredMembers.OrderBy(o => o.Rtl_Loc_Id); break;
                        case 3: filteredMembers = filteredMembers.OrderBy(o => o.Wkstn_Id); break;
                        case 4: filteredMembers = filteredMembers.OrderBy(o => o.Trans_Seq); break;
                        case 5: filteredMembers = filteredMembers.OrderBy(o => o.String_Value); break;
                        case 6: filteredMembers = filteredMembers.OrderBy(o => o.Business_Date); break;
                        case 7: filteredMembers = filteredMembers.OrderBy(o => o.Expr_Date); break;
                    }
                }
                else
                {
                    switch (sortIdx)
                    {
                        case 0: filteredMembers = filteredMembers.OrderByDescending(o => o.Serial_Nbr); break;
                        case 1: filteredMembers = filteredMembers.OrderByDescending(o => o.Organization_Id); break;
                        case 2: filteredMembers = filteredMembers.OrderByDescending(o => o.Rtl_Loc_Id); break;
                        case 3: filteredMembers = filteredMembers.OrderByDescending(o => o.Wkstn_Id); break;
                        case 4: filteredMembers = filteredMembers.OrderByDescending(o => o.Trans_Seq); break;
                        case 5: filteredMembers = filteredMembers.OrderByDescending(o => o.String_Value); break;
                        case 6: filteredMembers = filteredMembers.OrderByDescending(o => o.Business_Date); break;
                        case 7: filteredMembers = filteredMembers.OrderByDescending(o => o.Expr_Date); break;
                    }
                }
            }

            var Result = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            //Se devuelven los resultados por json
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = Result
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getUpExtender_NC(Ent_Extender_NC _Ent)
        {
            bool Result = false;
            string Estado = string.Empty;
            JsonResponse objResult = new JsonResponse();
            try
            {
                Result = datGuia.UpExtender_NC(_Ent, ref Estado);
                if (Result)
                {
                    objResult.Success = true;
                    objResult.Message = "La nueva fecha se actualizo correctamente.";
                }
                else
                {
                    objResult.Success = false;
                    objResult.Message = Estado;
                }
            }
            catch (Exception ex)
            {
                objResult.Success = false;
                objResult.Message = "Error al actualizar";
            }

            var JSON = JsonConvert.SerializeObject(objResult);
            return Json(JSON, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}