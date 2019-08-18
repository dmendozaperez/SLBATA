using CapaDato.BataClub;
using CapaDato.Maestros;
using CapaDato.Reporte;
using CapaEntidad.BataClub;
using CapaEntidad.Control;
using CapaEntidad.General;
using CapaEntidad.Util;
using CapaPresentacion.Bll;
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
        private Dat_Canal datCan = new Dat_Canal();
        private Dat_Ubigeo datUbi = new Dat_Ubigeo();
        private string _session_tabla_prom_private = "_session_tabla_prom_private"; 
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
            //    if (Session["Tienda"] != null)  
            //    {
            //        ViewBag.Tienda = tienda.get_ListaTiendaXstore().Where(t => t.cbo_codigo == Session["Tienda"].ToString()).ToList();
            //    }
            //    else
            //    {
            //        ViewBag.Tienda = tienda.get_ListaTiendaXstore(true);
            //    }

            //    return View();
            //}
            if (Session["_BataClub_Promociones_Combo"] == null)
            {
                ViewBag.Promocion = datProm.get_ListaPromociones();
                Session["_BataClub_Promociones_Combo"] = ViewBag.Promocion;
            }
            else
            { ViewBag.Promocion = Session["_BataClub_Promociones_Combo"]; }

            if (Session["_BataClub_Promociones_estado"] == null)
            {
                ViewBag.Estado = datProm.get_ListaEstados();
                Session["_BataClub_Promociones_estado"] = ViewBag.Estado;
            }
            else
            { ViewBag.Estado = Session["_BataClub_Promociones_estado"]; }

            if (Session["_BataClub_cupon_Combo"] == null)
            {
                ViewBag.Promo = datProm.get_ListaPromo_Disp();
                Session["_BataClub_cupon_Combo"] = ViewBag.Promo;
            }
            else {
                ViewBag.Promo = Session["_BataClub_cupon_Combo"];
            }

                return View();
        }

        //Table
        public PartialViewResult _Table(string dni, string cupon, string hidden, string correo,string dwprom, string dwest)
        {
            if (dwprom == null)
            { return PartialView(); }
            else
            { //string dwprom--> se reemplaza por hidden - para agarrar varios id de promociones con el combo multiselect
                return PartialView(listaTablaPromociones(dni, cupon, hidden, correo, dwest));
            }
        }

        public List<Ent_BataClub_CuponesCO> listaTablaPromociones(string dni, string cupon, string id_grupo, string correo,string dwest)
        {
            List<Ent_BataClub_CuponesCO> listguia = datProm.get_lista_prom(dni, cupon, id_grupo, correo, dwest);
            Session[_session_tabla_prom_private] = listguia;
            return listguia;
        }

        public ActionResult getTablePromoAjax(Ent_jQueryDataTableParams param, string lblConsumidos)
        {
            /*verificar si esta null*/
            if (Session[_session_tabla_prom_private] == null)
            {
                List<Ent_BataClub_CuponesCO> listdoc = new List<Ent_BataClub_CuponesCO>();
                Session[_session_tabla_prom_private] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_BataClub_CuponesCO> membercol = ((List<Ent_BataClub_CuponesCO>)(Session[_session_tabla_prom_private])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_BataClub_CuponesCO> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m =>
                    m.correo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.prom_des.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.est_des.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.cup_fecha_fin.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.cupon.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.dni.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.fec_doc.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    // m.id_grupo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.Nombres.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.nombres_venta.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.porc_desc.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.soles.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.tickets.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.tienda.ToUpper().Contains(param.sSearch.ToUpper()));
            }

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            Func<Ent_BataClub_CuponesCO, string> orderingFunction =
                   (
                      //m => sortIdx == 0 ? m.fec_doc :
                      // m.fec_doc
                      m => sortIdx == 0 ? m.prom_des :
                    sortIdx == 1 ? m.est_des :
                    sortIdx == 2 ? m.cup_fecha_fin :
                    sortIdx == 3 ? m.Nombres :
                    m.tienda
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
                             a.prom_des,
                             a.est_des,
                             a.cup_fecha_fin,
                             a.Nombres,
                            // a.Apellidos,
                             a.dni,
                             a.correo,
                             a.cupon,
                             a.tienda,
                            // a.dni_venta,
                             a.nombres_venta,
                            // a.correo_venta,
                           //  a.telefono_venta,
                             a.tickets,
                             a.soles,
                             //a.grupo,
                             a.porc_desc,
                             a.fec_doc
                         };

            var numvariable1 = filteredMembers.Count(n => n.est_des == "CONSUMIDO");
            var numvariable2 = filteredMembers.Count(n => n.est_des == "DISPONIBLE");
            var numvariable3 = filteredMembers.Count(n => n.est_des == "CADUCADO");
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
                variable1=param.variable1,
                variable2 = param.variable2,
                variable3 = param.variable3
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
            List<Ent_BataClub_CuponesCO> listbataclub = (List<Ent_BataClub_CuponesCO>)Session[_session_tabla_prom_private];
            string[] columns = { "Nombres", "Apellidos", "dni", "correo", "cupon", "tienda", "dni_venta", "nombres_venta", "correo_venta", "telefono_venta", "tickets", "soles"
            , "grupo", "porc_desc", "fec_doc"};
            byte[] filecontent = ExcelExportHelper.ExportExcel(listbataclub, "BATACLUB_Promociones", true, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "BATACLUB_Promociones.xlsx");
        }
        #endregion

        #region Bataclub/Cupon
        //Index
        [HttpGet]
        public ActionResult Cupon(string dwpromo)
        {
            string cod_prom = "";

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
            //    if (Session["Tienda"] != null)  
            //    {
            //        ViewBag.Tienda = tienda.get_ListaTiendaXstore().Where(t => t.cbo_codigo == Session["Tienda"].ToString()).ToList();
            //    }
            //    else
            //    {
            //        ViewBag.Tienda = tienda.get_ListaTiendaXstore(true);
            //    }

            //    return View();
            //}

            List<Ent_BataClub_CuponesCO> list = null;

            if (Session["_BataClub_Promociones_Combo"] == null)
            {
                ViewBag.Promocion = datProm.get_ListaPromociones();
                Session["_BataClub_Promociones_Combo"] = ViewBag.Promocion;
            }
            else
            { ViewBag.Promocion = Session["_BataClub_Promociones_Combo"]; }


            if (Session["_BataClub_cupon_Combo"] == null)
            {
                ViewBag.PromoPop = datProm.get_ListaPromo_Disp();
                Session["_BataClub_cupon_Combo"] = ViewBag.PromoPop;
                cod_prom = ViewBag.PromoPop[0].prom_id.ToString();
                list =datProm.getPromDet(cod_prom);
                ViewBag.Descuento = list[0].porc_desc.ToString();
                ViewBag.FechaFin = list[0].cup_fecha_fin.ToString();
                ViewBag.Pares = list[0].max_pares.ToString();
                Session["_BataClub_Cupon_Desc"] = ViewBag.Descuento;
                Session["_BataClub_Cupon_FechaFin"] = ViewBag.FechaFin;
                Session["_BataClub_Cupon_Pares"] = ViewBag.Pares;
            }
            else
            {
                list = datProm.getPromDet(dwpromo);            
                ViewBag.PromoPop = ((List<Ent_BataClub_ComboProm>)(Session[_BataClub_cupon_Combo])).Where(t => t.prom_id == dwpromo);
                ViewBag.Descuento = list[0].porc_desc.ToString();
                ViewBag.FechaFin = list[0].cup_fecha_fin.ToString();
                ViewBag.Pares = list[0].max_pares.ToString();
            }
            return PartialView();
        }

        //Table cupón 
        public ActionResult getTableCuponAjax(Ent_jQueryDataTableParams param /*, int nListado*/)
        {
            /*verificar si esta null*/
            if (Session[_session_tabla_cupon_private] == null)
            {
                List<Ent_BataClub_CuponesCO> listdoc = new List<Ent_BataClub_CuponesCO>();
                Session[_session_tabla_cupon_private] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_BataClub_CuponesCO> membercol = ((List<Ent_BataClub_CuponesCO>)(Session[_session_tabla_cupon_private])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_BataClub_CuponesCO> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m =>
                    m.correo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.dni.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.Nombres.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.Apellidos.ToUpper().Contains(param.sSearch.ToUpper()));
            }

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            Func<Ent_BataClub_CuponesCO, string> orderingFunction =
                   (
                      m => sortIdx == 0 ? m.Nombres :
                    m.Apellidos
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
                             a.Nombres,
                             a.Apellidos,
                             a.dni,
                             a.correo
                         };

            //var numvariable1 = filteredMembers.Count(n => n.est_des == "CONSUMIDO");
            //var numvariable2 = filteredMembers.Count(n => n.est_des == "DISPONIBLE");
            //var numvariable3 = filteredMembers.Count(n => n.est_des == "CADUCADO");
            //// param.variable1 = lblConsumidos;
            //param.variable1 = numvariable1.ToString();
            //param.variable2 = numvariable2.ToString();
            //param.variable3 = numvariable3.ToString();

            //Se devuelven los resultados por json
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = result
                //variable1 = param.variable1,
                //variable2 = param.variable2,
                //variable3 = param.variable3
            }, JsonRequestBehavior.AllowGet);
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
        public PartialViewResult _TableCupon(string identificacion)
        {
            //if (/*Session[_session_tabla_cupon_private] != null || */((List<Ent_BataClub_CuponesCO>)(Session[_session_tabla_cupon_private])).Count > 0)              
            //{
            //    var list_orig = (List<Ent_BataClub_CuponesCO>)Session["_session_tabla_cupon_private"];
            //    if (identificacion == "" | identificacion == null)
            //    {
            //        //list_orig = (List<Ent_BataClub_CuponesCO>)Session["_session_tabla_cupon_private"];
            //        return PartialView(list_orig);
            //    }
            //    else {
            //        // list_orig = (List<Ent_BataClub_CuponesCO>)Session["_session_tabla_cupon_private"];
            //        IQueryable<Ent_BataClub_CuponesCO> membercol = ((List<Ent_BataClub_CuponesCO>)(Session[_session_tabla_cupon_private])).AsQueryable();
            //        IEnumerable<Ent_BataClub_CuponesCO> filteredMembers = membercol;
            //        filteredMembers = membercol
            //            .Where(m =>
            //           m.dni.ToUpper().Contains(identificacion));
            //        //  filteredMembers.ToList();
            //        //  var contador = filteredMembers.Count(n => n.dni == identificacion);
            //        var contador = filteredMembers.Count();
            //        if (contador > 0)
            //        {  return PartialView(); }
            //        else
            //        {  return PartialView(listaTablaClientes(identificacion)); }
            //    }
            //}
            //else
            //{  return PartialView(listaTablaClientes(identificacion));  }

            //// return PartialView(listaTablaClientes(identificacion));


            var list_orig = (List<Ent_BataClub_CuponesCO>)Session["_session_tabla_cupon_private"];
            if (identificacion == "" | identificacion == null)
                { return PartialView(list_orig); }
                else
                {
                    if (((List<Ent_BataClub_CuponesCO>)(Session[_session_tabla_cupon_private])).Count > 0)
                     {               
                        IQueryable<Ent_BataClub_CuponesCO> membercol = ((List<Ent_BataClub_CuponesCO>)(Session[_session_tabla_cupon_private])).AsQueryable();
                        IEnumerable<Ent_BataClub_CuponesCO> filteredMembers = membercol;
                        filteredMembers = membercol
                            .Where(m =>
                            m.dni.ToUpper().Contains(identificacion));
                        var contador = filteredMembers.Count();
                        if (contador > 0) { return PartialView(); } else { return PartialView(listaTablaClientes(identificacion)); }           
                    }
                    else
                    { return PartialView(listaTablaClientes(identificacion)); }
                }

        }

        public List<Ent_BataClub_CuponesCO> listaTablaClientes(string identificacion)
        {
            List<Ent_BataClub_CuponesCO> list_orig = null;
            List<Ent_BataClub_CuponesCO> list = datProm.get_cliente(identificacion);

            if (list.Count()==0) {
                list_orig = (List<Ent_BataClub_CuponesCO>)Session["_session_tabla_cupon_private"];
            }
            else {
                 list_orig = (List<Ent_BataClub_CuponesCO>)Session["_session_tabla_cupon_private"];
                list_orig.Add(list[0]);
                Session["_session_tabla_cupon_private"] = list_orig;
                //  lista_ag = list_orig;
            }
            return list_orig;

            /**/
            //List<Ent_BataClub_CuponesCO> lista_ag = null;
            //List<Ent_BataClub_CuponesCO> list = datProm.get_cliente(identificacion);

            //if (((List<Ent_BataClub_CuponesCO>)(Session[_session_tabla_cupon_private])).Count == 0/*Session[_session_tabla_cupon_private] == null*/)
            //{
            //    Session[_session_tabla_cupon_private] = list;
            //    lista_ag = list;
            //}
            //else
            //{
            //    var list_orig = (List<Ent_BataClub_CuponesCO>)Session["_session_tabla_cupon_private"];
            //    list_orig.Add(list[0]);
            //    lista_ag = list_orig;
            //}

            //return lista_ag;
            /**/

            //List<Ent_BataClub_CuponesCO> lista_ag=null;
            //if (identificacion ==""|| identificacion==null) {
            //    lista_ag = (List<Ent_BataClub_CuponesCO>)Session["_session_tabla_cupon_private"];

            //}
            //else
            //{
            //List<Ent_BataClub_CuponesCO> list = datProm.get_cliente(identificacion);
            ////Ent_BataClub_CuponesCO lista_agregada = new Ent_BataClub_CuponesCO();
            //// List<Ent_BataClub_CuponesCO> lista_ag;

            //if (Session[_session_tabla_cupon_private] == null)
            //{
            //    Session[_session_tabla_cupon_private] = list;
            //    lista_ag = list;
            //}
            //else
            //{
            //    var list_orig = (List<Ent_BataClub_CuponesCO>)Session["_session_tabla_cupon_private"];
            //    list_orig.Add(list[0]);
            //    lista_ag = list_orig;
            //}
            //  }

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

            //ViewBag.Departamento = datUbi.get_lista_Departamento();
            //ViewBag.Provincia = datUbi.get_lista_Provincia(ViewBag.Provincia);
            //ViewBag.Distrito = datUbi.get_lista_Distrito(ViewBag.Departamento,ViewBag.Provincia);

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