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

namespace CapaPresentacion.Controllers
{
    public class ReporteController : Controller
    {
        public string reportServerUrl = "http://posperu.bgr.pe:80/BataRptSrv/";
        public ReportCredentials reportCredential = new ReportCredentials("ReportBata", "Bata2018**", "BataRptSrv");
        public string reportFolder = "ReportBata";
        private string gcodTda = "";
        private Dat_Combo datCbo = new Dat_Combo();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReporteVentasView()
        {
            List<Ent_Combo> list = new List<Ent_Combo>();
            Ent_Combo entCombo = new Ent_Combo();
            entCombo.cbo_codigo = "0";
            entCombo.cbo_descripcion = "----Todos----";
            list.Add(entCombo);
            ViewBag.Title = "Reporte de Planilla";
            ViewBag.Grupo =  datCbo.get_ListaGrupo();
            ViewBag.Estado = datCbo.get_ListaEstado();
            ViewBag.Categoria = list;

            return View();
        }

        public ActionResult ReportePlanillaTdaView()
        {
            gcodTda = (String)Session["Tienda"];
            List<Ent_Combo> list = new List<Ent_Combo>();
            Ent_Combo entCombo = new Ent_Combo();
            entCombo.cbo_codigo = "0";
            entCombo.cbo_descripcion = "----Todos----";
            list.Add(entCombo);
            ViewBag.Title = "Reporte de Planilla";
            ViewBag.Grupo = datCbo.get_ListaGrupo();
            ViewBag.Estado = datCbo.get_ListaEstado();
            ViewBag.Categoria = list;

            return View();
        }


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

        public ActionResult ReporteVentas(string tda, string grupo, string cate, string subcate, string estado)
        {
                   
            // ** Procesar Parametros
            string Param_tienda = (tda == "") ? "" : tda;
            string Param_grupo = (grupo == "") ? "" : grupo;
            string Param_cate = (cate == "") ? "" : cate;
            string Param_subcate = (subcate == "") ? "" : subcate;
            string Param_estado = (estado == "") ? "" : estado;
            gcodTda = (String)Session["Tienda"];

            if (gcodTda != "" && gcodTda!=null) {
                Param_tienda = gcodTda;
            }
            //Param_tienda = "50143";
            List<ReportParameter> paramList = new List<ReportParameter>();
            paramList.Add(new ReportParameter("cod_tda", Param_tienda, false));
            paramList.Add(new ReportParameter("Grupo", Param_grupo, false));
            paramList.Add(new ReportParameter("Categoria", Param_cate, false));
            paramList.Add(new ReportParameter("SubCategoria", Param_subcate, false));
            paramList.Add(new ReportParameter("Estado", Param_estado, false));
        
            // ** Procesar Reporte en Servidor
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;
            reportViewer.SizeToReportContent = true;
            reportViewer.ZoomMode = ZoomMode.FullPage;
            reportViewer.Width = Unit.Percentage(300);
            reportViewer.Height = Unit.Percentage(300);
            reportViewer.AsyncRendering = true;

            reportViewer.ServerReport.ReportServerCredentials = reportCredential;
            reportViewer.ServerReport.ReportServerUrl = new Uri(reportServerUrl);
            reportViewer.ServerReport.ReportPath = "/" + reportFolder + "/Rpt_PlanillaWeb";
            reportViewer.ShowParameterPrompts = false;
            reportViewer.ServerReport.SetParameters(paramList);
            reportViewer.ServerReport.Refresh();
            ViewBag.ReportViewer = reportViewer;
            ViewBag.Title = "Reporte de Planilla";
            return View();
        }


    }
}