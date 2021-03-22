using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.NPS
{
    public class Ent_NPS_Pregunta
    {
        public string COD_NPS_CON { get; set; }
        public string COD_NPS { get; set; }
        public string PREGUNTA_NPS { get; set; }
        public string COD_TIP_NPS { get; set; }
        public bool? RANGO_NPS { get; set; }
        public int? VALOR_MIN_NPS { get; set; }
        public int? VALOR_MAX_NPS { get; set; }
        public string ESTADO_NPS { get; set; }
        public string COD_NPS_EST { get; set; }
        //Campos adicionales
        public string ID { get; set; }
        //Campos Relacionados
        public List<Ent_NPS_Pregunta_Det> _ListarPreguntas { get; set; }
    }

    public class Ent_NPS_Pregunta_Det
    {
        public decimal ID { get; set; }
        public string COD_NPS_CON { get; set; }
        public string COD_NPS { get; set; }
        public string PREGUNTA_NPS { get; set; }
        public decimal? COD_NPS_OPC { get; set; }
        public string DES_NPS_OPC { get; set; }
        public string COD_NPS_EST { get; set; }
        public bool RANGO_NPS { get; set; }
        public int VALOR_MIN_NPS { get; set; }
        public int VALOR_MAX_NPS { get; set; }
    }
    public class Ent_TMP_NPS_Respuestas
    {
        public string COD_NPS_CON { get; set; }
        public string COD_NPS { get; set; }
        public decimal? COD_NPS_OPC { get; set; }
        public int VALOR_NPS { get; set; }
        public bool isOK { get; set; }
        //Campos adicionales
        public string ID { get; set; }
    }
}
