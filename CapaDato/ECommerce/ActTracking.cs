using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;

namespace CapaDato.ECommerce
{
    public class ActTracking
    {

        public string[] ActualizaTrackin(string orden, string tracking)//0=error 1=ok
        {
            //SqlConnection sql;
            //Conexion oConexion = new Conexion();

            MySqlConnection mysql;
            Conexion oConexionMySql = new Conexion();
            string[] ejecuto;
            string result = "";

            using (MySqlCommand cmd = new MySqlCommand())
            {
                try
                {
                    //abrir la conexion
                    mysql = oConexionMySql.getConexionMySQL();
                    mysql.Open();

                    // setear parametros del command
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = mysql;
                    cmd.CommandText = "USP_ActualizaTracking";

                    //asignar paramentros
                    cmd.Parameters.AddWithValue("ref_order", orden);
                    cmd.Parameters.AddWithValue("tracking", tracking);

                    //ejecutar el query
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dr.Read())
                    {
                        result = Convert.ToString(dr["error"]);
                    }
                    //mysql.Close();
                }
                catch (Exception ex)
                {
                    //mysql.Close();
                    ejecuto = new string[] { "0", ex.Message };
                    return ejecuto;
                } // end try

                if (mysql != null)
                    if (mysql.State ==ConnectionState.Open) mysql.Close(); 

            } // end using

            // Evalua el resultado obtenido
            if (result == "1")
            {
                ejecuto = new string[] { result, "Ejecución OK." };
            }
            else
            {
                ejecuto = new string[] { result, "Error al ejecutar SP." };
            }
            return ejecuto;
        }


        /// <summary>
        /// actualizar la guia de urbano a prestashop
        /// </summary>
        public void UpdGuiaUrbano_Prestashop()
        {
            DataTable dtguia_urbano = null;
            try
            {
                Dat_Urbano guias_urbano = new Dat_Urbano();
                dtguia_urbano = guias_urbano.getguiaUrbano();

                if (dtguia_urbano!=null)
                {
                    if (dtguia_urbano.Rows.Count>0)
                    {
                        foreach(DataRow fila in dtguia_urbano.Rows)
                        {
                            string guia_prestashop = fila["Ven_Pst_Ref"].ToString();
                            string guia_urbano = fila["Ven_Guia_Urbano"].ToString();
                            string[] valida= ActualizaTrackin(guia_prestashop, guia_urbano);

                            /*el valor 1 quiere decir que actualizo prestashop*/
                            if (valida[0]=="1")
                            {
                                guias_urbano.updprestashopGuia(guia_prestashop, guia_urbano);
                            }

                        }
                    }
                }


            }
            catch (Exception)
            {

                
            }
        }


