using CapaEntidad.Util;
using CapaEntidad.XstoreTda;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.XstoreTda
{
    public class Dat_XCenter
    {
        public Ent_DatosTienda_Xstore get_tienda_xcenter(string tienda)
        {
            string sqlquery = "USP_XSTORE_XCENTER_TIENDA";
            Ent_DatosTienda_Xstore tda_xstore = null;
            DataSet ds = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@COD_TDA", tienda);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            ds = new DataSet();
                            da.Fill(ds);
                            if (ds.Tables.Count>0)
                            {
                                DataTable dt_data_tda = ds.Tables[0];
                                DataTable dt_version_tda = ds.Tables[1];
                                DataTable dt_doc = ds.Tables[2];
                                if (dt_data_tda.Rows.Count>0)
                                {
                                    tda_xstore = new Ent_DatosTienda_Xstore();
                                    foreach(DataRow fila in dt_data_tda.Rows)
                                    {
                                        //tda_xstore.xs_rtl_loc_id = fila["rtl_loc_id"].ToString();
                                        //tda_xstore.xs_description = fila["description"].ToString();
                                        //tda_xstore.xs_address1 = fila["address1"].ToString();
                                        //tda_xstore.xs_store_name = fila["store_name"].ToString();
                                        //tda_xstore.xs_store_manager = fila["store_manager"].ToString();
                                        //tda_xstore.xs_calzado = fila["xs_calzado"].ToString();
                                        //tda_xstore.xs_no_calzado = fila["xs_no_calzado"].ToString();
                                        //tda_xstore.xs_total = fila["xs_total"].ToString();
                                    }
                                }

                            }
                        }
                    }

                }
            }
            catch 
            {
                tda_xstore = null;
            }
            return tda_xstore;
        }
    }
}
