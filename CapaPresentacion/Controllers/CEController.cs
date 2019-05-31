using CapaDato.Maestros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class CEController : Controller
    {
        // GET: CE
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EnvInterXstore()
        {            
            Dat_Orce get_orc = new Dat_Orce();
            ViewBag.cadena= get_orc.lista_cadena();
            ViewBag.tipotda = get_orc.lista_tipotda();
            ViewBag.tienda = get_orc.lista_tda_cadenatipo();
            ViewBag.tipointer = get_orc.lista_tipo_interface();

            return View();
        }
    }
}