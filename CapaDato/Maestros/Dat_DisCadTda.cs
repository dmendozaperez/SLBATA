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
    public class Dat_DisCadTda
    {
        public List<Ent_Combo_DisCadTda> list_dis_cad_tda(string Pais)
        {
            string sqlquery = "USP_XSTORE_GET_DISTRITO_CAD_TDA";
            List<Ent_Combo_DisCadTda> listar = null;
            DataTable dt = null;
            try
            {
                listar = new List<Ent_Combo_DisCadTda>();
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pais", Pais);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dt = new DataTable();
                            da.Fill(dt);
                            listar = (from DataRow fila in dt.Rows
                                      select new Ent_Combo_DisCadTda()
                                      {
                                          cod_distri = fila["cod_distri"].ToString(),
                                          des_distri = fila["des_distri"].ToString(),
                                          cod_cadena = fila["cod_cadena"].ToString(),
                                          des_cadena = fila["des_cadena"].ToString(),
                                          cod_entid = fila["cod_entid"].ToString(),
                                          des_entid = fila["des_entid"].ToString(),
                                      }).ToList();
                                    
                        }
                    }
                }
            }
            catch 
            {

                
            }
            return listar;
        }
    }
}
