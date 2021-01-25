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
using System.Web.Script.Serialization;
using CapaDato.XstoreTda;
using CapaPresentacion.Data;
using System.Net.NetworkInformation;
using CapaPresentacion.Data.Crystal.Reporte;
using CapaPresentacion.Models.Crystal.Reporte;

namespace CapaPresentacion.Controllers
{
    public class XstoreTdaController : Controller
    {
        private Dat_XstoreTienda dat_storeTda = new Dat_XstoreTienda();
        private string _session_listTdaXstore_private = "_session_listTda_private";
        private string _session_Totalxstore = "_session_totalxstore";
        private string _session_TotalNxstore = "_session_totalNxstore";

        private string _session_cc_central_xst = "_session_cc_central_xst";
        private string _session_cc_caja_xst = "_session_cc_caja_xst";
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
            List<Ent_TiendaConf> liststoreConf = dat_storeTda.List_Tienda_config(Session["PAIS"].ToString());


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
            List<Ent_TiendaConf> liststoreConf2 = dat_storeTda.List_Tienda_config(Session["PAIS"].ToString());
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
                    .Where(m => m.des_Entid.ToUpper().Contains(param.sSearch.ToUpper()) ||
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
                             a.bol_xstore,
                             a.outlet
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
                oJRespuesta.Data = (strTotalNXstore).ToString();
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

        public string listarStr_DatosTienda(string codTienda)
        {
            string strJson = "";
            JsonResult jRespuesta = null;

            strJson = dat_storeTda.listarStr_DatosTienda(codTienda);
            var serializer = new JavaScriptSerializer();
            jRespuesta = Json(serializer.Deserialize<List<Ent_DatosTienda>>(strJson), JsonRequestBehavior.AllowGet);

            return strJson;
        }
        public string listarStr_DatosXcenterTienda(string codTienda)
        {
            string strJson = "";
            JsonResult jRespuesta = null;

            strJson = dat_storeTda.listarStr_DatosXcenterTienda(codTienda);
            var serializer = new JavaScriptSerializer();
            jRespuesta = Json(serializer.Deserialize<List<Ent_DatosTienda>>(strJson), JsonRequestBehavior.AllowGet);

            return strJson;
        }
        public string listarStr_InterfacexDefecto(string cod_tda)
        {
            string strJson = "";

            strJson = dat_storeTda.listarStr_InterfacexDefecto(cod_tda);
            return strJson;
        }

        public ActionResult VentaHistorica(string cod_Tda, string descTienda)
        {

            Ent_Combo cbo = new Ent_Combo();
            cbo.cbo_codigo = cod_Tda;
            cbo.cbo_descripcion = descTienda;

            return View(cbo);

        }

        #region<Vista Data XCenter>
        public ActionResult ListaTienda_XCenter()
        {
            Dat_XCenter dat_xcenter = new Dat_XCenter();
            dat_xcenter.get_tienda_xcenter("50254");
            //Ent_DatosTienda_Xstore tda_xstore=

            return View();
        }
        #endregion

        #region ***CONFIG CONEXION***

        public ActionResult ConfigConexion()
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
                return View(listar_config_conexion());
            }
        }
        public Ent_ConfigConexion listar_config_conexion()
        {
            Ent_ConfigConexion config_conexion = dat_storeTda.XSTORE_GET_CONEXION_GLOBAL(Session["PAIS"].ToString());
            Session[_session_cc_central_xst] = config_conexion.list_central_xst;
            Session[_session_cc_caja_xst] = config_conexion.list_cajas_xst;
            return config_conexion;
        }
        public ActionResult central_conexion()
        {
            try
            {
                if (Session[_session_cc_central_xst] == null)
                {
                    listar_config_conexion();
                }
                //listar_config_conexion();
                List<Ent_Central_Xst> central_xst = (List<Ent_Central_Xst>)Session[_session_cc_central_xst];
                return Json(new { estado = 1, central_xst = central_xst });
            }
            catch (Exception ex)
            {
                return Json(new { estado = 0, resultados = ex.Message });
            }
        }
        public PartialViewResult _listaCajasXst()
        {
            return PartialView(listar_config_conexion());
        }

        public ActionResult getCajasXstAjax(Ent_jQueryDataTableParams param, string sinConexion)
        {

            if (Session[_session_cc_caja_xst] == null)
            {
                listar_config_conexion();
            }
            IQueryable<Ent_Cajas_Xst> membercol = ((List<Ent_Cajas_Xst>)(Session[_session_cc_caja_xst])).AsQueryable();  //lista().AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<Ent_Cajas_Xst> filteredMembers = membercol;


            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.TIENDA.ToUpper().Contains(param.sSearch.ToUpper()));
            }

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Ent_Cajas_Xst, string> orderingFunction =
            (
            m => m.TIENDA);
            var sortDirection = Request["sSortDir_0"];
            if (sortDirection == "asc")
                filteredMembers = filteredMembers.OrderBy(orderingFunction);
            else
                filteredMembers = filteredMembers.OrderByDescending(orderingFunction);

            if (Convert.ToBoolean(sinConexion))
            {
                filteredMembers = filteredMembers.Where(m => Convert.ToBoolean(m.ESTADO_CONEXION_CAJA_XST) == false);
            }

            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            //displayMembers.Select(a => { a.ESTADO_CONEXION_CAJA_XST = dat_storeTda.PingHost(a.IP); return a; }).ToList();

            var result = from a in displayMembers
                         select new
                         {
                             a.COD_ENTID,
                             a.TIENDA,
                             a.NCAJA,
                             a.IP,
                             a.VERSION_XST,
                             a.ESTADO_CONEXION_CAJA_XST
                         };
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
