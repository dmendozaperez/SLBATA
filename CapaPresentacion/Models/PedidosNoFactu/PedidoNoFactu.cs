using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models.PedidosNoFactu
{
    public class PedidoNoFactu
    {
        public string id_pedido { get; set; }
        public string cod_tienda { get; set; }
        public string nom_tienda { get; set; }
        public DateTime fec_pedido { get; set; }
        public string cod_articulo { get; set; }
        public string nom_articulo { get; set; }
        public string estado { get; set; }
        public string estado_ob { get; set; }
        public string nro_comprob { get; set; }


    }
}