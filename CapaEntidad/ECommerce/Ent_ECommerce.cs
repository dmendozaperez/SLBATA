using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ECommerce
{
    public class Ent_ECommerce
    {
        public string idPedido { get; set; }
        //public string tiendaOrigen { get; set; }
        //public string tiendaDestino { get; set; }
        public string RefPedido { get; set; }
        public DateTime fechaPedido { get; set; }
        public string tipoComprobante { get; set; }
        public string SerieDoc { get; set; }
        public string NroDoc { get; set; }
        public string CodSeguimiento { get; set; }
        public string nom_courier { get; set; }
        public string estado { get; set; }
        public string cliente { get; set; }
        public string direccionA { get; set; }
        public string direccionB { get; set; }
        public string direccionCliente { get; set; }
        public string referenciaCliente { get; set; }
        //public string hora { get; set; }
        public string TpDocCli { get; set; }
        public string noDocCli { get; set; }
        public string nombreCliente { get; set; }
        public string apePatCliente { get; set; }
        public string apeMatCliente { get; set; }
        public string nombreCompletoCliente { get; set; }
        public string cod_entid { get; set; }
        //public string idVendedor { get; set; }
        //public string nomVendedor { get; set; }
        public string nombreEstado { get; set; }
        //public string descripcionEstado { get; set; }
        //public string colorEstado { get; set; }
        //public decimal importeTotal { get; set; }
        //public string nombreTipoCV { get; set; }
        public List<Ent_DetallesECommerce> detalles { get; set; }
        //public List<Ent_HistorialEstadosCV> historialEstados { get; set; }
    }
    public class Ent_DetallesECommerce
    {
        public string codigoProducto { get; set; }
        public string nombreProducto { get; set; }
        public string cantidad { get; set; }
        public string precioUnitario { get; set; }
        public string descuento { get; set; }
        public string total { get; set; }
        public string talla { get; set; }
    }
    //public class Ent_HistorialEstadosCV
    //{
    //    public int id { get; set; }
    //    public string cod_entid { get; set; }
    //    public string fc_nint { get; set; }
    //    public string id_estado { get; set; }
    //    public DateTime fecha { get; set; }
    //    public string cod_usuario { get; set; }
    //    public string descripcion { get; set; }
    //    public string usu_nombre { get; set; }
    //    public string nombreEstado { get; set; }
    //    public string descripcionEstado { get; set; }
    //    public string colorEstado { get; set; }
    //    public string cod_vendedor { get; set; }
    //    public string nomVendedor { get; set; }
    //}
}
