using System;
using CapaEntidad.ArticuloStock;
using CapaEntidad.Util;
using CapaEntidad.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Interface
{
    public class Dat_Interface
    {
        
        
        public List<Ent_Combo> listar_Pais()
        {
            
            string sqlquery = "USP_Listar_Pais";
            List<Ent_Combo> lista = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {

                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            lista = new List<Ent_Combo>();
                            lista = (from DataRow dr in dt.Rows
                                     select new Ent_Combo()
                                     {
                                         cbo_codigo = dr["cbo_codigo"].ToString(),
                                         cbo_descripcion = dr["cbo_descripcion"].ToString(),

                                     }).ToList();

                        }
                    }
                }
            }
            catch (Exception exc)
            {

                lista = null;
            }
            return lista;
        }

        public string listarStr_Tienda(string pais)
        {
            string strJson = "";
            try
            {
                SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru);
                cn.Open();
                SqlCommand oComando = new SqlCommand("USP_GET_XSTORE_TIENDA_PAIS_WEB", cn);
                oComando.CommandType = CommandType.StoredProcedure;

                SqlParameter odepartamento = oComando.Parameters.Add("@PAIS", SqlDbType.VarChar);
                odepartamento.Direction = ParameterDirection.Input;
                odepartamento.Value = pais;

                SqlDataReader oReader = oComando.ExecuteReader(CommandBehavior.SingleResult);
                DataTable dataTable = new DataTable("row");
                dataTable.Load(oReader);

                strJson = JsonConvert.SerializeObject(dataTable, Newtonsoft.Json.Formatting.Indented);
                strJson = strJson.Replace(Environment.NewLine, "");
                //strJson = strJson.Replace(" ", "");
                cn.Close();
            }
            catch (Exception ex)
            {

                return strJson;
            }

            //return oLista;
            return strJson;
        }


        public string listarStr_Interface()
        {
            string strJson = "";
            try
            {
                SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru);
                cn.Open();
                SqlCommand oComando = new SqlCommand("USP_OBTENER_INTERFACE", cn);
                oComando.CommandType = CommandType.StoredProcedure;
             
                SqlDataReader oReader = oComando.ExecuteReader(CommandBehavior.SingleResult);
                DataTable dataTable = new DataTable("row");
                dataTable.Load(oReader);

                strJson = JsonConvert.SerializeObject(dataTable, Newtonsoft.Json.Formatting.Indented);
                strJson = strJson.Replace(Environment.NewLine, "");
                //strJson = strJson.Replace(" ", "");
                cn.Close();
            }
            catch (Exception ex)
            {

                return strJson;
            }

            //return oLista;
            return strJson;
        }


        public Boolean InsertarInterfaceTienda(Ent_TiendaInterface _InterfaceTienda)
        {
            //string sqlquery = "USP_Insertar_GeneracionValeCompra";
            string sqlquery = "USP_INSERTAR_INTERFACE_TIENDA";
            Boolean valida = false;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@intf_Pais", _InterfaceTienda.Cod_Pais);
                        cmd.Parameters.AddWithValue("@intf_Tda", _InterfaceTienda.Cod_Tda);
                        cmd.Parameters.AddWithValue("@intf_UsuCrea", _InterfaceTienda.IdUsu);
                        cmd.Parameters.AddWithValue("@intf_listDetalle", _InterfaceTienda.List_strListDetalle);
                       
                        cmd.ExecuteNonQuery();
                        valida = true;
                    }
                }

            }
            catch (Exception exc)
            {
                valida = false;
            }
            return valida;


        }
    }

  
}
