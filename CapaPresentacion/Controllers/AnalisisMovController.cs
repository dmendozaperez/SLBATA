using CapaDato.Maestros;
using CapaDato.Transac;
using CapaEntidad.Control;
using CapaEntidad.General;
using CapaEntidad.Transac;
using CapaEntidad.Util;
using CapaPresentacion.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class AnalisisMovController : Controller
    {
        // GET: AnalisisMov
        private Dat_ListaTienda dat_lista_tienda = new Dat_ListaTienda();
        private Dat_Analisis_Mov dat_analisis = new Dat_Analisis_Mov();
        private string _session_listanalisisDetalle_private = "_session_listanalisisDetalle_private";
        //private string gcodTda = "";
        public ActionResult Index()
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
                //gcodTda = (String)Session["Tienda"];
                /*SOLO TIENDA XSTORE*/
                ViewBag.tienda = dat_lista_tienda.get_tienda("PE", "1");
           
                string codTda = "";
                string gcodTda = (String)Session["Tienda"];
                if (gcodTda != "" && gcodTda != null)
                {
                    codTda = gcodTda;
                }
                ViewBag.codTienda = codTda;
            }
            return View();
        }
        public PartialViewResult ListaAnalisisMov(string dwtienda,string fecini, string fecfinc,string articulo,string talla)
        {

            if (dwtienda == null) dwtienda = Session["Tienda"].ToString();

            string _cod_art = Basico.Left(articulo, 7);
            string _calidad = Basico.Right(articulo, 1);

            if (talla.Trim().Length == 0) talla = "-1";

            List<Ent_Analisis_Mov> lista_mov = lista(dwtienda, Convert.ToDateTime(fecini), Convert.ToDateTime(fecfinc), _cod_art, _calidad, talla);

            Int32 inicial = 0; Int32 saldo = 0;
            if (lista_mov!=null)
            {
                
                if (lista_mov.Count>0)
                {
                    Int32 filas = lista_mov.Count();
                    inicial = lista_mov.Where(i => i.item == 1).Sum(s=>s.inicial);
                    saldo = lista_mov.Where(i => i.item== filas).Sum(s=>s.saldo);
                }
            }
            ViewBag.inicial = inicial;
            ViewBag.saldo = saldo;
            return PartialView(lista_mov);
        }
        public List<Ent_Analisis_Mov> lista(string cod_tda, DateTime fec_ini, DateTime fec_fin,
                                                string cod_art, string calidad, string talla)
        {
            string gcodTda = (String)Session["Tienda"];
            string strParams = "";
            if (gcodTda != "" && gcodTda != null)
            {
                strParams = gcodTda;
            }

            List<Ent_Analisis_Mov> listdoc = dat_analisis.get_lista(cod_tda, fec_ini, fec_fin, cod_art, calidad, talla);
            Session[_session_listanalisisDetalle_private] = listdoc;
            return listdoc;
        }
        public ActionResult getAnalisisMov(Ent_jQueryDataTableParams param)
        {

            /*verificar si esta null*/
            if (Session[_session_listanalisisDetalle_private] == null)
            {
                List<Ent_Analisis_Mov> listdoc = new List<Ent_Analisis_Mov>();
                Session[_session_listanalisisDetalle_private] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_Analisis_Mov> membercol = ((List<Ent_Analisis_Mov>)(Session[_session_listanalisisDetalle_private])).AsQueryable();  //lista().AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<Ent_Analisis_Mov> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.tipo_doc.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.num_doc.ToUpper().Contains(param.sSearch.ToUpper()));
            }
            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Ent_Analisis_Mov, string> orderingFunction =
            (
            m => sortIdx == 0 ? m.tipo_doc :
             m.num_doc
            );
            var sortDirection = Request["sSortDir_0"];
            //if (sortDirection == "asc")
            //    filteredMembers = filteredMembers.OrderBy(orderingFunction);
            //else
            //    filteredMembers = filteredMembers.OrderByDescending(orderingFunction);
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);
            var result = from a in displayMembers
                         select new
                         {
                             a.item,
                             a.fecha,
                             a.tipo_doc,
                             a.num_doc,
                             a.origen,
                             a.destino,
                             a.inicial,
                             a.ingreso,
                             a.salida,
                             a.saldo,                             
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