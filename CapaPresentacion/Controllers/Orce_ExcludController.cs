using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad.Util;
using CapaDato.Reporte;
using CapaDato.OrceExclud;
using CapaEntidad.General;
using CapaEntidad.OrceExlud;
using Newtonsoft.Json;
using CapaEntidad.Control;
using CapaPresentacion.Bll;
using CapaEntidad.BataClub;

namespace CapaPresentacion.Controllers
{
    public class Orce_ExcludController : Controller
    {
        // GET: Orce_Exclud
        Dat_OrceExclud datOE = new Dat_OrceExclud();
        string _session_lista_articulos = "_session_lista_articulos";
        string _session_atribuo_actual = "_session_atribuo_actual";
        string _session_lista_orce = "_session_lista_orce";
        string _session_lista_atributos = "_session_lista_atributos";
        string _session_tdas_xstore = "_session_tdas_xstore";
        string _session_art_excel = "_session_art_excel";
        string _session_cant_art_atr = "_session_cant_art_atr";
        string _session_ultimo_cod_orce = "_session_ultimo_cod_orce";

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
                Session[_session_lista_orce] = null;
                return View();
            }

        }
        public ActionResult Atributos()
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
                Session[_session_lista_atributos] = null;
                return View();
            }

        }
        public ActionResult getListaAjax(Ent_jQueryDataTableParams param , string actualizar )
        {
            List<Ent_Orce_Inter_Cab> listOrceExclude = null;
            if (!String.IsNullOrEmpty(actualizar))
            {
                listOrceExclude = datOE.get_lista_orce_exlude();
                Session[_session_lista_orce] = listOrceExclude;
            }            

            /*verificar si esta null*/
            if (Session[_session_lista_orce] == null)
            {
                List<Ent_Orce_Inter_Cab> liststoreConf = new List<Ent_Orce_Inter_Cab>();
                liststoreConf = datOE.get_lista_orce_exlude();
                if (liststoreConf == null)
                {
                    liststoreConf = new List<Ent_Orce_Inter_Cab>();
                }
                Session[_session_lista_orce] = liststoreConf;
            }
            List<Ent_Orce_Inter_Cab> _list = new List<Ent_Orce_Inter_Cab>();


            //Traer registros

            IQueryable<Ent_Orce_Inter_Cab> membercol = ((List<Ent_Orce_Inter_Cab>)(Session[_session_lista_orce])).AsQueryable();  //lista().AsQueryable();
            
            try
            {
                Session[_session_ultimo_cod_orce] = (Convert.ToInt32(membercol.Max(m => m.ORC_COD)) + 1).ToString();
            }
            catch
            {
                Session[_session_ultimo_cod_orce] = "0";
            }


            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<Ent_Orce_Inter_Cab> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.ORC_DESCRIPCION.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.ORC_FECHA_ING.ToString().Contains(param.sSearch.ToUpper()));
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
                        filteredMembers = filteredMembers.OrderBy(o => o.ORC_COD);
                    }
                    else if(sortIdx == 3)
                    {
                        filteredMembers = filteredMembers.OrderBy(o => Convert.ToDateTime(o.ORC_FECHA_ING));
                    }
                }
                else
                {
                    if (sortIdx == 0)
                    {
                        filteredMembers = filteredMembers.OrderByDescending(o => o.ORC_COD);
                    }
                    else if (sortIdx == 3)
                    {
                        filteredMembers = filteredMembers.OrderByDescending(o => Convert.ToDateTime(o.ORC_FECHA_ING));
                    }
                }
            }

            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);
            var result = from a in displayMembers
                         select new
                         {
                             a.ORC_COD,
                             a.ORC_DESCRIPCION,
                             a.ORC_ATRIBUTO,
                             a.ORC_ENVIADO,
                             a.ORC_FEC_ENV,
                             a.ORC_EST_ID,
                             a.ORC_FECHA_ING,
                             a.ORC_FECHA_ACT,
                             a.EST_ORC_DES
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

        public ActionResult getListaArticulosAjax(Ent_jQueryDataTableParams param, string actualizar)
        {
            List<Ent_Orce_Exclud_Atributo> listAtributos = null;
            if (!String.IsNullOrEmpty(actualizar))
            {
                listAtributos = datOE.get_lista_atributos();
                Session[_session_lista_atributos] = listAtributos;
            }

            /*verificar si esta null*/
            if (Session[_session_lista_atributos] == null)
            {
                List<Ent_Orce_Exclud_Atributo> liststoreConf = new List<Ent_Orce_Exclud_Atributo>();
                liststoreConf = datOE.get_lista_atributos();
                if (liststoreConf == null)
                {
                    liststoreConf = new List<Ent_Orce_Exclud_Atributo>();
                }
                Session[_session_lista_atributos] = liststoreConf;
            }

            //Traer registros

            IQueryable<Ent_Orce_Exclud_Atributo> membercol = ((List<Ent_Orce_Exclud_Atributo>)(Session[_session_lista_atributos])).AsQueryable();  //lista().AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<Ent_Orce_Exclud_Atributo> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.COD_ATR.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.DES_ATR.ToString().Contains(param.sSearch.ToUpper()) ||
                     m.FECHA_CREACION.ToString().Contains(param.sSearch.ToUpper()));
            }
            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Ent_Orce_Exclud_Atributo, DateTime> orderingFunction =
                (
                    m => Convert.ToDateTime( m.FECHA_CREACION)
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
                             a.COD_ATR,
                             a.DES_ATR,
                             a.ESTADO,
                             a.USUARIO_CREA,
                             a.USUARIO_MODIFICA,
                             a.FECHA_CREACION,
                             a.FECHA_MODIFICA
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

        public ActionResult getListaDetTdas(string cod_orce)
        {
            List<Ent_Orce_Inter_Det_Tda> liststoreConf = datOE.get_lista_det_tdas(cod_orce);
            if (liststoreConf == null)
            {
                liststoreConf = new List<Ent_Orce_Inter_Det_Tda>();
            }
            return Json(new { estado = 1 , tiendas = liststoreConf });
        }

        public ActionResult getListaDetArt(string cod_orce)
        {
            List<Ent_Orce_Inter_Art> liststoreConf = datOE.get_lista_det_art(cod_orce);
            if (liststoreConf == null)
            {
                liststoreConf = new List<Ent_Orce_Inter_Art>();
            }            
            return Json(new { estado = 1, articulos = liststoreConf });
        }
        public ActionResult getListaArtAjax(string cod_atr)
        {
            List<Ent_Orce_Inter_Art> liststoreConf = datOE.get_articulos_atributo(cod_atr);
            if (liststoreConf == null)
            {
                liststoreConf = new List<Ent_Orce_Inter_Art>();
            }
            Session[_session_lista_articulos] = liststoreConf;
            return Json(new { estado = 1, articulos = liststoreConf });
        }

        public ActionResult Nuevo()
        {
            List<Ent_Tda_Xstore> tdas = datOE.tiendatipo_xstore();
            Session[_session_tdas_xstore] = tdas;
            ViewBag.listCadena = tdas.Select(s => new { s.cod_cadena, s.des_cadena }).Distinct();

            Session[_session_lista_articulos] = null;
            Session[_session_art_excel] = null;
            Session[_session_cant_art_atr] = null;
          //  Session[_session_atribuo_actual] = null;

            //ViewBag.listCadena = datOE.get_cadena();
            ViewBag.listAtr = datOE.get_atributos();

            ViewBag.listTipoTda = new List<Ent_Tda_Xstore>();
            ViewBag.listTdaCadena = new List<Ent_Tda_Xstore>();
            ViewBag.idOrce = Session[_session_ultimo_cod_orce];

            Ent_Orce_Inter_Cab model = new Ent_Orce_Inter_Cab();
            model.TIENDAS = new List<Ent_Orce_Inter_Det_Tda>();
            
            return View(model);
        }
        public ActionResult Editar(int id)
        {
            List<Ent_Tda_Xstore> tdas = datOE.tiendatipo_xstore();
            Session[_session_tdas_xstore] = tdas;

            Session[_session_lista_articulos] = null;
            Session[_session_art_excel] = null;
            Session[_session_cant_art_atr] = null;
            ViewBag.listCadena = tdas.Select(s => new { s.cod_cadena, s.des_cadena }).Distinct();

            List<Ent_Orce_Inter_Det_Tda> _detTdas = datOE.get_lista_det_tdas(id.ToString());
            
            var tipoTdas = tdas.Where(w => _detTdas.Select(s => s.ORC_DET_TDA).Contains(w.cod_entid)).Select(s => s.tiptda_cod).Distinct();

            ViewBag.listTipoTda = tdas.Where(w => _detTdas.Select(s => s.ORC_DET_TDA_CAD).Contains(w.cod_cadena)).Select(s => new { s.tiptda_cod, s.tiptda_des }).Distinct().ToList();
            ViewBag.listTdaCadena = tdas.Where(w =>
                _detTdas.Select(s => s.ORC_DET_TDA_CAD).Contains(w.cod_cadena) && 
                tipoTdas.Contains(w.tiptda_cod)
            ).Select(s => new { s.cod_entid, s.des_entid }).Distinct().ToList();


            ViewBag.listAtr = datOE.get_atributos();
            ViewBag.tipoTdaSelected = tipoTdas;

            Ent_Orce_Inter_Cab model = new Ent_Orce_Inter_Cab();
            model = ((List<Ent_Orce_Inter_Cab>)Session[_session_lista_orce]).Where(d => d.ORC_COD == id).First();
            model.TIENDAS = _detTdas;
            
            Session[_session_lista_articulos] = datOE.get_lista_det_art(model.ORC_COD.ToString());
            Session[_session_atribuo_actual] = model.ORC_ATRIBUTO;
            return View(model);
        }

        public ActionResult get_tipo_cadena(string cadenas)
        {
            var list = ((List<Ent_Tda_Xstore>)Session[_session_tdas_xstore]).Where(w =>  String.Join(",",cadenas).Contains(w.cod_cadena)).Select(s => new { s.tiptda_cod, s.tiptda_des }).Distinct().ToList();
            return Json(list);
        }

        public ActionResult get_tda_cadena(string cadenas , string tipos)
        {
            var list = ((List<Ent_Tda_Xstore>)Session[_session_tdas_xstore]).Where(w => String.Join(",", cadenas).Contains(w.cod_cadena) && String.Join(",", tipos).Contains(w.tiptda_cod)).Select(s => new { s.cod_entid, s.des_entid }).Distinct().ToList();
            return Json(list);
        }

        public List<Ent_Orce_Inter_Art> get_articulos_atr(string atributo)
        {
            List<Ent_Orce_Inter_Art> list = datOE.get_articulos_atributo(atributo);
            if (list == null)
            {
                list = new List<Ent_Orce_Inter_Art>();
            }
            Session[_session_lista_articulos] = list;
            Session[_session_atribuo_actual] = atributo;
            //Session[_session_cant_art_atr] = list.Count();
            Session[_session_art_excel] = null;
            return (list);
        }

        public List<Ent_Orce_Inter_Art> unirListas(List<Ent_Orce_Inter_Art> lista)
        { 
            List<Ent_Orce_Inter_Art> listArt_Actual = (List<Ent_Orce_Inter_Art>)Session[_session_lista_articulos];
            listArt_Actual.Select(s => { s.GENERAR = false; return s; }).ToList();
            listArt_Actual = listArt_Actual.Where(w => !lista.Select(s => s.ARTICULO).ToList().Contains(w.ARTICULO)).OrderBy(o => o.ARTICULO).ToList(); // listArt_Actual.Except(lista).ToList();
            return listArt_Actual;
        }
        public ActionResult JsonExcelArticulos(string articulos)
        {
            List<Ent_Orce_Inter_Art> listArtExcel = null;            
            try
            {
                listArtExcel = new List<Ent_Orce_Inter_Art>();
                listArtExcel = JsonConvert.DeserializeObject<List<Ent_Orce_Inter_Art>>(articulos.ToUpper());
                listArtExcel.Select(s => { s.GENERAR = true; return s; }).ToList();
                Session[_session_art_excel] = listArtExcel.Select(s=>s.ARTICULO).ToArray();
                if (listArtExcel.Where(d => String.IsNullOrEmpty(d.ARTICULO) || String.IsNullOrEmpty(d.VALOR.ToString())).ToList().Count > 0)
                {
                    return Json(new { estado = 0, resultados = "El archivo no tiene el formato correcto. Los nombres de las columnas deben ser 'ARTICULO' Y 'VALOR' y no deben existir campos vacios." });
                }else
                {
                    Session[_session_lista_articulos] = listArtExcel.Union(unirListas(listArtExcel)).ToList();
                    return Json(new { estado = 1, resultados = "ok" });
                }                
            }
            catch (Exception ex)
            {
                return Json(new { estado = 0, resultados = ex.Message });
            }
        }
        public ActionResult getArtAjax(Ent_jQueryDataTableParams param, string atributo , string _art_mod , bool ordenar, bool _all_check ,bool _all_check_val , bool _all_check_gen , bool _all_checkG ,string _art_mod_gen = "" , bool check_excel = false)
        {
            if (atributo != "-1")
            {
                Session[_session_lista_articulos] = get_articulos_atr(atributo);
            }
            if (Session[_session_lista_articulos] == null)
            {
                List<Ent_Orce_Inter_Art> _listNull = new List<Ent_Orce_Inter_Art>();
                Session[_session_lista_articulos] = _listNull;
            }

            if (_art_mod != "")
            {
                List<Ent_Orce_Inter_Art> listArt = (List<Ent_Orce_Inter_Art>)Session[_session_lista_articulos];
                listArt.Where(w => w.ARTICULO == _art_mod).Select(a => { a.VALOR = !a.VALOR ; return a; }).ToList();
                Session[_session_lista_articulos] = listArt;
            }
            if (_art_mod_gen != "")
            {
                List<Ent_Orce_Inter_Art> listArt = (List<Ent_Orce_Inter_Art>)Session[_session_lista_articulos];
                listArt.Where(w => w.ARTICULO == _art_mod_gen).Select(a => { a.GENERAR = !a.GENERAR; return a; }).ToList();
                Session[_session_lista_articulos] = listArt;
            }
            if (_all_check)
            {
                List<Ent_Orce_Inter_Art> lista = (List<Ent_Orce_Inter_Art>)Session[_session_lista_articulos];
                lista.Select(a => { a.VALOR = _all_check_val; return a; }).ToList();
                Session[_session_lista_articulos] = lista;
            }
            if (_all_checkG)
            {
                List<Ent_Orce_Inter_Art> lista = (List<Ent_Orce_Inter_Art>)Session[_session_lista_articulos];
                lista.Select(a => { a.GENERAR = _all_check_gen; return a; }).ToList();
                Session[_session_lista_articulos] = lista;
            }
            if (check_excel)
            {
                List<Ent_Orce_Inter_Art> lista = (List<Ent_Orce_Inter_Art>)Session[_session_lista_articulos];
                lista.Where(w=>((string[])Session[_session_art_excel]).Contains(w.ARTICULO)).Select(a => { a.GENERAR = true; return a; }).ToList();
                Session[_session_lista_articulos] = lista;
            }
            IQueryable<Ent_Orce_Inter_Art> membercol = ((List<Ent_Orce_Inter_Art>)Session[_session_lista_articulos]).AsQueryable();  //lista().AsQueryable();

            //displayMembers.Select(a => { a.ESTADO_CONEXION_CAJA_XST = dat_storeTda.PingHost(a.IP); return a; }).ToList();
            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Orce_Inter_Art> filteredMembers = membercol;           

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.ARTICULO.ToUpper().Contains(param.sSearch.ToUpper()));
            }
            //Manejador de orden

            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Ent_Orce_Inter_Art, string> orderingFunction =
            (
            m => sortIdx == 0 ?  m.ARTICULO : m.VALOR.ToString());
            var sortDirection = Request["sSortDir_0"];
           if (ordenar)
            {
                if (sortDirection == "asc")
                    filteredMembers = filteredMembers.OrderBy(orderingFunction);
                else
                    filteredMembers = filteredMembers.OrderByDescending(orderingFunction);
            }

            var nroReg = filteredMembers.Count();
            var nroExcel = 0;
            if (Session[_session_art_excel] != null)
            {
                string[] arts = (string[])Session[_session_art_excel];
                nroExcel = (int)arts.Count();
            }            
            var nroGenerar = filteredMembers.Count(c => c.GENERAR == true);
            var nroTrue = filteredMembers.Count(c => c.VALOR == true);
            var nroFalse = filteredMembers.Count(c => c.VALOR == false);
            // param.variable1 = lblConsumidos;

            int[] cants;
            cants = new int[] { nroReg, nroExcel, nroGenerar , nroTrue , nroFalse };          
            
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                            select new
                            {
                                a.ARTICULO,
                                a.VALOR,
                                a.GENERAR
                            };
            //Se devuelven los resultados por json
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = result,
                variable1 = String.Join("|", cants)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ORCE_INTERFACE_EXCLUD_ACT(string codigo , string descripcion , string cadena, string tdaCadena, string atributo , string est_orce, int operacion)
        {
            string estado_orce = est_orce;
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];    
            List<Ent_Orce_Inter_Art> listArticulos = null; // new List<Ent_Articulo_OE>();
            string _error = "";
            string _mensaje = "";
            int result = 0;
            int _operacion = operacion;
            int _codigo = (String.IsNullOrEmpty(codigo) ? 0 : Convert.ToInt32(codigo));
            if (_usuario == null)
            {
                return Json(new { estado = 0, resultado = "Error", mensaje = "No hay usuario logeado." });
            }
            if (operacion != 3)
            {
                if (Session[_session_lista_articulos] != null)
                {
                    listArticulos = new List<Ent_Orce_Inter_Art>();
                    listArticulos = (List<Ent_Orce_Inter_Art>)Session[_session_lista_articulos];
                }
                if (listArticulos == null || (listArticulos != null && listArticulos.Count == 0))
                {
                    _error += "La lista de articulos está vacia" + Environment.NewLine;
                }
                if (listArticulos.Count(c => c.GENERAR == true) == 0)
                {
                    _error += "No hay articulos seleccionados para generar." + Environment.NewLine;
                }
                if (String.IsNullOrEmpty(descripcion.Trim()))
                {
                    _error += "Ingrese una descripcion." + Environment.NewLine;
                }
                if (String.IsNullOrEmpty(cadena))
                {
                    _error += "Seleccione cadena." + Environment.NewLine;
                }
                if (String.IsNullOrEmpty(tdaCadena) || tdaCadena == "-1")
                {
                    _error += "Seleccione tienda(s)." + Environment.NewLine;
                }
                if (String.IsNullOrEmpty(atributo) || atributo == "-1")
                {
                    _error += "Seleccione un atributo valido." + Environment.NewLine;
                }
            }
            if (_error != "")
            {
                return Json(new { estado = 0, resultado = "Error" , mensaje = _error });
            }
            else
            {
                result = datOE.ORCE_INTERFACE_EXCLUD_ACT(_codigo, String.IsNullOrEmpty(descripcion) ? "" : descripcion, String.IsNullOrEmpty(atributo) ? "" : atributo, estado_orce, _usuario.usu_id, operacion, listArticulos, String.IsNullOrEmpty(tdaCadena) ? "" : tdaCadena, ref _mensaje);
                if (result == 1)
                {
                    return Json(new { estado = 1 , resultado = "" , mensaje = "Operacion realizada con éxito." });
                }
                else
                {
                    return Json(new { estado = 0, resultado = "Error", mensaje = _mensaje });
                }
            }
        }
        public FileContentResult ListaArticulosExcel()
        {
            if (Session[_session_lista_articulos] == null)
            {
                List<Ent_Orce_Inter_Art> liststoreConf = new List<Ent_Orce_Inter_Art>();
                Session[_session_lista_articulos] = liststoreConf;
            }
            List<Ent_Orce_Inter_Art> lista = (List<Ent_Orce_Inter_Art>)Session[_session_lista_articulos];
            string[] columns = { "ARTICULO", "VALOR"};
            byte[] filecontent = ExcelExportHelper.ExportExcel3( lista, "", false, columns);
            string nom_excel = "Lista de Articulos";
            if (lista.Count > 0)
            {
                nom_excel = lista.First().ATRIBUTO;
            }            
            return File(filecontent, ExcelExportHelper.ExcelContentType, nom_excel + ".xlsx");
        }
        #region Cupones Orce
        public ActionResult Cupones()
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
        public ActionResult GenerarCupones()
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
        public ActionResult GenerarCuponesOrce(string prefx)
        {
            string _mensaje = "";
            Ent_BataClub_Orce_Promotion res = new Ent_BataClub_Orce_Promotion();
            try
            {
                res = datOE.ORCE_CUPONES_BATACLUB_REFRESH(prefx, ref _mensaje);
                if (_mensaje != "")
                {
                    return Json(new { estado = false, mensaje = _mensaje });
                }else
                {
                    return Json(new { estado = true, orceProm = res, mensaje = _mensaje });
                }
            }
            catch (Exception ex)
            {
                return Json(new { estado = false, mensaje = ex.Message });
            }
            
            
        }
        #endregion

    }
}
