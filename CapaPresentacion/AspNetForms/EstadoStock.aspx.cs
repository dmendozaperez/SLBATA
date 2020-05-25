using CapaPresentacion.Models.Crystal.Reporte;
using CapaPresentacion.RptsCrystal;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapaPresentacion.AspNetForms
{
    public partial class EstadoStock : System.Web.UI.Page
    {
        ReportDocument rd = null;
        protected void Page_Load(object sender, EventArgs e)
        {

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
            try
            {
                string strReportName = System.Web.HttpContext.Current.Session["ReportName"].ToString();
                //var rptSource = System.Web.HttpContext.Current.Session["rptSource"];

                Models_EstadoStock rptSource = (Models_EstadoStock) System.Web.HttpContext.Current.Session["rptSource"]; 

                rd = new ReportDocument();

                string strRptPath = Server.MapPath("~/") + "RptsCrystal//" + strReportName;

                rd.Load(strRptPath);

                // Setting report data source
                if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                { 
                    //rd.SetDataSource(rptSource.list_cab);
                    //rd.SetDataSource(rptSource.list_det);
                    //rd.SetDataSource(rptSource.list_fin);

                    rd.Database.Tables[0].SetDataSource(rptSource.list_cab);
                    rd.Database.Tables[1].SetDataSource(rptSource.list_det);
                    rd.Database.Tables[2].SetDataSource(rptSource.list_fin);
                    rd.Database.Tables[3].SetDataSource(rptSource.list_var);
                    rd.Database.Tables[4].SetDataSource(rptSource.list_ins);
                    rd.Database.Tables[5].SetDataSource(rptSource.list_saldos);
                }


                crv_estado_stk.ReportSource = rd;
                crv_estado_stk.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                crv_estado_stk.HasToggleGroupTreeButton = false;


            }
            catch (Exception exc)
            {


            }
        }
    }
}