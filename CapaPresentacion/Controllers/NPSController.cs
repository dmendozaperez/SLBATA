using CapaEntidad.Control;
using CapaDato.NPS;
using CapaDato.Maestros;
using CapaEntidad.NPS;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using CapaEntidad.Control;

namespace CapaPresentacion.Controllers
{
    public class NPSController : Controller
    {
        #region <DECLARACION DE VARIABLES>
        private Dat_NPS dat_NPS = new Dat_NPS();
        private Dat_NPS_Dashboard dat_NPS_Dashboard = new Dat_NPS_Dashboard();
        private string _session_ID = "_session_ID";
        private string _session_ListarChartEstado = "_session_ListarChartEstado";
        private string _session_ListarChartEstado_Excel = "_session_ListarChartEstado_Excel";
        private string _session_ListarChartTipo = "_session_ListarChartTipo";
        private string _session_ListarChartTipo_Excel = "_session_ListarChartTipo_Excel";
        private string _session_ListarChartDistrito = "_session_ListarChartDistrito";
        private string _session_ListarChartDistrito_Excel = "_session_ListarChartDistrito_Excel";
        #endregion

        #region <ENCUESTA BATACLUB>
        public ActionResult Index(string ID)
        {
            Session[_session_ID] = ID;
            Ent_NPS_Pregunta _Ent = new Ent_NPS_Pregunta();
            _Ent.ID = ID;

            //Lista la pregunta
            var Listar = dat_NPS.Leer_Preguntas(_Ent);

            if (Listar.COD_NPS_EST !="03")
            {
                //Actualiza en estado
                dat_NPS.Genera_Encuesta_Estado(_Ent);

                var GrpPregun = Listar._ListarPreguntas
                .GroupBy(x => new
                {
                    COD_NPS_CON = x.COD_NPS_CON,
                    COD_NPS = x.COD_NPS,
                    RANGO_NPS = x.RANGO_NPS,
                    PREGUNTA_NPS = x.PREGUNTA_NPS
                }).Select(y => new
                {
                    COD_NPS_CON = y.Key.COD_NPS_CON,
                    COD_NPS = y.Key.COD_NPS,
                    RANGO_NPS = y.Key.RANGO_NPS,
                    PREGUNTA_NPS = y.Key.PREGUNTA_NPS,
                    Alternativas = y.Select(z => new
                    {
                        DES_NPS_OPC = z.DES_NPS_OPC,
                        COD_NPS_OPC = z.COD_NPS_OPC
                    }).ToList()
                }).ToList();

                List<Ent_TMP_NPS_Respuestas> ListRespuesta = new List<Ent_TMP_NPS_Respuestas>();
                Ent_TMP_NPS_Respuestas entTmpNPS_Respuestas = new Ent_TMP_NPS_Respuestas();
                ViewBag.entTmpNPS_Respuestas = entTmpNPS_Respuestas;
                ViewBag.ListRespuesta = ListRespuesta;
                ViewBag.GrpPregun = GrpPregun;
                return View();
            }
            else
            {
                return View("Finalizado");
            }
        }
        public ActionResult Finalizado()
        {
            return View();
        }
        public ActionResult getGenerarEncuesta(List<Ent_TMP_NPS_Respuestas> _ListRespuesta, Ent_TMP_NPS_Respuestas _Ent)
        {
            bool Result = false;
            JsonRespuesta objResult = new JsonRespuesta();

            _Ent.ID = (string)Session[_session_ID];
            try
            {
                DataTable dtRespuestas = new DataTable();
                dtRespuestas.Columns.Add("COD_NPS_CON", typeof(string));
                dtRespuestas.Columns.Add("COD_NPS", typeof(string));
                dtRespuestas.Columns.Add("COD_NPS_OPC", typeof(decimal));
                dtRespuestas.Columns.Add("VALOR_NPS", typeof(int));
                var Fila = 0;
                foreach (var item in _ListRespuesta)
                {
                    dtRespuestas.Rows.Add();
                    dtRespuestas.Rows[Fila]["COD_NPS_CON"] = item.COD_NPS_CON;
                    dtRespuestas.Rows[Fila]["COD_NPS"] = item.COD_NPS;
                    dtRespuestas.Rows[Fila]["COD_NPS_OPC"] = Convert.ToDecimal(item.COD_NPS_OPC);
                    dtRespuestas.Rows[Fila]["VALOR_NPS"] = Convert.ToInt32(item.VALOR_NPS);
                    Fila++;
                }

                Result = dat_NPS.Genera_Encuesta(dtRespuestas, _Ent);
                if (Result)
                {
                    objResult.Success = true;
                    objResult.Message = "Por completar la encuestas.";
                }
                else
                {
                    objResult.Success = false;
                    objResult.Message = "¡Error! Al guardar correctamente la encuestas.";
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

        #region <BASHBOARD ENCUESTA BATABLUB>
        public ActionResult Dashboard()
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

                List<Ent_Combo_DisCadTda> combo_discadtda = dat_NPS_Dashboard.list_dis_cad_tda_NPS(pais);
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

                Ent_NPS_Dashboard_Estado entDashboardEstado = new Ent_NPS_Dashboard_Estado();
                ViewBag.entDashboardEstado = entDashboardEstado;

                Ent_NPS_Dashboard_Tipo EntDashboardTipo = new Ent_NPS_Dashboard_Tipo();
                ViewBag.EntDashboardTipo = EntDashboardTipo;

                Ent_NPS_Dashboard_Distrito entDashboardDistrito = new Ent_NPS_Dashboard_Distrito();
                ViewBag.entDashboardDistrito = entDashboardDistrito;

                List<Ent_NPS_Dashboard_Tipo> _Listar_Preguntas = dat_NPS_Dashboard._Listar_Preguntas();
                ViewBag._Listar_Preguntas = _Listar_Preguntas;
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
        public ActionResult getChartEstado(Ent_NPS_Dashboard_Estado _Ent)
        {
            Session[_session_ListarChartEstado] = null;
            JsonRespuesta objResult = new JsonRespuesta();
            Ent_NPS_Dashboard_Estado ChartEst = dat_NPS_Dashboard._getChartEstado(_Ent);
            Ent_NPS_Chart_Data chartDS = new Ent_NPS_Chart_Data();
            try
            {
                if (ChartEst._ListarChar.Count>0)
                {
                    Session[_session_ListarChartEstado] = ChartEst._ListarExcel.ToList();
                    chartDS.datasets = new List<Ent_NPS_Chart_DataSet>() {
                        (new Ent_NPS_Chart_DataSet()
                        {
                            label = "",
                            backgroundColor =new string[] { "RGBA(23,121,101,0.8)", "RGBA(245,134,52,0.7)", "RGBA(255,204,41,0.8)"},//{ "#3c8dbc", "#f56954", "#39CCCC", "#605ca8", "#ca195a", "#009473"},
                            borderWidth = "1",
                            data = ChartEst._ListarChar.Select(s=>s.TRANSAC).ToArray()
                        }) };

                    chartDS.labels = ChartEst._ListarChar.Select(s => s.DES_ESTADO).ToArray();

                    objResult.Data = chartDS;
                    objResult.Success =  true;
                }
                else
                {
                    objResult.Data = chartDS;
                    objResult.Success = false;
                } 
            }
            catch (Exception)
            {
                objResult.Message = "!Error inesperado¡";
                objResult.Data = chartDS;
                objResult.Success = false;
            }          

            var JSON = JsonConvert.SerializeObject(objResult);
            return Json(JSON, JsonRequestBehavior.AllowGet);
        }
        public ActionResult get_exporta_ListarChartEstado_excel(Ent_NPS_Dashboard_Estado _Ent)
        {
            JsonRespuesta objResult = new JsonRespuesta();
            try
            {
                Session[_session_ListarChartEstado_Excel] = null;
                string cadena = "";
                if (Session[_session_ListarChartEstado] != null)
                {

                    List<Ent_NPS_Estado_Lista> _ListarChartEstado = (List<Ent_NPS_Estado_Lista>)Session[_session_ListarChartEstado];
                    if (_ListarChartEstado.Count == 0)
                    {
                        objResult.Success = false;
                        objResult.Message = "No hay filas para exportar";

                    }
                    else
                    {
                        cadena = get_html_ListarChartEstado_str((List<Ent_NPS_Estado_Lista>)Session[_session_ListarChartEstado], _Ent);
                        if (cadena.Length == 0)
                        {
                            objResult.Success = false;
                            objResult.Message = "Error del formato html";
                        }
                        else
                        {
                            objResult.Success = true;
                            objResult.Message = "Se genero el excel correctamente";
                            Session[_session_ListarChartEstado_Excel] = cadena;
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

        public string get_html_ListarChartEstado_str(List<Ent_NPS_Estado_Lista> _ListarChartEstado, Ent_NPS_Dashboard_Estado _Ent)
        {
            StringBuilder sb = new StringBuilder();
            var Lista = _ListarChartEstado.ToList();
            try
            {
                sb.Append("<div>");
                sb.Append("<table cellspacing='0' style='width: 1000px' rules='all' border='0' style='border-collapse:collapse;'>");
                sb.Append("<tr><td Colspan='3'></td></tr>");
                sb.Append("<tr><td Colspan='3' valign='middle' align='center' style='vertical-align: middle;font-size: 16.0pt;font-weight: bold;color:#285A8F'>NPS ESTADO</td></tr>");
                //sb.Append("<tr><td Colspan='3' valign='middle' align='center' style='vertical-align: middle;font-size: 10.0pt;font-weight: bold;color:#000000'>Desde el " + String.Format("{0:dd/MM/yyyy}", _Ent.FEC_INI) + " hasta el " + String.Format("{0:dd/MM/yyyy}", _Ent.FEC_FIN) + "</td></tr>");//subtitulo
                sb.Append("<tr><td Colspan='3'></td></tr>");
                sb.Append("</table>");
                sb.Append("<table  border='1' bgColor='#ffffff' borderColor='#FFFFFF' cellSpacing='2' cellPadding='2' style='font-size:10.0pt; font-family:Calibri; background:white;width: 1000px'>");
                sb.Append("<tr bgColor='#1E77AB'>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>ESTADO</font></th>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>DNI</font></th>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>CORREO</font></th>\n");
                sb.Append("</tr>\n");
                foreach (var item in Lista)
                {
                    sb.Append("<tr>\n");
                    sb.Append("<td align=''>" + item.DES_ESTADO + "</td>\n");
                    sb.Append("<td align=''>" + item.DNI + "</td>\n");
                    sb.Append("<td align=''>" + item.CORREO + "</td>\n");

                    sb.Append("</tr>\n");
                }
                sb.Append("</table></div>");
            }
            catch
            {
                sb.Append("");
            }
            return sb.ToString();
        }

        public ActionResult ListarChartEstadoExcel()
        {
            string NombreArchivo = "ListasPorEstado";
            String style = style = @"<style> .textmode { mso-number-format:\@; } .textDecim { mso-number-format:0.00} </style> ";
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + NombreArchivo + ".xls");
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.Write(style);
                Response.Write(Session[_session_ListarChartEstado_Excel].ToString());
                Response.End();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            return Json(new { estado = 0, mensaje = 1 });
        }
        public ActionResult getChartTipo(Ent_NPS_Dashboard_Tipo _Ent)
        {
            Session[_session_ListarChartTipo] = null;
            JsonRespuesta objResult = new JsonRespuesta();
            Ent_Nps_Tipo_Chart_Data chartDS = new Ent_Nps_Tipo_Chart_Data();
            Ent_NPS_Dashboard_Tipo ChartTipo = dat_NPS_Dashboard._getChartTipo(_Ent);

            try
            {
                if (ChartTipo._ListarChar.Count>0)
                {
                    Session[_session_ListarChartTipo] = ChartTipo._ListarExcel.ToList();

                    var Nota = ChartTipo._ListarNota.Select(x => new { NOTA = x.NOTA }).ToList().ElementAt(0).NOTA.ToString();

                    var Total_respuesta = ChartTipo._ListarNota.Select(x => new { TOTAL_RESPUESTAS = x.TOTAL_RESPUESTAS }).ToList().ElementAt(0).TOTAL_RESPUESTAS.ToString();

                    chartDS.datasets = new List<Ent_Nps_Tipo_Chart_DataSet>() {
                    (new Ent_Nps_Tipo_Chart_DataSet()
                    {
                        label = "",
                        backgroundColor = ChartTipo._ListarChar.Select(s=>s.COLOR_RGBA).ToArray(),//new string[] { "#3c8dbc", "#f56954", "#39CCCC", "#605ca8", "#ca195a", "#009473"},//
                        borderWidth = "1",
                        data = ChartTipo._ListarChar.Select(s=>s.VALOR_PORC).ToArray()
                    }) };

                    chartDS.labels = ChartTipo._ListarChar.Select(s => s.DES_TIP_RES).ToArray();

                    chartDS.Notas = Nota;
                    chartDS.Respuesta = Total_respuesta;
                    objResult.Data = chartDS;
                    objResult.Success =  true;
                }
                else
                {
                    objResult.Data = chartDS;
                    objResult.Success =  false;
                }
            }
            catch (Exception)
            {
                objResult.Message = "!Error inesperado¡";
                objResult.Data = chartDS;
                objResult.Success = false;
            }                       

            var JSON = JsonConvert.SerializeObject(objResult);
            return Json(JSON, JsonRequestBehavior.AllowGet);
        }
        public ActionResult get_exporta_ListarChartTipo_excel(Ent_NPS_Dashboard_Tipo _Ent)
        {
            JsonRespuesta objResult = new JsonRespuesta();
            try
            {
                Session[_session_ListarChartTipo_Excel] = null;
                string cadena = "";
                if (Session[_session_ListarChartTipo] != null)
                {

                    List<Ent_NPS_Tipoo_Lista> _ListarChartTipo = (List<Ent_NPS_Tipoo_Lista>)Session[_session_ListarChartTipo];
                    if (_ListarChartTipo.Count == 0)
                    {
                        objResult.Success = false;
                        objResult.Message = "No hay filas para exportar";
                    }
                    else
                    {
                        cadena = get_html_ListarChartTipo_str((List<Ent_NPS_Tipoo_Lista>)Session[_session_ListarChartTipo], _Ent);
                        if (cadena.Length == 0)
                        {
                            objResult.Success = false;
                            objResult.Message = "Error del formato html";
                        }
                        else
                        {
                            objResult.Success = true;
                            objResult.Message = "Se genero el excel correctamente";
                            Session[_session_ListarChartTipo_Excel] = cadena;
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

        public string get_html_ListarChartTipo_str(List<Ent_NPS_Tipoo_Lista> _ListarChartTipo, Ent_NPS_Dashboard_Tipo _Ent)
        {
            StringBuilder sb = new StringBuilder();
            var Lista = _ListarChartTipo.ToList();
            try
            {
                sb.Append("<div>");
                sb.Append("<table cellspacing='0' style='width: 1000px' rules='all' border='0' style='border-collapse:collapse;'>");
                sb.Append("<tr><td Colspan='3'></td></tr>");
                sb.Append("<tr><td Colspan='3' valign='middle' align='center' style='vertical-align: middle;font-size: 16.0pt;font-weight: bold;color:#285A8F'>NPS TIPO</td></tr>");
                sb.Append("<tr><td Colspan='3'></td></tr>");
                sb.Append("</table>");
                sb.Append("<table  border='1' bgColor='#ffffff' borderColor='#FFFFFF' cellSpacing='2' cellPadding='2' style='font-size:10.0pt; font-family:Calibri; background:white;width: 1000px'>");
                sb.Append("<tr bgColor='#1E77AB'>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>TIPO</font></th>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>DNI</font></th>\n");
                sb.Append("<th style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>CORREO</font></th>\n");
                sb.Append("</tr>\n");
                foreach (var item in Lista)
                {
                    sb.Append("<tr>\n");
                    sb.Append("<td align=''>" + item.TIPO + "</td>\n");
                    sb.Append("<td align=''>" + item.DNI + "</td>\n");
                    sb.Append("<td align=''>" + item.CORREO + "</td>\n");

                    sb.Append("</tr>\n");
                }
                sb.Append("</table></div>");
            }
            catch
            {
                sb.Append("");
            }
            return sb.ToString();
        }

        public ActionResult ListarChartTipoExcel()
        {
            string NombreArchivo = "ListasPorTipo";
            String style = style = @"<style> .textmode { mso-number-format:\@; } .textDecim { mso-number-format:0.00} </style> ";
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + NombreArchivo + ".xls");
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.Write(style);
                Response.Write(Session[_session_ListarChartTipo_Excel].ToString());
                Response.End();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            return Json(new { estado = 0, mensaje = 1 });
        }
        public ActionResult _getCaliPorDistrito(Ent_NPS_Dashboard_Distrito _Ent)
        {
            Session[_session_ListarChartDistrito] = null;
            JsonRespuesta objResult = new JsonRespuesta();
            Ent_NPS_Dashboard_Distrito _EntResul = new Ent_NPS_Dashboard_Distrito();
            Ent_NPS_Dashboard_Distrito ChartDistrito = dat_NPS_Dashboard._getCaliPorDistrito(_Ent);

            try
            {
                if (ChartDistrito._ListarChar.Count>0)
                {
                    Session[_session_ListarChartDistrito] = ChartDistrito._ListarExcel.ToList();
                    var _Listar = ChartDistrito._ListarChar.ToList();
                    _EntResul.Rows = _Listar.GroupBy(x => new { x.COD_NPS, x.PREGUNTA_NPS }).Select(y => new { COD_NPS = y.Key.COD_NPS, PREGUNTA_NPS = y.Key.PREGUNTA_NPS }).ToArray();
                    _EntResul.Cols = _Listar.GroupBy(x => new { x.DISTRITO }).Select(y => new { DISTRITO = y.Key.DISTRITO }).ToArray();
                    _EntResul.Data = _Listar.ToArray();
                    objResult.Data = _EntResul;
                    objResult.Success =  true;
                }
                else
                {
                    objResult.Message = "!Error inesperado¡";
                    objResult.Data = ChartDistrito;
                    objResult.Success = false;
                }
            }
            catch (Exception e)
            {
                objResult.Message = "!Error inesperado¡";
                objResult.Data = ChartDistrito;
                objResult.Success = false;
            }
            
            var JSON = JsonConvert.SerializeObject(objResult);
            return Json(JSON, JsonRequestBehavior.AllowGet);
        }

        public ActionResult get_exporta_ListarChartDistrito_excel(Ent_NPS_Dashboard_Distrito _Ent)
        {
            JsonRespuesta objResult = new JsonRespuesta();
            try
            {
                Session[_session_ListarChartDistrito_Excel] = null;
                string cadena = "";
                if (Session[_session_ListarChartDistrito] != null)
                {

                    List<Ent_NPS_Distrito_Listar> _ListarChartDistrito = (List<Ent_NPS_Distrito_Listar>)Session[_session_ListarChartDistrito];
                    if (_ListarChartDistrito.Count == 0)
                    {
                        objResult.Success = false;
                        objResult.Message = "No hay filas para exportar";
                    }
                    else
                    {
                        cadena = get_html_ListarChartDistrito_str((List<Ent_NPS_Distrito_Listar>)Session[_session_ListarChartDistrito], _Ent);
                        if (cadena.Length == 0)
                        {
                            objResult.Success = false;
                            objResult.Message = "Error del formato html";
                        }
                        else
                        {
                            objResult.Success = true;
                            objResult.Message = "Se genero el excel correctamente";
                            Session[_session_ListarChartDistrito_Excel] = cadena;
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

        public string get_html_ListarChartDistrito_str(List<Ent_NPS_Distrito_Listar> _ListarChartDistrito, Ent_NPS_Dashboard_Distrito _Ent)
        {
            StringBuilder sb = new StringBuilder();
            var Lista = _ListarChartDistrito.ToList();
            var Cols = Lista.GroupBy(x => new { x.COD_NPS, x.PREGUNTA_NPS }).Select(y => new { COD_NPS = y.Key.COD_NPS, PREGUNTA_NPS = y.Key.PREGUNTA_NPS }).ToList();
            var Rows = Lista.GroupBy(A => new { A.DISTRITO, A.TIENDA }).Select(y => new { DISTRITO = y.Key.DISTRITO, TIENDA = y.Key.TIENDA }).ToList();
            string Cabecera = "";
            try
            {
                foreach (var item in Cols)
                {
                    Cabecera += "<th class='TdXlx' rowspan=4 width=95 style='mso-width-source:userset;width:100pt;text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>" + item.PREGUNTA_NPS + "</font></th>";
                }

                sb.Append("<div x:publishsource='Excel'>");

                sb.Append("<table border=0 cellpadding=0 cellspacing=0 width=706 style='border-collapse: collapse; table-layout:fixed; width: 529pt'>");
                sb.Append("<tr height=100 style='mso-height-source:userset;height:50pt'><td Colspan='11' valign='middle' align='center' style='vertical-align: middle;font-size: 16.0pt;font-weight: bold;color:#285A8F'>CALIFICACION POR DISTRITO</td></tr>");                
      
                sb.Append("<tr height=100 style='mso-height-source:userset;height:50pt'>");
                sb.Append("<th width=60 style='width:60pt'><font color='#FFFFFF'></font></th>\n");
                sb.Append("<th width=60 style='width:60pt'><font color='#FFFFFF'></font></th>\n");
                sb.Append(Cabecera);
                sb.Append("</tr>");

                sb.Append("<tr>\n");
                sb.Append("<td><font color='#FFFFFF'></font></td>\n");
                sb.Append("<td ><font color='#FFFFFF'></font></td>\n");
                sb.Append("</tr>\n");

                sb.Append("<tr>\n");
                sb.Append("<td ><font color='#FFFFFF'></font></td>\n");
                sb.Append("<td ><font color='#FFFFFF'></font></td>\n");
                sb.Append("</tr>\n");

                sb.Append("<tr >\n");
                sb.Append("<th bgColor='#3D8AB8' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>DISTRITO</font></th>\n");
                sb.Append("<th bgColor='#3D8AB8' style='text-align: center; font-weight:bold;font-size:11.0pt;'><font color='#FFFFFF'>TIENDA</font></th>\n");
                sb.Append("</tr>\n");
                sb.Append("<tbody>\n");
                foreach (var itemR in Rows)
                {
                    sb.Append("<tr>\n");
                    sb.Append("<td align='Center' bgColor='#e2f4fe'>" + itemR.DISTRITO + "</td>\n");
                    sb.Append("<td align='Center' bgColor='#e2f4fe'>" + itemR.TIENDA + "</td>\n");
                    foreach (var itemC in Cols)
                    {
                        var Result = Lista.Where(x => x.DISTRITO == itemR.DISTRITO && x.TIENDA == itemR.TIENDA && x.COD_NPS == itemC.COD_NPS).Select(y => new { NOTA = y.NOTA, COLOR = y.COLOR_RGBA }).ToList(); ;
                        if (Result.Count()>0)
                        {                            
                            sb.Append("<td style='width:10%;background-color: " + ConvertRgbaToHex(Result.ElementAt(0).COLOR.ToString().ToLower()) + ";font-weight:bold;font-size:11.0pt;' align='Center'><font color='#FFFFFF'>" + Result.ElementAt(0).NOTA + "</font></td>");
                        }
                        else
                        {
                            sb.Append("<td align=''></td>\n");
                        }
                    }
                    sb.Append("</tr>\n");
                }
                sb.Append("</tbody>");
                sb.Append("</table></div>");
            }
            catch
            {
                sb.Append("");
            }
            return sb.ToString();
        }
        public static string ConvertRgbaToHex(string rgba)
        {
            if (!Regex.IsMatch(rgba, @"rgba\((\d{1,3},\s*){3}(0(\.\d+)?|1)\)"))
                throw new FormatException("rgba string was in a wrong format");
            var matches = Regex.Matches(rgba, @"\d+");
            StringBuilder hexaString = new StringBuilder("#");

            for (int i = 0; i < matches.Count - 2; i++)
            {
                int value = Int32.Parse(matches[i].Value);

                hexaString.Append(value.ToString("X"));
            }
            return hexaString.ToString();
        }


        public ActionResult ListarChartDistritoExcel()
        {
            string NombreArchivo = "CalificacionPorDistrito";
            String style = style = @"<style> .textmode { mso-number-format:\@; } .textDecim { mso-number-format:0.00} 
                            .TdXlx1
	                                {padding-top:1px;
	                                padding-right:1px;
	                                padding-left:1px;
	                                mso-ignore:padding;
	                                color:black;
	                                font-size:11.0pt;
	                                font-weight:400;
	                                font-style:normal;
	                                text-decoration:none;
	                                font-family:Calibri, sans-serif;
	                                mso-font-charset:0;
	                                mso-number-format:General;
	                                text-align:general;
	                                vertical-align:bottom;
	                                mso-background-source:auto;
	                                mso-pattern:auto;
	                                white-space:nowrap;}
                            .TdXlx {
                                    padding-top:1px;
                                    padding-right:1px;
                                    padding-left:1px;
                                    mso-ignore:padding;
                                    color:white;
                                    font-size:12.0pt;
                                    font-weight:700;
                                    font-style:normal;
                                    text-decoration:none;
                                    font-family:Calibri, sans-serif;
                                    mso-font-charset:0;
                                    mso-number-format:General;
                                    text-align:center;
                                    vertical-align:middle;
                                    border-top:.5pt solid white;
                                    border-right:.5pt solid white;
                                    border-bottom:.5pt solid white;
                                    border-left:none;
                                    background:#3D8AB8;
                                    mso-pattern:black none;
                                    white-space:normal;
                                  } </style> ";
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + NombreArchivo + ".xls");
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.Write(style);
                Response.Write(Session[_session_ListarChartDistrito_Excel].ToString());
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