using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad.RuletaBata;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad.Util;

namespace CapaDato.RuletaBata
{
    public class Dat_RuletaBata
    {
        public List<Premios> get_premios ()
        {
            string sqlquery = "[usp_get_premios_ruleta_bata]";
            DataTable premios = new DataTable();
            List<Premios> _premios = null;
            try
            {
                //Ent_Conexion.conexion = "Server=192.168.1.242;Database=BDPOS;User ID=sa;Password=1;Trusted_Connection=False;";
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(premios);
                        _premios = new List<Premios>();
                        foreach (DataRow row in premios.Rows)
                        {
                            Premios _premio = new Premios();
                            _premio.id = Convert.ToInt32(row["id"]);
                            _premio.indice = Convert.ToInt32(row["indice"]);
                            _premio.nombre = row["nombre"].ToString();
                            _premio.color = row["color"].ToString();
                            _premio.tipo = Convert.ToInt32(row["tipo"]);
                            _premio.descripcion = row["descripcion"].ToString();
                            _premio.imagen = row["imagen"].ToString();
                            _premio.prom_id = row["prom_id"].ToString();
                            _premios.Add(_premio);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _premios = null;
            }
            return _premios;
        }

        public Premios get_ganador_rulta(string cod_tda)
        {
            string sqlquery = "[usp_get_ganador_ruleta]";
            DataTable dt_premios = new DataTable();
            Premios ganador = null;
            try
            {
                //Ent_Conexion.conexion = "Server=192.168.1.242;Database=BDPOS;User ID=sa;Password=1;Trusted_Connection=False;";
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cod_tda", cod_tda);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt_premios);
                        ganador = new Premios();
                        foreach (DataRow row in dt_premios.Rows)
                        {
                            Premios _premio = new Premios();
                            _premio.id = Convert.ToInt32(row["id"]);
                            _premio.indice = Convert.ToInt32(row["indice"]);
                            _premio.nombre = row["nombre"].ToString();
                            _premio.color = row["color"].ToString();
                            _premio.tipo = Convert.ToInt32(row["tipo"]);
                            _premio.descripcion = row["descripcion"].ToString();
                            _premio.prom_id = row["prom_id"].ToString();
                            ganador = _premio;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ganador = null;
            }
            return ganador;
        }
        public int insertar_ganador_ruleta(string codigo_premio,string cod_tda,string dni,string nombre,string ape_pat, string ape_mat,string telefono,string email,int id_premio)
        {
            string sqlquery = "usp_insertar_ganadores_ruleta_bata";
            try
            {
                //Ent_Conexion.conexion = "Server=192.168.1.242;Database=BDPOS;User ID=sa;Password=1;Trusted_Connection=False;";
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codigo_premio", codigo_premio);
                        cmd.Parameters.AddWithValue("@cod_tda",cod_tda);
                        cmd.Parameters.AddWithValue("@dni",dni);
                        cmd.Parameters.AddWithValue("@nombre",nombre);
                        cmd.Parameters.AddWithValue("@ape_pat",ape_pat);
                        cmd.Parameters.AddWithValue("@ape_mat", ape_mat);
                        cmd.Parameters.AddWithValue("@telefono",telefono);
                        cmd.Parameters.AddWithValue("@email",email);
                        cmd.Parameters.AddWithValue("@id_premio",id_premio);
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public DataTable get_info_cupon_ruleta(string v)
        {
            string sqlquery = "[usp_get_info_cupon_ruleta]";
            DataTable dt_premios = new DataTable();
            try
            {
                //Ent_Conexion.conexion = "Server=192.168.1.242;Database=BDPOS;User ID=sa;Password=1;Trusted_Connection=False;";
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codigo", v);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt_premios);                        
                    }
                }
            }
            catch (Exception ex)
            {
                dt_premios = null;
            }
            return dt_premios;
        }
    }
}
