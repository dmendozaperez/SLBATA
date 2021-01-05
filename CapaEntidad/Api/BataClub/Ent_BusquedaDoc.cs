using CapaEntidad.Api.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Api.BataClub
{
    /// <summary>
    /// Buscar
    /// </summary>
    public class BuscarDoc:SecurityViewModel
    {
        /// <summary>
        /// Numero de documento, factura ó boleta
        /// </summary>
        public string num_doc { get; set; }

        /// <summary>
        /// Tipo de Venta, Online ó Fisico
        /// </summary>
        public string tipo { get; set; }
        /// <summary>
        /// Lista de tiendas
        /// </summary>
        public List<ListaTiendas> listar_tienda { get; set; }        
    }
    public class ListaTiendas
    {
        /// <summary>
        /// Tiendas Bata
        /// </summary>
        public string tienda { get; set; }
    }
   public class BuscarPedido:SecurityViewModel
   {
        /// <summary>
        /// Numero de pedido
        /// </summary>
        public string nro_pedido { get; set; }
        /// <summary>
        /// Estado de pedido
        /// </summary>
        public string estado_pedido { get; set; }
   } 
    /// <summary>
    /// Buscar Cliente BataClub
    /// </summary>
    public class BuscarBC:SecurityViewModel
    {
        /// <summary>
        /// Numero de dni
        /// </summary>
        public string nro_dni { get; set; }
        /// <summary>
        /// Estado de Cliente
        /// </summary>
        public string bataclub { get; set; }
        /// <summary>
        /// Lista de promociones
        /// </summary>
        public List<ListarPromocion> listar_promocion { get; set; }
    }
    /// <summary>
    /// Listar de Promociones activas
    /// </summary>
    public class ListarPromocion
    {
        /// <summary>
        /// Descripcion de la promocion
        /// </summary>
        public string promocion { get; set; }
    }

}
