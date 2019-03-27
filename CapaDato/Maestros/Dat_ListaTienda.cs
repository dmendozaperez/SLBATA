using CapaEntidad.Maestros;
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
    public class Dat_ListaTienda
    {
        public List<Ent_ListaTienda> get_tienda(string pais,string xstore="-1")
        {
            string sqlquery = "[USP_GET_XSTORE_TIENDA]";
            List<Ent_ListaTienda> lista = null;
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
                            cmd.Parameters.AddWithValue("@PAIS", pais);
                            cmd.Parameters.AddWithValue("@xstore", xstore);

                            SqlDataReader dr = cmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                lista = new List<Ent_ListaTienda>();
                                while(dr.Read())
                                {
                                    Ent_ListaTienda tienda = new Ent_ListaTienda();
                                    tienda.cod_entid =dr["cod_entid"].ToString();
                                    tienda.des_entid = dr["des_entid"].ToString();
                                    tienda.cod_distri= dr["cod_distri"].ToString();
                                    tienda.xstore =Convert.ToBoolean(dr["xstore"]);
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
    }
}
