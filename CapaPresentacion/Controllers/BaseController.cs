using CapaDato.Api.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CapaPresentacion.Controllers
{

    /// <summary>
    /// Token de Acceso
    /// </summary>
    public class BaseController : ApiController
    {
        /// <summary>
        /// Verifica la clave token
        /// </summary>
        /// <param name="token">cadena</param>
        /// <returns></returns>
        public Boolean Verify(string token)
        {
            Boolean valida = false;
            Dat_Acceso acceso_api = null;
            try
            {
                /*acceso token*/
                acceso_api = new Dat_Acceso();
                valida = acceso_api.ExisteTokenApi("05", token);
            }
            catch (Exception)
            {               
            }
            return valida;
        }
    }
}
