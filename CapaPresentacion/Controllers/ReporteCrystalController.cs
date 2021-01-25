using CapaDato.Reporte;
using CapaEntidad.Util;
using Data.Crystal.Reporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CapaPresentacion.Controllers
{
    public class ReporteCrystalController : Controller
    {
        // GET: ReporteCrystal
        private Dat_Combo datCbo = new Dat_Combo();
        public ActionResult Index()
        {
            List<Ent_Combo> list = new List<Ent_Combo>();
            Ent_Combo entCombo = new Ent_Combo();
            entCombo.cbo_codigo = "0";
            entCombo.cbo_descripcion = "----Todos----";
            list.Add(entCombo);
            ViewBag.Title = "Reporte de Planilla";
            ViewBag.Grupo = datCbo.get_ListaGrupo();
            ViewBag.Estado = datCbo.get_ListaEstado();
            //VLADIMIR
            ViewBag.Tienda = datCbo.get_ListaTiendaXstore(Session["PAIS"].ToString());
            //VLADIMIR ENBD
            ViewBag.Categoria = list;

            string strJson = "";
            JsonResult jRespuesta = null;
            var serializer = new JavaScriptSerializer();


            strJson = datCbo.listarStr_ListaCategoria("");
            jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson), JsonRequestBehavior.AllowGet);
            ViewBag.ClCategoria = jRespuesta;

            strJson = datCbo.listarStr_ListaSubCategoria("");
            jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson), JsonRequestBehavior.AllowGet);
            ViewBag.ClSBCategoria = jRespuesta;

            return View();
        }
        public JsonResult GenerarCombo(int Numsp, string Params)
        {
            string strJson = "";
            JsonResult jRespuesta = null;
            var serializer = new JavaScriptSerializer();


            switch (Numsp)
            {
                case 1:
                    strJson = datCbo.listarStr_ListaCategoria(Params);
                    jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson), JsonRequestBehavior.AllowGet);
                    break;
                case 2:
                    strJson = datCbo.listarStr_ListaSubCategoria(Params);
                    jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson), JsonRequestBehavior.AllowGet);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            return jRespuesta;
        }
        [HttpPost]
        public ActionResult ShowGenericReportInNewWin(string cod_tda, string grupo, string categoria, string subcategoria, string estados, string tipo)
        {
            //grupo = "0";categoria = "0";subcategoria = "0";estado = "0";
            Data_Planilla pl = new Data_Planilla();
            this.HttpContext.Session["ReportName"] = "Planilla.rpt";
            this.HttpContext.Session["rptSource"] =pl.get_planilla(cod_tda,grupo,categoria,subcategoria, estados, tipo);
            this.HttpContext.Session["rptSource_sub"] = pl.get_reglamed_cab();
            return Json(new
            {
                estado = "1"
            }
            );
        }
    }
}