using CapaDato.Maestros;
using CapaDato.Reporte;
using CapaEntidad.Control;
using CapaEntidad.Util;
using CapaEntidad.ValeCompra;
using Data.Crystal.Reporte;
using Models.Crystal.Reporte;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class AlmacenController : Controller
    {
        private string _session_LLegMercaderia = "_session_LLegMercaderia";
        private string _session_LLegMercaderia_Excel = "_session_LLegMercaderia_Excel";
        private Dat_DisCadTda discattda = new Dat_DisCadTda();
        private string _session_dis_cad_tda = "_session_dis_cad_tda";
        private Dat_Combo datCbo = new Dat_Combo();
        // GET: Almacen
        public ActionResult Index()
        {
            return View();
        }
        #region <REPORTE LLEGADA MERCADERIA TIENDA>

        public ActionResult LLegada_Mercaderia()
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

                string pais = "PE";
                if (Session["PAIS"] != null)
                {
                    pais = Session["PAIS"].ToString();
                }

                List<Ent_Combo_DisCadTda> combo_discadtda = discattda.list_dis_cad_tda(pais);
                if (Session["Tienda"] != null)
                {
                    combo_discadtda = combo_discadtda.Where(t => t.cod_entid == Session["Tienda"].ToString()).ToList();
                }

                ViewBag.Distrito = combo_distrito(combo_discadtda);
                ViewBag.DisCadTda = combo_discadtda;
                Session[_session_dis_cad_tda] = combo_discadtda;

                List<Ent_Combo_DisCadTda> list_cad = new List<Ent_Combo_DisCadTda>();
                Ent_Combo_DisCadTda entCombo_cad = new Ent_Combo_DisCadTda();
                entCombo_cad.cod_cadena = "-1";
                entCombo_cad.des_cadena = "----Todos----";
                list_cad.Add(entCombo_cad);

                List<Ent_Combo_DisCadTda> list_tda = new List<Ent_Combo_DisCadTda>();
                Ent_Combo_DisCadTda entCombo_tda = new Ent_Combo_DisCadTda();
                entCombo_tda.cod_entid = "-1";
                entCombo_tda.des_entid = "----Todos----";
                list_tda.Add(entCombo_tda);

                ViewBag.Cadena = list_cad;
                ViewBag.Tienda = list_tda;

                List<Ent_Combo> listD = new List<Ent_Combo>();
                Ent_Combo entComboD = new Ent_Combo();
                entComboD.cbo_codigo = "0";
                entComboD.cbo_descripcion = "----Todos----";
                listD.Add(entComboD);
                ViewBag.Categoria = listD;

                listD = new List<Ent_Combo>();
                listD = datCbo.get_ListaEstadoPres();
                ViewBag.Estado = listD;

                ViewBag.Tipo2 = datCbo.get_ListaTipos2();
                Models_LLegMercaderia EntLLegMercaderia = new Models_LLegMercaderia();
                ViewBag.EntLLegMercaderia = EntLLegMercaderia;

                return View();
            }
        }
        private List<Ent_Combo_DisCadTda> combo_distrito(List<Ent_Combo_DisCadTda> combo_general)
        {
            List<Ent_Combo_DisCadTda> listar = null;
            try
            {
                listar = new List<Ent_Combo_DisCadTda>();
                listar = (from grouping in combo_general.GroupBy(x => new Tuple<string, string>(x.cod_distri, x.des_distri))
                          select new Ent_Combo_DisCadTda
                          {
                              cod_distri = grouping.Key.Item1,
                              des_distri = grouping.Key.Item2,
                          }).ToList();
            }
            catch
            {


            }
            return listar;
        }
        public ActionResult ShowGenericReportLLegMercaderia(string cod_tda, string FEC_INI, string FEC_FIN, string Concepto)
        {
            Session[_session_LLegMercaderia] = null;
            JsonResponse objResult = new JsonResponse();
            Models_LLegMercaderia _Ent = new Models_LLegMercaderia();
            _Ent.CodTda = cod_tda;
            _Ent.FEC_INI = DateTime.Parse(FEC_INI);
            _Ent.FEC_FIN = DateTime.Parse(FEC_FIN);
            _Ent.Concepto = Concepto;

            Data_Planilla pl = new Data_Planilla();
            this.HttpContext.Session["ReportName"] = "LlegMercaderia.rpt";

            List<Models_LLegMercaderia> lisLlegMercaderia = pl.get_reporteLLegMercaderia(_Ent);
            Session[_session_LLegMercaderia] = lisLlegMercaderia;

            this.HttpContext.Session["rptSource"] = lisLlegMercaderia;

            var count = lisLlegMercaderia.Count;

            objResult.Success = (count == 0 ? false : true);
            objResult.Message = (count == 0 ? "No hay información para mostrar." : "El reporte se genero correctamente.");

            var JSON = JsonConvert.SerializeObject(objResult);

            return Json(JSON, JsonRequestBehavior.AllowGet);
        }

        public ActionResult get_exporta_LLegMercaderia_excel(Models_LLegMercaderia _Ent)
        {
            JsonResponse objResult = new JsonResponse();
            try
            {
                Session[_session_LLegMercaderia_Excel] = null;
                string cadena = "";
                if (Session[_session_LLegMercaderia] != null)
                {

                    List<Models_LLegMercaderia> _Listar_LLegMercaderia = (List<Models_LLegMercaderia>)Session[_session_LLegMercaderia];
                    if (_Listar_LLegMercaderia.Count == 0)
                    {
                        objResult.Success = false;
                        objResult.Message = "No hay filas para exportar";
                    }
                    else
                    {
                        cadena = get_html_Listar_LLegMercaderia_str((List<Models_LLegMercaderia>)Session[_session_LLegMercaderia], _Ent);
                        if (cadena.Length == 0)
                        {
                            objResult.Success = false;
                            objResult.Message = "Error del formato html";
                        }
                        else
                        {
                            objResult.Success = true;
                            objResult.Message = "Se genero el excel correctamente";
                            Session[_session_LLegMercaderia_Excel] = cadena;
                        }
                    }
                }
                else
                {
                    objResult.Success = false;
                    objResult.Message = "No hay filas para exportar";
                }

            }
            catch (Exception ex)
            {
                objResult.Success = false;
                objResult.Message = "No hay filas para exportar";
            }

            var JSON = JsonConvert.SerializeObject(objResult);

            return Json(JSON, JsonRequestBehavior.AllowGet);
        }

        public string get_html_Listar_LLegMercaderia_str(List<Models_LLegMercaderia> _Listar_LLegMercaderia, Models_LLegMercaderia _Ent)
        {
            StringBuilder sb = new StringBuilder();
            var Lista = _Listar_LLegMercaderia.ToList();
            try
            {
                sb.Append("<div>");
                sb.Append("<table cellspacing='0' style='width: 1000px' rules='all' border='0' style='border-collapse:collapse;'>");
                sb.Append("<tr><td Colspan='8'></td></tr>");
                sb.Append("<tr><td Colspan='8' valign='middle' align='center' style='vertical-align: middle;font-size: 18.0pt;font-weight: bold;color:#285A8F'>REPORTE DE PROMOTOR POR DIRECTORA</td></tr>");
                sb.Append("<tr><td Colspan='8' valign='middle' align='center' style='vertical-align: middle;font-size: 10.0pt;font-weight: bold;color:#000000'>Rango : del " + String.Format("{0:dd/MM/yyyy}", _Ent.FEC_INI) + " al " + String.Format("{0:dd/MM/yyyy}", _Ent.FEC_FIN) + "</td></tr>");//subtitulo
                sb.Append("<tr bgColor='#1E77AB'>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>TIENDA</font></th>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>TIENDA ORIGEN</font></th>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>TIPO</font></th>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>NUMERO</font></th>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>FECHA RECEPCION EN SISTEMAS</font></th>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>TRANSPORTISTA</font></th>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>FECHA DE RECEPCION FISICA</font></th>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>HORA DE RECEPCION FISICA</font></th>\n");
                sb.Append("</tr>\n");

                foreach (var item in Lista)
                {
                    sb.Append("<tr>\n");
                    sb.Append("<td align=''>" + item.TIENDA + "</td>\n");
                    sb.Append("<td align=''>" + item.TIENDA_ORI + "</td>\n");
                    sb.Append("<td align=''>" + item.TIPO + "</td>\n");
                    sb.Append("<td align='Center'>" + item.NUMERO + "</td>\n");
                    sb.Append("<td align='Center'>" + Convert.ToDateTime(String.Format("{0:d}", item.FECHA)) + "</td>\n");
                    sb.Append("<td align=''>" + item.TRANSPORTISTA + "</td>\n");
                    sb.Append("<td align='Center'>" + Convert.ToDateTime(String.Format("{0:d}", item.FECHA_RECEPCION_FISICA)) + "</td>\n");
                    sb.Append("<td align='Center'>" + item.HORA_RECEPCION_FISICA + "</td>\n");

                    sb.Append("</tr>\n");
                }
                //sb.Append("<tfoot>\n");
                //sb.Append("<tr bgcolor='#085B8C'>\n");
                //sb.Append("</tr>\n");
                //sb.Append("</tfoot>\n");
                sb.Append("</table></div>");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return sb.ToString();
        }
        public ActionResult ListarMercaderia_Excel()
        {
            string NombreArchivo = "ReporteLLegadaMercaderia";
            String style = style = @"<style> .textmode { mso-number-format:\@; } </script> ";
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + NombreArchivo + ".xls");
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.Write(style);
                Response.Write(Session[_session_LLegMercaderia_Excel].ToString());
                Response.End();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return Json(new { estado = 0, mensaje = 1 });
        }
        #endregion </REPORTE LLEGADA MERCADERIA TIENDA>
    }
}