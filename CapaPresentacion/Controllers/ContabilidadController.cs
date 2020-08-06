using CapaDato.Contabilidad;
using CapaDato.Maestros;
using CapaDato.Reporte;
using CapaEntidad.Contabilidad;
using CapaEntidad.Control;
using CapaEntidad.General;
using CapaEntidad.Maestros;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class ContabilidadController : Controller
    {
        private Dat_ListaTienda dat_lista_tienda = new Dat_ListaTienda();
        private Dat_Contabilidad_EstadoDocumento datConta = new Dat_Contabilidad_EstadoDocumento();
        private string _session_contabilidad_num_private = "_session_contabilidad_num_private";
        private string _session_contb_tienda_peru = "_session_contb_tienda_peru";
        private string _session_contb_popup = "_session_contb_popup";

        // GET: Contabilidad

        #region Contabilidad_Estado_Documento   
        //INDEX
        public ActionResult Estado_Documento()
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
                if (Session["PAIS"].ToString() == "PE") // LISTA DE TIENDAS PARA BATA ECUADOR
                {
                    ViewBag.Tienda = dat_lista_tienda.get_tienda("PE", "1");
                    List<Ent_ListaTienda> listienda = ViewBag.Tienda;
                    Session[_session_contb_tienda_peru] = listienda;
                }
                else
                {
                    ViewBag.Tienda = dat_lista_tienda.get_tienda("EC", "1");
                    List<Ent_ListaTienda> listienda = ViewBag.Tienda;
                    Session[_session_contb_tienda_peru] = listienda;
                }

            }
            return View();
            //else
            //{
            //if (Session[_session_contb_tienda_peru] != null)
            //{
            //    ViewBag.Tienda = Session["_session_contb_tienda_peru"];
            //}
            //else
            //{
            //    ViewBag.Tienda = dat_lista_tienda.get_tienda("PE", "1");
            //    List<Ent_ListaTienda> listienda = ViewBag.Tienda;
            //    Session[_session_contb_tienda_peru] = listienda;
            //}

            //return View();
            //  }
        }

        public List<Ent_Contabilidad_EstadoDocumento> listaTable(string cod_entid, DateTime fec_ini, DateTime fec_fin)
        {
            List<Ent_Contabilidad_EstadoDocumento> listaTable = datConta.get_lista(cod_entid, fec_ini, fec_fin);
            Session[_session_contabilidad_num_private] = listaTable;
            return listaTable;
        }

        public PartialViewResult _Table(string hidden, string fec_ini, string fec_fin, string dwtda)
        {
            if (hidden == null || hidden == "")
            { return PartialView(); }
            else
            {   //string dwtda--> se reemplaza por hidden - para agarrar varios id de tiendas por el combo multiselect
                return PartialView(listaTable(hidden, Convert.ToDateTime(fec_ini), Convert.ToDateTime(fec_fin)));
            }
        }

        public ActionResult getDetAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_contabilidad_num_private] == null)
            {
                List<Ent_Contabilidad_EstadoDocumento> listdoc = new List<Ent_Contabilidad_EstadoDocumento>();
                Session[_session_contabilidad_num_private] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_Contabilidad_EstadoDocumento> membercol = ((List<Ent_Contabilidad_EstadoDocumento>)(Session[_session_contabilidad_num_private])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Contabilidad_EstadoDocumento> filteredMembers = membercol;

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Ent_Contabilidad_EstadoDocumento, string> orderingFunction =
                   (
                   m => sortIdx == 0 ? m.fecha :
                    m.fecha
                   );
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.estado.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.fecha.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.numero.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.serie.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.tienda.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.tipo_doc.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.ruc.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.login_ws.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.clave_ws.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.tipodoc.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.folio.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.total.ToUpper().Contains(param.sSearch.ToUpper()));
            }
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
                             a.estado,
                             a.fecha,
                             a.numero,
                             a.serie,
                             a.tienda,
                             a.tipo_doc,
                             a.total,
                             a.ruc,
                             a.login_ws,
                             a.clave_ws,
                             a.tipodoc,
                             a.folio
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
        //POP UP - DETALLE

        public List<Ent_Contabilidad_EstadoDocumento_Det> listarStr_Detalle_PopUp(string ruc, string login_ws, string clave_ws, string tipodoc, string folio)
        {
            List<Ent_Contabilidad_EstadoDocumento_Det> listguia = datConta.listarStr_Detalle_PopUp(ruc, login_ws, clave_ws, tipodoc, folio);
            Session[_session_contb_popup] = listguia;
            return listguia;
        }

        public PartialViewResult _popUpDetalle(string ruc, string login_ws, string clave_ws, string tipodoc, string folio)
        {
            return PartialView(listarStr_Detalle_PopUp(ruc, login_ws, clave_ws, tipodoc, folio));
        }

        public ActionResult getDetalleAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_contb_popup] == null)
            {
                List<Ent_Contabilidad_EstadoDocumento_Det> listdoc = new List<Ent_Contabilidad_EstadoDocumento_Det>();
                Session[_session_contb_popup] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_Contabilidad_EstadoDocumento_Det> membercol = ((List<Ent_Contabilidad_EstadoDocumento_Det>)(Session[_session_contb_popup])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Contabilidad_EstadoDocumento_Det> filteredMembers = membercol;

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Ent_Contabilidad_EstadoDocumento_Det, string> orderingFunction =
          (
          m => sortIdx == 0 ? m.ESTADO :
           m.ESTADO
          );
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.PDF.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.ESTADO.ToUpper().Contains(param.sSearch.ToUpper()));
            }
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
                             a.PDF,
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

        #endregion

    }
}