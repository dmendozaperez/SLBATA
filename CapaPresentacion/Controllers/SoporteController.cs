using CapaDato.Maestros;
using CapaDato.Reporte;
using CapaDato.Soporte;
using CapaEntidad.Control;
using CapaEntidad.General;
using CapaEntidad.Soporte;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers
{
    public class SoporteController : Controller
    {
        //private Dat_ListaTienda dat_lista_tienda = new Dat_ListaTienda();
        Dat_Tienda_Config tiendaconfig = new Dat_Tienda_Config();

        private Dat_Documento_Transac datGuia = new Dat_Documento_Transac(); //gft
        private string _session_doc_transac_private = "_session_doc_transac_private"; //gft
        private string _session_doc_transac_doc_private = "_session_doc_transac_doc_private"; //gft
        private Dat_Combo tienda = new Dat_Combo();//gft

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
            //Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            //string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            //string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            //string return_view = actionName + "|" + controllerName;

            //if (_usuario == null)
            //{
            //    return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            //}
            //else
            //{
            //    if (Session["Tienda"] != null)  //ejemplo : Session["Tienda"]
            //    {
            //        ViewBag.Tienda = tienda.get_ListaTiendaXstore().Where(t => t.cbo_codigo == Session["Tienda"].ToString()).ToList();
            //    }
            //    else
            //    {
            //        ViewBag.Tienda = tienda.get_ListaTiendaXstore(true);
            //    }

            //    return View();
            //}
            if (Session["Tienda"] != null)
            {
                ViewBag.Tienda = tienda.get_ListaTiendaXstore().Where(t => t.cbo_codigo == Session["Tienda"].ToString()).ToList();
            }
            else
            {
                ViewBag.Tienda = tienda.get_ListaTiendaXstore(true);
            }
            return View();

        }

        public List<Ent_Documento_Transac> listaGuia(string cod_entid, DateTime fec_ini, DateTime fec_fin)
        {
            List<Ent_Documento_Transac> listguia = datGuia.get_lista(cod_entid, fec_ini, fec_fin);
            Session[_session_doc_transac_private] = listguia;
            return listguia;
        }

        public PartialViewResult _envioTable(string hidden, string fec_ini,string fec_fin)
        {
            //string dwtda--> se reemplaza por hidden - para agarrar varios id de tiendas por el combo multiselect
            return PartialView(listaGuia(hidden, Convert.ToDateTime(fec_ini), Convert.ToDateTime(fec_fin)));
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
            string strJson = "";
            //  JsonResult jRespuesta = null;
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            strJson = datGuia.Envio_chk(cadena, _usuario.usu_id);
            var serializer = new JavaScriptSerializer();
            //jRespuesta = Json(serializer.Deserialize<List<Ent_Documento_TransacDoc>>(strJson), JsonRequestBehavior.AllowGet);

            return strJson;
        }

        #endregion
    }
}