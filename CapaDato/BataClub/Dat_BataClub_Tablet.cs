using CapaEntidad.BataClub;
using CapaEntidad.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapaDato.BataClub
{
    public class Dat_BataClub_Tablet
    {
        public List<Ent_BataClub_Preg_Encuesta> get_ListaPromo_Disp()
        {
            List<Ent_BataClub_Preg_Encuesta> list = null;
            string sqlquery = "USP_BATACLUB_GET_ENCUESTA";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_BataClub_Preg_Encuesta>();
                            Ent_BataClub_Preg_Encuesta preg = new Ent_BataClub_Preg_Encuesta();
                            while (dr.Read())
                            {
                                preg = new Ent_BataClub_Preg_Encuesta();
                                preg.COD_PREG_ENC = dr["COD_PREG_ENC"].ToString();
                                preg.COD_TIPO_PREG = dr["COD_TIPO_PREG"].ToString();
                                preg.NOMBRE = dr["NOMBRE"].ToString();
                                preg.DESCRIPCION =dr["DESCRIPCION"].ToString();
                                preg.VALOR_MIN = Convert.ToInt32(dr["VALOR_MIN"].ToString());
                                preg.VALOR_MAX = Convert.ToInt32(dr["VALOR_MAX"].ToString());
                                preg.ESTADO_PREG = Convert.ToBoolean(dr["ESTADO_PREG"].ToString());
                                preg.FEC_INGRESO =Convert.ToDateTime( dr["FEC_INGRESO"].ToString());
                                preg.FEC_MODIF = Convert.ToDateTime(dr["FEC_MODIF"].ToString());
                                preg.USUARIO = dr["USUARIO"].ToString();
                                list.Add(preg);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                list = null;
            }
            return list;
        }
        public Ent_BataClub_Encuesta BATACLUB_VALIDAR_COMPROBANTE_ENCUESTA(string cod_entid , string serie , string numero)
        {
            Ent_BataClub_Encuesta encuesta = null;
            string sqlquery = "USP_BATACLUB_VALIDAR_COMPROBANTE_ENCUESTA";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        DataTable dt = new DataTable();
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cod_entid", cod_entid );
                        cmd.Parameters.AddWithValue("@serie", serie );
                        cmd.Parameters.AddWithValue("@numero", numero);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        if (dt != null && dt.Rows.Count> 0)
                        {
                            encuesta = new Ent_BataClub_Encuesta();
                            encuesta.COD_ENTID = dt.Rows[0]["COD_ENTID"].ToString();
                            encuesta.FC_NINT = dt.Rows[0]["FC_NINT"].ToString();
                            encuesta.FC_SFAC = dt.Rows[0]["FC_SFAC"].ToString();
                            encuesta.FC_NFAC = dt.Rows[0]["FC_NFAC"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                encuesta = null;
            }
            return encuesta;
        }
        public bool BATACLUB_SET_ENCUESTA(Ent_BataClub_Encuesta encuesta, ref string _mensaje)
        {
            bool res = false;
            string sqlquery = "USP_BATACLUB_SET_ENCUESTA";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        DataTable dt_resp = new DataTable();

                        if (encuesta.respuestas != null)
                        {
                            dt_resp = _toDTListRptas(encuesta.respuestas);
                        }
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@COD_TDA_ENC", encuesta.COD_TDA_ENC);
                        cmd.Parameters.AddWithValue("@COD_ENTID", encuesta.COD_ENTID);
                        cmd.Parameters.AddWithValue("@FC_NINT", encuesta.FC_NINT );
                        cmd.Parameters.AddWithValue("@FC_SFAC", encuesta.FC_SFAC );
                        cmd.Parameters.AddWithValue("@FC_NFAC", encuesta.FC_NFAC );
                        cmd.Parameters.AddWithValue("@CORREO", encuesta.CORREO );
                        cmd.Parameters.AddWithValue("@DNI", encuesta.DNI);
                        cmd.Parameters.AddWithValue("@USUARIO", encuesta.USUARIO );
                        cmd.Parameters.AddWithValue("@TYPE_BATACLUB_RESP_ENCUESTA", dt_resp);
                        res= cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _mensaje = ex.Message;
                res = false;
            }
            return res;
        }
        private DataTable _toDTListRptas(List<Ent_BataClub_Resp_Encuesta> respuestas)
        {
            DataTable dtRet = new DataTable();
            dtRet.Columns.Add("COD_PREG_ENC");
            dtRet.Columns.Add("VALOR");
            foreach (var item in respuestas)
            {
                dtRet.Rows.Add(item.COD_PREG_ENC, item.VALOR);
            }
            return dtRet;
        }
    }
}
