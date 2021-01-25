
using CapaEntidad.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDato.Reporte
{
    public class Dat_Combo
    {
       public List<Ent_Combo> get_ListaGrupo()
        {
            List<Ent_Combo> list = null;
            string sqlquery = "Leer_Grupo";
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

                            while (dr.Read())
                            {
                                Ent_Combo combo = new Ent_Combo();
                                combo.cbo_codigo = dr["cod_line3"].ToString();
                                combo.cbo_descripcion = dr["des_line3"].ToString();                            
                                
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
        
               

        public string listarStr_ListaGrupoTipo()
        {
            string strJson = "";
            try
            {
                SqlConnection cn = new SqlConnection(Ent_Conexion.conexion);
                cn.Open();
                SqlCommand oComando = new SqlCommand("USP_GET_GRUPO", cn);
                oComando.CommandType = CommandType.StoredProcedure;

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

        public string listarStr_ListaTienda(string pais)
        {
            string strJson = "";
            try
            {
                SqlConnection cn = new SqlConnection(Ent_Conexion.conexion);
                cn.Open();
                SqlCommand oComando = new SqlCommand("USP_LISTAR_TIENDAS", cn);
                oComando.CommandTimeout = 0;
                oComando.CommandType = CommandType.StoredProcedure;
                //oComando.Parameters.AddWithValue("@PAIS", pais);
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
        public List<Ent_Combo> get_ListaTipoCategoria()
        {
            List<Ent_Combo> list = null;
            string sqlquery = "USP_GET_TIPOCATEGORIA";
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

                            while (dr.Read())
                            {
                                Ent_Combo combo = new Ent_Combo();
                                combo.cbo_codigo = dr["CAT_TIP_COD"].ToString();
                                combo.cbo_descripcion = dr["CAT_TIP_DES"].ToString();

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

        public List<Ent_Combo> get_lista_anios( int a_inicio)
        {
            List<Ent_Combo> list = null;
            string sqlquery = "USP_GET_LISTA_ANIOS";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ANIO_INICIO", a_inicio);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Combo>();

                            while (dr.Read())
                            {
                                Ent_Combo combo = new Ent_Combo();
                                combo.cbo_codigo = dr["cod"].ToString();
                                combo.cbo_descripcion = dr["anio"].ToString();

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


        public List<Ent_Combo> get_ListaSemana()
        {
            List<Ent_Combo> list = null;
            string sqlquery = "USP_GET_SEMANA";
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

                            while (dr.Read())
                            {
                                Ent_Combo combo = new Ent_Combo();
                                combo.cbo_codigo = dr["cbo_codigo"].ToString();
                                combo.cbo_descripcion = dr["cbo_descripcion"].ToString();

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
        public List<Ent_Combo> get_ListaTiendaXstore(string pais, Boolean _selecciona=false)
        {
            List<Ent_Combo> list = null;
            string sqlquery = "USP_LISTAR_TIENDA_XSTORE";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pais", pais);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Combo>();
                            Ent_Combo combo = new Ent_Combo();
                            //if (_selecciona)
                            //{
                            //    combo.cbo_codigo = "-1";
                            //    combo.cbo_descripcion = "--SELECCIONAR--";
                            //    list.Add(combo);
                            //}

                            while (dr.Read())
                            {
                                combo = new Ent_Combo();
                                combo.cbo_codigo = dr["codigo"].ToString();
                                combo.cbo_descripcion = dr["descrip"].ToString();

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


        /*lista xstore Bata Ecuador*/
        public List<Ent_Combo> get_ListaTiendaXstore_EC(string Pais)
        {
            List<Ent_Combo> list = null;
            string sqlquery = "USP_LISTAR_TIENDA_XSTORE";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pais", Pais);
                        
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Combo>();
                            Ent_Combo combo = new Ent_Combo();
                            //if (_selecciona)
                            //{
                            //    combo.cbo_codigo = "-1";
                            //    combo.cbo_descripcion = "--SELECCIONAR--";
                            //    list.Add(combo);
                            //}

                            while (dr.Read())
                            {
                                combo = new Ent_Combo();
                                combo.cbo_codigo = dr["codigo"].ToString();
                                combo.cbo_descripcion = dr["descrip"].ToString();

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



        public List<Ent_Combo> get_ListaTiendaXstoreActivo(string pais,string codTda = "")
        {
            List<Ent_Combo> list = null;
            string sqlquery = "USP_LISTAR_TIENDA_XSTORE_ACTIVA";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter otda = cmd.Parameters.Add("@COD_TDA", SqlDbType.VarChar);
                        cmd.Parameters.AddWithValue("@pais", pais);
                        otda.Direction = ParameterDirection.Input;
                        otda.Value = codTda;

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Combo>();

                            while (dr.Read())
                            {
                                Ent_Combo combo = new Ent_Combo();
                                combo.cbo_codigo = dr["CODIGO"].ToString();
                                combo.cbo_descripcion = dr["DESCRIP"].ToString();

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
        public List<Ent_Combo> get_ListaCadena()
        {
            List<Ent_Combo> list = null;
            string sqlquery = "USP_LISTAR_CADENA";
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

                            while (dr.Read())
                            {
                                Ent_Combo combo = new Ent_Combo();
                                combo.cbo_codigo = dr["CODIGO"].ToString();
                                combo.cbo_descripcion = dr["DESCRIP"].ToString();

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

        public string listarStr_ListaCategoria(string strGrupo)
        {
            string strJson = "";
            try
            {
                SqlConnection cn = new SqlConnection(Ent_Conexion.conexion);
                cn.Open();
                SqlCommand oComando = new SqlCommand("USP_Leer_Categoria", cn);
                oComando.CommandType = CommandType.StoredProcedure;

                SqlParameter odepartamento = oComando.Parameters.Add("@Grupo", SqlDbType.VarChar);
                odepartamento.Direction = ParameterDirection.Input;
                odepartamento.Value = strGrupo;

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
        public List<Ent_Combo> get_ListaCategoria(string strGrupo)
        {
            List<Ent_Combo> list = null;
            string sqlquery = "Leer_Categoria";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Grupo", strGrupo);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Combo>();

                            while (dr.Read())
                            {
                                Ent_Combo combo = new Ent_Combo();
                                combo.cbo_codigo = dr["cod_cate3"].ToString();
                                combo.cbo_descripcion = dr["des_cate3"].ToString();

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

        public string listarStr_ListaSubCategoria(string strCate)
        {
            string strJson = "";
            try
            {
                SqlConnection cn = new SqlConnection(Ent_Conexion.conexion);
                cn.Open();
                SqlCommand oComando = new SqlCommand("USP_Leer_SubCategoria", cn);
                oComando.CommandType = CommandType.StoredProcedure;

                SqlParameter odepartamento = oComando.Parameters.Add("@Categoria", SqlDbType.VarChar);
                odepartamento.Direction = ParameterDirection.Input;
                odepartamento.Value = strCate;

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

        public List<Ent_Combo> get_ListaSubCategoria(string strCate)
        {
            List<Ent_Combo> list = null;
            string sqlquery = "Leer_SubCategoria";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Categoria", strCate);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Combo>();

                            while (dr.Read())
                            {
                                Ent_Combo combo = new Ent_Combo();
                                combo.cbo_codigo = dr["cod_subc3"].ToString();
                                combo.cbo_descripcion = dr["des_subc3"].ToString();

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

        public List<Ent_Combo> get_ListaEstado()
        {
            List<Ent_Combo> list = null;
            string sqlquery = "Leer_Estado";
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

                            while (dr.Read())
                            {
                                Ent_Combo combo = new Ent_Combo();
                                combo.cbo_codigo = dr["des_campo1"].ToString();
                                combo.cbo_descripcion = dr["des_campo2"].ToString();

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

        public Ent_ComboList Listar_Filtros_OBS()
        {
            DataSet dsReturn = new DataSet();
            string sqlquery = "USP_LISTAR_FILTRO_OBS";
            List<Ent_Combo> ListCalidad = null;
            List<Ent_Combo> ListTipo = null;
            List<Ent_Combo> ListRango = null;

            Ent_ComboList ListaFiltro = new Ent_ComboList();         

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

                            da.Fill(dsReturn);
                            ListCalidad = new List<Ent_Combo>();
                            ListCalidad = (from DataRow dr in dsReturn.Tables[0].Rows
                                             select new Ent_Combo()
                                             {
                                                 cbo_codigo = dr["cbo_codigo"].ToString(),
                                                 cbo_descripcion = dr["cbo_descripcion"].ToString(),

                                             }).ToList();

                            ListTipo = new List<Ent_Combo>();
                            ListTipo = (from DataRow dr in dsReturn.Tables[1].Rows
                                           select new Ent_Combo()
                                           {
                                               cbo_codigo = dr["cbo_codigo"].ToString(),
                                               cbo_descripcion = dr["cbo_descripcion"].ToString(),

                                           }).ToList();

                            ListRango = new List<Ent_Combo>();
                            ListRango = (from DataRow dr in dsReturn.Tables[2].Rows
                                           select new Ent_Combo()
                                           {
                                               cbo_codigo = dr["cbo_codigo"].ToString(),
                                               cbo_descripcion = dr["cbo_descripcion"].ToString(),

                                           }).ToList();



                        }
                    }
                }

                ListaFiltro.Lista_1 = ListCalidad;
                ListaFiltro.Lista_2 = ListTipo;
                ListaFiltro.Lista_3 = ListRango;


            }
            catch (Exception exc)
            {

                ListaFiltro = null;
            }
            return ListaFiltro;
        }

        public List<Ent_Combo> get_ListaCalidad()
        {
            List<Ent_Combo> list = null;
            string sqlquery = "USP_LISTAR_CALIDAD";
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

                            while (dr.Read())
                            {
                                Ent_Combo combo = new Ent_Combo();
                                combo.cbo_codigo = dr["cbo_codigo"].ToString();
                                combo.cbo_descripcion = dr["cbo_descripcion"].ToString();

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

        public List<Ent_Combo> get_ListaEstadoPres()
        {
            List<Ent_Combo> list = null;
            string sqlquery = "USP_XSTORE_GET_ESTADO_INV";
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

                            while (dr.Read())
                            {
                                Ent_Combo combo = new Ent_Combo();
                                combo.cbo_codigo = dr["EST_XOF_COD"].ToString();
                                combo.cbo_descripcion = dr["EST_XOF_DES"].ToString();
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

        public List<Ent_Combo> get_ListaTipos2()
        {
            List<Ent_Combo> list = null;
            string sqlquery = "USP_GET_CONCEPTO_MOVLAM";
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

                            while (dr.Read())
                            {
                                Ent_Combo combo = new Ent_Combo();
                                combo.cbo_codigo = dr["CON_ID"].ToString();
                                combo.cbo_descripcion = dr["CON_DES"].ToString();
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
    }
}
