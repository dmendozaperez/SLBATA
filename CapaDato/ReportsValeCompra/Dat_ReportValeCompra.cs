using System;
using CapaEntidad.ReportsValeCompra;
using CapaEntidad.Util;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.ReportsValeCompra
{
    public class Dat_ReportValeCompra
    {
        public List<Reporte_Resultado> listarReporte(Reporte_Filtro filtro)  
        {
            string sqlquery = "USP_ListarValesCompra";
            List<Reporte_Resultado> lista = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {


                        SqlParameter oEstado = cmd.Parameters.Add("@Estado", SqlDbType.VarChar);
                        oEstado.Direction = ParameterDirection.Input;
                        oEstado.Value = filtro.report_Estado;

                        SqlParameter ostrRuc = cmd.Parameters.Add("@strRucCliente", SqlDbType.VarChar);
                        ostrRuc.Direction = ParameterDirection.Input;
                        ostrRuc.Value = filtro.report_listRuc;


                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            lista = new List<Reporte_Resultado>();
                            lista = (from DataRow dr in dt.Rows
                                     select new Reporte_Resultado()
                                     {
                                       
                                         rep_dni = dr["rep_dni"].ToString(),
                                         rep_nombre = dr["rep_nombre"].ToString(),
                                         rep_apellidoPater = dr["rep_apellidoPater"].ToString(),
                                         rep_apellidoMater = dr["rep_apellidoMater"].ToString(),
                                         rep_email = dr["rep_email"].ToString(),

                                         rep_CupBarra = dr["rep_CupBarra"].ToString(),
                                         rep_CupNumero = dr["rep_CupNumero"].ToString(),
                                         rep_CupMonto = dr["rep_CupMonto"].ToString(),
                                         rep_CupEstado = dr["rep_CupEstado"].ToString(),

                                         rep_docTipo = dr["rep_docTipo"].ToString(),
                                         rep_docSerie = dr["rep_docSerie"].ToString(),
                                         rep_docNro = dr["rep_docNro"].ToString(),
                                         rep_docfecha = dr["rep_docfecha"].ToString(),

                                         rep_tdaCodigo = dr["rep_tdaCodigo"].ToString(),
                                         rep_tdaDesc = dr["rep_tdaDesc"].ToString(),
                                         rep_tdaDirec = dr["rep_tdaDirec"].ToString(),


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

    }
}
