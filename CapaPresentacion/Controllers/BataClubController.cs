using CapaDato.BataClub;
using CapaDato.Maestros;
using CapaDato.OrceExclud;
using CapaDato.Reporte;
using CapaEntidad.BataClub;
using CapaEntidad.Control;
using CapaEntidad.General;
using CapaEntidad.Util;
using CapaPresentacion.Bll;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace CapaPresentacion.Controllers
{
    public class BataClubController : Controller
    {

        private Dat_BataClub_CuponesCO datProm = new Dat_BataClub_CuponesCO();
        private Dat_BataClub_Cliente datCli = new Dat_BataClub_Cliente();
        private Dat_BataClub_Dashboard datDash = new Dat_BataClub_Dashboard();
        private Dat_Combo datCbo = new Dat_Combo();
        Dat_OrceExclud datOE = new Dat_OrceExclud();
        private Dat_Canal datCan = new Dat_Canal();
        private Dat_Ubigeo datUbi = new Dat_Ubigeo();
        private Dat_BataClub_Tablet datTab = new Dat_BataClub_Tablet();
        private string _session_tabla_cupones = "_session_tabla_cupones";
        private string _session_lista_promociones = "_session_lista_promociones";
        private string _session_lista_clientes_cupon = "_session_lista_clientes_cupon";
        private string _session_lista_cupones_excel = "_session_lista_cupones_excel";

        private string _session_det_tdas_sup = "_session_det_tdas_sup";
        //DetallesTipoCompra
        private string _session_DetallesTipoCompra = "_session_DetallesTipoCompra";

        private string _session_CompraCliente = "_session_CompraCliente";

        private string _session_det_tdas_sup_excel = "_session_det_tdas_sup_excel";
        private string _session_par_sol_mes_excel = "_session_par_sol_mes_excel";

        #region SessionTablet
        private string _session_boleta_encuesta = "_session_boleta_encuesta";
        #endregion

        private string _session_prom_generar_cupon = "_session_prom_generar_cupon";

        private string _BataClub_Promociones_Combo = "_BataClub_Promociones_Combo";
        private string _BataClub_Canal_Combo = "_BataClub_Canal_Combo";
        private string _session_tabla_cupon_private = "_session_tabla_cupon_private";
        private string _session_tabla_cliente_private = "_session_tabla_cliente_private";
        private string _session_tabla_cupon_exportar_private = "_session_tabla_cupon_exportar_private";
        private string _BataClub_cupon_Combo = "_BataClub_cupon_Combo";
        private string _BataClub_Cupon_Desc = "_BataClub_Cupon_Desc";
        private string _BataClub_Cupon_FechaFin = "_BataClub_Cupon_FechaFin";
        private string _BataClub_Cupon_Pares = "_BataClub_Cupon_Pares";
        private string _BataClub_Promociones_estado = "_BataClub_Promociones_estado";
        private string _BataClub_Promociones_grafica = "_BataClub_Promociones_grafica";

        private string _BC_Dashboard_data_CVB = "_BC_Dashboard_data_CVB";
        private string _BC_Dashboard_Distritos = "_BC_Dashboard_Distritos";
        // GET: BataClub
        #region Bataclub/Index
        public ActionResult Index()
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
                ViewBag.Estado = datProm.get_ListaEstados();
                ViewBag.Promocion = datProm.get_ListaPromociones();
                Session[_session_tabla_cupones] = null;
                return View();
            }
        }
        public ActionResult Promociones()
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
                List<Ent_BataClub_Promociones> proms = datProm.get_ListaPromociones();
                Session[_session_lista_promociones] = proms;
                return View();
            }
        }
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
                if (Session["_dashboardData"] == null)
                {
                    Ent_BataClub_DashBoard dashboard_session = null;
                    Session["_dashboardData"] = datDash.GET_INFO_DASHBOARD(ref dashboard_session);
                }
                //Ent_BataClub_DashBoard dashboard_session = null;
                //Session["_dashboardData"] = datDash.GET_INFO_DASHBOARD(ref dashboard_session);

                Ent_BataClub_DashBoard dashboard = (Ent_BataClub_DashBoard)Session["_dashboardData"];
                ViewBag.general = datDash.BATACLUB_DASHBOARD_GENERAL(); // Informacion general.
                //ViewBag.chartDS = informeBarChartData(dashboard, 2); // Barras mensual regsitros/miembros
                //ViewBag.totalesGeneros = dashboard.listMesGenero;
                //ViewBag.chartDonut = informeCanales(dashboard); // Donut anual canales
                ViewBag.chartMesParesSoles = informeBarChartData(dashboard, 4);
                ViewBag.promsPS = dashboard.listPromsPS;
                ViewBag.anios = datCbo.get_lista_anios(2015);

                ViewBag.chartIncompletos = informeDataIncompleto(dashboard);

                ViewBag.BarChartTranReg = informeBarChartData(dashboard, 6);
                ViewBag.BarChartCompras = informeBarChartData(dashboard, 7); //informeCompras(dashboard);  // informeBarChartData(dashboard, 7);

                //ViewBag.DetallesTiendaSuperv = dashboard.listTiendasSupervTot;

                ViewBag.chartComCL = informeCompraCl(dashboard);

                ViewBag.DetallesTipoCompra = dashboard.listTipoComprasTot;

                ViewBag.Semana = datCbo.get_ListaSemana();

                Session[_session_DetallesTipoCompra] = dashboard.listTipoComprasTot;

               // Session[_session_det_tdas_sup] = dashboard.listTiendasSupervTot;
                Session[_session_par_sol_mes_excel] = dashboard.listPromsPS;

                List<Ent_BataClub_Canalventa> list_canal_venta = new List<Ent_BataClub_Canalventa>();
                Ent_BataClub_Canalventa canal_venta = new Ent_BataClub_Canalventa();
                canal_venta.can_venta_id = "TD";
                canal_venta.can_venta_des = "TIENDAS";
                list_canal_venta.Add(canal_venta);
                canal_venta = new Ent_BataClub_Canalventa();
                canal_venta.can_venta_id = "EC";
                canal_venta.can_venta_des = "ECOMMERCE";
                list_canal_venta.Add(canal_venta);

                ViewBag.canal_venta = list_canal_venta;
                //Session[_session_det_tdas_sup_excel] = dashboard.listTiendasSupervTot;
                return View();
            }
        }

        public ActionResult GetChartDist(string fechaini = null, string fechafin = null , string refresh = null)
        {
            Ent_BataClub_Chart_Data chartDS = new Ent_BataClub_Chart_Data();
            List<Ent_BataClub_DashBoard_Distritos> resumen = new List<Ent_BataClub_DashBoard_Distritos>();
            List<Ent_BataClub_DashBoard_Tiendas_Distritos> detalles = new List<Ent_BataClub_DashBoard_Tiendas_Distritos>();
            Ent_BataClub_DashBoard data = new Ent_BataClub_DashBoard(); //datDash.get_info_distritos(Convert.ToDateTime(fechaini), Convert.ToDateTime(fechafin));
            if (String.IsNullOrEmpty(refresh))
            {
                data = datDash.get_info_distritos(Convert.ToDateTime(fechaini), Convert.ToDateTime(fechafin));
                //data = data.OrderByDescending(o => o.porc).ToList();
                Session[_BC_Dashboard_Distritos] = data;
            }
            else
            {
                if (Session[_BC_Dashboard_Distritos] == null)
                {
                    data = datDash.get_info_distritos(Convert.ToDateTime(fechaini), Convert.ToDateTime(fechafin));
                    //data = data.OrderByDescending(o => o.porc).ToList();
                    Session[_BC_Dashboard_Distritos] = data;
                }
                else
                {
                    data = (Ent_BataClub_DashBoard)Session[_BC_Dashboard_Distritos];
                }
            }
            resumen = data.listDistritos;
            detalles = data.listDistritosTiendas;
            chartDS.datasets = new List<Ent_BataClub_Chart_DataSet>() {
                    (new Ent_BataClub_Chart_DataSet()
                    {
                        label = "TRANSACCIONES",
                        backgroundColor = Enumerable.Repeat("rgba(0, 166, 90,0.8)", resumen.Count).ToArray(),
                        borderWidth = "1",
                        data = resumen.Select(s => s.transac).ToArray()
                    }),
                    (new Ent_BataClub_Chart_DataSet()
                    {
                        label = "REGISTROS",
                        backgroundColor = Enumerable.Repeat("rgba(180, 180, 180,0.8)", resumen.Count).ToArray(), // new string[] { "rgba(180, 180, 180,0.7)" },
                        borderWidth = "1",
                        data = resumen.Select(s => s.registros).ToArray()
                    }),

                    (new Ent_BataClub_Chart_DataSet()
                    {
                        label = "CONSUMIDOS",
                        backgroundColor = Enumerable.Repeat("rgba(243, 156, 18, 0.8)", resumen.Count).ToArray(),
                        borderWidth = "1",
                        data = resumen.Select(s => s.consumido).ToArray()
                    }),

                     (new Ent_BataClub_Chart_DataSet()
                    {
                        label = "MIEMBROS",
                        backgroundColor = Enumerable.Repeat("rgba(230, 101, 101, 0.8)", resumen.Count).ToArray(),
                        borderWidth = "1",
                        data = resumen.Select(s => s.bataclub).ToArray()
                    })
                };

            chartDS.labels = resumen.Select(s => s.distrito + " (" + s.supervisor.Substring(0,s.supervisor.IndexOf(" ")) + ")").ToArray();
            return Json(new { result = JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }) , detalles  = detalles.OrderBy(o=>o.supervisor).ThenBy(t=>t.distrito).ToList() });
        }

        public ActionResult GetChartVxC(string fechaini = null, string fechafin = null)
        {
            Ent_BataClub_Chart_Data chartDS = new Ent_BataClub_Chart_Data();
            List<Ent_BC_Venta_Categoria> chartPSM = datDash.get_info_venta_categoria(Convert.ToDateTime(fechaini), Convert.ToDateTime(fechafin));
            chartDS.datasets = new List<Ent_BataClub_Chart_DataSet>() {
                (new Ent_BataClub_Chart_DataSet()
                {
                    label = "BATA",
                    backgroundColor = Enumerable.Repeat("rgba(147, 97, 228, 0.8)", chartPSM.Count).ToArray(), // new string[] { "rgba(180, 180, 180,0.7)" },
                    borderWidth = "1",
                    data = chartPSM.Select(s => s.TOTAL_BATA).ToArray()
                }),
                (new Ent_BataClub_Chart_DataSet()
                {
                    label = "BATACLUB",
                    backgroundColor = Enumerable.Repeat("rgb(80, 230, 161 , 0.8)", chartPSM.Count).ToArray(),
                    borderWidth = "1",
                    data = chartPSM.Select(s => s.TOTAL_BATACLUB).ToArray()
                }) };
            chartDS.labels = chartPSM.Select(s => s.CATEGORIA).ToArray();
            return Json(new { result = JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }) });
        }


        public ActionResult GetChartPSM (string fechaini = null , string fechafin = null)
        {
            Ent_BataClub_Chart_Data chartDS = new Ent_BataClub_Chart_Data();
            List<Ent_BataClub_Dashboard_PSM> chartPSM = datDash.GetChartPSM(fechaini , fechafin);
            chartDS.datasets = new List<Ent_BataClub_Chart_DataSet>() {
                (new Ent_BataClub_Chart_DataSet()
                {
                    label = "PARES",
                    backgroundColor = Enumerable.Repeat("rgba(147, 97, 228, 0.8)", chartPSM.Count).ToArray(), // new string[] { "rgba(180, 180, 180,0.7)" },
                    borderWidth = "1",
                    data = chartPSM.Select(s => s.pares).ToArray()
                }),
                (new Ent_BataClub_Chart_DataSet()
                {
                    label = "SOLES",
                    backgroundColor = Enumerable.Repeat("rgb(80, 230, 161 , 0.8)", chartPSM.Count).ToArray(),
                    borderWidth = "1",
                    data = chartPSM.Select(s => s.soles).ToArray()
                }) };
            chartDS.labels = chartPSM.Select(s => s.marca).ToArray();
            return Json(new { result = JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }) });
        }
        public ActionResult GetChartPPS(string fechaini = null, string fechafin = null,string canal_venta_ps= "TD,EC")
        {
            Ent_BataClub_Chart_Data chartDS = new Ent_BataClub_Chart_Data();
            Ent_BataClub_Dashboard_PPS data = datDash.BATACLUB_DASHBOARD_PPS(fechaini, fechafin, canal_venta_ps);

            Ent_BataClub_Chart_DataSet dsP = (new Ent_BataClub_Chart_DataSet()
            {
                backgroundColor = new string[] { "rgb(75, 192, 192,0.9)",
                        "rgb(255, 159, 64)"},
                data = new decimal[] { data.PORC_PARES_BATACLUB, data.PORC_PARES_BATA }
            });

            Ent_BataClub_Chart_DataSet dsS = (new Ent_BataClub_Chart_DataSet()
            {
                backgroundColor = new string[] { "rgb(52, 132, 132 , 0.8)",
                        "rgb(228, 142, 56)"},
                data = new decimal[] { data.PORC_SOLES_BATACLUB, data.PORC_SOLES_BATA }
            });

            string str_canal = "GENERAL";

            if (canal_venta_ps == "EC")
            {
                str_canal = "E-COMMERCE";
            }
            if (canal_venta_ps == "TD")
            {
                str_canal = "TIENDAS";
            }


            chartDS.labels = new string[] { "BATACLUB" , str_canal } ;
            chartDS.datasets = new List<Ent_BataClub_Chart_DataSet>() { dsP,dsS };
            return Json(new { chartDS = JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }) });
        }
        public ActionResult GetChartCVB(string[] semana , string refresh = null)
        {
            Ent_BataClub_Chart_Data chartDS = new Ent_BataClub_Chart_Data();
            List<Ent_BC_Dashboard_CVB> data = new List<Ent_BC_Dashboard_CVB>();
            if (String.IsNullOrEmpty(refresh))
            {
                data = datDash.get_info_cump_venta(String.Join(",",semana));
                data = data.OrderByDescending(o => o.n_semana).ThenBy(t=>t.porc).ToList();
                Session[_BC_Dashboard_data_CVB] = data;
            }
            else
            {
                if (Session[_BC_Dashboard_data_CVB] == null)
                {
                    data = datDash.get_info_cump_venta(String.Join(",", semana));
                    data = data.OrderByDescending(o => o.n_semana).ThenBy(t => t.porc).ToList();
                    Session[_BC_Dashboard_data_CVB] = data;
                }
                else
                {
                    data = (List<Ent_BC_Dashboard_CVB>)Session[_BC_Dashboard_data_CVB];
                }
            }
            chartDS.datasets = new List<Ent_BataClub_Chart_DataSet>();
            string[] n_semanas = data.OrderByDescending(o=>o.n_semana).Select(s => s.n_semana).Distinct().ToArray();
            /* //Detallado 
            foreach (string n_sem in n_semanas)
            {
                decimal mas100 = data.Where(w=>w.n_semana == n_sem).Count(c => c.porc >= 100);
                decimal entre90100 = data.Where(w => w.n_semana == n_sem).Count(c => c.porc >= 90 && c.porc < 100);
                decimal menos90 = data.Where(w => w.n_semana == n_sem).Count(c => c.porc < 90);
                Ent_BataClub_Chart_DataSet dsP = (new Ent_BataClub_Chart_DataSet()
                {
                    label = data.Where(w=>w.n_semana == n_sem).Select(s=>s.semana_ant + " — " + s.semana_act).FirstOrDefault().ToString(),
                    backgroundColor = new string[] { "rgba(228, 82, 82,0.9)", "rgba(243, 156, 18, 0.9)", "rgba(0, 166, 90,0.9)" },
                    data = new decimal[] { menos90, entre90100, mas100 }
                });
                chartDS.datasets.Add(dsP);
            }      
            */
            decimal mas100 = data.Count(c => c.porc >= 100);
            decimal entre90100 = data.Count(c => c.porc >= 90 && c.porc < 100);
            decimal menos90 = data.Count(c => c.porc < 90);
            Ent_BataClub_Chart_DataSet dsP = (new Ent_BataClub_Chart_DataSet()
            {                
                backgroundColor = new string[] { "rgba(228, 82, 82,0.9)", "rgba(243, 156, 18, 0.9)", "rgba(0, 166, 90,0.9)" },
                data = new decimal[] { menos90, entre90100, mas100 }
            });
            chartDS.datasets.Add(dsP);
            chartDS.labels = new string[] { "-90%", "90% - 100%" , "+100%" };            
            return Json(new { chartDS = JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), detalle = data });
        }
        public FileContentResult GetExcelCVB()
        {
            if (Session[_BC_Dashboard_data_CVB] == null)
            {
                List<Ent_BC_Dashboard_CVB> liststoreConf = new List<Ent_BC_Dashboard_CVB>();
                Session[_BC_Dashboard_data_CVB] = liststoreConf;
            }
            List<Ent_BC_Dashboard_CVB> lista = (List<Ent_BC_Dashboard_CVB>)Session[_BC_Dashboard_data_CVB];
            string[] columns = { "n_semana", "cod_entid", "des_entid", "anterior", "actual", "porc"};
            byte[] filecontent = ExcelExportHelper.ExportExcel(lista, "", false, columns);
            string nom_excel = "Lista de tiendas Cumplimiento Venta Bata ";
            return File(filecontent, ExcelExportHelper.ExcelContentType, nom_excel + ".xlsx");
        }//
        public ActionResult GetChartCxM(string anio = "2020")
        {
            Ent_BataClub_Chart_Data chartDS = new Ent_BataClub_Chart_Data();
            Ent_BataClub_Dashboard_CxM chartPSM = datDash.BATACLUB_DASHBOARD_CLIENTES_MES(anio);
            chartDS.datasets = new List<Ent_BataClub_Chart_DataSet>() {
                (new Ent_BataClub_Chart_DataSet()
                {
                    label = "REGISTROS",
                    backgroundColor = Enumerable.Repeat("rgba(60, 141, 188,0.8)", chartPSM.meses.Count).ToArray(), // new string[] { "rgba(180, 180, 180,0.7)" },
                    borderWidth = "1",
                    data = chartPSM.meses.Select(s => s.NUMERO).ToArray()
                }),
                (new Ent_BataClub_Chart_DataSet()
                {
                    label = "MIEMBROS",
                    backgroundColor = Enumerable.Repeat("rgba(221, 75, 57,0.8)", chartPSM.meses.Count).ToArray(),
                    borderWidth = "1",
                    data = chartPSM.meses.Select(s => s.NUMERO2).ToArray()
                }) };
            chartDS.labels = chartPSM.meses.Select(s => s.MES_STR).ToArray();
            return Json(new { chartDS = JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }) ,
                genero = JsonConvert.SerializeObject(chartPSM.genero, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            });
        }
        public ActionResult GetChartTP(string fechaini = null, string fechafin = null)
        {
            Ent_BataClub_Chart_Data chartDS = new Ent_BataClub_Chart_Data();
            List<Ent_BC_Dashboard_Ticket_Promedio> chartPSM = datDash.get_info_ticket_promedio(Convert.ToDateTime(fechaini), Convert.ToDateTime(fechafin));
            chartDS.datasets = new List<Ent_BataClub_Chart_DataSet>() {
                (new Ent_BataClub_Chart_DataSet()
                {
                    label = "",
                    backgroundColor =new string[] { "rgb(255, 99, 132,08)" ,"rgb(54, 162, 235,0.8)" },
                    borderWidth = "1",
                    data = chartPSM.Select(s=>Math.Round(s.TICKETPROM,2)).ToArray()
                }) };
            //(new Ent_BataClub_Chart_DataSet()
            //{
            //    label = "BATA",
            //    backgroundColor = new string[] { "rgb(54, 162, 235,0.8)" },
            //    borderWidth = "1",
            //    data = chartPSM.Where(W=>W.GRUPO == "BATA" ).Select(s => Math.Round(s.TICKETPROM,2)).ToArray()
            //}) };
            chartDS.labels = chartPSM.Select(s => s.GRUPO).ToArray();
            return Json(new
            {
                chartDS = JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),                
            });
        }

        public ActionResult GetChartCxF(string fechaini = null, string fechafin = null , bool canal = false )
        {
            Ent_BataClub_Chart_Data chartDS = new Ent_BataClub_Chart_Data();
            List<Ent_BataClub_Dashboard_Canales> data = datDash.BATACLUB_DASHBOARD_CANALES_FECHA(fechaini, fechafin , canal);

            Ent_BataClub_Chart_DataSet ds = (new Ent_BataClub_Chart_DataSet()
            {
                backgroundColor = new string[] { "rgba(99, 143, 197, 0.9)",
                        "rgba(221, 75, 57,0.9)",
                        "rgba(255, 206, 86, 0.8)",
                        "rgba(75, 192, 192, 0.8)"},
                data = data.Select(s => s.REGISTROS).ToArray()
            });

            chartDS.labels = data.Select(s => s.CANAL).ToArray();
            chartDS.datasets = new List<Ent_BataClub_Chart_DataSet>() { ds };
            return Json(new { chartDS = JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }) });
        }

        public ActionResult updateChartData(string anio, int informe, int mes = 0, string fecini = null, string fecfin = null, string prom = "", string sup = "", string fecini_canal = null, string fecfin_canaL = null, string fecini_com = null, string fecfin_com = null, string fecini_com_cl = null, string fecfin_com_cl = null, string opcion_data_in = "FN",string fechaIni_PS=null,string fechaFin_PS=null,string canal_venta=null)
        {
            Ent_BataClub_DashBoard dashboard = (Ent_BataClub_DashBoard)Session["_dashboardData"];

            if (dashboard == null)
            {
                /*retornar al login*/
                Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
                string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
                string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
                string return_view = actionName + "|" + controllerName;
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }


            if (fecini != null)
            {
                if (fecini.Length == 0) fecini = null;
                if (fecfin.Length == 0) fecfin = null;
            }

            if (fecini_canal != null)
            {
                if (fecini_canal.Length == 0) fecini_canal = null;
                if (fecfin_canaL.Length == 0) fecfin_canaL = null;
            }

            if (fecini_com != null)
            {
                if (fecini_com.Length == 0) fecini_com = null;
                if (fecfin_com.Length == 0) fecfin_com = null;
            }

            if (fecini_com_cl != null)
            {
                if (fecini_com_cl.Length == 0) fecini_com_cl = null;
                if (fecfin_com_cl.Length == 0) fecfin_com_cl = null;
            }

            if (fechaIni_PS != null)
            {
                if (fechaIni_PS.Length == 0) fechaIni_PS = null;
                if (fechaFin_PS.Length == 0) fechaFin_PS = null;
            }

            if (informe == 5 || informe == 7 || informe == 8 || informe == 9 || informe == 11)
            {
                dashboard = (Ent_BataClub_DashBoard)Session["_dashboardData"];
            }
            else
            {
                dashboard = (Ent_BataClub_DashBoard)Session["_dashboardData"];

                if (informe == 10) informe = 7;

                dashboard = datDash.GET_INFO_DASHBOARD(ref dashboard, anio, informe, mes, fecini, fecfin, prom, fecini_canal, fecfin_canaL, fecini_com, fecfin_com, fecini_com_cl, fecfin_com_cl, opcion_data_in, fechaIni_PS, fechaFin_PS, canal_venta);
                Session["_dashboardData"] = dashboard;
            }
            Ent_BataClub_Chart_Data chartDS = null;
            JsonResult jsonResult = new JsonResult();
            if (informe == 2)
            {
                chartDS = new Ent_BataClub_Chart_Data();
                chartDS = informeBarChartData(dashboard, 2);
                jsonResult = Json(new { result = JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), totalesGenero = dashboard.listMesGenero });
            }
            else if (informe == 3)
            {
                chartDS = new Ent_BataClub_Chart_Data();
                chartDS = informeCanales(dashboard);
                jsonResult = Json(JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            }
            else if (informe == 4)
            {
                chartDS = new Ent_BataClub_Chart_Data();
                chartDS = informeBarChartData(dashboard, 4);
                jsonResult = Json(new { result = JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), promsPS = dashboard.listPromsPS });
                Session[_session_par_sol_mes_excel] = dashboard.listPromsPS;
            }
            else if (informe == 5)
            {

                /*en este caso vamos a filtrar con linkend agrupar */
                Ent_BataClub_DashBoard lista_prom = new Ent_BataClub_DashBoard();
                lista_prom.listPromsPS = (from dr in dashboard.dtventa_bataclub.AsEnumerable().
                                          Where(myRow => myRow.Field<int>("MES") == mes && myRow.Field<int>("ANIO") == Convert.ToInt32(anio))
                                              //where dr.Field<string>("ANIO") == anio
                                              //&& (int)dr["MES"] == mes
                                          group dr by
                                          new
                                          {
                                              promocion = dr["PROMOCION"].ToString()
                                          }
                                        into G
                                          select new Ent_BataClub_DashBoard_Proms()
                                          {
                                              promocion = G.Key.promocion,
                                              pares = G.Sum(r => Convert.ToInt32(r["PARES"])),
                                              soles = G.Sum(r => Convert.ToInt32(r["SOLES"])),
                                          }
                                        ).OrderByDescending(c => c.soles).ToList();

                dashboard.listPromsPS = lista_prom.listPromsPS;

                jsonResult = Json(new { promsPS = dashboard.listPromsPS });
                Session[_session_par_sol_mes_excel] = dashboard.listPromsPS;
            }
            //else if (informe == 6)
            //{
            //    chartDS = new Ent_BataClub_Chart_Data();
            //    chartDS = informeBarChartData(dashboard, 6);
            //    //Session[_session_det_tdas_sup] = dashboard.listTiendasSupervTot;
            //    //Session[_session_det_tdas_sup_excel] = dashboard.listTiendasSupervTot;
            //    jsonResult = Json(new { result = JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), tiendas = dashboard.listTiendasSupervTot });
            //}
            else if (informe == 7)
            {
                chartDS = new Ent_BataClub_Chart_Data();
                chartDS = informeBarChartData(dashboard, 7);
                Session[_session_DetallesTipoCompra] = dashboard.listTipoComprasTot;
                //Session[_session_det_tdas_sup_excel] = dashboard.listTiendasSupervTot;
                jsonResult = Json(new { result = JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), tipo = dashboard.listTipoComprasTot });


            }
            //else if (informe == 8)
            //{
            //    jsonResult = Json(new
            //    {

            //        tiendas = JsonConvert.SerializeObject(((List<Ent_BataClub_DashBoard_TiendasSupervisor>)Session[_session_det_tdas_sup]).Where(w => w.supervisor == sup), Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            //    });
            //    Session[_session_det_tdas_sup_excel] = ((List<Ent_BataClub_DashBoard_TiendasSupervisor>)Session[_session_det_tdas_sup]).Where(w => w.supervisor == sup).ToList();
            //}
            else if (informe == 9)
            {

                jsonResult = Json(new
                {

                    tipo = JsonConvert.SerializeObject(((List<Ent_BataClub_DashBoard_Tipo_Compras>)Session[_session_DetallesTipoCompra]).Where(w => w.tipo == sup), Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                });
            }
            else if (informe == 11)
            {
                Ent_BataClub_DashBoard lista_prom = new Ent_BataClub_DashBoard();
                if (mes == 0)
                {
                    lista_prom.listDetPromTda = (from dr in dashboard.dtventa_bataclub.AsEnumerable().
                                            Where(myRow => myRow.Field<string>("PROMOCION") == prom && myRow.Field<int>("ANIO") == Convert.ToInt32(anio))
                                                     //where dr.Field<string>("ANIO") == anio
                                                     //&& (int)dr["MES"] == mes
                                                 group dr by
                                                 new
                                                 {
                                                     promocion = dr["PROMOCION"].ToString(),
                                                     tienda = dr["TIENDA"].ToString()
                                                 }
                                        into G
                                                 select new Ent_BataClub_DashBoard_Proms()
                                                 {
                                                     promocion = G.Key.promocion,
                                                     tienda = G.Key.tienda,
                                                     pares = G.Sum(r => Convert.ToInt32(r["PARES"])),
                                                     soles = G.Sum(r => Convert.ToInt32(r["SOLES"])),
                                                 }
                                        ).OrderByDescending(c => c.soles).ToList();
                }
                else
                {
                    lista_prom.listDetPromTda = (from dr in dashboard.dtventa_bataclub.AsEnumerable().
                                           Where(myRow => myRow.Field<string>("PROMOCION") == prom && myRow.Field<int>("ANIO") == Convert.ToInt32(anio)
                                                      && myRow.Field<int>("MES") == mes)
                                                     //where dr.Field<string>("ANIO") == anio
                                                     //&& (int)dr["MES"] == mes
                                                 group dr by
                                                 new
                                                 {
                                                     promocion = dr["PROMOCION"].ToString(),
                                                     tienda = dr["TIENDA"].ToString()
                                                 }
                                       into G
                                                 select new Ent_BataClub_DashBoard_Proms()
                                                 {
                                                     promocion = G.Key.promocion,
                                                     tienda = G.Key.tienda,
                                                     pares = G.Sum(r => Convert.ToInt32(r["PARES"])),
                                                     soles = G.Sum(r => Convert.ToInt32(r["SOLES"])),
                                                 }
                                       ).OrderByDescending(c => c.soles).ToList();
                }

                dashboard.listDetPromTda = lista_prom.listDetPromTda;

                jsonResult = Json(new { promsDetPromTda = dashboard.listDetPromTda });

            }
            else if (informe == 12)
            {

                chartDS = new Ent_BataClub_Chart_Data();
                chartDS = informeCompraCl(dashboard);
                jsonResult = Json(JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            }
            else if (informe == 13)
            {
                chartDS = new Ent_BataClub_Chart_Data();
                chartDS = informeDataIncompleto(dashboard);
                jsonResult = Json(JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            }
            return jsonResult;
        }
        //public Ent_BataClub_Chart_Data informeSupervisor(Ent_BataClub_DashBoard dashboard)
        //{
        //    Ent_BataClub_Chart_Data chartDSDonut = new Ent_BataClub_Chart_Data();
        //    Ent_BataClub_Chart_DataSet dsBCDonut = (new Ent_BataClub_Chart_DataSet()
        //    {
        //        backgroundColor = new string[] {
        //                "rgba(99, 143, 197, 0.9)",
        //                "rgba(221, 75, 57,0.9)",
        //                "rgba(255, 206, 86, 0.8)",
        //                "rgba(44, 122, 192, 0.8)",
        //                "rgba(122, 65, 32,0.9)",
        //                "rgba(245, 123, 56, 0.8)",
        //                "rgba(14, 123, 245, 0.8)",
        //                "rgba(33, 154, 34,0.9)",
        //                "rgba(142, 121, 86, 0.8)",
        //                "rgba(54, 45, 65, 0.8)" },
        //        data = dashboard.listSupervisorTot.Select(s => s.registros).ToArray()
        //    });
        //    chartDSDonut.labels = dashboard.listCanles.Select(s => s.CANAL).ToArray();
        //    chartDSDonut.datasets = new List<Ent_BataClub_Chart_DataSet>() { dsBCDonut };
        //    return chartDSDonut;
        //}
        public Ent_BataClub_Chart_Data informeCanales(Ent_BataClub_DashBoard dashboard)
        {
            Ent_BataClub_Chart_Data chartDSDonut = new Ent_BataClub_Chart_Data();
            Ent_BataClub_Chart_DataSet dsBCDonut = (new Ent_BataClub_Chart_DataSet()
            {
                backgroundColor = new string[] { "rgba(99, 143, 197, 0.9)",
                        "rgba(221, 75, 57,0.9)",
                        "rgba(255, 206, 86, 0.8)",
                        "rgba(75, 192, 192, 0.8)"},
                data = dashboard.listCanles.Select(s => s.REGISTROS).ToArray()
            });
            chartDSDonut.labels = dashboard.listCanles.Select(s => s.CANAL).ToArray();
            chartDSDonut.datasets = new List<Ent_BataClub_Chart_DataSet>() { dsBCDonut };
            return chartDSDonut;
        }
        public Ent_BataClub_Chart_Data informeDataIncompleto(Ent_BataClub_DashBoard dashboard)
        {
            Ent_BataClub_Chart_Data chartDSDonut = new Ent_BataClub_Chart_Data();
            Ent_BataClub_Chart_DataSet dsBCDonut = (new Ent_BataClub_Chart_DataSet()
            {
                backgroundColor = new string[] { "rgba(99, 143, 197, 0.9)",
                        "rgba(221, 75, 57,0.9)",
                        "rgba(255, 206, 86, 0.8)",
                        "rgba(75, 192, 192, 0.8)"},
                data = dashboard.listincompletos.Select(s => s.porc).ToArray()
            });
            chartDSDonut.labels = dashboard.listincompletos.Select(s => s.campo).ToArray();
            chartDSDonut.datasets = new List<Ent_BataClub_Chart_DataSet>() { dsBCDonut };
            return chartDSDonut;
        }

        public Ent_BataClub_Chart_Data informeCompraCl(Ent_BataClub_DashBoard dashboard)
        {
            Ent_BataClub_Chart_Data chartDSDonut = new Ent_BataClub_Chart_Data();
            Ent_BataClub_Chart_DataSet dsBCDonut = (new Ent_BataClub_Chart_DataSet()
            {
                backgroundColor = new string[] { "rgba(99, 143, 197, 0.9)",
                        "rgba(221, 75, 57,0.9)",
                        "rgba(255, 206, 86, 0.8)",
                        "rgba(75, 192, 192, 0.8)"},
                data = dashboard.listComprasCliTot.Select(s => s.nclientes).ToArray()
            });
            chartDSDonut.labels = dashboard.listComprasCliTot.Select(s => s.com_des).ToArray();
            chartDSDonut.datasets = new List<Ent_BataClub_Chart_DataSet>() { dsBCDonut };
            return chartDSDonut;
        }
        public Ent_BataClub_Chart_Data informeCompras(Ent_BataClub_DashBoard dashboard)
        {
            Ent_BataClub_Chart_Data chartDSDonut = new Ent_BataClub_Chart_Data();
            Ent_BataClub_Chart_DataSet dsBCDonut = (new Ent_BataClub_Chart_DataSet()
            {
                backgroundColor = new string[] { "rgba(66, 186, 192, 0.9)",
                        "rgba(211, 126, 114,0.9)",
                        "rgba(255, 206, 86, 0.8)",
                        "rgba(75, 192, 192, 0.8)"},
                data = dashboard.listComprasTot.Select(s => s.transac).ToArray()
            });
            chartDSDonut.labels = dashboard.listComprasTot.Select(s => s.tipo).ToArray();
            chartDSDonut.datasets = new List<Ent_BataClub_Chart_DataSet>() { dsBCDonut };
            return chartDSDonut;
        }
        public Ent_BataClub_Chart_Data informeBarChartData(Ent_BataClub_DashBoard dashboard, int informe)
        {
            Ent_BataClub_Chart_Data chartDS = new Ent_BataClub_Chart_Data();
            if (informe == 2)
            {
                chartDS.datasets = new List<Ent_BataClub_Chart_DataSet>() {
                (new Ent_BataClub_Chart_DataSet()
                {
                    label = "REGISTROS",
                    backgroundColor = Enumerable.Repeat("rgba(60, 141, 188,0.8)", dashboard.listMesRegistros.Count).ToArray(), // new string[] { "rgba(180, 180, 180,0.7)" },
                    borderWidth = "1",
                    data = dashboard.listMesRegistros.Select(s => s.NUMERO).ToArray()
                }),
                (new Ent_BataClub_Chart_DataSet()
                {
                    label = "MIEMBROS",
                    backgroundColor = Enumerable.Repeat("rgba(221, 75, 57,0.8)", dashboard.listMesRegistros.Count).ToArray(),
                    borderWidth = "1",
                    data = dashboard.listMesMiembros.Select(s => s.NUMERO).ToArray()
                }) };
                chartDS.labels = dashboard.listMesMiembros.Select(s => s.MES_STR).ToArray();
            }
            else if (informe == 4)
            {
                chartDS.datasets = new List<Ent_BataClub_Chart_DataSet>() {
                (new Ent_BataClub_Chart_DataSet()
                {
                    label = "PARES",
                    backgroundColor = Enumerable.Repeat("rgba(136, 61, 45,0.8)", dashboard.listMesParesSoles.Count).ToArray(), // new string[] { "rgba(180, 180, 180,0.7)" },
                    borderWidth = "1",
                    data = dashboard.listMesParesSoles.Select(s => s.NUMERO).ToArray()
                }),
                (new Ent_BataClub_Chart_DataSet()
                {
                    label = "SOLES",
                    backgroundColor = Enumerable.Repeat("rgba(205, 236, 33 ,0.8)", dashboard.listMesParesSoles.Count).ToArray(),
                    borderWidth = "1",
                    data = dashboard.listMesParesSoles.Select(s => s.NUMERO2).ToArray()
                }) };
                chartDS.labels = dashboard.listMesParesSoles.Select(s => s.MES_STR).ToArray();
            }
            //else if (informe == 6)
            //{
            //    chartDS.datasets = new List<Ent_BataClub_Chart_DataSet>() {
            //        (new Ent_BataClub_Chart_DataSet()
            //        {
            //            label = "TRANSACCIONES",
            //            backgroundColor = Enumerable.Repeat("rgba(0, 166, 90,0.8)", dashboard.listSupervisorTot.Count).ToArray(),
            //            borderWidth = "1",
            //            data = dashboard.listSupervisorTot.Select(s => s.transac).ToArray()
            //        }),
            //        (new Ent_BataClub_Chart_DataSet()
            //        {
            //            label = "REGISTROS",
            //            backgroundColor = Enumerable.Repeat("rgba(180, 180, 180,0.8)", dashboard.listSupervisorTot.Count).ToArray(), // new string[] { "rgba(180, 180, 180,0.7)" },
            //            borderWidth = "1",
            //            data = dashboard.listSupervisorTot.Select(s => s.registros).ToArray()
            //        }),

            //        (new Ent_BataClub_Chart_DataSet()
            //        {
            //            label = "CONSUMIDOS",
            //            backgroundColor = Enumerable.Repeat("rgba(243, 156, 18, 0.8)", dashboard.listSupervisorTot.Count).ToArray(),
            //            borderWidth = "1",
            //            data = dashboard.listSupervisorTot.Select(s => s.consumido).ToArray()
            //        }),

            //         (new Ent_BataClub_Chart_DataSet()
            //        {
            //            label = "MIEMBROS",
            //            backgroundColor = Enumerable.Repeat("rgba(230, 101, 101, 0.8)", dashboard.listSupervisorTot.Count).ToArray(),
            //            borderWidth = "1",
            //            data = dashboard.listSupervisorTot.Select(s => s.bataclub).ToArray()
            //        })
            //    };

            //    chartDS.labels = dashboard.listSupervisorTot.Select(s => s.supervisor).ToArray();
            //    chartDS.labelsTooltip = new string[] { "Hola", "Hola", "Hola", "Hola", "Hola", "Hola", "Hola", "HOLA" };
            //}
            else if (informe == 7)
            {
                chartDS.datasets = new List<Ent_BataClub_Chart_DataSet>() {
                    (new Ent_BataClub_Chart_DataSet()
                    {
                        label = "TRANSACCIONES",
                        backgroundColor = Enumerable.Repeat("rgba(60, 141, 188,0.8)", dashboard.listComprasTot.Count).ToArray(),
                        borderWidth = "1",
                        data = dashboard.listComprasTot.Select(s => s.transac).ToArray()
                    }),
                    (new Ent_BataClub_Chart_DataSet()
                    {
                        label = "MONTO",
                        backgroundColor = Enumerable.Repeat("rgba(221, 75, 57,0.8)", dashboard.listComprasTot.Count).ToArray(), // new string[] { "rgba(180, 180, 180,0.7)" },
                        borderWidth = "1",
                        data = dashboard.listComprasTot.Select(s => s.monto).ToArray()
                    }),

                    //(new Ent_BataClub_Chart_DataSet()
                    //{
                    //    label = "CONSUMIDOS",
                    //    backgroundColor = Enumerable.Repeat("rgba(243, 156, 18, 0.8)", dashboard.listSupervisorTot.Count).ToArray(),
                    //    borderWidth = "1",
                    //    data = dashboard.listSupervisorTot.Select(s => s.consumido).ToArray()
                    //}),

                    // (new Ent_BataClub_Chart_DataSet()
                    //{
                    //    label = "MIEMBROS",
                    //    backgroundColor = Enumerable.Repeat("rgba(230, 101, 101, 0.8)", dashboard.listSupervisorTot.Count).ToArray(),
                    //    borderWidth = "1",
                    //    data = dashboard.listSupervisorTot.Select(s => s.bataclub).ToArray()
                    //})
                };

                chartDS.labels = dashboard.listComprasTot.Select(s => s.tipo).ToArray();
                chartDS.labelsTooltip = new string[] { "Hola", "Hola", "Hola", "Hola", "Hola", "Hola", "Hola", "HOLA" };
            }
            return chartDS;
        }
        private string _session_det_canal_excel = "_session_det_canal_excel";
        private string _session_com_cl_excel = "_session_com_cl_excel";
        private string _session_data_incompleto_excel = "_session_data_incompleto_excel";

        public JsonResult DataIncompleto_Excel(string opcion_data_in)
        {
            Dat_BataClub_Dashboard daIncompleto_excel = new Dat_BataClub_Dashboard();
            List<Ent_BataClub_Datos_Incompletos_Excel> lista = daIncompleto_excel.get_datos_incompletos_excel(13,opcion_data_in);
            Boolean valida_lista = false;
            if (lista != null)
            {
                if (lista.Count > 0)
                {
                    valida_lista = true;
                    Session[_session_data_incompleto_excel] = lista;
                }
            }
            return Json(new { estado = (valida_lista) ? "1" : "-1", desmsg = (valida_lista) ? "Exportando el Excel." : "Hubo un Error ó No hay Datos para exportar." });

        }
        public FileContentResult ExportarDataIncompleto_Excel()
        {
            List<Ent_BataClub_Datos_Incompletos_Excel> lista = (List<Ent_BataClub_Datos_Incompletos_Excel>)Session[_session_data_incompleto_excel];
            string[] columns = { "dni", "nombres", "correo" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(lista, "Datos Incompletos BataClub", true, columns);
            string nom_excel = "Datos Incompletos BataClub";
            return File(filecontent, ExcelExportHelper.ExcelContentType, nom_excel + ".xlsx");
        }

        public JsonResult CanalDetExcel_Data(string fecini_canal = null, string fecfin_canal = null)
        {
            Dat_BataClub_Dashboard canal_excel = new Dat_BataClub_Dashboard();
            List<Ent_Bataclub_Canales_Excel> lista = canal_excel.get_canales_excel(3, Convert.ToDateTime(fecini_canal), Convert.ToDateTime(fecfin_canal));
            Boolean valida_lista = false;
            if (lista!=null)
            {
                if (lista.Count>0)
                {
                    valida_lista = true;
                    Session[_session_det_canal_excel] = lista;
                }
            }           
            return Json(new { estado = (valida_lista) ? "1" : "-1", desmsg = (valida_lista) ? "Exportando el Excel." : "Hubo un Error ó No hay Datos para exportar." });
            
        }
        public JsonResult Compras_CL_Excel_Data(string fecini_com_cl = null, string fecfin_com_cl = null)
        {
            Dat_BataClub_Dashboard canal_excel = new Dat_BataClub_Dashboard();
            List<Ent_BataClub_Compras_CL_Excel> lista = canal_excel.get_compras_excel(12, Convert.ToDateTime(fecini_com_cl), Convert.ToDateTime(fecfin_com_cl));
            Boolean valida_lista = false;
            if (lista != null)
            {
                if (lista.Count > 0)
                {
                    valida_lista = true;
                    Session[_session_com_cl_excel] = lista;
                }
            }
            return Json(new { estado = (valida_lista) ? "1" : "-1", desmsg = (valida_lista) ? "Exportando el Excel." : "Hubo un Error ó No hay Datos para exportar." });

        }
        public FileContentResult ExportarCanalDetExcel()
        {            
                List<Ent_Bataclub_Canales_Excel> lista = (List<Ent_Bataclub_Canales_Excel>)Session[_session_det_canal_excel]; 
                string[] columns = { "Canal", "Tienda", "Dni", "Nombres", "Correo", "Miem_Bataclub", "Fec_Registro", "Fec_Miembro" };
                byte[] filecontent = ExcelExportHelper.ExportExcel(lista, "Lista de Canales Detallado", true, columns);
                string nom_excel = "Lista de Canales x Rango de Fecha Detallado";
                return File(filecontent, ExcelExportHelper.ExcelContentType, nom_excel + ".xlsx");                                      
        }
        public FileContentResult ExportaComClExcel()
        {
            List<Ent_BataClub_Compras_CL_Excel> lista = (List<Ent_BataClub_Compras_CL_Excel>)Session[_session_com_cl_excel];
            string[] columns = { "dni", "correo", "compras" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(lista, "Lista de Compras Clientes", true, columns);
            string nom_excel = "Lista_de_Compra_Cliente";
            return File(filecontent, ExcelExportHelper.ExcelContentType, nom_excel + ".xlsx");
        }
        public FileContentResult RegTraConTdaExcel()
        {
            if (Session[_BC_Dashboard_Distritos] == null)
            {
                List<Ent_BataClub_DashBoard> liststoreConf = new List<Ent_BataClub_DashBoard>();
                Session[_BC_Dashboard_Distritos] = liststoreConf;
            }
            List<Ent_BataClub_DashBoard_Tiendas_Distritos> lista = ((Ent_BataClub_DashBoard)Session[_BC_Dashboard_Distritos]).listDistritosTiendas.OrderBy(o => o.supervisor).ThenBy(t => t.distrito).ToList();
            string[] columns = { "supervisor","distrito", "tienda", "registros", "transac", "consumido","bataclub" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(lista, "", false, columns);
            string nom_excel = "Lista de tiendas distrito RTCxST";
            return File(filecontent, ExcelExportHelper.ExcelContentType, nom_excel + ".xlsx");
        }//

        public FileContentResult ParSolMesExcel()
        {
            if (Session[_session_par_sol_mes_excel] == null)
            {
                List<Ent_BataClub_DashBoard_Proms> liststoreConf = new List<Ent_BataClub_DashBoard_Proms>();
                Session[_session_par_sol_mes_excel] = liststoreConf;
            }
            /*
                               public string promocion { get; set; }
                                public string tienda { get; set; }
                                public decimal pares { get; set; }
                                public decimal soles { get; set; }
             */
            List<Ent_BataClub_DashBoard_Proms> lista = (List<Ent_BataClub_DashBoard_Proms>)Session[_session_par_sol_mes_excel];
            string[] columns = { "promocion", "pares", "soles" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(lista, "", false, columns);
            string nom_excel = "Lista de promociones PSxM";
            return File(filecontent, ExcelExportHelper.ExcelContentType, nom_excel + ".xlsx");
        }

        public ActionResult get_tda_cadena(string cadenas)
        {
            List<Ent_Combo> list = datOE.get_tda_cadena(cadenas, 1);
            if (list == null)
            {
                list = new List<Ent_Combo>();
            }
            return Json(list);
        }

        [HttpPost]
        public ActionResult getListaCupProm(string codProm, string operacion)
        {
            if (String.IsNullOrEmpty(operacion))
            {
                List<Ent_BataClub_Cupones> listCups = datProm.get_ListaCuponesPromocion(codProm);
                if (listCups == null)
                {
                    listCups = new List<Ent_BataClub_Cupones>();
                    Session[_session_lista_cupones_excel] = listCups;
                    Session[_session_lista_clientes_cupon] = listCups;
                }
                else
                {
                    Session[_session_lista_cupones_excel] = listCups;
                    Session[_session_lista_clientes_cupon] = listCups;
                }
            }
            return View();
        }

        public ActionResult getDetalleCupon(string cupon)
        {
            string detalles = "";
            detalles = datProm.get_detalles_cupon(cupon);
            if (detalles != "")
            {
                return Json(new { estado = 1, detalles = detalles });
            }
            else
            {
                return Json(new { estado = 0, resultados = "No hay resultados." });
            }
        }

        //Table
        public PartialViewResult _Table(string dni, string cupon, string hidden, string correo, string[] dwprom, string[] dwest)
        {
            if (dwprom == null && dwest == null && String.IsNullOrEmpty(dni) && String.IsNullOrEmpty(cupon) && String.IsNullOrEmpty(correo))
            {
                Session[_session_tabla_cupones] = null;
                return PartialView();
            }
            else
            { //string dwprom--> se reemplaza por hidden - para agarrar varios id de promociones con el combo multiselect

                dwprom = dwprom == null ? new string[] { "" } : dwprom;
                dwest = dwest == null ? new string[] { "" } : dwest;
                return PartialView(listaTablaPromociones(dni, cupon, String.Join(",", dwprom), correo, String.Join(",", dwest)));
            }
        }

        public List<Ent_BataClub_Cupones> listaTablaPromociones(string dni, string cupon, string id_grupo, string correo, string dwest)
        {
            List<Ent_BataClub_Cupones> listguia = datProm.get_lista_cupones(dni, cupon, id_grupo, correo, dwest);
            Session[_session_tabla_cupones] = listguia;
            return listguia;
        }

        public ActionResult getTableCuponesAjax(Ent_jQueryDataTableParams param, string lblConsumidos)
        {
            /*verificar si esta null*/
            if (Session[_session_tabla_cupones] == null)
            {
                List<Ent_BataClub_Cupones> listdoc = new List<Ent_BataClub_Cupones>();
                Session[_session_tabla_cupones] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_BataClub_Cupones> membercol = ((List<Ent_BataClub_Cupones>)(Session[_session_tabla_cupones])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_BataClub_Cupones> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m =>
                    m.correo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.promocion.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.fechaFin.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.nombresCliente.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.cupon.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.dniCliente.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.correo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.cupon.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.porcDesc.ToString().Contains(param.sSearch.ToUpper()));
            }

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            var sortDirection = Request["sSortDir_0"];
            if (param.iSortingCols > 0)
            {
                if (sortDirection == "asc")
                {
                    if (sortIdx == 0)
                    {
                        filteredMembers = filteredMembers.OrderBy(o => o.promocion);
                    }
                    else if (sortIdx == 1)
                    {
                        filteredMembers = filteredMembers.OrderBy(o => o.estado);
                    }

                }
                else
                {
                    if (sortIdx == 0)
                    {
                        filteredMembers = filteredMembers.OrderByDescending(o => o.promocion);
                    }
                    else if (sortIdx == 1)
                    {
                        filteredMembers = filteredMembers.OrderByDescending(o => o.estado);
                    }
                }
            }
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.promocion,
                             a.estado,
                             a.fechaFin,
                             a.nombresCliente,
                             a.dniCliente,
                             a.correo,
                             a.cupon,
                             a.porcDesc
                         };
            var numvariable1 = filteredMembers.Count(n => n.estado.ToUpper() == "CONSUMIDO");
            var numvariable2 = filteredMembers.Count(n => n.estado.ToUpper() == "DISPONIBLE");
            var numvariable3 = filteredMembers.Count(n => n.estado.ToUpper() == "CADUCADO");
            // param.variable1 = lblConsumidos;
            param.variable1 = numvariable1.ToString();
            param.variable2 = numvariable2.ToString();
            param.variable3 = numvariable3.ToString();
            //Se devuelven los resultados por json
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = result,
                variable1 = param.variable1,
                variable2 = param.variable2,
                variable3 = param.variable3
            }, JsonRequestBehavior.AllowGet);
        }
        public string addClienteLista(int operacion, string dniCorreo, string mesCumple, string genero)
        {
            List<Ent_BataClub_Cupones> clientes = new List<Ent_BataClub_Cupones>();
            if (operacion == 2)
            {
                if (!String.IsNullOrEmpty(dniCorreo))
                    clientes = datProm.get_cliente(dniCorreo);
            }
            else
            {
                clientes = datProm.get_cliente(mesCumple, genero);
                ViewBag.Operacion = 1;
            }
            List<Ent_BataClub_Cupones> listaClientes = null;
            string result = "";
            if (Session[_session_lista_clientes_cupon] == null)
            {
                List<Ent_BataClub_Cupones> _cup = new List<Ent_BataClub_Cupones>();
                Session[_session_lista_clientes_cupon] = _cup;
            }
            if (clientes != null)
            {
                listaClientes = (List<Ent_BataClub_Cupones>)Session[_session_lista_clientes_cupon];
                if (listaClientes.Count == 0)
                {
                    listaClientes = clientes;
                }
                else
                {
                    listaClientes = listaClientes.Union(clientes, new Ent_BataClub_CuponesComparer()).ToList();
                }
                Session[_session_lista_clientes_cupon] = listaClientes;
            }
            else
            {
                result = ("Sin resultados " + (!String.IsNullOrEmpty(dniCorreo) ? " para: " + dniCorreo : "")).Trim();
            }
            return result;
        }
        public ActionResult getListaPromocionesAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_lista_promociones] == null)
            {
                List<Ent_BataClub_Promociones> listdoc = new List<Ent_BataClub_Promociones>();
                Session[_session_lista_promociones] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_BataClub_Promociones> membercol = ((List<Ent_BataClub_Promociones>)(Session[_session_lista_promociones])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_BataClub_Promociones> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m =>
                    m.Codigo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.Descripcion.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.FechaFin.ToUpper().Contains(param.sSearch.ToUpper()));
            }

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            var sortDirection = Request["sSortDir_0"];
            if (param.iSortingCols > 0)
            {
                if (sortDirection == "asc")
                {
                    if (sortIdx == 0) filteredMembers = filteredMembers.OrderBy(o => o.Codigo);
                    else if (sortIdx == 1) filteredMembers = filteredMembers.OrderBy(o => o.Descripcion);
                    else if (sortIdx == 2) filteredMembers = filteredMembers.OrderBy(o => o.Porc_Dcto);
                    else if (sortIdx == 3) filteredMembers = filteredMembers.OrderBy(o => o.MaxPares);
                    else if (sortIdx == 4) filteredMembers = filteredMembers.OrderBy(o => o.FechaFin);
                    else if (sortIdx == 5) filteredMembers = filteredMembers.OrderBy(o => o.PromActiva);
                    else if (sortIdx == 6) filteredMembers = filteredMembers.OrderBy(o => o.nroCupones);
                }
                else
                {
                    if (sortIdx == 0) filteredMembers = filteredMembers.OrderByDescending(o => o.Codigo);
                    else if (sortIdx == 1) filteredMembers = filteredMembers.OrderByDescending(o => o.Descripcion);
                    else if (sortIdx == 2) filteredMembers = filteredMembers.OrderByDescending(o => o.Porc_Dcto);
                    else if (sortIdx == 3) filteredMembers = filteredMembers.OrderByDescending(o => o.MaxPares);
                    else if (sortIdx == 4) filteredMembers = filteredMembers.OrderByDescending(o => o.FechaFin);
                    else if (sortIdx == 6) filteredMembers = filteredMembers.OrderByDescending(o => o.nroCupones);
                }
            }
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.Codigo,
                             a.Descripcion,
                             a.Porc_Dcto,
                             a.MaxPares,
                             a.FechaFin,
                             a.PromActiva,
                             a.nroCupones,
                             a.Coupon_Code                             
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
        //Gráfica
        public string listarStr_graph()
        {
            string strJson = "";
            //  JsonResult jRespuesta = null;
            /*verificar si esta null*/
            if (Session[_BataClub_Promociones_grafica] == null)
            {
                strJson = datProm.listarStr_graph();
                Session[_BataClub_Promociones_grafica] = strJson;
            }
            else
            {
                strJson = Session[_BataClub_Promociones_grafica].ToString();
            }

            // var serializer = new JavaScriptSerializer();
            // jRespuesta = Json(serializer.Deserialize<List<Articulo_Stock_Tienda>>(strJson), JsonRequestBehavior.AllowGet);
            return strJson;
        }

        public PartialViewResult _popUpGrafica()
        {
            return PartialView(/*listarStr_graph()*/);
        }

        //Exportar Excel
        [HttpGet]
        public FileContentResult ExportToExcel()
        {
            List<Ent_BataClub_Cupones> listbataclub = (List<Ent_BataClub_Cupones>)Session[_session_tabla_cupones];
            string[] columns = { "promocion", "estado", "fechaFin", "nombresCliente", "dniCliente", "correo", "cupon", "porcDesc" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(listbataclub, "", true, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "BATACLUB_Promociones.xlsx");
        }
        #endregion

        #region Bataclub/Cupon
        //Index
        [HttpPost]
        public ActionResult GenerarCupon(string codigo, string descripcion, decimal dscto, DateTime fecha, int pares, string coupon_code)
        {
            Session[_session_lista_clientes_cupon] = null;
            if (String.IsNullOrEmpty(codigo))
            {
                return RedirectToAction("Promociones", "BataClub");
            }
            else
            {
                ViewBag.Codigo = codigo;
                ViewBag.Descripcion = descripcion;
                ViewBag.dscto = dscto;
                ViewBag.fechaF = fecha.ToString("dd-MM-yyyy");
                ViewBag.fechaI = DateTime.Now.ToString("dd-MM-yyyy");
                ViewBag.pares = pares;
                List<Ent_BataClub_Orce_Promotion> listOP = datProm.GET_ORCE_PROMOTION(0, coupon_code);
                if (listOP == null)
                {
                    listOP = new List<Ent_BataClub_Orce_Promotion>();
                }
                ViewBag.listOP = listOP;
                return View();
            }
        }
        public ActionResult CrearPromocion()
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
                Session[_session_lista_clientes_cupon] = null;

                ViewBag.listCadena = datOE.get_cadena();
                List<Ent_Combo> list = new List<Ent_Combo>();
                ViewBag.listTdaCadena = list;
                ViewBag.Operacion = 1;
                List<Ent_Combo> listMeses = datProm.get_ListaMeses();
                listMeses.Insert(0, new Ent_Combo() { cbo_codigo = "0", cbo_descripcion = "TODOS" });
                ViewBag.Meses = listMeses;
                ViewBag.fechaF = DateTime.Now.ToString("dd-MM-yyyy");
                ViewBag.fechaI = DateTime.Now.ToString("dd-MM-yyyy");
                List<Ent_Combo> anios = datCbo.get_lista_anios(2015);
                anios.Insert(0, new Ent_Combo() { cbo_codigo = "0", cbo_descripcion = "TODOS" });
                ViewBag.anios = anios;

                List<Ent_BataClub_Orce_Promotion> listOP = datProm.GET_ORCE_PROMOTION(1);
                if (listOP == null)
                {
                    listOP = new List<Ent_BataClub_Orce_Promotion>();
                }
                ViewBag.listOP = listOP;

                return View();
            }
        }
        //Table cupón 
        public ActionResult getTableCuponAjax(Ent_jQueryDataTableParams param, string dniEliminar)
        {
            /*verificar si esta null*/
            if (Session[_session_lista_clientes_cupon] == null)
            {
                List<Ent_BataClub_Cupones> listdoc = new List<Ent_BataClub_Cupones>();
                Session[_session_lista_clientes_cupon] = listdoc;
            }
            if (!String.IsNullOrEmpty(dniEliminar))
            {
                List<Ent_BataClub_Cupones> listAct = (List<Ent_BataClub_Cupones>)(Session[_session_lista_clientes_cupon]);
                listAct.Remove(listAct.Where(w => w.dniCliente == dniEliminar).FirstOrDefault());
                Session[_session_lista_clientes_cupon] = listAct;
            }
            //Traer registros
            IQueryable<Ent_BataClub_Cupones> membercol = ((List<Ent_BataClub_Cupones>)(Session[_session_lista_clientes_cupon])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_BataClub_Cupones> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m =>
                    m.correo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.nombresCliente.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.dniCliente.ToUpper().Contains(param.sSearch.ToUpper()) || m.apellidosCliente.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    (m.nombresCliente.Trim() + " " + m.apellidosCliente.Trim()).ToUpper().Contains(param.sSearch.ToUpper())
                    );
            }

            //Manejador de orden
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.promocion,
                             a.dniCliente,
                             a.nombresCliente,
                             a.correo,
                             a.apellidosCliente,
                             a.genero,
                             a.mesCumple,
                             a.miemBataClub,
                             a.cupon
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

        public ActionResult BATACLUB_INSERTAR_CUPONES(int operacion, string promocion, string dscto, string pares, string fechaF, string mesCumple, 
            string genero, string[] tienda, string[] tienda2, string anio,string fechaI , string prefix)
        {
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            List<Ent_BataClub_Cupones> listaClientes = null;
            string _error = "";
            string _mensaje = "";
            string _prom_id = "";
            List<Ent_BataClub_Cupones> resultList = null;
            if (Session[_session_lista_clientes_cupon] != null)
            {
                listaClientes = new List<Ent_BataClub_Cupones>();
                listaClientes = (List<Ent_BataClub_Cupones>)Session[_session_lista_clientes_cupon];
            }
            if ((listaClientes == null || (listaClientes != null && listaClientes.Count == 0)) && operacion == 2)
            {
                _error += "La lista de clientes está vacia" + Environment.NewLine;
            }

            if (String.IsNullOrEmpty(prefix.Trim()))
            {
                _error += "Seleccione promocion ORCE." + Environment.NewLine;
            }

            if (String.IsNullOrEmpty(fechaF.Trim()))
            {
                _error += "Ingrese fecha fin por favor." + Environment.NewLine;
            }
            if (String.IsNullOrEmpty(fechaI.Trim()))
            {
                _error += "Ingrese fecha de inicio por favor." + Environment.NewLine;
            }
            if (String.IsNullOrEmpty(promocion.Trim()))
            {
                _error += "Ingrese el nombre de la promoción por favor." + Environment.NewLine;
            }
            if (!IsNumeric(dscto.ToString().Trim()))
            {
                _error += "Ingrese un valor válido en el campo descuento por favor." + Environment.NewLine;
            }
            if (!IsNumeric(pares.ToString().Trim()))
            {
                _error += "Ingrese un valor válido en el campo pares por favor." + Environment.NewLine;
            }
            if (_error != "")
            {
                return Json(new { estado = 0, resultado = "Error", mensaje = _error });
            }
            else
            {
                resultList = datProm.BATACLUB_INSERTAR_CUPONES(operacion, Convert.ToDecimal(dscto), Convert.ToDateTime(fechaF), Convert.ToDecimal(pares), 
                    promocion, _usuario.usu_id, listaClientes, mesCumple, genero, String.Join(",", tienda), String.Join(",", tienda2), 
                    anio, Convert.ToDateTime(fechaI), prefix ,ref _prom_id, ref _mensaje);
                if (resultList == null)
                {
                    Session[_session_lista_cupones_excel] = null;
                    //Session[_session_lista_clientes_cupon] = null;
                    return Json(new { estado = 0, resultado = "Error", mensaje = _mensaje });
                }
                else
                {
                    Session[_session_lista_cupones_excel] = resultList;
                    Session[_session_lista_clientes_cupon] = resultList;
                    return Json(new { estado = 1, resultado = "", mensaje = "Operacion realizada con éxito." + (operacion == 1 ? "\nCodigo de la promocion: " + _prom_id : ""), prom_id = _prom_id });
                }
            }
        }
        public ActionResult getListaDetTdasProm(string prom_id)
        {
            List<Ent_BataClub_ListTdasProm> liststoreConf = datProm.get_lista_det_tdas_prom(prom_id);
            if (liststoreConf == null)
            {
                liststoreConf = new List<Ent_BataClub_ListTdasProm>();
            }
            return Json(new { estado = 1, tiendas = liststoreConf });
        }

        public FileContentResult ListaCuponesExcel()
        {
            if (Session[_session_lista_cupones_excel] == null)
            {
                List<Ent_BataClub_Cupones> liststoreConf = new List<Ent_BataClub_Cupones>();
                Session[_session_lista_cupones_excel] = liststoreConf;
            }
            List<Ent_BataClub_Cupones> lista = (List<Ent_BataClub_Cupones>)Session[_session_lista_cupones_excel];
            string[] columns = { "promocion", "dniCliente", "nombresCliente", "apellidosCliente", "correo", "cupon" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(lista, "", false, columns);
            string nom_excel = "Lista de Cupones generados";
            return File(filecontent, ExcelExportHelper.ExcelContentType, nom_excel + ".xlsx");
        }

        //Dropdownlist - cambio de fechas
        public ActionResult getPromDet(Ent_jQueryDataTableParams param, string valor_prom)
        {
            List<Ent_BataClub_CuponesCO> list = null;
            list = datProm.getPromDet(valor_prom);
            //datProm.get_ListaPromociones();
            return Json(
                list
            , JsonRequestBehavior.AllowGet);
        }

        //Table cupón
        // [HttpGet]
        public PartialViewResult _TableCupon(int operacion, string identificacion, string mesCumple, string genero)
        {
            if (operacion == 0)
            {
                Session[_session_lista_clientes_cupon] = null;
            }
            else
            {
                string mensaje = addClienteLista(operacion, identificacion, mesCumple, genero);
                if (mensaje != "")
                {
                    TempData["Error"] = mensaje;
                }
            }

            return PartialView();
        }

        //Borrar registro de la tabla
        public void BorrarRegistro(string dni)
        {
            if (((List<Ent_BataClub_CuponesCO>)(Session[_session_tabla_cupon_private])).Count > 0)
            {
                var list_orig = (List<Ent_BataClub_CuponesCO>)Session["_session_tabla_cupon_private"];
                IQueryable<Ent_BataClub_CuponesCO> membercol = ((List<Ent_BataClub_CuponesCO>)(Session[_session_tabla_cupon_private])).AsQueryable();
                IEnumerable<Ent_BataClub_CuponesCO> filteredMembers = membercol;
                filteredMembers = membercol
                    .Where(m =>
                   !m.dni.ToUpper().Contains(dni));

                Session[_session_tabla_cupon_private] = filteredMembers.ToList();

                //  var displayMembers = filteredMembers
                //.Skip(param.iDisplayStart)
                //.Take(param.iDisplayLength);

                //  var result = from a in displayMembers
                //               select new
                //               {
                //                   a.Nombres,
                //                   a.Apellidos,
                //                   a.dni,
                //                   a.correo
                //               };


                //  Session[_session_tabla_cupon_private] = listdoc;
            }

            // return RedirectToAction("_TableCupon");
            //  return PartialView("_TableCupon");
        }

        //Generación de cupones
        public string GenerarCupones(string desc, string fec_fin, int pares, string prom_des)
        {
            // Convert to DataTable.
            // DataTable table = ConvertListToDataTable(list_orig2);

            //var table = list_orig.Select(x => new {
            //    x.dni,
            //    Apellidos = x.Apellidos,
            //     Nombres = x.Nombres
            //}).CopyToDataTable();

            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string str = "0";
            var list_orig = (List<Ent_BataClub_CuponesCO>)Session["_session_tabla_cupon_private"];

            var table = ConvertToDataTable(list_orig);
            var list_cupones = datProm.GenerarCupones(_usuario.usu_id, Convert.ToDecimal(desc), Convert.ToDateTime(fec_fin), pares, prom_des, table);
            if (list_cupones.Count() > 0)
            { //Se generó correctamente. Entra a la sesión para luego exportar a excel.
                Session["_session_tabla_cupon_exportar_private"] = list_cupones;
                str = "1";
            }
            else
            { str = "0"; }
            return str;
        }

        //Exportar Excel de cupones generados
        [HttpGet]
        public FileContentResult ExportToExcel_Cupones()
        {
            //DateTime.Today.Date.ToShortDateString()
            List<Ent_BataClub_ListaCliente> list_cupones = (List<Ent_BataClub_ListaCliente>)Session[_session_tabla_cupon_exportar_private];
            string[] columns = { "dni", "nombre", "apellidos", "email", "barra", "error" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(list_cupones, "BATACLUB_Cupones", true, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "BATACLUB_Cupones.xlsx");
        }

        //Convertir Lista a DataTable
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
        //Convertir Lista String a DataTable
        //static DataTable ConvertListToDataTable(List<string[]> list)
        //{
        //    // New table.
        //    DataTable table = new DataTable();

        //    // Get max columns.
        //    int columns = 0;
        //    foreach (var array in list)
        //    {
        //        if (array.Length > columns)
        //        {
        //            columns = array.Length;
        //        }
        //    }

        //    // Add columns.
        //    for (int i = 0; i < columns; i++)
        //    {
        //        table.Columns.Add();
        //    }

        //    // Add rows.
        //    foreach (var array in list)
        //    {
        //        table.Rows.Add(array);
        //    }

        //    return table;
        //}

        #endregion

        #region Bataclub/Cliente
        //Index
        public ActionResult Cliente()
        {
            //Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            //string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            //string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            //string return_view = actionName + "|" + controllerName;

            //if (_usuario == null)
            //{
            //    return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            //}
            //else
            //{
            //    return View();
            //}
            if (Session["_BataClub_Canal_Combo"] == null)
            {
                ViewBag.Canal = datCan.get_lista();
                Session["_BataClub_Canal_Combo"] = ViewBag.Canal;
            }
            else
            { ViewBag.Canal = Session["_BataClub_Canal_Combo"]; }

            ViewBag.Departamento = datUbi.get_lista_Departamento();
            ViewBag.Provincia = datUbi.get_lista_Provincia(ViewBag.Provincia);
            ViewBag.Distrito = datUbi.get_lista_Distrito(ViewBag.Departamento, ViewBag.Provincia);

            return View();
        }


        //Table
        public PartialViewResult _TableCliente(string dni = "", string nombre = "", string apellido = "", string correo = "")
        {
            return PartialView(listaTablaCliente(dni, nombre, apellido, correo));
        }

        public List<Ent_BataClub_Cliente> listaTablaCliente(string dni, string nombre, string apellido, string correo)
        {
            List<Ent_BataClub_Cliente> list = datCli.get_lista_cliente(dni, nombre, apellido, correo);
            Session[_session_tabla_cliente_private] = list;
            return list;
        }

        public ActionResult BATACLUB_CONSULTA_CLIENTES_PROMOCION(string dni)
        {
            try
            {
                 List<Ent_Cliente_Promocion> proms = datCli.BATACLUB_CONSULTA_CLIENTES_PROMOCION(dni);
                if (proms == null)
                {
                    proms = new List<Ent_Cliente_Promocion>();
                }
                return Json(new { estado = 1, proms = proms });
            }
            catch (Exception ex)
            {
                return Json(new { estado = 0 , mensaje = ex.Message});
            }
        }

        public ActionResult getTableClienteAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_tabla_cliente_private] == null)
            {
                List<Ent_BataClub_Cliente> listdoc = new List<Ent_BataClub_Cliente>();
                Session[_session_tabla_cliente_private] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_BataClub_Cliente> membercol = ((List<Ent_BataClub_Cliente>)(Session[_session_tabla_cliente_private])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_BataClub_Cliente> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m =>
                    m.can_des.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    // m.canal.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.dni.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.nombres.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    // m.apellido_pat.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    // m.apellido_mat.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.genero.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.correo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.fec_nac.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.telefono.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    //  m.ubigeo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.ubigeo_distrito.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.fec_modif.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.fec_registro.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    // m.fec_miembro.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.cod_tda.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.des_entid.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.cod_cadena.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    // m.envio_correo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    //  m.fec_envio_correo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    //  m.gene_cupon.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.miem_bataclub.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.miem_bataclub_fecha.ToUpper().Contains(param.sSearch.ToUpper()));
            }

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            Func<Ent_BataClub_Cliente, string> orderingFunction =
                   (
                      //m => sortIdx == 0 ? m.fec_doc :
                      // m.fec_doc
                      m => sortIdx == 0 ? m.fec_registro :
                    sortIdx == 1 ? m.miem_bataclub_fecha :
                    sortIdx == 2 ? m.fec_modif :
                    sortIdx == 3 ? m.can_des :
                    m.nombres
                   );
            var sortDirection = Request["sSortDir_0"];
            if (sortDirection == "asc")
                filteredMembers = filteredMembers.OrderBy(orderingFunction);
            else
                filteredMembers = filteredMembers.OrderByDescending(orderingFunction);
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.can_des,
                             // a.canal,
                             a.dni,
                             a.nombres,
                             //  a.apellido_pat,
                             // a.apellido_mat,
                             a.genero,
                             a.correo,
                             a.fec_nac,
                             a.telefono,
                             // a.ubigeo,
                             a.ubigeo_distrito,
                             a.fec_modif,
                             a.fec_registro,
                             //  a.fec_miembro,
                             //  a.cod_tda,
                             a.des_entid,
                             a.cod_cadena,
                             // a.envio_correo,
                             // a.fec_envio_correo,
                             // a.gene_cupon,
                             a.miem_bataclub,
                             a.miem_bataclub_fecha
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

        //Exportar Excel
        [HttpGet]
        public FileContentResult ExportToExcelClientes()
        {
            List<Ent_BataClub_Cliente> listaclientes = (List<Ent_BataClub_Cliente>)Session[_session_tabla_cliente_private];
            string[] columns = { "can_des", "dni", "dni", "nombres", "apellido_pat", "apellido_mat", "genero", "correo", "fec_nac", "telefono", "ubigeo_distrito", "fec_modif"
            , "fec_registro", "fec_miembro", "cod_tda", "des_entid", "cod_cadena", "envio_correo", "fec_envio_correo", "gene_cupon", "miem_bataclub", "miem_bataclub_fecha"};
            byte[] filecontent = ExcelExportHelper.ExportExcel(listaclientes, "BATACLUB_Clientes", true, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "BATACLUB_Clientes.xlsx");
        }
        #endregion

        #region BataClub/Tablet

        public ActionResult TableBataPE()
        {
            return View();
        }

        public ActionResult TabletPrincipal()
        {
            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;
            HttpCookie cookie = HttpContext.Request.Cookies.Get("TiendaBata");
            if (cookie == null)
            {                
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {
                if (!String.IsNullOrEmpty(cookie.Value))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Control", new { returnUrl = return_view });
                }
            }
        }

        public ActionResult TabletPreRegistro()
        {
            return View();
        }
        public ActionResult TabletRegistro(Ent_BataClub_Registro registro)
        {
            ViewBag.Depto = datUbi.get_lista_Departamento();
            return View(registro);
        }
        public ActionResult TabletPreEncuesta()
        {
            Session[_session_boleta_encuesta] = null;
            return View();
        }
        public ActionResult TabletEncuesta()
        {
            List<Ent_BataClub_Preg_Encuesta> pregs = new List<Ent_BataClub_Preg_Encuesta>();
            pregs = datTab.get_ListaPromo_Disp();
            ViewBag.NPSValues = GetNpsThicks(pregs.First().VALOR_MIN, pregs.First().VALOR_MAX);
            ViewBag.Preguntas = pregs;
            
            return View();
        }
        public ActionResult PruebaTablet()
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
                List<Ent_BataClub_Preg_Encuesta> pregs = new List<Ent_BataClub_Preg_Encuesta>();
                pregs = datTab.get_ListaPromo_Disp();
                ViewBag.Depto = datUbi.get_lista_Departamento();
                ViewBag.NPSValues = GetNpsThicks(pregs.First().VALOR_MIN, pregs.First().VALOR_MAX);
                ViewBag.Preguntas = pregs;
                Session[_session_boleta_encuesta] = null;
                return View();
            }
        }
        
        public ActionResult RegistrarMiembro(Ent_BataClub_Registro registro)
        {
            string _mensaje = "";
            DateTime _temp;

            HttpCookie cookie = HttpContext.Request.Cookies.Get("TiendaBata");

            if (registro == null || cookie == null)
            {
                _mensaje += "No hay datos para registrar.";
                return Json(new { resultado = 0, mensaje = _mensaje });
            }
            if (String.IsNullOrEmpty(registro.Genero))
            {
                _mensaje += "Seleccione genero." + Environment.NewLine;
            }
            if (String.IsNullOrEmpty(registro.Nombres))
            {
                _mensaje += "Ingrese nombres por favor." + Environment.NewLine;
            }
            if (String.IsNullOrEmpty(registro.ApellidoPaterno))
            {
                _mensaje += "Ingrese apellido paterno por favor" + Environment.NewLine;
            }
            if (!IsDni(registro.Dni))
            {
                _mensaje += "Ingrese dni válido por favor" + Environment.NewLine;
            }
            if (String.IsNullOrEmpty(registro.Celular))
            {
                _mensaje += "Ingrese celular por favor" + Environment.NewLine;
            }
            if (!IsCorreo(registro.CorreoElectronico))
            {
                _mensaje += "Ingrese un correo electronico válido por favor." + Environment.NewLine;
            }
            if (!IsCorreo(registro.CorreoElectronico2))
            {
                _mensaje += "Ingrese un correo electronico de confirmacion válido por favor." + Environment.NewLine;
            }
            if (registro.CorreoElectronico != registro.CorreoElectronico2)
            {
                _mensaje += "Las direcciones de correo electronico no coinciden" + Environment.NewLine;
            }
            if (!DateTime.TryParse(registro.FechaNacimiento, out _temp)) {
                _mensaje += "Ingrese una fecha de nacimiento válido" + Environment.NewLine;
            }
            if (String.IsNullOrEmpty(registro.Departamento))
            {
                _mensaje += "Seleccione departamento por favor" + Environment.NewLine;
            }
            if (String.IsNullOrEmpty(registro.Provincia))
            {
                _mensaje += "Seleccione provincia por favor" + Environment.NewLine;
            }
            if (String.IsNullOrEmpty(registro.Distrito))
            {
                _mensaje += "Seleccione distrito por favor" + Environment.NewLine;
            }
            if (!registro.Politica)
            {
                _mensaje += "Debe aceptar la politica de privacidad y proteccion de datos personales para continuar...";
            }
            if (_mensaje != "")
            {
                return Json(new { resultado = 0, mensaje = _mensaje });
            }
            else
            {
                registro.Ubigeo = registro.Departamento + registro.Provincia + registro.Distrito;
                registro.UbigeoDistrito = datUbi.get_lista_Departamento().Where(w => w.cbo_codigo == registro.Departamento).Select(s => s.cbo_descripcion).FirstOrDefault()+", "+
                    datUbi.get_lista_Provincia(registro.Departamento).Where(w => w.cbo_codigo == registro.Provincia).Select(s => s.cbo_descripcion).FirstOrDefault()+", "+
                    datUbi.get_lista_Distrito(registro.Departamento,registro.Provincia).Where(w => w.cbo_codigo == registro.Distrito).Select(s => s.cbo_descripcion).FirstOrDefault();
                string _cor = "";
                string _tel = "";
                string _res = "";
                string _desc =  actualiza_cliente(registro, cookie.Value, ref _cor,ref _tel , ref _res);
                if (_res != "0")
                {
                    return Json(new { resultado = 0, mensaje = _mensaje + Environment.NewLine + _desc });
                }
                return Json(new { resultado = 1, mensaje = "Cliente registrado correctamente." });
            }
        }

        public ActionResult RegistrarActualizar(string dni , string operacion)
        {
            try
            {
                if (dni == null || dni == "" || dni.Length != 8 || !IsNumeric(dni))
                {
                    return Json(new { estado = 0, mensaje = "Ingrese un DNI válido por favor" });
                }
                string _mensaje = "";
                Ent_BataClub_Registro cliente = ValidarDNI(dni, "0" , ref _mensaje);
                return Json(new { estado = 1 ,  cliente = cliente });
            }
            catch (Exception ex)
            {
                return Json(new { estado = 0, mensaje = "Error al consultar DNI" + Environment.NewLine + ex.Message });
            }
           
        }
        public ActionResult EnviarCorreo(string dni)
        {
            try
            {
                string _mensaje = "";
                Ent_BataClub_Registro cliente = ValidarDNI(dni, null, ref _mensaje);
                return Json(new { estado = 1, mensaje = "Mensaje enviado."});
            }
            catch (Exception ex)
            {
                return Json(new { estado = 0, mensaje = "Error al enviar el mensaje." });
            }
        }
        public static string actualiza_cliente(Ent_BataClub_Registro registro, string tda, ref string correo_envio, ref string _telef_envia , ref string result)
        {
            string _valida = "";
            try
            {
                #region<REGION BATACLUB BATA>
                BataClub.BataEcommerceSoapClient cliente_bataclub = new BataClub.BataEcommerceSoapClient();
                BataClub.ValidateAcceso header = new BataClub.ValidateAcceso();
                header.Username = "EA646294-11F4-4836-8C6E-F5D9B5F681FC";
                header.Password = "DB959DFE-E49A-4F9B-8CD5-97364EE31FBA";

                BataClub.Ent_Cliente_BataClub cliente = new BataClub.Ent_Cliente_BataClub();
                cliente.canal = "06";
                cliente.dni = registro.Dni;
                cliente.primerNombre = registro.Nombres;
                cliente.apellidoPater = registro.ApellidoPaterno;
                cliente.apellidoMater = registro.ApellidoMaterno;
                cliente.correo = registro.CorreoElectronico;
                cliente.telefono = registro.Celular;
                cliente.tienda = tda;
                cliente.genero = registro.Genero;
                cliente.ubigeo = registro.Ubigeo;
                cliente.ubigeo_distrito = registro.UbigeoDistrito;
                cliente.fecNac = registro.FechaNacimiento;

                BataClub.Cliente_Parameter_Bataclub parameter = new BataClub.Cliente_Parameter_Bataclub();
                parameter.dni = registro.Dni;
                parameter.dni_barra = "";

                var existe_cl = cliente_bataclub.ws_consultar_Cliente(header, parameter);

                var registra_cl = cliente_bataclub.ws_registrar_Cliente(header, cliente);
                //if (!existe_cl.existe_cliente)
                //{
                //    if (_email.Length != 0)
                //    {
                //        var respuesta3 = cliente_metri.EnviarCorreoBienvenidaCliente_DniString(_ruc);
                //        _correo_envio = "1";
                //    }
                //    else
                //    {
                //        _telef_envia = "1";
                //    }
                //}
                if (registra_cl != null)
                {
                    result = registra_cl.codigo;
                    /*se inserto correctamente*/
                    if (registra_cl.codigo == "0")
                    {
                        if (!existe_cl.existe_cliente)
                        {
                            if (registro.CorreoElectronico.Length != 0)
                            {
                                correo_envio = "1";
                            }
                            else
                            {
                                _telef_envia = "1";
                            }
                        }
                        else
                        {
                            if (registro.CorreoElectronico.Length != 0)
                            {
                                correo_envio = "0";
                            }
                            else
                            {
                                _telef_envia = "0";
                            }
                        }
                    }
                    else
                    {
                        _valida = registra_cl.descripcion;
                    }

                }
                else
                {
                    _valida = "error de conexion de web service bata";
                }
                #endregion
            }
            catch (Exception exc)
            {
                _valida = exc.Message;
            }
            return _valida;
        }

        public Ent_BataClub_Registro ValidarDNI(string dni , string envia, ref string _mensaje )
        {
            Ent_BataClub_Registro cliente = new Ent_BataClub_Registro();            
            try
            {
                BataClub.BataEcommerceSoapClient cliente_bataclub = new BataClub.BataEcommerceSoapClient();
                BataClub.ValidateAcceso header = new BataClub.ValidateAcceso();
                header.Username = "EA646294-11F4-4836-8C6E-F5D9B5F681FC";
                header.Password = "DB959DFE-E49A-4F9B-8CD5-97364EE31FBA";

                BataClub.Cliente_Parameter_Bataclub parameter = new BataClub.Cliente_Parameter_Bataclub();
                parameter.dni = dni;
                parameter.dni_barra = "";
                parameter.envia_correo = envia; // "0"/*QUE NO ENVIE CORREO*/;

                var datacliente = cliente_bataclub.ws_consultar_Cliente(header, parameter);
                if (datacliente != null)
                {
                    cliente.Existe = datacliente.existe_cliente;
                    if (datacliente.existe_cliente)
                    {
                        cliente.Dni = datacliente.dni.ToString();
                        cliente.Nombres = datacliente.primerNombre + " " + datacliente.segundoNombre;
                        cliente.ApellidoPaterno = datacliente.apellidoPater;
                        cliente.ApellidoMaterno = datacliente.apellidoMater;
                        cliente.Miembro = datacliente.miembro_bataclub;
                        cliente.Celular = datacliente.telefono;
                        cliente.CorreoElectronico = datacliente.correo;
                        cliente.CorreoElectronico2 = datacliente.correo;
                    }
                    else
                    {
                        SunatReniec.Sunat_Reniec_PESoapClient clienteSunatReniec = new SunatReniec.Sunat_Reniec_PESoapClient();
                        SunatReniec.validateLogin la = new SunatReniec.validateLogin();
                        la.Username = "BataPeru";
                        la.Password = "Bata2018**.";

                        var dataClienteReniec = clienteSunatReniec.ws_persona_reniec(la, dni);
                        if (dataClienteReniec.Valida_Reniec.Estado == "0")
                        {
                            cliente.Dni = dataClienteReniec.Dni.ToString();
                            cliente.Nombres = dataClienteReniec.Nombres;
                            cliente.ApellidoPaterno = dataClienteReniec.ApePat;
                            cliente.ApellidoMaterno = dataClienteReniec.ApeMat;
                            cliente.Miembro = false;
                        }
                    }

                }else
                {
                    cliente.Dni = dni;
                    cliente.Existe = false;
                    cliente.Miembro = false;
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.Message;
                cliente.Dni = dni;
                cliente.Existe = false;
                cliente.Miembro = false;
            }
            return cliente;
        }
        public ActionResult get_otro_select(string operacion, string iddep = "0" , string idpro = "0")
        {
            List<Ent_Combo> res = null;
            if (operacion == "1" )
            {
                res = datUbi.get_lista_Provincia(iddep);
            }else if (operacion == "2")
            {
                res = datUbi.get_lista_Distrito(iddep,idpro);
            }
            return Json(new { estado = 1, result = res });
        }
        public ActionResult BATACLUB_VALIDAR_COMPROBANTE_ENCUESTA (string boleta)
        {
            Session[_session_boleta_encuesta] = null;
            try
            {
                HttpCookie cookie = HttpContext.Request.Cookies.Get("TiendaBata");
                int posGuion = boleta.IndexOf("-");
                string serie = (posGuion > -1 ? boleta.Substring(0, posGuion) : (cookie == null ? "x" : cookie.Value.Substring(2)));
                string strnumero = (posGuion > -1 ? boleta.Substring(posGuion + 1) : boleta);
                int numero = 0;
                if (IsNumeric(strnumero))
                {
                    numero = Convert.ToInt32(strnumero);
                }
                string cod_entid = "50" + (serie.Length > 3 ? serie.Substring(serie.Length - 3) : "x");
                Ent_BataClub_Encuesta encuesta = new Ent_BataClub_Encuesta();
                encuesta = datTab.BATACLUB_VALIDAR_COMPROBANTE_ENCUESTA(cod_entid, serie, numero.ToString());
                if (encuesta == null)
                {
                    return Json(new { resultado = 0, mensaje = "La boleta/factura no existe o ya fue utilizada en una encuesta ateriormente." });
                }
                else
                {
                    Session[_session_boleta_encuesta] = encuesta;
                    return Json(new { resultado = 1, mensaje = "Encontrado", info = encuesta });
                }
            }
            catch (Exception ex)
            {
                return Json(new { resultado = 0, mensaje = "Error al consultar el comprobante." + Environment.NewLine + ex.Message });
            }
         
        }
        public ActionResult BATACLUB_REGISTRAR_ENCUESTA(List<Ent_BataClub_Resp_Encuesta> respuestas, string correo = "")
        {
            try
            {
                HttpCookie cookie = HttpContext.Request.Cookies.Get("TiendaBata");
                string _mensaje = "";
                if (cookie == null)
                {
                    return Json(new { resultado = 0, mensaje = "No hay usuario Logeado." });
                }
                var regex = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                if (!System.Text.RegularExpressions.Regex.IsMatch(correo, regex))
                {
                    return Json(new { resultado = 0, mensaje = "Ingrese un correo electronico válido por favor." });
                }
                if (Session[_session_boleta_encuesta] == null)
                {
                    return Json(new { resultado = 0, mensaje = "No existe informacion del comprobante de pago, por favor, vuelva a ingresar." });
                }
                Ent_BataClub_Encuesta _encuesta = (Ent_BataClub_Encuesta)Session[_session_boleta_encuesta];
                _encuesta.COD_TDA_ENC = cookie.Value;
                _encuesta.CORREO = correo;
                _encuesta.DNI = "";
                //_encuesta.USUARIO = _usuario.usu_id.ToString();
                _encuesta.respuestas = respuestas;
                bool res = datTab.BATACLUB_SET_ENCUESTA(_encuesta, ref _mensaje);
                if (!res)
                {
                    return Json(new { resultado = 0, mensaje = _mensaje });
                }
                return Json(new { resultado = 1, mensaje = "Operacion completada con éxito." + Environment.NewLine + "Te hemos enviado un correo electronico con tu cupon." });
            }
            catch (Exception ex )
            {
                return Json(new { resultado = 0, mensaje = ex.Message });
            }            
        }
        
        public List<int> GetNpsThicks(int min, int max)
        {
            List<int> values = new List<int>();
            for (int i = min; i < max + 1; i++)
            {
                values.Add(i);
            }
            return values;
        }
        #endregion
        #region Helpers
        public static Boolean IsNumeric(string valor)
        {
            if (String.IsNullOrEmpty(valor))
            {
                return false;
            }
            int result;
            return int.TryParse(valor, out result);
        }
        public static Boolean IsCorreo(string valor)
        {

            var regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z"; ;
            if (String.IsNullOrEmpty(valor))
            {
                return false;
            }
            else
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(valor, regex))
                {
                    return true;
                }
            }
            return false;
        }
        public static Boolean IsDni (string valor)
        {
            if (!IsNumeric(valor))
            {
                return false;
            }
            if (valor.Length == 8)
            {
                return true;
            }
            return false;
        }
        //public bool IsCorreo(string emailaddress)
        //{
        //    try
        //    {
        //        MailAddress m = new MailAddress(emailaddress);
        //        return true;
        //    }
        //    catch (FormatException)
        //    {
        //        return false;
        //    }
        //}
        #endregion

        #region<BATACLUB CLIENTES REGISTROS Y MIEMBROS>
        private string _session_lista_registro_private = "_session_lista_registro_private";
        private Dat_BataClub_Lista_Registro dat_lst_reg = null;
        public ActionResult Index_ClientesBataClub()
        {
            ViewBag.Depto = datUbi.get_lista_Departamento();
            Session[_session_lista_registro_private] = null;
            return View();
        }
        public PartialViewResult _Lista_ClientesBataClub(string fechaIni, string fechaFin, string dni, string email)
        {

            List<Ent_BataClub_Lista_Registro> listar_cli = lista_cli_reg_bataclub(Convert.ToDateTime(fechaIni),Convert.ToDateTime(fechaFin), dni, email);

            return PartialView(listar_cli);
        }
        public List<Ent_BataClub_Lista_Registro> lista_cli_reg_bataclub(DateTime fec_ini, DateTime fec_fin, string dni, string correo)
        {
            List<Ent_BataClub_Lista_Registro> listar = null;
            try
            {
                dat_lst_reg = new Dat_BataClub_Lista_Registro();
                listar = dat_lst_reg.lista_registro(fec_ini, fec_fin, dni, correo);
                ViewBag.Depto = datUbi.get_lista_Departamento();
                Session[_session_lista_registro_private] = listar;
            }
            catch (Exception exc)
            {
             
            }
            return listar;
        }
        public ActionResult getListarCliBa(Ent_jQueryDataTableParams param)
        {

            /*verificar si esta null*/
            if (Session[_session_lista_registro_private] == null)
            {
                List<Ent_BataClub_Lista_Registro> listdoc = new List<Ent_BataClub_Lista_Registro>();
                Session[_session_lista_registro_private] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_BataClub_Lista_Registro> membercol = ((List<Ent_BataClub_Lista_Registro>)(Session[_session_lista_registro_private])).AsQueryable();  //lista().AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<Ent_BataClub_Lista_Registro> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.correo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.dni.ToUpper().Contains(param.sSearch.ToUpper()));
            }
            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            //Func<Ent_BataClub_Lista_Registro, string> orderingFunction =
            //(
            //m => sortIdx == 12 ? Convert.ToDateTime(m.fec_registro) :
            // "";
            //);
            var sortDirection = Request["sSortDir_0"];
            if (param.iSortingCols > 0)
            {
                if (sortDirection == "asc")
                {
                    switch (sortIdx)
                    {                        
                        case 0: filteredMembers = filteredMembers.OrderBy(o => o.canal);break;
                        case 1: filteredMembers = filteredMembers.OrderBy(o => o.tienda); break;
                        case 12: filteredMembers = filteredMembers.OrderBy(o => Convert.ToDateTime(o.fec_registro)); break;
                        case 13: filteredMembers = filteredMembers.OrderBy(o => (o.miem_bataclub_fecha == "" ? new DateTime() : Convert.ToDateTime(o.miem_bataclub_fecha))); break;
                        case 14: filteredMembers = filteredMembers.OrderBy(o => o.miem_bataclub); break;
                        default: break;
                    }
                }
                else
                {
                    switch (sortIdx)
                    {
                        case 0: filteredMembers = filteredMembers.OrderByDescending(o => o.canal); break;
                        case 1: filteredMembers = filteredMembers.OrderByDescending(o => o.tienda); break;
                        case 12: filteredMembers = filteredMembers.OrderByDescending(o => Convert.ToDateTime(o.fec_registro)); break;
                        case 13: filteredMembers = filteredMembers.OrderByDescending(o => (o.miem_bataclub_fecha == "" ? new DateTime() : Convert.ToDateTime(o.miem_bataclub_fecha))); break;
                        case 14: filteredMembers = filteredMembers.OrderByDescending(o => o.miem_bataclub); break;
                        default: break;
                    }
                }
            }


            //if (sortDirection == "asc")
            //    filteredMembers = filteredMembers.OrderBy(orderingFunction);
            //else
            //    filteredMembers = filteredMembers.OrderByDescending(orderingFunction);
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);
            var result = from a in displayMembers
                         select new
                         {
                             a.canal,
                             a.tienda,
                             a.dni,
                             a.primer_nombre,
                             a.segundo_nombre,
                             a.apellido_pat,
                             a.apellido_mat,
                             a.genero,
                             a.correo,
                             a.fec_nac,
                             a.telefono,
                             a.ubicacion,
                             a.fec_registro,
                             a.miem_bataclub_fecha,
                             a.miem_bataclub
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
        public ActionResult ModificarCliente(Ent_BataClub_Registro registro)
        {
            string _mensaje = "";
            try
            {
                Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
                if (!IsCorreo(registro.CorreoElectronico))
                {
                    _mensaje += "Ingrese un correo electronico válido por favor." + Environment.NewLine;
                }
                if (_mensaje == "")
                {
                    registro.Ubigeo = registro.Departamento + registro.Provincia + registro.Distrito;
                    registro.Canal = "07";
                    Dat_BataClub_Lista_Registro dato = new Dat_BataClub_Lista_Registro();
                                      
                    bool b = dato.Modificar_Cliente_Bataclub(registro, _usuario.usu_id.ToString(), ref _mensaje);
                    return Json(new { estado = b, mensaje = _mensaje == "" ? "Cliente BataClub modificado con éxito." : _mensaje });
                }
                else
                {
                    return Json(new {estado = false, mensaje = _mensaje });
                }
                
            }
            catch (Exception ex)
            {
                _mensaje = ex.Message;
                return Json(new { estado = false, mensaje = _mensaje });
            }
            
        }
        #endregion

        #region<CORREOS REBOTES BOUNCES>
        private string _session_listbounces_private = "_session_listbounces_private";
        private Dat_Bataclub_Bounce bc_bounces = new Dat_Bataclub_Bounce();
        public ActionResult icommkt_bounces()
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
                return View();
            }
        }
        public FileContentResult ExportToExcel_Bounce()
        {
            List<Ent_Bataclub_Bounce> listbataclub = (List<Ent_Bataclub_Bounce>)Session[_session_listbounces_private];
            string[] columns = { "RecordType", "MessageID", "Details", "Email", "BouncedAt", "Subject", "Canal", "Dni", "FecRegistro", "Tienda" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(listbataclub, "", true, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "BATACLUB_BOUNCES.xlsx");
        }
        public List<Ent_Bataclub_Bounce> lista(string fecha_ini, string fecha_fin)
        {
            List<Ent_Bataclub_Bounce> listbounces = bc_bounces.listar_bounce(Convert.ToDateTime(fecha_ini), Convert.ToDateTime(fecha_fin));
            Session[_session_listbounces_private] = listbounces;
            return listbounces;
        }

        public ActionResult getListBouncesAjax(Ent_jQueryDataTableParams param, string actualizar, string fecha_ini, string fecha_fin)
        {

            List<Ent_Bataclub_Bounce> listbounces = new List<Ent_Bataclub_Bounce>();

            if (!String.IsNullOrEmpty(actualizar))
            {
                listbounces = lista(fecha_ini, fecha_fin);
                //listAtributos = datOE.get_lista_atributos();
                Session[_session_listbounces_private] = listbounces;
            }

            /*verificar si esta null*/
            if (Session[_session_listbounces_private] == null)
            {

                listbounces = new List<Ent_Bataclub_Bounce>();

                Session[_session_listbounces_private] = listbounces;
            }


            //Traer registros
            IQueryable<Ent_Bataclub_Bounce> membercol = ((List<Ent_Bataclub_Bounce>)(Session[_session_listbounces_private])).AsQueryable();  //lista().AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<Ent_Bataclub_Bounce> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.Email.ToUpper().Contains(param.sSearch.ToUpper()) || m.Canal.ToUpper().Contains(param.sSearch.ToUpper()) 
                    || m.Dni.ToUpper().Contains(param.sSearch.ToUpper()));
            }
            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            if (param.iSortingCols > 0)
            {
                //if (Request["sSortDir_0"].ToString() == "asc")
                //{
                //    switch (sortIdx)
                //    {
                //        case 0: filteredMembers = filteredMembers.OrderBy(o => o.desp_nrodoc); break;
                //        case 1: filteredMembers = filteredMembers.OrderBy(o => o.desp_tipo_descripcion); break;
                //        case 2: filteredMembers = filteredMembers.OrderBy(o => Convert.ToDateTime(o.desp_fechacre)); break;
                //        case 3: filteredMembers = filteredMembers.OrderBy(o => o.totalparesenviado); break;
                //        case 4: filteredMembers = filteredMembers.OrderBy(o => o.estado); break;

                //    }
                //}
                //else
                //{
                //    switch (sortIdx)
                //    {
                //        case 0: filteredMembers = filteredMembers.OrderByDescending(o => o.desp_nrodoc); break;
                //        case 1: filteredMembers = filteredMembers.OrderByDescending(o => o.desp_tipo_descripcion); break;
                //        case 2: filteredMembers = filteredMembers.OrderByDescending(o => Convert.ToDateTime(o.desp_fechacre)); break;
                //        case 3: filteredMembers = filteredMembers.OrderByDescending(o => o.totalparesenviado); break;
                //        case 4: filteredMembers = filteredMembers.OrderByDescending(o => o.estado); break;
                //    }
                //}
            }
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);


            var result = from a in displayMembers
                         select new
                         {
                             a.RecordType,
                             a.MessageID,
                             a.Details,
                             a.Email,
                             a.BouncedAt,
                             a.Subject,
                             a.Canal,
                             a.Dni,
                             a.FecRegistro,
                             a.Tienda
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

    }
}