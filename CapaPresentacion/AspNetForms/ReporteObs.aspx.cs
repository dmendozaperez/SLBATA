using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapaPresentacion.AspNetForms
{
    public partial class ReporteObs : System.Web.UI.Page
    {
        ReportDocument rd = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            //if (!this.IsPostBack)
            //{
               // Data.Crystal.Reporte.Data_Bata r = new Data.Crystal.Reporte.Data_Bata();
                string strReportName = "ReporteOBS.rpt";

                //if (Session["data"] == null)
                 //   Session["data"] = r.list_obs("-1", "50390", "R", "-1", "-1", "-1", -1, -1, "-1", "-1");

                var rptSource = Session["data"];

                rd = new ReportDocument();

                string strRptPath = Server.MapPath("~/") + "RptsCrystal//" + strReportName;

                rd.Load(strRptPath);

                // Setting report data source
                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                    rd.SetDataSource(rptSource);

            rd.DataDefinition.FormulaFields["supress"].Text = Session["obs_resumen"].ToString();

            crv_obs.ReportSource = rd;
                crv_obs.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                crv_obs.HasToggleGroupTreeButton = false;
            

            //}
            //else
            //{
            //    if (Session["data"] != null)
            //    {
            //        crv_obs.ReportSource = Session["data"];
            //        crv_obs.DataBind();
            //    }

            //}
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //string strReportName = System.Web.HttpContext.Current.Session["ReportName"].ToString();
                //var rptSource = System.Web.HttpContext.Current.Session["rptSource"];
                //Data.Crystal.Reporte.Data_Bata r = new Data.Crystal.Reporte.Data_Bata(); 
                //string strReportName = "ReporteOBS.rpt";

                //if (Session["data"]==null)
                //    Session["data"]= r.list_obs("-1", "50390", "R", "-1", "-1", "-1", -1, -1, "-1", "-1");

                //var  rptSource = Session["data"];

                //rd = new ReportDocument();

                //string strRptPath = Server.MapPath("~/") + "RptsCrystal//" + strReportName;

                //rd.Load(strRptPath);

                //// Setting report data source
                //if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                //    rd.SetDataSource(rptSource);


                //crv_obs.ReportSource = rd;
                //crv_obs.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                //crv_obs.HasToggleGroupTreeButton = false;


            }
            catch (Exception exc)
            {


            }
        }

        protected void crv_obs_Unload(object sender, EventArgs e)
        {
            if ((rd != null) && rd.IsLoaded)
            {
                rd.Close();
                rd.Dispose();
            }
        }
    }
}