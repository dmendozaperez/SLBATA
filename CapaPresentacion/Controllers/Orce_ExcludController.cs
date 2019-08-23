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

namespace CapaPresentacion.Controllers
{
    public class Orce_ExcludController : Controller
    {
        // GET: Orce_Exclud
        Dat_OrceExclud datOE = new Dat_OrceExclud();
        string _session_lista_articulos = "_session_lista_articulos";
        string _session_atribuo_actual = "_session_atribuo_actual";
        string _session_lista_orce = "_session_lista_orce";
        public ActionResult Index()
        {
            Session[_session_lista_orce] = null;
            return View();

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
                Session[_session_lista_orce] = liststoreConf;
            }
            
            //Traer registros
            IQueryable<Ent_Orce_Inter_Cab> membercol = ((List<Ent_Orce_Inter_Cab>)(Session[_session_lista_orce])).AsQueryable();  //lista().AsQueryable();

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
            Func<Ent_Orce_Inter_Cab , int> orderingFunction =
                (
                    m => m.ORC_COD
                );
            Func<Ent_Orce_Inter_Cab, string> orderingFunction2 =
                (
                    m => m.ORC_FECHA_ACT
                );
            var sortDirection = Request["sSortDir_0"];
            if (sortDirection == "asc")
                filteredMembers = sortIdx == 0 ? filteredMembers.OrderBy(orderingFunction) : filteredMembers.OrderBy(orderingFunction2);
            else
                filteredMembers = sortIdx == 0 ? filteredMembers.OrderByDescending(orderingFunction) : filteredMembers.OrderByDescending(orderingFunction2);
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

        public ActionResult Nuevo()
        {
            Session[_session_lista_articulos] = null;
            ViewBag.listCadena = datOE.get_cadena();
            ViewBag.listAtr = datOE.get_atributos();

            List<Ent_Combo> list = new List<Ent_Combo>();
            ViewBag.listTdaCadena = list;

            Ent_Orce_Inter_Cab model = new Ent_Orce_Inter_Cab();
            model.TIENDAS = new List<Ent_Orce_Inter_Det_Tda>();
            return View(model);
        }
        public ActionResult Editar(int id)
        {
            Session[_session_lista_articulos] = null;
            ViewBag.listCadena = datOE.get_cadena();
            ViewBag.listAtr = datOE.get_atributos();

            List <Ent_Orce_Inter_Det_Tda> tdas = datOE.get_lista_det_tdas(id.ToString());
            List<Ent_Combo> tdas_cadena = datOE.get_tda_cadena(String.Join("," , tdas.Select(s => s.ORC_DET_TDA_CAD).Distinct()));

            Ent_Orce_Inter_Cab model = new Ent_Orce_Inter_Cab();
            model = ((List<Ent_Orce_Inter_Cab>)Session[_session_lista_orce]).Where(d => d.ORC_COD == id).First();
            model.TIENDAS = tdas;
            ViewBag.listTdaCadena = tdas_cadena;
            Session[_session_lista_articulos] = datOE.get_lista_det_art(model.ORC_COD.ToString());
            Session[_session_atribuo_actual] = model.ORC_ATRIBUTO;
            return View(model);
        }
        public ActionResult get_tda_cadena(string cadenas)
        {
            List<Ent_Combo> list = datOE.get_tda_cadena(cadenas);
            if (list == null)
            {
                list = new List<Ent_Combo>();
            }
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
            return (list);
        }

        public List<Ent_Orce_Inter_Art> unirListas(List<Ent_Orce_Inter_Art> lista)
        {
            List<Ent_Orce_Inter_Art> listArt_Actual = (List<Ent_Orce_Inter_Art>)Session[_session_lista_articulos];
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
                if (listArtExcel.Where(d => String.IsNullOrEmpty(d.ARTICULO) || String.IsNullOrEmpty(d.VALOR.ToString())).ToList().Count > 0)
                {
                    return Json(new { estado = 0, resultados = "El archivo no tiene el formato correcto. Los nombres de las columnas deben ser 'ARTICULO' Y 'VALOR' y no deben existir campos vacios." });
                }else
                {
                    Session[_session_lista_articulos] = unirListas(listArtExcel).Union(listArtExcel).ToList();
                    return Json(new { estado = 1, resultados = "ok" });
                }                
            }
            catch (Exception ex)
            {
                return Json(new { estado = 0, resultados = ex.Message });
            }
        }
        public ActionResult getArtAjax(Ent_jQueryDataTableParams param, string atributo , string _art_mod , bool ordenar)
        {
            /*verificar si esta null*/
            if (Session[_session_lista_articulos] == null || Session[_session_atribuo_actual].ToString() != atributo )
            {
                Session[_session_lista_articulos] = get_articulos_atr(atributo);
            }

            if (_art_mod != "")
            {
                List<Ent_Orce_Inter_Art> listArt = (List<Ent_Orce_Inter_Art>)Session[_session_lista_articulos];
                listArt.Where(w => w.ARTICULO == _art_mod).Select(a => { a.VALOR = !a.VALOR ; return a; }).ToList();
                Session[_session_lista_articulos] = listArt;
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
                
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                            select new
                            {
                                a.ARTICULO,
                                a.VALOR
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
    }
}
