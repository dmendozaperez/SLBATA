using CapaDato.Bata;
using CapaEntidad.Bata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class BataTransitosController : Controller
    {
        #region <DECLARACION DE VARIABLES>
        private Dat_Transito empcadtda = new Dat_Transito();
        #endregion
        // GET: BataTransitos
        public ActionResult ConsultaTransito()
        {
            string pais = "PE";
            if (Session["PAIS"] != null)
            {
                pais = Session["PAIS"].ToString();
            }

            List<Ent_Tran_Emp_Cad_Tda> combo_empcadtda = empcadtda.lista_Emp_Cad_Tda_trans(pais);
            List<Ent_Tran_concepto> combo_concepto = empcadtda.lista_concepto_trans();
            List<Ent_Tran_Articulo> como_articulo = empcadtda.lista_articulo_trans(pais);


            ViewBag.Empresa = combo_empresa(combo_empcadtda);
            ViewBag.EmpCadTda = combo_empcadtda;

            ViewBag.Concepto = combo_concepto;
            ViewBag.Articulo = como_articulo;

            List<Ent_Tran_Emp_Cad_Tda> list_cad = new List<Ent_Tran_Emp_Cad_Tda>();
            Ent_Tran_Emp_Cad_Tda entCombo_cad = new Ent_Tran_Emp_Cad_Tda();
            entCombo_cad.cod_cadena = "-1";
            entCombo_cad.des_cadena = "----Todos----";
            list_cad.Add(entCombo_cad);

            List<Ent_Tran_Emp_Cad_Tda> list_tda = new List<Ent_Tran_Emp_Cad_Tda>();
            Ent_Tran_Emp_Cad_Tda entCombo_tda = new Ent_Tran_Emp_Cad_Tda();
            entCombo_tda.cod_entid = "-1";
            entCombo_tda.des_entid = "----Todos----";
            list_tda.Add(entCombo_tda);

            ViewBag.Cadena = list_cad;
            ViewBag.Tienda = list_tda;

            return View();
        }
        private List<Ent_Tran_Emp_Cad_Tda> combo_empresa(List<Ent_Tran_Emp_Cad_Tda> combo_general)
        {
            List<Ent_Tran_Emp_Cad_Tda> listar = null;
            try
            {
                listar = new List<Ent_Tran_Emp_Cad_Tda>();
                listar = (from grouping in combo_general.GroupBy(x => new Tuple<string, string>(x.cod_empresa, x.des_empresa))
                          select new Ent_Tran_Emp_Cad_Tda
                          {
                              cod_empresa = grouping.Key.Item1,
                              des_empresa = grouping.Key.Item2,
                          }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listar;
        }
    }
}