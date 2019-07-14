using CapaEntidad.ECommerce;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using CapaDato.ECommerce;

namespace CapaDato.comercioxpress
{
    public class EnviaPedidoCxpress
    {

        /*usemos el web service de comercioespress*/
        WSOrdenServicioClient obj1 = null;
        //Integrado.comercioxpress.OrdenServicioReqParm objcla = new comercioxpress.OrdenServicioReqParm();
        public string casa = "";

        //public Reg_PedidoCxpress()
        
        public Ent_Cexpress sendCexpress(string _ven_id, ref string nroserv)
        {
            
            /*Inicio Carga de Data para Envio*/
            Boolean valida = false;
            DataTable dt = null;
            Ent_Cexpress post_data = null;
            /*Fin Carga*/
            
            obj1 = new WSOrdenServicioClient();
            EnviaPedidoCxpress d = new EnviaPedidoCxpress();
            OrdenServicioReqParm objcla = new OrdenServicioReqParm();
            WSOrdenServicioClient dd = new WSOrdenServicioClient();
            EnviaPedidoCxpress s = new EnviaPedidoCxpress();
            //s.
            //dd.
            //  var r = obj1.registrar(dd);

            // //obj1.re
            // Boolean valida = false;
            ////DataTable dt = null;
            //Ent_Cexpress post_data = null;
            try
            {

                /*inicio Data */
                post_data = new Ent_Cexpress();

                Dat_Cexpress data_Cexpress = new Dat_Cexpress();
                //Ent_Urbano acceso = data_urbano.get_acceso();

                dt = data_Cexpress.get_data(_ven_id);
               ;

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        /*agrupamos los pedidos*/
                        var grupo_pedido = from item in dt.AsEnumerable()
                                           group item by
                                           new
                                           {
                                               cod_rastreo = Convert.ToString(item["cod_rastreo"]),
                                               fech_emi_vent = Convert.ToString(item["fech_emi_vent"].ToString()),
                                               nro_guia_trans = Convert.ToString(item["nro_guia_trans"]),
                                               nro_factura = Convert.ToString(item["nro_factura"].ToString()),
                                               cod_empresa = Convert.ToString(item["cod_empresa"].ToString()),
                                               nom_empresa = Convert.ToString(item["nom_empresa"].ToString()),
                                               Cod_Pto_Recojo = Convert.ToString(item["Cod_Pto_Recojo"].ToString()),
                                               cod_cliente = Convert.ToString(item["cod_cliente"]),
                                               nom_cliente = Convert.ToString(item["nom_cliente"]),
                                               ape_cliente = Convert.ToString(item["ape_cliente"]),
                                               tipdoc_clie = Convert.ToString(item["tipdoc_clie"]),
                                               nro_telf = Convert.ToString(item["ref_telef"]).Split('/')[0],
                                               nro_telf_mobil = Convert.ToString(item["ref_telef"]).Split('/')[1],
                                               correo_elec = Convert.ToString(item["correo_elec"].ToString()),
                                               dir_entrega = Convert.ToString(item["dir_entrega"].ToString()),
                                               ubi_direc = Convert.ToString(item["ubi_direc"].ToString()),
                                               ref_direc = Convert.ToString(item["ref_direc"].ToString()),
                                               peso_total = Convert.ToDecimal(item["peso_total"].ToString()),
                                               cant_total = Convert.ToInt32(item["tot_cant"].ToString()),

                                           }
                                           into G
                                           select new
                                           {
                                               cod_rastreo = G.Key.cod_rastreo,
                                               fech_emi_vent = G.Key.fech_emi_vent,
                                               nro_guia_trans = G.Key.nro_guia_trans,
                                               nro_factura = G.Key.nro_factura,
                                               cod_empresa = G.Key.cod_empresa,
                                               nom_empresa = G.Key.nom_empresa,
                                               Cod_Pto_Recojo = G.Key.Cod_Pto_Recojo,
                                               cod_cliente = G.Key.cod_cliente,
                                               nom_cliente = G.Key.nom_cliente,
                                               ape_cliente = G.Key.ape_cliente,
                                               tipdoc_clie = G.Key.tipdoc_clie,
                                               nro_telf = G.Key.nro_telf,
                                               nro_telf_mobil = G.Key.nro_telf_mobil,
                                               correo_elec = G.Key.correo_elec,
                                               dir_entrega = G.Key.dir_entrega,
                                               ubi_prov = G.Key.ubi_direc.Substring(2,2),
                                               ubi_dist = G.Key.ubi_direc.Substring(4, 2),
                                               ref_direc = G.Key.ref_direc,
                                               peso_total = G.Key.peso_total,
                                               cant_total = G.Key.cant_total,
                                           };
                                        /*fin cab*/


                        /*inicio*/

                        /*recorremos los pedidos para agregar al pedido*/
                        foreach (var key in grupo_pedido)
                        {
                            /* inicio cab*/
                            //string[] nroPedido = { "F09500000013" };
                            //objcla.nroPedido = new String[] { "Ped-1234567" };// nroPedido;
                            //objcla.nroPedido = new String[] { _ven_id };// nroPedido;
                            //objcla.nroPedido = new String[] { _gia_presta };// nroPedido;
                            //objcla.nroPedido = new String[] { key.nro_guia_trans };// nroPedido;    ok
                            objcla.nroPedido = new String[] { key.nro_guia_trans };// nroPedido;    ok


                            var ped_det = from item in dt.AsEnumerable()
                                          where item.Field<string>("cod_rastreo") == Convert.ToString(key.cod_rastreo)
                                          select new
                                          {
                                              cod_sku = item["cod_sku"].ToString(),
                                              descr_sku = Convert.ToString(item["descr_sku"]),
                                              modelo_sku = Convert.ToString(item["modelo_sku"]),
                                              marca_sku = Convert.ToString(item["marca_sku"]),
                                              peso_sku = Convert.ToInt32(item["peso_sku"]),
                                              cantidad_sku = Convert.ToInt32(item["cantidad_sku"]),
                                          };


                            ///*Inicio Det Item*/
                            List<CapaDato.comercioxpress.item> lista = new List<CapaDato.comercioxpress.item>();

                            List<Ent_Producto> productos_items = new List<Ent_Producto>();
                            foreach (var key_det in ped_det)
                            {

                                CapaDato.comercioxpress.item objdet = new CapaDato.comercioxpress.item();
                                objdet.descItem = new String[] { key_det.cod_sku };
                                objdet.cantItem = new int[] { key_det.cantidad_sku };
                                objdet.pesoMasa = new float[] { key_det.peso_sku };
                                objdet.altoItem = new float[] { 1 };
                                objdet.largoItem = new float[] { 1 };
                                objdet.anchoItem = new float[] { 1  };
                                objdet.valorItem = new float[] { 1 };
                                lista.Add(objdet);
                            }

                            objcla.listaItems = lista.ToArray();
                            obj1.registrar(objcla);
                            ///*Info*/




                            // Use StringBuilder for concatenation in tight loops.
                            //var sb = new System.Text.StringBuilder();
                            //for (int i = 0; i < 20; i++)
                            //{
                            //    sb.AppendLine(i.ToString());
                            //}
                            //System.Console.WriteLine(sb.ToString());




                            ///*Inicio Det Item*/
                            //List<Integrado.comercioxpress.item> lista = new List<comercioxpress.item>();

                            //Integrado.comercioxpress.item objdet = new Integrado.comercioxpress.item();
                            //objdet.descItem = new String[] { vdescr_sku.ToString() };
                            //objdet.cantItem = new int[] { 1, 1, 8};
                            //objdet.pesoMasa = new float[] { 1, 5, 6 };
                            //objdet.altoItem = new float[] { 1, 2, 3 };
                            //objdet.largoItem = new float[] { 1, 2, 3 };
                            //objdet.anchoItem = new float[] { 1, 2, 3  };
                            //objdet.valorItem = new float[] { 0, 2, 3 };
                            //lista.Add(objdet);

                            //objcla.listaItems = lista.ToArray();
                            //obj1.registrar(objcla);
                            ///*Info*/


                            objcla.volumen = new double[] { 10 };           //No hay 
                            objcla.tipoServicio = new long[] { 101 };       // 
                            /*Codigos para prueba 141 y  142*/
                            objcla.codCliente = new long[] { 467 };         //entregado por CX 141
                            objcla.codCtaCliente = new long[] { 493 };      //entregado por CX 142
                            objcla.cantPiezas = new int[] { key.cant_total };
                            objcla.codRef1 = new String[] { "0012071801" }; //opsional
                            objcla.codRef2 = new String[] { "0012071801" }; //opsional
                            objcla.valorProducto = new double[] { 1 };
                            objcla.tipoOrigenRecojo = new int[] { 1 };
                            objcla.codTipoDocProveedor = new long[] { 112 };    //entregado por CX
                            /*Para nroDocProveedor 20145556666*/
                            objcla.nroDocProveedor = new String[] { "20101951872" };   ///20145556666
                            /*Para codDireccionProveedor 900055*/
                            objcla.codDireccionProveedor = new long[] { Convert.ToInt32(key.Cod_Pto_Recojo) };  //entregado por CX 0900055
                            objcla.indicadorGeneraRecojo = new int[] { 1 };
                            objcla.tipoDestino = new int[] { 1 };
                            objcla.direccEntrega = new String[] { key.dir_entrega };  // Dirección de entrega
                            //Ubigeo dirección entrega  key.ubi_direc
                            objcla.refDireccEntrega = new String[] { key.ref_direc }; //Referencia dirección entrega
                            objcla.codDepartEntrega = new String[] { "15" }; //Departamento = Lima
                            objcla.codProvEntrega = new String[] { key.ubi_prov }; //Provincia = Lima
                            objcla.codDistEntrega = new String[] { key.ubi_dist }; 
                            objcla.nomDestEntrega = new String[] { key.nom_cliente };
                            objcla.apellDestEntrega = new String[] { key.ape_cliente };  //"Perez Luna"
                            objcla.codTipoDocDestEntrega = new String[] { key.tipdoc_clie };
                            objcla.nroDocDestEntrega = new String[] { key.cod_cliente };    //"12345678"
                            objcla.telefDestEntrega = new String[] { key.nro_telf_mobil };        //"991276768"
                            objcla.emailDestEntrega = new String[] { key.correo_elec };     //"juanperez@gmail.com"
                            objcla.idUsuario = new String[] { "EMPRESA  S.A.C." };
                            objcla.deTerminal = new String[] { "LIMA" };

                        }
                        /*fin*/


                        /*var e = dd.registrar(objcla);*/
                        response e = dd.registrar(objcla);

                        nroserv = e.nroOrdenServicio.ToString();
                        //nroserv = e.listaPiezas[0].nuPieza.ToString();


                    }
                }
            }
            catch (Exception exc)
            {
                //_error = exc.Message;
                post_data = null;
            }

            //catch (Exception)
            //{
            //    post_data=null;
            //}
            return post_data;
        }

    }
}
