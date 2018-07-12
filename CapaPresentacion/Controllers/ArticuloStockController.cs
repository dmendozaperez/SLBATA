using CapaDato.ValeCompra;
using CapaDato.ReportsValeCompra;
using CapaEntidad.ValeCompra;
using CapaEntidad.ArticuloStock;
using CapaEntidad.Control;
using CapaDato.Control;
using CapaEntidad.Menu;
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

namespace CapaPresentacion.Controllers
{
    public class ArticuloStockController : Controller
    {
     
    
        private Dat_ArticuloStock datArticuloStock = new Dat_ArticuloStock();
     
        private string _session_liststockArticulo = "_session_listArticuloTienda";

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
                #region<VALIDACION DE ROLES DE USUARIO>
                Boolean valida_rol = true;
                Basico valida_controller = new Basico();
                List<Ent_Menu_Items> menu = (List<Ent_Menu_Items>)Session[Ent_Global._session_menu_user];
                valida_rol = valida_controller.AccesoMenu(menu, this);
                #endregion
                if (valida_rol)
                {
                    var lista = datArticuloStock.listar_Departamento();
                    var obj = lista[0];
                    List<Departamento> listobj = new List<Departamento>();
                    listobj.Add(obj);

                    ViewBag.listDepartamento = lista;
                    ViewBag.General = listobj;
                    ViewBag.Usuario = _usuario.usu_nombre;



                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Control", new { returnUrl = return_view });
                }
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
                    strJson = datArticuloStock.listarStr_Provincia(Params);
                    jRespuesta = Json(serializer.Deserialize<List<Provincia>>(strJson), JsonRequestBehavior.AllowGet);
                    break;
                case 2:
                    String[] substrings = Params.Split('|');
                    strJson = datArticuloStock.listarStr_Distrito(substrings[0].ToString(), substrings[1].ToString());
                    jRespuesta = Json(serializer.Deserialize<List<Distrito>>(strJson), JsonRequestBehavior.AllowGet);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            return jRespuesta;
        }

        public JsonResult UpdateStats()
        {
            Dat_Usuario _usuario = new Dat_Usuario();
            Ent_Usuario _data_user = _usuario.get_login("Invitado");
            Session[Ent_Constantes.NameSessionUser] = _data_user;

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public List<Articulo_Stock_Tienda> lista_ArticuloStockTienda(string codArticulo)
        {

            List<Articulo_Stock_Tienda> list = datArticuloStock.listar_ArticuloStock(codArticulo);
            
            Session[_session_liststockArticulo] = list;
            return list;
        }

        public string listarStr_ArticuloStock(string codArticulo, string CodDpto, string CodPrv, string CodDist)
        {
            string strJson = "";
            JsonResult jRespuesta = null;
            strJson = datArticuloStock.listarStr_ArticuloStock(codArticulo, CodDpto, CodPrv, CodDist);
            var serializer = new JavaScriptSerializer();
            jRespuesta = Json(serializer.Deserialize<List<Articulo_Stock_Tienda>>(strJson), JsonRequestBehavior.AllowGet);

            return strJson;
        }


    }
}