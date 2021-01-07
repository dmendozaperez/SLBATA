using CapaDato.Api.BataClub;
using CapaEntidad.Api.BataClub;
using CapaEntidad.Api.Seguridad;
using CapaPresentacion.Models.Api_WS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CapaPresentacion.Controllers
{
    /// <summary>
    /// Consultas de Clientes Bata
    /// </summary>
    public class ApiBClubController : BaseController
    {
        /// <summary>
        /// Busqueda de Factura
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]        
        public Reply Buscar_Venta([FromBody] BuscarDoc model)
        {
            Reply or = new Reply();
            or.result = 0;
            Dat_Keos dat_doc = null;
            try
            {
                if (!Verify(model.token))
                {
                    or.messaje = "No autorizado";
                    return or;
                }
                if (model.num_doc == null)
                {
                    or.messaje = "Ingrese el numero de boleta a buscar";
                    return or;
                }
                if (model.num_doc.Trim().Length == 0)
                {
                    or.messaje = "Ingrese el numero de boleta a buscar";
                    return or;
                }

                if (model.num_doc.Trim().Length != 12)
                {
                    or.messaje = "El numero de boleta debe tener 12 de longitud";
                    return or;
                }

                dat_doc = new Dat_Keos();

                Int32 resultado = 0;
                or.data = dat_doc.buscar_venta(model.token, model.num_doc, ref resultado);
                or.result = (resultado==1)? 1:0;// resultado;

                or.data = (resultado == 0 ? null : or.data);

                or.messaje = (resultado == 1) ? "Documento encontrado" : "El Nro de boleta no existe, por favor verificar";

            }
            catch (Exception exc)
            {
                or.messaje = "¡Ocurrio un error en el servidor!";

            }

            return or;
        }

      
        /// <summary>
        /// Busqueda de estado de pedido
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public Reply Buscar_Pedido([FromBody] BuscarPedido model)
        {
            Reply or = new Reply();
            or.result = 0;
            Dat_Keos dat_doc = null;
            try
            {
                if (!Verify(model.token))
                {
                    or.messaje = "No autorizado";
                    return or;
                }
                if (model.nro_pedido == null)
                {
                    or.messaje = "Ingrese el numero de pedido a buscar";
                    return or;
                }
                if (model.nro_pedido.Trim().Length == 0)
                {
                    or.messaje = "Ingrese el numero de pedido a buscar";
                    return or;
                }

                dat_doc = new Dat_Keos();

                Int32 resultado = 0;
                or.data = dat_doc.buscar_pedido(model.token, model.nro_pedido, ref resultado);
                or.result = (resultado == 1) ? 1 : 0;// resultado;
                or.data = (resultado == 0 ? null : or.data);
                or.messaje = (resultado == 1) ? "Pedido encontrado" : "El Nro de Pedido no existe, por favor verificar";

            }
            catch (Exception exc)
            {
                or.messaje = "¡Ocurrio un error en el servidor!";

            }

            return or;
        }

        /// <summary>
        /// Busqueda de cliente Bataclub
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public Reply Buscar_BC([FromBody] BuscarBC model)
        {
            Reply or = new Reply();
            or.result = 0;
            Dat_Keos dat_doc = null;
            try
            {
                if (!Verify(model.token))
                {
                    or.messaje = "No autorizado";
                    return or;
                }
                if (model.nro_dni == null)
                {
                    or.messaje = "Ingrese el numero de dni a buscar";
                    return or;
                }
                if (model.nro_dni.Trim().Length == 0)
                {
                    or.messaje = "Ingrese el numero de dni a buscar";
                    return or;
                }
                if (model.nro_dni.Trim().Length != 8)
                {
                    or.messaje = "El numero de dni debe tener 8 digitos, por favor verificar ";
                    return or;
                }

                dat_doc = new Dat_Keos();

                Int32 resultado = 0;
                or.data = dat_doc.buscar_bc(model.token, model.nro_dni, ref resultado);
                //or.result = 1;//  resultado;
                or.result = (resultado == 1) ? 1 : 0;// resultado;
                or.data = (resultado == 0 ? null : or.data);
                or.messaje = (resultado == 1) ? "Documento encontrado" : "No es miembro de Bataclub";

            }
            catch (Exception exc)
            {
                or.messaje = "¡Ocurrio un error en el servidor!";

            }

            return or;
        }

    }
}
