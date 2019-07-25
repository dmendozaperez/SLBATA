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
    public class Dat_Prom_Filtro
    {
        public List<Ent_Prom_Filtro> lista_prom_fltro()
        {
            string sqlquery = "USP_COMBO_PROMOCION_FILTRO";
            List<Ent_Prom_Filtro> lista = null;
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
                            SqlDataReader dr = cmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                lista = new List<Ent_Prom_Filtro>();

                                while (dr.Read())
                                {
                                    Ent_Prom_Filtro prom = new Ent_Prom_Filtro();
                                    prom.cod = dr["cod"].ToString();
                                    prom.descripcion= dr["descripcion"].ToString();
                                    lista.Add(prom);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch 
            {
                lista = null;              
            }
            return lista;
        }
    }
}