        //public static void Main()
        //{
        //    ActTracking exe = new ActTracking();
        //    exe.ActualizaTrackin("XKBKNABJK", "123123");
        //}
    }

    public class UpdaEstado
    {
        //-- Modificado por : Henry Morales - 17/05/2018
        //-- Se cambio la forma de conexión, para obtener datos desde la BD ECOMMERCE
        private static Conexion oConexionWS = new Conexion();
        private static DataTable datosWS = oConexionWS.getConexionWSPresta();
        /*DESARROLLO*/
        //public static string BaseUrl = "http://181.177.242.172/bata/api/";
        //public static string Account = "7UAQDKE187QTB3JT14NLQ3V3XSB6R7HR";


        /*************************/
        /*PRODUCCION*/
        //public static string BaseUrl = "http://bata.com.pe/tienda/api/";
        //public static string Account = "7UAQDKE187QTB3JT14NLQ3V3XSB6R7HR";

        public static string BaseUrl = datosWS.Rows[0]["Url"].ToString().Trim();
        public static string Account = datosWS.Rows[0]["Usuario"].ToString().Trim();

        //public static string BaseUrl = "http://138.197.73.71/tienda/api/";
        //public static string Account = "7UAQDKE187QTB3JT14NLQ3V3XSB6R7HR";

        /****************/
        public static string Password = datosWS.Rows[0]["Contrasena"].ToString().Trim();
        //public static string Password = "";

/*
        public static OrderFactory of = new OrderFactory(BaseUrl, Account, Password);
        public static CartFactory oc = new CartFactory(BaseUrl, Account, Password);
        public static OrderCarrierFactory OrderCarrierFactory = new OrderCarrierFactory(BaseUrl, Account, Password);
        public static StockAvailableFactory sf = new StockAvailableFactory(BaseUrl, Account, Password);
        public static ProductFactory pf = new ProductFactory(BaseUrl, Account, Password);
        public static OrderPaymentFactory opf = new OrderPaymentFactory(BaseUrl, Account, Password);
        public static OrderStateFactory osf = new OrderStateFactory(BaseUrl, Account, Password);
*/

        /// <summary>
        /// Sets the cert policy.
        /// </summary>
        public static void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback
                       += RemoteCertificateValidate;
        }

        
        /// <summary>
        /// Remotes the certificate validate.
        /// </summary>
        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            // trust any certificate!!!
            //System.Console.WriteLine("Warning, trust any certificate");
            return true;
        }
        
        public string[] ActualizaEstadoPS(string orden, int estado)//0=error 1=ok
        {
            //SqlConnection sql;
            //Conexion oConexion = new Conexion();

            MySqlConnection mysql;
            Conexion oConexionMySql = new Conexion();
            string[] ejecuto;
            string result = "";

            using (MySqlCommand cmd = new MySqlCommand())
            {
                try
                {
                    //abrir la conexion
                    mysql = oConexionMySql.getConexionMySQL();
                    mysql.Open();

                    // setear parametros del command
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = mysql;
                    cmd.CommandText = "USP_ActualizaEstadoPS";

                    //asignar paramentros
                    cmd.Parameters.AddWithValue("ref_order", orden);
                    cmd.Parameters.AddWithValue("estado", estado);

                    //ejecutar el query
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dr.Read())
                    {
                        result = Convert.ToString(dr["error"]);
                    }
                    //mysql.Close();
                }
                catch (Exception ex)
                {
                    //mysql.Close();
                    ejecuto = new string[] { "0", ex.Message };
                    return ejecuto;
                } // end try

                if (mysql != null)
                    if (mysql.State == ConnectionState.Open) mysql.Close();

            } // end using

            // Evalua el resultado obtenido
            if (result == "1")
            {
                ejecuto = new string[] { result, "Ejecución OK." };
            }
            else
            {
                ejecuto = new string[] { result, "Error al ejecutar SP." };
            }
            return ejecuto;
        }

        /// <summary>
        /// Procedimiento de Pedido: Actualizar Nro de Guia por Prestashop WebService
        /// </summary>
        /// <param name="id_orden">Id de Orden</param>
        /// <param name="serie_guia">Numero de Guia</param>
        /// <returns>Verdadero/Falso</returns>
        public  bool ActualizarReference(string reference)
        {
            bool result = false;
            try
            {
                // Actualizar Nro Guia - Order Carrier
                string[] actualiza;
                actualiza = ActualizaEstadoPS(reference,18);
                /*Dictionary<string, string> dtn = new Dictionary<string, string>();
                dtn.Add("reference", reference);
                SetCertificatePolicy();
                OrderFactory of = new OrderFactory(BaseUrl, Account, Password);
                order orden = of.GetByFilter(dtn, null, null).FirstOrDefault();

                // Actualizar Estado
                orden.current_state = 18;
                of.Update(orden);
                */
                if (actualiza[0] == "1")
                {
                    result = true;
                }
            }
            catch (Exception exc)
            {
                result = false;
                throw;
            }
            return result;
        }

        //public void act_presta_urbano(string ven_id,ref string error)
        //{
        //    Dat_PrestaShop action_presta = null;
        //    error = "";
        //    try
        //    {
        //        string guia_presta = ""; string guia_urb = "";
        //        action_presta = new Dat_PrestaShop();
        //        action_presta.get_guia_presta_urba(ven_id, ref guia_presta, ref guia_urb);

        //        if (guia_presta.Trim().Length>0)
        //        {                   
        //            Boolean valida = ActualizarReference(guia_presta);

        //            if (valida)
        //            {
        //                action_presta.updestafac_prestashop(guia_presta);
        //            }
        //        }

        //    }
        //    catch (Exception exc)
        //    {
        //        error = exc.Message;
        //    }
        //}

        /// <summary>
        /// actualizar estado prestashop por lista
        /// </summary>
        public void updateestadofac_presta()
        {
            DataTable dt = null;
            try
            {
                Dat_PrestaShop estadofac_presta = new Dat_PrestaShop();
                dt = estadofac_presta.getestadofac();

                if (dt!=null)
                {
                    if (dt.Rows.Count>0)
                    {
                        foreach(DataRow fila in dt.Rows)
                        {
                            string guiaref_presta = fila["Ven_Pst_Ref"].ToString();
                            Boolean valida = ActualizarReference(guiaref_presta);

                            if (valida)
                            {
                                estadofac_presta.updestafac_prestashop(guiaref_presta);
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
