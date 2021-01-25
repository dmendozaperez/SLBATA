using CapaDato.Inventario;
using CapaDato.Maestros;
using CapaEntidad.Control;
using CapaEntidad.General;
using CapaEntidad.Inventario;
using CapaEntidad.Maestros;
using CapaEntidad.Util;
using CapaPresentacion.Bll;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaPresentacion.Data.Crystal.Reporte;
using CapaPresentacion.Models.Crystal.Reporte;

namespace CapaPresentacion.Controllers
{
    public class InventarioController : Controller
    {
        private Dat_Inventario_Consulta datInv = new Dat_Inventario_Consulta(); //gft
        private Dat_ListaTienda dat_lista_tienda = new Dat_ListaTienda();
        private string _session_tabla_inventCons_private = "_session_tabla_inventCons_private"; //gft
        private string _Inventario_Tienda_Combo = "_Inventario_Tienda_Combo";
        private string _Inventario_TiendaFecha_Combo = "_Inventario_TiendaFecha_Combo";
        private string _session_lista_articulos_inv = "_session_lista_articulos";
        private string _session_lista_ajuste_inv = "_session_lista_ajuste_inv";

        // GET: Inventario

        #region Consulta inventario
        //Index
        public ActionResult Consulta_Inv()
        {
            string cod_entid = "";

            //Combo Tienda
            if (Session["_Inventario_Tienda_Combo"] == null)
            {
                ViewBag.Tienda = datInv.get_ListaTienda();
                // ((datInv.get_ListaTienda()).Items[0]).cod_entid
                //List<Ent_Inventario_Tienda> list1 = new List<Ent_Inventario_Tienda>()
                //{
                //    new Ent_Inventario_Tienda(){  cod_entid = "-1", des_entid="Seleccione Tienda" },
                //};
                Session["_Inventario_Tienda_Combo"] = ViewBag.Tienda;
                cod_entid = ViewBag.Tienda[0].cod_entid.ToString();
                ViewBag.Fecha = datInv.get_ListaFecha(cod_entid);
                Session["_Inventario_TiendaFecha_Combo"] = ViewBag.Fecha;
            }
            else
            {
                ViewBag.Tienda = Session["_Inventario_Tienda_Combo"];
                // cod_entid = ViewBag.Tienda[0].cod_entid.ToString();
                //  ViewBag.Fecha = datInv.get_ListaFecha(cod_entid);
                ViewBag.Fecha = Session["_Inventario_TiendaFecha_Combo"];
            }
            return View();
        }

        //Table
        public PartialViewResult _TableConsInv(string articulo, string talla, string dwtda, string dwfec)
        {
            //xst_inv_fec_aud
            if (dwtda == null || dwfec == null)
            { return PartialView(); }
            else
            { return PartialView(listaTablaConsulta(articulo, talla, dwtda, dwfec)); }
        }

        public List<Ent_Inventario_Consulta> listaTablaConsulta(string articulo, string talla, string dwtda, string dwfec)
        {
            List<Ent_Inventario_Consulta> list = datInv.get_ListaInv_Consulta(dwtda, dwfec, articulo, talla);
            Session[_session_tabla_inventCons_private] = list;
            return list;
        }

