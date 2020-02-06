using CapaDato.Orce;
using CapaEntidad.Control;
using CapaEntidad.General;
using CapaEntidad.Orce;
using CapaEntidad.Util;
using CapaPresentacion.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class OrceController : Controller
    {
        #region<CONSULTA DE PROMOCIONES ORCE>
        private Dat_Promotion_Orce promo = new Dat_Promotion_Orce();
        private string _session_lis_promo_orce_private = "_session_lis_promo_orce_private";
        // GET: Orce
        public ActionResult Promotion()
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

                Ent_Promotion_Orce_Lista param = new Ent_Promotion_Orce_Lista();

                param = promo.lista_tipo_param();

                ViewBag.estado = param.Promotion_Orce_Status;
                ViewBag.tipo = param.Promotion_Orce_Type;
                ViewBag.usuario = param.Promotion_Orce_User;
            }

            return View();
        }
        public PartialViewResult _Lista_Promotion(string[] dwestado, string[] dwtipo, string[] dwusuario, string fecini, string fecfin)
        {
            dwestado = dwestado == null ? new string[] { "" } : dwestado;
            dwtipo = dwtipo == null ? new string[] { "" } : dwtipo;
            dwusuario = dwusuario == null ? new string[] { "" } : dwusuario;

            List<Ent_Promotion_Orce> lista_prom = promo.lista_prom_orce(String.Join(",",dwestado), String.Join(",", dwtipo), Convert.ToDateTime(fecini),Convert.ToDateTime(fecfin), String.Join(",", dwusuario));

            Session[_session_lis_promo_orce_private] = lista_prom;

            return PartialView(lista_prom);
        }
        private string _session_lista_prom_tipo_excel = "_session_lista_prom_tipo_excel";
        private string _session_lista_prom_tipo = "_session_lista_prom_tipo";
        //[HttpPost]
        public PartialViewResult getListaPromOrceTipo(string promotion_id, string deal_id, string tipo)
        {
            
                List<Ent_Promotion_Orce_Atributos> listprom_tipo =promo.lista_prom_orce_atributos(promotion_id, deal_id, tipo);
                if (listprom_tipo == null)
                {
                    listprom_tipo = new List<Ent_Promotion_Orce_Atributos>();
                    Session[_session_lista_prom_tipo_excel] = listprom_tipo;
                    Session[_session_lista_prom_tipo] = listprom_tipo;
                }
                else
                {
                    Session[_session_lista_prom_tipo_excel] = listprom_tipo;
                    Session[_session_lista_prom_tipo] = listprom_tipo;
                }

            return PartialView(listprom_tipo);// View();
        }
        public ActionResult getTablePromTipoAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_lista_prom_tipo] == null)
            {
                List<Ent_Promotion_Orce_Atributos> listdoc = new List<Ent_Promotion_Orce_Atributos>();
                Session[_session_lista_prom_tipo] = listdoc;
            }
            //if (!String.IsNullOrEmpty(dniEliminar))
            //{
            //    List<Ent_BataClub_Cupones> listAct = (List<Ent_BataClub_Cupones>)(Session[_session_lista_clientes_cupon]);
            //    listAct.Remove(listAct.Where(w => w.dniCliente == dniEliminar).FirstOrDefault());
            //    Session[_session_lista_clientes_cupon] = listAct;
            //}
            //Traer registros
            IQueryable<Ent_Promotion_Orce_Atributos> membercol = ((List<Ent_Promotion_Orce_Atributos>)(Session[_session_lista_prom_tipo])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Promotion_Orce_Atributos> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m =>
                    m.promotion_id.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.deal_id.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.tipo.ToUpper().Contains(param.sSearch.ToUpper()) || m.attribute_name.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.attribute_value.ToUpper().Contains(param.sSearch.ToUpper())
                    );
            }

            //Manejador de orden
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.promotion_id,
                             a.deal_id,
                             a.tipo,
                             a.attribute_name,
                             a.attribute_value,                             
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
        public FileContentResult ListaPromTipoExcel()
        {
            if (Session[_session_lista_prom_tipo_excel] == null)
            {
                List<Ent_Promotion_Orce_Atributos> liststoreConf = new List<Ent_Promotion_Orce_Atributos>();
                Session[_session_lista_prom_tipo_excel] = liststoreConf;
            }
            List<Ent_Promotion_Orce_Atributos> lista = (List<Ent_Promotion_Orce_Atributos>)Session[_session_lista_prom_tipo_excel];
            string[] columns = { "promotion_id", "deal_id", "tipo", "attribute_name", "attribute_value"};
            byte[] filecontent = ExcelExportHelper.ExportExcel(lista, "", false, columns);
            string nom_excel = "Lista de Promocion Orce-Tipo";
            return File(filecontent, ExcelExportHelper.ExcelContentType, nom_excel + ".xlsx");
        }
        public ActionResult getpromotion_orce(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_lis_promo_orce_private] == null)
            {
                List<Ent_Promotion_Orce> lisprom = new List<Ent_Promotion_Orce>();
                Session[_session_lis_promo_orce_private] = lisprom;
            }

            //Traer registros
            IQueryable<Ent_Promotion_Orce> membercol = ((List<Ent_Promotion_Orce>)(Session[_session_lis_promo_orce_private])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<Ent_Promotion_Orce> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => 
                     m.CAMPAIGN_ID.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.PROMOTION_ID.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.PROMOTION_NAME.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.TIPO_PROMOCION.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.USUARIO_CREACION.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.ESTADO.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.ID_OFERTA.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.CODIGO_OFERTA.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.CODIGO_CUPON.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.NAME_OFERTA.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.NOMBRE_PDV.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.TIPO_OFERTA.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.DEAL_ACTIVADO.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.ESTILO_UMBRAL.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.TIPO_UMBRAL.ToUpper().Contains(param.sSearch.ToUpper()) 

                     );
            }
            //Manejador de orden
            //var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            //Func<Models_Guia, string> orderTO = (m => m.TIENDA_ORI);
            //Func<Models_Guia, string> orderNumero = (m => m.NUMERO);
            //Func<Models_Guia, DateTime> orderFecha = (m => Convert.ToDateTime(m.FECHA));
            //Func<Models_Guia, int> orderPares = (m => Convert.ToInt32(m.PARES));
            //Func<Models_Guia, double> orderVC = (m => Convert.ToDouble(m.VCALZADO));
            //Func<Models_Guia, int> orderNC = (m => Convert.ToInt32(m.NOCALZADO));
            //Func<Models_Guia, double> orderVNC = (m => Convert.ToDouble(m.VNOCALZADO));
            //Func<Models_Guia, string> orderEstado = (m => m.ESTADO);

            //var sortDirection = Request["sSortDir_0"];
            //if (sortDirection == "asc")
            //{
            //    if (sortIdx == 0) filteredMembers = filteredMembers.OrderBy(orderTO);
            //    else if (sortIdx == 2) filteredMembers = filteredMembers.OrderBy(orderNumero);
            //    else if (sortIdx == 3) filteredMembers = filteredMembers.OrderBy(orderFecha);
            //    else if (sortIdx == 4) filteredMembers = filteredMembers.OrderBy(orderPares);
            //    else if (sortIdx == 5) filteredMembers = filteredMembers.OrderBy(orderVC);
            //    else if (sortIdx == 6) filteredMembers = filteredMembers.OrderBy(orderNC);
            //    else if (sortIdx == 7) filteredMembers = filteredMembers.OrderBy(orderVNC);
            //    else if (sortIdx == 8) filteredMembers = filteredMembers.OrderBy(orderEstado);
            //}
            //else
            //{
            //    if (sortIdx == 0) filteredMembers = filteredMembers.OrderByDescending(orderTO);
            //    else if (sortIdx == 2) filteredMembers = filteredMembers.OrderByDescending(orderNumero);
            //    else if (sortIdx == 3) filteredMembers = filteredMembers.OrderByDescending(orderFecha);
            //    else if (sortIdx == 4) filteredMembers = filteredMembers.OrderByDescending(orderPares);
            //    else if (sortIdx == 5) filteredMembers = filteredMembers.OrderByDescending(orderVC);
            //    else if (sortIdx == 6) filteredMembers = filteredMembers.OrderByDescending(orderNC);
            //    else if (sortIdx == 7) filteredMembers = filteredMembers.OrderByDescending(orderVNC);
            //    else if (sortIdx == 8) filteredMembers = filteredMembers.OrderByDescending(orderEstado);
            //}
            var displayMembers = filteredMembers.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.CAMPAIGN_ID,
                             a.PROMOTION_ID,
                             a.PROMOTION_NAME,
                             a.TIPO_PROMOCION,
                             a.FEC_INICIO,
                             a.FEC_FIN,
                             a.HORA_INICIO,
                             a.HORA_FIN,
                             a.FEC_CREACION,
                             a.USUARIO_CREACION,
                             a.ESTADO,
                             a.ULTIMA_EXPORTACION,
                             a.UBIC_INCLUIDAS,
                             a.UBIC_EXCLUIDAS,
                             a.SEGMENTO_OBJETIVO,
                             a.PROMO_ACTIVADO,
                             a.EXPORTS,
                             a.ID_OFERTA,
                             a.CODIGO_OFERTA,
                             a.CODIGO_CUPON,
                             a.CANT_CUPON_GEN,
                             a.NAME_OFERTA,
                             a.NOMBRE_PDV,
                             a.TIPO_OFERTA,
                             a.CALIF_INCL,
                             a.CALIF_EXCL,
                             a.AWARD_INCL,
                             a.AWARD_EXCL,
                             a.TIPO_PROMO_PREVISTA,
                             a.SUBTOTAL_MIN,
                             a.SUBTOTAL_MAX,
                             a.IMPORTE_MAX_PREMIO,
                             a.DEAL_ACTIVADO,
                             a.ESTILO_UMBRAL,
                             a.TIPO_UMBRAL,
                             a.UMBRAL,
                             a.TIPO_DESCUENTO,
                             a.VALOR_DESCUENTO,
                             a.CANTIDAD_MAXIMA,
                             a.LIMITE_OFERTA,
                             a.DESCUENTO_PRORRATEADO
                             
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

        [HttpGet]
        public FileContentResult ExportToExcel_Prom()
        {
            List<Ent_Promotion_Orce> listpromorce = (List<Ent_Promotion_Orce>)Session[_session_lis_promo_orce_private];


            //List<Technology> technologies = StaticData.Technologies;
            string[] columns = { "CAMPAIGN_ID", "PROMOTION_ID", "PROMOTION_NAME", "TIPO_PROMOCION", "FEC_INICIO", "FEC_FIN", "HORA_INICIO", "HORA_FIN", "FEC_CREACION", "USUARIO_CREACION", "ESTADO", "ULTIMA_EXPORTACION", "UBIC_INCLUIDAS", "UBIC_EXCLUIDAS", "SEGMENTO_OBJETIVO", "PROMO_ACTIVADO", "EXPORTS", "ID_OFERTA", "CODIGO_OFERTA", "CODIGO_CUPON", "CANT_CUPON_GEN", "NAME_OFERTA", "NOMBRE_PDV", "TIPO_OFERTA", "CALIF_INCL", "CALIF_EXCL", "AWARD_INCL", "AWARD_EXCL", "TIPO_PROMO_PREVISTA", "SUBTOTAL_MIN", "SUBTOTAL_MAX", "IMPORTE_MAX_PREMIO", "DEAL_ACTIVADO", "ESTILO_UMBRAL", "TIPO_UMBRAL", "UMBRAL", "TIPO_DESCUENTO", "VALOR_DESCUENTO", "CANTIDAD_MAXIMA", "LIMITE_OFERTA", "DESCUENTO_PRORRATEADO" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(listpromorce, "Promocion Orce", true, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "PromOrce.xlsx");
        }

        #endregion
    }
}