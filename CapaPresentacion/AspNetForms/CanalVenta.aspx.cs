using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CapaPresentacion.AspNetForms
{
    public partial class CanalVenta : System.Web.UI.Page
    {
        ReportDocument rd = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strReportName = "CanalVenta.rpt";
                var rptSource = System.Web.HttpContext.Current.Session["rptSource"];


                rd = new ReportDocument();

                string strRptPath = Server.MapPath("~/") + "RptsCrystal//" + strReportName;

                rd.Load(strRptPath);

                // Setting report data source
                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                    rd.SetDataSource(rptSource);


                CrystalReportViewer1.ReportSource = rd;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;


            }
            catch (Exception exc)
            {


            }

            //try
            //{
            //    string strReportName = "CanalVenta.rpt";

            //    var rptSource = System.Web.HttpContext.Current.Session["rptSource"];
            //    rd = new ReportDocument();
            //    string strRptPath = Server.MapPath("~/") + "RptsCrystal//" + strReportName;
            //    rd.Load(strRptPath);
            //    // Setting report data source
            //    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
            //        rd.SetDataSource(rptSource);

            //    //rd.SetParameterValue("pA", System.Web.HttpContext.Current.Session["pA"]);
            //    //rd.SetParameterValue("pB", System.Web.HttpContext.Current.Session["pB"]);
            //    //rd.SetParameterValue("pDesde", System.Web.HttpContext.Current.Session["pDesde"]);
            //    //rd.SetParameterValue("pHasta", System.Web.HttpContext.Current.Session["pHasta"]);
            //    //rd.SetParameterValue("pTipos", System.Web.HttpContext.Current.Session["pTipos"]);
            //    //rd.SetParameterValue("pCliente", System.Web.HttpContext.Current.Session["pCliente"]);
            //    //rd.SetParameterValue("pNoDocumento", System.Web.HttpContext.Current.Session["pNoDocumento"]);
            //    //rd.SetParameterValue("pEstado", System.Web.HttpContext.Current.Session["pEstado"]);
            //    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "reporte de canal de ventas");
            //    Response.End();
            //    //rystalReportViewer1.ReportSource = rd;
            //}
            //catch (Exception exc)
            //{


            //}

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