        public ActionResult getTableInventAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_tabla_inventCons_private] == null)
            {
                List<Ent_Inventario_Consulta> listdoc = new List<Ent_Inventario_Consulta>();
                Session[_session_tabla_inventCons_private] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_Inventario_Consulta> membercol = ((List<Ent_Inventario_Consulta>)(Session[_session_tabla_inventCons_private])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Inventario_Consulta> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m =>
                    m.articulo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.calidad.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    //  m.des_entid.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.diferencia.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.talla.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    //  m.fecha_inv.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.teorico.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.fisico.ToUpper().Contains(param.sSearch.ToUpper()));
            }

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            Func<Ent_Inventario_Consulta, string> orderingFunction =
                   (
                      //m => sortIdx == 0 ? m.fec_doc :
                      // m.fec_doc
                      m => sortIdx == 0 ? m.fecha_inv :
                    sortIdx == 1 ? m.articulo :
                    sortIdx == 2 ? m.talla :
                    sortIdx == 3 ? m.fisico :
                    m.diferencia
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
                             a.articulo,
                             a.calidad,
                             a.des_entid,
                             a.diferencia,
                             a.fecha_inv,
                             a.fisico,
                             a.teorico,
                             a.talla
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

        //Dropdownlist - cambio de fechas
        public ActionResult getDropdrowlistFecha(Ent_jQueryDataTableParams param, string valor_tienda)
        {
            ViewBag.Fecha = datInv.get_ListaFecha(valor_tienda);

            //return Json(new[] {
            //    ViewBag.Fecha
            //}, JsonRequestBehavior.AllowGet);
            return Json(
                ViewBag.Fecha
            , JsonRequestBehavior.AllowGet);
        }

        //Exportar Excel
        [HttpGet]
        public FileContentResult ExportToExcel()
        {
            List<Ent_Inventario_Consulta> list = (List<Ent_Inventario_Consulta>)Session[_session_tabla_inventCons_private];
            string[] columns = { "des_entid", "articulo", "calidad", "talla", "teorico", "fisico", "diferencia", "nombres_venta", "fecha_inv" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(list, "Inventario_Consulta", true, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "Inventario_Consulta.xlsx");
        }

        #endregion

        #region ****Movimientos por Fecha*****

        public ActionResult ConsultaMovimiento()
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
                ViewBag.Tienda = dat_lista_tienda.get_tienda(Session["PAIS"].ToString(), "1");
                Session["Lista_Consulta_Movimiento"] = null;
                return View();
            }


        }

        public PartialViewResult MostrarResultados(string fec, string dwtda)
        {
            return PartialView(ListarConsultaMovimiento(fec, dwtda));
        }
        public List<Ent_Consulta_Movimiento> ListarConsultaMovimiento(string fec, string dwtda)
        {
            List<Ent_Consulta_Movimiento> lista = datInv.get_Lista_Movimiento_Fecha(fec, dwtda);
            Session["Lista_Consulta_Movimiento"] = lista;
            return lista;
        }

        [HttpPost]
        public ActionResult GetUltimoMovimiento()
        {
            try
            {
                if (Session["Lista_Consulta_Movimiento"] == null)
                {
                    List<Ent_Consulta_Movimiento> liststoreConf = new List<Ent_Consulta_Movimiento>();
                    Session["Lista_Consulta_Movimiento"] = liststoreConf;
                }
                List<Ent_Consulta_Movimiento> lista = (List<Ent_Consulta_Movimiento>)Session["Lista_Consulta_Movimiento"];
                Ent_Consulta_Movimiento ultimo = (Ent_Consulta_Movimiento)lista.OrderByDescending(p => Convert.ToDateTime(p.FECHA)).First();


                if (ultimo != null)
                {
                    return Json(new { estado = 1, fecha = ultimo.FECHA, saldo_calzado = ultimo.SALDO_CALZADO, saldo_no_calzado = ultimo.SALDO_NO_CALZADO });
                }
                else
                {
                    return Json(new { estado = 0, resultados = "Sin Resultados" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { estado = 0, resultados = "Sin Resultados" });
            }
        }

        public ActionResult ListarConsultaMovimiento_DataTable(Ent_jQueryDataTableParams param)
        {
            if (Session["Lista_Consulta_Movimiento"] == null)
            {
                List<Ent_Consulta_Movimiento> liststoreConf = new List<Ent_Consulta_Movimiento>();
                Session["Lista_Consulta_Movimiento"] = liststoreConf;
            }
            IQueryable<Ent_Consulta_Movimiento> membercol = ((List<Ent_Consulta_Movimiento>)(Session["Lista_Consulta_Movimiento"])).AsQueryable();  //lista().AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<Ent_Consulta_Movimiento> filteredMembers = membercol;


            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.FECHA.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.FECHA.ToUpper().Contains(param.sSearch.ToUpper()));
            }

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Ent_Consulta_Movimiento, DateTime> orderingFunction =
            (
            m => Convert.ToDateTime(m.FECHA));
            filteredMembers = filteredMembers.OrderBy(orderingFunction);
            //var sortDirection = Request["sSortDir_0"];
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
                             a.TIENDA,
                             a.FECHA,
                             a.INI_CALZADO,
                             a.INI_NO_CALZADO,
                             a.VEN_CALZADO,
                             a.VEN_NO_CALZADO,
                             a.ING_CALZADO,
                             a.ING_NO_CALZADO,
                             a.SAL_CALZADO,
                             a.SAL_NO_CALZADO,
                             a.SALDO_CALZADO,
                             a.SALDO_NO_CALZADO
                         };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = membercol.Count(),
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public FileContentResult ConsultaMovimientoExcel()
        {
            if (Session["Lista_Consulta_Movimiento"] == null)
            {
                List<Ent_Consulta_Movimiento> liststoreConf = new List<Ent_Consulta_Movimiento>();
                Session["Lista_Consulta_Movimiento"] = liststoreConf;
            }
            List<Ent_Consulta_Movimiento> lista = (List<Ent_Consulta_Movimiento>)Session["Lista_Consulta_Movimiento"];
            string[] columns = { "FECHA", "INI_CALZADO", "INI_NO_CALZADO", "VEN_CALZADO", "VEN_NO_CALZADO", "ING_CALZADO", "ING_NO_CALZADO", "SAL_CALZADO", "SAL_NO_CALZADO", "SALDO_CALZADO", "SALDO_NO_CALZADO" };
            string[] headers = { "FECHA", "INICIAL", "VENTA", "INGRESO", "SALIDA", "SALDO" };
            byte[] filecontent = ExcelExportHelper.ExportExcel2(headers, lista, "Inventario: Consulta de Movimientos por Fecha" + Environment.NewLine + "Tienda: " + lista[0].TIENDA, false, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "Inventario_Consulta_Movimientos_Fecha.xlsx");
        }
        #endregion
        #region AJUSTE DE INVENTARIO

        public ActionResult AjusteInventario()
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
                ViewBag.tienda = dat_lista_tienda.get_tienda("PE", "1");
                Session[_session_lista_articulos_inv] = null;
                return View();
            }
        }

        public ActionResult getInvArtAjax(Ent_jQueryDataTableParams param, bool corteInventario, string tienda, string fecha)//, string _art_mod, bool ordenar, bool _all_check, bool _all_check_val)
        {
            /*verificar si esta null*/
            if (corteInventario)
            {
                List<Ent_Inv_Ajuste_Articulos> list = datInv.getListaTeorico(tienda, Convert.ToDateTime(fecha));
                Session[_session_lista_articulos_inv] = list;
            }
            if (Session[_session_lista_articulos_inv] == null)
            {
                List<Ent_Inv_Ajuste_Articulos> list = new List<Ent_Inv_Ajuste_Articulos>();
                Session[_session_lista_articulos_inv] = list;
            }

            IQueryable<Ent_Inv_Ajuste_Articulos> membercol = ((List<Ent_Inv_Ajuste_Articulos>)Session[_session_lista_articulos_inv]).AsQueryable();  //lista().AsQueryable();

            //displayMembers.Select(a => { a.ESTADO_CONEXION_CAJA_XST = dat_storeTda.PingHost(a.IP); return a; }).ToList();
            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Inv_Ajuste_Articulos> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.ARTICULO.ToUpper().Contains(param.sSearch.ToUpper()) || m.MEDIDA.ToUpper().Contains(param.sSearch.ToUpper()));
            }
            //Manejador de orden

            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Ent_Inv_Ajuste_Articulos, string> orderingFunction =
            (
            m => sortIdx == 0 ? m.ARTICULO : m.CALIDAD.ToString());

            Func<Ent_Inv_Ajuste_Articulos, string> orderingCodigo = (m => m.ARTICULO);
            Func<Ent_Inv_Ajuste_Articulos, string> orderingCalidad = (m => m.CALIDAD);
            Func<Ent_Inv_Ajuste_Articulos, string> orderingMedida = (m => m.MEDIDA);
            Func<Ent_Inv_Ajuste_Articulos, decimal?> orderingFisico = (m => m.STOCK);
            Func<Ent_Inv_Ajuste_Articulos, decimal?> orderingTeorico = (m => m.TEORICO);
            Func<Ent_Inv_Ajuste_Articulos, decimal?> orderingDiferencia = (m => m.DIFERENCIA);

            var sortDirection = Request["sSortDir_0"];

            if (sortDirection == "asc")
            {
                if (sortIdx == 0) filteredMembers = filteredMembers.OrderBy(orderingCodigo);
                else if (sortIdx == 1) filteredMembers = filteredMembers.OrderBy(orderingCalidad);
                else if (sortIdx == 2) filteredMembers = filteredMembers.OrderBy(orderingMedida);
                else if (sortIdx == 3) filteredMembers = filteredMembers.OrderBy(orderingFisico);
                else if (sortIdx == 4) filteredMembers = filteredMembers.OrderBy(orderingTeorico);
                else if (sortIdx == 5) filteredMembers = filteredMembers.OrderBy(orderingDiferencia);
            }
            else
            {
                if (sortIdx == 0) filteredMembers = filteredMembers.OrderByDescending(orderingCodigo);
                else if (sortIdx == 1) filteredMembers = filteredMembers.OrderByDescending(orderingCalidad);
                else if (sortIdx == 2) filteredMembers = filteredMembers.OrderByDescending(orderingMedida);
                else if (sortIdx == 3) filteredMembers = filteredMembers.OrderByDescending(orderingFisico);
                else if (sortIdx == 4) filteredMembers = filteredMembers.OrderByDescending(orderingTeorico);
                else if (sortIdx == 5) filteredMembers = filteredMembers.OrderByDescending(orderingDiferencia);
            }

            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.ARTICULO,
                             a.CALIDAD,
                             a.MEDIDA,
                             a.TEORICO,
                             a.STOCK,
                             a.DIFERENCIA
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

        public ActionResult JsonExcelArticulos(string articulos)
        {
            List<Ent_Inv_Ajuste_Articulos> listArtExcel = null;
            try
            {
                listArtExcel = new List<Ent_Inv_Ajuste_Articulos>();
                listArtExcel = JsonConvert.DeserializeObject<List<Ent_Inv_Ajuste_Articulos>>(articulos.ToUpper());
                if (listArtExcel.Where(w => String.IsNullOrEmpty(w.ARTICULO) || String.IsNullOrEmpty(w.CALIDAD) || String.IsNullOrEmpty(w.MEDIDA) || String.IsNullOrEmpty(w.STOCK.ToString())).ToList().Count > 0)
                {
                    return Json(new { estado = 0, resultados = "El Archivo no tiene el formato correcto ó hay campos vacios.\nVerifique el archivo." });
                }
                else
                {
                    listArtExcel = listArtExcel.GroupBy(d => new { d.ARTICULO, d.CALIDAD, d.MEDIDA })
                    .Select(g => new Ent_Inv_Ajuste_Articulos() { STOCK = g.Sum(s => s.STOCK), ARTICULO = g.First().ARTICULO, CALIDAD = g.First().CALIDAD, MEDIDA = g.First().MEDIDA })
                    .ToList();
                    string msg_validar = datInv.validarExcel(listArtExcel);

                    if (msg_validar == "")
                    {
                        Session[_session_lista_articulos_inv] = unirListas(listArtExcel);
                        return Json(new { estado = 1, resultados = "ok" });
                    }
                    else
                    {
                        return Json(new { estado = 0, resultados = msg_validar });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { estado = 0, resultados = ex.Message });
            }
        }

        private List<Ent_Inv_Ajuste_Articulos> unirListas(List<Ent_Inv_Ajuste_Articulos> listArtExcel)
        {
            List<Ent_Inv_Ajuste_Articulos> oldList = (List<Ent_Inv_Ajuste_Articulos>)Session[_session_lista_articulos_inv];


            //decimal valor = 0;
            foreach (var item in listArtExcel.GroupBy(t => new { t.ARTICULO, t.CALIDAD, t.MEDIDA }).Select(g => new { ARTICULO = g.Key.ARTICULO, MEDIDA = g.Key.MEDIDA, CALIDAD = g.Key.CALIDAD, STOCK = g.Sum(X => X.STOCK) }))
            {
                //valor += Convert.ToDecimal(item.STOCK);
                oldList.Where(o => o.ARTICULO == item.ARTICULO && o.MEDIDA == item.MEDIDA && o.CALIDAD == item.CALIDAD).Select(a => { a.STOCK = item.STOCK; a.DIFERENCIA = item.STOCK - a.TEORICO; return a; }).ToList();
            }
            //decimal valor2 = oldList.Sum(t => t.STOCK).Value;

            List<Ent_Inv_Ajuste_Articulos> newExcelList = new List<Ent_Inv_Ajuste_Articulos>();

            foreach (var ritem in oldList)
            {
                listArtExcel.Remove(listArtExcel.Where(o => o.ARTICULO == ritem.ARTICULO && o.MEDIDA == ritem.MEDIDA && o.CALIDAD == ritem.CALIDAD).FirstOrDefault());
            }


            return oldList.Union(listArtExcel).ToList();
        }

        public ActionResult XSTORE_INSERTAR_INVENTARIO(string cod_tda, string inv_des, string inv_fec_inv)
        {
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            List<Ent_Inv_Ajuste_Articulos> listArticulos = null;
            string _error = "";
            string _mensaje = "";
            int result = 0;
            decimal tot_teorico = 0;
            decimal tot_fisico = 0;
            decimal tot_actual = 0;
            if (Session[_session_lista_articulos_inv] != null)
            {
                listArticulos = new List<Ent_Inv_Ajuste_Articulos>();
                listArticulos = (List<Ent_Inv_Ajuste_Articulos>)Session[_session_lista_articulos_inv];
            }
            if (listArticulos == null || (listArticulos != null && listArticulos.Count == 0))
            {
                _error += "La lista de articulos está vacia" + Environment.NewLine;
            }
            if (String.IsNullOrEmpty(cod_tda))
            {
                _error += "Seleccione tienda(s)." + Environment.NewLine;
            }
            if (String.IsNullOrEmpty(inv_des.Trim()))
            {
                _error += "Ingrese una descripcion." + Environment.NewLine;
            } //validar fecha
            if (String.IsNullOrEmpty(inv_fec_inv.Trim()))
            {
                _error += "Ingrese una Fecha." + Environment.NewLine;
            }
            if (_error != "")
            {
                return Json(new { estado = 0, resultado = "Error", mensaje = _error });
            }
            else
            {
                result = datInv.XSTORE_INSERTAR_INVENTARIO(cod_tda, inv_des, Convert.ToDateTime(inv_fec_inv), _usuario.usu_id, listArticulos, ref tot_teorico, ref tot_fisico, ref tot_actual, ref _mensaje);
                if (result == 1)
                {
                    return Json(new { estado = 1, resultado = "", mensaje = "Operacion realizada con éxito.", tot_teorico = tot_teorico, tot_fisico = tot_fisico, tot_actual = tot_actual });
                }
                else
                {
                    return Json(new { estado = 0, resultado = "Error", mensaje = _mensaje });
                }
            }
        }
        public ActionResult getResumen()
        {
            try
            {
                if (Session[_session_lista_articulos_inv] == null)
                {
                    List<Ent_Inv_Ajuste_Articulos> liststoreConf = new List<Ent_Inv_Ajuste_Articulos>();
                    Session[_session_lista_articulos_inv] = liststoreConf;
                }
                List<Ent_Inv_Ajuste_Articulos> lista = (List<Ent_Inv_Ajuste_Articulos>)Session[_session_lista_articulos_inv];
                var tot_teorico = lista.Sum(s => s.TEORICO);
                var tot_fisico = lista.Sum(s => s.STOCK);
                var tot_diferencia = lista.Sum(s => s.DIFERENCIA);

                return Json(new { estado = 1, tot_teorico = tot_teorico, tot_fisico = tot_fisico, tot_diferencia = tot_diferencia });
            }
            catch (Exception ex)
            {
                return Json(new { estado = 0, resultados = "Sin Resultados " + ex.Message });
            }
        }

        public ActionResult ListaAjustes()
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
                List<Ent_ListaTienda> tiendas = new List<Ent_ListaTienda>();
                tiendas.Add(new Ent_ListaTienda() { cod_entid = "-1", des_entid = "TODOS" });
                ViewBag.tienda = tiendas.Concat(dat_lista_tienda.get_tienda(Session["PAIS"].ToString(), "1"));
                Session[_session_lista_ajuste_inv] = null;
                return View();
            }
        }
        public PartialViewResult ListaAjustesInv(string tienda)
        {
            List<Ent_Inventario_Ajuste> lista = datInv.getListaAjustesInv(tienda);
            Session[_session_lista_ajuste_inv] = lista;
            return PartialView();
        }
        public ActionResult getListaAjustesInvAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_lista_ajuste_inv] == null)
            {
                List<Ent_Inventario_Ajuste> list = new List<Ent_Inventario_Ajuste>();
                Session[_session_lista_ajuste_inv] = list;
            }

            IQueryable<Ent_Inventario_Ajuste> membercol = ((List<Ent_Inventario_Ajuste>)Session[_session_lista_ajuste_inv]).AsQueryable();  //lista().AsQueryable();

            int totalCount = membercol.Count();

            IEnumerable<Ent_Inventario_Ajuste> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.TIENDA.ToUpper().Contains(param.sSearch.ToUpper())
                    || m.DESCRIPCION.ToUpper().Contains(param.sSearch.ToUpper())
                    || m.FECHA_INV.ToUpper().Contains(param.sSearch.ToUpper())
                    || m.CODIGO.ToString().Contains(param.sSearch.ToUpper()));
            }
            //Manejador de orden

            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            Func<Ent_Inventario_Ajuste, decimal> orderingCodigo = (m => m.CODIGO);
            Func<Ent_Inventario_Ajuste, string> orderingTienda = (m => m.TIENDA);
            Func<Ent_Inventario_Ajuste, string> orderingDescripcion = (m => m.DESCRIPCION);
            Func<Ent_Inventario_Ajuste, DateTime> orderingFecha = (m => Convert.ToDateTime(m.FECHA_INV));
            Func<Ent_Inventario_Ajuste, decimal> orderingFisico = (m => m.FISICO);
            Func<Ent_Inventario_Ajuste, decimal> orderingTeorico = (m => m.TEORICO);
            Func<Ent_Inventario_Ajuste, decimal> orderingDiferencia = (m => m.DIFERENCIA);

            var sortDirection = Request["sSortDir_0"];

            if (sortDirection == "asc")
            {
                if (sortIdx == 0) filteredMembers = filteredMembers.OrderBy(orderingCodigo);
                else if (sortIdx == 1) filteredMembers = filteredMembers.OrderBy(orderingTienda);
                else if (sortIdx == 2) filteredMembers = filteredMembers.OrderBy(orderingDescripcion);
                else if (sortIdx == 3) filteredMembers = filteredMembers.OrderBy(orderingFecha);
                else if (sortIdx == 4) filteredMembers = filteredMembers.OrderBy(orderingFisico);
                else if (sortIdx == 5) filteredMembers = filteredMembers.OrderBy(orderingTeorico);
                else if (sortIdx == 6) filteredMembers = filteredMembers.OrderBy(orderingDiferencia);
            }
            else
            {
                if (sortIdx == 0) filteredMembers = filteredMembers.OrderByDescending(orderingCodigo);
                else if (sortIdx == 1) filteredMembers = filteredMembers.OrderByDescending(orderingTienda);
                else if (sortIdx == 2) filteredMembers = filteredMembers.OrderByDescending(orderingDescripcion);
                else if (sortIdx == 3) filteredMembers = filteredMembers.OrderByDescending(orderingFecha);
                else if (sortIdx == 4) filteredMembers = filteredMembers.OrderByDescending(orderingFisico);
                else if (sortIdx == 5) filteredMembers = filteredMembers.OrderByDescending(orderingTeorico);
                else if (sortIdx == 6) filteredMembers = filteredMembers.OrderByDescending(orderingDiferencia);
            }

            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.CODIGO,
                             a.TIENDA,
                             a.DESCRIPCION,
                             a.FECHA_INV,
                             a.FISICO,
                             a.TEORICO,
                             a.DIFERENCIA
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
        public ActionResult getListaDetArt(string cod_ajus)
        {
            List<Ent_Inv_Ajuste_Articulos> listArts = datInv.get_list_arts_ajus_inv(cod_ajus);
            if (listArts == null)
            {
                listArts = new List<Ent_Inv_Ajuste_Articulos>();
                Session[_session_lista_articulos_inv] = listArts;
            }
            else
            {
                Session[_session_lista_articulos_inv] = listArts;
            }
            return Json(new { estado = 1, articulos = listArts });
        }
        public FileContentResult ListaArticulosExcel()
        {
            if (Session[_session_lista_articulos_inv] == null)
            {
                List<Ent_Inv_Ajuste_Articulos> liststoreConf = new List<Ent_Inv_Ajuste_Articulos>();
                Session[_session_lista_articulos_inv] = liststoreConf;
            }
            List<Ent_Inv_Ajuste_Articulos> lista = (List<Ent_Inv_Ajuste_Articulos>)Session[_session_lista_articulos_inv];
            string[] columns = { "ARTICULO", "CALIDAD", "MEDIDA", "STOCK", "TEORICO", "DIFERENCIA" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(lista, "", false, columns);
            string nom_excel = "Lista de Articulos";
            return File(filecontent, ExcelExportHelper.ExcelContentType, nom_excel + ".xlsx");
        }
        #endregion

        #region<CONSULTA DE DOCUMENTO POR FECHA>
        private Dat_MovDoc_Consulta con_doc = new Dat_MovDoc_Consulta();
        public ActionResult ConsultaDocMov()
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
                ViewBag.Tienda = datInv.get_ListaTienda();
                return View();
            }

        }

        public PartialViewResult _Lista_ConsultaDocMov(string dwtienda, string fecini, string fecfin, string numdoc)
        {
            List<Ent_MovDoc_Consulta> lista_cons_doc = con_doc.lista_movdoc("0", dwtienda, Convert.ToDateTime(fecini), Convert.ToDateTime(fecfin), numdoc);
            Session[_session_lista_consultadoc_mov] = lista_cons_doc;
            return PartialView(lista_cons_doc);
        }
        private string _session_lista_consultadoc_mov = "_session_lista_consultadoc_mov";
        private string _session_lista_consultadoc_mov_det = "_session_lista_consultadoc_mov_det";
        public ActionResult getconsultadocmov(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_lista_consultadoc_mov] == null)
            {
                List<Ent_MovDoc_Consulta> lisprom = new List<Ent_MovDoc_Consulta>();
                Session[_session_lista_consultadoc_mov] = lisprom;
            }

            //Traer registros
            IQueryable<Ent_MovDoc_Consulta> membercol = ((List<Ent_MovDoc_Consulta>)(Session[_session_lista_consultadoc_mov])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<Ent_MovDoc_Consulta> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m =>
                     m.tipo_doc.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.tipo_transac.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.num_doc.ToUpper().Contains(param.sSearch.ToUpper())
                     );
            }
            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            //Func<Models_Guia, string> orderTO = (m => m.TIENDA_ORI);
            //Func<Models_Guia, string> orderNumero = (m => m.NUMERO);
            //Func<Models_Guia, DateTime> orderFecha = (m => Convert.ToDateTime(m.FECHA));
            //Func<Models_Guia, int> orderPares = (m => Convert.ToInt32(m.PARES));
            //Func<Models_Guia, double> orderVC = (m => Convert.ToDouble(m.VCALZADO));
            //Func<Models_Guia, int> orderNC = (m => Convert.ToInt32(m.NOCALZADO));
            //Func<Models_Guia, double> orderVNC = (m => Convert.ToDouble(m.VNOCALZADO));
            //Func<Models_Guia, string> orderEstado = (m => m.ESTADO);

            var sortDirection = Request["sSortDir_0"];
            if (sortDirection == "asc")
            {
                switch (sortIdx)
                {

                    case 1: filteredMembers = filteredMembers.OrderBy(o => o.tipo_transac); break;
                    case 2: filteredMembers = filteredMembers.OrderBy(o => o.tipo_doc); break;
                    case 3: filteredMembers = filteredMembers.OrderBy(o => o.num_doc); break;
                    case 4: filteredMembers = filteredMembers.OrderBy(o => Convert.ToDateTime(o.fecha_doc)); break;
                    case 5: filteredMembers = filteredMembers.OrderBy(o => o.cant); break;
                    case 6: filteredMembers = filteredMembers.OrderBy(o => o.tda_ori); break;
                    case 7: filteredMembers = filteredMembers.OrderBy(o => o.tda_des); break;


                    default: break;
                }
            }
            else
            {
                switch (sortIdx)
                {
                    case 1: filteredMembers = filteredMembers.OrderByDescending(o => o.tipo_transac); break;
                    case 2: filteredMembers = filteredMembers.OrderByDescending(o => o.tipo_doc); break;
                    case 3: filteredMembers = filteredMembers.OrderByDescending(o => o.num_doc); break;
                    case 4: filteredMembers = filteredMembers.OrderByDescending(o => Convert.ToDateTime(o.fecha_doc)); break;
                    case 5: filteredMembers = filteredMembers.OrderByDescending(o => o.cant); break;
                    case 6: filteredMembers = filteredMembers.OrderByDescending(o => o.tda_ori); break;
                    case 7: filteredMembers = filteredMembers.OrderByDescending(o => o.tda_des); break;


                    default: break;
                }
            }
            var displayMembers = filteredMembers.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.mc_id,
                             a.tienda,
                             a.tipo_transac,
                             a.tipo_doc,
                             a.num_doc,
                             a.fecha_doc,
                             a.cant,
                             a.tda_des,
                             a.tda_ori,
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

        public PartialViewResult getListadocmov_det(string mc_id)
        {

            List<Ent_MovDoc_Consulta_Detalle> listdoc_det = con_doc.lista_movdoc_det(mc_id);

            /*agrupar articulo para las tallas*/
            List<Ent_MovDoc_Consulta_Detalle_Articulo> listdoc_det_art =
            (from grouping in listdoc_det.GroupBy(x => new Tuple<string, string, string, string>(x.articulo, x.calidad, x.linea, x.categoria))
             select new Ent_MovDoc_Consulta_Detalle_Articulo
             {
                 articulo = grouping.Key.Item1,
                 calidad = grouping.Key.Item2,
                 linea = grouping.Key.Item3,
                 categoria = grouping.Key.Item4,
                 total = listdoc_det.Where(a => a.articulo == grouping.Key.Item1 && a.calidad == grouping.Key.Item2).Sum(s => s.cantidad),
                 list_talla = (from det_talla in listdoc_det.Where(a => a.articulo == grouping.Key.Item1 && a.calidad == grouping.Key.Item2).Select(s => new { s.talla, s.cantidad })
                               select new Ent_MovDoc_Consulta_Detalle_Articulo_Talla()
                               {
                                   talla = det_talla.talla,
                                   cantidad = det_talla.cantidad
                               }).ToList()
             }).ToList();
            /*********************************************+*/
            if (listdoc_det_art == null)
            {
                listdoc_det_art = new List<Ent_MovDoc_Consulta_Detalle_Articulo>();
                //Session[_session_lista_prom_tipo_excel] = listprom_tipo;
                Session[_session_lista_consultadoc_mov_det] = listdoc_det_art;
            }
            else
            {
                //Session[_session_lista_prom_tipo_excel] = listprom_tipo;
                Session[_session_lista_consultadoc_mov_det] = listdoc_det_art;
            }

            return PartialView(listdoc_det);// View();
        }
        public ActionResult getTableconsdocDetAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_lista_consultadoc_mov_det] == null)
            {
                List<Ent_MovDoc_Consulta_Detalle_Articulo> listdoc_det = new List<Ent_MovDoc_Consulta_Detalle_Articulo>();
                Session[_session_lista_consultadoc_mov_det] = listdoc_det;
            }
            //if (!String.IsNullOrEmpty(dniEliminar))
            //{
            //    List<Ent_BataClub_Cupones> listAct = (List<Ent_BataClub_Cupones>)(Session[_session_lista_clientes_cupon]);
            //    listAct.Remove(listAct.Where(w => w.dniCliente == dniEliminar).FirstOrDefault());
            //    Session[_session_lista_clientes_cupon] = listAct;
            //}
            //Traer registros
            IQueryable<Ent_MovDoc_Consulta_Detalle_Articulo> membercol = ((List<Ent_MovDoc_Consulta_Detalle_Articulo>)(Session[_session_lista_consultadoc_mov_det])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_MovDoc_Consulta_Detalle_Articulo> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m =>
                    m.articulo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.linea.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.categoria.ToUpper().Contains(param.sSearch.ToUpper())
                    );
            }

            //Manejador de orden
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.articulo,
                             a.calidad,
                             a.linea,
                             a.categoria,
                             a.list_talla,
                             a.total,
                             //a.talla,
                             //a.cantidad,
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

        #region REPORTE DE INVENTARIO DE PLANILLA

        [HttpPost]
        public ActionResult ShowGenericReportInventarioPlanillaInNewWin(string tda, string fecIni)
        {

            try
            {

                string CodTda = "";
                var ip = new Dat_InventarioPlanilla();
                HttpContext.Session["ReportName"] = "ReporteInventarioPlanilla.rpt";

                //if (Session["Tienda"] != null)
                //{
                //    CodTda = Session["Tienda"].ToString();
                //}
                //else
                //{
                //    //CodTda = "-1";
                //    CodTda = tda;
                //}

                //ReporteVentasEcommerce ModeloRepVentaEcommerce = ec.get_ecommerce_reporteventa(CodTda, fecIni, FecFin, tipo);
                Models_InventarioPlanilla ModeloInventarioPlanilla = ip.get_InventarioPlanilla(tda, fecIni);

                HttpContext.Session["rptSource"] = ModeloInventarioPlanilla.ListInventarioPlanilla;

                var _estado = (ModeloInventarioPlanilla == null) ? "0" : "1";

                if (ModeloInventarioPlanilla != null)
                {
                    if (ModeloInventarioPlanilla.ListInventarioPlanilla.Count == 0)
                    {
                        _estado = "-1";
                        //ViewBag.Tienda = ec.get_ListaTienda();
                    }

                }

                return Json(new
                {
                    estado = _estado
                });

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                throw;
            }
        }

        public ActionResult ReporteInventarioPlanilla()
        {
            string tipo = (Request.HttpMethod == "POST" ? Request.Params["tipo"] : "1,2,3");
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            //Dat_ECommerce ec = new Dat_ECommerce();
            var ip = new Dat_InventarioPlanilla();

            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;
            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {

                ViewBag.Tienda = ip.get_ListaTienda("",0,Session["PAIS"].ToString());

                //ViewBag._selectTipos = SelectTipos((tipo == null ? " '',R,E" : tipo));

                //if (_usuario.usu_tip_id == "05") //INVITADO (TIENDAS)
                //{
                //    ViewBag.Tienda = ec.get_ListaTienda(_usuario.usu_login, 0);

                //}
                //else
                //{
                //    ViewBag.Tienda = ec.get_ListaTienda("", 1);
                //}

                //ViewBag.usu_tipo = _usuario.usu_tip_id;

            }
            return View();
        }


        /*reporte inventario movimiento*/

        public ActionResult ShowGenericReportInventarioMovimientoInNewWin(string tda, DateTime fecIni, DateTime fecFin)
        {

            try
            {

                string CodTda = "";
                var im = new Dat_Inventario_Movimiento();
                HttpContext.Session["ReportName"] = "ReporteInventarioMovimiento.rpt";

                //if (Session["Tienda"] != null)
                //{
                //    CodTda = Session["Tienda"].ToString();
                //}
                //else
                //{
                //    //CodTda = "-1";
                //    CodTda = tda;
                //}

                //ReporteVentasEcommerce ModeloRepVentaEcommerce = ec.get_ecommerce_reporteventa(CodTda, fecIni, FecFin, tipo);
                Models_InventarioMovimiento ModeloInventarioMovimiento = im.get_InventarioMovimiento(tda, fecIni, fecFin);

                HttpContext.Session["rptSource"] = ModeloInventarioMovimiento.ListInventarioMovimiento;

                var _estado = (ModeloInventarioMovimiento == null) ? "0" : "1";

                if (ModeloInventarioMovimiento != null)
                {
                    if (ModeloInventarioMovimiento.ListInventarioMovimiento.Count == 0)
                    {
                        _estado = "-1";
                        //ViewBag.Tienda = ec.get_ListaTienda();
                    }

                }

                return Json(new
                {
                    estado = _estado
                });

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                throw;
            }
        }

        public ActionResult ReporteInventarioMovimiento()
        {
            string tipo = (Request.HttpMethod == "POST" ? Request.Params["tipo"] : "1,2,3");
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            //Dat_ECommerce ec = new Dat_ECommerce();
            var ip = new Dat_InventarioPlanilla();

            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;
            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {

                ViewBag.Tienda = ip.get_ListaTienda("",0,Session["PAIS"].ToString());

                //ViewBag._selectTipos = SelectTipos((tipo == null ? " '',R,E" : tipo));

                //if (_usuario.usu_tip_id == "05") //INVITADO (TIENDAS)
                //{
                //    ViewBag.Tienda = ec.get_ListaTienda(_usuario.usu_login, 0);

                //}
                //else
                //{
                //    ViewBag.Tienda = ec.get_ListaTienda("", 1);
                //}

                //ViewBag.usu_tipo = _usuario.usu_tip_id;

            }
            return View();
        }

        #endregion

    }
}