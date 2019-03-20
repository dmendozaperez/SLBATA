using CapaEntidad.Util;
using Models.Crystal.Reporte;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Data.Crystal.Reporte
{
    public class Data_Bata
    {
        public List<Models_Art_Sin_Mov> list_art_sin_mov(string cadena,string tienda,Int32 nsemana,Int32 maxpares)
        {
            string sqlquery = "USP_XSTORE_REPORTE_ART_SIN_MOVIMIENTOS";
            List<Models_Art_Sin_Mov> lista = null;
            DataTable dt = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@cadena", cadena);
                            cmd.Parameters.AddWithValue("@codtda", tienda);
                            cmd.Parameters.AddWithValue("@nsemanas", nsemana);
                            cmd.Parameters.AddWithValue("@nstock", maxpares);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                dt = new DataTable();
                                da.Fill(dt);
                                lista = new List<Models_Art_Sin_Mov>();
                                lista = (from DataRow dr in dt.Rows
                                         select new Models_Art_Sin_Mov()
                                         {
                                             tiend=dr["tiend"].ToString(),
                                             des_entid= dr["des_entid"].ToString(),
                                             store_name = dr["storename"].ToString(),
                                             semana_str = dr["semana_str"].ToString(),
                                             cate3 = dr["cate3"].ToString(),
                                             subc3 = dr["subc3"].ToString(),
                                             artic = dr["artic"].ToString(),
                                             pplan =Convert.ToDecimal(dr["pplan"]),
                                             pares =Convert.ToInt32(dr["pares"]),
                                             stock =Convert.ToInt32(dr["stock"]),
                                         }).ToList();
                            }
                        }
                    }
                    catch 
                    {
                        
                    }
                }
            }
            catch 
            {
                
            }
            return lista;
        }
    }
}