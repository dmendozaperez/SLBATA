using CapaEntidad.ECommerce;
using CapaEntidad.Util;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace CapaDato.ECommerce
{
    public class Dat_VTEX
    {

        //----INICIO---SB-VTEX2020---20201119_12:02----
        public async Task<dynamic> PraparandoTrama_Actualizar_Estado_Invoice(string ven_id)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var client = new HttpClient())
                {
                    //client.BaseAddress = new Uri("https://localhost:44391/");
                    client.BaseAddress = new Uri("http://posperu.bgr.pe/Ws_VTEX/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Add("Authorization", "Basic N0VxcVk1bWRJYlo5Ui83TjlvZVlkdz09OlZLaXl2RmtXOWd3bURseGlPMU9Ob1VMQVNYREhlQnpBZUJYYU12QlpTWmM9");
                    client.DefaultRequestHeaders.Add("Authorization", "Basic MDBYcXNncFVhN0t6N3VYK1N0d0ttZz09OnhPZzMwdEJaOVVoUDNLZ2hEVkN4N1NrYlNnUFlVdnB5TFNxRGN0eFZ0akE9");
                    var content = new StringContent("", Encoding.UTF8, "application/json");
                    response = await client.PostAsync("api/StatusVTEX/ActualizarStatusVTEX_Invoice?venid=" + ven_id, content);
                }
            }
            catch (Exception ex)
            {
                response = null;
            }
            return response;
            //----FIN---SB-VTEX2020---20201119_12:02----
        }

    }
}

