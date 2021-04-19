using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using CapaEntidad.Control;
using CapaEntidad.Util;
using CapaEntidad.General;
using CapaEntidad.Reports;
using CapaDato.Maestros;
using CapaDato.Reporte;
using Newtonsoft.Json;

namespace CapaPresentacion.Controllers
{
    public class Bata_ReporteController : Controller
    {
        #region <DECLARACION DE VARIABLES>
        private Dat_DisCadTda discattda = new Dat_DisCadTda();
        private Dat_XstoreReporte datXstoreReporte = new Dat_XstoreReporte();
        private string _session_ListarXstore_Vendedor = "_session_ListarXstore_Vendedor";
        private string _session_ListarXstore_Vendedor_Excel = "_session_ListarXstore_Vendedor_Excel";
        #endregion

        #region <XSTORE REPORTE INCENTIVO ESCOLAR VENDEDORES>
        public ActionResult Reporte_Incentivo()
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

                ViewBag.TipoReporte = new List<Ent_Combo>() {
                    new Ent_Combo() { cbo_codigo = "-1", cbo_descripcion = "RESUMIDO" } ,
                    new Ent_Combo() { cbo_codigo = "1", cbo_descripcion = "DETALLADO" }
                };

                ViewBag.EntXstoreVendedor = new Ent_Xstore_Vendedor();
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
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listar;
        }
        public JsonResult getListarXstore_VendedorAjax(Ent_jQueryDataTableParams param, bool isOkUpdate,string FechaInicio, string FechaFin,string Cod_Tda, string Tip_Rep)
        {
            Ent_Xstore_Vendedor Ent_Xstore_Vendedor = new Ent_Xstore_Vendedor();

            if (isOkUpdate)
            {
                Ent_Xstore_Vendedor.FechaInicio = DateTime.Parse(FechaInicio);
                Ent_Xstore_Vendedor.FechaFin = DateTime.Parse(FechaFin);
                Ent_Xstore_Vendedor.Cod_Tda = Cod_Tda;
                Ent_Xstore_Vendedor.Tip_Rep = Tip_Rep;
                Session[_session_ListarXstore_Vendedor] = datXstoreReporte.ListarIncentivoVendedor(Ent_Xstore_Vendedor).ToList();
            }

            /*verificar si esta null*/
            if (Session[_session_ListarXstore_Vendedor] == null)
            {
                List<Ent_Xstore_Vendedor> _ListarXstore_Vendedor = new List<Ent_Xstore_Vendedor>();
                Session[_session_ListarXstore_Vendedor] = _ListarXstore_Vendedor;
            }

            IQueryable<Ent_Xstore_Vendedor> entDocTrans = ((List<Ent_Xstore_Vendedor>)(Session[_session_ListarXstore_Vendedor])).AsQueryable();

            //Manejador de filtros
            int totalCount = entDocTrans.Count();
            IEnumerable<Ent_Xstore_Vendedor> filteredMembers = entDocTrans;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = entDocTrans.Where(
                        m =>
                            m.Distrito.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Des_Cadena.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Dni.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Dni_Nombre.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Fecha.ToString().Contains(param.sSearch.ToUpper()) ||
                            m.Documento.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Articulo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Cod_Categ.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Cod_Subca.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Cantidad.ToString().Contains(param.sSearch.ToUpper()) ||
                            m.Total.ToString().Contains(param.sSearch.ToUpper()) ||
                            m.Tot_Transacc.ToString().Contains(param.sSearch.ToUpper()) ||
                            m.Esc_Negro_2x50.ToString().Contains(param.sSearch.ToUpper()) ||
                            m.Total_Incentivo.ToString().Contains(param.sSearch.ToUpper()) ||
                            m.Incentivo.ToString().Contains(param.sSearch.ToUpper()) ||
                            m.Incentivo1.ToString().Contains(param.sSearch.ToUpper()) ||
                            m.Incentivo2.ToString().Contains(param.sSearch.ToUpper())
                );
            }
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            if (param.iSortingCols > 0)
            {
                if (Request["sSortDir_0"].ToString() == "asc")
                {
                    switch (sortIdx)
                    {
                        case 0: filteredMembers = filteredMembers.OrderBy(o => o.Distrito); break;
                        case 1: filteredMembers = filteredMembers.OrderBy(o => o.Des_Cadena); break;
                        case 2: filteredMembers = filteredMembers.OrderBy(o => o.Dni); break;
                        case 3: filteredMembers = filteredMembers.OrderBy(o => o.Dni_Nombre); break;
                        case 4: filteredMembers = filteredMembers.OrderBy(o => o.Fecha); break;
                        case 5: filteredMembers = filteredMembers.OrderBy(o => o.Documento); break;
                        case 6: filteredMembers = filteredMembers.OrderBy(o => o.Articulo); break;
                        case 7: filteredMembers = filteredMembers.OrderBy(o => o.Cod_Categ); break;
                        case 8: filteredMembers = filteredMembers.OrderBy(o => o.Cod_Subca); break;
                        case 9: filteredMembers = filteredMembers.OrderBy(o => o.Cantidad); break;
                        case 10: filteredMembers = filteredMembers.OrderBy(o => o.Total); break;
                        case 11: filteredMembers = filteredMembers.OrderBy(o => o.Tot_Transacc); break;
                        case 12: filteredMembers = filteredMembers.OrderBy(o => o.Esc_Negro_2x50); break;
                        case 13: filteredMembers = filteredMembers.OrderBy(o => o.Total_Incentivo); break;
                        case 14: filteredMembers = filteredMembers.OrderBy(o => o.Incentivo); break;
                        case 15: filteredMembers = filteredMembers.OrderBy(o => o.Incentivo1); break;
                        case 16: filteredMembers = filteredMembers.OrderBy(o => o.Incentivo2); break;
                    }
                }
                else
                {
                    switch (sortIdx)
                    {
                        case 0: filteredMembers = filteredMembers.OrderByDescending(o => o.Distrito); break;
                        case 1: filteredMembers = filteredMembers.OrderByDescending(o => o.Des_Cadena); break;
                        case 2: filteredMembers = filteredMembers.OrderByDescending(o => o.Dni); break;
                        case 3: filteredMembers = filteredMembers.OrderByDescending(o => o.Dni_Nombre); break;
                        case 4: filteredMembers = filteredMembers.OrderByDescending(o => o.Fecha); break;
                        case 5: filteredMembers = filteredMembers.OrderByDescending(o => o.Documento); break;
                        case 6: filteredMembers = filteredMembers.OrderByDescending(o => o.Articulo); break;
                        case 7: filteredMembers = filteredMembers.OrderByDescending(o => o.Cod_Categ); break;
                        case 8: filteredMembers = filteredMembers.OrderByDescending(o => o.Cod_Subca); break;
                        case 9: filteredMembers = filteredMembers.OrderByDescending(o => o.Cantidad); break;
                        case 10: filteredMembers = filteredMembers.OrderByDescending(o => o.Total); break;
                        case 11: filteredMembers = filteredMembers.OrderByDescending(o => o.Tot_Transacc); break;
                        case 12: filteredMembers = filteredMembers.OrderByDescending(o => o.Esc_Negro_2x50); break;
                        case 13: filteredMembers = filteredMembers.OrderByDescending(o => o.Total_Incentivo); break;
                        case 14: filteredMembers = filteredMembers.OrderByDescending(o => o.Incentivo); break;
                        case 15: filteredMembers = filteredMembers.OrderByDescending(o => o.Incentivo1); break;
                        case 16: filteredMembers = filteredMembers.OrderByDescending(o => o.Incentivo2); break;
                    }
                }
            }

