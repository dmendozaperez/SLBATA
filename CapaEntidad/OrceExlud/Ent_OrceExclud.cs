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
        public bool GENERAR { get; set; }
    }
    public class Ent_Orce_Exclud_Atributo
    {
        public string COD_ATR {get;set;}
        public string DES_ATR {get;set;}
        public int ID {get;set;}
        public bool ESTADO {get;set;}
        public string USUARIO_CREA {get;set;}
        public string USUARIO_MODIFICA {get;set;}
        public string FECHA_CREACION {get;set;}
        public string FECHA_MODIFICA { get; set; }
    }
    public class Ent_Tda_Xstore
    {
        public string cod_entid     {get;set;}
        public string des_entid     {get;set;}
        public string tiptda_cod    {get;set;}
        public string tiptda_des    {get;set;}
        public string cod_cadena    {get;set;}
        public string des_cadena { get; set; }
    }

}
