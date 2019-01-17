using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Comunicado
{
    public class Modulo
    {
        public string get_empresa_comunicado(string cod_tda)
        {
            string sqlquery = "USP_GET_TIENDA_EMP_COMUNICADO";
            string nom_emp = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    try
                    {
                        if (cn.State == 0) cn.Open();
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@COD_TDA", cod_tda);
                            cmd.Parameters.Add("@NOM_EMP", SqlDbType.VarChar, 30);
                            cmd.Parameters["@NOM_EMP"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            nom_emp = cmd.Parameters["@NOM_EMP"].Value.ToString();
                        }
                    }
                    catch (Exception)
                    {

                        
                    }
                }
            }
            catch 
            {
                
            }
            return nom_emp;
        }
        public  Boolean _existe_bg_bw(string _tda, ref string _cadena)
        {
            Boolean _valida = false;
            string sqlquery = "USP_Existe_Tda_BGWB";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                _tda = _tda.Substring(2, 3);

                cn = new SqlConnection(Ent_Conexion.conexion);
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_entid", _tda);

                cmd.Parameters.Add("@existe", SqlDbType.Bit);
                cmd.Parameters["@existe"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("@cadena", SqlDbType.VarChar, 5);
                cmd.Parameters["@cadena"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                _valida = (Boolean)cmd.Parameters["@existe"].Value;
                _cadena = (string)cmd.Parameters["@cadena"].Value;
            }
            catch (Exception exc)
            {
                _valida = false;                
            }
            if (cn != null)
                if (cn.State == ConnectionState.Open) cn.Close();
            return _valida;
        }
        public string _estado_acceso(string _tda)
        {
            //DataTable dt = null;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            //SqlDataAdapter da = null;
            string sqlcommand = "USP_VefificaTda";
            string _estado = "";
            try
            {
                cn = new SqlConnection(Ent_Conexion.conexion);
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlcommand, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@cod_tda", _tda);
                cmd.Parameters.Add("@estado", SqlDbType.VarChar, 5);
                cmd.Parameters["@estado"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                _estado = cmd.Parameters["@estado"].Value.ToString();
            }
            catch
            {
                if (cn != null)
                    if (cn.State == ConnectionState.Open) cn.Close();
                _estado = "0";
            }
            if (cn != null)
                if (cn.State == ConnectionState.Open) cn.Close();
            return _estado;
        }
    }
}
