using CapaEntidad.NPS;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CapaDato.NPS
{
    public class Dat_NPS_Dashboard
    {
        public Ent_NPS_Dashboard_Estado _getChartEstado(Ent_NPS_Dashboard_Estado _Ent)
        {
            Ent_NPS_Dashboard_Estado objResult = null;
            string sqlquery = "[USP_BATACLUB_NPS_DASHBOARD_ESTADO]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FEC_INI", DbType.DateTime).Value = _Ent.FEC_INI;
                        cmd.Parameters.AddWithValue("@FEC_FIN", DbType.DateTime).Value = _Ent.FEC_FIN;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            DataTable dt_Chart = ds.Tables[0];
                            DataTable dt_Excel = ds.Tables[1];
                            objResult = new Ent_NPS_Dashboard_Estado();
                            List<Ent_NPS_Estado_Chart> _ListarChar = new List<Ent_NPS_Estado_Chart>();
                            _ListarChar = (from DataRow fila in dt_Chart.Rows
                                           select new Ent_NPS_Estado_Chart()
                                           {
                                               COD_ESTADO = fila["COD_ESTADO"].ToString(),
                                               DES_ESTADO = fila["DES_ESTADO"].ToString(),
                                               TRANSAC = Convert.ToInt32(fila["TRANSAC"])
                                           }).ToList();
                            objResult._ListarChar = _ListarChar;

                            List<Ent_NPS_Estado_Lista> _ListarExcel = new List<Ent_NPS_Estado_Lista>();
                            _ListarExcel = (from DataRow fila in dt_Excel.Rows
                                            select new Ent_NPS_Estado_Lista()
                                            {
                                                DES_ESTADO = fila["DES_ESTADO"].ToString(),
                                                DNI = fila["DNI"].ToString(),
                                                CORREO = fila["CORREO"].ToString()
                                            }).ToList();
                            objResult._ListarExcel = _ListarExcel;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objResult = new Ent_NPS_Dashboard_Estado();
            }
            return objResult;
        }
        public List<Ent_NPS_Dashboard_Tipo> _Listar_Preguntas()
        {
            List<Ent_NPS_Dashboard_Tipo> Listar = new List<Ent_NPS_Dashboard_Tipo>();
            string sqlquery = "[USP_BATACLUB_NPS_DASHBOARD_PREGUNTA]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            Listar = new List<Ent_NPS_Dashboard_Tipo>();
                            Listar = (from DataRow dr in dt.Rows
                                      select new Ent_NPS_Dashboard_Tipo()
                                      {
                                          Codigo = Convert.ToString(dr["COD_NPS"]),
                                          Descripcion = Convert.ToString(dr["PREGUNTA_NPS"])
                                      }
                                    ).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Listar;
        }

        public List<Ent_Combo_DisCadTda> list_dis_cad_tda_NPS(string pais = "PE")
        {
            string sqlquery = "[USP_XSTORE_GET_DISTRITO_CAD_TDA_NPS]";
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
                                          cod_distri = fila["cod_distri"].ToString(),
                                          des_distri = fila["des_distri"].ToString(),
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

        public Ent_NPS_Dashboard_Tipo _getChartTipo(Ent_NPS_Dashboard_Tipo _Ent)
        {
            Ent_NPS_Dashboard_Tipo objResult = null;
            string sqlquery = "[USP_BATACLUB_NPS_DASHBOARD_TIPO]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FEC_INI", DbType.DateTime).Value = _Ent.FEC_INI;
                        cmd.Parameters.AddWithValue("@FEC_FIN", DbType.DateTime).Value = _Ent.FEC_FIN;
                        cmd.Parameters.AddWithValue("@COD_NPS", DbType.String).Value = _Ent.COD_NPS;
                        cmd.Parameters.AddWithValue("@CODTDA", DbType.String).Value = _Ent.CODTDA;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            DataTable dt_Chart = ds.Tables[0];
                            DataTable dt_Nota = ds.Tables[1];
                            DataTable dt_Excel = ds.Tables[2];
                            objResult = new Ent_NPS_Dashboard_Tipo();

                            List<Ent_NPS_Tipo_Chart> _ListarChar = new List<Ent_NPS_Tipo_Chart>();
                            _ListarChar = (from DataRow fila in dt_Chart.Rows
                                           select new Ent_NPS_Tipo_Chart()
                                           {
                                               DES_TIP_RES = fila["DES_TIP_RES"].ToString(),
                                               VALOR_PORC = (Decimal)fila["VALOR_PORC"],
                                               COLOR_RGBA = fila["COLOR_RGBA"].ToString()
                                           }).ToList();
                            objResult._ListarChar = _ListarChar;

                            List<Ent_NPS_Tipo_Nota> _ListarNota = new List<Ent_NPS_Tipo_Nota>();
                            _ListarNota = (from DataRow fila in dt_Nota.Rows
                                           select new Ent_NPS_Tipo_Nota()
                                           {
                                               NOTA = (fila["NOTA"] is DBNull)? (int?)null : Convert.ToInt32(fila["NOTA"])
                                           }).ToList();
                            objResult._ListarNota = _ListarNota;

                            List<Ent_NPS_Tipoo_Lista> _ListarExcel = new List<Ent_NPS_Tipoo_Lista>();
                            _ListarExcel = (from DataRow fila in dt_Excel.Rows
                                            select new Ent_NPS_Tipoo_Lista()
                                            {
                                                TIPO = fila["TIPO"].ToString(),
                                                DNI = fila["DNI"].ToString(),
                                                CORREO = fila["CORREO"].ToString()
                                            }).ToList();
                            objResult._ListarExcel = _ListarExcel;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objResult = new Ent_NPS_Dashboard_Tipo();
            }
            return objResult;
        }

        public Ent_NPS_Dashboard_Distrito _getCaliPorDistrito(Ent_NPS_Dashboard_Distrito _Ent)
        {
            Ent_NPS_Dashboard_Distrito objResult = null;
            string sqlquery = "[USP_BATACLUB_NPS_DAHSBOARD_DISTRITO]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FEC_INI", DbType.DateTime).Value = _Ent.FEC_INI;
                        cmd.Parameters.AddWithValue("@FEC_FIN", DbType.DateTime).Value = _Ent.FEC_FIN;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            DataTable dt_Chart = ds.Tables[0];
                            DataTable dt_Excel = ds.Tables[1];
                            objResult = new Ent_NPS_Dashboard_Distrito();

                            List<Ent_NPS_Distrito_Chart> _ListarChar = new List<Ent_NPS_Distrito_Chart>();
                            _ListarChar = (from DataRow fila in dt_Chart.Rows
                                           select new Ent_NPS_Distrito_Chart()
                                           {
                                               COD_NPS = fila["COD_NPS"].ToString(),
                                               PREGUNTA_NPS = fila["PREGUNTA_NPS"].ToString(),
                                               DISTRITO = fila["DISTRITO"].ToString(),
                                               NOTA = Convert.ToInt32(fila["NOTA"]),
                                               COLOR_RGBA = fila["COLOR_RGBA"].ToString()
                                           }).ToList();
                            objResult._ListarChar = _ListarChar;

                            List<Ent_NPS_Distrito_Listar> _ListarExcel = new List<Ent_NPS_Distrito_Listar>();
                            _ListarExcel = (from DataRow fila in dt_Excel.Rows
                                            select new Ent_NPS_Distrito_Listar()
                                            {
                                                DISTRITO = fila["DISTRITO"].ToString(),
                                                TIENDA = fila["TIENDA"].ToString(),
                                                COD_NPS = fila["COD_NPS"].ToString(),
                                                PREGUNTA_NPS = fila["PREGUNTA_NPS"].ToString(),
                                                NOTA = Convert.ToInt32(fila["NOTA"]),
                                                COLOR_RGBA = fila["COLOR_RGBA"].ToString(),

                                            }).ToList();
                            objResult._ListarExcel = _ListarExcel;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objResult = new Ent_NPS_Dashboard_Distrito();
            }
            return objResult;
        }
    }
}
