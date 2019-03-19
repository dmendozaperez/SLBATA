using System;
using CapaEntidad.ArticuloStock;
using CapaEntidad.Util;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Inventario
{
    public class Dat_Inventario
    {
        public string[] Registrar_CorteInventario(string codTienda, string descripcion,decimal usuario)
        {
            string[] resultDoc = new string[2];
            string sqlquery = "USP_INSERTAR_CORTEINVENTARIO";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            try
            {
                
                cn = new SqlConnection(Ent_Conexion.conexion);
                if (cn.State == 0) cn.Open();

                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CodTda", codTienda);
                cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                cmd.Parameters.AddWithValue("@UsuCrea", usuario);
                cmd.Parameters.Add("@InvId", SqlDbType.Decimal, 12);
                cmd.Parameters["@InvId"].Value = 0;
                cmd.Parameters["@InvId"].Direction = ParameterDirection.InputOutput;

                cmd.ExecuteNonQuery();
                resultDoc[0] = cmd.Parameters["@InvId"].Value.ToString();
            }
            catch (Exception ex)
            {
                if (cn != null)
                    if (cn.State == ConnectionState.Open) cn.Close();
                resultDoc[0] = "-1";
                resultDoc[1] = ex.Message;
            }
            if (cn != null)
                if (cn.State == ConnectionState.Open) cn.Close();
            return resultDoc;
        }


    }
}
