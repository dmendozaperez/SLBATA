using CapaDato.Contabilidad;
using CapaDato.Maestros;
using CapaDato.Reporte;
using CapaEntidad.Contabilidad;
using CapaEntidad.Control;
using CapaEntidad.General;
using CapaEntidad.Maestros;
using CapaEntidad.Util;
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
    public class ContabilidadController : Controller
    {
        private Dat_ListaTienda dat_lista_tienda = new Dat_ListaTienda();
        private Dat_Contabilidad_EstadoDocumento datConta = new Dat_Contabilidad_EstadoDocumento();
        private Data_Contabilidad dat_Contabilidad = new Data_Contabilidad();
        private Dat_DisCadTda discattda = new Dat_DisCadTda();

        private string _session_contabilidad_num_private = "_session_contabilidad_num_private";
        private string _session_contb_tienda_peru = "_session_contb_tienda_peru";
        private string _session_contb_popup = "_session_contb_popup";
        private string _session_VentaEfectivo = "_session_VentaEfectivo";
        private string _session_VentaEfectivo_Excel = "_session_VentaEfectivo_Excel";

        //_session_VentaEfectivo_Excel
        // GET: Contabilidad

        #region Contabilidad_Estado_Documento   
        //INDEX
        public ActionResult Estado_Documento()
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
                if (Session["PAIS"].ToString() == "PE") // LISTA DE TIENDAS PARA BATA ECUADOR
                {
                    ViewBag.Tienda = dat_lista_tienda.get_tienda("PE", "1");
                    List<Ent_ListaTienda> listienda = ViewBag.Tienda;
                    Session[_session_contb_tienda_peru] = listienda;
                }
                else
                {
                    ViewBag.Tienda = dat_lista_tienda.get_tienda("EC", "1");
                    List<Ent_ListaTienda> listienda = ViewBag.Tienda;
                    Session[_session_contb_tienda_peru] = listienda;
                }

            }
            return View();
            //else
            //{
            //if (Session[_session_contb_tienda_peru] != null)
            //{
            //    ViewBag.Tienda = Session["_session_contb_tienda_peru"];
            //}
            //else
            //{
            //    ViewBag.Tienda = dat_lista_tienda.get_tienda("PE", "1");
            //    List<Ent_ListaTienda> listienda = ViewBag.Tienda;
            //    Session[_session_contb_tienda_peru] = listienda;
            //}

            //return View();
            //  }
        }

        public List<Ent_Contabilidad_EstadoDocumento> listaTable(string cod_entid, DateTime fec_ini, DateTime fec_fin)
        {
            List<Ent_Contabilidad_EstadoDocumento> listaTable = datConta.get_lista(cod_entid, fec_ini, fec_fin);
            Session[_session_contabilidad_num_private] = listaTable;
            return listaTable;
        }

        public PartialViewResult _Table(string hidden, string fec_ini, string fec_fin, string dwtda)
        {
            if (hidden == null || hidden == "")
            { return PartialView(); }
            else
            {   //string dwtda--> se reemplaza por hidden - para agarrar varios id de tiendas por el combo multiselect
                return PartialView(listaTable(hidden, Convert.ToDateTime(fec_ini), Convert.ToDateTime(fec_fin)));
            }
        }

        public ActionResult getDetAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_contabilidad_num_private] == null)
            {
                List<Ent_Contabilidad_EstadoDocumento> listdoc = new List<Ent_Contabilidad_EstadoDocumento>();
                Session[_session_contabilidad_num_private] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_Contabilidad_EstadoDocumento> membercol = ((List<Ent_Contabilidad_EstadoDocumento>)(Session[_session_contabilidad_num_private])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Contabilidad_EstadoDocumento> filteredMembers = membercol;

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Ent_Contabilidad_EstadoDocumento, string> orderingFunction =
                   (
                   m => sortIdx == 0 ? m.fecha :
                    m.fecha
                   );
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.estado.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.fecha.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.numero.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.serie.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.tienda.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.tipo_doc.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.ruc.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.login_ws.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.clave_ws.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.tipodoc.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.folio.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.total.ToUpper().Contains(param.sSearch.ToUpper()));
            }
            var sortDirection = Request["sSortDir_0"];
            if (sortDirection == "desc")
                filteredMembers = filteredMembers.OrderBy(orderingFunction);
            else
                filteredMembers = filteredMembers.OrderByDescending(orderingFunction);
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.estado,
                             a.fecha,
                             a.numero,
                             a.serie,
                             a.tienda,
                             a.tipo_doc,
                             a.total,
                             a.ruc,
                             a.login_ws,
                             a.clave_ws,
                             a.tipodoc,
                             a.folio
                         };

            //Se devuelven los resultados por json
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }
        //POP UP - DETALLE

        public List<Ent_Contabilidad_EstadoDocumento_Det> listarStr_Detalle_PopUp(string ruc, string login_ws, string clave_ws, string tipodoc, string folio)
        {
            List<Ent_Contabilidad_EstadoDocumento_Det> listguia = datConta.listarStr_Detalle_PopUp(ruc, login_ws, clave_ws, tipodoc, folio);
            Session[_session_contb_popup] = listguia;
            return listguia;
        }

        public PartialViewResult _popUpDetalle(string ruc, string login_ws, string clave_ws, string tipodoc, string folio)
        {
            return PartialView(listarStr_Detalle_PopUp(ruc, login_ws, clave_ws, tipodoc, folio));
        }

        public ActionResult getDetalleAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_contb_popup] == null)
            {
                List<Ent_Contabilidad_EstadoDocumento_Det> listdoc = new List<Ent_Contabilidad_EstadoDocumento_Det>();
                Session[_session_contb_popup] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_Contabilidad_EstadoDocumento_Det> membercol = ((List<Ent_Contabilidad_EstadoDocumento_Det>)(Session[_session_contb_popup])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Contabilidad_EstadoDocumento_Det> filteredMembers = membercol;

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Ent_Contabilidad_EstadoDocumento_Det, string> orderingFunction =
          (
          m => sortIdx == 0 ? m.ESTADO :
           m.ESTADO
          );
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.PDF.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.ESTADO.ToUpper().Contains(param.sSearch.ToUpper()));
            }
            var sortDirection = Request["sSortDir_0"];
            if (sortDirection == "desc")
                filteredMembers = filteredMembers.OrderBy(orderingFunction);
            else
                filteredMembers = filteredMembers.OrderByDescending(orderingFunction);
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);


            var result = from a in displayMembers
                         select new
                         {
                             a.PDF,
                             a.ESTADO
                         };

            //Se devuelven los resultados por json
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region <REPORTE DE VENTAS EFECTIVO>

        public ActionResult ReporteVentaEfectivo()
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

                ViewBag.Cadena = list_cad;
                ViewBag.Tienda = list_tda;

                Models_VentaEfectivo EntVentaEfectivo = new Models_VentaEfectivo();
                ViewBag.EntVentaEfectivo = EntVentaEfectivo;

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
        public ActionResult ShowGenericReportVentaEfectivo(Models_VentaEfectivo _Ent)
        {
            Session[_session_VentaEfectivo] = null;
            JsonRespuesta objResult = new JsonRespuesta();
            List<Models_VentaEfectivo> lisVentaEfectivo = dat_Contabilidad.get_reporteVentaEfectivo(_Ent);
            try
            {
                if (lisVentaEfectivo.Count>0)
                {                    
                    Session[_session_VentaEfectivo] = lisVentaEfectivo;

                    this.HttpContext.Session["ReportName"] = "ReportVentaEfectivo.rpt";
                    this.HttpContext.Session["rptSource"] = lisVentaEfectivo;

                    objResult.Success =  true;
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

        public ActionResult get_exporta_VentaEfectivo_excel(Models_VentaEfectivo _Ent)
        {
            JsonRespuesta objResult = new JsonRespuesta();
            try
            {
                Session[_session_VentaEfectivo_Excel] = null;
                string cadena = "";
                if (Session[_session_VentaEfectivo] != null)
                {

                    List<Models_VentaEfectivo> _Listar_VentaEfectivo = (List<Models_VentaEfectivo>)Session[_session_VentaEfectivo];
                    if (_Listar_VentaEfectivo.Count == 0)
                    {
                        objResult.Success = false;
                        objResult.Message = "No hay filas para exportar";
                    }
                    else
                    {
                        cadena = get_html_Listar_VentaEfectivo_str((List<Models_VentaEfectivo>)Session[_session_VentaEfectivo], _Ent);
                        if (cadena.Length == 0)
                        {
                            objResult.Success = false;
                            objResult.Message = "Error del formato html";
                        }
                        else
                        {
                            objResult.Success = true;
                            objResult.Message = "Se genero el excel correctamente";
                            Session[_session_VentaEfectivo_Excel] = cadena;
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

        public string get_html_Listar_VentaEfectivo_str(List<Models_VentaEfectivo> _Listar_VentaEfectivo, Models_VentaEfectivo _Ent)
        {
            StringBuilder sb = new StringBuilder();
            var Lista = _Listar_VentaEfectivo.ToList();
            try
            {
                sb.Append("<div>");
                sb.Append("<table cellspacing='0' style='width: 1000px' rules='all' border='0' style='border-collapse:collapse;'>");
                sb.Append("<tr><td Colspan='7'></td></tr>");
                sb.Append("<tr><td Colspan='7' valign='middle' align='center' style='vertical-align: middle;font-size: 18.0pt;font-weight: bold;color:#285A8F'>REPORTE DE VENTAS EFECTIVO</td></tr>");
                sb.Append("<tr><td Colspan='7' valign='middle' align='center' style='vertical-align: middle;font-size: 10.0pt;font-weight: bold;color:#000000'>Rango : del " + String.Format("{0:dd/MM/yyyy}", _Ent.FechaInicio) + " al " + String.Format("{0:dd/MM/yyyy}", _Ent.FechaFin) + "</td></tr>");//subtitulo
                sb.Append("<tr><td Colspan='7'></td></tr>");
                sb.Append("<tr>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Distrito</font></th>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Cadena</font></th>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Nombre Tienda</font></th>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Documento</font></th>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Fecha</font></th>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Total</font></th>\n");
                sb.Append("<th  bgColor='#1E77AB' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>Redondeo</font></th>\n");
                sb.Append("</tr>\n");

                foreach (var item in Lista)
                {
                    sb.Append("<tr>\n");
                    sb.Append("<td align=''>" + item.Distrito + "</td>\n");
                    sb.Append("<td align=''>" + item.Cadena + "</td>\n");
                    sb.Append("<td align=''>" + item.Nombre_Tienda + "</td>\n");
                    sb.Append("<td align='Center'>" + item.Documento + "</td>\n");
                    sb.Append("<td align='Center'>" + String.Format("{0:d}", item.Fecha) + "</td>\n");                    
                    sb.Append("<td align='right'>" + Convert.ToDecimal(string.Format("{0:F2}", item.Total)) + "</td>");
                    sb.Append("<td align='right'>" + Convert.ToDecimal(string.Format("{0:F2}", item.Redondeo)) + "</td>");
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
        public ActionResult ListarVentaEfectivo_Excel()
        {
            string NombreArchivo = "ReporteVentaEfectivo";
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
                Response.Write(Session[_session_VentaEfectivo_Excel].ToString());
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