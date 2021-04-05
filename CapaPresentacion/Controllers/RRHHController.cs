using CapaDato.Maestros;
using CapaEntidad.Util;
using CapaEntidad.Control;
using Models.Crystal.Reporte;
using Data.Crystal.Reporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Newtonsoft.Json;
namespace CapaPresentacion.Controllers
{
    public class RRHHController : Controller
    {
        #region <DECLARACION DE VARIABLES>
        private Dat_DisCadTda discattda = new Dat_DisCadTda();
        private Dat_RRHH dat_RRHH = new Dat_RRHH();
        private string _session_CuponRedimidos = "_session_CuponRedimidos";
        private string _session_CuponRedimidos_Excel = "_session_CuponRedimidos_Excel";
        #endregion
        #region <REPORTE DE CUPONES REDIMIDOS>
        public ActionResult ReporteCuponRedimidos()
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

                ViewBag.Prefijo = dat_RRHH.get_Prefijos();
                ViewBag.Cadena = list_cad;
                ViewBag.Tienda = list_tda;

                Models_CuponRedimidos EntCuponRedimidos = new Models_CuponRedimidos();
                ViewBag.EntCuponRedimidos = EntCuponRedimidos;

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
                listar = new List<Ent_Combo_DisCadTda>();
            }
            return listar;
        }
        public ActionResult ShowGenericReportCuponRedimidos(Models_CuponRedimidos _Ent)
        {
            Session[_session_CuponRedimidos] = null;
            JsonRespuesta objResult = new JsonRespuesta();
            List<Models_CuponRedimidos> lisCuponRedimidos = dat_RRHH.get_reporteCuponRedimidos(_Ent);
            try
            {
                if (lisCuponRedimidos.Count > 0)
                {
                    Session[_session_CuponRedimidos] = lisCuponRedimidos;

                    this.HttpContext.Session["ReportName"] = "ReportCuponRedimidos.rpt";
                    this.HttpContext.Session["rptSource"] = lisCuponRedimidos;

                    objResult.Success = true;
                    objResult.Message = "El reporte se genero correctamente.";
                }
                else
                {
                    objResult.Success = false;
                    objResult.Message = "No hay información para mostrar.";
                }
            }
            catch (Exception)
            {
                objResult.Success = false;
                objResult.Message = "Error al mostrar el reporte.";
            }

            var JSON = JsonConvert.SerializeObject(objResult);
            return Json(JSON, JsonRequestBehavior.AllowGet);
        }

        public ActionResult get_exporta_CuponRedimidos_excel(Models_CuponRedimidos _Ent)
        {
            JsonRespuesta objResult = new JsonRespuesta();
            try
            {
                Session[_session_CuponRedimidos_Excel] = null;
                string cadena = "";
                if (Session[_session_CuponRedimidos] != null)
                {
                    List<Models_CuponRedimidos> _Listar_CuponRedimidos = (List<Models_CuponRedimidos>)Session[_session_CuponRedimidos];
                    if (_Listar_CuponRedimidos.Count == 0)
                    {
                        objResult.Success = false;
                        objResult.Message = "No hay filas para exportar";
                    }
                    else
                    {
                        cadena = get_html_Listar_CuponRedimidos_str((List<Models_CuponRedimidos>)Session[_session_CuponRedimidos], _Ent);
                        if (cadena.Length == 0)
                        {
                            objResult.Success = false;
                            objResult.Message = "Error del formato html";
                        }
                        else
                        {
                            objResult.Success = true;
                            objResult.Message = "Se genero el excel correctamente";
                            Session[_session_CuponRedimidos_Excel] = cadena;
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

        public string get_html_Listar_CuponRedimidos_str(List<Models_CuponRedimidos> _Listar_CuponRedimidos, Models_CuponRedimidos _Ent)
        {
            StringBuilder sb = new StringBuilder();
            var Lista = _Listar_CuponRedimidos.ToList();
            try
            {
                sb.Append("<div>");
                sb.Append("<table cellspacing='0' style='width: 1000px' rules='all' border='0' style='border-collapse:collapse;'>");
                sb.Append("<tr><td Colspan='7'></td></tr>");
                sb.Append("<tr><td Colspan='7' valign='middle' align='center' style='vertical-align: middle;font-size: 18.0pt;font-weight: bold;color:#285A8F'>REPORTE DE CUPONES REDIMIDOS</td></tr>");
                sb.Append("<tr><td Colspan='7' valign='middle' align='center' style='vertical-align: middle;font-size: 10.0pt;font-weight: bold;color:#000000'>Rango : del " + String.Format("{0:dd/MM/yyyy}", _Ent.FechaInicio) + " al " + String.Format("{0:dd/MM/yyyy}", _Ent.FechaFin) + "</td></tr>");//subtitulo
                sb.Append("<tr><td Colspan='7'></td></tr>");
                sb.Append("<tr>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Distrito</font></th>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Cadena</font></th>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Cod. Tienda</font></th>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Nom. Tienda</font></th>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Documento</font></th>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Fecha</font></th>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Dni</font></th>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Cliente</font></th>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Cupon</font></th>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>CampaÑa</font></th>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Descripcion</font></th>\n");
                sb.Append("</tr>\n");

                foreach (var item in Lista)
                {
                    sb.Append("<tr>\n");
                    sb.Append("<td align=''>" + item.Distrito + "</td>\n");
                    sb.Append("<td align=''>" + item.Cadena + "</td>\n");
                    sb.Append("<td align='Center'>" + item.Cod_Tienda + "</td>\n");
                    sb.Append("<td align=''>" + item.Nombre_Tienda + "</td>\n");
                    sb.Append("<td align=''>" + item.Documento + "</td>\n");
                    sb.Append("<td align='Center'>" + String.Format("{0:d}", item.Fecha) + "</td>\n");
                    sb.Append("<td align='Center'>" + item.Dni + "</td>\n");
                    sb.Append("<td align=''>" + item.Cliente + "</td>\n");
                    sb.Append("<td align='Center'>" + item.Cupon + "</td>\n");
                    sb.Append("<td align=''>" + item.Campanna + "</td>\n");
                    sb.Append("<td align=''>" + item.Descripcion + "</td>\n");
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
        public ActionResult ListarCuponRedimidos_Excel()
        {
            string NombreArchivo = "ReporteCuponRedimidos";
            String style = style = @"<style> .textmode { mso-number-format:\@; } </style> ";
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + NombreArchivo + ".xls");
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.Write(style);
                Response.Write(Session[_session_CuponRedimidos_Excel].ToString());
                Response.End();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return Json(new { estado = 0, mensaje = 1 });
        }
        #endregion </REPORTE DE VENTAS EFECTIVO>
    }
}