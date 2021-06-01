using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.NPS
{
    /// <summary>
    /// CHART ESTADO
    /// </summary>
    public class Ent_NPS_Dashboard_Estado
    {
        public List<Ent_NPS_Estado_Chart> _ListarChar { get; set; }
        public List<Ent_NPS_Estado_Lista> _ListarExcel { get; set; }
        //Campos adicionales
        public DateTime FEC_INI { get; set; }
        public DateTime FEC_FIN { get; set; }
    }
    public class Ent_NPS_Estado_Chart
    {
        public string COD_ESTADO { get; set; }
        public string DES_ESTADO { get; set; }
        public int TRANSAC { get; set; }
    }
    public class Ent_NPS_Estado_Lista
    {
        public string DES_ESTADO { get; set; }
        public string DNI { get; set; }
        public string CORREO { get; set; }
    }
    public class Ent_NPS_Chart_Data
    {
        public string[] labels { get; set; }
        public string[] labelsTooltip { get; set; }
        public List<Ent_NPS_Chart_DataSet> datasets { get; set; }
    }
    public class Ent_NPS_Chart_DataSet
    {
        public string label { get; set; }
        public string[] backgroundColor { get; set; }
        public string borderWidth { get; set; }
        public int[] data { get; set; }
    }
    /// <summary>
    /// CHART TIPO
    /// </summary>
    public class Ent_NPS_Dashboard_Tipo
    {
        public List<Ent_NPS_Tipo_Chart> _ListarChar { get; set; }
        public List<Ent_NPS_Tipo_Nota> _ListarNota { get; set; }
        public List<Ent_NPS_Tipoo_Lista> _ListarExcel { get; set; }
        //Campos adicionales
        public string COD_NPS { get; set; }
        public string CODTDA { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FEC_INI { get; set; }
        public DateTime FEC_FIN { get; set; }
    }

    public class Ent_NPS_Tipo_Chart
    {
        public string DES_TIP_RES { get; set; }
        public Decimal VALOR_PORC { get; set; }
        public string COLOR_RGBA { get; set; }
    }
    public class Ent_NPS_Tipo_Nota
    {
        public decimal? NOTA { get; set; }
        public Decimal? TOTAL_RESPUESTAS { get; set; }
    }
    public class Ent_NPS_Tipoo_Lista
    {
        public string TIPO { get; set; }
        public string DNI { get; set; }
        public string CORREO { get; set; }
    }
    public class Ent_Nps_Tipo_Chart_Data
    {
        public string Notas { get; set; }
        public string Respuesta { get; set; }
        public string[] labels { get; set; }
        public string[] labelsTooltip { get; set; }
        public List<Ent_Nps_Tipo_Chart_DataSet> datasets { get; set; }
    }
    public class Ent_Nps_Tipo_Chart_DataSet
    {
        public string label { get; set; }
        public string[] backgroundColor { get; set; }
        public string borderWidth { get; set; }
        public decimal[] data { get; set; }
    }
    /// <summary>
    /// DISTRITO
    /// </summary>
    public class Ent_NPS_Dashboard_Distrito
    {
        public List<Ent_NPS_Distrito_Chart> _ListarChar { get; set; }
        public List<Ent_NPS_Distrito_Listar> _ListarExcel { get; set; }
        //Campos adicionales
        public DateTime FEC_INI { get; set; }
        public DateTime FEC_FIN { get; set; }
        public object Cols { get; set; }
        public object Rows { get; set; }
        public object Data { get; set; }
    }

    public class Ent_NPS_Distrito_Chart
    {
        public string COD_NPS { get; set; }
        public string PREGUNTA_NPS { get; set; }
        public string DISTRITO { get; set; }
        public int NOTA { get; set; }
        public string COLOR_RGBA { get; set; }

    }
    public class Ent_NPS_Distrito_Listar
    {
        public string DISTRITO { get; set; }
        public string TIENDA { get; set; }
        public string COD_NPS { get; set; }
        public string PREGUNTA_NPS { get; set; }
        public int NOTA { get; set; }
        public string COLOR_RGBA { get; set; }
    }
}