            var Result = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            //Se devuelven los resultados por json
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = Result,
                isTipoRepor = (Tip_Rep == "1" ? true : false)
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult get_exporta_Xstore_Vendedor_excel(Ent_Xstore_Vendedor _Ent)
        {
            JsonRespuesta objResult = new JsonRespuesta();
            try
            {
                Session[_session_ListarXstore_Vendedor_Excel] = null;
                string cadena = "";
                if (Session[_session_ListarXstore_Vendedor] != null)
                {

                    List<Ent_Xstore_Vendedor> _ListarXstore_Vendedor = (List<Ent_Xstore_Vendedor>)Session[_session_ListarXstore_Vendedor];
                    if (_ListarXstore_Vendedor.Count == 0)
                    {
                        objResult.Success = false;
                        objResult.Message = "No hay filas para exportar";

                    }
                    else
                    {
                        cadena = get_html_ListarXstore_Vendedor_str((List<Ent_Xstore_Vendedor>)Session[_session_ListarXstore_Vendedor], _Ent);
                        if (cadena.Length == 0)
                        {
                            objResult.Success = false;
                            objResult.Message = "Error del formato html";
                        }
                        else
                        {
                            objResult.Success = true;
                            objResult.Message = "Se genero el excel correctamente";
                            Session[_session_ListarXstore_Vendedor_Excel] = cadena;
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

        public string get_html_ListarXstore_Vendedor_str(List<Ent_Xstore_Vendedor> _ListarListarXstore_Vendedor, Ent_Xstore_Vendedor _Ent)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                var Lista = _ListarListarXstore_Vendedor.ToList();
                int colspan = (_Ent.Tip_Rep == "1" ? 14 : 7);
                sb.Append("<div>");
                sb.Append("<table cellspacing='0' style='width: 1000px' rules='all' border='0' style='border-collapse:collapse;'>");
                sb.Append("<tr><td Colspan='" + colspan + "'></td></tr>");
                sb.Append("<tr><td Colspan='" + colspan + "' valign='middle' align='center' style='vertical-align: middle;font-size: 18.0pt;font-weight: bold;color:#285A8F'>REPORTE INCENTIVO ESCOLAR A LOS VENDEDORES</td></tr>");
                sb.Append("<tr><td Colspan='" + colspan + "' valign='middle' align='center' style='vertical-align: middle;font-size: 10.0pt;font-weight: bold;color:#000000'>Rango: del " + String.Format("{0:dd/MM/yyyy}", _Ent.FechaInicio) + " al " + String.Format("{0:dd/MM/yyyy}", _Ent.FechaFin) + "</td></tr>");//subtitulo
                sb.Append("</table>");
                sb.Append("<table  border='1' bgColor='#ffffff' borderColor='#FFFFFF' cellSpacing='2' cellPadding='2' style='font-size:10.0pt; font-family:Calibri; background:white;width: 1000px'><tr  bgColor='#5799bf'>\n");
                sb.Append("<tr bgColor='#1E77AB'>\n");

                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Distrito</font></th>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Entidad</font></th>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Dni</font></th>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Nombre</font></th>\n");
                if (_Ent.Tip_Rep=="1")
                {
                    sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Fecha</font></th>\n");
                    sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Documento</font></th>\n");
                    sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Articulo</font></th>\n");
                    sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Cod. Categ</font></th>\n");
                    sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Cod. Sub Categoría</font></th>\n");
                    sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Cantidad</font></th>\n");
                    sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Total</font></th>\n");
                    sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Tot. Transacción</font></th>\n");
                    sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Esc_Negro_2x50</font></th>\n");
                    sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Incentivo</font></th>\n");
                }
                else
                {
                    sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Incentivo 1</font></th>\n");
                    sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Incentivo 2</font></th>\n");
                    sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Total Incentivo</font></th>\n");
                }


                sb.Append("</tr>\n");

                foreach (var item in Lista)
                {
                    sb.Append("<tr>\n");
                    sb.Append("<td>" + item.Distrito + "</td>\n");
                    sb.Append("<td>" + item.Des_Entid + "</td>\n");
                    sb.Append("<td>" + item.Dni + "</td>\n");
                    sb.Append("<td>" + item.Dni_Nombre + "</td>\n");
                    if (_Ent.Tip_Rep == "1")
                    {
                        sb.Append("<td align='Center'>" + item.Fecha + "</td>\n");
                        sb.Append("<td align='Center'>" + item.Documento + "</td>\n");
                        sb.Append("<td align='Center'>" + item.Articulo + "</td>\n");
                        sb.Append("<td>" + item.Cod_Categ + "</td>\n");
                        sb.Append("<td>" + item.Cod_Subca + "</td>\n");
                        sb.Append("<td align='right'>" + item.Cantidad + "</td>\n");
                        sb.Append("<td align='right'>" + item.Total + "</td>\n");
                        sb.Append("<td align='right'>" + item.Tot_Transacc + "</td>\n");
                        sb.Append("<td align='right'>" + item.Esc_Negro_2x50 + "</td>\n");
                        sb.Append("<td align='right'>" + string.Format("{0:F2}", item.Incentivo) + "</td>\n");
                    }
                    else
                    {
                        sb.Append("<td align='right'>" + item.Incentivo1 + "</td>\n");
                        sb.Append("<td align='right'>" + item.Incentivo2 + "</td>\n");
                        sb.Append("<td align='right'>" + string.Format("{0:F2}", item.Total_Incentivo) + "</td>\n");
                    }
                    sb.Append("</tr>\n");
                }
                sb.Append("</table></div>");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return sb.ToString();
        }
        public ActionResult ListaXstore_Vendedor_Excel()
        {
            string NombreArchivo = "INCENTIVOS_" + DateTime.Today.ToShortDateString();
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
                Response.Write(Session[_session_ListarXstore_Vendedor_Excel].ToString());
                Response.End();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return Json(new { estado = 0, mensaje = 1 });
        }
        #endregion

    }
}