
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

                var lista = datInterface.listar_Pais();
               
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
             Ent_ValeCompra _compra = new Ent_ValeCompra();

            Boolean respuesta = datInterface.InsertarInterfaceTienda(_InterfaceTienda);
            
            oJRespuesta.Message = respuesta.ToString();

            return Json(oJRespuesta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenerearArchivoInterface(string Cod_Pais, string Cod_Tda, List<string> listInterface)
        {
            var oJRespuesta = new JsonResponse();
            var oJRpta= new JsonRespuesta();
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];

            oJRpta = datInterface.GenerearArchivoInterface(Cod_Pais, Cod_Tda, listInterface);

            oJRespuesta.Message = oJRpta.Message;
            oJRespuesta.Success = oJRpta.Success;
         

            return Json(oJRespuesta, JsonRequestBehavior.AllowGet);
        }

        public FileResult Download(string strInterface_Tienda)
        {
            string codTienda = "";


            string directorio = System.Web.HttpContext.Current.Server.MapPath(Ent_Conexion.strDirectorio);
            byte[] fileBytes = System.IO.File.ReadAllBytes(directorio + codTienda + ".zip");
            string fileName = "Bata_" + codTienda + ".zip";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }



    }
}