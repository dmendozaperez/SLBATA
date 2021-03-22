using CapaDato.NPS;
using CapaEntidad.NPS;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using CapaEntidad.Util;

namespace CapaPresentacion.Controllers
{
    public class NPSController : Controller
    {
        private Dat_NPS dat_NPS = new Dat_NPS();
        private string _session_ID = "_session_ID";
        public ActionResult Index(string ID)
        {
            Session[_session_ID] = ID;
            Ent_NPS_Pregunta _Ent = new Ent_NPS_Pregunta();
            _Ent.ID = ID;

            //Lista la pregunta
            var Listar = dat_NPS.Leer_Preguntas(_Ent);

            if (Listar.COD_NPS_EST !="03")
            {
                //Actualiza en estado
                dat_NPS.Genera_Encuesta_Estado(_Ent);

                var GrpPregun = Listar._ListarPreguntas
                .GroupBy(x => new
                {
                    COD_NPS_CON = x.COD_NPS_CON,
                    COD_NPS = x.COD_NPS,
                    RANGO_NPS = x.RANGO_NPS,
                    PREGUNTA_NPS = x.PREGUNTA_NPS
                }).Select(y => new
                {
                    COD_NPS_CON = y.Key.COD_NPS_CON,
                    COD_NPS = y.Key.COD_NPS,
                    RANGO_NPS = y.Key.RANGO_NPS,
                    PREGUNTA_NPS = y.Key.PREGUNTA_NPS,
                    Alternativas = y.Select(z => new
                    {
                        DES_NPS_OPC = z.DES_NPS_OPC,
                        COD_NPS_OPC = z.COD_NPS_OPC
                    }).ToList()
                }).ToList();

                List<Ent_TMP_NPS_Respuestas> ListRespuesta = new List<Ent_TMP_NPS_Respuestas>();
                Ent_TMP_NPS_Respuestas entTmpNPS_Respuestas = new Ent_TMP_NPS_Respuestas();
                ViewBag.entTmpNPS_Respuestas = entTmpNPS_Respuestas;
                ViewBag.ListRespuesta = ListRespuesta;
                ViewBag.GrpPregun = GrpPregun;
                return View();
            }
            else
            {
                return View("Finalizado");
            }
        }
        public ActionResult Finalizado()
        {
            return View();
        }
        public ActionResult getGenerarEncuesta(List<Ent_TMP_NPS_Respuestas> _ListRespuesta, Ent_TMP_NPS_Respuestas _Ent)
        {
            bool Result = false;
            JsonRespuesta objResult = new JsonRespuesta();

            _Ent.ID = (string)Session[_session_ID];
            try
            {
                DataTable dtRespuestas = new DataTable();
                dtRespuestas.Columns.Add("COD_NPS_CON", typeof(string));
                dtRespuestas.Columns.Add("COD_NPS", typeof(string));
                dtRespuestas.Columns.Add("COD_NPS_OPC", typeof(decimal));
                dtRespuestas.Columns.Add("VALOR_NPS", typeof(int));
                var Fila = 0;
                foreach (var item in _ListRespuesta)
                {
                    dtRespuestas.Rows.Add();
                    dtRespuestas.Rows[Fila]["COD_NPS_CON"] = item.COD_NPS_CON;
                    dtRespuestas.Rows[Fila]["COD_NPS"] = item.COD_NPS;
                    dtRespuestas.Rows[Fila]["COD_NPS_OPC"] = Convert.ToDecimal(item.COD_NPS_OPC);
                    dtRespuestas.Rows[Fila]["VALOR_NPS"] = Convert.ToInt32(item.VALOR_NPS);
                    Fila++;
                }

                Result = dat_NPS.Genera_Encuesta(dtRespuestas, _Ent);
                if (Result)
                {
                    objResult.Success = true;
                    objResult.Message = "Por completar la encuestas.";
                }
                else
                {
                    objResult.Success = false;
                    objResult.Message = "¡Error! Al se guardar correctamente la encuestas.";
                }
            }
            catch (Exception ex)
            {
                objResult.Success = false;
                objResult.Message = "Error al registrar";
            }
            var JSON = JsonConvert.SerializeObject(objResult);
            return Json(JSON, JsonRequestBehavior.AllowGet);
        }
    }
}