using CapaEntidad.ECommerce;
using CapaEntidad.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.ECommerce
{
    public class Dat_PedNoFactu
    {

        public List<Ent_PedidoNoFactu> get_data(string nroped, string tienda)
        {
            //var dt = new DataTable();
            string sqlquery = "usp_get_ped_nofactu";
            List<Ent_PedidoNoFactu> list = null;

            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@nroped", nroped);
                        cmd.Parameters.AddWithValue("@codtienda", tienda);
                        //using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        //{
                        //    //dt = new DataTable();
                        //    da.Fill(dt);
                        //}

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_PedidoNoFactu>();

                            while (dr.Read())
                            {
                                Ent_PedidoNoFactu enti = new Ent_PedidoNoFactu();
                                enti.id_pedido = dr["id_pedido"].ToString();
                                enti.cod_tienda = dr["cod_tienda"].ToString();
                                enti.nom_tienda = dr["nom_tienda"].ToString();
                                enti.cod_articulo = dr["cod_articulo"].ToString();
                                enti.nom_articulo = dr["nom_articulo"].ToString();
                                enti.estado = dr["estado"].ToString();
                                enti.estado_ob = dr["estado_ob"].ToString();
                                enti.nro_comprob = dr["nro_comprob"].ToString();
                                list.Add(enti);
                            }
                        }
                    }
                }
            }
            catch (Exception  ex)
            {
                list = null;
            }

            return list;
        }

    }
}
