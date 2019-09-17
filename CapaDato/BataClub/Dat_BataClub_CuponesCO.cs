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
    public class Dat_BataClub_CuponesCO
    {
        // Combo de Promociones disponibles (Cupón)
        public List<Ent_BataClub_ComboProm> get_ListaPromo_Disp()
        {
            List<Ent_BataClub_ComboProm> list = null;
            string sqlquery = "USP_BATACLUB_GET_PROMO_DISPONIBLE";
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
                            list = new List<Ent_BataClub_ComboProm>();
                            Ent_BataClub_ComboProm combo = new Ent_BataClub_ComboProm();
                            while (dr.Read())
                            {
                                combo = new Ent_BataClub_ComboProm();
                                combo.prom_id = dr["Prom_ID"].ToString();
                                combo.prom_des = dr["Prom_Des"].ToString();
                                list.Add(combo);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = null;
            }
            return list;
        }

        // Combo de Promociones
        public List<Ent_BataClub_Promociones> get_ListaPromociones()
        {
            List<Ent_BataClub_Promociones> list = null;
            string sqlquery = "USP_BATACLUB_PROMOCION_LISTA";
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
                            list = new List<Ent_BataClub_Promociones>();
                            Ent_BataClub_Promociones prom = new Ent_BataClub_Promociones();
                            while (dr.Read())
                            {
                                prom = new Ent_BataClub_Promociones();
                                prom.Codigo = dr["Codigo"].ToString();
                                prom.Descripcion = dr["Descripcion"].ToString();
                                prom.Porc_Dcto = dr["Porc_Dcto"].ToString();
                                prom.MaxPares = Convert.ToInt32( dr["MaxPares"].ToString());
                                prom.FechaFin = dr["FechaFin"].ToString();
                                prom.PromActiva = Convert.ToBoolean(dr["PromActiva"]);
                                list.Add(prom);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = null;
            }
            return list;
        }

        // Combo de Estados de cupón
        public List<Ent_BataClub_ComboEstCupon> get_ListaEstados()
        {
            List<Ent_BataClub_ComboEstCupon> list = null;
            string sqlquery = "USP_BATACLUB_GET_ESTADOS_CUPON";
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
                            list = new List<Ent_BataClub_ComboEstCupon>();
                            Ent_BataClub_ComboEstCupon combo = new Ent_BataClub_ComboEstCupon();
                            while (dr.Read())
                            {
                                combo = new Ent_BataClub_ComboEstCupon();
                                combo.est_id = dr["Est_Id"].ToString();
                                combo.est_des = dr["Est_Des"].ToString();
                                list.Add(combo);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = null;
            }
            return list;
        }

        //Listado Tabla principal
        public List<Ent_BataClub_Cupones> get_lista_cupones(string dni, string cupon, string prom, string correo,string estado)
        {
            string sqlquery = "USP_BATACLUB_CONSULTAR_CUPONES";
            List<Ent_BataClub_Cupones> listar = null;
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
                            cmd.Parameters.AddWithValue("@estado_con", 1);
                            cmd.Parameters.AddWithValue("@grupo", prom);
                            cmd.Parameters.AddWithValue("@dni", dni);
                            cmd.Parameters.AddWithValue("@cupon", cupon);
                            cmd.Parameters.AddWithValue("@correo", correo);
                            cmd.Parameters.AddWithValue("@estado", estado);
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_BataClub_Cupones>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_BataClub_Cupones()
                                          {
                                              promocion = dr["Promocion"].ToString(),
                                              estado = dr["Estado"].ToString(),
                                              fechaFin = Convert.ToDateTime(dr["FechaFin"]).ToString("dd-MM-yyyy"),
                                              nombresCliente = dr["Nombres"].ToString(),
                                              dniCliente = dr["Dni"].ToString(),
                                              correo = dr["Correo"].ToString(),
                                              cupon = dr["Cupon"].ToString(),
                                              porcDesc = Convert.ToDecimal(dr["Porc_Desc"].ToString()),
                                          }).ToList();                                    
                               }
                         }
                    }
                    catch (Exception ex)
                    {
                        var mensaje = ex.Message;
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

        public string get_detalles_cupon(string cupon)
        {
            string sqlquery = "USP_BATACLUB_CONSULTAR_CUPONES";
            string json = "";
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
                            cmd.Parameters.AddWithValue("@estado_con", 2);
                            cmd.Parameters.AddWithValue("@cupon_det", cupon);
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                json = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.None);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var mensaje = ex.Message;
                        json = "";
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception)
            {
                json = "";
            }
            return json;
        }


        // *******Cambiar procedimiento al original luego (Cupón)
        public List<Ent_BataClub_CuponesCO> getPromDet(string prom_id)
        {
            List<Ent_BataClub_CuponesCO> list = null;
            string sqlquery = "USP_BATACLUB_GET_PROMO_PARAMETROS_gft";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PROM_ID", prom_id);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_BataClub_CuponesCO>();
                            Ent_BataClub_CuponesCO cup = new Ent_BataClub_CuponesCO();
                            while (dr.Read())
                            {
                                cup = new Ent_BataClub_CuponesCO();
                                cup.porc_desc = dr["PORC_DESC"].ToString();
                                cup.max_pares = dr["MAX_PARES"].ToString();
                                cup.prom_des = dr["PROMOCION"].ToString();
                                // cup.cup_fecha_fin = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dr["FECHA_FIN"]));
                                cup.cup_fecha_fin = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dr["FECHA_FIN"]));
                                list.Add(cup);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = null;
            }
            return list;
        }

        //Listado Tabla principal Cupón (Cupón)
        public List<Ent_BataClub_CuponesCO> get_cliente(string dni_correo)
        {
            string sqlquery = "USP_BATACLUB_GET_EXISTE_CL";
            List<Ent_BataClub_CuponesCO> listar = null;
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
                            cmd.Parameters.AddWithValue("@DNI_CORREO", dni_correo);
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                listar = new List<Ent_BataClub_CuponesCO>();
                                listar = (from DataRow dr in dt.Rows
                                          select new Ent_BataClub_CuponesCO()
                                          {
                                              Nombres = dr["NOMBRES"].ToString(),
                                              Apellidos = dr["APELLIDOS"].ToString(),
                                              dni = dr["DNI"].ToString(),
                                              correo = dr["CORREO"].ToString()       
                                          }).ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var mensaje = ex.Message;
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

        //Gráfica del consumo de los cupones por mes
        public string listarStr_graph()
        {
            string strJson = "";
            try
            {
                SqlConnection cn = new SqlConnection(Ent_Conexion.conexion);
                cn.Open();
                SqlCommand oComando = new SqlCommand("USP_BataClub_Cupones_Grafica", cn);
                oComando.CommandType = CommandType.StoredProcedure;

                //SqlParameter oArticulo = oComando.Parameters.Add("@codArticulo", SqlDbType.VarChar);
                //oArticulo.Direction = ParameterDirection.Input;
                //oArticulo.Value = Cod_Articulo;

                SqlDataReader oReader = oComando.ExecuteReader(CommandBehavior.SingleResult);
                DataTable dataTable = new DataTable("row");
                dataTable.Load(oReader);

                strJson = JsonConvert.SerializeObject(dataTable, Newtonsoft.Json.Formatting.Indented);
                strJson = strJson.Replace(Environment.NewLine, "");
                //strJson = strJson.Replace(" ", "");
                cn.Close();
            }
            catch (Exception ex)
            {
                return strJson;
            }

            //return oLista;
            return strJson;
        }

        //Retorno de lista de clientes con la barra de cupón
        public static List<Ent_BataClub_ListaCliente> return_barra_list(Ent_BataClub_ListaItems list_cliente, decimal _pordes, DateTime fecfin, int _pares_max, string _tipo_des, decimal usu_id)
        {
            string sqlquery = "USP_BATACLUB_INSERTAR_CUPONES";
            List<Ent_BataClub_ListaCliente> list_return = null;
            try
            {
                if (list_cliente.Lista.Count() > 0)
                {
                    /*verificamos si en la lista hay valores; si esta así, entonces creamos un datatable temporal*/
                    DataTable dt = new DataTable();
                    dt.Columns.Add("dni", typeof(string));
                    dt.Columns.Add("nombres", typeof(string));
                    dt.Columns.Add("apellidos", typeof(string));
                    dt.Columns.Add("email", typeof(string));
                    dt.Columns.Add("barra", typeof(string));

                    foreach (var item in list_cliente.Lista)
                    {
                        dt.Rows.Add(item.dni, item.nombre, item.apellidos, item.email, item.barra);
                    }

                    using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@pordes", _pordes);
                            cmd.Parameters.AddWithValue("@fecfin", String.Format("{0:MM-dd-yyyy}", Convert.ToDateTime(fecfin)));
                            cmd.Parameters.AddWithValue("@paresmax", _pares_max);
                            cmd.Parameters.AddWithValue("@prom_des", _tipo_des);
                            cmd.Parameters.AddWithValue("@tmpcupones", dt);
                            cmd.Parameters.AddWithValue("@usu_id", usu_id);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dtbarra = new DataTable();
                                da.Fill(dtbarra);

                                if (dtbarra.Rows.Count > 0)
                                {
                                    list_return = new List<Ent_BataClub_ListaCliente>();

                                    list_return = (from item in dtbarra.AsEnumerable()
                                                   select new Ent_BataClub_ListaCliente
                                                   {
                                                       dni = item.Field<string>("dni"),
                                                       nombre = item.Field<string>("nombres"),
                                                       apellidos = item.Field<string>("apellidos"),
                                                       email = item.Field<string>("email"),
                                                       barra = item.Field<string>("barra"),
                                                   }).ToList();
                                }

                            }
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                list_return = null;
            }
            return list_return;
        }

        //Generación de listado de cupones
        public List<Ent_BataClub_ListaCliente> genera_list_barra(decimal usu_id, decimal pordes, DateTime fecfin, int paresmax, string prom_des, Ent_BataClub_ListaItems list_cliente)
        {          
            List<Ent_BataClub_ListaCliente> lista_return_barra = null;
            try
            {
                if (list_cliente.Lista.Count() > 0)
                {
                    lista_return_barra = return_barra_list(list_cliente, pordes, fecfin, paresmax, prom_des, usu_id);
                }
            }
            catch (Exception ex)
            {
                lista_return_barra = null;
            }
            return lista_return_barra;
        }

        //Generacion de cupones
        public List<Ent_BataClub_ListaCliente> GenerarCupones(decimal usu_id, decimal pordes, DateTime fecfin, int paresmax, string prom_des, DataTable dt_lista)
        {
            DataTable dtprueba = dt_lista; //datatable
            List<Ent_BataClub_ListaCliente> list_client = new List<Ent_BataClub_ListaCliente>();

            foreach (DataRow fila in dtprueba.Rows)
            {
                var item = new Ent_BataClub_ListaCliente();
                item.dni = fila["dni"].ToString();
                item.nombre = fila["Nombres"].ToString();
                item.apellidos = fila["Apellidos"].ToString();
                item.email = fila["correo"].ToString();
                list_client.Add(item);
            }
            var array = new Ent_BataClub_ListaItems();
            array.Lista = list_client.ToArray();

            var list_client_return = genera_list_barra( usu_id, pordes, fecfin, paresmax, prom_des, array);
 
            return list_client_return;

        }

    }
}
