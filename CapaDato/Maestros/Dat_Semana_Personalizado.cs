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
    public class Dat_Semana_Personalizado
    {
        public List<Ent_Semana_Personalizado> listar_semanas()
        {
            List<Ent_Semana_Personalizado> listar = null;
            string sqlquery = "USP_XSTORE_SEMANA_PERSONALIZADO";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            listar = new List<Ent_Semana_Personalizado>();
                            listar = (from DataRow fila in dt.Rows
                                      select new Ent_Semana_Personalizado()
                                      {
                                          cod_semana = fila["cod_Semana"].ToString(),
                                          fec_inicio=fila["fec_inicio"].ToString(),
                                          fec_final =fila["fec_final"].ToString(),
                                          sem_defecto= fila["sem_defecto"].ToString(),
                                      }
                                    ).ToList();
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
