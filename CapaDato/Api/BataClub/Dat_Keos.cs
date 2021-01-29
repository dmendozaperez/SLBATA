using CapaEntidad.Api.BataClub;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Api.BataClub
{
    public class Dat_Keos
    {
        public BuscarDoc buscar_venta(string token,string numdoc,ref Int32 resultado)
        {
            string sqlquery = "[USP_BATACLUB_KEOS_CAMBIO]";
            BuscarDoc obj = null;
            try
            {
                obj = new BuscarDoc();
                obj.token = token;
                obj.num_doc = numdoc;
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    try
                    {
                        if (cn.State == 0) cn.Open();
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@NUMDOC", numdoc);

                            cmd.Parameters.Add("@TIPO", SqlDbType.VarChar, 30);
                            cmd.Parameters["@TIPO"].Direction = ParameterDirection.Output;

                            cmd.Parameters.Add("@ESTADO", SqlDbType.Int);
                            cmd.Parameters["@ESTADO"].Direction = ParameterDirection.Output;

                            cmd.ExecuteNonQuery();


                            //string tipo =Convert.ToString(cmd.Parameters["@TIPO"].Value);
                            resultado = Convert.ToInt32(cmd.Parameters["@ESTADO"].Value);
                            List<ListaTiendas> tda = new List<ListaTiendas>();
                            if (resultado==1 || resultado == 2)
                            {
                                obj.tipo = Convert.ToString(cmd.Parameters["@TIPO"].Value);
                                /*si es online extraer tiendas*/
                                if (resultado == 2)
                                {
                                    SqlDataReader dr = cmd.ExecuteReader();
                                    if (dr.HasRows)
                                    {
                                        

                                        while(dr.Read())
                                        {
                                            ListaTiendas t = new ListaTiendas();
                                            t.tienda = dr["Tiendas"].ToString();
                                            tda.Add(t);
                                        }
                                                                                                               
                                    }

                                    resultado = 1;
                                }

                            }
                            obj.listar_tienda = tda;
                        }
                    }
                    catch 
                    {

                        
                    }
                    if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception exc)
            {

                
            }
            return obj;
        } 

        public BuscarPedido buscar_pedido(string token,string nro_pedido,ref Int32 resultado)
        {
            BuscarPedido obj = null;
            string sqlquery = "USP_BATACLUB_KEOS_PEDIDO_EC";
            try
            {
                obj = new BuscarPedido();
                obj.token = token;
                obj.nro_pedido = nro_pedido;

                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {

                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PEDIDO", nro_pedido);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count == 0)
                            {
                                obj.estado_pedido = "No Existe";
                                resultado = 0;
                            }
                            else
                            {
                                obj.estado_pedido = dt.Rows[0]["estado"].ToString();
                                resultado = 1;
                            }
                        }

                    }

                }

            }
            catch
            {

                
            }
            return obj;
        }

        public BuscarBC buscar_bc(string token, string numdni, ref Int32 resultado)
        {
            string sqlquery = "[USP_BATACLUB_KEOS_MIEMBRO_BC]";
            BuscarBC obj = null;
            try
            {
                obj = new BuscarBC();
                obj.token = token;
                obj.nro_dni = numdni;
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    try
                    {
                        if (cn.State == 0) cn.Open();
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@DNI", numdni);

                            cmd.Parameters.Add("@MIEMBRO", SqlDbType.VarChar, 20);
                            cmd.Parameters["@MIEMBRO"].Direction = ParameterDirection.Output;

                            
                            cmd.ExecuteNonQuery();


                            string bc_miembro =Convert.ToString(cmd.Parameters["@MIEMBRO"].Value);
                            obj.bataclub = bc_miembro;
                            List<ListarPromocion> prom = new List<ListarPromocion>();
                            if (bc_miembro=="Si")
                            {
                                resultado = 1;
                                SqlDataReader dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                   

                                    while (dr.Read())
                                    {
                                        ListarPromocion t = new ListarPromocion();
                                        t.promocion = dr["Promocion"].ToString();
                                        prom.Add(t);
                                    }
                                   


                                }
                                

                            }
                            obj.listar_promocion = prom;

                        }
                    }
                    catch
                    {


                    }
                    if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception exc)
            {


            }
            return obj;
        }

    }
}
