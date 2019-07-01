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
    public class Dat_BataClub_CuponesCO
    {
       // Combo de Promociones
        public List<Ent_BataClub_ComboProm> get_ListaPromociones()
        {
            List<Ent_BataClub_ComboProm> list = null;
            string sqlquery = "USP_BATACLUB_GET_PROMOCION";
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
                            list = new List<Ent_BataClub_ComboProm>();
                            Ent_BataClub_ComboProm combo = new Ent_BataClub_ComboProm();
                            while (dr.Read())
                            {
                                combo = new Ent_BataClub_ComboProm();
                                combo.prom_id = dr["Prom_ID"].ToString();
                                combo.prom_des = dr["Prom_Des"].ToString();
                                list.Add(combo);
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

        //Listado Tabla principal
        public List<Ent_BataClub_CuponesCO> get_lista_prom(string dni, string cupon, string id_grupo)
        {
            string sqlquery = "USP_Report_CuponBataClub_GFT";
            List<Ent_BataClub_CuponesCO> listar = null;
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
                            cmd.Parameters.AddWithValue("@grupo", id_grupo);
                            cmd.Parameters.AddWithValue("@dni", dni);
                            cmd.Parameters.AddWithValue("@cupon", cupon);
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_BataClub_CuponesCO>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_BataClub_CuponesCO()
                                          {
                                              Nombres = dr["Nombres"].ToString(),
                                              Apellidos = dr["Apellidos"].ToString(),
                                              dni = dr["DNI"].ToString(),
                                              correo = dr["Correo"].ToString(),
                                              cupon = dr["Cupon"].ToString(),
                                              tienda = dr["Tienda"].ToString(),
                                              dni_venta = dr["Dni_Venta"].ToString(),
                                              nombres_venta = dr["Nombres_Venta"].ToString(),
                                              correo_venta = dr["Correo_Venta"].ToString(),
                                              telefono_venta = dr["Telefono_Venta"].ToString(),
                                              tickets = dr["Tickets"].ToString(),
                                              soles = dr["Soles"].ToString(),
                                              //id_grupo = dr["id_grupo"].ToString(),
                                              grupo = dr["Grupo"].ToString(),
                                              porc_desc = dr["Porc_Desc"].ToString(),
                                              fec_doc = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["Fec_Doc"]))
                                          }).ToList();
                            }

                        }
                    }
                    catch (Exception)
                    {
                        listar = null;
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception)
            {
                listar = null;
            }
            return listar;
        }

    }
}
