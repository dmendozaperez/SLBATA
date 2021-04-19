using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ECommerce
{
    public class Ent_Vtex
    {

        public string Id_Orden { get; set; }
        public string Fecha_Pedido { get; set; }
        //public string Est_Sis_Fact { get; set; }
        public string Estado_Pedido { get; set; }
        public string Estado_Orob { get; set; }
        public string Fecha_Facturacion { get; set; }
        public string Comprobante { get; set; }
        public string Tipo_courier { get; set; }
        public string Almacen { get; set; }
        public string Ubigeo { get; set; }
        //public string Ubicacion { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Semana { get; set; }
        public string ArticuloId { get; set; }
        public string Talla { get; set; }
        public Int32 Cantidad { get; set; }
        public decimal Precio_conIGV { get; set; }
        public decimal Precio_sinIGV { get; set; }
        public string Cod_Linea3 { get; set; }
        public string Des_Linea3 { get; set; }
        public string Cod_Cate3 { get; set; }
        public string Des_Cate3 { get; set; }
        public string Cod_Subc3 { get; set; }
        public string Des_Subc3 { get; set; }
        public string Cod_Marc3 { get; set; }
        public string Des_Marca { get; set; }
        public decimal Precio_Planilla { get; set; }
        public decimal Costo { get; set; }
        public Int32 Alm_C { get; set; }
        public Int32 Alm_5 { get; set; }
        public Int32 Alm_B { get; set; }
        public Int32 Alm_W { get; set; }
        public Int32 Alm_1 { get; set; }


    }

}
