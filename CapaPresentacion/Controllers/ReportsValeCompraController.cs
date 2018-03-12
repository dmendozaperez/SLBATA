using CapaDato.ValeCompra;
using CapaDato.ReportsValeCompra;
using CapaEntidad.ValeCompra;
using CapaEntidad.ReportsValeCompra;
using CapaEntidad.Control;
using CapaEntidad.Menu;
using CapaEntidad.Util;
using CapaPresentacion.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.IO.Compression;
using System.Text;

namespace CapaPresentacion.Controllers
{
    public class ReportsValeCompraController : Controller
    {
        // GET: ReportsValeCompra
        private Dat_ValeCompra datvalecompra = new Dat_ValeCompra();
        private Dat_ReportValeCompra datReportvalecompra = new Dat_ReportValeCompra();
        private Dat_Cliente datCliente = new Dat_Cliente();
        private string _session_listValeCompraDetalle_private = "_session_listValeCompraDetalle_private";

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
                #region<VALIDACION DE ROLES DE USUARIO>
                Boolean valida_rol = true;
                Basico valida_controller = new Basico();
                List<Ent_Menu_Items> menu = (List<Ent_Menu_Items>)Session[Ent_Global._session_menu_user];
                valida_rol = valida_controller.AccesoMenu(menu, this);
                #endregion
                if (valida_rol)
                {
                    ViewBag.cliente = datCliente.get_lista();
                    return View(lista("",""));
                }
                else
                {
                    return RedirectToAction("Login", "Control", new { returnUrl = return_view });
                }
            }
        }

        public PartialViewResult ListaValeCompra(string []dwcliente, string  estado)
        {
            Reporte_Filtro filtro = new Reporte_Filtro();
            string strListRuc = "";
            if(estado == null)
                estado = "";

            if (dwcliente == null)
                strListRuc = "";
            else {

                for (int i = 0; i < dwcliente.Length; i++) {
                    strListRuc = strListRuc + ',' + dwcliente[i];
                }
                strListRuc = strListRuc.Substring(1);

            }

            filtro.report_Estado = estado;
            filtro.report_listRuc = "";

            return PartialView(lista(strListRuc, estado));
        }

        public List<Reporte_Resultado> lista(string rucCliente, string estado)//(Reporte_Filtro filtro)
        {
            Reporte_Filtro filtro =  new Reporte_Filtro();
            filtro.report_listRuc = rucCliente;
            filtro.report_Estado = estado;
            List<Reporte_Resultado> list = datReportvalecompra.listarReporte(filtro);
            Session[_session_listValeCompraDetalle_private] = list;
            return list;
        }
    }
}