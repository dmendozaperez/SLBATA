using CapaEntidad.General;
using CapaDato.Transitos;
using CapaDato.Reporte;
using CapaEntidad.Util;
using CapaEntidad.Control;
using CapaEntidad.Transitos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Newtonsoft.Json;
namespace CapaPresentacion.Controllers
{
    public class TransitosController : Controller
    {
        #region <DECLARACION DE VARIABLES>
        private Dat_Combo tienda = new Dat_Combo();
        private Dat_Transitos datTransitos = new Dat_Transitos();
        private string _session_ListarConsulta_Transitos = "_session_ListarConsulta_Transitos";
        private string _session_ListarConsulta_Transitos_Excel = "_session_ListarConsulta_Transitos_Excel";
        private string _session_ListarConsulta_TransitosArt = "_session_ListarConsulta_TransitosArt";
        private string _session_ListarConsulta_TransitosArt_Excel = "_session_ListarConsulta_TransitosArt_Excel";
        #endregion

        #region <CONSULTA DE TRANSITOS>

        public ActionResult Consulta_Transitos()
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
                Session[_session_ListarConsulta_Transitos] = null;
            Session[_session_ListarConsulta_TransitosArt] = null;
            string pais = "PE";
            if (Session["PAIS"] != null)
            {
                pais = Session["PAIS"].ToString();
            }

            List<Ent_Combo_DisCadTda> combo_discadtda = datTransitos.List_Emp_Cad_tda(pais);
            List<Ent_concepto_Transitos> combo_concepto = datTransitos.List_Concepto_Transitos();
            List<Ent_Articulo_Transitos> como_articulo = new List<Ent_Articulo_Transitos> { new Ent_Articulo_Transitos() { Cod_Artic = "-1",Des_Artic ="--Seleccione--" } };

