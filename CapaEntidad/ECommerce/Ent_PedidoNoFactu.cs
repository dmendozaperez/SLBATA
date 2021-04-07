using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ECommerce
{
    public class Ent_PedidoNoFactu
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
