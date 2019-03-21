using CapaEntidad.XstoreTda;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Maestros
{
    public class Dat_XstoreTienda
    {
        public List<Ent_TiendaConf> List_Tienda_config()
        {
            string sqlquery = "USP_LISTAR_TIENDA";
            List<Ent_TiendaConf> lista = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    try
                    {
                        if (cn.State == 0) cn.Open();
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                          
                            SqlDataReader dr = cmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                lista = new List<Ent_TiendaConf>();
                                while(dr.Read())
                                {
                                    Ent_TiendaConf tienda = new Ent_TiendaConf();
                                    tienda.cod_Entid = dr["COD_ENTID"].ToString();
                                    tienda.des_Entid = dr["DES_ENTID"].ToString();
                                    tienda.cod_Emp = dr["COD_EMP"].ToString();
                                    tienda.des_Emp = dr["DES_EMP"].ToString(); 
                                    tienda.des_Cadena = dr["DES_CAD"].ToString();
                                    tienda.direccion = dr["DIRECCION"].ToString();
                                    tienda.cod_Jefe = dr["COD_JEFE"].ToString();
                                    tienda.consecionario = dr["CONSECIONARIO"].ToString();
                                    tienda.bol_xstore = dr["XSTORE"].ToString();
                                    lista.Add(tienda);
                                }
                            }

                        }
                    }
                    catch (Exception)
                    {
                        lista = null;
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch
            {
                lista = null;
            }
            return lista;
        }

        public int ActualizarTiendaXstore(string codTienda, Int32 Estado, decimal usuario) {

            Int32 intRespuesta = 0;

            string sqlquery = "USP_ACTIVAR_TIENDA_XSTORE";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try { 

                cn = new SqlConnection(Ent_Conexion.conexion);
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodTienda", codTienda);
                cmd.Parameters.AddWithValue("@Estado", Estado);
                cmd.Parameters.AddWithValue("@usuUpd", usuario);
                cmd.ExecuteNonQuery();
                intRespuesta = 1;

            }
            catch (Exception ex) {

                intRespuesta = -1;

            }

            return intRespuesta;
        }
    }
}
