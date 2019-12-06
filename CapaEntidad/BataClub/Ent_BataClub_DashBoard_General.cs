using System;
using System.Collections.Generic;
using System.Data;
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
        public List<Ent_BataClub_Dashboard_Canales> listCanles { get; set; }
        public List<Ent_BataClub_DashBoard_Mensual> listMesParesSoles { get; set; }
        public List<Ent_BataClub_DashBoard_Proms> listPromsPS { get; set; }
        public List<Ent_BataClub_Dashboard_Genero> listMesGenero { get; set; }
        public List<Ent_BataClub_DashBoard_Supervisor> listSupervisorTot { get; set; }
        public List<Ent_BataClub_DashBoard_TiendasSupervisor> listTiendasSupervTot { get; set; }
        public List<Ent_BataClub_DashBoard_Proms> listDetPromTda { get; set; }

        public DataTable dtventa_bataclub { get; set; }

    }
    public class Ent_BataClub_DashBoard_General
    {
        public decimal REGISTROS { get; set; }
        public decimal MIEMBROS { get; set; }        
        public decimal TRANSAC_CUPON { get; set; }
        public decimal PARES { get; set; }
        public decimal SOLES { get; set; }
    }
    public class Ent_BataClub_DashBoard_Mensual
    {
        public string ANIO { get; set; }
        public int MES { get; set; }
        public string MES_STR { get; set; }
        public decimal NUMERO { get; set; }
        public decimal NUMERO2 { get; set; }
    }   
    public class Ent_BataClub_Dashboard_Canales
    {
        public string CANAL { get; set; }
        public decimal REGISTROS { get; set; }
        public decimal PORC { get; set; }
    }
    public class Ent_BataClub_Chart_Data
    {
        public string[] labels { get; set; }
        public string[] labelsTooltip { get; set; }
        public List<Ent_BataClub_Chart_DataSet> datasets { get; set; }        
    }
    public class Ent_BataClub_Chart_DataSet
    {
        public string label                { get; set; }
        public string[] backgroundColor { get;set;}
        public string borderWidth { get;set;}
        public decimal[] data { get; set; }
    }
    public class Ent_BataClub_DashBoard_Proms
    {
        public string promocion { get; set; }
        public string tienda { get; set; }
        public decimal pares { get; set; }
        public decimal soles { get; set; }
    }
    public class Ent_BataClub_Dashboard_Genero
    {
        public string anio { get; set; }
        public string genero { get; set; }
        public decimal registros { get; set; }
    }
    public class Ent_BataClub_DashBoard_Supervisor
    {
        public string supervisor { get; set; }
        public decimal registros { get; set; }
        public decimal transac { get; set; }
        public decimal consumido { get; set; }
    }
    public class Ent_BataClub_DashBoard_TiendasSupervisor
    {
        public string supervisor { get; set; }
        public string tienda { get; set; }
        public decimal registros { get; set; }
        public decimal transac { get; set; }
        public decimal consumido { get; set; }
    }

    public class Ent_Bataclub_Canales_Excel
    {
        public string Canal { get; set; }
        public string Tienda { get; set; }
        public string Dni { get; set; }
        public string Nombres { get; set; }
        public string Correo { get; set; }
        public string Miem_Bataclub { get; set; }
        public string Fec_Registro { get; set; }
        public string Fec_Miembro { get; set; }
    }
}
