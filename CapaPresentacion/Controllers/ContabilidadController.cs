using CapaDato.Contabilidad;
using CapaDato.Reporte;
using CapaEntidad.Contabilidad;
using CapaEntidad.Control;
using CapaEntidad.General;
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

        private Dat_Contabilidad_EstadoDocumento datConta = new Dat_Contabilidad_EstadoDocumento(); //gft
        private string _session_contabilidad_num_private = "_session_contabilidad_num_private"; //gft
        private Dat_Combo tienda = new Dat_Combo();//gft
        // GET: Contabilidad
        //INDEX
        public ActionResult Estado_Documento()
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
                if (Session["Tienda"] != null)
                {
                    ViewBag.Tienda = tienda.get_ListaTiendaXstore().Where(t => t.cbo_codigo == Session["Tienda"].ToString()).ToList();
                }
                else
                {
                    ViewBag.Tienda = tienda.get_ListaTiendaXstore(true);
                }
                return View();
          //  }
        }


        public List<Ent_Contabilidad_EstadoDocumento> listaTable(string cod_entid, DateTime fec_ini, DateTime fec_fin)
        {
            List<Ent_Contabilidad_EstadoDocumento> listaTable = datConta.get_lista(cod_entid, fec_ini, fec_fin);
            Session[_session_contabilidad_num_private] = listaTable;
            return listaTable;
        }

        public PartialViewResult _Table(string hidden, string fec_ini, string fec_fin)
        {
            //string dwtda--> se reemplaza por hidden - para agarrar varios id de tiendas por el combo multiselect
            return PartialView(listaTable(hidden, Convert.ToDateTime(fec_ini), Convert.ToDateTime(fec_fin)));
            //return PartialView(listaTable(hidden,fec_ini, fec_fin));
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
                             a.total
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