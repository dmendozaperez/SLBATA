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
    public class Dat_Transito
    {
        public List<Ent_Tran_Emp_Cad_Tda> lista_Emp_Cad_Tda_trans(string pais)
        {
            List<Ent_Tran_Emp_Cad_Tda> listar = null;
            string sqlquery = "USP_LEER_EMP_CAD_TDA";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PAIS",pais);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            listar = new List<Ent_Tran_Emp_Cad_Tda>();
                            listar = (from DataRow fila in dt.Rows
                                      select new Ent_Tran_Emp_Cad_Tda()
                                      {
                                          cod_entid = fila["cod_entid"].ToString(),
                                          des_entid = fila["des_entid"].ToString(),
                                          cod_empresa = fila["cod_empresa"].ToString(),
                                          des_empresa = fila["des_empresa"].ToString(),
                                          cod_cadena = fila["cod_cadena"].ToString(),
                                          des_cadena = fila["des_cadena"].ToString(),
                                      }
                                    ).ToList();
                        }
                    }
                }
            }
            catch 
            {
                listar = new List<Ent_Tran_Emp_Cad_Tda>();
            }
            return listar;
        }

        public List<Ent_Tran_concepto> lista_concepto_trans()
        {
            List<Ent_Tran_concepto> listar = null;
            string sqlquery = "USP_LEER_CONCEPTO";
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
                            listar = new List<Ent_Tran_concepto>();
                            listar = (from DataRow fila in dt.Rows
                                      select new Ent_Tran_concepto()
                                      {
                                          con_id = fila["con_id"].ToString(),
                                          con_des = fila["con_des"].ToString(),                                          
                                          con_tran=Convert.ToBoolean(fila["con_tran"]),
                                      }
                                    ).ToList().Where(a=>a.con_tran==true).ToList();
                        }
                    }
                }
            }
            catch
            {
                listar = new List<Ent_Tran_concepto>();
            }
            return listar;
        }
        public List<Ent_Tran_Articulo> lista_articulo_trans(string pais)
        {
            List<Ent_Tran_Articulo> listar = null;
            string sqlquery = "USP_LISTA_ARTICULO_X_PAIS";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PAIS", pais);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            listar = new List<Ent_Tran_Articulo>();
                            listar = (from DataRow fila in dt.Rows
                                      select new Ent_Tran_Articulo()
                                      {
                                          cod_artic = fila["cod_artic"].ToString(),
                                          des_artic = fila["des_artic"].ToString(),                                         
                                      }
                                    ).ToList();
                        }
                    }
                }
            }
            catch
            {
                listar = new List<Ent_Tran_Articulo>();
            }
            return listar;
        }
    }
}
