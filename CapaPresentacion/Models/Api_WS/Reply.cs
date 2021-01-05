using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models.Api_WS
{
    public class Reply
    {
        public int result { get; set; }
        public object data { get; set; }
        public string messaje { get; set; }
    }
}