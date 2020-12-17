using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ECommerce
{
    public class Ent_Ecommerce_Chazki
    {
        public string NroDocumento { get; set; }
        public string Ruc { get; set; }
        public string Cliente { get; set; }
        public string Tipo { get; set; }
        public string Importe { get; set; }
        public string Fecha { get; set; }
        public string CodSeguimiento { get; set; }
        public string Tienda { get; set; }
        public string CodInterno { get; set; }
        public string Ubigeo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Referencia { get; set; }
        public string Estado { get; set; }
        public string FlagCourier { get; set; }


        public List<Ent_DetallesVentaCanal_E> detalles2 { get; set; }
        public Ent_Informacion_Tienda_envio_E informacionTiendaEnvio { get; set; }
        public Ent_Informacion_Tienda_Destinatario_E informacionTiendaDestinatario { get; set; }
    }
    public class Ent_DetallesVentaCanal_E
    {
        public string codigoProducto { get; set; }
        public string nombreProducto { get; set; }
        public int cantidad { get; set; }
        public string precioUnitario { get; set; }
        public string descuento { get; set; }
        public string total { get; set; }
        public string talla { get; set; }
        public string fd_colo { get; set; }
    }
    public class Ent_Informacion_Tienda_envio_E
    {
        public int id { get; set; }
        public string cod_entid { get; set; }
        public string courier { get; set; }
        public string cx_nroDocProveedor { get; set; }
        public string cx_codTipoDocProveedor { get; set; }
        public string cx_codDireccionProveedor { get; set; }
        public string cx_codCliente { get; set; }
        public string cx_codCtaCliente { get; set; }
        public string id_usuario { get; set; }
        public string de_terminal { get; set; }
        public string chaski_storeId { get; set; }
        public string chaski_branchId { get; set; }
        public string chaski_api_key { get; set; }
        public string deliveryTrack_Code { get; set; }
        public string mode { get; set; }
        public string tiempo { get; set; }
        public string payment_method { get; set; }
        public string country { get; set; }
        public string proofPayment { get; set; }
    }
    public class Ent_Informacion_Tienda_Destinatario_E
    {
        public int id { get; set; }
        public string cliente { get; set; }
        public string dni_ruc { get; set; }
        public string cod_entid { get; set; }
        public string direccion_entrega { get; set; }
        public string referencia { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string nroDocumento { get; set; }
        public string ubigeo { get; set; }



    }
}




