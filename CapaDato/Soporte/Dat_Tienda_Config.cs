using CapaEntidad.Maestros;
using CapaEntidad.Soporte;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Soporte
{
    public class Dat_Tienda_Config
    {
        public List<Ent_ListaTienda> Listar()
        {
            string sqlquery = "USP_GETTDA_PER_ACT";
            List<Ent_ListaTienda> list = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            list = new List<Ent_ListaTienda>();
                            Ent_ListaTienda row = new Ent_ListaTienda();
                            row.cod_entid = "-1";
                            row.des_entid = "--SELECCIONE--";
                            list.Add(row);
                            while (dr.Read())
                            {
                                row = new Ent_ListaTienda();
                                row.cod_entid = dr["cod_entid"].ToString();
                                row.des_entid = dr["des_entid"].ToString();
                                list.Add(row);
                            }
                        }
                    }
                }
            }
            catch
            {
                list = null;
            }
            return list;
        }
        public Ent_Tienda_Config get_config(string codtda)
        {
            string sqlquery = "USP_GetConfig_TDA";
            Ent_Tienda_Config item = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cod_tda", codtda);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item = new Ent_Tienda_Config();
                                string _codigo_interno = dr["codigo_interno"].ToString();
                                string _boleta = dr["boleta"].ToString();
                                string _factura = dr["factura"].ToString();
                                string _nc_boleta = dr["nc_boleta"].ToString();
                                string _nc_factura = dr["nc_factura"].ToString();
                                item.CODIGO_INTERNO = _codigo_interno;
                                item.BOLETA = _boleta;
                                item.FACTURA = _factura;
                                item.NCBOLETA = _nc_boleta;
                                item.NCFACTURA = _nc_factura;
                            }
                        }
                    }
                }
            }
            catch
            {
                item = null;
            }
            return item;
        }
    }
}
