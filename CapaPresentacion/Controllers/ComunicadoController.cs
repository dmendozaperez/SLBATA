using DevExpress.Web.Mvc;
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
            return View();
        }

        [ValidateInput(false)]
        public ActionResult FileManagerPartial()
        {
            return PartialView("_FileManagerPartial", ComunicadoControllerFileManagerSettings.Model);
        }

        public FileStreamResult FileManagerPartialDownload()
        {
            return FileManagerExtension.DownloadFiles("FileManager", ComunicadoControllerFileManagerSettings.Model);
        }
    }
    public class ComunicadoControllerFileManagerSettings
    {
        public const string RootFolder = @"~\Files\Comunicado";

        public static string Model { get { return RootFolder; } }
    }

}