using CapaDato.Maestros;
using CapaDato.Soporte;
using CapaEntidad.Control;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class SoporteController : Controller
    {
        //private Dat_ListaTienda dat_lista_tienda = new Dat_ListaTienda();
        Dat_Tienda_Config tiendaconfig = new Dat_Tienda_Config();
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
    }
}