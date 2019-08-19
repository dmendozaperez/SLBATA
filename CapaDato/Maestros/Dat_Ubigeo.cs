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
    public class Dat_Ubigeo
    {
        public List<Ent_Departamento> get_lista_Departamento()
        {
            List<Ent_Departamento> list = null;
            string sqlquery = "USP_Leer_Departamento";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Departamento>();
                            Ent_Departamento dep = new Ent_Departamento();
                            dep.dep_id = "0";
                            dep.dep_descripcion = "--Ninguno--";
                            list.Add(dep);
                            while (dr.Read())
                            {
                                dep = new Ent_Departamento();
                                dep.dep_id = dr["DEP_ID"].ToString();
                                dep.dep_descripcion = dr["DEP_DESCRIPCION"].ToString();
                                list.Add(dep);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = null;
            }
            return list;
        }

        public List<Ent_Provincia> get_lista_Provincia(string cod_dep)
        {
            List<Ent_Provincia> list = null;
            string sqlquery = "USP_Leer_Provincia";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cod_dep", cod_dep);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Provincia>();
                            Ent_Provincia prv = new Ent_Provincia();
                            prv.prv_cod = "0";
                            prv.prv_descripcion = "--Ninguno--";
                            list.Add(prv);
                            while (dr.Read())
                            {
                                prv = new Ent_Provincia();
                                prv.prv_cod = dr["PRV_ID"].ToString();
                                prv.prv_descripcion = dr["PRV_DESCRIPCION"].ToString();
                                list.Add(prv);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = null;
            }
            return list;
        }

        public List<Ent_Distrito> get_lista_Distrito(string cod_dep, string cod_prv)
        {
            List<Ent_Distrito> list = null;
            string sqlquery = "USP_Leer_Distrito";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cod_dep", cod_dep);
                        cmd.Parameters.AddWithValue("@cod_prv", cod_prv);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Distrito>();
                            Ent_Distrito dis = new Ent_Distrito();
                            dis.dis_cod = "0";
                            dis.dis_descripcion = "--Ninguno--";
                            list.Add(dis);
                            while (dr.Read())
                            {
                                dis = new Ent_Distrito();
                                dis.dis_cod = dr["DIS_COD"].ToString();
                                dis.dis_descripcion = dr["DIS_DESCRIPCION"].ToString();
                                list.Add(dis);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = null;
            }
            return list;
        }


    }
}
