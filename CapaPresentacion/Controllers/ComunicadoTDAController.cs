using CapaEntidad.Control;
using CapaEntidad.Util;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class ComunicadoTDAController : Controller
    {
        // GET: ComunicadoTDA
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
                return View();
            }
        }

        [ValidateInput(false)]
        public ActionResult FileManagerPartial()
        {
            /*string _emp = "Emcomer"*/
            ;// "Emcomer";//(string)Session["empresa"];//"Emcomer";//(string)Session["empresa"];//this.Request.Params["Opcion"].ToString() ;
            string _tda = "";
            if (Session["Tienda"] != null)
                _tda = Session["Tienda"].ToString();


            //string _tda = "143";            
            string _folder_root_d = @"~\Files\ComunicadoTDA\";
            string opcion_admin = "0";

            if (_tda.Trim().Length > 0) opcion_admin = "1";
            ViewBag.opcion_admin = opcion_admin;

            if (opcion_admin == "1")
            {
                _folder_root_d = _folder_root_d + "\\" + _tda;

                string _path_Formularios = _folder_root_d + "\\Formularios";

                bool exists = System.IO.Directory.Exists(Server.MapPath(_folder_root_d));

               // bool exists_form = System.IO.Directory.Exists(Server.MapPath(_path_Formularios));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(_folder_root_d));

                //if (!exists_form)
                 //   System.IO.Directory.CreateDirectory(Server.MapPath(_path_Formularios));
            }

            Session["_folder_root_d"] = _folder_root_d;

            return PartialView("_FileManagerPartial", _folder_root_d);
        }

        public FileStreamResult FileManagerPartialDownload()
        {
            return FileManagerExtension.DownloadFiles(ComunicadoTDAControllerFileManagerSettings.CreateFileManagerDownloadSettings(), (string)Session["_folder_root_d"].ToString());
        }
    }
    public class ComunicadoTDAControllerFileManagerSettings
    {
        public static DevExpress.Web.Mvc.FileManagerSettings CreateFileManagerDownloadSettings()
        {
            var settings = new DevExpress.Web.Mvc.FileManagerSettings();

            settings.SettingsEditing.AllowDownload = true;
            settings.Name = "FileManager";
            return settings;
        }
    }

}