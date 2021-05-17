using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CapaEntidad.Util
{
    public class Ent_Conexion
    {

        public static string conexion { get; set; }
        public static string conexionPosPeru { get; set; }
        public static string conexionEcommerce { get { return (HttpContext.Current.Session["PAIS"].ToString() == "EC") ?  ConfigurationManager.ConnectionStrings["SQL_ECOM_EC"].ConnectionString :  ConfigurationManager.ConnectionStrings["SQL_ECOM"].ConnectionString; } }

        //public static string conexion
        //{
        //    get {
        //        // return "Server=des.bgr.pe;Database=BdTiendaReplica;User ID=junior;Password=Bata2018**@;Trusted_Connection=False;";
        //        //return "Server=10.10.10.208;Database=BdTienda;User ID=sa;Password=Bata2013;Trusted_Connection=False;";
        //        return "Server=posperu.bgr.pe;Database=BDPOS;User ID=pos_oracle;Password=Bata2018**;Trusted_Connection=False;";

        //    }
        //}

        //public static string conexionPosPeru
        //{
        //    get
        //    {
        //        // return "Server=des.bgr.pe;Database=BdTiendaReplica;User ID=junior;Password=Bata2018**@;Trusted_Connection=False;";
        //        return "Server=posperu.bgr.pe;Database=BDPOS;User ID=pos_oracle;Password=Bata2018**;Trusted_Connection=False;";

        //    }
        //}
        public static string strDirectorio
        {
            get
            {
                return "~/Cupones/Bata_"; 
            }
        }
        public static string strDirectorio_Interface
        {
            get
            {
                return "~/Interface/";

            }
        }

        public static string strDirectorio_StkLedger
        {
            get
            {

                return "~/StkLedger/"; 

            }
        }
        public static string plantilla
        {
            get
            {
            
                return "~/FormatoCupon/htmlcupon20.html";

            }
        }

        public static string servidorReporte
        {
            get
            {
                //return "http://posperu.bgr.pe:80/BataRptSrv/";
                return "http://172.28.7.14/BataRptSrv/";
            }
        }

        public static string usuarioReporte
        {
            get
            {
                return "ReportBata";
            }
        }

        public static string passwordReporte
        {
            get
            {
                return "Bata2018**";
            }
        }

        public static string dominioReporte
        {
            get
            {
                return "BataRptSrv";
            }
        }

        public static string CarpetaPlanillaReporte
        {
            get
            {
                return "ReportBata";
            }
        }
        public Ent_Conexion()
        {
            //HttpContext.Current.Session
            //if (Session["PAIS"].ToString() == "EC")
            //{
            //    Ent_Conexion.conexionEcommerce = ConfigurationManager.ConnectionStrings["SQL_ECOM_EC"].ConnectionString;
            //}
            //else if (Session["PAIS"].ToString() == "PE")
            //{
            //    Ent_Conexion.conexionEcommerce = ConfigurationManager.ConnectionStrings["SQL_ECOM"].ConnectionString;
            //}
        }

    }


}