            ViewBag.Concepto = combo_concepto;
            ViewBag.Articulo = como_articulo.Concat(datTransitos.Lista_Articulo_Transitos(pais));
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
        /// <summary>
        /// Transito x documento
        /// </summary>
        public JsonResult getLisConsulta_TransitosAjax(Ent_jQueryDataTableParams param, bool isOkUpdate, string Fecha, string Empresa, string Cadena, string Concepto, string Destino)
        {
            Ent_Consulta_Transitos EntConsultaTransitos = new Ent_Consulta_Transitos();

            if (isOkUpdate)
            {
                EntConsultaTransitos.Fecha = DateTime.Parse(Fecha);
                EntConsultaTransitos.Empresa = Empresa;
                EntConsultaTransitos.Cadena = Cadena;
                EntConsultaTransitos.Concepto = Concepto;
                EntConsultaTransitos.Destino = Destino;

                List<Ent_Consulta_Transitos> _ListarConsulta_Transitos = datTransitos.ListarConsulta_Transitos(EntConsultaTransitos).ToList();
                Session[_session_ListarConsulta_Transitos] = _ListarConsulta_Transitos;
            }

            /*verificar si esta null*/
            if (Session[_session_ListarConsulta_Transitos] == null)
            {
                List<Ent_Consulta_Transitos> _ListarConsulta_Transitos = new List<Ent_Consulta_Transitos>();
                Session[_session_ListarConsulta_Transitos] = _ListarConsulta_Transitos;
            }

            IQueryable<Ent_Consulta_Transitos> entDocTrans = ((List<Ent_Consulta_Transitos>)(Session[_session_ListarConsulta_Transitos])).AsQueryable();
            //Manejador de filtros
            int totalCount = entDocTrans.Count();
            IEnumerable<Ent_Consulta_Transitos> filteredMembers = entDocTrans;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = entDocTrans.Where(
                        m =>
                        m.Empre.ToUpper().Contains(param.sSearch.ToUpper()) ||
                        m.Caden.ToUpper().Contains(param.sSearch.ToUpper()) ||
                        m.Concepto.ToUpper().Contains(param.sSearch.ToUpper()) ||
                        m.Origen.ToUpper().Contains(param.sSearch.ToUpper()) ||
                        m.Destino.ToUpper().Contains(param.sSearch.ToUpper()) ||
                        m.Guia.ToUpper().Contains(param.sSearch.ToUpper()) ||
                        m.Calz.ToString().Contains(param.sSearch.ToUpper()) ||
                        m.No_Calz.ToString().Contains(param.sSearch.ToUpper()) ||
                        m.Cajas.ToString().Contains(param.sSearch.ToUpper()) ||
                        m.Estado.ToUpper().Contains(param.sSearch.ToUpper()) ||
                        m.Tran_Ini.ToString().Contains(param.sSearch.ToUpper()) ||
                        m.Tran_Fin.ToString().Contains(param.sSearch.ToUpper())
                );
            }
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            if (param.iSortingCols > 0)
            {
                if (Request["sSortDir_0"].ToString() == "asc")
                {
                    switch (sortIdx)
                    {
                        case 0: filteredMembers = filteredMembers.OrderBy(o => o.Empre); break;
                        case 1: filteredMembers = filteredMembers.OrderBy(o => o.Caden); break;
                        case 2: filteredMembers = filteredMembers.OrderBy(o => o.Concepto); break;
                        case 3: filteredMembers = filteredMembers.OrderBy(o => o.Origen); break;
                        case 4: filteredMembers = filteredMembers.OrderBy(o => o.Destino); break;
                        case 5: filteredMembers = filteredMembers.OrderBy(o => o.Guia); break;
                        case 6: filteredMembers = filteredMembers.OrderBy(o => o.Calz); break;
                        case 7: filteredMembers = filteredMembers.OrderBy(o => o.No_Calz); break;
                        case 8: filteredMembers = filteredMembers.OrderBy(o => o.Cajas); break;
                        case 9: filteredMembers = filteredMembers.OrderBy(o => o.Estado); break;
                        case 10: filteredMembers = filteredMembers.OrderBy(o => o.Tran_Ini); break;
                        case 11: filteredMembers = filteredMembers.OrderBy(o => o.Tran_Fin); break;
                    }
                }
                else
                {
                    switch (sortIdx)
                    {
                        case 0: filteredMembers = filteredMembers.OrderByDescending(o => o.Empre); break;
                        case 1: filteredMembers = filteredMembers.OrderByDescending(o => o.Caden); break;
                        case 2: filteredMembers = filteredMembers.OrderByDescending(o => o.Concepto); break;
                        case 3: filteredMembers = filteredMembers.OrderByDescending(o => o.Origen); break;
                        case 4: filteredMembers = filteredMembers.OrderByDescending(o => o.Destino); break;
                        case 5: filteredMembers = filteredMembers.OrderByDescending(o => o.Guia); break;
                        case 6: filteredMembers = filteredMembers.OrderByDescending(o => o.Calz); break;
                        case 7: filteredMembers = filteredMembers.OrderByDescending(o => o.No_Calz); break;
                        case 8: filteredMembers = filteredMembers.OrderByDescending(o => o.Cajas); break;
                        case 9: filteredMembers = filteredMembers.OrderByDescending(o => o.Estado); break;
                        case 10: filteredMembers = filteredMembers.OrderByDescending(o => o.Tran_Ini); break;
                        case 11: filteredMembers = filteredMembers.OrderByDescending(o => o.Tran_Fin); break;
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
                aaData = Result
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult get_exporta_LisConsulta_Transitos_excel(Ent_Consulta_Transitos _Ent)
        {
            JsonRespuesta objResult = new JsonRespuesta();
            try
            {
                Session[_session_ListarConsulta_Transitos_Excel] = null;
                string cadena = "";
                if (Session[_session_ListarConsulta_Transitos] != null)
                {

                    List<Ent_Consulta_Transitos> _ListarConsulta_Transitos = (List<Ent_Consulta_Transitos>)Session[_session_ListarConsulta_Transitos];
                    if (_ListarConsulta_Transitos.Count == 0)
                    {
                        objResult.Success = false;
                        objResult.Message = "No hay filas para exportar";
                    }
                    else
                    {
                        cadena = get_html_ListarConsulta_Transitos_str((List<Ent_Consulta_Transitos>)Session[_session_ListarConsulta_Transitos], _Ent);
                        if (cadena.Length == 0)
                        {
                            objResult.Success = false;
                            objResult.Message = "Error del formato html";
                        }
                        else
                        {
                            objResult.Success = true;
                            objResult.Message = "Se genero el excel correctamente";
                            Session[_session_ListarConsulta_Transitos_Excel] = cadena;
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

        public string get_html_ListarConsulta_Transitos_str(List<Ent_Consulta_Transitos> _ListarConsulta_Transitos, Ent_Consulta_Transitos _Ent)
        {
            StringBuilder sb = new StringBuilder();
            var Lista = _ListarConsulta_Transitos.ToList();
            try
            {
                sb.Append("<div>");
                sb.Append("<table cellspacing='0' style='width: 1000px' rules='all' border='0' style='border-collapse:collapse;'>");
                sb.Append("<tr><td Colspan='12'></td></tr>");
                sb.Append("<tr><td Colspan='12' valign='middle' align='center' style='vertical-align: middle;font-size: 18.0pt;font-weight: bold;color:#285A8F'>TRANSITO POR DOCUMENTO</td></tr>");
                //sb.Append("<tr><td Colspan='10' valign='middle' align='center' style='vertical-align: middle;font-size: 10.0pt;font-weight: bold;color:#000000'>Desde el " + String.Format("{0:dd/MM/yyyy}", _Ent.FechaInicio) + " hasta el " + String.Format("{0:dd/MM/yyyy}", _Ent.FechaFin) + "</td></tr>");//subtitulo
                sb.Append("<tr>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>EMPRE</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>CADEN</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>CONCEPTO</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>ORIGEN</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>DESTINO</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>GUIA</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>CALZ</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>NO_CALZ</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>CAJAS</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>ESTADO</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>TRAN_INI</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>TRAN_FIN</font></th>\n");
                sb.Append("</tr>\n");
                // {0:N2} Separacion miles , {0:F2} solo dos decimales
                foreach (var item in Lista)
                {
                    sb.Append("<tr>\n");
                    sb.Append("<td align=''>" + item.Empre + "</td>\n");
                    sb.Append("<td align=''>" + item.Caden + "</td>\n");
                    sb.Append("<td align=''>" + item.Concepto + "</td>\n");
                    sb.Append("<td align=''>" + item.Origen + "</td>\n");
                    sb.Append("<td align=''>" + item.Destino + "</td>\n");
                    sb.Append("<td align='Center'>" + item.Guia + "</td>\n");
                    sb.Append("<td align='Center'>" + item.Calz + "</td>\n");
                    sb.Append("<td align='Center'>" + item.No_Calz + "</td>\n");
                    sb.Append("<td align='Center'>" + item.Cajas + "</td>\n");
                    sb.Append("<td align='Center'>" + item.Estado + "</td>\n");
                    sb.Append("<td align='Center'>" + item.Tran_Ini + "</td>\n");
                    sb.Append("<td align='Center'>" + item.Tran_Fin + "</td>\n");
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

        public ActionResult ListarConsulta_TransitosExcel()
        {
            string NombreArchivo = "TrasitoXdocumento";
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
                Response.Write(Session[_session_ListarConsulta_Transitos_Excel].ToString());
                Response.End();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return Json(new { estado = 0, mensaje = 1 });
        }
        /// <summary>
        /// Transito x documento - articulos
        /// </summary>

        public JsonResult getLisConsulta_TransitosArtAjax(Ent_jQueryDataTableParams param, bool isOkUpdate, string Fecha, string Empresa, string Cadena, string Concepto, string Destino, string Articulo)
        {
            Ent_Articulo_TransitosArt EntConsultaTransitos = new Ent_Articulo_TransitosArt();

            if (isOkUpdate)
            {
                EntConsultaTransitos.Fecha = DateTime.Parse(Fecha);
                EntConsultaTransitos.Empresa = Empresa;
                EntConsultaTransitos.Cadena = Cadena;
                EntConsultaTransitos.Concepto = Concepto;
                EntConsultaTransitos.Destino = Destino;
                EntConsultaTransitos.Articulo = Articulo;

                List<Ent_Articulo_TransitosArt> _ListarConsulta_TransitosArt = datTransitos.ListarConsulta_TransitosArt(EntConsultaTransitos).ToList();
                Session[_session_ListarConsulta_TransitosArt] = _ListarConsulta_TransitosArt;
            }

            /*verificar si esta null*/
            if (Session[_session_ListarConsulta_TransitosArt] == null)
            {
                List<Ent_Articulo_TransitosArt> _ListarConsulta_TransitosArt = new List<Ent_Articulo_TransitosArt>();
                Session[_session_ListarConsulta_TransitosArt] = _ListarConsulta_TransitosArt;
            }

            IQueryable<Ent_Articulo_TransitosArt> entDocTrans = ((List<Ent_Articulo_TransitosArt>)(Session[_session_ListarConsulta_TransitosArt])).AsQueryable();
            //Manejador de filtros
            int totalCount = entDocTrans.Count();
            IEnumerable<Ent_Articulo_TransitosArt> filteredMembers = entDocTrans;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = entDocTrans.Where(
                        m =>
                            m.Empresa.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Cadena.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Concepto.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Origen.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Destino.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Guia.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Articulo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Cal.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Talla.ToUpper().Contains(param.sSearch.ToUpper()) ||
                            m.Cantidad.ToString().Contains(param.sSearch.ToUpper())

                );
            }
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            if (param.iSortingCols > 0)
            {
                if (Request["sSortDir_0"].ToString() == "asc")
                {
                    switch (sortIdx)
                    {
                        case 0: filteredMembers = filteredMembers.OrderBy(o => o.Empresa); break;
                        case 1: filteredMembers = filteredMembers.OrderBy(o => o.Cadena); break;
                        case 2: filteredMembers = filteredMembers.OrderBy(o => o.Concepto); break;
                        case 3: filteredMembers = filteredMembers.OrderBy(o => o.Origen); break;
                        case 4: filteredMembers = filteredMembers.OrderBy(o => o.Destino); break;
                        case 5: filteredMembers = filteredMembers.OrderBy(o => o.Guia); break;
                        case 6: filteredMembers = filteredMembers.OrderBy(o => o.Articulo); break;
                        case 7: filteredMembers = filteredMembers.OrderBy(o => o.Cal); break;
                        case 8: filteredMembers = filteredMembers.OrderBy(o => o.Talla); break;
                        case 9: filteredMembers = filteredMembers.OrderBy(o => o.Cantidad); break;
                    }
                }
                else
                {
                    switch (sortIdx)
                    {
                        case 0: filteredMembers = filteredMembers.OrderByDescending(o => o.Empresa); break;
                        case 1: filteredMembers = filteredMembers.OrderByDescending(o => o.Cadena); break;
                        case 2: filteredMembers = filteredMembers.OrderByDescending(o => o.Concepto); break;
                        case 3: filteredMembers = filteredMembers.OrderByDescending(o => o.Origen); break;
                        case 4: filteredMembers = filteredMembers.OrderByDescending(o => o.Destino); break;
                        case 5: filteredMembers = filteredMembers.OrderByDescending(o => o.Guia); break;
                        case 6: filteredMembers = filteredMembers.OrderByDescending(o => o.Articulo); break;
                        case 7: filteredMembers = filteredMembers.OrderByDescending(o => o.Cal); break;
                        case 8: filteredMembers = filteredMembers.OrderByDescending(o => o.Talla); break;
                        case 9: filteredMembers = filteredMembers.OrderByDescending(o => o.Cantidad); break;
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
                aaData = Result
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult get_exporta_LisConsulta_TransitosArt_excel(Ent_Articulo_TransitosArt _Ent)
        {
            JsonRespuesta objResult = new JsonRespuesta();
            try
            {
                Session[_session_ListarConsulta_TransitosArt_Excel] = null;
                string cadena = "";
                if (Session[_session_ListarConsulta_TransitosArt] != null)
                {
                    List<Ent_Articulo_TransitosArt> _ListarConsulta_TransitosArt = (List<Ent_Articulo_TransitosArt>)Session[_session_ListarConsulta_TransitosArt];
                    if (_ListarConsulta_TransitosArt.Count == 0)
                    {
                        objResult.Success = false;
                        objResult.Message = "No hay filas para exportar";
                    }
                    else
                    {
                        cadena = get_html_ListarConsulta_TransitosArt_str((List<Ent_Articulo_TransitosArt>)Session[_session_ListarConsulta_TransitosArt], _Ent);
                        if (cadena.Length == 0)
                        {
                            objResult.Success = false;
                            objResult.Message = "Error del formato html";
                        }
                        else
                        {
                            objResult.Success = true;
                            objResult.Message = "Se genero el excel correctamente";
                            Session[_session_ListarConsulta_TransitosArt_Excel] = cadena;
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

        public string get_html_ListarConsulta_TransitosArt_str(List<Ent_Articulo_TransitosArt> _ListarConsulta_TransitosArt, Ent_Articulo_TransitosArt _Ent)
        {
            StringBuilder sb = new StringBuilder();
            var Lista = _ListarConsulta_TransitosArt.ToList();
            try
            {
                sb.Append("<div>");
                sb.Append("<table cellspacing='0' style='width: 1000px' rules='all' border='0' style='border-collapse:collapse;'>");
                sb.Append("<tr><td Colspan='10'></td></tr>");
                sb.Append("<tr><td Colspan='10' valign='middle' align='center' style='vertical-align: middle;font-size: 18.0pt;font-weight: bold;color:#285A8F'>TRANSITO POR DOCUMENTO-ARTICULO</td></tr>");
                //sb.Append("<tr><td Colspan='10' valign='middle' align='center' style='vertical-align: middle;font-size: 10.0pt;font-weight: bold;color:#000000'>Desde el " + String.Format("{0:dd/MM/yyyy}", _Ent.FechaInicio) + " hasta el " + String.Format("{0:dd/MM/yyyy}", _Ent.FechaFin) + "</td></tr>");//subtitulo
                sb.Append("<tr>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>EMPRESA</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>CADENA</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>CONCEPTO</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>ORIGEN</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>DESTINO</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>GUIA</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>ARTICULO</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>CAL</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>TALLA</font></th>\n");
                sb.Append("<th bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>CANTIDAD</font></th>\n");
                sb.Append("</tr>\n");
                // {0:N2} Separacion miles , {0:F2} solo dos decimales
                foreach (var item in Lista)
                {
                    sb.Append("<tr>\n");
                    sb.Append("<td align=''>" + item.Empresa + "</td>\n");
                    sb.Append("<td align=''>" + item.Cadena + "</td>\n");
                    sb.Append("<td align=''>" + item.Concepto + "</td>\n");
                    sb.Append("<td align=''>" + item.Origen + "</td>\n");
                    sb.Append("<td align=''>" + item.Destino + "</td>\n");
                    sb.Append("<td align='Center'>" + item.Guia + "</td>\n");
                    sb.Append("<td align='Center'>" + item.Articulo + "</td>\n");
                    sb.Append("<td align='Center'>" + item.Cal + "</td>\n");
                    sb.Append("<td align='Center'>" + item.Talla + "</td>\n");
                    sb.Append("<td align='Center'>" + item.Cantidad + "</td>\n");
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

        public ActionResult ListarConsulta_TransitosArtExcel()
        {
            string NombreArchivo = "TrasitoXdocumentoArticulo";
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
                Response.Write(Session[_session_ListarConsulta_TransitosArt_Excel].ToString());
                Response.End();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return Json(new { estado = 0, mensaje = 1 });
        }
        #endregion

        #region <ANULACION DE TRANSITOS>
        public ActionResult Anulacion_Transitos()
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
                List<Ent_concepto_Transitos> combo_concepto = new List<Ent_concepto_Transitos> {
                    new Ent_concepto_Transitos() { Con_Id ="-1",Descripcion = "--Seleccione--" }
                };
                Ent_Consulta_Transitos_Doc Ent_ConTran = new Ent_Consulta_Transitos_Doc();
                ViewBag.Ent_ConTran = Ent_ConTran;
                ViewBag.Concepto = combo_concepto.Concat(datTransitos.List_Concepto_Transitos());    

                return View();
            }
        }

        public ActionResult getConsultaDoc(Ent_Consulta_Transitos_Doc _Ent)
        {
            Ent_Consulta_Transitos_Doc Result = new Ent_Consulta_Transitos_Doc();
            JsonRespuesta objResult = new JsonRespuesta();
            try
            {
                Result = datTransitos.ValConsultaDoc(_Ent);
                if (Result.NroDocumento!=null)
                {
                    if (Result.Estado == "pendiente")
                    {
                        objResult.Data = Result;
                        objResult.Success = true;
                    }
                    else
                    {
                        objResult.Message = "El Nro docuemnto: " + Result.NroDocumento + " ya esta " + Result.Estado;
                        objResult.Data = Result;
                        objResult.Success = false;
                    }
                }
                else
                {
                    objResult.Message = "Documento no encontrado.";
                    objResult.Data = Result;
                    objResult.Success = false;
                }
            }
            catch (Exception ex)
            {
                objResult.Success = false;
                objResult.Message = "Error";
            }

            var JSON = JsonConvert.SerializeObject(objResult);
            return Json(JSON, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getAnularTransito(Ent_Consulta_Transitos_Doc _Ent)
        {
            Ent_Consulta_Transitos_Doc Result = new Ent_Consulta_Transitos_Doc();

            JsonRespuesta objResult = new JsonRespuesta();
            try
            {
                DateTime Hoy = DateTime.Now;
                _Ent.FechaAnulacion = Hoy;
                Result = datTransitos.ValConsultaDoc(_Ent);
                if (Result.NroDocumento != null)
                {
                    Result = datTransitos.DelAnularTransito(_Ent);
                    if (Result.IsOk == 1)
                    {
                        objResult.Success = true;
                        objResult.Message = Result.Mensaje;
                    }
                    else
                    {
                        objResult.Success = false;
                        objResult.Message = Result.Mensaje;
                    }
                }
                else
                {                    
                    objResult.Success = false;
                    objResult.Message = "Documento no encontrado.";
                }
            }
            catch (Exception ex)
            {
                objResult.Success = false;
                objResult.Message = "Error al registrar";
            }

            var JSON = JsonConvert.SerializeObject(objResult);
            return Json(JSON, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}