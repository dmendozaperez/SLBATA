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
        public ActionResult NuevoCompartir()
        {
            ViewBag.tienda = compartir.lista_tienda();
            return View();
        }
        [HttpPost]
        public ActionResult NuevoCompartir(string dni, string correo, string tienda,Int32 envio)
        {

            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];

            Boolean _valida_nuevo = compartir.insert_edit_compartir(dni,correo,tienda, _usuario.usu_id, (envio==1)?true:false); //true;// funcion.InsertarFuncion();

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
    }
}