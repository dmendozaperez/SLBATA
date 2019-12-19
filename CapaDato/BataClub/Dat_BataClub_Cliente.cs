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
    //CLIENTES BATACLUB
    public class Dat_BataClub_Cliente
    {
        //Listado Tabla Consulta Clientes
        public List<Ent_BataClub_Cliente> get_lista_cliente(string dni, string nombre, string apellido, string correo)
        {
            string sqlquery = "USP_BATACLUB_CONSULTAR_CLIENTES";
            List<Ent_BataClub_Cliente> listar = null;
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
                            cmd.Parameters.AddWithValue("@ubigeo", -1);
                            cmd.Parameters.AddWithValue("@dni", dni);
                            cmd.Parameters.AddWithValue("@nombre", nombre);
                            cmd.Parameters.AddWithValue("@apellido", apellido);
                            cmd.Parameters.AddWithValue("@correo", correo);
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_BataClub_Cliente>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_BataClub_Cliente()
                                          {
                                              can_des = dr["CAN_DES"].ToString(),
                                             // canal = dr["CANAL"].ToString(),
                                              dni = dr["DNI"].ToString(),
                                              nombres = dr["NOMBRES"].ToString(),
                                             // apellido_pat = dr["APELLIDO_PAT"].ToString(),                                             
                                             // apellido_mat = dr["APELLIDO_MAT"].ToString(),
                                              genero = dr["GENERO"].ToString(),
                                              correo = dr["CORREO"].ToString(),
                                              fec_nac = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["FEC_NAC"])),
                                              telefono = dr["TELEFONO"].ToString(),
                                              //ubigeo = dr["UBIGEO"].ToString(),
                                              ubigeo_distrito = dr["UBIGEO_DISTRITO"].ToString(),
                                              fec_modif = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["FEC_MODIF"])),
                                              fec_registro = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["FEC_REGISTRO"])),
                                             // fec_miembro = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["FEC_MIEMBRO"])),
                                              //cod_tda = dr["COD_TDA"].ToString(),
                                              des_entid = dr["DES_ENTID"].ToString(),
                                              cod_cadena = dr["COD_CADENA"].ToString(),
                                             // envio_correo = dr["ENVIO_CORREO"].ToString(),
                                             // fec_envio_correo = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["FEC_ENVIO_CORREO"])),
                                             // gene_cupon =  dr["GENE_CUPON"].ToString(),
                                              miem_bataclub =  dr["MIEM_BATACLUB"].ToString(),
                                              miem_bataclub_fecha = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["MIEM_BATACLUB_FECHA"]))
                                          }).ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var mensaje = ex.Message;
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

        public List<Ent_Cliente_Promocion> BATACLUB_CONSULTA_CLIENTES_PROMOCION(string dni)
        {
            string sqlquery = "USP_BATACLUB_CONSULTA_CLIENTES_PROMOCION";
            List<Ent_Cliente_Promocion> listar = null;
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
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_Cliente_Promocion>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_Cliente_Promocion()
                                          {
                                              Promocion = dr["Promocion"].ToString(),
                                              Barra = dr["Barra"].ToString(),
                                              Estado = dr["Estado"].ToString(),
                                              cup_fecha_ini = dr["cup_fecha_ini"] == null || dr["cup_fecha_ini"].ToString() == "" ? "" : Convert.ToDateTime(dr["cup_fecha_ini"]).ToString("dd/MM/yyyy"),
                                              cup_fecha_fin = dr["cup_fecha_fin"] == null || dr["cup_fecha_fin"].ToString() == "" ? "" : Convert.ToDateTime(dr["cup_fecha_fin"]).ToString("dd/MM/yyyy"),
                                              Tienda = dr["Tienda"].ToString(),
                                              Doc = dr["Doc"].ToString(),
                                              Ndoc = dr["Ndoc"].ToString(),
                                              FecDoc = dr["FecDoc"] == null || dr["FecDoc"].ToString() == "" ? "" : Convert.ToDateTime(dr["FecDoc"]).ToString("dd/MM/yyyy hh:mm:ss"),
                                          }).ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var mensaje = ex.Message;
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
