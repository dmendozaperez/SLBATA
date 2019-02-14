using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class DocumentoController : Controller
    {
        // GET: Documento
        public ActionResult Index()
        {
            return View();
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
            string _folder_root_d = @"~\Files\Documento\";          
            string opcion_admin = "0";
            
            if (_tda.Trim().Length > 0) opcion_admin = "1";
            ViewBag.opcion_admin = opcion_admin;

            if (opcion_admin == "1")
            {
                _folder_root_d = _folder_root_d + "\\" + _tda;

                string _path_Formularios = _folder_root_d + "\\Formularios";

                bool exists = System.IO.Directory.Exists(Server.MapPath(_folder_root_d));

                bool exists_form = System.IO.Directory.Exists(Server.MapPath(_path_Formularios));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(_folder_root_d));

                if (!exists_form)
                    System.IO.Directory.CreateDirectory(Server.MapPath(_path_Formularios));              
            }

            Session["_folder_root_d"] = _folder_root_d;

            return PartialView("_FileManagerPartial", _folder_root_d);           
        }

        public FileStreamResult FileManagerPartialDownload()
        {
            return FileManagerExtension.DownloadFiles(ComunicadoControllerFileManagerSettings.CreateFileManagerDownloadSettings(), (string)Session["_folder_root_d"].ToString());
        }
    }
    public class DocumentoControllerFileManagerSettings
    {
        public const string RootFolder = @"~\Files\Documento";

        public static string Model { get { return RootFolder; } }
    }

}