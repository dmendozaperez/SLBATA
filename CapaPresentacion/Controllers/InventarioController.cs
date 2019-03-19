
using CapaEntidad.ArticuloSustituto;
using CapaEntidad.Control;
using CapaDato.Control;
using CapaDato.Inventario;
using CapaEntidad.Menu;
using CapaEntidad.Util;
using CapaPresentacion.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CapaDato.Menu;
using CapaDato.Articulosustituto;
using CapaDato.Reporte;
using CapaEntidad.ValeCompra;

namespace CapaPresentacion.Controllers
{
    public class InventarioController : Controller
    {
        private Dat_Combo datCbo = new Dat_Combo();
        private Dat_Inventario datInventario = new Dat_Inventario();
        public ActionResult CorteInventario()
        {
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string codTienda = (String)Session["Tienda"];

            if ((_usuario == null && codTienda == null)||(_usuario.usu_tip_id!="01" && codTienda == null))
            {
                return RedirectToAction("Login", "Control", new { returnUrl = "" });
            }
            else
            {
                codTienda = _usuario.usu_tip_id == "01" ? "" : codTienda;
                ViewBag.CodTienda = codTienda;
                ViewBag.ListTienda = datCbo.get_Listar_TiendaXstore(codTienda);
                return View();
            }
        }


        public ActionResult GenerarCorteInventario(string codTda, string descripcion)
        {            
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string[] nroInventario;

            nroInventario = datInventario.Registrar_CorteInventario(codTda, descripcion,_usuario.usu_id);

            var oJRespuesta = new JsonResponse();

            if ((nroInventario[0]).ToString() != "-1")
            {
                oJRespuesta.Message = (nroInventario[0]).ToString();
                oJRespuesta.Data = true;
                oJRespuesta.Success = true;
            }
            else
            {

                oJRespuesta.Message = (nroInventario[1]).ToString();
                oJRespuesta.Data = false;
                oJRespuesta.Success = false;
            }

            return Json(oJRespuesta, JsonRequestBehavior.AllowGet);
        }



    }
}