using CapaDato.Maestros;
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
using CapaEntidad.XstoreTda;
using CapaEntidad.ValeCompra;

namespace CapaPresentacion.Controllers
{
    public class XstoreTdaController : Controller
    {
        private Dat_XstoreTienda dat_storeTda = new Dat_XstoreTienda();  
        private string _session_listTdaXstore_private = "_session_listTda_private";
        private string _session_Totalxstore = "_session_totalxstore";
        private string _session_TotalNxstore = "_session_totalNxstore";
        // GET: Consulta
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
                return View(lista());
            }
        }
        public PartialViewResult ListaTienda()
        {
            lista();
            return PartialView();
        }

        public List<Ent_TiendaConf> lista()
        {           
            List<Ent_TiendaConf> liststoreConf = dat_storeTda.List_Tienda_config();


            int nroXstoreActivo = (from n in liststoreConf
                                    where n.bol_xstore == "True"
                                    select n).Count();

            int nroXstoreInactivo = (from n in liststoreConf
                                   where n.bol_xstore == "False"
                                   select n).Count();

            Session[_session_Totalxstore] = nroXstoreActivo;
            Session[_session_TotalNxstore] = nroXstoreInactivo;

            Session[_session_listTdaXstore_private] = liststoreConf;
            return liststoreConf;
        }
        public ActionResult getTienda(Ent_jQueryDataTableParams param)
        {
            List<Ent_TiendaConf> liststoreConf2 = dat_storeTda.List_Tienda_config();
            Session[_session_listTdaXstore_private] = liststoreConf2;          

            /*verificar si esta null*/
            if (Session[_session_listTdaXstore_private] == null)
            {
                List<Ent_TiendaConf> liststoreConf = new List<Ent_TiendaConf>();
                Session[_session_listTdaXstore_private] = liststoreConf;
            }

            int nroXstoreActivo = (from n in liststoreConf2
                                   where n.bol_xstore == "True"
                                   select n).Count();

            int nroXstoreInactivo = (from n in liststoreConf2
                                     where n.bol_xstore == "False"
                                     select n).Count();

            Session[_session_Totalxstore] = nroXstoreActivo;
            Session[_session_TotalNxstore] = nroXstoreInactivo;

            //Traer registros
            IQueryable<Ent_TiendaConf> membercol = ((List<Ent_TiendaConf>)(Session[_session_listTdaXstore_private])).AsQueryable();  //lista().AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<Ent_TiendaConf> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.cod_Entid.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.des_Entid.ToUpper().Contains(param.sSearch.ToUpper()));
            }
            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Ent_TiendaConf, string> orderingFunction =
            (
            m => sortIdx == 0 ? m.bol_xstore :
             m.bol_xstore
            );
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
                             a.cod_Entid,
                             a.des_Entid,
                             a.cod_Emp,
                             a.des_Emp,
                             a.des_Cadena,
                             a.direccion,
                             a.cod_Jefe,
                             a.consecionario,
                             a.bol_gcorrelativo,
                             a.bol_xstore                        
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


        public ActionResult ActualizarEstadoTienda(string CodTienda, Int32 estado)
        {

            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            Int32 respuesta = 0;
            respuesta = dat_storeTda.ActualizarTiendaXstore(CodTienda, estado, _usuario.usu_id);

            var oJRespuesta = new JsonResponse();

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

        public ActionResult GetTotales()
        {
            Int32 strTotalXstore = (Int32)Session[_session_Totalxstore];
            Int32 strTotalNXstore = (Int32)Session[_session_TotalNxstore];
         
            Int32 respuesta = 0;
            respuesta = 1;

            var oJRespuesta = new JsonResponse();

            if (respuesta == 1)
            {
                oJRespuesta.Message = (strTotalXstore).ToString();
                oJRespuesta.Data = (strTotalNXstore).ToString();  
                oJRespuesta.Success = true;
            }
            else
            {

                oJRespuesta.Message = (strTotalXstore).ToString();
                oJRespuesta.Data =  (strTotalNXstore).ToString();
                oJRespuesta.Success = false;
            }

            return Json(oJRespuesta, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerarCorrelativoTienda(string CodTienda)
        {

            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            Int32 respuesta = 0;
            respuesta = dat_storeTda.GenerarCorrelativoTiendaXstore(CodTienda, _usuario.usu_id);

            var oJRespuesta = new JsonResponse();

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
