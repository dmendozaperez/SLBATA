using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapaPresentacion.AspNetForms
{
    public partial class KeyCategoryPerformance : System.Web.UI.Page
    {
        ReportDocument rd = null;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                string strReportName = System.Web.HttpContext.Current.Session["ReportName"].ToString();
                var rptSource = System.Web.HttpContext.Current.Session["rptSource"];


                List<CapaPresentacion.Models.Crystal.Reporte.Key_Category_Performance> model_Art_sn_mov = (List<CapaPresentacion.Models.Crystal.Reporte.Key_Category_Performance>)rptSource;

                string TIENDA_DES = "";
                string COD_CADENA = "";
                //if (model_Art_sn_mov.Count > 0)
                //{
                //    TIENDA_DES = model_Art_sn_mov[0].TIENDA_DES;
                //    COD_CADENA = model_Art_sn_mov[0].COD_CADENA;
                //}


                rd = new ReportDocument();



                string strRptPath = Server.MapPath("~/") + "RptsCrystal//" + strReportName;
                //Loading Report
                rd.Load(strRptPath);

                // Setting report data source
                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                    rd.SetDataSource(rptSource);
               // rd.SetParameterValue("tienda", TIENDA_DES);


                crv_key_category_performance.ReportSource = rd;
                crv_key_category_performance.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                crv_key_category_performance.HasToggleGroupTreeButton = false;
            }
            catch (Exception exc)
            {


            }
        }
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
        protected void Page_Unload(object sender, EventArgs e)
        {
            /// Reporte generalizado
            if ((rd != null) && rd.IsLoaded)
            {
                rd.Close();
                rd.Dispose();
            }
        }


    }
}