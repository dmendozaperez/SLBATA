using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.BataClub
{
    public class Ent_BataClub_DashBoard
    {
        public Ent_BataClub_DashBoard_General General { get; set; }
        public List<Ent_BataClub_DashBoard_Mensual> listMesRegistros { get; set; }
        public List<Ent_BataClub_DashBoard_Mensual> listMesMiembros { get; set; }
    }
    public class Ent_BataClub_DashBoard_General
    {
        public decimal REGISTROS { get; set; }
        public decimal MIEMBROS { get; set; }
        public decimal TRANSAC_CUPON { get; set; }
    }
    public class Ent_BataClub_DashBoard_Mensual
    {
        public string ANIO { get; set; }
        public int MES { get; set; }
        public string MES_STR { get; set; }
        public decimal NUMERO { get; set; }
    }
    public class Ent_BataClub_Chart_Data
    {
        public string[] labels { get; set; }
        public List<Ent_BataClub_Chart_DataSet> datasets { get; set; }        
    }
    public class Ent_BataClub_Chart_DataSet
    {
        public string label                { get; set; }
        public string backgroundColor { get;set;}
        public string borderWidth { get;set;}
        //public string pointColor           {get;set;}
        //public string pointStrokeColor     {get;set;}
        //public string pointHighlightFill   {get;set;}
        //public string pointHighlightStroke {get;set;}
        public decimal[] data { get; set; }
    }
}
