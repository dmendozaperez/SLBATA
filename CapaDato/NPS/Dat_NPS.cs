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
    public class Dat_NPS
    {
        //public Ent_NPS_Pregunta 
        public Ent_NPS_Pregunta Leer_Preguntas(Ent_NPS_Pregunta _Ent)
        {
            string sqlquery = "USP_BATACLUB_NPS_CONSULTA_PREGUNTA";
            Ent_NPS_Pregunta objResult = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", DbType.String).Value = _Ent.ID;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);

                            DataTable dt_Estado = ds.Tables[0];
                            DataTable dt_Pregunstas = ds.Tables[1];

                            objResult = new Ent_NPS_Pregunta();
                            objResult.COD_NPS_EST = dt_Estado.Rows[0]["COD_NPS_EST"].ToString();

                            List<Ent_NPS_Pregunta_Det> _Preguntas = new List<Ent_NPS_Pregunta_Det>();
                            _Preguntas = (from DataRow fila in dt_Pregunstas.Rows
                                                  select new Ent_NPS_Pregunta_Det()
                                                  {
                                                      ID = Convert.ToDecimal(fila["ID"]),
                                                      COD_NPS_CON = fila["COD_NPS_CON"].ToString(),
                                                      COD_NPS = fila["COD_NPS"].ToString(),
                                                      PREGUNTA_NPS = fila["PREGUNTA_NPS"].ToString(),
                                                      COD_NPS_OPC = (fila["COD_NPS_OPC"] is DBNull) ? (Decimal?)null : Convert.ToDecimal(fila["COD_NPS_OPC"]),
                                                      DES_NPS_OPC = fila["DES_NPS_OPC"].ToString(),
                                                      COD_NPS_EST = fila["COD_NPS_EST"].ToString(),
                                                      RANGO_NPS = Convert.ToBoolean(fila["RANGO_NPS"]),
                                                      VALOR_MIN_NPS = Convert.ToInt32(fila["VALOR_MIN_NPS"]),
                                                      VALOR_MAX_NPS = Convert.ToInt32(fila["VALOR_MAX_NPS"])
                                                  }
                                                ).ToList();
                            objResult._ListarPreguntas = _Preguntas;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                objResult = new Ent_NPS_Pregunta();
            }
            return objResult;
        }

        public bool Genera_Encuesta(DataTable dt, Ent_TMP_NPS_Respuestas ent)
        {
            string sqlquery = "USP_BATACLUB_NPS_ACTUALIZA_RESPUESTA";
            bool result = false;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Ent_Conexion.conexion);
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", ent.ID);
                cmd.Parameters.AddWithValue("@COMENTARIO", ent.COMENTARIO);                
                cmd.Parameters.AddWithValue("@TMP", dt);
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch (Exception exc)
            {
                result = false;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return result;
        }


        public void Genera_Encuesta_Estado(Ent_NPS_Pregunta ent)
        {
            string sqlquery = "USP_BATACLUB_NPS_ACTUALIZA_ESTADO";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Ent_Conexion.conexion);
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", ent.ID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            if (cn.State == ConnectionState.Open) cn.Close();
        }

        
    }
}
