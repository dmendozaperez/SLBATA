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
        private string _session_tabla_cupones = "_session_tabla_cupones";
        private string _session_lista_promociones = "_session_lista_promociones";
        private string _session_lista_clientes_cupon = "_session_lista_clientes_cupon";
        private string _session_lista_cupones_excel = "_session_lista_cupones_excel";



        private string _session_prom_generar_cupon = "_session_prom_generar_cupon";


        private string _BataClub_Promociones_Combo = "_BataClub_Promociones_Combo";
        private string _BataClub_Canal_Combo = "_BataClub_Canal_Combo";
        private string _session_tabla_cupon_private = "_session_tabla_cupon_private";
        private string _session_tabla_cliente_private = "_session_tabla_cliente_private";
        private string _session_tabla_cupon_exportar_private= "_session_tabla_cupon_exportar_private";
        private string _BataClub_cupon_Combo = "_BataClub_cupon_Combo";
        private string _BataClub_Cupon_Desc = "_BataClub_Cupon_Desc";
        private string _BataClub_Cupon_FechaFin = "_BataClub_Cupon_FechaFin";
        private string _BataClub_Cupon_Pares = "_BataClub_Cupon_Pares";
        private string _BataClub_Promociones_estado = "_BataClub_Promociones_estado"; 
        private string _BataClub_Promociones_grafica= "_BataClub_Promociones_grafica"; 

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
                //if (Session["_dashboardData"] == null)
                //{
                //    Session["_dashboardData"] = datDash.GET_INFO_DASHBOARD();
                //}
                Session["_dashboardData"] = datDash.GET_INFO_DASHBOARD();

                Ent_BataClub_DashBoard dashboard = (Ent_BataClub_DashBoard)Session["_dashboardData"];
                ViewBag.general = dashboard.General;
                ViewBag.chartDS = informeBarChartData(dashboard, 2); // Barras mensual regsitros/miembros
                ViewBag.totalesGeneros = dashboard.listMesGenero;
                ViewBag.chartDonut = informeCanales(dashboard); // Donut anual canales
                ViewBag.chartMesParesSoles = informeBarChartData(dashboard, 4);
                ViewBag.promsPS = dashboard.listPromsPS;
                ViewBag.anios = datCbo.get_lista_anios(2015);

                ViewBag.BarChartTranReg = informeBarChartData(dashboard, 6);
                ViewBag.DetallesTiendaSuperv = dashboard.listTiendasSupervTot;

                return View();
            }
        }
        public ActionResult updateChartData(string anio, int informe , int mes = 0,string fecini = null , string fecfin = null , string prom = "")
        {
            Ent_BataClub_DashBoard dashboard = datDash.GET_INFO_DASHBOARD(anio, informe, mes,fecini , fecfin , prom);
            Ent_BataClub_Chart_Data chartDS = null;
            JsonResult jsonResult = new JsonResult();
            if (informe == 2)
            {
                chartDS = new Ent_BataClub_Chart_Data();
                chartDS = informeBarChartData(dashboard,2);
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
                chartDS = informeBarChartData(dashboard,4);
                jsonResult = Json(new { result = JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), promsPS = dashboard.listPromsPS });
            }
            else if (informe == 5)
            {               
                jsonResult = Json(new { promsPS = dashboard.listPromsPS });
            }
            else if (informe == 6)
            {
                chartDS = new Ent_BataClub_Chart_Data();
                chartDS = informeBarChartData(dashboard, 6);
                jsonResult = Json(new { result = JsonConvert.SerializeObject(chartDS, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), tiendas = dashboard.listTiendasSupervTot });
            }
            else if (informe == 7)
            {
                jsonResult = Json(new { promsDetPromTda = dashboard.listDetPromTda });
            }

            return jsonResult;        
        }
        public Ent_BataClub_Chart_Data informeSupervisor(Ent_BataClub_DashBoard dashboard)
        {
            Ent_BataClub_Chart_Data chartDSDonut = new Ent_BataClub_Chart_Data();
            Ent_BataClub_Chart_DataSet dsBCDonut = (new Ent_BataClub_Chart_DataSet()
            {
                backgroundColor = new string[] {
                        "rgba(99, 143, 197, 0.9)",
                        "rgba(221, 75, 57,0.9)",
                        "rgba(255, 206, 86, 0.8)",
                        "rgba(44, 122, 192, 0.8)",
                        "rgba(122, 65, 32,0.9)",
                        "rgba(245, 123, 56, 0.8)",
                        "rgba(14, 123, 245, 0.8)",
                        "rgba(33, 154, 34,0.9)",
                        "rgba(142, 121, 86, 0.8)",
                        "rgba(54, 45, 65, 0.8)" },
                data = dashboard.listSupervisorTot.Select(s => s.registros).ToArray()
            });
            chartDSDonut.labels = dashboard.listCanles.Select(s => s.CANAL).ToArray();
            chartDSDonut.datasets = new List<Ent_BataClub_Chart_DataSet>() { dsBCDonut };
            return chartDSDonut;
        }
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
            else if (informe == 6)
            {
                chartDS.datasets = new List<Ent_BataClub_Chart_DataSet>() {
                (new Ent_BataClub_Chart_DataSet()
                {
                    label = "REGISTROS",
                    backgroundColor = Enumerable.Repeat("rgba(180, 180, 180,0.8)", dashboard.listSupervisorTot.Count).ToArray(), // new string[] { "rgba(180, 180, 180,0.7)" },
                    borderWidth = "1",
                    data = dashboard.listSupervisorTot.Select(s => s.registros).ToArray()
                }),
                (new Ent_BataClub_Chart_DataSet()
                {
                    label = "TRANSACCIONES",
                    backgroundColor = Enumerable.Repeat("rgba(0, 166, 90,0.8)", dashboard.listSupervisorTot.Count).ToArray(),
                    borderWidth = "1",
                    data = dashboard.listSupervisorTot.Select(s => s.transac).ToArray()
                }) };
                chartDS.labels = dashboard.listSupervisorTot.Select(s => s.supervisor).ToArray();
                chartDS.labelsTooltip = new string[] { "Hola", "Hola", "Hola", "Hola", "Hola", "Hola", "Hola", "HOLA" };
            }     
            return chartDS;
        }

        public ActionResult get_tda_cadena(string cadenas)
        {
            List<Ent_Combo> list = datOE.get_tda_cadena(cadenas,1   );
            if (list == null)
            {
                list = new List<Ent_Combo>();
            }
            return Json(list);
        }

        [HttpPost]
        public ActionResult getListaCupProm(string codProm , string operacion)
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
                return Json(new { estado = 1, detalles =  detalles});
            }
            else
            {
                return Json(new { estado = 0, resultados = "No hay resultados." });
            }   
        }

        //Table
        public PartialViewResult _Table(string dni, string cupon, string hidden, string correo,string[] dwprom, string[] dwest)
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
                return PartialView(listaTablaPromociones(dni, cupon, String.Join(",",dwprom) , correo, String.Join(",", dwest)  ));
            }
        }

        public List<Ent_BataClub_Cupones> listaTablaPromociones(string dni, string cupon, string id_grupo, string correo,string dwest)
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
        public string addClienteLista(int operacion , string dniCorreo, string mesCumple , string genero)
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
            if(Session[_session_lista_clientes_cupon] == null)
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
                result = ("Sin resultados " + (!String.IsNullOrEmpty(dniCorreo) ?   " para: " + dniCorreo : "")).Trim();
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
                             a.nroCupones
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
                Session[_BataClub_Promociones_grafica]= strJson;
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
            string[] columns = { "promocion", "estado", "fechaFin", "nombresCliente", "dniCliente", "correo", "cupon", "porcDesc"};
            byte[] filecontent = ExcelExportHelper.ExportExcel(listbataclub, "", true, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "BATACLUB_Promociones.xlsx");
        }
        #endregion

        #region Bataclub/Cupon
        //Index
        [HttpPost]
        public ActionResult GenerarCupon(string codigo , string descripcion , decimal dscto , DateTime fecha, int pares)
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
                ViewBag.fecha = fecha.ToString("dd-MM-yyyy");
                ViewBag.pares = pares;
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
                ViewBag.fecha = DateTime.Now.ToString("dd-MM-yyyy");
                return View();
            }
        }
        //Table cupón 
        public ActionResult getTableCuponAjax(Ent_jQueryDataTableParams param , string dniEliminar)
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
                    m.dniCliente.ToUpper().Contains(param.sSearch.ToUpper()) || m.apellidosCliente.ToUpper().Contains(param.sSearch.ToUpper())||
                    (m.nombresCliente.Trim() + " " + m.apellidosCliente.Trim()).ToUpper().Contains(param.sSearch.ToUpper())
                    ) ;
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
        public static Boolean IsNumeric(string valor)
        {
            int result;
            return int.TryParse(valor, out result);
        }
        public ActionResult BATACLUB_INSERTAR_CUPONES(int operacion , string promocion , string dscto , string pares , string fecha, string mesCumple , string genero , string[] tienda)
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
            if (String.IsNullOrEmpty(fecha.Trim()))
            {
                _error += "Ingrese fecha por favor." + Environment.NewLine;
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
                resultList = datProm.BATACLUB_INSERTAR_CUPONES(operacion, Convert.ToDecimal(dscto), Convert.ToDateTime(fecha), Convert.ToDecimal(pares), promocion, _usuario.usu_id, listaClientes, mesCumple , genero , String.Join(",",tienda),ref _prom_id ,ref _mensaje);
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
                    return Json(new { estado = 1, resultado = "", mensaje = "Operacion realizada con éxito."+(operacion == 1 ? "\nCodigo de la promocion: " + _prom_id : ""), prom_id = _prom_id });
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
            string[] columns = { "promocion", "dniCliente","nombresCliente", "apellidosCliente", "correo", "cupon"};
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
        public PartialViewResult _TableCupon(int operacion, string identificacion,  string mesCumple , string genero)
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
                       ! m.dni.ToUpper().Contains(dni));

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
            { str = "0";}
            return str;
        }

        //Exportar Excel de cupones generados
        [HttpGet]
        public FileContentResult ExportToExcel_Cupones()
        {
            //DateTime.Today.Date.ToShortDateString()
             List<Ent_BataClub_ListaCliente> list_cupones = (List<Ent_BataClub_ListaCliente>)Session[_session_tabla_cupon_exportar_private];
            string[] columns = { "dni", "nombre", "apellidos", "email", "barra", "error"};
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
        public PartialViewResult _TableCliente(string dni="", string nombre = "", string apellido="", string correo = "")
        {
            return PartialView(listaTablaCliente(dni, nombre, apellido, correo));
        }

        public List<Ent_BataClub_Cliente> listaTablaCliente(string dni, string nombre, string apellido, string correo )
        {
            List<Ent_BataClub_Cliente> list = datCli.get_lista_cliente(dni, nombre,  apellido, correo);
            Session[_session_tabla_cliente_private] = list;
            return list;
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

    }
}