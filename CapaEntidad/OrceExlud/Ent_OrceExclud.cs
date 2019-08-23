using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.OrceExlud
{
    public class Ent_Orce_Inter_Cab
    {
        public int ORC_COD         {get;set;}  
        public string ORC_DESCRIPCION {get;set;}
        public string ORC_ATRIBUTO    {get;set;}
        public bool ORC_ENVIADO     {get;set;}
        public string ORC_FEC_ENV     {get;set;}
        public string ORC_EST_ID      {get;set;}
        public string ORC_FECHA_ING   {get;set;}
        public string ORC_FECHA_ACT   {get;set;}
        public int ORC_USUID_ING   {get;set;}
        public int ORC_USUID_ACT   {get;set;}
        public string EST_ORC_DES { get; set; }
        public List<Ent_Orce_Inter_Det_Tda> TIENDAS { get; set; }
        public string[] cadenas { get; set; }

    }
    public class Ent_Orce_Inter_Det_Tda
    {
        public int ORC_DET_TDA_COD {get;set;}
        public string ORC_DET_TDA { get; set; }
        public string ORC_DET_TDA_DES { get; set; }
        public string ORC_DET_TDA_CAD { get; set; }
    }

    public class Ent_Orce_Inter_Art
    {
        public int ORC_DET_ART_COD  {get;set;}
        public string ARTICULO { get; set; }
        public string ATRIBUTO { get; set; }
        public bool VALOR { get; set; }
    }
}
