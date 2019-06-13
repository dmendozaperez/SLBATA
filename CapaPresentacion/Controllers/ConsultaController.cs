﻿using CapaDato.Maestros;
using CapaDato.Transac;
using CapaEntidad.General;
using CapaEntidad.Transac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using CapaEntidad.Util;
using CapaEntidad.Control;
using CapaDato.Reporte;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers
{
    public class ConsultaController : Controller
    {
        private Dat_Concepto_Suna dat_concepto_suna = new Dat_Concepto_Suna();
        private Dat_Documentos_Tda dat_doc = new Dat_Documentos_Tda();
        private string _session_listdocumentoDetalle_private = "_session_listdocumentoDetalle_private";

        private string _session_listguia_private = "_session_listguia_private";

        private Dat_Combo datCbo = new Dat_Combo();
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

                ViewBag.concepto_suna = dat_concepto_suna.lista_concepto_suna();
                return View();
            }
        }
        public PartialViewResult ListaDocumento(string dwtipodoc, string numdoc, string fecini, string fecfinc)
        {
            return PartialView(lista(dwtipodoc, numdoc, fecini, fecfinc));
        }

        public List<Ent_Documentos_Tda> lista(string tipo_doc, string num_doc, string fec_ini, string fec_fin)
        {
            string gcodTda = (String)Session["Tienda"];
            string strParams = "";
            if (gcodTda != "" && gcodTda != null)
            {
                strParams = gcodTda;
            }

            List<Ent_Documentos_Tda> listdoc = dat_doc.get_lista(strParams, tipo_doc, num_doc, fec_ini, fec_fin);
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
                if (Session["Tienda"] != null)
                {
                    ViewBag.Tienda = datCbo.get_ListaTiendaXstore().Where(t => t.cbo_codigo == Session["Tienda"].ToString()).ToList();
                }
                else
                {
                    ViewBag.Tienda = datCbo.get_ListaTiendaXstore();
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
            string jsonData = js.Serialize(datGuia.get_lista(tda_destino, num_guia));

            return jsonData;
        }

        public List<Ent_Consultar_Guia> listaGuia(string tda_destino, string num_guia)
        {
            List<Ent_Consultar_Guia> listguia = datGuia.get_lista(tda_destino, num_guia);
            Session[_session_listguia_private] = listguia;
            return listguia;
        }


        public ActionResult getGuiaAjax(Ent_jQueryDataTableParams param)
        {
            //Traer registros
            string tda_destino = Request["tda_destino"];
            string num_guia = Request["num_guia"];

            List<Ent_Consultar_Guia> mGuia = datGuia.get_lista(tda_destino, num_guia);

            IQueryable<Ent_Consultar_Guia> membercol = ((List<Ent_Consultar_Guia>)(mGuia)).AsQueryable();  //lista().AsQueryable();

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
                             a.fec_recep
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
    }
}
