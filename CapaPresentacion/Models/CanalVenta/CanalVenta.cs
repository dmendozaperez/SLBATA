using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CapaPresentacion.Models.CanalVenta;
using System.Data;

namespace CapaPresentacion.Models.CanalVenta
{
    public class CanalVenta
    {
        public string serieNumero { get; set; }
        public string tiendaOrigen { get; set; }
        public string tiendaDestino { get; set; }
        public string fechaVenta { get; set; }
        public string tipo { get; set; }
        public string estado { get; set; }
        public string cliente { get; set; }
        public string direccionA { get; set; }
        public string direccionB { get; set; }
        public string direccionCliente { get; set; }
        public string referenciaCliente { get; set; }
        public string hora { get; set; }
        public string noDocCli { get; set; }
        public string nombreCliente { get; set; }
        public string apePatCliente { get; set; }
        public string apeMatCliente { get; set; }
        public string nombreCompletoCliente { get; set; }
        public string tipoComprobante { get; set; }
        public string fc_nint { get; set; }
        public string cod_entid { get; set; }
        public string idVendedor { get; set; }
        public string nomVendedor { get; set; }
        public string nombreEstado { get; set; }
        public string descripcionEstado { get; set; }
        public string colorEstado { get; set; }
        public decimal importeTotal { get; set; }
        public string nombreTipoCV { get; set; }
        public string cod_entid_b { get; set; }
        public string guia_electronica { get; set; }
        public string ubigeoCliente { get; set; }
        public string ubigeoTienda { get; set; }
        public string telefonoCliente { get; set; }
        public decimal nroPares { get; set; }
        public decimal totalSinIgv { get; set; }
        public List<DetallesCanalVenta> detalles { get; set; }
        public List<HistorialEstadosCV> historialEstados { get; set; }
        public Informacion_Tienda_envio informacionTiendaEnvio { get; set; }
        public Informacion_Tienda_Destinatario informacionTiendaDestinatario { get; set; }
    }
    public class DetallesCanalVenta
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
    public class HistorialEstadosCV
    {
        public int id { get; set; }
        public string cod_entid { get; set; }
        public string fc_nint { get; set; }
        public string id_estado { get; set; }
        public DateTime fecha { get; set; }
        public string cod_usuario { get; set; }
        public string descripcion { get; set; }
        public string usu_nombre { get; set; }
        public string nombreEstado { get; set; }
        public string descripcionEstado { get; set; }
        public string colorEstado { get; set; }
        public string cod_vendedor { get; set; }
        public string nomVendedor { get; set; }
        public string serieNumero { get; set; }
    }
    public class GuiaElectronica
    {
        public string guia { get; set; }
        public string cliente { get; set; }
        public string direccion { get; set; }
        public string referencia { get; set; }
        public string ubigeo { get; set; }        
    }
    public class Informacion_Tienda_envio
    {
        public int id { get; set; }
        public string cod_entid { get; set; }
        public string courier { get; set; } = "";
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
    }

    public class Informacion_Tienda_Destinatario
    {
        public int id { get; set; }
        public string cod_entid { get; set; }
        public string direccion_entrega { get; set; }
        public string referencia { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string nroDocumento { get; set; }
       
    }

}