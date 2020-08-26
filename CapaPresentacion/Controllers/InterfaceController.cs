
using CapaDato.Interface;
using CapaEntidad.ValeCompra;
using CapaEntidad.ArticuloStock;
using CapaDato.Control;
using CapaEntidad.Menu;
using CapaEntidad.Control;
using CapaEntidad.Interface;
using CapaEntidad.Util;
using CapaPresentacion.Bll;
using System;
using CapaEntidad.General;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.IO.Compression;
using System.Text;
using System.Web.Script.Serialization;
using CapaDato.Menu;
using System.Data;

namespace CapaPresentacion.Controllers
{
    public class InterfaceController : Controller
    {

        private Dat_Interface datInterface = new Dat_Interface();

        private string _session_listInterface = "_session_listInterface";

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
                var lista = datInterface.listar_Pais(Session["PAIS"].ToString());
                ViewBag.listPais = lista;
                return View();
            }
        }

        public JsonResult GenerarCombo(int Numsp, string Params)
        {
            string strJson = "";
            JsonResult jRespuesta = null;
            var serializer = new JavaScriptSerializer();


            switch (Numsp)
            {
                case 1:
                    strJson = datInterface.listarStr_Tienda(Params);
                    jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson), JsonRequestBehavior.AllowGet);
                    break;

                default:
                    Console.WriteLine("Default case");
                    break;
            }
            return jRespuesta;
        }

        public string listarStr_Interface()
        {
            string strJson = "";
            JsonResult jRespuesta = null;
            strJson = datInterface.listarStr_Interface();

            return strJson;
        }


        public JsonResult GuardarInterface(Ent_TiendaInterface _InterfaceTienda)
        {
            var oJRespuesta = new JsonResponse();
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];

            _InterfaceTienda.IdUsu = _usuario.usu_id;

            Boolean respuesta = datInterface.InsertarInterfaceTienda(_InterfaceTienda);

            oJRespuesta.Message = respuesta.ToString();
            oJRespuesta.Success = respuesta;

            return Json(oJRespuesta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenerarArchivoInterface(string Cod_Pais, List<string> listTienda, List<string> listInterface)
        {
            var oJRespuesta = new JsonResponse();
            var oJRpta = new JsonRespuesta();
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            DataTable dt_item = null;
            DataTable dt_price_item = null;
            DataTable dt_item_images = null;
            foreach (string Cod_Tda in listTienda)
            {
                string ruta1 = System.Web.HttpContext.Current.Server.MapPath(Ent_Conexion.strDirectorio_Interface);
                oJRpta = datInterface.GenerarArchivoInterface(Cod_Pais, Cod_Tda, listInterface, ruta1, ref dt_item, ref dt_price_item, ref dt_item_images);
                //if (oJRpta.Success) {

                //    string strDirectorio = oJRpta.Message;
                //    string startPath = strDirectorio ;
                //    string zipPath = strDirectorio + ".zip";

                //    if (System.IO.File.Exists(zipPath))
                //    {
                //        System.IO.File.Delete(zipPath);
                //    }

                //    ZipFile.CreateFromDirectory(startPath, zipPath);           
                //    System.IO.Directory.Delete(startPath, true);
                //}
            }


            string strDirectorio = Ent_Conexion.strDirectorio_Interface;
            string startPath = System.Web.HttpContext.Current.Server.MapPath(strDirectorio.Remove(strDirectorio.Length - 1));
            string zipPath = startPath + ".zip";
            string ruta = zipPath;

            if (System.IO.File.Exists(zipPath))
            {
                System.IO.File.Delete(zipPath);
            }

            ZipFile.CreateFromDirectory(startPath, zipPath);

            foreach (string Cod_Tda in listTienda)
            {
                string startPathAux = System.Web.HttpContext.Current.Server.MapPath(Ent_Conexion.strDirectorio_Interface + Cod_Tda);
                System.IO.Directory.Delete(startPathAux, true);
            }

            oJRespuesta.Message = oJRpta.Message;
            oJRespuesta.Success = oJRpta.Success;


            return Json(oJRespuesta, JsonRequestBehavior.AllowGet);
        }

        public FileResult Download()
        {
            string directorio = (Ent_Conexion.strDirectorio_Interface);
            directorio = System.Web.HttpContext.Current.Server.MapPath(directorio.Remove(directorio.Length - 1) + ".zip");
            byte[] fileBytes = System.IO.File.ReadAllBytes(directorio);
            if (System.IO.File.Exists(directorio))
            {
                System.IO.File.Delete(directorio + ".zip");
            }
            DateTime thisDay = DateTime.Today;
            var fecha = thisDay.ToString("yyyyMMdd");
            var hora = DateTime.Now.ToString("hhmmss");

            string fileName = "xstore_" + "Interface_" + fecha + "_" + hora + ".zip";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);


        }



    }
}