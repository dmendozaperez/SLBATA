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
                                          fec_nac = dr["fec_nac"] == null || dr["fec_nac"].ToString() == "" ? "" : Convert.ToDateTime(dr["fec_nac"]).ToString("dd/MM/yyyy"),
                                          telefono = dr["telefono"].ToString(),
                                          ubicacion = dr["ubicacion"].ToString(),
                                          fec_registro = dr["fec_registro"] == null || dr["fec_registro"].ToString() == "" ? "" : Convert.ToDateTime(dr["fec_registro"]).ToString("dd/MM/yyyy hh:mm:ss"),
                                          miem_bataclub_fecha = dr["miem_bataclub_fecha"] == null || dr["miem_bataclub_fecha"].ToString() == "" ? "" : Convert.ToDateTime(dr["miem_bataclub_fecha"]).ToString("dd/MM/yyyy hh:mm:ss"),
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
        public bool Modificar_Cliente_Bataclub(Ent_BataClub_Registro cliente , string usuario, ref string mensaje)
        {
            string sqlquery = "USP_INSERTAR_CLIENTE_BATACLUB";
            bool ret = false;   
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

                            cmd.Parameters.AddWithValue("@canal", cliente.Canal);
                            cmd.Parameters.AddWithValue("@dni", cliente.Dni);
                            cmd.Parameters.AddWithValue("@primerNombre", cliente.PrimerNombre);
                            cmd.Parameters.AddWithValue("@segundoNombre", cliente.SegundoNombre);
                            cmd.Parameters.AddWithValue("@apellidoPat", cliente.ApellidoPaterno);
                            cmd.Parameters.AddWithValue("@apellidoMat", cliente.ApellidoMaterno);
                            cmd.Parameters.AddWithValue("@genero", cliente.Genero);
                            cmd.Parameters.AddWithValue("@correo", cliente.CorreoElectronico);
                            cmd.Parameters.AddWithValue("@fecNac", (cliente.FechaNacimiento == "" || cliente.FechaNacimiento == null ? null : Convert.ToDateTime(cliente.FechaNacimiento).ToString("yyyyMMdd")));
                            cmd.Parameters.AddWithValue("@telefono", cliente.Celular);
                            cmd.Parameters.AddWithValue("@ubigeo", cliente.Ubigeo);
                            cmd.Parameters.AddWithValue("@usuario", usuario );
                            cmd.Parameters.AddWithValue("@ubigeo_distrito", cliente.UbigeoDistrito == null ? "" : cliente.UbigeoDistrito);

                            cmd.ExecuteNonQuery();
                            mensaje = "Cliente Actualizado con exito";
                            ret = true;
                        }
                    }
                    catch (Exception exc)
                    {
                        ret = false;
                        mensaje = exc.Message;
                    }

                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception exc)
            {
                ret = false;
                mensaje = exc.Message;
            }
            return ret;
        }
    }
}
