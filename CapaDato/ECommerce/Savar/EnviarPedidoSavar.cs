using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad.Util;


namespace CapaDato.ECommerce.Savar
{
    public class EnviarPedidoSavar
    {
        public DataTable get_Ventas_por_Savar(string ven_id)
        {
            string sqlquery = "USP_ECOMMERCE_LISTA_SAVAR";
            DataTable dtSavar = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0) cn.Open();
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ven_id", ven_id);

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            if (ds.Tables.Count > 0)
                            {
                                dtSavar = ds.Tables[0];
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                dtSavar = null;
            }
            return dtSavar;
        }

    }
}
