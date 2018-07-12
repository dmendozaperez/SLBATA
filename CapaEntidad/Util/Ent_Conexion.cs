using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Util
{
    public class Ent_Conexion
    {
        public static string conexion
        {
            get {
                // return "Server=des.bgr.pe;Database=BdTiendaReplica;User ID=junior;Password=Bata2018**@;Trusted_Connection=False;";
                return "Server=10.10.10.208;Database=BdTienda;User ID=sa;Password=Bata2013;Trusted_Connection=False;";

            }
        }

        public static string conexionPosPeru
        {
            get
            {
                // return "Server=des.bgr.pe;Database=BdTiendaReplica;User ID=junior;Password=Bata2018**@;Trusted_Connection=False;";
                return "Server=posperu.bgr.pe;Database=BDPOS;User ID=junior;Password=bata2018;Trusted_Connection=False;";

            }
        }
        public static string strDirectorio
        {
            get
            {

                return "~/Cupones/Bata_"; ;

            }
        }

        public static string plantilla
        {
            get
            {
            
                return "~/FormatoCupon/htmlcupon20.html";

            }
        }
    }
}
