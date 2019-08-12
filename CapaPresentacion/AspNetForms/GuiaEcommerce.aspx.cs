using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace CapaPresentacion.AspNetForms
{
    public partial class GuiaEC : System.Web.UI.Page
    {
        ReportDocument rd = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                string strReportName = "GuiaECommerce.rpt";                 //System.Web.HttpContext.Current.Session["ReportName"].ToString();
                var rptSource = System.Web.HttpContext.Current.Session["rptSource"];
                var rptSource_sub= System.Web.HttpContext.Current.Session["rptSource_sub"];

                rd = new ReportDocument();
                
                string strRptPath = Server.MapPath("~/") + "RptsCrystal//" + strReportName;
                //Loading Report
                rd.Load(strRptPath);

                // Setting report data source
                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                    rd.SetDataSource(rptSource);
                    rd.OpenSubreport("GuiaECDet").SetDataSource(rptSource_sub);

                crv_guiaEC.ReportSource = rd;
                crv_guiaEC.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                crv_guiaEC.HasToggleGroupTreeButton = false;


                //CrystalReportViewer1.RefreshReport();

                //Session["ReportName"] = "";
                ////Session["rptFromDate"] = "";
                ////Session["rptToDate"] = "";
                //Session["rptSource"] = "";

            }
            catch (Exception exc)
            {


            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            /// Reporte generalizado
            if ((rd != null) && rd.IsLoaded)
            {
                rd.Close();
                rd.Dispose();
            }
        }

        //protected void print_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string strReportName = System.Web.HttpContext.Current.Session["ReportName"].ToString();
        //        var rptSource = System.Web.HttpContext.Current.Session["rptSource"];
        //        var rptSource_sub = System.Web.HttpContext.Current.Session["rptSource_sub"];

        //        rd = new ReportDocument();

        //        string strRptPath = Server.MapPath("~/") + "RptsCrystal//" + strReportName;
        //        //Loading Report
        //        rd.Load(strRptPath);

        //        // Setting report data source
        //        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
        //            rd.SetDataSource(rptSource);
        //        rd.OpenSubreport("rmedida").SetDataSource(rptSource_sub);
        //        //rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, this.Response, true, "lider_liq_x_Grupo");

        //        //rd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.WordForWindows, vrutaserver);
        //        //if (!string.IsNullOrEmpty(strFromDate))
        //        //    rd.SetParameterValue("fromDate", strFromDate);
        //        //if (!string.IsNullOrEmpty(strToDate))
        //        //    rd.SetParameterValue("toDate", strFromDate);
        //        crv_planilla.ReportSource = rd;
        //        crv_planilla.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
        //        crv_planilla.HasToggleGroupTreeButton = false;
        //        //CrystalReportViewer1.RefreshReport();

        //        //Session["ReportName"] = "";
        //        ////Session["rptFromDate"] = "";
        //        ////Session["rptToDate"] = "";
        //        //Session["rptSource"] = "";

        //    }
        //    catch (Exception exc)
        //    {


        //    }
        //}
        protected void Print(object sender, EventArgs e)
        {
            
            rd.Refresh();            
            rd.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
            // Set Paper Size.
            rd.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
            
            rd.PrintOptions.PrinterName = GetDefaultPrinter();
            rd.PrintToPrinter(1, true, 0, 0);
        }
        private string GetDefaultPrinter()
        {
            PrinterSettings settings = new PrinterSettings();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                settings.PrinterName = printer;
                if (settings.IsDefaultPrinter)
                {
                    return printer;
                }
            }
            return string.Empty;
        }
    }
}