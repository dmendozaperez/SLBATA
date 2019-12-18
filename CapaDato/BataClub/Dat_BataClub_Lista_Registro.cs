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
    public class Dat_BataClub_Lista_Registro
    {
        public List<Ent_BataClub_Lista_Registro> lista_registro(DateTime fec_ini,DateTime fec_fin,string dni,string correo)
        {
            string sqlquery = "USP_BATACLUB_CONSULTA_CLIENTES";
            List<Ent_BataClub_Lista_Registro> listar = null;
            DataTable dt = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType =CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fec_ini", fec_ini);
                        cmd.Parameters.AddWithValue("@fec_fin", fec_fin);
                        cmd.Parameters.AddWithValue("@dni", dni);
                        cmd.Parameters.AddWithValue("@correo", correo);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dt = new DataTable();
                            da.Fill(dt);
                            listar = new List<Ent_BataClub_Lista_Registro>();
                            listar = (from DataRow dr in dt.Rows
                                      select new Ent_BataClub_Lista_Registro()
                                      {
                                          canal=dr["canal"].ToString(),
                                          tienda = dr["tienda"].ToString(),
                                          dni = dr["dni"].ToString(),
                                          primer_nombre = dr["primer_nombre"].ToString(),
                                          segundo_nombre = dr["segundo_nombre"].ToString(),
                                          apellido_pat = dr["apellido_pat"].ToString(),
                                          apellido_mat = dr["apellido_mat"].ToString(),
                                          genero = dr["genero"].ToString(),
                                          correo = dr["correo"].ToString(),
                                          fec_nac =  dr["fec_nac"].ToString(),//== DBNull.Value) ? (DateTime?)null : ((DateTime)(dr["fec_nac"])),
                                          telefono = dr["telefono"].ToString(),
                                          ubicacion = dr["ubicacion"].ToString(),
                                          fec_registro = dr["fec_registro"].ToString(),// == DBNull.Value) ? (string?)null : ((dr["fec_registro"].ToString())), 
                                          miem_bataclub_fecha =dr["miem_bataclub_fecha"].ToString(),// == DBNull.Value) ? (DateTime?)null : ((DateTime)(dr["miem_bataclub_fecha"])), 
                                          miem_bataclub = dr["miem_bataclub"].ToString(),                                          
                                      }).ToList();
                        }
                    }
                }
            }
            catch 
            {

                throw;
            }
            return listar;
        }
    }
}
