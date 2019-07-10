using CapaPresentacion.Bll;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using CapaEntidad.Util;
using System.Linq;
using ClosedXML;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using CapaEntidad.Control;
using Data.Crystal.Reporte;
using Models.Crystal.Reporte;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using CapaPresentacion.Models.ECommerce;
using CapaEntidad.ECommerce;

namespace CapaPresentacion.Controllers
{
    public class GuiaECController : Controller
    {

        public string reportServerUrl = Ent_Conexion.servidorReporte;
        public ReportCredentials reportCredential = new ReportCredentials(Ent_Conexion.usuarioReporte, Ent_Conexion.passwordReporte, Ent_Conexion.dominioReporte);
        public string reportFolder = Ent_Conexion.CarpetaPlanillaReporte;
        private string gcodTda = "";
        private DataTable dtPromociones = new DataTable();
        private string _session_listcomparativo_private = "_session_listcomparativo_private";
        private string _session_listguia_private = "_session_listguia_private";
        //public ActionResult Index()
        //{
        //    return View();
        //}
        

        [HttpPost]
        public ActionResult ShowGenericReport( string cod_venta)
        {
            //grupo = "0";categoria = "0";subcategoria = "0";estado = "0";
            //   tipoReport = "1";
            Data_Ecommerce pl = new Data_Ecommerce();
            //string nombreReporte = tipoReport == "-1" ? "Planilla.rpt" : "Planilladet.rpt";
            this.HttpContext.Session["ReportName"] = "GuiaECommerce";

            //string tipo_rep = "1";

            List<ECommerce> model_guia = pl.getGuia_EC(cod_venta);
            List<DetallesECommerce> model_detguia = pl.get_DetGuia_EC(cod_venta);

            this.HttpContext.Session["rptSource"] = model_guia;
            this.HttpContext.Session["rptSource_sub"] = model_detguia;

            /*error=0;exito=1*/
            string _estado = (model_guia == null) ? "0" : "1";

            //if (model_planilla==null)

            return Json(new
            {
                estado = _estado
            });
        }
        
    }

}