using CapaDato.Comunicado;
using CapaEntidad.Control;
using CapaEntidad.Util;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class ComunicadoController : Controller
    {
        // GET: Comunicado
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
            //return View();
        }

        [ValidateInput(false)]
        public ActionResult FileManagerPartial()
        {           
            /*string _emp = "Emcomer"*/;// "Emcomer";//(string)Session["empresa"];//"Emcomer";//(string)Session["empresa"];//this.Request.Params["Opcion"].ToString() ;
            string _tda = "";
            if (Session["Tienda"] != null)
                _tda = Session["Tienda"].ToString();
            

            //string _tda = "143";
            string _cadena = "";
            string _folder_root = @"~\Files\Comunicado\";
            Modulo opt_folder = new Modulo();
            string _emp = opt_folder.get_empresa_comunicado(_tda); //"Emcomer";
            Boolean _BG_WB=opt_folder._existe_bg_bw(_tda, ref _cadena);
            string opcion_admin = "0";

            string _tipo_tda = "0";

            if (_tda.Trim().Length > 0) opcion_admin = "1";
            ViewBag.opcion_admin = opcion_admin;

            if (opcion_admin=="1")
            {            
                if (_BG_WB)
                {
                    _tipo_tda = "BGBW";
                    _emp = "Tiendas Especializadas";
                    _folder_root = _folder_root + _emp;
                    ViewBag.cadena = _cadena;            
                }
                else
                {

                    _tipo_tda = "BATA";
                    _folder_root = _folder_root + _emp;                
                    if (_emp.ToUpper() == "Emcomer".ToUpper())
                    {
                        string _estado_acceso = opt_folder._estado_acceso(_tda);                                        
                        ViewBag.estado_acceso = _estado_acceso;
                    }
                }
                ViewBag.empresa = _emp;
                ViewBag.tipo_tda = _tipo_tda;
            }
       
            Session["_folder_root"] = _folder_root;       

            return PartialView("_FileManagerPartial", _folder_root);
        }

        public FileStreamResult FileManagerPartialDownload()
        {
            return FileManagerExtension.DownloadFiles(ComunicadoControllerFileManagerSettings.CreateFileManagerDownloadSettings(), (string)Session["_folder_root"].ToString());
        }

    }
    public class ComunicadoControllerFileManagerSettings
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