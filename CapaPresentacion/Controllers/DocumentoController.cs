using CapaDato.Contabilidad;
using CapaEntidad.Contabilidad;
using CapaEntidad.Control;
using CapaEntidad.Util;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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
          

                //foreach(var sem in listar_semana)
                //{
                //    string fol_sem = sem;
                //}


                /*string _emp = "Emcomer"*/
                ;// "Emcomer";//(string)Session["empresa"];//"Emcomer";//(string)Session["empresa"];//this.Request.Params["Opcion"].ToString() ;
            string _tda = "";
            if (Session["Tienda"] != null)
                _tda = Session["Tienda"].ToString();
           // _tda = "50147";

            //string _tda = "143";            
            string _folder_root_d = @"~\Files\Documento\";          
            string opcion_admin = "0";
            
            if (_tda.Trim().Length > 0) opcion_admin = "1";
            ViewBag.opcion_admin = opcion_admin;

            if (opcion_admin == "1")
            {
                _folder_root_d = _folder_root_d + "\\" + _tda;
             
                //string _path_Formularios = _folder_root_d + "\\Formularios";


                #region<CREAR FOLDER>
                    Dat_Folder_Documento dat_folder = new Dat_Folder_Documento();
                    List<Ent_Folder_Documento> listar_folder = dat_folder.listar_folder();
                    foreach (var item_sem in listar_folder.GroupBy(t => new { t.cod_semana }).Select(g => new { cod_semana = g.Key.cod_semana }))
                    {                      
                        string cod_Semana = item_sem.cod_semana;

                        foreach(var padre in listar_folder.Where(s=>s.cod_semana==item_sem.cod_semana && s.Fol_Padre.Length==0))
                        {
                            string _str_ruta = _folder_root_d + "\\" + cod_Semana + "\\" + padre.Fol_Des;
                            foreach (var hijo in listar_folder.Where(s => s.Fol_Padre == padre.Fol_id && s.cod_semana==item_sem.cod_semana))
                            {
                                _str_ruta = _folder_root_d + "\\" + cod_Semana + "\\" + padre.Fol_Des + "\\" + hijo.Fol_Des;
                                if (!Directory.Exists(Server.MapPath(_str_ruta))) Directory.CreateDirectory(Server.MapPath(_str_ruta));
                            }                   
                            
                            if (!Directory.Exists(Server.MapPath(_str_ruta))) Directory.CreateDirectory(Server.MapPath(_str_ruta));
                        }

                        

                    }
                #endregion


                //bool exists = System.IO.Directory.Exists(Server.MapPath(_folder_root_d));

                //bool exists_form = System.IO.Directory.Exists(Server.MapPath(_path_Formularios));

                //if (!exists)
                 //   System.IO.Directory.CreateDirectory(Server.MapPath(_folder_root_d));

                //if (!exists_form)
                //    System.IO.Directory.CreateDirectory(Server.MapPath(_path_Formularios));              
            }

            Session["_folder_root_d"] = _folder_root_d;

            return PartialView("_FileManagerPartial", _folder_root_d);           
        }

        public FileStreamResult FileManagerPartialDownload()
        {
            return FileManagerExtension.DownloadFiles(DocumentoControllerFileManagerSettings.CreateFileManagerDownloadSettings(), (string)Session["_folder_root_d"].ToString());
        }
    }
    public class DocumentoControllerFileManagerSettings
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