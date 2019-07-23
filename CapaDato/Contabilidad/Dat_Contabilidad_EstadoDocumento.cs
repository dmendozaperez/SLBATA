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
    public class Dat_Contabilidad_EstadoDocumento
    {
        //Listado Tabla principal
        public List<Ent_Contabilidad_EstadoDocumento> get_lista(string cod_entid, DateTime fec_ini, DateTime fec_fin)
        {
            string sqlquery = "USP_XSTORE_CONSULTA_DOCUMENTOS";
            List<Ent_Contabilidad_EstadoDocumento> listar = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    try
                    {
                        //if (cn.State == 0) cn.Open();
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@TIENDA", cod_entid);
                            cmd.Parameters.AddWithValue("@FECHA_INI", fec_ini);
                            cmd.Parameters.AddWithValue("@FECHA_FIN", fec_fin);
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_Contabilidad_EstadoDocumento>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_Contabilidad_EstadoDocumento()
                                          {
                                              tienda = dr["TIENDA"].ToString(),
                                              fecha = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["FECHA"])),
                                              tipo_doc = dr["TIPO_DOC"].ToString(),
                                              serie = dr["SERIE"].ToString(),
                                              numero = dr["NUMERO"].ToString(),
                                              total = dr["TOTAL"].ToString(),
                                              estado = dr["ESTADO"].ToString(),
                                              ruc = dr["RUC"].ToString(),
                                              login_ws = dr["LOGIN_WS"].ToString(),
                                              clave_ws = dr["CLAVE_WS"].ToString(),
                                              tipodoc = dr["TIPODOC"].ToString(),
                                              folio = dr["FOLIO"].ToString()
                                        }).ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        listar = null;                     
                    }
                    //if (cn != null)
                    //    if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception)
            {
                listar = null;
            }
            return listar;
        }

        ///Listado Tabla de Detalle - PopUp
        public List<Ent_Contabilidad_EstadoDocumento_Det> listarStr_Detalle_PopUp(string ruc, string login_ws, string clave_ws, string tipodoc, string folio)
        {
            string sqlquery = "USP_PAPERLESS_ESTADO_DOC";
            List<Ent_Contabilidad_EstadoDocumento_Det> listar = null;
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
                            cmd.Parameters.AddWithValue("@ruc", ruc);
                            cmd.Parameters.AddWithValue("@login", login_ws);
                            cmd.Parameters.AddWithValue("@clave", clave_ws);
                            cmd.Parameters.AddWithValue("@tipoDoc", tipodoc);
                            cmd.Parameters.AddWithValue("@folio", folio);
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_Contabilidad_EstadoDocumento_Det>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_Contabilidad_EstadoDocumento_Det()
                                          {
                                              PDF = dr["PDF"].ToString(),
                                              ESTADO = dr["ESTADO"].ToString()
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
