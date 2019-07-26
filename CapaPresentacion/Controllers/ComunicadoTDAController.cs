using CapaDato.GestionInterno;
using CapaDato.Reporte;
using CapaEntidad.Control;
using CapaEntidad.General;
using CapaEntidad.GestionInterno;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class ComunicadoTDAController : Controller
    {
        // GET: ComunicadoTDA
        private string _session_listcom_private = "_session_listcom_private";
        //tienda
        private Dat_Combo tienda = new Dat_Combo();
        // private Dat_Combo datCbo = new Dat_Combo();

        private Dat_Comunicado datCom = new Dat_Comunicado();

        public ActionResult Index()
        {
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;

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
            ViewBag.Tienda = tienda.get_ListaTiendaXstore(true);
            //    }

            return View();
            //}
        }
        public PartialViewResult _comTable(string dwtda)
        {
            return PartialView(listaCom(dwtda));
        }
        public List<Ent_Comunicado> listaCom(string tda_destino)
        {
            List<Ent_Comunicado> listcom = datCom.lista_comunicado(tda_destino);
            Session[_session_listcom_private] = listcom;
            return listcom;
        }
        public ActionResult getComAjax(Ent_jQueryDataTableParams param)
        {

            /*verificar si esta null*/
            if (Session[_session_listcom_private] == null)
            {
                List<Ent_Comunicado> listcom = new List<Ent_Comunicado>();
                Session[_session_listcom_private] = listcom;
            }

            //Traer registros
            // string tda_destino = Request["tda_destino"];
            // string num_guia = Request["num_guia"];

            //  List<Ent_Consultar_Guia> mGuia = datGuia.get_lista(tda_destino, num_guia);

            //  IQueryable<Ent_Consultar_Guia> membercol = ((List<Ent_Consultar_Guia>)(mGuia)).AsQueryable();  //lista().AsQueryable();
            IQueryable<Ent_Comunicado> membercol = ((List<Ent_Comunicado>)(Session[_session_listcom_private])).AsQueryable();  //lista().AsQueryable();


            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Comunicado> filteredMembers = membercol;

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            Func<Ent_Comunicado,decimal> orderingFunction =
                  (
                  m => sortIdx == 0 ? m.id :
                   m.id
                  );
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.tienda.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.archivo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.descripcion.ToUpper().Contains(param.sSearch.ToUpper()));                                     
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
                             a.id,
                             a.tienda,
                             a.archivo,
                             a.descripcion,
                             a.url,
                             a.fecha_hora_crea,
                             a.fecha_hora_mod,
                             a.file_leido_fecha,
                             a.file_leido,                             
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