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

        public Int32 get_valida_ruleta()
        {
            string sqlquery = "USP_BATACLUB_VALIDA_RULETA";
            Int32 valida = 0;
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
                            cmd.Parameters.Add("@VALIDA",SqlDbType.Int);
                            cmd.Parameters["@VALIDA"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            valida =Convert.ToInt32(cmd.Parameters["@VALIDA"].Value);

                        }

                    }
                    catch (Exception)
                    {
                        valida = 0;
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch
            {
                valida = 0;

            }
            return valida;
        }
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
        public int insertar_ganador_ruleta(string codigo_premio,string cod_tda,string dni,string nombre,string ape_pat, string ape_mat,string telefono,string email,int id_premio,int usu_id,ref string barra)
        {
            //string sqlquery = "usp_insertar_ganadores_ruleta_bata";
            string sqlquery = "USP_BATACLUB_GENERA_GANADOR";
            int valida = 0;
            try
            {
                //Ent_Conexion.conexion = "Server=192.168.1.242;Database=BDPOS;User ID=sa;Password=1;Trusted_Connection=False;";
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    try
                    {
                        if (cn.State == 0) cn.Open();
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id_premio", id_premio);
                            cmd.Parameters.AddWithValue("@dni", dni);
                            cmd.Parameters.AddWithValue("@cod_tda", cod_tda);
                            cmd.Parameters.AddWithValue("@usu_id", usu_id);

                            cmd.Parameters.Add("@barra", SqlDbType.VarChar, 30);
                            cmd.Parameters["@barra"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();

                            barra =Convert.ToString(cmd.Parameters["@barra"].Value);
                            valida = 1;
                        }
                    }
                    catch (Exception)
                    {

                        valida = 0;
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                    //if (cn.State == 0) cn.Open();
                    //using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    //{
                    //    cmd.CommandTimeout = 0;
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.AddWithValue("@codigo_premio", codigo_premio);
                    //    cmd.Parameters.AddWithValue("@cod_tda",cod_tda);
                    //    cmd.Parameters.AddWithValue("@dni",dni);
                    //    cmd.Parameters.AddWithValue("@nombre",nombre);
                    //    cmd.Parameters.AddWithValue("@ape_pat",ape_pat);
                    //    cmd.Parameters.AddWithValue("@ape_mat", ape_mat);
                    //    cmd.Parameters.AddWithValue("@telefono",telefono);
                    //    cmd.Parameters.AddWithValue("@email",email);
                    //    cmd.Parameters.AddWithValue("@id_premio",id_premio);
                    //    return cmd.ExecuteNonQuery();
                    //}
                }
            }
            catch (Exception)
            {
                valida=0;
            }
            return valida;
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

        public Ent_Ruleta_Valida get_valida_dni(string dni)
        {
            string sqlquery = "USP_BATACLUB_VALIDA_RULETA_DNI";
            Ent_Ruleta_Valida obj = null;
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
                            cmd.Parameters.Add("@COMPRAS", SqlDbType.Decimal);
                            cmd.Parameters.Add("@BATACLUB", SqlDbType.Bit);
                            cmd.Parameters.Add("@PRIM_NOM", SqlDbType.VarChar,100);
                            cmd.Parameters.Add("@SEG_NOM", SqlDbType.VarChar, 100);
                            cmd.Parameters.Add("@PRIM_APE", SqlDbType.VarChar, 100);
                            cmd.Parameters.Add("@SEG_APE", SqlDbType.VarChar, 100);
                            cmd.Parameters.Add("@TELEFONO", SqlDbType.VarChar, 100);
                            cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar, 200);

                            cmd.Parameters["@COMPRAS"].Direction = ParameterDirection.Output;
                            cmd.Parameters["@BATACLUB"].Direction = ParameterDirection.Output;
                            cmd.Parameters["@PRIM_NOM"].Direction = ParameterDirection.Output;
                            cmd.Parameters["@SEG_NOM"].Direction = ParameterDirection.Output;
                            cmd.Parameters["@PRIM_APE"].Direction = ParameterDirection.Output;
                            cmd.Parameters["@SEG_APE"].Direction = ParameterDirection.Output;
                            cmd.Parameters["@TELEFONO"].Direction = ParameterDirection.Output;
                            cmd.Parameters["@EMAIL"].Direction = ParameterDirection.Output;

                            cmd.ExecuteNonQuery();

                            obj = new Ent_Ruleta_Valida();
                            obj.compras =Convert.ToDecimal(cmd.Parameters["@COMPRAS"].Value);
                            obj.bataclub = Convert.ToBoolean(cmd.Parameters["@BATACLUB"].Value);
                            obj.prim_nom = Convert.ToString(cmd.Parameters["@PRIM_NOM"].Value);
                            obj.seg_nom = Convert.ToString(cmd.Parameters["@SEG_NOM"].Value);
                            obj.pri_ape = Convert.ToString(cmd.Parameters["@PRIM_APE"].Value);
                            obj.seg_ape = Convert.ToString(cmd.Parameters["@SEG_APE"].Value);
                            obj.telefono = Convert.ToString(cmd.Parameters["@TELEFONO"].Value);
                            obj.correo = Convert.ToString(cmd.Parameters["@EMAIL"].Value);                            
                        }
                    }
                    catch 
                    {
                        obj = new Ent_Ruleta_Valida();

                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch 
            {
                obj = new Ent_Ruleta_Valida();
            }
            return obj;
        }

    }
}
