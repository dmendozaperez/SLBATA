using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models.CanalVenta
{
    public class Urbano
    {
        public string linea { get; set; }
        public string id_contrato { get; set; }

        public string cod_rastreo { get; set; }
        public string cod_barra { get; set; }
        public string fech_emi_vent { get; set; }
        public string nro_o_compra { get; set; }
        public string nro_guia_trans { get; set; }
        public string nro_factura { get; set; }
        
        public string cod_cliente { get; set; }
        public string nom_cliente { get; set; }
        public string nom_empresa { get; set; }
        public string nro_telf { get; set; }
        public string nro_telf_mobil { get; set; }
        public string correo_elec { get; set; }

        public string dir_entrega { get; set; }
        public int nro_via { get; set; }
        public string nom_urb { get; set; }
        public string ubi_direc { get; set; }
        public string ref_direc { get; set; }
        public int id_direc { get; set; }

        public string fec_pro { get; set; }
        public string arco_hor { get; set; }
        public string fech_venc { get; set; }

        public string nom_autorizado { get; set; }
        public string nro_doc_autorizado { get; set; }
        public string nom_autorizado_2 { get; set; }
        public string nro_doc_autorizado_2 { get; set; }

        public string med_pago { get; set; }
        public string descripcion { get; set; }
        public string anotacion { get; set; }
        public string moneda { get; set; }
        public string importe { get; set; }
        
        public string peso_total { get; set; }
        public string pieza_total { get; set; }
        public string urgente { get; set; }
        public string picking { get; set; }
        public string mecanizado { get; set; }
        public string asegurado { get; set; }
        public string monto_asegurado { get; set; }
        public string via_aereo { get; set; }
        
        public string venta_seller { get; set; }
        public string sell_codigo { get; set; }
        public string sell_nombre { get; set; }
        public string sell_direcc { get; set; }
        public string sell_ubigeo { get; set; }

        public List<Productos> productos { get; set; }

        public List<Cedibles> cedibles { get; set; }

    }
    public class Productos
    {
        public string cod_sku { get; set; }
        public string descr_sku { get; set; }
        public string modelo_sku { get; set; }
        public string marca_sku { get; set; }
        public string peso_sku { get; set; }
        public string peso_v_sku { get; set; }
        public string valor_sku { get; set; }
        public string cantidad_sku { get; set; }
        public string alto { get; set; }
        public string largo { get; set; }
        public string ancho { get; set; }
    }
    public class Cedibles
    {
        public string cod_pod { get; set; }
        public string descr_pod { get; set; }
    }
    public class Respuesta
    {
        public int sql_error { get; set; }
        public string msg_error { get; set; }
        public int error { get; set; }
        public string mensaje { get; set; }
        public string guia { get; set; } = "";
    }


}