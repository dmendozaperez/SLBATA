using CapaDato.GestionInterno;
using CapaDato.Reporte;
using CapaEntidad.Control;
using CapaEntidad.General;
using CapaEntidad.GestionInterno;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.IO;
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
            Session[_session_listcom_private] = null;
            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {
                if (Session["Tienda"] != null)
                {//VLADIMIR
                    ViewBag.Tienda = tienda.get_ListaTiendaXstore(Session["PAIS"].ToString()).Where(t => t.cbo_codigo == Session["Tienda"].ToString()).ToList();
                }
                else
                {
                    ViewBag.Tienda = tienda.get_ListaTiendaXstore(Session["PAIS"].ToString(),true);
                }//VLADIMIR END

                return View();
            }
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
        public ActionResult getComAjax(Ent_jQueryDataTableParams param , string noLeidos)
        {
            
            /*verificar si esta null*/
            if (Session[_session_listcom_private] == null)
            {
                List<Ent_Comunicado> listcom = new List<Ent_Comunicado>();
                if (Session["Tienda"] != null)
                {
                    listcom = listaCom(Session["Tienda"].ToString());
                }                
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

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.descripcion.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.archivo.ToUpper().Contains(param.sSearch.ToUpper()));
            }
            //Manejador de orden
            //0 = ID 
            //5 = fecha envio
            //6 = fecha leido
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Ent_Comunicado, DateTime> orderingFunctionFE =(m => Convert.ToDateTime(m.fecha_hora_mod));
            Func<Ent_Comunicado, decimal> orderingFunctionID =(m => m.id);
            Func<Ent_Comunicado, string> orderingFunctionFL = (m => m.file_leido_fecha);

            var sortDirection = Request["sSortDir_0"];
            Boolean order_colum = false;
            if (param!=null)
            {
                if (param.sEcho != "1") order_colum = true;
            }

            if (order_colum)
            { 
                if (sortDirection == "asc")
                { 
                    if (sortIdx == 0)
                    {
                        filteredMembers = filteredMembers.OrderBy(orderingFunctionID);
                    }
                    else if(sortIdx == 5)
                    {
                        filteredMembers = filteredMembers.OrderBy(orderingFunctionFE);
                    }else
                    {
                        filteredMembers = filteredMembers.OrderBy(orderingFunctionFL);
                    }
                }
                else
                { 
                    if (sortIdx == 0)
                    {
                        filteredMembers = filteredMembers.OrderByDescending(orderingFunctionID);
                    }
                    else if (sortIdx == 5)
                    {
                        filteredMembers = filteredMembers.OrderByDescending(orderingFunctionFE);
                    }
                    else
                    {
                        filteredMembers = filteredMembers.OrderByDescending(orderingFunctionFL);
                    }
                }
            }

            if (Convert.ToBoolean(noLeidos)){
                filteredMembers = filteredMembers.Where(m => m.file_leido == false).OrderByDescending(orderingFunctionFE);
            }
                   

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
                             a.fecha,
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

        public ActionResult LeerComunicado(string id_comunicado , string leido)
        {
            try
            {
                if (Session["Tienda"] == null)
                {
                    return Json(new { estado = 1, resultados = "No Leido" });
                }
                Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
                if (_usuario == null)
                {
                    return RedirectToAction("Login", "Control");
                }
                if (!Convert.ToBoolean(leido))
                {
                    datCom.leer_comunicado(id_comunicado, _usuario.usu_id);
                    Session[_session_listcom_private] = null;
                }
                return Json(new { estado = 1, resultados = "Leido" });
            }
            catch (Exception)
            {
                return Json(new { estado = 0,  resultados = "Error al marcar como leido el comunicado." });
            }
        }
        public ActionResult NotificacionesComunicados(string id_comunicado)
        {
            int no_noti = 0;
            try
            {                
                if (Session["Tienda"] != null)
                {
                    no_noti = datCom.NotificacionesComunicado(Session["Tienda"].ToString());
                }               
                return Json(new { estado = 1, resultados = "correcto" , no_noti = no_noti });
            }
            catch (Exception)
            {
                return Json(new { estado = 0, resultados = "Error al obtener comunicados pendientes.", no_noti = no_noti });
            }
        }
    }
}