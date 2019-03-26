﻿using CapaEntidad.XstoreTda;
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
    public class Dat_XstoreTienda
    {
        public List<Ent_TiendaConf> List_Tienda_config()
        {
            string sqlquery = "USP_LISTAR_TIENDA";
            List<Ent_TiendaConf> list = null;
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
                         
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                list = new List<Ent_TiendaConf>();
                                list = (from DataRow dr in dt.Rows
                                        select new Ent_TiendaConf()
                                        {

                                            cod_Entid = dr["COD_ENTID"].ToString(),
                                            des_Entid = dr["DES_ENTID"].ToString(),
                                            cod_Emp = dr["COD_EMP"].ToString(),
                                            des_Emp = dr["DES_EMP"].ToString(),
                                            des_Cadena = dr["DES_CAD"].ToString(),
                                            direccion = dr["DIRECCION"].ToString(),
                                            cod_Jefe = dr["COD_JEFE"].ToString(),
                                            consecionario = dr["CONSECIONARIO"].ToString(),
                                            bol_xstore = dr["XSTORE"].ToString(),
                                            bol_gcorrelativo = dr["CORRE_GENERADO"].ToString(),

                                        }).ToList();

                            }
                        

                        }
                    }
                    catch (Exception)
                    {
                        list = null;
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch
            {
                list = null;
            }
            return list;
        }

        public int ActualizarTiendaXstore(string codTienda, Int32 Estado, decimal usuario) {

            Int32 intRespuesta = 0;

            string sqlquery = "USP_ACTIVAR_TIENDA_XSTORE";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try { 

                cn = new SqlConnection(Ent_Conexion.conexion);
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodTienda", codTienda);
                cmd.Parameters.AddWithValue("@Estado", Estado);
                cmd.Parameters.AddWithValue("@usuUpd", usuario);
                cmd.ExecuteNonQuery();
                intRespuesta = 1;

            }
            catch (Exception ex) {

                intRespuesta = -1;

            }

            return intRespuesta;
        }

        public int GenerarCorrelativoTiendaXstore(string codTienda, decimal usuario)
        {

            Int32 intRespuesta = 0;

            string sqlquery = "USP_SETEAR_CORRELATIVOS_TDA_WEB";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {

                cn = new SqlConnection(Ent_Conexion.conexion);
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COD_TDA", codTienda);
                cmd.Parameters.AddWithValue("@USU", usuario);
                cmd.ExecuteNonQuery();
                intRespuesta = 1;

            }
            catch (Exception ex)
            {

                intRespuesta = -1;

            }

            return intRespuesta;
        }

    }


}
