using CapaPresentacion.Bll;
using CapaDato.Reporte;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using CapaEntidad.Util;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using CapaEntidad.Control;
using Data.Crystal.Reporte;
using Models.Crystal.Reporte;

namespace CapaPresentacion.Controllers
{
    public class ReporteController : Controller
    {
       
        public string reportServerUrl = Ent_Conexion.servidorReporte;
        public ReportCredentials reportCredential = new ReportCredentials(Ent_Conexion.usuarioReporte, Ent_Conexion.passwordReporte, Ent_Conexion.dominioReporte);
        public string reportFolder = Ent_Conexion.CarpetaPlanillaReporte;
        private string gcodTda = "";
        private Dat_Combo datCbo = new Dat_Combo();
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult ReportePlanilla()
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
                List<Ent_Combo> list = new List<Ent_Combo>();
                Ent_Combo entCombo = new Ent_Combo();
                entCombo.cbo_codigo = "0";
                entCombo.cbo_descripcion = "----Todos----";
                list.Add(entCombo);
                ViewBag.Categoria = list;

                
                ViewBag.Title = "Reporte de Planilla";
       
                ViewBag.Tipo = datCbo.get_ListaTipoCategoria();              

                ViewBag.Estado = datCbo.get_ListaEstado();

                if (Session["Tienda"]!=null)
                {
                    ViewBag.Tienda = datCbo.get_ListaTiendaXstore().Where(t => t.cbo_codigo == Session["Tienda"].ToString()).ToList();
                }
                else
                {
                    ViewBag.Tienda = datCbo.get_ListaTiendaXstore();
                }

                
                

                string strJson = "";
                JsonResult jRespuesta = null;
                var serializer = new JavaScriptSerializer();


