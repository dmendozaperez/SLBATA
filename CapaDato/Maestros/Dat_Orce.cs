﻿using CapaEntidad.Maestros;
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
    public class Dat_Orce
    {
        public List<Ent_Cadena> lista_cadena()
        {
            List<Ent_Cadena> listar = null;
            string sqlquery = "USP_LISTAR_CADENA_XSTORE";
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
                                listar = new List<Ent_Cadena>();
                                while (dr.Read())
                                {
                                    Ent_Cadena cad = new Ent_Cadena();
                                    cad.codigo = dr["codigo"].ToString();
                                    cad.descrip= dr["descrip"].ToString();
                                    listar.Add(cad);
                                }
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
        public List<Ent_TipoTienda> lista_tipotda()
        {
            List<Ent_TipoTienda> listar = null;
            string sqlquery = "USP_GET_TIPO_TDA";
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
                                listar = new List<Ent_TipoTienda>();
                                while (dr.Read())
                                {
                                    Ent_TipoTienda tip = new Ent_TipoTienda();
                                    tip.tip_cod = dr["tip_cod"].ToString();
                                    tip.tip_des = dr["tip_des"].ToString();
                                    listar.Add(tip);
                                }
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
        public List<Ent_Tienda_CadTip> lista_tda_cadenatipo()
        {
            List<Ent_Tienda_CadTip> listar = null;
            string sqlquery = "USP_GET_TDA_CADENATIPO";
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
                                listar = new List<Ent_Tienda_CadTip>();
                                while (dr.Read())
                                {
                                    Ent_Tienda_CadTip tda = new Ent_Tienda_CadTip();
                                    tda.cod_entid = dr["cod_entid"].ToString();
                                    tda.des_entid = dr["des_entid"].ToString();
                                    tda.cod_cadena= dr["cod_cadena"].ToString();
                                    tda.cod_tiptda2 = dr["cod_tiptda2"].ToString();
                                    listar.Add(tda);
                                }
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
