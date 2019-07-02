using CapaDato.BataClub;
using CapaDato.Reporte;
using CapaEntidad.BataClub;
using CapaEntidad.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class BataClubController : Controller
    {
       // private Dat_Combo promocion = new Dat_Combo();//gft
        private Dat_BataClub_CuponesCO datProm = new Dat_BataClub_CuponesCO(); //gft
        private string _session_tabla_prom_private = "_session_tabla_prom_private"; //gft
        private string _BataClub_Promociones_Combo = "_BataClub_Promociones_Combo"; //gft

        // GET: BataClub
        public ActionResult Index()
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
            //    if (Session["Tienda"] != null)  
            //    {
            //        ViewBag.Tienda = tienda.get_ListaTiendaXstore().Where(t => t.cbo_codigo == Session["Tienda"].ToString()).ToList();
            //    }
            //    else
            //    {
            //        ViewBag.Tienda = tienda.get_ListaTiendaXstore(true);
            //    }

            //    return View();
            //}
            if (Session["_BataClub_Promociones_Combo"] == null)
               {
                ViewBag.Promocion = datProm.get_ListaPromociones();
                Session["_BataClub_Promociones_Combo"] = ViewBag.Promocion;
               }
            else
            {
                ViewBag.Promocion = Session["_BataClub_Promociones_Combo"];
            }
            return View();
        }

        public PartialViewResult _Table(string dni, string cupon, string hidden)
        {
            //string dwprom--> se reemplaza por hidden - para agarrar varios id de promociones con el combo multiselect
            return PartialView(listaTablaPromociones(dni, cupon, hidden));
        }

        public List<Ent_BataClub_CuponesCO> listaTablaPromociones(string dni, string cupon, string id_grupo)
        {
            List<Ent_BataClub_CuponesCO> listguia = datProm.get_lista_prom(dni, cupon, id_grupo);
            Session[_session_tabla_prom_private] = listguia;
            return listguia;
        }

        //public ActionResult getTablePromoAjax(Ent_jQueryDataTableParams param)
        //{
        //    /*verificar si esta null*/
        //    if (Session[_session_tabla_prom_private] == null)
        //    {
        //        List<Ent_BataClub_CuponesCO> listdoc = new List<Ent_BataClub_CuponesCO>();
        //        Session[_session_tabla_prom_private] = listdoc;
        //    }

        //    //Traer registros
        //    IQueryable<Ent_BataClub_CuponesCO> membercol = ((List<Ent_BataClub_CuponesCO>)(Session[_session_tabla_prom_private])).AsQueryable();

        //    //Manejador de filtros
        //    int totalCount = membercol.Count();

        //    IEnumerable<Ent_BataClub_CuponesCO> filteredMembers = membercol;

        //    //Manejador de orden
        //    var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

        //    var displayMembers = filteredMembers
        //        .Skip(param.iDisplayStart)
        //        .Take(param.iDisplayLength);

        //    var result = from a in displayMembers
        //                 select new
        //                 {
        //                     a.Nombres,
        //                     a.Apellidos,
        //                     a.dni,
        //                     a.correo,
        //                     a.cupon,
        //                     a.tienda,
        //                     a.dni_venta,
        //                     a.nombres_venta,
        //                     a.correo_venta,
        //                     a.telefono_venta,
        //                     a.tickets,
        //                     a.soles,
        //                     a.grupo,
        //                     a.porc_desc,
        //                     a.fec_doc
        //                 };

        //    //Se devuelven los resultados por json
        //    return Json(new
        //    {
        //        sEcho = param.sEcho,
        //        iTotalRecords = totalCount,
        //        iTotalDisplayRecords = filteredMembers.Count(),
        //        aaData = result
        //    }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult getTablePromoAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_tabla_prom_private] == null)
            {
                List<Ent_BataClub_CuponesCO> listdoc = new List<Ent_BataClub_CuponesCO>();
                Session[_session_tabla_prom_private] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_BataClub_CuponesCO> membercol = ((List<Ent_BataClub_CuponesCO>)(Session[_session_tabla_prom_private])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_BataClub_CuponesCO> filteredMembers = membercol;

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            Func<Ent_BataClub_CuponesCO, string> orderingFunction =
                   (
                   m => sortIdx == 0 ? m.fec_doc :
                    m.fec_doc
                   );
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.Apellidos.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.correo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.correo_venta.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.cupon.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.dni.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.dni_venta.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.fec_doc.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.grupo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.Nombres.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.nombres_venta.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.porc_desc.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.soles.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.telefono_venta.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.tickets.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.tienda.ToUpper().Contains(param.sSearch.ToUpper()));
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
                             a.Nombres,
                             a.Apellidos,
                             a.dni,
                             a.correo,
                             a.cupon,
                             a.tienda,
                             a.dni_venta,
                             a.nombres_venta,
                             a.correo_venta,
                             a.telefono_venta,
                             a.tickets,
                             a.soles,
                             a.grupo,
                             a.porc_desc,
                             a.fec_doc
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

    }
}