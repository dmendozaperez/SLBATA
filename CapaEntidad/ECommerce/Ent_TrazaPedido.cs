using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ECommerce
{
    public class Ent_TrazaPedido
    {
        public string ID_PEDIDO { get; set; }
        public string CLIENTE { get; set; }
        public string IMPORTE_PEDIDO { get; set; }
        public string DESPACHO { get; set; }
        public string TIPO_ENTREGA { get; set; }
        public string FECHA_PEDIDO { get; set; }
        public string FECHA_ING_FACTURACION { get; set; }
        public string FECHA_REG_VENTA { get; set; }
        public string FECHA_DESPACHO { get; set; }
        public string FECHA_REG_COURIER { get; set; }
        public string TIPO_PEDIDO { get; set; }
        public string NRO_DOCUMENTO { get; set; }
        public string CODIGO_SEGUIMIENTO { get; set; }
        public string ESTADO { get; set; }
        public string TRAZABILIDAD { get; set; }
        public string COLOR { get; set; }
        public bool VALOR { get; set; }
        public Int32 FLG_REASIGNA { get; set; }
        public string USUARIO_WS { get; set; }
        public string CLAVE_WS { get; set; }
        public string RUC_WS { get; set; }
        public int TIPODOC_WS { get; set; }
        public string NRODOC_WS { get; set; }
        public int TIPRETOR_WS { get; set; }
    }

}
