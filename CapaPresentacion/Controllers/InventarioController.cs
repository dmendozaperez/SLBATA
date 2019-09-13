using CapaDato.Inventario;
using CapaDato.Maestros;
using CapaEntidad.Control;
using CapaEntidad.General;
using CapaEntidad.Inventario;
using CapaEntidad.Util;
using CapaPresentacion.Bll;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class InventarioController : Controller
    {
        private Dat_Inventario_Consulta datInv = new Dat_Inventario_Consulta(); //gft
        private Dat_ListaTienda dat_lista_tienda = new Dat_ListaTienda(); 
        private string _session_tabla_inventCons_private = "_session_tabla_inventCons_private"; //gft
        private string _Inventario_Tienda_Combo = "_Inventario_Tienda_Combo"; 
        private string _Inventario_TiendaFecha_Combo = "_Inventario_TiendaFecha_Combo";
        private string _session_lista_articulos = "_session_lista_articulos";

        // GET: Inventario

        #region Consulta inventario
        //Index
        public ActionResult Consulta_Inv( )
        {
            string cod_entid="";

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
            { ViewBag.Tienda = Session["_Inventario_Tienda_Combo"];
               // cod_entid = ViewBag.Tienda[0].cod_entid.ToString();
              //  ViewBag.Fecha = datInv.get_ListaFecha(cod_entid);
                ViewBag.Fecha = Session["_Inventario_TiendaFecha_Combo"];
            }
            return View();
        }
       
        //Table
        public PartialViewResult _TableConsInv( string articulo, string talla, string dwtda, string dwfec)
        {
            //xst_inv_fec_aud
            if (dwtda == null|| dwfec == null)
            { return PartialView(); }
            else
            {  return PartialView(listaTablaConsulta(articulo, talla, dwtda, dwfec)); }
        }

        public List<Ent_Inventario_Consulta> listaTablaConsulta(string articulo, string talla, string dwtda, string dwfec)
        {
            List<Ent_Inventario_Consulta> list = datInv.get_ListaInv_Consulta( dwtda, dwfec, articulo, talla);
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
            string[] columns = { "des_entid", "articulo", "calidad", "talla", "teorico", "fisico", "diferencia", "nombres_venta", "fecha_inv"};
            byte[] filecontent = ExcelExportHelper.ExportExcel(list, "Inventario_Consulta", true, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "Inventario_Consulta.xlsx");
        }

    #endregion

        #region ****Movimientos por Fecha*****

        public ActionResult ConsultaMovimiento()
        {
            ViewBag.Tienda = dat_lista_tienda.get_tienda("PE", "1");
            Session["Lista_Consulta_Movimiento"] = null;
            return View();
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
                Ent_Consulta_Movimiento ultimo = (Ent_Consulta_Movimiento)lista.OrderByDescending(p => Convert.ToDateTime( p.FECHA)).First();


                if (ultimo != null)
                {                    
                    return Json(new { estado = 1, fecha = ultimo.FECHA, saldo_calzado = ultimo.SALDO_CALZADO, saldo_no_calzado = ultimo.SALDO_NO_CALZADO});
                }
                else
                {
                    return Json(new { estado = 0, resultados = "Sin Resultados" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { estado = 0,  resultados = "Sin Resultados" });
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
            string[] headers = { "FECHA", "INICIAL", "VENTA", "INGRESO", "SALIDA", "SALDO"};
            byte[] filecontent = ExcelExportHelper.ExportExcel2(headers, lista, "Inventario: Consulta de Movimientos por Fecha" + Environment.NewLine + "Tienda: " + lista[0].TIENDA ,false,columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "Inventario_Consulta_Movimientos_Fecha.xlsx");
        }
        #endregion
        #region AJUSTE DE INVENTARIO
        
        public ActionResult AjusteInventario()
        {
            ViewBag.tienda = dat_lista_tienda.get_tienda("PE", "1");
            Session[_session_lista_articulos] = null;
            return View();
        }

        public ActionResult getInvArtAjax(Ent_jQueryDataTableParams param,bool corteInventario, string tienda , DateTime fecha)//, string _art_mod, bool ordenar, bool _all_check, bool _all_check_val)
        {
            /*verificar si esta null*/
            if (corteInventario)
            {
                //List<Ent_Inv_Ajuste_Articulos> list = new List<Ent_Inv_Ajuste_Articulos>();
                //list.Add(new Ent_Inv_Ajuste_Articulos() { ARTICULO = "0034869", CALIDAD = "1", MEDIDA = "04", TEORICO = 3, STOCK = 3, DIFERENCIA = 0});
                //list.Add(new Ent_Inv_Ajuste_Articulos() { ARTICULO = "0034869", CALIDAD = "1", MEDIDA = "05", TEORICO = 5, STOCK = 3, DIFERENCIA = 0 });
                //list.Add(new Ent_Inv_Ajuste_Articulos() { ARTICULO = "0043824", CALIDAD = "1", MEDIDA = "03", TEORICO = 4, STOCK = 3, DIFERENCIA = 0 });
                //list.Add(new Ent_Inv_Ajuste_Articulos() { ARTICULO = "0049910", CALIDAD = "1", MEDIDA = "05", TEORICO = 5, STOCK = 3, DIFERENCIA = 0 });
                //list.Add(new Ent_Inv_Ajuste_Articulos() { ARTICULO = "0064806", CALIDAD = "1", MEDIDA = "02", TEORICO = 7, STOCK = 3, DIFERENCIA = 0 });
                //list.Select(a => { a.DIFERENCIA =  a.STOCK - a.TEORICO; return a; }).ToList();
                //Session[_session_lista_articulos] = list;
                List<Ent_Inv_Ajuste_Articulos> list = datInv.getListaTeorico(tienda,fecha);
                Session[_session_lista_articulos] = list;
            }
            if (Session[_session_lista_articulos] == null)
            {
                List<Ent_Inv_Ajuste_Articulos> list = new List<Ent_Inv_Ajuste_Articulos>();
                Session[_session_lista_articulos] = list;
            }
            
            IQueryable<Ent_Inv_Ajuste_Articulos> membercol =((List<Ent_Inv_Ajuste_Articulos>)Session[_session_lista_articulos]).AsQueryable();  //lista().AsQueryable();

            //displayMembers.Select(a => { a.ESTADO_CONEXION_CAJA_XST = dat_storeTda.PingHost(a.IP); return a; }).ToList();
            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Inv_Ajuste_Articulos> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.ARTICULO.ToUpper().Contains(param.sSearch.ToUpper()));
            }
            //Manejador de orden

            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Ent_Inv_Ajuste_Articulos, string> orderingFunction =
            (
            m => sortIdx == 0 ? m.ARTICULO : m.CALIDAD.ToString());
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
                             a.ARTICULO,
                             a.CALIDAD  ,
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

                listArtExcel = listArtExcel.GroupBy(d => new { d.ARTICULO, d.CALIDAD, d.MEDIDA })
                    .Select(g => new Ent_Inv_Ajuste_Articulos() { STOCK = g.Sum(s => s.STOCK), ARTICULO = g.First().ARTICULO , CALIDAD = g.First().CALIDAD , MEDIDA = g.First().MEDIDA  })
                    .ToList();
                string msg_validar = datInv.validarExcel(listArtExcel);

                if (msg_validar == "")
                {
                    Session[_session_lista_articulos] = unirListas(listArtExcel);
                    return Json(new { estado = 1, resultados = "ok" });
                }
                else
                {
                    return Json(new { estado = 0, resultados = msg_validar });
                }
            }
            catch (Exception ex)
            {
                return Json(new { estado = 0, resultados = ex.Message });
            }
        }

        private List<Ent_Inv_Ajuste_Articulos> unirListas(List<Ent_Inv_Ajuste_Articulos> listArtExcel)
        {
            List<Ent_Inv_Ajuste_Articulos> oldList = (List<Ent_Inv_Ajuste_Articulos>)Session[_session_lista_articulos];
            foreach (var item in listArtExcel)
            {
                oldList.Where(o => o.ARTICULO == item.ARTICULO && o.MEDIDA == item.MEDIDA && o.CALIDAD == item.CALIDAD).Select(a => { a.STOCK = item.STOCK; a.DIFERENCIA = item.STOCK - a.TEORICO; return a; }).ToList();                              
            }

            List<Ent_Inv_Ajuste_Articulos> newExcelList = new List<Ent_Inv_Ajuste_Articulos>();

            foreach (var ritem in oldList )
            {
                listArtExcel.Remove(listArtExcel.Where(o => o.ARTICULO == ritem.ARTICULO && o.MEDIDA == ritem.MEDIDA && o.CALIDAD == ritem.CALIDAD).FirstOrDefault());
            }           

            return oldList.Union(newExcelList).ToList();
        }

        public ActionResult XSTORE_INSERTAR_INVENTARIO (string cod_tda, string inv_des, DateTime inv_fec_inv)
        {
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            List<Ent_Inv_Ajuste_Articulos> listArticulos = null;
            string _error = "";
            string _mensaje = "";
            int result = 0;
            decimal tot_teorico = 0;
            decimal tot_fisico = 0;
            decimal tot_actual = 0;
            if (Session[_session_lista_articulos] != null)
            {
                listArticulos = new List<Ent_Inv_Ajuste_Articulos>();
                listArticulos = (List<Ent_Inv_Ajuste_Articulos>)Session[_session_lista_articulos];
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
            if (_error != "")
            {
                return Json(new { estado = 0, resultado = "Error", mensaje = _error });
            }
            else
            {
                result = datInv.XSTORE_INSERTAR_INVENTARIO(cod_tda, inv_des, inv_fec_inv, _usuario.usu_id, listArticulos, ref tot_teorico, ref tot_fisico, ref tot_actual, ref _mensaje);
                if (result == 1)
                {
                    return Json(new { estado = 1, resultado = "", mensaje = "Operacion realizada con éxito." });
                }
                else
                {
                    return Json(new { estado = 0, resultado = "Error", mensaje = _mensaje });
                }
            }
        }

        #endregion
    }
}