                strJson = datCbo.listarStr_ListaGrupoTipo();
                jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson), JsonRequestBehavior.AllowGet);
                ViewBag.ClGrupo = jRespuesta;

                strJson = datCbo.listarStr_ListaCategoria("");
                jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson), JsonRequestBehavior.AllowGet);
                ViewBag.ClCategoria = jRespuesta;                          

                strJson = datCbo.listarStr_ListaSubCategoria("");
                jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson), JsonRequestBehavior.AllowGet);
                ViewBag.ClSBCategoria = jRespuesta;

                return View();
            }

           

            //Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            //string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            //string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            //string return_view = actionName + "|" + controllerName;

            //if (_usuario == null)
            //{
            //    return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            //}
            //else
            //{
            //    List<Ent_Combo> list = new List<Ent_Combo>();
            //    Ent_Combo entCombo = new Ent_Combo();
            //    entCombo.cbo_codigo = "0";
            //    entCombo.cbo_descripcion = "----Todos----";
            //    list.Add(entCombo);
            //    ViewBag.Title = "Reporte de Planilla";
            //    ViewBag.Grupo = datCbo.get_ListaGrupo();
            //    ViewBag.Estado = datCbo.get_ListaEstado();
            //    ViewBag.Tienda = datCbo.get_ListaTiendaXstore();
            //    ViewBag.Categoria = list;

            //    return View();
            //}
        }
        //public ActionResult ReportePlanillaTdaView()
        //{

        //    Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
        //    string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
        //    string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
        //    string return_view = actionName + "|" + controllerName;

        //    if (_usuario == null)
        //    {
        //        return RedirectToAction("Login", "Control", new { returnUrl = return_view });
        //    }
        //    else
        //    {
        //        gcodTda = (String)Session["Tienda"];
        //        List<Ent_Combo> list = new List<Ent_Combo>();
        //        Ent_Combo entCombo = new Ent_Combo();
        //        entCombo.cbo_codigo = "0";
        //        entCombo.cbo_descripcion = "----Todos----";
        //        list.Add(entCombo);
        //        ViewBag.Title = "Reporte de Planilla";
        //        ViewBag.Grupo = datCbo.get_ListaGrupo();
        //        ViewBag.Estado = datCbo.get_ListaEstado();
        //        ViewBag.Categoria = list;
        //        return View();
        //    }

        //}
        //public JsonResult GenerarCombo(int Numsp, string Params)
        //{
        //    string strJson = "";
        //    JsonResult jRespuesta = null;
        //    var serializer = new JavaScriptSerializer();


        //    switch (Numsp)
        //    {
        //        case 1:
        //            strJson = datCbo.listarStr_ListaCategoria(Params);
        //            jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson), JsonRequestBehavior.AllowGet);
        //            break;
        //        case 2:
        //            strJson = datCbo.listarStr_ListaSubCategoria(Params);
        //            jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson), JsonRequestBehavior.AllowGet);
        //            break;
        //        default:
        //            Console.WriteLine("Default case");
        //            break;
        //    }
        //    return jRespuesta;
        //}

        //public ActionResult ReportePlanilla(string tda, string grupo, string cate, string subcate, string estado)
        //{

        //    Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
        //    string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
        //    string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
        //    string return_view = actionName + "|" + controllerName;

        //    string gcodTda = (String)Session["Tienda"];
        //    if (gcodTda != "" && gcodTda != null)
        //    {
        //        tda = gcodTda;
        //    }

        //    if (_usuario == null)
        //    {
        //        return RedirectToAction("Login", "Control", new { returnUrl = return_view });
        //    }
        //    else
        //    {
        //        string Param_tienda = (tda == "") ? "" : tda;
        //        string Param_grupo = (grupo == "") ? "" : grupo;
        //        string Param_cate = (cate == "") ? "0" : cate;
        //        string Param_subcate = (subcate == "") ? "0" : subcate;
        //        string Param_estado = (estado == "") ? "0" : estado;

        //        gcodTda = (String)Session["Tienda"];

        //        if (gcodTda != "" && gcodTda != null)
        //        {
        //            Param_tienda = gcodTda;
        //        }
        //        //Param_tienda = "50143";
        //        List<ReportParameter> paramList = new List<ReportParameter>();
        //        paramList.Add(new ReportParameter("cod_tda", Param_tienda, false));
        //        paramList.Add(new ReportParameter("Grupo", Param_grupo, false));
        //        paramList.Add(new ReportParameter("Categoria", Param_cate, false));
        //        paramList.Add(new ReportParameter("SubCategoria", Param_subcate, false));
        //        paramList.Add(new ReportParameter("Estado", Param_estado, false));

        //        // ** Procesar Reporte en Servidor
        //        ReportViewer reportViewer = new ReportViewer();
        //        reportViewer.ProcessingMode = ProcessingMode.Remote;
        //        reportViewer.SizeToReportContent = true;
        //        reportViewer.ZoomMode = ZoomMode.FullPage;
        //        reportViewer.Width = Unit.Percentage(300);
        //        reportViewer.Height = Unit.Percentage(300);
        //        reportViewer.AsyncRendering = true;

        //        reportViewer.ShowPrintButton = true;

        //        reportViewer.ServerReport.ReportServerCredentials = reportCredential;
        //        reportViewer.ServerReport.ReportServerUrl = new Uri(reportServerUrl);
        //        reportViewer.ServerReport.ReportPath = "/" + reportFolder + "/Rpt_PlanillaWeb";
        //        reportViewer.ShowParameterPrompts = false;
        //        reportViewer.ServerReport.SetParameters(paramList);
        //        reportViewer.ServerReport.Refresh();
        //        ViewBag.ReportViewer = reportViewer;
        //        ViewBag.Title = "Reporte de Planilla";

        //        return View();
        //    }

        //}
        public JsonResult GenerarCombo(int Numsp, string Params)
        {
            string strJson = "";
            JsonResult jRespuesta = null;
            var serializer = new JavaScriptSerializer();


            switch (Numsp)
            {
                case 1:
                    strJson = datCbo.listarStr_ListaCategoria(Params);
                    jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson), JsonRequestBehavior.AllowGet);
                    break;
                case 2:
                    strJson = datCbo.listarStr_ListaSubCategoria(Params);
                    jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson), JsonRequestBehavior.AllowGet);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            return jRespuesta;
        }
        [HttpPost]
        public ActionResult ShowGenericReportInNewWin(string cod_tda, string grupo, string categoria, string subcategoria, string estado,string tipo)
        {
            //grupo = "0";categoria = "0";subcategoria = "0";estado = "0";
            Data_Planilla pl = new Data_Planilla();
            this.HttpContext.Session["ReportName"] = "Planilla.rpt";

            List<Models_Planilla> model_planilla= pl.get_planilla(cod_tda, grupo, categoria, subcategoria, estado, tipo);

            this.HttpContext.Session["rptSource"] = model_planilla;
            this.HttpContext.Session["rptSource_sub"] = pl.get_reglamed_cab();

            /*error=0;exito=1*/
            string _estado = (model_planilla==null)?"0":"1";

            //if (model_planilla==null)

            return Json(new
            {                
                estado = _estado
            });
        }

        public ActionResult ReporteVendedor()
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

                ViewBag.Title = "Reporte Vendedor";            

                if (Session["Tienda"] != null)
                {
                    ViewBag.Tienda = datCbo.get_ListaTiendaXstoreActivo(Session["Tienda"].ToString());
                }
                else
                {
                    ViewBag.Tienda = datCbo.get_ListaTiendaXstoreActivo("");
                }

                return View();
            }
           
        }

        [HttpPost]
        public ActionResult ShowGenericReportVendedorInNewWin(string cod_tda, string fecIni, string FecFin)
        {
            //grupo = "0";categoria = "0";subcategoria = "0";estado = "0";
            Data_Planilla pl = new Data_Planilla();
            this.HttpContext.Session["ReportName"] = "Vendedor.rpt";

            List<Models_Vendedor> model_vendedor = pl.get_reporteVendedor(cod_tda, fecIni, FecFin);

            this.HttpContext.Session["rptSource"] = model_vendedor;
           

            /*error=0;exito=1*/
            string _estado = (model_vendedor == null) ? "0" : "1";

            //if (model_planilla==null)

            return Json(new
            {
                estado = _estado
            });
        }

        public ActionResult ReporteArticuloSinMov()
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

                ViewBag.Title = "Reporte Articulo sin movimiento";

                if (Session["Tienda"] != null)
                {
                    ViewBag.Tienda = datCbo.get_ListaTiendaXstoreActivo(Session["Tienda"].ToString());
                }
                else
                {
                    ViewBag.Tienda = datCbo.get_ListaTiendaXstoreActivo("");
                }

                //ViewBag.Cadena = datCbo.get_ListaCadena();

                return View();
            }

        }

        [HttpPost]
        public ActionResult ShowGenericReportArtSinMovInNewWin(string cod_cadena, string cod_tda, Int32 nsemana, Int32 maxpares)
        {
            //grupo = "0";categoria = "0";subcategoria = "0";estado = "0";
            Data_Bata pl = new Data_Bata();
            this.HttpContext.Session["ReportName"] = "ReportArtSinMov.rpt";

            List<Models_Art_Sin_Mov> model_Art_sn_mov = pl.list_art_sin_mov(cod_cadena, cod_tda, nsemana, maxpares);

            this.HttpContext.Session["rptSource"] = model_Art_sn_mov;


            /*error=0;exito=1*/
            string _estado = (model_Art_sn_mov == null) ? "0" : "1";

            //if (model_planilla==null)

            return Json(new
            {
                estado = _estado
            });
        }
    }
}