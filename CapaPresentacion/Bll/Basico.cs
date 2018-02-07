using CapaEntidad.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Bll
{
    public class Basico:Controller
    {
        public Boolean AccesoMenu(List<Ent_Menu_Items> menu, Controller cont)
        {
            Boolean valida = false;
            try
            {
                string actionName = cont.ControllerContext.RouteData.GetRequiredString("action");
                string controllerName = cont.ControllerContext.RouteData.GetRequiredString("controller");

                var existe = menu.Where(t => t.action == actionName && t.controller == controllerName).ToList();

                if (existe.Count > 0) valida = true;

            }
            catch
            {

                valida = false;
            }
            return valida;
        }
    }
}