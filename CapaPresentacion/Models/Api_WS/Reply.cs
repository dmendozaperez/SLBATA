using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models.Api_WS
{
    /// <summary>
    /// 
    /// </summary>
    public class Reply
    {
        /// <summary>
        /// si es 1 entonces la consulta es correcta, si es 0 no hubo exito en la consulta
        /// </summary>
        public int result { get; set; }
        /// <summary>
        /// array de tipo cadena
        /// </summary>
        public object data { get; set; }
        /// <summary>
        /// Mensaje depende de la consulta realizada
        /// </summary>
        public string messaje { get; set; }
    }
}