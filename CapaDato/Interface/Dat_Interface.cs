using System;
using CapaEntidad.ArticuloStock;
using CapaEntidad.Util;
using CapaEntidad.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CapaDato.Interface
{
    public class Dat_Interface
    {
        
        
        public List<Ent_Combo> listar_Pais(string opcion)
        {

            string sqlquery = "USP_Listar_Pais";
            List<Ent_Combo> lista = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {

                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@opcion", opcion );
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            lista = new List<Ent_Combo>();
                            lista = (from DataRow dr in dt.Rows
                                     select new Ent_Combo()
                                     {
                                         cbo_codigo = dr["cbo_codigo"].ToString(),
                                         cbo_descripcion = dr["cbo_descripcion"].ToString(),

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

        public string listarStr_Tienda(string pais)
        {
            string strJson = "";
            try
            {
                SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru);
                cn.Open();
                SqlCommand oComando = new SqlCommand("USP_GET_XSTORE_TIENDA_PAIS_WEB", cn);
                oComando.CommandType = CommandType.StoredProcedure;

                SqlParameter odepartamento = oComando.Parameters.Add("@PAIS", SqlDbType.VarChar);
                odepartamento.Direction = ParameterDirection.Input;
                odepartamento.Value = pais;

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


        public string listarStr_Interface()
        {
            string strJson = "";
            try
            {
                SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru);
                cn.Open();
                SqlCommand oComando = new SqlCommand("USP_OBTENER_INTERFACE", cn);
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


        public Boolean InsertarInterfaceTienda(Ent_TiendaInterface _InterfaceTienda)
        {
            //string sqlquery = "USP_Insertar_GeneracionValeCompra";
            string sqlquery = "USP_INSERTAR_INTERFACE_TIENDA";
            Boolean valida = false;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@intf_Pais", _InterfaceTienda.Cod_Pais);
                        cmd.Parameters.AddWithValue("@intf_Tda", _InterfaceTienda.Cod_Tda);
                        cmd.Parameters.AddWithValue("@intf_UsuCrea", _InterfaceTienda.IdUsu);
                        cmd.Parameters.AddWithValue("@intf_listDetalle", _InterfaceTienda.List_strListDetalle);
                       
                        cmd.ExecuteNonQuery();
                        valida = true;
                    }
                }

            }
            catch (Exception exc)
            {
                valida = false;
            }
            return valida;


        }

        //DataTable dt_item = null;
        public JsonRespuesta GenerarArchivoInterface(string Cod_Pais, string Cod_Tda, List<string> listInterface, string ruta,ref DataTable dt_item,ref DataTable dt_price_item,ref DataTable dt_item_images)
        {

            JsonRespuesta jsRpta = new JsonRespuesta();
            DataSet ds = null;
            string in_maestros = "";
            Cod_Tda = Cod_Tda=="-1" ? "TODOS" : Cod_Tda;
            string _entorno = "XOFICCE";
           
            string _gen_ruta = ruta;
            string Tienda_Carpeta = _gen_ruta + Cod_Tda;
            string str_procedimiento = "";
            string str_procedimiento2 = "";
            string nombreBK = "";
           

            try
            {
                if (!(Directory.Exists(Tienda_Carpeta)))
                {
                    Directory.CreateDirectory(Tienda_Carpeta);
                    
                }          
                
                
                foreach (string _gen_inter_name in listInterface)
                {
                    str_procedimiento = "";
                    str_procedimiento2 = "";
                    string rutaInterface = "";

                    nombreBK = _gen_inter_name;
                    if (_gen_inter_name == "ITEM_MAINTENANCE") _entorno = "ORCE";

                    switch (_entorno)
                    {
                        case "XOFICCE":
                            switch (_gen_inter_name)
                            {
                                case "ITEM":
                                    str_procedimiento = (Cod_Pais == "PE") ? "USP_XSTORE_GET_ITEM" : "USP_XSTORE_GET_ITEM_EC";
                                    break;
                                case "PRICE_UPDATE":
                                    str_procedimiento = (Cod_Pais == "PE") ? "USP_XSTORE_GET_PRICE_UPDATE_2" : "USP_XSTORE_GET_PRICE_UPDATE_2_ECU";
                                    break;
                                case "MERCH_HIER":
                                    str_procedimiento = (Cod_Pais == "PE") ? "USP_XSTORE_GET_MERCH_HIER" : "USP_XSTORE_GET_MERCH_HIER_ECU";
                                    break;
                                case "ITEM_IMAGES":
                                    str_procedimiento = (Cod_Pais == "PE") ? "USP_XSTORE_GET_ITEM_IMAGES" : "USP_XSTORE_GET_ITEM_IMAGES_EC";
                                    break;
                                case "RETAIL_LOCATION":
                                    str_procedimiento = (Cod_Pais == "PE") ? "USP_XSTORE_GET_RETAIL_LOCATION" : "USP_XSTORE_GET_RETAIL_LOCATION_ECU";
                                    break;
                                case "ITEM_DIMENSION":
                                    str_procedimiento = (Cod_Pais == "PE") ? "USP_XSTORE_GET_ITEM_DIMENSION_TYPE" : "USP_XSTORE_GET_ITEM_DIMENSION_TYPE_EC";
                                    str_procedimiento2 = (Cod_Pais == "PE") ? "USP_XSTORE_GET_ITEM_DIMENSION_VALUE" : "USP_XSTORE_GET_ITEM_DIMENSION_VALUE_EC";
                                    break;
                                case "PARTY":
                                    str_procedimiento = (Cod_Pais == "PE") ? "USP_XSTORE_GET_PARTY" : "USP_XSTORE_GET_PARTY_ECU";
                                    break;
                                case "INV_LOCATION_PROPERTY":
                                    str_procedimiento = (Cod_Pais == "PE") ? "USP_XSTORE_GET_INV_LOCATION_PROPERTY" : "USP_XSTORE_GET_INV_LOCATION_PROPERTY_ECU";
                                    break;
                                case "STOCK_LEDGER":
                                    str_procedimiento = (Cod_Pais == "PE") ? "USP_XSTORE_GET_STOCK_LEDGER" : "USP_XSTORE_GET_STOCK_LEDGER_ECU";
                                    break;
                                case "COUNTRY_CITY":
                                    str_procedimiento = (Cod_Pais == "PE") ? "USP_XSTORE_GET_BCL_COUNTY_CITY" : "USP_XSTORE_GET_BCL_COUNTY_CITY_ECU";
                                    str_procedimiento2 = (Cod_Pais == "PE") ? "USP_XSTORE_GET_BCL_STATE_COUNTY" : "USP_XSTORE_GET_BCL_STATE_COUNTY_ECU";
                                    break;
                                case "ELECTRONIC_CORRELATIVES":
                                    break;
                                case "MANUAL_CORRELATIVES":
                                    break;
                                case "TENDER_REPOSITORY":
                                    str_procedimiento = (Cod_Pais == "PE") ? "USP_XSTORE_GET_TENDER_REPOSITORY" : "USP_XSTORE_GET_TENDER_REPOSITORY_ECU";
                                    str_procedimiento2 = (Cod_Pais == "PE") ? "USP_XSTORE_GET_TENDER_REPOSITORY_PROPERTY" : "USP_XSTORE_GET_TENDER_REPOSITORY_PROPERTY_ECU";
                                    break;
                                case "INV_VALID_DESTINATIONS":
                                    str_procedimiento = (Cod_Pais == "PE") ? "USP_XSTORE_GET_INV_VALID_DESTINATIONS" : "USP_XSTORE_GET_INV_VALID_DESTINATIONS_ECU";
                                    str_procedimiento2 = (Cod_Pais == "PE") ? "USP_XSTORE_GET_INV_VALID_DESTINATIONS_PROPERTY" : "USP_XSTORE_GET_INV_VALID_DESTINATIONS_PROPERTY_ECU";
                                    break;
                                case "VENTAS HISTORICAS":
                                    str_procedimiento = "USP_SET_XSTORE_VENTA_EXPORTAR";
                                    break;


                            }
                            break;

                        case "ORCE":
                            switch (_gen_inter_name)
                            {
                                case "ITEM_MAINTENANCE":
                                    break;
                                case "MERCHANDISE_HIERARCHY_MAINTENANCE":
                                    break;
                                case "ORCE RETAIL_LOCATIONS":
                                    break;
                            }
                            break;
                    }

               

                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(str_procedimiento, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PAIS", Cod_Pais);
                        cmd.Parameters.AddWithValue("@CODTIENDA", Cod_Tda);

                        if (_gen_inter_name == "PARTY") {
                                cmd.Parameters.AddWithValue("@CODSPL", "S");
                                cmd.Parameters.AddWithValue("@CODEMPL", "S");
                        }

                        if (_gen_inter_name == "STOCK_LEDGER")
                        {
                            cmd.Parameters.AddWithValue("@fecha_stk", "");                      
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            ds = new DataSet();
                            
                            if (_gen_inter_name=="ITEM" || _gen_inter_name == "PRICE_UPDATE" || _gen_inter_name=="ITEM_IMAGES")
                            {
                                    switch(_gen_inter_name)
                                    {
                                        case "ITEM":
                                            if (dt_item == null)
                                            {
                                                da.Fill(ds);
                                                dt_item = ds.Tables[0];
                                            }
                                            break;
                                        case "PRICE_UPDATE":
                                            if (dt_price_item == null)
                                            {
                                                da.Fill(ds);
                                                dt_price_item = ds.Tables[0];
                                            }
                                            break;
                                        case "ITEM_IMAGES":
                                            if (dt_item_images == null)
                                            {
                                                da.Fill(ds);
                                                dt_item_images = ds.Tables[0];
                                            }
                                            break;
                                    }                                                                        
                            }
                            else                         
                            { 

                                    da.Fill(ds);
                            }
                        }

                    }
                }

                if (str_procedimiento2 != "") {
                        DataTable dt1 = null;
                        using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                        {
                            using (SqlCommand cmd = new SqlCommand(str_procedimiento2, cn))
                            {
                                cmd.CommandTimeout = 0;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@PAIS", Cod_Pais);
                                cmd.Parameters.AddWithValue("@CODTIENDA", Cod_Tda);

                                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                                {
                                    dt1 = new DataTable();
                                    da.Fill(dt1);
                                }
                            }
                        }

                        ds.Tables.Add(dt1);
                    }

                StringBuilder str = null;
                string str_cadena = "";
                string name_file = "";
                string sufijoNombre = Cod_Tda;
                DataTable dt= null;
                bool valida_genera = false;               

                switch (_entorno)
                {
                    case "XOFICCE":
                        switch (_gen_inter_name)
                        {
                            case "ITEM":
                               #region<ITEM> 
                                dt = new DataTable();
                                dt = dt_item;// ds.Tables[0];                                                           

                                if (dt != null)
                                {                                  
                                    dt = dt_replace_tda(dt, Cod_Tda);

                                    if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        Decimal i = 0;
                                        foreach (DataRow fila in dt.Rows)
                                        {
                                            str.Append(fila["ITEM"].ToString());
                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");
                                            }
                                            i += 1;
                                        }

                                        str_cadena = str.ToString();
                                        name_file = "ITEM_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd") + ".MNT";
                                        rutaInterface = Tienda_Carpeta + "\\" + _gen_inter_name;

                                        if (!(Directory.Exists(rutaInterface)))
                                        {
                                            Directory.CreateDirectory(rutaInterface);                                           
                                        }

                                        in_maestros = rutaInterface + "\\" + name_file;

                                        if (File.Exists(@in_maestros)) File.Delete(@in_maestros);
                                          File.WriteAllText(@in_maestros, str_cadena);
                                     }
                                }
                                break;
                            #endregion
                            case "PRICE_UPDATE":
                                #region<PRICE_UPDATE> 
                                dt = new DataTable();
                                dt = dt_price_item; //ds.Tables[0];
                                if (dt != null)
                                {
                                        dt = dt_replace_tda(dt, Cod_Tda);
                                        if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        for (Int32 i = 0; i < dt.Rows.Count; ++i)
                                        {
                                            str.Append(dt.Rows[i]["PRICE_UPDATE_2"].ToString());

                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");
                                            }
                                        }

                                        str_cadena = str.ToString();
                                        rutaInterface = Tienda_Carpeta + "\\" + _gen_inter_name;

                                        if (!(Directory.Exists(rutaInterface)))
                                        {
                                            Directory.CreateDirectory(rutaInterface);
                                        }

                                        name_file = "PRICE_UPDATE_2_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd")  + ".MNT";
                                        in_maestros = rutaInterface + "\\" + name_file;

                                        if (File.Exists(@in_maestros)) File.Delete(@in_maestros);
                                        File.WriteAllText(@in_maestros, str_cadena);
                                           
                                    }
                                }


                                #endregion
                                break;
                            case "MERCH_HIER":
                                #region<MERCH_HIER>     
                                dt = new DataTable();
                                dt = ds.Tables[0];
                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        Decimal i = 0;
                                        foreach (DataRow fila in dt.Rows)
                                        {

                                            str.Append(fila["MERCH_HIER"].ToString());
                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");
                                            }
                                            i += 1;
                                        }

                                        str_cadena = str.ToString();

                                        rutaInterface = Tienda_Carpeta + "\\" + _gen_inter_name;

                                        if (!(Directory.Exists(rutaInterface)))
                                        {
                                            Directory.CreateDirectory(rutaInterface);
                                        }

                                        name_file = "MERCH_HIER_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd") + ".MNT";
                                        in_maestros = rutaInterface + "\\" + name_file;
                                           
                                        if (File.Exists(@in_maestros)) File.Delete(@in_maestros);
                                        File.WriteAllText(@in_maestros, str_cadena);
                                        valida_genera = true;
                                    }
                                }
                                break;
                            #endregion
                            case "ITEM_IMAGES":
                                #region<ITEM_IMAGES>    
                                dt = new DataTable();
                                dt = dt_item_images;// ds.Tables[0];

                                if (dt != null)
                                {
                                        dt = dt_replace_tda(dt, Cod_Tda);
                                        if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        for (Int32 i = 0; i < dt.Rows.Count; ++i)
                                        {
                                            str.Append(dt.Rows[i]["ITEM_IMAGES"].ToString());

                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");

                                            }
                                        }
                                        str_cadena = str.ToString();

                                        rutaInterface = Tienda_Carpeta + "\\" + _gen_inter_name;

                                        if (!(Directory.Exists(rutaInterface)))
                                        {
                                            Directory.CreateDirectory(rutaInterface);
                                        }

                                        name_file = "ITEM_IMAGES_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd")  + ".MNT";
                                        in_maestros = rutaInterface + "\\" + name_file;

                                        if (File.Exists(@in_maestros)) File.Delete(@in_maestros);
                                        File.WriteAllText(@in_maestros, str_cadena);
                                        valida_genera = true;
                                    }
                                }
                                break;
                                #endregion
                           case "RETAIL_LOCATION":
                                 #region<location> 
                                    
                                    if (ds != null)
                                    {
                                        DataTable dt_retail_location = ds.Tables[0];
                                        DataTable dt_retail_location_property = ds.Tables[1];

                                        rutaInterface = Tienda_Carpeta + "\\" + _gen_inter_name;

                                        if (!(Directory.Exists(rutaInterface)))
                                        {
                                            Directory.CreateDirectory(rutaInterface);
                                        }

                                        if (dt_retail_location.Rows.Count > 0)
                                        {
                                            str = new StringBuilder();
                                            for (Int32 i = 0; i < dt_retail_location.Rows.Count; ++i)
                                            {
                                                str.Append(dt_retail_location.Rows[i]["RETAIL_LOCATION"].ToString());

                                                if (i < dt_retail_location.Rows.Count - 1)
                                                {
                                                    str.Append("\r\n");

                                                }
                                            }
                                            str_cadena = str.ToString();

                                            name_file = "RETAIL_LOCATION_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd")+ ".MNT";
                                            in_maestros = rutaInterface + "\\" + name_file;

                                            if (File.Exists(@in_maestros)) File.Delete(@in_maestros);
                                            File.WriteAllText(@in_maestros, str_cadena);
                                        }
                                        if (dt_retail_location_property.Rows.Count > 0)
                                        {
                                            str = new StringBuilder();
                                            for (Int32 i = 0; i < dt_retail_location_property.Rows.Count; ++i)
                                            {
                                                str.Append(dt_retail_location_property.Rows[i]["RETAIL_LOCATION_PROPERTY"].ToString());

                                                if (i < dt_retail_location_property.Rows.Count - 1)
                                                {
                                                    str.Append("\r\n");

                                                }
                                            }
                                            str_cadena = str.ToString();

                                            name_file = "RETAIL_LOCATION_PROPERTY_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd")+ ".MNT";
                                            in_maestros = rutaInterface + "\\" + name_file;

                                            if (File.Exists(@in_maestros)) File.Delete(@in_maestros);
                                            File.WriteAllText(@in_maestros, str_cadena);
                                            valida_genera = true;
                                        }

                                    }
                                    break;
                                #endregion
                           case "ITEM_DIMENSION":
                                #region<Dimension>                                    
                                rutaInterface = Tienda_Carpeta + "\\" + _gen_inter_name;

                                if (!(Directory.Exists(rutaInterface)))
                                {
                                    Directory.CreateDirectory(rutaInterface);
                                }

                                dt = ds.Tables[0];
                                    
                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        for (Int32 i = 0; i < dt.Rows.Count; ++i)
                                        {
                                            str.Append(dt.Rows[i]["ITEM_DIMENSION_TYPE"].ToString());

                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");
                                            }
                                        }
                                        str_cadena = str.ToString();

                                        name_file = "ITEM_DIMENSION_TYPE_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd") + ".MNT";
                                        in_maestros = rutaInterface + "\\" + name_file;

                                        if (File.Exists(@in_maestros)) File.Delete(@in_maestros);
                                        File.WriteAllText(@in_maestros, str_cadena);
                                    }
                                }

                                dt = ds.Tables[1];

                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        for (Int32 i = 0; i < dt.Rows.Count; ++i)
                                        {
                                            str.Append(dt.Rows[i]["ITEM_DIMENSION_VALUE"].ToString());

                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");

                                            }

                                        }
                                        str_cadena = str.ToString();

                                        name_file = "ITEM_DIMENSION_VALUE_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd") + ".MNT";
                                        in_maestros = rutaInterface + "\\" + name_file;

                                        if (File.Exists(@in_maestros)) File.Delete(@in_maestros);
                                        File.WriteAllText(@in_maestros, str_cadena);
                                        valida_genera = true;
                                    }
                                }
                                   
                               break;
                            #endregion
                           case "PARTY":
                              #region<Party>
                                dt = new DataTable();
                                dt = ds.Tables[0];

                                if (dt != null)
                                    {
                                        if (dt.Rows.Count > 0)
                                        {
                                            str = new StringBuilder();
                                            Decimal i = 0;
                                            foreach (DataRow fila in dt.Rows)
                                            {
                                                str.Append(fila["PARTY"].ToString());
                                                if (i < dt.Rows.Count - 1)
                                                {
                                                    str.Append("\r\n");
                                                }
                                                i += 1;
                                            }

                                            str_cadena = str.ToString();
                                            rutaInterface = Tienda_Carpeta + "\\" + _gen_inter_name;

                                            if (!(Directory.Exists(rutaInterface)))
                                            {
                                                Directory.CreateDirectory(rutaInterface);
                                            }

                                            name_file = "PARTY_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd") + ".MNT";
                                            in_maestros = rutaInterface + "\\" + name_file;

                                            if (File.Exists(@in_maestros)) File.Delete(@in_maestros);
                                            File.WriteAllText(@in_maestros, str_cadena);
                                            valida_genera = true;
                                        }
                                    }
                                    break;
                                #endregion
                           case "INV_LOCATION_PROPERTY":
                                #region<INV_LOCATION_PROPERTY>    
                                dt = new DataTable();
                                dt = ds.Tables[0];

                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        Decimal i = 0;
                                        foreach (DataRow fila in dt.Rows)
                                        {

                                            str.Append(fila["INV_LOCATION_PROPERTY"].ToString());
                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");

                                            }
                                            i += 1;
                                        }
                                        str_cadena = str.ToString();
                                        name_file = "INV_LOCATION_PROPERTY_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd") + ".MNT";
                                        rutaInterface = Tienda_Carpeta + "\\" + _gen_inter_name;
                                        if (!(Directory.Exists(rutaInterface)))
                                        {
                                            Directory.CreateDirectory(rutaInterface);
                                        }

                                        in_maestros = rutaInterface + "\\" + name_file;

                                        if (File.Exists(@in_maestros)) File.Delete(@in_maestros);
                                        File.WriteAllText(@in_maestros, str_cadena);
                                        valida_genera = true;
                                    }
                                }
                                break;
                                #endregion
                           case "STOCK_LEDGER":
                                #region<STOCK_LEDGER>
                                dt = new DataTable();
                                dt = ds.Tables[0];
                                if (dt != null)
                                {
                                    string in_stock_ledger = "";
                                    if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        for (Int32 i = 0; i < dt.Rows.Count; ++i)
                                        {
                                            str.Append(dt.Rows[i]["STOCK_LEDGER"].ToString());

                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");
                                            }
                                        }
                                        str_cadena = str.ToString();

                                        name_file = "STOCK_LEDGER_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd") + ".MNT";

                                        rutaInterface = Tienda_Carpeta + "\\" + _gen_inter_name;
                                        in_stock_ledger = rutaInterface + "\\" + name_file;
                                       
                                        if (!(Directory.Exists(rutaInterface)))
                                        {
                                            Directory.CreateDirectory(rutaInterface);
                                        }

                                            if (File.Exists(@in_stock_ledger)) File.Delete(@in_stock_ledger);
                                        File.WriteAllText(@in_stock_ledger, str_cadena);
                                        valida_genera = true;
                                    }
                                }
                                break;
                                #endregion
                            case "COUNTRY_CITY":
                                #region<country_city>

                                rutaInterface = Tienda_Carpeta + "\\" + _gen_inter_name;

                                if (!(Directory.Exists(rutaInterface)))
                                {
                                    Directory.CreateDirectory(rutaInterface);
                                }

                                dt = new DataTable();
                                dt = ds.Tables[0];

                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        for (Int32 i = 0; i < dt.Rows.Count; ++i)
                                        {
                                            str.Append(dt.Rows[i]["BCL_COUNTY_CITY"].ToString());

                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");
                                            }
                                        }
                                        str_cadena = str.ToString();

                                        name_file = "BCL_COUNTY_CITY_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd")  + ".MNT";
                                        in_maestros = rutaInterface + "\\" + name_file;
                                       

                                            if (File.Exists(@in_maestros)) File.Delete(@in_maestros);
                                        File.WriteAllText(@in_maestros, str_cadena);
                                    }
                                }

                                dt = new DataTable();
                                dt = ds.Tables[1];
                                    if (dt != null)
                                    {
                                        if (dt.Rows.Count > 0)
                                        {
                                            str = new StringBuilder();
                                            for (Int32 i = 0; i < dt.Rows.Count; ++i)
                                            {
                                                str.Append(dt.Rows[i]["BCL_STATE_COUNTY"].ToString());

                                                if (i < dt.Rows.Count - 1)
                                                {
                                                    str.Append("\r\n");

                                                }

                                            }
                                            str_cadena = str.ToString();

                                            name_file = "BCL_STATE_COUNTY_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd")  + ".MNT";
                                            in_maestros = rutaInterface + "\\" + name_file;

                                            if (File.Exists(@in_maestros)) File.Delete(@in_maestros);
                                            File.WriteAllText(@in_maestros, str_cadena);
                                            valida_genera = true;
                                        }
                                    }

                                    break;
                                #endregion                                  
                            case "ELECTRONIC_CORRELATIVES":
                                break;
                            case "MANUAL_CORRELATIVES":
                                break;
                            case "TENDER_REPOSITORY":
                             #region<TENDER_REPOSITORY>

                               rutaInterface = Tienda_Carpeta + "\\" + _gen_inter_name;

                                if (!(Directory.Exists(rutaInterface)))
                                {
                                    Directory.CreateDirectory(rutaInterface);
                                }

                                dt = new DataTable();
                                dt = ds.Tables[0];

                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        for (Int32 i = 0; i < dt.Rows.Count; ++i)
                                        {
                                            str.Append(dt.Rows[i]["TENDER_REPOSITORY"].ToString());

                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");
                                            }
                                        }
                                        str_cadena = str.ToString();

                                        name_file = "TENDER_REPOSITORY_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd") + ".MNT";
                                        in_maestros = rutaInterface + "\\" + name_file;

                                        if (File.Exists(@in_maestros)) File.Delete(@in_maestros);
                                        File.WriteAllText(@in_maestros, str_cadena);
                                    }
                                }

                                dt = new DataTable();
                                dt = ds.Tables[1];
                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        for (Int32 i = 0; i < dt.Rows.Count; ++i)
                                        {
                                            str.Append(dt.Rows[i]["TENDER_REPOSITORY_PROPERTY"].ToString());

                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");
                                            }

                                        }
                                        str_cadena = str.ToString();

                                        name_file = "TENDER_REPOSITORY_PROPERTY_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd") + ".MNT";
                                        in_maestros = rutaInterface + "\\" + name_file;

                                        if (File.Exists(@in_maestros)) File.Delete(@in_maestros);
                                        File.WriteAllText(@in_maestros, str_cadena);
                                        valida_genera = true;
                                    }
                                }

                                 break;
                                #endregion
                                 
                            case "INV_VALID_DESTINATIONS":
                                #region<INV_VALID_DESTINATIONS>
                                rutaInterface = Tienda_Carpeta + "\\" + _gen_inter_name;

                                if (!(Directory.Exists(rutaInterface)))
                                {
                                    Directory.CreateDirectory(rutaInterface);
                                }

                                dt = new DataTable();
                                dt = ds.Tables[0];

                                if (dt != null)
                                {
                                    string name_inv_valid = ""; string in_inv_valid = "";
                                    if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        for (Int32 i = 0; i < dt.Rows.Count; ++i)
                                        {
                                            str.Append(dt.Rows[i]["INV_VALID_DESTINATIONS"].ToString());

                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");

                                            }

                                        }
                                        str_cadena = str.ToString();

                                        name_file = "INV_VALID_DESTINATIONS_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd") + ".MNT";
                                        in_inv_valid = rutaInterface + "\\" + name_file;

                                        if (File.Exists(@in_inv_valid)) File.Delete(@in_inv_valid);
                                        File.WriteAllText(@in_inv_valid, str_cadena);
                                        valida_genera = true;
                                    }
                                }

                                dt = new DataTable();
                                dt = ds.Tables[1];
                                if (dt != null)
                                {
                                    string name_inv_valid = ""; string in_inv_valid = "";
                                    if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        for (Int32 i = 0; i < dt.Rows.Count; ++i)
                                        {
                                            str.Append(dt.Rows[i]["INV_VALID_DESTINATIONS_PROPERTY"].ToString());

                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");
                                            }
                                        }
                                        str_cadena = str.ToString();

                                        name_file = "INV_VALID_DESTINATIONS_PROPERTY_" + sufijoNombre + "_" + DateTime.Today.ToString("yyyyMMdd") + ".MNT";
                                        in_inv_valid = rutaInterface + "\\" + name_file;

                                        if (File.Exists(@in_inv_valid)) File.Delete(@in_inv_valid);
                                        File.WriteAllText(@in_inv_valid, str_cadena);
                                        valida_genera = true;
                                    }
                                }

                                break;
                                #endregion
                                                                        
                            case "VENTAS HISTORICAS":
                                #region<INV_HISTORICAS>
                                string in_archivos = "";
                                rutaInterface = Tienda_Carpeta + "\\" + _gen_inter_name;

                                if (!(Directory.Exists(rutaInterface)))
                                {
                                    Directory.CreateDirectory(rutaInterface);
                                }

                                #region<TRANS_LINE_TENDER>                               
                                dt = ds.Tables[0];
                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        for (Int32 i = 0; i < dt.Rows.Count; ++i)
                                        {
                                            str.Append(dt.Rows[i]["TRANS_LINE_TENDER"].ToString());

                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");

                                            }

                                        }
                                        str_cadena = str.ToString();

                                        name_file = "TRANS_LINE_TENDER_" + sufijoNombre + ".MNT";
                                        in_archivos = rutaInterface + "\\" + name_file;

                                        if (File.Exists(@in_archivos)) File.Delete(@in_archivos);
                                        File.WriteAllText(@in_archivos, str_cadena);
                                    }
                                }
                                #endregion
                                #region<TRANS_TAX>                               
                                dt = ds.Tables[1];
                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        for (Int32 i = 0; i < dt.Rows.Count; ++i)
                                        {
                                            str.Append(dt.Rows[i]["TRANS_TAX"].ToString());

                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");

                                            }

                                        }
                                        str_cadena = str.ToString();



                                        name_file = "TRANS_TAX_" + sufijoNombre + ".MNT";
                                        @in_archivos = rutaInterface + "\\" + name_file;

                                        if (File.Exists(@in_archivos)) File.Delete(@in_archivos);
                                        File.WriteAllText(@in_archivos, str_cadena);
                                    }
                                }
                                #endregion
                                #region<TRANS_LINE_ITEM_TAX>                               
                                dt = ds.Tables[2];
                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        Decimal i = 0;
                                        foreach (DataRow fila in dt.Rows)
                                        {

                                            str.Append(fila["TRANS_LINE_ITEM_TAX"].ToString());
                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");

                                            }
                                            i += 1;

                                        }

                                        str_cadena = str.ToString();
                                        name_file = "TRANS_LINE_ITEM_TAX_" + sufijoNombre + ".MNT";
                                        in_archivos = rutaInterface + "\\" + name_file;

                                        if (File.Exists(@in_archivos)) File.Delete(@in_archivos);
                                        File.WriteAllText(@in_archivos, str_cadena);
                                    }
                                }
                                #endregion
                                #region<TRANS_LINE_ITEM>                               
                                dt = ds.Tables[3];
                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        for (Int32 i = 0; i < dt.Rows.Count; ++i)
                                        {
                                            str.Append(dt.Rows[i]["TRANS_LINE_ITEM"].ToString());

                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");

                                            }
                                        }
                                        str_cadena = str.ToString();

                                        name_file = "TRANS_LINE_ITEM_" + sufijoNombre + ".MNT";
                                        in_archivos = rutaInterface + "\\" + name_file;

                                        if (File.Exists(@in_archivos)) File.Delete(@in_archivos);
                                        File.WriteAllText(@in_archivos, str_cadena);
                                    }
                                }

                                #endregion
                                #region<TRANS_HEADER>                               
                                dt = ds.Tables[4];
                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        str = new StringBuilder();
                                        for (Int32 i = 0; i < dt.Rows.Count; ++i)
                                        {
                                            str.Append(dt.Rows[i]["TRANS_HEADER"].ToString());

                                            if (i < dt.Rows.Count - 1)
                                            {
                                                str.Append("\r\n");

                                            }

                                        }
                                        str_cadena = str.ToString();



                                        name_file = "TRANS_HEADER_" + sufijoNombre + ".MNT";
                                        in_archivos = rutaInterface + "\\" + name_file;

                                        if (File.Exists(@in_archivos)) File.Delete(@in_archivos);
                                        File.WriteAllText(@in_archivos, str_cadena);
                                        valida_genera = true;
                                    }
                                }
                                #endregion                                   
                                #endregion
                                break;


                        }
                        break;
                    case "ORCE":
                        switch (_gen_inter_name)
                        {
                            case "ITEM_MAINTENANCE":
                                break;
                            case "MERCHANDISE_HIERARCHY_MAINTENANCE":
                                break;
                            case "ORCE RETAIL_LOCATIONS":
                                break;
                        }
                        break;
                }

                }

                jsRpta.Success = true;
                jsRpta.Message = Tienda_Carpeta;
                jsRpta.Data = Tienda_Carpeta;


            }
            catch(Exception ex)
            {

                jsRpta.Success = false;
                jsRpta.Message = "Error: " + ex.ToString();
                return jsRpta;
                throw;
               
            }
            return jsRpta;
        }

        private DataTable dt_replace_tda(DataTable dt, string cod_tda)
        {
            DataTable dt_replace = null;
            try
            {

                if (dt != null)
                {
                    dt_replace = dt;
                    string file_cab = dt.Rows[0][0].ToString();

                    string str_tda_ant = file_cab.Substring(file_cab.IndexOf(':') - 6, 13);

                    string str_tda_new = "\"" + str_tda_ant.Replace(str_tda_ant, "STORE:" + cod_tda) + "\"";

                    file_cab = file_cab.Replace(str_tda_ant, str_tda_new);
                    dt_replace.Rows[0][0] = file_cab.ToString();

                }
            }
            catch
            {
                dt_replace = null;
            }
            return dt_replace;
        }

    }


}
