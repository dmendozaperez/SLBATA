using CapaEntidad.Bata;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Bata
{
    public class Dat_Bata_Sac
    {
        public Ent_Bata_Sac Get_Bata_Sac(string dni)
        {
            string sqlquery = "USP_BATA_SAC_CONSULTA_DNI";
            Ent_Bata_Sac sac = null;
            try
            {
                sac = new Ent_Bata_Sac();
                List<Ent_Bata_Sac_Cliente> sac_cliente = null;
                List <Ent_Bata_Sac_Cupones> sac_cupones = null;
                List <Ent_Bata_Sac_Venta> sac_venta = null;

                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DNI", dni);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);

                            DataTable dt_sac_cliente = ds.Tables[0];
                            DataTable dt_sac_cupones = ds.Tables[1];
                            DataTable dt_sac_venta = ds.Tables[2];

                            sac_cliente = new List<Ent_Bata_Sac_Cliente>();
                            sac_cliente = (from DataRow fila in dt_sac_cliente.Rows
                                           select new Ent_Bata_Sac_Cliente()
                                           {
                                               dni= fila["dni"].ToString(),
                                               nombre = fila["nombre"].ToString(),
                                               correo = fila["correo"].ToString(),
                                               telefono = fila["telefono"].ToString(),
                                               ubicacion = fila["ubicacion"].ToString(),
                                               fec_nac = fila["fec_nac"].ToString(),
                                               bataclub = fila["bataclub"].ToString(),                                               

                                           }
                                         ).ToList();

                            sac_cupones = new List<Ent_Bata_Sac_Cupones>();
                            sac_cupones = (from DataRow fila in dt_sac_cupones.Rows
                                           select new Ent_Bata_Sac_Cupones()
                                           {
                                               barra = fila["barra"].ToString(),
                                               promocion = fila["promocion"].ToString(),
                                               fecha_expiracion = fila["fecha_expiracion"].ToString(),                                             
                                           }
                                         ).ToList();

                            sac_venta = new List<Ent_Bata_Sac_Venta>();
                            sac_venta = (from DataRow fila in dt_sac_venta.Rows
                                           select new Ent_Bata_Sac_Venta()
                                           {
                                               cod_tda = fila["cod_tda"].ToString(),
                                               canal = fila["canal"].ToString(),
                                               tienda = fila["tienda"].ToString(),
                                               tipodoc = fila["tipodoc"].ToString(),
                                               numdoc = fila["numdoc"].ToString(),
                                               fecha = fila["fecha"].ToString(),
                                               estado = fila["estado"].ToString(),
                                               pedido = fila["pedido"].ToString(),
                                               fc_suna = fila["fc_suna"].ToString(),
                                               fc_sfac = fila["fc_sfac"].ToString(),
                                               fc_nfac = fila["fc_nfac"].ToString(),
                                               fc_nint = fila["fc_nint"].ToString(),
                                           }
                                         ).ToList();

                            sac.Bata_Sac_Cliente = sac_cliente;
                            sac.Bata_Sac_Cupones = sac_cupones;
                            sac.Bata_Sac_Venta = sac_venta;
                        }
                    }
                }
            }
            catch (Exception)
            {
                sac = new Ent_Bata_Sac();

            }
            return sac;
        }

        public List<Ent_Bata_Sac_Venta_Detalle> Get_Venta_detalle(string canal,string cod_tda,string fc_nint,string numdoc,string pedido)
        {
            string sqlquery = "USP_BATA_SAC_CONSULTA_VENTA_DETALLE";
            List<Ent_Bata_Sac_Venta_Detalle> lista=null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CANAL",canal);
                        cmd.Parameters.AddWithValue("@COD_TDA", cod_tda);
                        cmd.Parameters.AddWithValue("@FC_NINT", fc_nint);
                        cmd.Parameters.AddWithValue("@NUMDOC", numdoc);
                        cmd.Parameters.AddWithValue("@PEDIDO", pedido);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            lista = new List<Ent_Bata_Sac_Venta_Detalle>();
                            lista = (from DataRow fila in dt.Rows
                                     select new Ent_Bata_Sac_Venta_Detalle
                                     {
                                         articulo = fila["articulo"].ToString(),
                                         talla = fila["talla"].ToString(),
                                         cantidad =Convert.ToInt32(fila["cantidad"]),
                                         precio =Convert.ToDecimal(fila["precio"]),
                                         descuento =Convert.ToDecimal(fila["descuento"]),
                                         total_linea =Convert.ToDecimal(fila["total_linea"]),
                                         promocion= fila["promocion"].ToString(),
                                     }
                                   ).ToList();
                        }
                    }
                }
            }
            catch 
            {
                lista = new List<Ent_Bata_Sac_Venta_Detalle>();
            }
            return lista;
        }
    }
}
