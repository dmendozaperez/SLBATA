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
        public List<Ent_BataClub_DashBoard_Distritos> listDistritos { get; set; }
        public List<Ent_BataClub_DashBoard_Tiendas_Distritos> listDistritosTiendas { get; set; }
        public List<Ent_BataClub_DashBoard_Proms> listDetPromTda { get; set; }
        public List<Ent_BataClub_DashBoard_Compras> listComprasTot { get; set; }
        public List<Ent_BataClub_DashBoard_Tipo_Compras> listTipoComprasTot { get; set; }
        public List<Ent_BataClub_DashBoard_Compras_Cliente> listComprasCliTot { get; set; }
        public List<Ent_BataClub_Dashboard_Datos_Incompletos> listincompletos { get; set; }
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
    public class Ent_BataClub_Canalventa
    {
        public string can_venta_id { get; set; }
        public string can_venta_des { get; set; }
    }
    public class Ent_BataClub_Dashboard_Canales
    {
        public string CANAL { get; set; }
        public decimal REGISTROS { get; set; }
        public decimal PORC { get; set; }
    }
    public class Ent_BataClub_Dashboard_Datos_Incompletos
    {
        public string campo { get; set; }
        public Decimal porc { get; set; }

    }
    public class Ent_BataClub_Datos_Incompletos_Excel
    {
        public string dni { get; set; }
        public string nombres { get; set; }
        public string correo { get; set; }
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
    public class Ent_BataClub_DashBoard_Distritos
    {
        public string supervisor { get; set; }
        public string distrito { get; set; }
        public decimal registros { get; set; }
        public decimal transac { get; set; }
        public decimal consumido { get; set; }
        public Decimal bataclub { get; set; }
    }
    public class Ent_BataClub_DashBoard_Tiendas_Distritos
    {
        public string supervisor { get; set; }
        public string distrito { get; set; }
        public string tienda { get; set; }
        public decimal registros { get; set; }
        public decimal transac { get; set; }
        public decimal consumido { get; set; }
        public Decimal bataclub { get; set; }
    }
    public class Ent_BataClub_Compras_CL_Excel
    {
        public string dni { get; set; }
        public string correo { get; set; }
        public Decimal compras { get; set; }
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
    public class Ent_BataClub_DashBoard_Compras
    {
        public Decimal transac { get; set; }
        //public string prom { get; set; }
        public Decimal monto { get; set; }
        public string tipo { get; set; }
    }
    public class Ent_BataClub_DashBoard_Tipo_Compras
    {
        public string prom { get; set; }
        public Decimal monto { get; set; }
        public Decimal transac { get; set; }
        public string tipo { get; set; }

    }
    public class Ent_BataClub_DashBoard_Compras_Cliente
    {
        public string com_des { get; set; }
        public decimal nclientes { get; set; }
    }
    public class Ent_BataClub_Dashboard_PSM
    {
        public string marca { get; set; }
        public decimal pares { get; set; }
        public decimal soles { get; set; }
    }
    public class Ent_BataClub_Dashboard_CxM
    {
        public List<Ent_BataClub_DashBoard_Mensual> meses { get; set; }
        public List<Ent_BataClub_Dashboard_Genero> genero { get; set; }
    }
    public class Ent_BataClub_Dashboard_PPS
    {
        public decimal PORC_PARES_BATACLUB {get;set;}
        public decimal PORC_SOLES_BATACLUB {get;set;}
        public decimal PORC_PARES_BATA     {get;set;}
        public decimal PORC_SOLES_BATA { get; set; }
    }

    public class Ent_BataClub_Dashboard_PSPM
    {
        /* Pares, soles , promoicion,meses */
        public string COD_ENTID { get; set; }
        public string TIENDA { get; set; }
        public string ANIO { get; set; }
        public string MES { get; set; }
        public string MES_STR { get; set; }
        public decimal PARES { get; set; }
        public decimal SOLES { get; set; }
        public string PROMOCION { get; set; }
    }

    public class Ent_BC_Dashboard_Ticket_Promedio
    {
        public string GRUPO { get; set; }
        public  decimal TRANSAC { get; set; }
        public decimal TOTAL { get; set; }
        public decimal TICKETPROM { get; set; }
    }
    public class Ent_BC_Dashboard_CVB
    {
        public string n_semana { get; set; }
        public string cod_entid { get; set; }
        public string des_entid { get; set; }
        public decimal anterior { get; set; }
        public decimal actual { get; set; }
        public decimal porc { get; set; }
        public string semana_ant { get; set; }
        public string semana_act { get; set; }
    }
    public class Ent_BC_Venta_Categoria
    {
        public string CATEGORIA { get; set; }
        public decimal TOTAL_BATACLUB { get; set; }
        public decimal TOTAL_BATA { get; set; }
    }
}
