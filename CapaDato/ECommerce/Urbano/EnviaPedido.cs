using CapaDato.ECommerce;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using CapaEntidad.ECommerce;

namespace CapaDato.ECommerce.Urbano
{
    public class EnviaPedido
    {

        public Ent_Urbano sendUrbano(string _ven_id)
        {
            Boolean valida = false;
            DataTable dt = null;
            Ent_Urbano post_data = null;
            try
            {
                post_data = new Ent_Urbano();
                Dat_Urbano data_urbano = new Dat_Urbano();
                Ent_Urbano acceso = data_urbano.get_acceso();

                dt = data_urbano.get_data(_ven_id);
                
                if (dt!=null)
                {
                    if (dt.Rows.Count>0)
                    {
                        /*agrupamos los pedidos*/
                        var grupo_pedido = from item in dt.AsEnumerable()                                            
                                           group item by
                                           new
                                           {
                                               // Modificado por : Henry Morales - 21/05/2018
                                               // Se cambiaron los nombres y telefonos, para ser tomados los de referencia entrega
                                               cod_rastreo = Convert.ToString(item["cod_rastreo"]),
                                               fech_emi_vent = Convert.ToString(item["fech_emi_vent"].ToString()),
                                               nro_guia_trans = Convert.ToString(item["nro_guia_trans"]),
                                               nro_factura = Convert.ToString(item["nro_factura"].ToString()),
                                               cod_empresa = Convert.ToString(item["cod_empresa"].ToString()),
                                               nom_empresa = Convert.ToString(item["nom_empresa"].ToString()),                                           
                                               cod_cliente = Convert.ToString(item["cod_cliente"]),
                                               nom_cliente = Convert.ToString(item["ref_nombre"]),
                                               //nom_cliente = Convert.ToString(item["nom_cliente"]),
                                               nro_telf = Convert.ToString(item["ref_telef"]).Split('/')[0],
                                               //nro_telf = Convert.ToString(item["nro_telf"]),
                                               nro_telf_mobil = Convert.ToString(item["ref_telef"]).Split('/')[1],
                                               //nro_telf_mobil = Convert.ToString(item["nro_telf_mobil"]),
                                               correo_elec = Convert.ToString(item["correo_elec"].ToString()),
                                               dir_entrega = Convert.ToString(item["dir_entrega"].ToString()),
                                               ubi_direc = Convert.ToString(item["ubi_direc"].ToString()),
                                               ref_direc = Convert.ToString(item["ref_direc"].ToString()),
                                               peso_total = Convert.ToDecimal(item["peso_total"].ToString()),
                                               cant_total = Convert.ToDecimal(item["tot_cant"].ToString()),

                                           }
                                       into G
                                           select new
                                           {
                                               cod_rastreo =G.Key.cod_rastreo,
                                               fech_emi_vent = G.Key.fech_emi_vent,
                                               nro_guia_trans = G.Key.nro_guia_trans,
                                               nro_factura = G.Key.nro_factura,
                                               cod_empresa = G.Key.cod_empresa,
                                               nom_empresa = G.Key.nom_empresa,
                                               cod_cliente = G.Key.cod_cliente,
                                               nom_cliente = G.Key.nom_cliente,
                                               nro_telf = G.Key.nro_telf,
                                               nro_telf_mobil = G.Key.nro_telf_mobil,
                                               correo_elec = G.Key.correo_elec,
                                               dir_entrega = G.Key.dir_entrega,
                                               ubi_direc = G.Key.ubi_direc,
                                               ref_direc = G.Key.ref_direc,
                                               peso_total = G.Key.peso_total,
                                               cant_total = G.Key.cant_total,
                                           };
                       

                        /*recorremos los pedidos para agregar al pedido*/
                        foreach (var key in grupo_pedido)
                        {

                            #region<DATOS DEL PEDIDO CAB>
                            GuiaUrbano guiaUrbano = new GuiaUrbano();
                            guiaUrbano.linea = acceso.linea;//Linea, "3", dato de proveedor logístico==>desde la base de datos , estatico
                            guiaUrbano.id_contrato = acceso.contrato;//ID de contrato, 7207, dato de proveedor logístico==>desde la base de datos , estatico
                            guiaUrbano.cod_rastreo = key.cod_rastreo;//Codigo de rastreo, # de orden de Prestashop ==> Referencia de pedido
                            guiaUrbano.cod_barra = key.cod_rastreo;//Codigo de barra, # de orden de Prestashop ==> Referencia de pedido
                            guiaUrbano.fech_emi_vent = key.fech_emi_vent;//Fecha de emisión de venta==> fecha de venta   
                            guiaUrbano.nro_o_compra =key.cod_rastreo;//# de orden de compra==> # de orden de Prestashop ==> Referencia de pedido
                            guiaUrbano.nro_guia_trans =key.nro_guia_trans;//# de guía de transporte==># numero de guia sistemas
                            guiaUrbano.nro_factura = key.nro_factura;//# de factura ==> # numero desde base
                            guiaUrbano.cod_empresa = key.cod_empresa;//RUC BATA ==> # desde base
                            guiaUrbano.nom_empresa = key.nom_empresa;//BATA - Emcomer S.A. ==> # desde base
                            guiaUrbano.cod_cliente = key.cod_cliente;//DNI - RUC cliente ==> # desde base
                            guiaUrbano.nom_cliente = key.nom_cliente;//Nombre de cliente ==> # desde base
                            guiaUrbano.nro_telf =key.nro_telf;//# de teléfono de cliente ==> # desde base
                            guiaUrbano.nro_telf_mobil =key.nro_telf_mobil;//# de celular de cliente ==> # desde base
                            guiaUrbano.correo_elec =key.correo_elec;//Email de cliente ==> # desde base
                            guiaUrbano.dir_entrega = key.dir_entrega;// Dirección de entrega 
                            guiaUrbano.nro_via = "";//# de vía
                            guiaUrbano.nro_int = "";//# de interior
                            guiaUrbano.nom_urb = "";//Nombre de urbanización
                            guiaUrbano.ubi_direc = key.ubi_direc;//Ubigeo dirección entrega
                            guiaUrbano.ref_direc = key.ref_direc;//Referencia dirección entrega
                            guiaUrbano.peso_total = key.peso_total.ToString(); //Peso total, 0.3g por defecto para cada par
                            guiaUrbano.pieza_total = key.cant_total.ToString(); //Cantidad total, No se considera el Envío
                            //guiaUrbano.pieza_total = "3";//# de bultos
                            #endregion


                            var ped_det = from item in dt.AsEnumerable()
                                          where item.Field<string>("cod_rastreo") == Convert.ToString(key.cod_rastreo)                                          
                                          select new
                                          {
                                              cod_sku = item["cod_sku"].ToString(),
                                              descr_sku = Convert.ToString(item["descr_sku"]),
                                              modelo_sku = Convert.ToString(item["modelo_sku"]),
                                              marca_sku = Convert.ToString(item["marca_sku"]),
                                              peso_sku = Convert.ToDecimal(item["peso_sku"]),
                                              cantidad_sku = Convert.ToDecimal(item["cantidad_sku"]),
                                          };
                            List<Ent_Producto> productos_items = new List<Ent_Producto>();
                            foreach (var key_det in ped_det)
                            {

                                Ent_Producto prod_item = new Ent_Producto
                                {
                                    cod_sku = key_det.cod_sku,
                                    descr_sku = key_det.descr_sku,
                                    modelo_sku = key_det.modelo_sku,
                                    marca_sku =key_det.marca_sku,
                                    peso_sku =key_det.peso_sku.ToString(),
                                    cantidad_sku =key_det.cantidad_sku.ToString()
                                };
                                productos_items.Add(prod_item);
                            }
                            guiaUrbano.productos = productos_items;
                            #region<CONSTRUEY CADENA PARA ENVIO A URBANO>
                            //Construyendo cadena
                            string guiaEncoded = "";
                            guiaEncoded += "%22linea%22%3A%22" + guiaUrbano.linea + "%22%2C";
                            guiaEncoded += "%22id_contrato%22%3A%22" + guiaUrbano.id_contrato + "%22%2C";
                            guiaEncoded += "%22cod_rastreo%22%3A%22" + guiaUrbano.cod_rastreo + "%22%2C";
                            guiaEncoded += "%22cod_barra%22%3A%22" + guiaUrbano.cod_barra + "%22%2C";
                            guiaEncoded += "%22fech_emi_vent%22%3A%22" + HttpUtility.UrlEncode(guiaUrbano.fech_emi_vent) + "%22%2C";
                            guiaEncoded += "%22nro_o_compra%22%3A%22" + guiaUrbano.nro_o_compra + "%22%2C";
                            guiaEncoded += "%22nro_guia_trans%22%3A%22" + guiaUrbano.nro_guia_trans + "%22%2C";
                            guiaEncoded += "%22nro_factura%22%3A%22" + guiaUrbano.nro_factura + "%22%2C";
                            guiaEncoded += "%22cod_empresa%22%3A%22" + guiaUrbano.cod_empresa + "%22%2C";
                            //guiaEncoded += "%22nom_empresa%22%3A%22" + HttpUtility.UrlEncode(guiaUrbano.nom_empresa) + "%22%2C";
                            guiaEncoded += "%22cod_cliente%22%3A%22" + guiaUrbano.cod_cliente + "%22%2C";
                            guiaEncoded += "%22nom_cliente%22%3A%22" + HttpUtility.UrlEncode(guiaUrbano.nom_cliente) + "%22%2C";
                            guiaEncoded += "%22nro_telf%22%3A%22" + guiaUrbano.nro_telf + "%22%2C";
                            guiaEncoded += "%22nro_telf_mobil%22%3A%22" + guiaUrbano.nro_telf_mobil + "%22%2C";
                            guiaEncoded += "%22correo_elec%22%3A%22" + HttpUtility.UrlEncode(guiaUrbano.correo_elec) + "%22%2C";
                            guiaEncoded += "%22dir_entrega%22%3A%22" + HttpUtility.UrlEncode(guiaUrbano.dir_entrega) + "%22%2C";
                            guiaEncoded += "%22nro_via%22%3A%22" + guiaUrbano.nro_via + "%22%2C";
                            guiaEncoded += "%22nro_int%22%3A%22" + guiaUrbano.nro_int + "%22%2C";
                            guiaEncoded += "%22nom_urb%22%3A%22" + guiaUrbano.nom_urb + "%22%2C";
                            guiaEncoded += "%22ubi_direc%22%3A%22" + guiaUrbano.ubi_direc + "%22%2C";
                            guiaEncoded += "%22ref_direc%22%3A%22" + HttpUtility.UrlEncode(guiaUrbano.ref_direc) + "%22%2C";
                            guiaEncoded += "%22peso_total%22%3A%22" + guiaUrbano.peso_total + "%22%2C";
                            guiaEncoded += "%22pieza_total%22%3A%22" + guiaUrbano.pieza_total + "%22%2C";

                            //Construyendo cadena desde lista de productos
                            string productos = "";
                            int i = 1;
                            foreach (Ent_Producto p in guiaUrbano.productos)
                            {
                                productos += "%22cod_sku%22%3A%22" + p.cod_sku + "%22%2C";
                                productos += "%22descr_sku%22%3A%22" + HttpUtility.UrlEncode(p.descr_sku) + "%22%2C";
                                productos += "%22modelo_sku%22%3A%22" + p.modelo_sku + "%22%2C";
                                productos += "%22marca_sku%22%3A%22" + HttpUtility.UrlEncode(p.marca_sku) + "%22%2C";
                                productos += "%22peso_sku%22%3A%22" + p.peso_sku + "%22%2C";
                                productos += "%22cantidad_sku%22%3A%22" + p.cantidad_sku + "%22";
                                if (i != guiaUrbano.productos.Count)
                                {
                                    productos += "%7D%2C%7B";
                                }
                                i++;
                            }

                            //Cadena final
                            string content = "json=%7B" + guiaEncoded + "%22productos%22%3A%5B%7B" + productos + "%7D%5D%7D";

                            //Llamando al webservice de Urbano
                            using (WebClient client = new WebClient())
                            {
                                client.Headers.Add("Content-type", "application/x-www-form-urlencoded");
                                client.Headers.Add("user", acceso.usuario);
                                client.Headers.Add("pass", acceso.password);

                                var response = client.UploadString(acceso.url, "POST", content);

                                Ent_Urbano post = JsonConvert.DeserializeObject<Ent_Urbano>(response);


                                //en este paso envia si es que urbano recibio los datos con exito
                                post_data = post;
                                //if (post.error=="1")
                               // {
                               //     if (post.guia.Trim().Length>0)
                                //        data_urbano.update_guia(key.cod_rastreo, post.guia);
                                //}

                                //Console.WriteLine(post.guia);



                                //Console.ReadLine();
                            }
                            #endregion


                        }


                    }
                }

            }
            catch (Exception exc)
            {
                post_data = null;                                
            }
            return post_data ;
        }
        public Boolean send()
        {
            Boolean valida = false;
            try
            {
                string usuario = "Ws.pr33b4";
                string password = "186b2a62e7b6fc61144a1a7cc6bbbabce6f386b5";
                string url = "http://app.urbano.com.pe:8000/ws/ue/ge";

                GuiaUrbano guiaUrbano = new GuiaUrbano
                {
                    linea = "3",//Linea, "3", dato de proveedor logístico==>desde la base de datos , estatico
                    id_contrato = "7207",//ID de contrato, 7207, dato de proveedor logístico==>desde la base de datos , estatico
                    cod_rastreo = "1000145107001",//Codigo de rastreo, # de orden de Prestashop ==> Referencia de pedido
                    cod_barra = "1000145107001",//Codigo de barra, # de orden de Prestashop ==> Referencia de pedido
                    fech_emi_vent = "15/12/2017",//Fecha de emisión de venta==> fecha de venta   
                    nro_o_compra = "1024319",//# de orden de compra==> # de orden de Prestashop ==> Referencia de pedido
                    nro_guia_trans = "1254",//# de guía de transporte==># numero de guia sistemas
                    nro_factura = "999999",//# de factura ==> # numero desde base
                    cod_empresa = "20123456781",//RUC BATA ==> # desde base
                    nom_empresa = "BATA",//BATA - Emcomer S.A. ==> # desde base
                    cod_cliente = "45678912",//DNI - RUC cliente ==> # desde base
                    nom_cliente = "Jorge Solis Mc Lellan",//Nombre de cliente ==> # desde base
                    nro_telf = "",//# de teléfono de cliente ==> # desde base
                    nro_telf_mobil = "987654321",//# de celular de cliente ==> # desde base
                    correo_elec = "jorge.solis@tawa.com.pe",//Email de cliente ==> # desde base
                    dir_entrega = "Av. Lima 944",// Dirección de entrega 
                    nro_via = "1111",//# de vía
                    nro_int = "2121",//# de interior
                    nom_urb = "",//Nombre de urbanización
                    ubi_direc = "150120",//Ubigeo dirección entrega
                    ref_direc = "Alt. cdra 10 Av. San Miguel",//Referencia dirección entrega
                    peso_total = "1",//Peso total, 0.3g por defecto para cada par
                    pieza_total = "3",//# de bultos

                    //Añadiendo productos
                    productos = new List<Ent_Producto> {
                    new Ent_Producto { cod_sku = "8451230-30", descr_sku = "Zapato de cuero negro talla 30", modelo_sku = "8451230", marca_sku = "BATA", peso_sku = "0.3", cantidad_sku = "1" },
                    new Ent_Producto { cod_sku = "8451240-42", descr_sku = "Zapato marrón talla 42", modelo_sku = "8451240", marca_sku ="BATA", peso_sku = "0.3", cantidad_sku = "1" },
                    new Ent_Producto { cod_sku = "8451250-45", descr_sku = "Zapato de gamuza azul talla 45", modelo_sku = "8451250", marca_sku = "BATA", peso_sku = "0.3", cantidad_sku = "1" },
                }
                };
                //Construyendo cadena
                string guiaEncoded = "";
                guiaEncoded += "%22linea%22%3A%22" + guiaUrbano.linea + "%22%2C";
                guiaEncoded += "%22id_contrato%22%3A%22" + guiaUrbano.id_contrato + "%22%2C";
                guiaEncoded += "%22cod_rastreo%22%3A%22" + guiaUrbano.cod_rastreo + "%22%2C";
                guiaEncoded += "%22cod_barra%22%3A%22" + guiaUrbano.cod_barra + "%22%2C";
                guiaEncoded += "%22fech_emi_vent%22%3A%22" + HttpUtility.UrlEncode(guiaUrbano.fech_emi_vent) + "%22%2C";
                guiaEncoded += "%22nro_o_compra%22%3A%22" + guiaUrbano.nro_o_compra + "%22%2C";
                guiaEncoded += "%22nro_guia_trans%22%3A%22" + guiaUrbano.nro_guia_trans + "%22%2C";
                guiaEncoded += "%22nro_factura%22%3A%22" + guiaUrbano.nro_factura + "%22%2C";
                guiaEncoded += "%22cod_empresa%22%3A%22" + guiaUrbano.cod_empresa + "%22%2C";
                guiaEncoded += "%22nom_empresa%22%3A%22" + HttpUtility.UrlEncode(guiaUrbano.nom_empresa) + "%22%2C";
                guiaEncoded += "%22cod_cliente%22%3A%22" + guiaUrbano.cod_cliente + "%22%2C";
                guiaEncoded += "%22nom_cliente%22%3A%22" + HttpUtility.UrlEncode(guiaUrbano.nom_cliente) + "%22%2C";
                guiaEncoded += "%22nro_telf%22%3A%22" + guiaUrbano.nro_telf + "%22%2C";
                guiaEncoded += "%22nro_telf_mobil%22%3A%22" + guiaUrbano.nro_telf_mobil + "%22%2C";
                guiaEncoded += "%22correo_elec%22%3A%22" + HttpUtility.UrlEncode(guiaUrbano.correo_elec) + "%22%2C";
                guiaEncoded += "%22dir_entrega%22%3A%22" + HttpUtility.UrlEncode(guiaUrbano.dir_entrega) + "%22%2C";
                guiaEncoded += "%22nro_via%22%3A%22" + guiaUrbano.nro_via + "%22%2C";
                guiaEncoded += "%22nro_int%22%3A%22" + guiaUrbano.nro_int + "%22%2C";
                guiaEncoded += "%22nom_urb%22%3A%22" + guiaUrbano.nom_urb + "%22%2C";
                guiaEncoded += "%22ubi_direc%22%3A%22" + guiaUrbano.ubi_direc + "%22%2C";
                guiaEncoded += "%22ref_direc%22%3A%22" + HttpUtility.UrlEncode(guiaUrbano.ref_direc) + "%22%2C";
                guiaEncoded += "%22peso_total%22%3A%22" + guiaUrbano.peso_total + "%22%2C";
                guiaEncoded += "%22pieza_total%22%3A%22" + guiaUrbano.pieza_total + "%22%2C";

                //Construyendo cadena desde lista de productos
                string productos = "";
                int i = 1;
                foreach (Ent_Producto p in guiaUrbano.productos)
                {
                    productos += "%22cod_sku%22%3A%22" + p.cod_sku + "%22%2C";
                    productos += "%22descr_sku%22%3A%22" + HttpUtility.UrlEncode(p.descr_sku) + "%22%2C";
                    productos += "%22modelo_sku%22%3A%22" + p.modelo_sku + "%22%2C";
                    productos += "%22marca_sku%22%3A%22" + HttpUtility.UrlEncode(p.marca_sku) + "%22%2C";
                    productos += "%22peso_sku%22%3A%22" + p.peso_sku + "%22%2C";
                    productos += "%22cantidad_sku%22%3A%22" + p.cantidad_sku + "%22";
                    if (i != guiaUrbano.productos.Count)
                    {
                        productos += "%7D%2C%7B";
                    }
                    i++;
                }

                //Cadena final
                string content = "json=%7B" + guiaEncoded + "%22productos%22%3A%5B%7B" + productos + "%7D%5D%7D";

                //Llamando al webservice de Urbano
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("Content-type", "application/x-www-form-urlencoded");
                    client.Headers.Add("user", usuario);
                    client.Headers.Add("pass", password);

                    var response = client.UploadString(url, "POST", content);

                    Ent_Urbano post = JsonConvert.DeserializeObject<Ent_Urbano>(response);

                    Console.WriteLine(post.guia);

                    Console.ReadLine();
                }

            }
            catch (Exception)
            {
                valida = false;                
            }
            return valida;
        }








    }
}
