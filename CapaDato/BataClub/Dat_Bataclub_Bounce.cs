using CapaEntidad.BataClub;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.BataClub
{
    public class Dat_Bataclub_Bounce
    {
        public List<Ent_Bataclub_Bounce> listar_bounce(DateTime fecha_ini,DateTime fecha_fin)
        {
            string sqlquery = "USP_BATACLUB_REPORTE_BOUNCE";
            List<Ent_Bataclub_Bounce> listar = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.Parameters.AddWithValue("@fecha_ini", fecha_ini);
                        cmd.Parameters.AddWithValue("@fecha_fin", fecha_fin);
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            listar = new List<Ent_Bataclub_Bounce>();
                            listar = (from DataRow fila in dt.Rows
                                      select new Ent_Bataclub_Bounce
                                      {
                                          RecordType=fila["RecordType"].ToString(),
                                          MessageID = fila["MessageID"].ToString(),
                                          Details = fila["Details"].ToString(),
                                          Email = fila["Email"].ToString(),
                                          BouncedAt = fila["BouncedAt"].ToString(),
                                          Subject = fila["Subject"].ToString(),
                                          Canal = fila["Canal"].ToString(),
                                          Dni = fila["Dni"].ToString(),
                                          FecRegistro = fila["FecRegistro"].ToString(),
                                          Tienda = fila["Tienda"].ToString(),
                                      
                                      }
                                    ).ToList();
                        }
                    }
                }
            }
            catch 
            {
                listar = new List<Ent_Bataclub_Bounce>();
            }
            return listar;
        }
    }
}
