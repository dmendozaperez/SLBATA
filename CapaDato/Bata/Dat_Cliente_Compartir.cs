using CapaEntidad.Bata;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Bata
{

    public class Dat_Cliente_Compartir
    {
        public List<Ent_Barra_Compartir> listar_barra(string dni)
        {
            List<Ent_Barra_Compartir> list = null;
            string sqlquery = "USP_BATACLUB_CONSULTA_COMPARTIR_CLIENTE_BARRA";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {

                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DNI", dni);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            list = (from DataRow fila in dt.Rows
                                    select new Ent_Barra_Compartir()
                                    {
                                        barra = fila["barra"].ToString(),
                                        estado = fila["est_des"].ToString(),                                      
                                    }
                                  ).ToList();
                        }
                    }

                }
            }
            catch
            {
                list = new List<Ent_Barra_Compartir>();

            }
            return list;
        }
        public Boolean insert_edit_compartir(string dni,string correo,string cod_tda,decimal usu,Boolean envio,string numdoc="")
        {
            Boolean valida = false;
            string sqlquery = "USP_BATACLUB_INSERTAR_ENVIAR_COMPARTIR_BATAWEB";
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
                            cmd.Parameters.AddWithValue("@dni", dni);
                            cmd.Parameters.AddWithValue("@correo", correo);
                            cmd.Parameters.AddWithValue("@cod_tda", cod_tda);
                            cmd.Parameters.AddWithValue("@usu", usu);
                            cmd.Parameters.AddWithValue("@envio", envio);
                            cmd.Parameters.AddWithValue("@numdoc", numdoc);
                            cmd.ExecuteNonQuery();
                            valida = true;
                        }
                    }
                    catch 
                    {
                        valida = false;
                        
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch 
            {
                valida = false;
            }
            return valida;
        }
        public Boolean existe_dni(string dni)
        {
            string sqlquery = "USP_BATACLUB_EXISTE_COMPARTIR_DNI";
            Boolean existe = false;
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
                            cmd.Parameters.AddWithValue("@DNI", dni);
                            cmd.Parameters.Add("@EXISTE", SqlDbType.Bit);
                            cmd.Parameters["@EXISTE"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            existe =Convert.ToBoolean(cmd.Parameters["@EXISTE"].Value);
                        }
                    }
                    catch 
                    {
                        existe = false;
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch
            {

                existe = false;
            }
            return existe;
        }
        public List<Ent_Tienda_Compartir> lista_tienda()
        {
            string sqlquery = "USP_BATACLUB_CONSULTA_COMPARTIR_TIENDA";
            List<Ent_Tienda_Compartir> listar = null;
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
                            listar = new List<Ent_Tienda_Compartir>();
                            listar = (from DataRow fila in dt.Rows
                                      select new Ent_Tienda_Compartir()
                                      {
                                          cod_entid = fila["cod_entid"].ToString(),
                                          des_entid = fila["des_entid"].ToString(),
                                      }
                                    ).ToList();
                        }
                    }
                }
            }
            catch
            {

                listar = new List<Ent_Tienda_Compartir>();
            }
            return listar;
        }
        public List<Ent_Cliente_Compartir> listar(string buscar="")
        {
            List<Ent_Cliente_Compartir> list = null;
            string sqlquery = "USP_BATACLUB_CONSULTA_COMPARTIR_CLIENTE";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {

                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@buscar", buscar);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            list = (from DataRow fila in dt.Rows
                                    select new Ent_Cliente_Compartir()
                                    {
                                        dni = fila["dni"].ToString(),
                                        correo = fila["correo"].ToString(),
                                        envio_email = fila["envio_email"].ToString(),
                                        tienda = fila["tienda"].ToString(),
                                        fecha_ing = fila["fecha_ing"].ToString(),
                                        fecha_env = fila["fecha_env"].ToString(),
                                        cod_tda = fila["cod_tda"].ToString(),
                                        num_doc = fila["num_doc"].ToString(),
                                    }
                                  ).ToList();
                        }
                    }

                }
            }
            catch 
            {
                list = new List<Ent_Cliente_Compartir>();
                
            }
            return list;
        }
    }
}
