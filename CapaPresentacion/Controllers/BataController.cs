using CapaDato.Bata;
using CapaEntidad.Bata;
using CapaEntidad.Control;
using CapaEntidad.General;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class BataController : Controller
    {
        // GET: Bata
        private Dat_Cliente_Compartir  compartir = new Dat_Cliente_Compartir();
        private string _session_listcompartir_private = "_session_listcompartir_private";
        private string _session_listcompartir_private_cupones = "_session_listcompartir_private_cupones";
        public ActionResult ConsultaCompartir()
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
                return View(lista());
            }
        }
        public List<Ent_Cliente_Compartir> lista(string buscar="")
        {
            List<Ent_Cliente_Compartir> listcompartir = compartir.listar(buscar);
            Session[_session_listcompartir_private] = listcompartir;
            return listcompartir;
        }
        public ActionResult getListaCompartirAjax(Ent_jQueryDataTableParams param, string actualizar,string buscar)
        {

            List<Ent_Cliente_Compartir> listcompartir = new List<Ent_Cliente_Compartir>();

            if (!String.IsNullOrEmpty(actualizar))
            {
                listcompartir = lista(buscar);
                //listAtributos = datOE.get_lista_atributos();
                Session[_session_listcompartir_private] = listcompartir;
            }

            /*verificar si esta null*/
            if (Session[_session_listcompartir_private] == null)
            {
                listcompartir = new List<Ent_Cliente_Compartir>();
                listcompartir = lista(); //datOE.get_lista_atributos();
                if (listcompartir == null)
                {
                    listcompartir = new List<Ent_Cliente_Compartir>();
                }
                Session[_session_listcompartir_private] = listcompartir;
            }

            //Traer registros

            IQueryable<Ent_Cliente_Compartir> membercol = ((List<Ent_Cliente_Compartir>)(Session[_session_listcompartir_private])).AsQueryable();  //lista().AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<Ent_Cliente_Compartir> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.dni.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.correo.ToString().ToUpper().Contains(param.sSearch.ToUpper()) 
                     );
            }
            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"];
            if (param.iSortingCols > 0)
            {
                if (sortDirection == "asc")
                {
                    if (sortIdx == 0) filteredMembers = filteredMembers.OrderBy(o => o.dni);
                    else if (sortIdx == 1) filteredMembers = filteredMembers.OrderBy(o => o.correo);                    
                }
                else
                {
                    if (sortIdx == 0) filteredMembers = filteredMembers.OrderByDescending(o => o.dni);
                    else if (sortIdx == 1) filteredMembers = filteredMembers.OrderByDescending(o => o.correo);                    
                }
            }

         
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);
            var result = from a in displayMembers
                         select new
                         {
                             a.dni,
                             a.correo,
                             a.envio_email,
                             a.tienda,
                             a.fecha_ing,
                             a.fecha_env,
                             a.cod_tda,
                             a.num_doc                             
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
        public ActionResult NuevoCompartir()
        {
            ViewBag.tienda = compartir.lista_tienda();
            return View();
        }
        [HttpPost]
        public ActionResult NuevoCompartir(string dni, string correo, string tienda,Int32 envio,string numdoc)
        {

            if (numdoc == null) numdoc = "";

            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];

            Boolean _valida_nuevo = compartir.insert_edit_compartir(dni,correo,tienda, _usuario.usu_id, (envio==1)?true:false,numdoc); //true;// funcion.InsertarFuncion();

            return Json(new { estado = (_valida_nuevo) ? "1" : "-1", desmsg = (_valida_nuevo) ? "Se actualizo satisfactoriamente." : "Hubo un error al actualizar." });
        }
        public ActionResult valida_dni(string dni)
        {
            string mensaje = "";
            try
            {
                /*si trare valor 0 entonces no hay concidencias*/
                string valida =(compartir.existe_dni(dni))?"1":"0";
                switch (valida)
                {
                    case "1":
                        mensaje = "El Numero de documento existe, ingrese otro numero";
                        break;
                    case "0":
                        mensaje = "";
                        break;
                    
                }

                return Json(new { estado = valida, mensaje = mensaje });
            }
            catch (Exception ex)
            {
                return Json(new { estado = "3", mensaje = ex.Message });
            }


        }

        [HttpPost]
        public ActionResult getListaCupPromCompartir(string dni)
        {
            
                List<Ent_Barra_Compartir> listCups = compartir.listar_barra(dni);
                if (listCups == null)
                {
                    listCups = new List<Ent_Barra_Compartir>();                 
                    Session[_session_listcompartir_private_cupones] = listCups;
                }
                else
                {                    
                    Session[_session_listcompartir_private_cupones] = listCups;
                }
           
            return View();
        }
        public ActionResult getTableCuponCompartirAjax(Ent_jQueryDataTableParams param, string dniEliminar)
        {
            /*verificar si esta null*/
            if (Session[_session_listcompartir_private_cupones] == null)
            {
                List<Ent_Barra_Compartir> listdoc = new List<Ent_Barra_Compartir>();
                Session[_session_listcompartir_private_cupones] = listdoc;
            }           
            //Traer registros
            IQueryable<Ent_Barra_Compartir> membercol = ((List<Ent_Barra_Compartir>)(Session[_session_listcompartir_private_cupones])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Barra_Compartir> filteredMembers = membercol;

            //if (!string.IsNullOrEmpty(param.sSearch))
            //{
            //    filteredMembers = membercol
            //        .Where(m =>
            //        m.correo.ToUpper().Contains(param.sSearch.ToUpper()) ||
            //        m.nombresCliente.ToUpper().Contains(param.sSearch.ToUpper()) ||
            //        m.dniCliente.ToUpper().Contains(param.sSearch.ToUpper()) || m.apellidosCliente.ToUpper().Contains(param.sSearch.ToUpper()) ||
            //        (m.nombresCliente.Trim() + " " + m.apellidosCliente.Trim()).ToUpper().Contains(param.sSearch.ToUpper())
            //        );
            //}

            //Manejador de orden
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.barra,
                             a.estado,                             
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

        #region<REGION DE REPORTE SAC>
        public ActionResult BataSac()
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
                Session[_session_sac_cupones] = null;
                Session[_session_sac_venta] = null;
                return View();
            }

         
        }
        private string _session_sac_cupones = "_session_sac_cupones";
        private string _session_sac_venta = "_session_sac_venta";
        private string _session_sac_venta_detalle = "_session_sac_venta_detalle";
        public ActionResult get_batasac(string dni)
        {
            string mensaje = "";
            string estado = "0";
            try
            {
                Session[_session_sac_cupones] = null;
                Session[_session_sac_venta] = null;
                Session[_session_sac_venta_detalle] = null;

                Ent_Bata_Sac batasac =lista_batasac(dni);
                List<Ent_Bata_Sac_Cliente> sac_cliente = batasac.Bata_Sac_Cliente;
                List<Ent_Bata_Sac_Cupones> sac_cupones = batasac.Bata_Sac_Cupones;
                List<Ent_Bata_Sac_Venta> sac_venta = batasac.Bata_Sac_Venta;

                if (sac_cliente.Count==0)
                {
                    mensaje = "El Numero de dni no existe en nuestra base de datos";

                }
                else
                {
                    estado = "1";
                    Session[_session_sac_cupones] = sac_cupones;
                    Session[_session_sac_venta] = sac_venta;
                }                                
                return Json(new { estado = estado, mensaje = mensaje, sac_cliente= sac_cliente, sac_cupones= sac_cupones });
            }
            catch (Exception ex)
            {
                return Json(new { estado = "-1", mensaje = ex.Message });
            }

        }

        public ActionResult get_venta_detalle(string canal, string cod_tda, string fc_nint, string numdoc, string pedido)
        {
            Session[_session_sac_venta_detalle] = null;
            List<Ent_Bata_Sac_Venta_Detalle> listar_venta_det = lista_venta_detalle(canal,cod_tda,fc_nint,numdoc,pedido);

            Session[_session_sac_venta_detalle] = listar_venta_det;

            ViewBag.total = listar_venta_det.Sum(s=>s.total_linea);

            return View();
            //if (listar_venta_det.Count > 0)
            //{
            //    return View();// Json(new { estado = "1", mensaje = "" });
            //}
            //else
            //{
            //    return Json(new { estado = "0", mensaje = "No hay registros para mostrar" });
            //}

        }
        public Ent_Bata_Sac lista_batasac(string dni)
        {
            Dat_Bata_Sac data_sac = null;
            Ent_Bata_Sac lista_sac = null;
            try
            {

                data_sac = new Dat_Bata_Sac();
                lista_sac = data_sac.Get_Bata_Sac(dni);
            }
            catch (Exception)
            {

                lista_sac = new Ent_Bata_Sac();
            }
            return lista_sac;
        }

        public List<Ent_Bata_Sac_Venta_Detalle> lista_venta_detalle(string canal,string cod_tda,string fc_nint,string numdoc,string pedido)
        {            
            List<Ent_Bata_Sac_Venta_Detalle> listar = null;
            Dat_Bata_Sac data_sac = null;
            try
            {

                data_sac = new Dat_Bata_Sac();
                listar = data_sac.Get_Venta_detalle(canal,cod_tda,fc_nint,numdoc,pedido);
            }
            catch (Exception)
            {

                listar = new List<Ent_Bata_Sac_Venta_Detalle>();
            }
            return listar;
        }

        public ActionResult getTableBataSacVentaAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_sac_venta] == null)
            {
                List<Ent_Bata_Sac_Venta> listdoc = new List<Ent_Bata_Sac_Venta>();
                Session[_session_sac_venta] = listdoc;
            }
            //if (!String.IsNullOrEmpty(dniEliminar))
            //{
            //    List<Ent_BataClub_Cupones> listAct = (List<Ent_BataClub_Cupones>)(Session[_session_lista_clientes_cupon]);
            //    listAct.Remove(listAct.Where(w => w.dniCliente == dniEliminar).FirstOrDefault());
            //    Session[_session_lista_clientes_cupon] = listAct;
            //}
            //Traer registros
            IQueryable<Ent_Bata_Sac_Venta> membercol = ((List<Ent_Bata_Sac_Venta>)(Session[_session_sac_venta])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Bata_Sac_Venta> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m =>
                    m.tienda.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.numdoc.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.pedido.ToUpper().Contains(param.sSearch.ToUpper())
                    );
            }

            //Manejador de orden
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.canal,
                             a.tienda,
                             a.tipodoc,
                             a.numdoc,
                             a.fecha,
                             a.estado,
                             a.pedido,
                             a.fc_nint,
                             a.cod_tda
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

        public ActionResult getTableBataSacCuponAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_sac_cupones] == null)
            {
                List<Ent_Bata_Sac_Cupones> listdoc = new List<Ent_Bata_Sac_Cupones>();
                Session[_session_sac_cupones] = listdoc;
            }
            //if (!String.IsNullOrEmpty(dniEliminar))
            //{
            //    List<Ent_BataClub_Cupones> listAct = (List<Ent_BataClub_Cupones>)(Session[_session_lista_clientes_cupon]);
            //    listAct.Remove(listAct.Where(w => w.dniCliente == dniEliminar).FirstOrDefault());
            //    Session[_session_lista_clientes_cupon] = listAct;
            //}
            //Traer registros
            IQueryable<Ent_Bata_Sac_Cupones> membercol = ((List<Ent_Bata_Sac_Cupones>)(Session[_session_sac_cupones])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Bata_Sac_Cupones> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m =>
                    m.barra.ToUpper().Contains(param.sSearch.ToUpper()) 
                    );
            }

            //Manejador de orden
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.barra,
                             a.promocion,
                             a.fecha_expiracion,                             
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
        public ActionResult getTableBataSacVentaDetAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_sac_venta_detalle] == null)
            {
                List<Ent_Bata_Sac_Venta_Detalle> listdoc = new List<Ent_Bata_Sac_Venta_Detalle>();
                Session[_session_sac_venta_detalle] = listdoc;
            }
            //if (!String.IsNullOrEmpty(dniEliminar))
            //{
            //    List<Ent_BataClub_Cupones> listAct = (List<Ent_BataClub_Cupones>)(Session[_session_lista_clientes_cupon]);
            //    listAct.Remove(listAct.Where(w => w.dniCliente == dniEliminar).FirstOrDefault());
            //    Session[_session_lista_clientes_cupon] = listAct;
            //}
            //Traer registros
            IQueryable<Ent_Bata_Sac_Venta_Detalle> membercol = ((List<Ent_Bata_Sac_Venta_Detalle>)(Session[_session_sac_venta_detalle])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Bata_Sac_Venta_Detalle> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m =>
                    m.articulo.ToUpper().Contains(param.sSearch.ToUpper())
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
                             a.talla,
                             a.cantidad,
                             a.precio,
                             a.descuento,
                             a.total_linea,
                             a.promocion,
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