using CapaEntidad.Contabilidad;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Contabilidad
{
    public class Dat_Folder_Documento
    {
        public List<Ent_Folder_Documento> listar_folder()
        {
            string sqlquery = "USP_GET_FOLDER_DOCUMENTO";
            List<Ent_Folder_Documento> listar = null;
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
                            listar = new List<Ent_Folder_Documento>();
                            listar = (from DataRow fila in dt.Rows
                                      select new Ent_Folder_Documento()
                                      {
                                          cod_semana = fila["cod_semana"].ToString(),
                                          Fol_id = fila["Fol_id"].ToString(),
                                          Fol_Des = fila["Fol_Des"].ToString(),
                                          Fol_Padre = fila["Fol_Padre"].ToString(),
                                          Fol_Item =Convert.ToInt32(fila["Fol_Item"]),
                                      }
                                    ).ToList();
                        }
                    }
                }
            }
            catch 
            {
                listar = new List<Ent_Folder_Documento>();
            }
            return listar;
        }

        //public List<Ent_Folder_Documento> list_semana()
        //{
        //    List<Ent_Folder_Documento> listar = null;
        //    try
        //    {

        //    }
        //    catch (Exception)
        //    {

                
        //    }
        //    return listar;
        //}

    }
}
