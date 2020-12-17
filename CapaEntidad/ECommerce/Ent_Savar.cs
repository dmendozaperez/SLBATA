using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapaEntidad.ECommerce
{
    public class Ent_Savar
    {
        public string CodPaquete { get; set; }
        public string NomRemitente { get; set; }
        public string DireccionRemitente { get; set; }
        public string DistritoRemitente { get; set; }
        public string TelefonoRemitente { get; set; }
        public string CodigoProducto { get; set; }
        public string MarcaProducto { get; set; }
        public string ModeloProducto { get; set; }
        public string ColorProducto { get; set; }
        public string TipoProducto { get; set; }
        public string DescProducto { get; set; }
        public int cantidad { get; set; }
        public string NomConsignado { get; set; }
        public string NumDocConsignado { get; set; }
        public string DireccionConsignado { get; set; }
        public string DistritoConsignado { get; set; }
        public string Referencia { get; set; }
        public string TelefonoConsignado { get; set; }
        public string CorreoConsignado { get; set; }
        public string Subservicio { get; set; }
        public string TipoPago { get; set; }
        public string MetodoPago { get; set; }
        public decimal Monto { get; set; }
        public decimal Largo { get; set; }
        public decimal Ancho { get; set; }
        public decimal Alto { get; set; }
        public decimal Peso { get; set; }
        public string ValorComercial { get; set; }
        public string HoraIni1 { get; set; }
        public string HoraFin1 { get; set; }
        public string HoraIni2 { get; set; }
        public string HoraFin2 { get; set; }
        public string Comentario { get; set; }
        public string Comentario2 { get; set; }
        //public List<Response_Registro_Savar> listaJSON { get; set; }
    }
   

    public class Response_Registro_Savar
    {
        public string Message { get; set; }

    }
}
