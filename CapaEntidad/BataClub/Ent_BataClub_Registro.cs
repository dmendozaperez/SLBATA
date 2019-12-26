using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.BataClub
{
    public class Ent_BataClub_Registro
    {
        public string Canal { get; set; }
        public string Genero { get; set; }
        public string Nombres { get; set; } = "";
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string ApellidoPaterno { get; set; } = "";
        public string ApellidoMaterno { get; set; } = "";
        public string Dni{ get; set; } = "";
        public string Celular { get; set; } = "";
        public string CorreoElectronico{ get; set; } = "";
        public string CorreoElectronico2{ get; set; } = "";
        public string FechaNacimiento{ get; set; }
        public string Departamento{ get; set; }
        public string Provincia{ get; set; }
        public string Distrito{ get; set; }
        public bool Politica { get; set; }
        public bool Existe { get; set; }
        public bool Miembro { get; set; }
        public string Ubigeo { get; set; }
        public string UbigeoDistrito { get; set; }
    }
    public class Ent_BataClub_Preg_Encuesta
    {
        public string  COD_PREG_ENC{get;set;}
        public string COD_TIPO_PREG       {get;set;}
        public string NOMBRE              {get;set;}
        public string DESCRIPCION         {get;set;}
        public int VALOR_MIN       {get;set;}
        public int VALOR_MAX       {get;set;}
        public bool ESTADO_PREG         {get;set;}
        public DateTime FEC_INGRESO         {get;set;}
        public DateTime FEC_MODIF           {get;set;}
        public string USUARIO { get; set; }
        public List<string> labels { get; set; }
        public List<int> values { get; set; }
    }
    public class Ent_BataClub_Encuesta
    {
        public string COD_ENCUESTA {get;set;}
        public string COD_TDA_ENC  {get;set;}
        public string COD_ENTID    {get;set;}
        public string FC_NINT      {get;set;}
        public string FC_SFAC      {get;set;}
        public string FC_NFAC      {get;set;}
        public string CORREO       {get;set;}
        public string DNI          {get;set;}
        public string FEC_INGRESO  {get;set;}
        public string FEC_MODIF    {get;set;}
        public string USUARIO { get; set; }
        public List<Ent_BataClub_Resp_Encuesta> respuestas { get; set; }
    }
    public class Ent_BataClub_Resp_Encuesta
    {
        public string COD_RESP_ENC {get;set;}
        public string COD_ENCUESTA {get;set;}
        public string COD_PREG_ENC {get;set;}
        public string VALOR        {get;set;}
        public string FEC_INGRESO  {get;set;}
        public string FEC_MODIF    {get;set;}
        public string USUARIO { get; set; }        
        
    }
}
