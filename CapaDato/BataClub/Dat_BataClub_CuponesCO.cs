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
        public List<Ent_BataClub_Promociones> get_ListaPromo_Disp()
        {
            List<Ent_BataClub_Promociones> list = null;
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
                            list = new List<Ent_BataClub_Promociones>();
                            Ent_BataClub_Promociones combo = new Ent_BataClub_Promociones();
                            while (dr.Read())
                            {
                                combo = new Ent_BataClub_Promociones();
                                combo.Codigo = dr["Prom_ID"].ToString();
                                combo.Descripcion = dr["Prom_Des"].ToString();
                                combo.MaxPares = Convert.ToInt32(dr["Prom_Max_pares"].ToString());
                                combo.Porc_Dcto = Convert.ToDecimal(dr["Prom_Porc_Dcto"].ToString());
                                list.Add(combo);
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
                                prom.Porc_Dcto = Convert.ToDecimal(dr["Porc_Dcto"].ToString());
                                prom.MaxPares = Convert.ToInt32( dr["MaxPares"].ToString());
                                prom.FechaFin = Convert.ToDateTime(dr["FechaFin"]).ToString("dd-MM-yyyy");
                                prom.PromActiva = Convert.ToBoolean(dr["PromActiva"]);
                                prom.nroCupones = Convert.ToInt32(dr["Num_Cupon"]);
                                prom.Coupon_Code = Convert.ToString(dr["Coupon_Code"]);
                                list.Add(prom);
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

        public List<Ent_BataClub_Cupones> get_ListaCuponesPromocion(string codProm)
        {
            List<Ent_BataClub_Cupones> list = null;
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
                        cmd.Parameters.AddWithValue("@prom_id", codProm);
                        cmd.Parameters.AddWithValue("@estado", 1);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_BataClub_Cupones>();
                            Ent_BataClub_Cupones cup = new Ent_BataClub_Cupones();
                            while (dr.Read())
                            {
                                cup = new Ent_BataClub_Cupones();
                                cup.promocion = dr["Promocion"].ToString();
                                cup.estado = dr["Estado"].ToString();
                                cup.dniCliente = dr["Dni"].ToString(); ;
                                cup.nombresCliente = dr["Nombres"].ToString(); ;
                                cup.apellidosCliente = dr["Apellidos"].ToString(); ;
                                cup.correo = dr["Email"].ToString(); ;
                                cup.cupon = dr["Barra"].ToString();                                 
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

        public List<Ent_Combo> get_ListaMeses()
        {
            List<Ent_Combo> list = null;
            string sqlquery = "USP_BATACLUB_LISTA_MESES";
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
                            list = new List<Ent_Combo>();
                            Ent_Combo cup = new Ent_Combo();
                            while (dr.Read())
                            {
                                cup = new Ent_Combo();
                                cup.cbo_codigo = dr["MesInt"].ToString();
                                cup.cbo_descripcion = dr["MesStr"].ToString();
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
        public List<Ent_BataClub_Cupones> get_cliente(string dni_correo)
        {
            string sqlquery = "USP_BATACLUB_GET_EXISTE_CL";
            List<Ent_BataClub_Cupones> list = null;
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
                                list = new List<Ent_BataClub_Cupones>();
                                list = (from DataRow dr in dt.Rows
                                          select new Ent_BataClub_Cupones()
                                          {
                                              nombresCliente = dr["Nombres"].ToString(),
                                              apellidosCliente = dr["Apellidos"].ToString(),
                                              dniCliente = dr["Dni"].ToString(),
                                              correo = dr["Email"].ToString()
                                          }).ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var mensaje = ex.Message;
                        list = null;
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception)
            {
                list = null;
            }
            return list;
        }
        public List<Ent_BataClub_Cupones> get_cliente(string mes , string genero)
        {
            string sqlquery = "USP_BATACLUB_LISTA_CLIENTES_PROM";
            List<Ent_BataClub_Cupones> list = null;
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
                            cmd.Parameters.AddWithValue("@MES_INT", mes);
                            cmd.Parameters.AddWithValue("@GENERO", genero);
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                list = new List<Ent_BataClub_Cupones>();
                                list = (from DataRow dr in dt.Rows
                                        select new Ent_BataClub_Cupones()
                                        {
                                            nombresCliente = dr["Nombres"].ToString(),
                                            apellidosCliente = dr["Apellidos"].ToString(),
                                            dniCliente = dr["Dni"].ToString(),
                                            correo = dr["Email"].ToString(),
                                            genero = dr["Genero"].ToString(),
                                            mesCumple = dr["MesCumple"].ToString(),
                                            miemBataClub =Convert.ToBoolean(dr["Miem_BataClub"])
                                        }).ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var mensaje = ex.Message;
                        list = null;
                    }
                    if (cn != null)
                        if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            catch (Exception)
            {
                list = null;
            }
            return list;
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
        public List<Ent_BataClub_Cupones> BATACLUB_INSERTAR_CUPONES(int operacion, decimal por_desc , DateTime fecha_fin , 
            decimal pares , string prom_des, decimal usu_id , List<Ent_BataClub_Cupones> clientes , string mesCumple , string genero, 
            string tiendas , string tiendas2 , string anio, DateTime fecha_ini , string prefix , ref string _prom_id , ref string mensaje)
        {
            string sqlquery = "USP_BATACLUB_INSERTAR_CUPONES_GRUPO"; // (operacion == 2 ? "USP_BATACLUB_INSERTAR_CUPONES" : "USP_BATACLUB_INSERTAR_CUPONES_GRUPO");         
            DataTable tmpcupones = new DataTable();
            string clientesArray = "";
            if (operacion == 2)
            {
                try
                {
                    clientesArray = String.Join(",", clientes.Select(s => s.dniCliente).ToArray());
                }
                catch { clientesArray = ""; }
            }                
            List<Ent_BataClub_Cupones> list = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@operacion", operacion);
                        cmd.Parameters.AddWithValue("@pordes", por_desc);
                        cmd.Parameters.AddWithValue("@fecini", fecha_ini);
                        cmd.Parameters.AddWithValue("@fecfin", fecha_fin);
                        cmd.Parameters.AddWithValue("@paresmax", pares);
                        cmd.Parameters.AddWithValue("@prom_des", prom_des);
                        if (operacion == 2)
                        {
                            cmd.Parameters.AddWithValue("@dni_str", clientesArray);
                        }
                        cmd.Parameters.AddWithValue("@usu_id", usu_id);                        
                        if (operacion == 1)
                        {
                            cmd.Parameters.AddWithValue("@Genero", genero);
                            cmd.Parameters.AddWithValue("@Mes_Int", mesCumple);//@Tiendas
                            cmd.Parameters.AddWithValue("@Tiendas", tiendas);
                            cmd.Parameters.AddWithValue("@tdas_reg", tiendas2);
                            cmd.Parameters.AddWithValue("@anio", anio);
                        }
                        cmd.Parameters.AddWithValue("@COUPON_CODE_PREFIX", prefix);
                        cmd.Parameters.Add("@ID_PROM", SqlDbType.VarChar, 5).Direction = ParameterDirection.Output;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dtCupones = new DataTable();
                        da.Fill(dtCupones);
                        if (dtCupones.Rows.Count > 0)
                        {
                            _prom_id = cmd.Parameters["@ID_PROM"].Value.ToString();                                                        
                            list = new List<Ent_BataClub_Cupones>();
                            list = (from DataRow dr in dtCupones.Rows
                                    select new Ent_BataClub_Cupones() {
                                        promocion = dr["Promocion"].ToString(),
                                        dniCliente = dr["Dni"].ToString(),
                                        nombresCliente = dr["Nombres"].ToString(),
                                        apellidosCliente = dr["Apellidos"].ToString(),
                                        correo = dr["Email"].ToString(),
                                        cupon = dr["Barra"].ToString()
                                        }).ToList();
                        }
                    }
                }
            }
            catch (Exception ex )
            {
                mensaje = ex.Message;
                _prom_id = "";
                list = null;
            }
            return list;
        }

        private DataTable _toDTListCli(List<Ent_BataClub_Cupones> clientes)
        {
            DataTable dtRet = new DataTable();
            dtRet.Columns.Add("dni");
            dtRet.Columns.Add("nombres");
            dtRet.Columns.Add("apellidos");
            dtRet.Columns.Add("email");
            dtRet.Columns.Add("barra");
            foreach (var item in clientes)
            {
                dtRet.Rows.Add(item.dniCliente, item.nombresCliente,item.apellidosCliente  , item.correo , "");
            }
            return dtRet;
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

        public List<Ent_BataClub_ListTdasProm> get_lista_det_tdas_prom(string prom_id)
        {
            List<Ent_BataClub_ListTdasProm> list = null;

            string sqlquery = "[USP_BATACLUB_GET_PROM_DET_TDA]";
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
                            list = new List<Ent_BataClub_ListTdasProm>();
                            while (dr.Read())
                            {
                                Ent_BataClub_ListTdasProm det_tdas = new Ent_BataClub_ListTdasProm();
                                det_tdas.prom_id = dr["PROM_ID"].ToString();
                                det_tdas.cod_tda = dr["COD_TDA"].ToString();
                                det_tdas.des_tda = dr["DES_TDA"].ToString();
                                det_tdas.des_cadena_tda = dr["DES_CADENA_TDA"].ToString();
                                list.Add(det_tdas);
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
        public List<Ent_BataClub_Orce_Promotion> GET_ORCE_PROMOTION(int op = 0 , string cod = "")
        {
            List<Ent_BataClub_Orce_Promotion> list = null;

            string sqlquery = "[USP_GET_ORCE_PROMOTION]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OP", op);
                        cmd.Parameters.AddWithValue("@Coupon_Code", cod);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_BataClub_Orce_Promotion>();
                            while (dr.Read())
                            {
                                Ent_BataClub_Orce_Promotion det_tdas = new Ent_BataClub_Orce_Promotion();
                                det_tdas.ORCE_COD_PROM = dr["ORCE_COD_PROM"].ToString();
                                det_tdas.ORCE_DES_PROM = dr["ORCE_DES_PROM"].ToString();
                                list.Add(det_tdas);
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
    }
}
