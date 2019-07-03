using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad.Util;

namespace CapaDato.ECommerce
{
    public class Dat_Cexpress
    {


        /// <summary>
        /// acceso de web service de urbano
        /// </summary>
        /// <returns></returns>
        //public Ent_Urbano get_acceso()
        //{
        //    string sqlquery = "USP_Urbano_AccesoWS";
        //    Ent_Urbano _acceso = null;
        //    try
        //    {
        //        using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
        //        {
        //            if (cn.State == 0) cn.Open();
        //            using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                SqlDataReader dr = cmd.ExecuteReader();

        //                if (dr.HasRows)
        //                {
        //                    _acceso = new Ent_Urbano();
        //                    while (dr.Read())
        //                    {
        //                        _acceso.url = dr["urb_url"].ToString();
        //                        _acceso.usuario = dr["urb_usu"].ToString();
        //                        _acceso.password = dr["urb_pass"].ToString();
        //                        _acceso.linea = dr["Urb_Linea"].ToString();
        //                        _acceso.contrato = dr["Urb_Contrato"].ToString();
        //                        break;
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        _acceso = null;
        //    }
        //    return _acceso;
        //}



        /// <summary>
        /// acceso a los datos a enviar urbano
        /// </summary>
        /// <returns></returns>
        public DataTable get_data(string _ven_id)
        {
            DataTable dt = null;
            string sqlquery = "USP_Urbano_SendData2";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ven_id", _ven_id);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dt = new DataTable();
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception)
            {
                dt = null;
            }
            return dt;
        }
        /// <summary>
        /// update venta guia urbano
        /// </summary>
        /// <param name="guia_prestasop"></param>
        /// <param name="guia_urbano"></param>
        /// <returns></returns>
        public Boolean update_guia(string guia_prestasop, string guia_urbano)
        {
            Boolean valida = false;
            string sqlquery = "USP_Urbano_UpdateGuia";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Ven_Pst_Ref", guia_prestasop);
                        cmd.Parameters.AddWithValue("@Ven_Guia_Urbano", guia_urbano);
                        cmd.ExecuteNonQuery();
                        valida = true;
                    }
                }
            }
            catch (Exception)
            {
                valida = false;
                throw;
            }
            return valida;
        }
        /// <summary>
        /// leer guia de urbano para enviar a prestashop
        /// </summary>
        /// <returns></returns>
        public DataTable getguiaUrbano()
        {
            DataTable dt = null;
            string sqlquery = "USP_GetGuiaUrbano";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dt = new DataTable();
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                dt = null;

            }
            return dt;
        }
        /// <summary>
        /// update en la tabla venta , para ya no volver a enviar la guia
        /// </summary>
        /// <returns></returns>
        public Boolean updprestashopGuia(string guia_prestashop, string guia_urbano)
        {
            string sqlquery = "USP_UpdPrestashop_Guia";
            Boolean valida = false;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ven_pst_ref", guia_prestashop);
                        cmd.Parameters.AddWithValue("@ven_guia_urbano", guia_urbano);
                        cmd.ExecuteNonQuery();
                        valida = true;
                    }
                }
            }
            catch (Exception)
            {
                valida = false;
            }
            return valida;
        }
        /*
        public Ent_Etiqueta get_etiqueta(string ven_id)
        {
            string sqlquery = "USP_EcommerceImpEtiqueta";
            Ent_Etiqueta etiqueta = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ven_id", ven_id);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            etiqueta = new Ent_Etiqueta();
                            while (dr.Read())
                            {
                                etiqueta.strNroGuia = dr["nroguia"].ToString();
                                etiqueta.cliente = dr["cliente"].ToString();
                                etiqueta.empresa = dr["Emp_Comercial"].ToString();
                                etiqueta.nro_pedido = dr["nro_pedido"].ToString();
                                etiqueta.direccion = dr["direccion"].ToString();
                                etiqueta.referencia = dr["Ven_Dir_Ref"].ToString();
                                etiqueta.ubigeo = dr["Ven_Ubigeo_Ent"].ToString();
                                etiqueta.cod_refer = dr["Ven_Pst_Ref"].ToString();
                                etiqueta.telefono = dr["telefono"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                etiqueta = null;
            }
            return etiqueta;
        }*/
    }
}
