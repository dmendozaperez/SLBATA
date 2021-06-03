using CapaEntidad.Util;
using CapaEntidad.Transitos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Transitos
{
    public class Dat_Transitos
    {
        public List<Ent_Combo_DisCadTda> List_Emp_Cad_tda(string pais = "PE")
        {
            string sqlquery = "[USP_LEER_EMP_CAD_TDA]";
            List<Ent_Combo_DisCadTda> listar = null;
            DataTable dt = null;
            try
            {
                listar = new List<Ent_Combo_DisCadTda>();
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pais", DbType.String).Value = pais;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dt = new DataTable();
                            da.Fill(dt);
                            listar = (from DataRow fila in dt.Rows
                                      select new Ent_Combo_DisCadTda()
                                      {
                                          cod_distri = fila["cod_empresa"].ToString(),
                                          des_distri = fila["des_empresa"].ToString(),
                                          cod_cadena = fila["cod_cadena"].ToString(),
                                          des_cadena = fila["des_cadena"].ToString(),
                                          cod_entid = fila["cod_entid"].ToString(),
                                          des_entid = fila["des_entid"].ToString(),
                                      }).ToList();
                        }
                    }
                }
            }
            catch
            {
                listar = new List<Ent_Combo_DisCadTda>();
            }
            return listar;
        }
        public List<Ent_concepto_Transitos> List_Concepto_Transitos()
        {
            List<Ent_concepto_Transitos> listar = null;
            string sqlquery = "USP_LEER_CONCEPTO";
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
                            listar = new List<Ent_concepto_Transitos>();
                            listar = (from DataRow fila in dt.Rows
                                      select new Ent_concepto_Transitos()
                                      {
                                          Con_Id = fila["con_id"].ToString(),
                                          Con_Des = fila["con_des"].ToString(),
                                          Con_Tran = Convert.ToBoolean(fila["con_tran"]),
                                          Descripcion = Convert.ToString(fila["Descripcion"]),
                                      }
                                    ).ToList().Where(a => a.Con_Tran == true).ToList();
                        }
                    }
                }
            }
            catch
            {
                listar = new List<Ent_concepto_Transitos>();
            }
            return listar;
        }
        public List<Ent_Articulo_Transitos> Lista_Articulo_Transitos(string pais)
        {
            List<Ent_Articulo_Transitos> listar = null;
            string sqlquery = "USP_LISTA_ARTICULO_X_PAIS";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PAIS", pais);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            listar = new List<Ent_Articulo_Transitos>();
                            listar = (from DataRow fila in dt.Rows
                                      select new Ent_Articulo_Transitos()
                                      {
                                          Cod_Artic = fila["cod_artic"].ToString(),
                                          Des_Artic = fila["des_artic"].ToString(),
                                      }
                                    ).ToList();
                        }
                    }
                }
            }
            catch
            {
                listar = new List<Ent_Articulo_Transitos>();
            }
            return listar;
        }
        public List<Ent_Consulta_Transitos> ListarConsulta_Transitos(Ent_Consulta_Transitos _Ent)
        {
            List<Ent_Consulta_Transitos> Listar;
            string sqlquery = "[Usp_Transito_Documento]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fecha", DbType.String).Value = _Ent.Fecha;
                        cmd.Parameters.AddWithValue("@empresa", DbType.String).Value = _Ent.Empresa;
                        cmd.Parameters.AddWithValue("@cadena", DbType.String).Value = _Ent.Cadena;
                        cmd.Parameters.AddWithValue("@concepto", DbType.String).Value = _Ent.Concepto;
                        cmd.Parameters.AddWithValue("@destino", DbType.String).Value = _Ent.Destino;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            Listar = new List<Ent_Consulta_Transitos>();
                            Listar = (from DataRow fila in dt.Rows
                                      select new Ent_Consulta_Transitos()
                                      {
                                          Empre = (fila["Empre"] is DBNull) ? string.Empty : (string)(fila["Empre"]),
                                          Caden = (fila["Caden"] is DBNull) ? string.Empty : (string)(fila["Caden"]),
                                          Concepto = (fila["Concepto"] is DBNull) ? string.Empty : (string)(fila["Concepto"]),
                                          Origen = (fila["Origen"] is DBNull) ? string.Empty : (string)(fila["Origen"]),
                                          Destino = (fila["Destino"] is DBNull) ? string.Empty : (string)(fila["Destino"]),
                                          Guia = (fila["Guia"] is DBNull) ? string.Empty : (string)(fila["Guia"]),
                                          Calz = (fila["Calz"] is DBNull) ? (Decimal?)null : Convert.ToDecimal(fila["Calz"]),
                                          No_Calz = (fila["No_Calz"] is DBNull) ? (Decimal?)null : Convert.ToDecimal(fila["No_Calz"]),
                                          Cajas = (fila["Cajas"] is DBNull) ? (Decimal?)null : Convert.ToDecimal(fila["Cajas"]),
                                          Estado = (fila["Estado"] is DBNull) ? string.Empty : (string)(fila["Estado"]),
                                          Tran_Ini = (fila["Tran_Ini"] is DBNull) ? (DateTime?)null : Convert.ToDateTime(fila["Tran_Ini"]),
                                          Tran_Fin = (fila["Tran_Fin"] is DBNull) ? (DateTime?)null : Convert.ToDateTime(fila["Tran_Fin"])
                                      }
                                    ).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Listar = new List<Ent_Consulta_Transitos>();
            }
            return Listar;
        }

        public List<Ent_Articulo_TransitosArt> ListarConsulta_TransitosArt(Ent_Articulo_TransitosArt _Ent)
        {
            List<Ent_Articulo_TransitosArt> Listar;
            string sqlquery = "[usp_transito_articulo]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fecha", DbType.String).Value = _Ent.Fecha;
                        cmd.Parameters.AddWithValue("@empresa", DbType.String).Value = _Ent.Empresa;
                        cmd.Parameters.AddWithValue("@cadena", DbType.String).Value = _Ent.Cadena;
                        cmd.Parameters.AddWithValue("@concepto", DbType.String).Value = _Ent.Concepto;
                        cmd.Parameters.AddWithValue("@destino", DbType.String).Value = _Ent.Destino;
                        cmd.Parameters.AddWithValue("@articulo", DbType.String).Value = _Ent.Articulo;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            Listar = new List<Ent_Articulo_TransitosArt>();
                            Listar = (from DataRow fila in dt.Rows
                                      select new Ent_Articulo_TransitosArt()
                                      {
                                          Empresa = (fila["Empre"] is DBNull) ? string.Empty : (string)(fila["Empre"]),
                                          Cadena = (fila["Caden"] is DBNull) ? string.Empty : (string)(fila["Caden"]),
                                          Concepto = (fila["Concepto"] is DBNull) ? string.Empty : (string)(fila["Concepto"]),
                                          Origen = (fila["Origen"] is DBNull) ? string.Empty : (string)(fila["Origen"]),
                                          Destino = (fila["Destino"] is DBNull) ? string.Empty : (string)(fila["Destino"]),
                                          Guia = (fila["Guia"] is DBNull) ? string.Empty : (string)(fila["Guia"]),
                                          Articulo = (fila["Articulo"] is DBNull) ? string.Empty : (string)(fila["Articulo"]),
                                          Cal = (fila["Cal"] is DBNull) ? string.Empty : (string)(fila["Cal"]),
                                          Talla = (fila["Talla"] is DBNull) ? string.Empty : (string)(fila["Talla"]),
                                          Cantidad = (fila["Cantidad"] is DBNull) ? (int?)null : Convert.ToInt32(fila["Cantidad"])
                                      }
                                    ).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Listar = new List<Ent_Articulo_TransitosArt>();
            }
            return Listar;
        }

        public Ent_Consulta_Transitos_Doc ValConsultaDoc(Ent_Consulta_Transitos_Doc ent)
        {
            Ent_Consulta_Transitos_Doc _EntResul = new Ent_Consulta_Transitos_Doc();
            string sqlquery = "[USP_transito_consulta_doc]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@concepto", DbType.String).Value = ent.Concepto;
                        cmd.Parameters.AddWithValue("@documento", DbType.String).Value = ent.NroDocumento;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count>0)
                            {
                                var results = from Row in dt.AsEnumerable()
                                              select new
                                              {
                                                  Concepto = Row.Field<string>("Concepto"),
                                                  NroDocumento = Row.Field<string>("Documento"),
                                                  Estado = Row.Field<string>("Estado"),
                                                  Fecha = Row.Field<DateTime>("Fecha"),
                                                  Origen = Row.Field<string>("Origen"),
                                                  Destino = Row.Field<string>("Destino"),
                                                  Cantidad = Row.Field<Decimal>("cantidad")
                                              };

                                foreach (var item in results)
                                {
                                    _EntResul.Concepto = item.Concepto;
                                    _EntResul.NroDocumento = item.NroDocumento;
                                    _EntResul.Estado = item.Estado;
                                    _EntResul.Fecha = item.Fecha;
                                    _EntResul.Origen = item.Origen;
                                    _EntResul.Destino = item.Destino;
                                    _EntResul.Cantidad = item.Cantidad;
                                }
                            }
                            else
                            {
                                _EntResul = new Ent_Consulta_Transitos_Doc();
                            } 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _EntResul;
        }
        public Ent_Consulta_Transitos_Doc DelAnularTransito(Ent_Consulta_Transitos_Doc _Ent)
        {
            Ent_Consulta_Transitos_Doc EntResul = new Ent_Consulta_Transitos_Doc();
            string sqlquery = "[USP_transito_anula]";
            int Resul = 0;
            string Mensaje = "";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Ent_Conexion.conexion);
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@concepto", DbType.String).Value = _Ent.Concepto;
                cmd.Parameters.AddWithValue("@documento", DbType.String).Value = _Ent.NroDocumento;
                cmd.Parameters.AddWithValue("@fechaanul", DbType.DateTime).Value = _Ent.FechaAnulacion;
                cmd.Parameters.AddWithValue("@usuario", DbType.String).Value = _Ent.Autorizado;
                cmd.Parameters.AddWithValue("@referencia", DbType.String).Value = _Ent.Referencia;
                cmd.Parameters.Add("@isOK", SqlDbType.Int, 500);
                cmd.Parameters["@isOK"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500);
                cmd.Parameters["@Mensaje"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                EntResul.IsOk = Convert.ToInt32(cmd.Parameters["@isOK"].Value);
                EntResul.Mensaje = Convert.ToString(cmd.Parameters["@Mensaje"].Value);
            }
            catch
            {
                EntResul = new Ent_Consulta_Transitos_Doc();
                EntResul.IsOk = -1;
                EntResul.Mensaje = "Se produjo un error al tratar de anular, consulte al Administrador.";
            }
            return EntResul;
        }
    }
}
