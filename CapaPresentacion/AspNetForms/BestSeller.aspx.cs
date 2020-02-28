using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapaPresentacion.AspNetForms
{
    public partial class BestSeller : System.Web.UI.Page
    {
        ReportDocument rd = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strReportName = System.Web.HttpContext.Current.Session["ReportName"].ToString();
                var rptSource = System.Web.HttpContext.Current.Session["rptSource"];


                rd = new ReportDocument();

                string strRptPath = Server.MapPath("~/") + "RptsCrystal//" + strReportName;

                rd.Load(strRptPath);

                // Setting report data source
                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                    rd.SetDataSource(rptSource);


                crv_BestSeller.ReportSource = rd;
                crv_BestSeller.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                crv_BestSeller.HasToggleGroupTreeButton = false;


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
        protected void Page_Init(object sender, EventArgs e)
        {

        }
    }
}