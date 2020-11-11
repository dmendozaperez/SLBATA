using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDato.RuletaBata;
using CapaEntidad.RuletaBata;
using CapaPresentacion.Models.RuletaBata;
using CapaEntidad.Util;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Data;
using CapaEntidad.Control;

namespace CapaPresentacion.Controllers
{
    public class RuletaBataController : Controller
    {
        // GET: Ruleta
        Dat_RuletaBata _datos = new Dat_RuletaBata();        
        public ActionResult Index()
        {
           // Session["Tienda"] = "50143";
            if (Session["Tienda"] == null)
            {
                return RedirectToAction("Login", "Control");
            }
            Ent_RuletaBata ruleta = new Ent_RuletaBata();
            List<Premios> _prem = _datos.get_premios();            
            ruleta.listPremios = _prem;

            ViewBag.valida_ruleta = _datos.get_valida_ruleta();

            return View(ruleta);
        }

        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }

        [HttpPost]
        public ActionResult RegistrarClienteBataclub(GanadorRuleta ganador, string w01, string[] codigos, string afiliarse, string sinDNI)
        {
            string men_validacion_campos = "";
            var regex = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            try
            {
                //if (Session["Tienda"] == null || Session["PremioGanador"] == null)
                if (Session["Tienda"] == null )
                {
                    return RedirectToAction("Login", "Control");
                }
                if (sinDNI != "on")
                {
                    if (ganador.dni == null || ganador.dni == "" || ganador.dni.Length != 8 || !IsNumeric(ganador.dni))
                    {
                        men_validacion_campos += "Ingrese un número de documento válido." + Environment.NewLine;
                    }
                }
                else
                {
                    if (ganador.dni == null || ganador.dni == "" || ganador.dni.Length < 6)
                    {
                        men_validacion_campos += "Ingrese un número de documento válido." + Environment.NewLine;
                    }
                }

                if (ganador.nombre == null || ganador.nombre == "")
                {
                    men_validacion_campos += "Ingrese el nombre del participante." + Environment.NewLine;
                }
                if (ganador.ape_pat == null || ganador.ape_pat == "")
                {
                    men_validacion_campos += "Ingrese el apellido paterno del participante." + Environment.NewLine;
                }
                //if (ganador.ape_mat == null || ganador.ape_mat == "")
                //{
                //    men_validacion_campos += "Ingrese el apellido materno del participante." + Environment.NewLine;
                //}
                if ((ganador.telefono == null || ganador.telefono == "") && (ganador.email == null || ganador.email == ""))
                {
                    men_validacion_campos += "Ingrese un numero de telefono o email válido del participante." + Environment.NewLine;
                }
                if (ganador.email != null && ganador.email.Length > 0 && !System.Text.RegularExpressions.Regex.IsMatch(ganador.email, regex))
                {
                    men_validacion_campos += "Ingrese un email válido del participante." + Environment.NewLine;
                }
                if (men_validacion_campos.Trim() != "")
                {
                    return Json(new { estado = 0, codigo = "", resultados = men_validacion_campos.Trim() });
                }
                string corre_envio = "";
                string telef_envia = "";
                string codigo = "";
                string tienda = Session["Tienda"].ToString(); ;
                //Premios premio = (Premios)Session["PremioGanador"];
                int estado = 0;
                //if (premio.indice.ToString() == w01)
                //{
                    /*generar codigo real y registrar al ganador.*/
                    //codigo += ganador.dni.ToString().Substring(4);
                    //codigo += premio.id.ToString();
                    //codigo += DateTime.Now.ToString("HHmmss");
                    //codigo = codigo.Replace(":", "");

                    //codigo = codigo.PadLeft(15, '0');

                    //int result_insert = _datos.insertar_ganador_ruleta(codigo, Session["Tienda"].ToString(), ganador.dni, ganador.nombre, ganador.ape_pat, ganador.ape_mat, ganador.telefono, ganador.email, premio.id);
                    //if (result_insert > 0)
                    //{
                    //    if (afiliarse == "on" && sinDNI != "on")
                        //{
                        string valida_ingreso=actualiza_cliente(ganador.dni, ganador.nombre, ganador.ape_pat, ganador.ape_mat, ganador.telefono, ganador.email, Session["Tienda"].ToString(), ref corre_envio, ref telef_envia);
                        string resultados = "";

                        estado = (valida_ingreso.Length == 0) ? 1 : 0;
                        resultados = (valida_ingreso.Length == 0) ? "Se registró correctamente al Cliente." : valida_ingreso;
                //}
                //estado = 1;
                        return Json(new { estado = estado, codigo = codigo, resultados = resultados });
                    //}
                    //else
                    //{
                    //    estado = 0;
                    //    return Json(new { estado = estado, codigo = "", resultados = "Error al registrar el ganador." });
                    //}
                //}
                //else
                //{
                //    estado = 0;
                //    return Json(new { estado = estado, codigo = "", resultados = "Ganador. No válido." });
                //}
            }
            catch (Exception ex)
            {
                return Json(new { estado = 0, codigo = "", resultados = "Error al registrar el Cliente. Verifique los datos ingresados." });
            }
        }

        [HttpPost]
        public ActionResult RegistrarGanador(GanadorRuleta ganador, string w01 , string[] codigos , string afiliarse, string sinDNI)
        {
            string men_validacion_campos = "";
            var regex = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            try
            {
                Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
                Int32 usu_id = Convert.ToInt32(_usuario.usu_id);//Convert.ToInt32(_usuario.usu_id);
                //usu_id = 1;
                if (Session["Tienda"] == null || Session["PremioGanador"]== null)
                {
                    return RedirectToAction("Login", "Control");
                }

                //if (sinDNI != "on")
                //{
                //    if (ganador.dni == null || ganador.dni == "" || ganador.dni.Length != 8 || !IsNumeric(ganador.dni))
                //    {
                //        men_validacion_campos += "Ingrese un número de documento válido." + Environment.NewLine;
                //    }
                //}else
                //{
                //    if (ganador.dni == null || ganador.dni == "" || ganador.dni.Length < 6)
                //    {
                //        men_validacion_campos += "Ingrese un número de documento válido." + Environment.NewLine;
                //    }
                //}
                    
                //if (ganador.nombre == null || ganador.nombre == "")
                //{
                //    men_validacion_campos += "Ingrese el nombre del participante." + Environment.NewLine;
                //}
                //if (ganador.ape_pat == null || ganador.ape_pat == "")
                //{
                //    men_validacion_campos += "Ingrese el apellido paterno del participante." + Environment.NewLine;
                //}
                ////if (ganador.ape_mat == null || ganador.ape_mat == "")
                ////{
                ////    men_validacion_campos += "Ingrese el apellido materno del participante." + Environment.NewLine;
                ////}
                //if ((ganador.telefono == null || ganador.telefono == "") && (ganador.email == null || ganador.email == ""))
                //{
                //    men_validacion_campos += "Ingrese un numero de telefono o email válido del participante." + Environment.NewLine;
                //}
                //if (ganador.email != null && ganador.email.Length > 0 && !System.Text.RegularExpressions.Regex.IsMatch(ganador.email, regex))
                //{
                //    men_validacion_campos += "Ingrese un email válido del participante." + Environment.NewLine;
                //}
                //if (men_validacion_campos.Trim() != "")
                //{
                //    return Json(new { estado = 0, codigo = "", resultados = men_validacion_campos.Trim() });
                //}
                string corre_envio = "";
                string telef_envia = ""; 
                string codigo = "";
                string barra = "";
                string tienda = Session["Tienda"].ToString(); ;
                Premios premio = (Premios)Session["PremioGanador"];
                int estado = 0;
                if (premio.indice.ToString() == w01)
                {   
                    /*generar codigo real y registrar al ganador.*/
                    codigo += ganador.dni.ToString().Substring(4);
                    codigo += premio.id.ToString();
                    codigo += DateTime.Now.ToString("HHmmss");
                    codigo = codigo.Replace(":", "");

                    codigo = codigo.PadLeft(15, '0');

                    int result_insert = _datos.insertar_ganador_ruleta(codigo, Session["Tienda"].ToString(), ganador.dni, ganador.nombre, ganador.ape_pat,ganador.ape_mat, ganador.telefono, ganador.email, premio.id, usu_id,ref barra);
                    if (result_insert > 0)
                    {
                        if (afiliarse == "on" && sinDNI != "on")
                        {
                            //actualiza_cliente(ganador.dni, ganador.nombre, ganador.ape_pat, ganador.ape_mat, ganador.telefono, ganador.email, Session["Tienda"].ToString(), ref corre_envio, ref telef_envia);
                        }
                        codigo = barra;
                        estado = 1;
                        return Json(new { estado = estado, codigo = codigo, resultados = "Se registró correctamente al ganador." , premio = premio.nombre , codigos = (codigos == null ? 1 : codigos.Length + 1), prom_id = premio.prom_id });
                    }
                    else
                    {
                        estado = 0;
                        return Json(new { estado = estado, codigo = "", resultados = "Error al registrar el ganador." });
                    }                        
                }
                else
                {
                    estado = 0;
                    return Json(new { estado = estado, codigo = "", resultados = "Ganador. No válido." });
                }                
            }
            catch(Exception ex)
            {
                return Json(new { estado = 0, codigo = "", resultados = "Error al registrar ganador. Verifique los datos ingresados."});
            }                   
        }
        [HttpPost]
        public ActionResult ValidarMiembroBataClub(GanadorRuleta ganador)
        {
            Ent_Ruleta_Valida ruleta_valida = null;
            Int32 estado = 0;
            string mensaje = "";
            try
            {

                ruleta_valida = _datos.get_valida_dni(ganador.dni.ToString());
                estado = 1;
                

            }
            catch (Exception exc)
            {
                estado = 0;
                mensaje = exc.Message;

            }
            return Json(new { estado = estado, mensaje = mensaje, ruleta_valida = ruleta_valida });
            //bool nuevo_bataclub = false;
            //try
            //{
            //    BataClub.BataEcommerceSoapClient cliente_bataclub = new BataClub.BataEcommerceSoapClient();
            //    BataClub.ValidateAcceso header = new BataClub.ValidateAcceso();
            //    header.Username = "EA646294-11F4-4836-8C6E-F5D9B5F681FC";
            //    header.Password = "DB959DFE-E49A-4F9B-8CD5-97364EE31FBA";

            //    BataClub.Cliente_Parameter_Bataclub parameter = new BataClub.Cliente_Parameter_Bataclub();
            //    parameter.dni = ganador.dni;
            //    parameter.dni_barra = "";
            //    parameter.envia_correo = "0"/*QUE NO ENVIE CORREO*/;

            //    var datacliente = cliente_bataclub.ws_consultar_Cliente(header, parameter);

            //    if (datacliente != null)
            //    {
            //        if (datacliente.existe_cliente)
            //        {
            //            string _fc_ruc = datacliente.dni.ToString();//  datosCliente.DNI_String.ToString();
            //            string _fc_nomb = datacliente.primerNombre;//(datosCliente.Nombres != null) ? datosCliente.Nombres.ToString() : "";
            //            string _fc_apep = datacliente.apellidoPater;// (datosCliente.Apellidos != null) ? datosCliente.Apellidos.ToString() : "";
            //            string _fc_apem = datacliente.apellidoMater;// (datosCliente.ApellidoMaterno != null) ? datosCliente.ApellidoMaterno.ToString() : "";
            //            string _fc_tele = datacliente.telefono;// (datosCliente.Celular != null) ? datosCliente.Celular : "";
            //                                                   //if (fc_tele.Length == 0) fc_tele = (datosCliente.Fono != null) ? datosCliente.Fono.ToString() : "";
            //            string _fc_mail = datacliente.correo;// (datosCliente.eMail != null) ? datosCliente.eMail.ToString() : "";
            //            string _fc_dcli = "";//(datosCliente.Localidad != null) ? datosCliente.Localidad.ToString() : "";
            //                                 //dt.Rows.Add(_fc_ruc, fc_nomb.ToUpper(), fc_apep.ToUpper(), fc_apem, fc_tele, fc_mail, fc_dcli.ToUpper(), "");
            //            bool flujo_metri = datacliente.miembro_bataclub;// datosCliente.RegistradoEnFlujosBataClub;                    
            //            return Json(new { estado = 1, existe = datacliente.existe_cliente, nuevo_bataclub = !datacliente.miembro_bataclub, _dni = _fc_ruc, nombre = _fc_nomb, ape_pat = _fc_apep, ape_mat = _fc_apem, telefono = _fc_tele, email = _fc_mail });
            //        }
            //        else
            //        {
            //            SunatReniec.Sunat_Reniec_PESoapClient clienteSunatReniec = new SunatReniec.Sunat_Reniec_PESoapClient();
            //            SunatReniec.validateLogin la = new SunatReniec.validateLogin();
            //            la.Username = "BataPeru";
            //            la.Password = "Bata2018**.";

            //            var dataClienteReniec = clienteSunatReniec.ws_persona_reniec(la, ganador.dni);
            //            if (dataClienteReniec.Valida_Reniec.Estado == "0")
            //            {
            //                return Json(new { estado = 1, existe = false, nuevo_bataclub = true, _dni = dataClienteReniec.Dni, nombre = dataClienteReniec.Nombres, ape_pat = dataClienteReniec.ApePat, ape_mat = dataClienteReniec.ApeMat });
            //            }
            //            else
            //            {
            //                return Json(new
            //                {
            //                    estado = 1,
            //                    existe = false,
            //                    nuevo_bataclub = true,
            //                    _dni = "",
            //                    nombre = "",
            //                    ape_pat = "",
            //                    ape_mat = ""
            //                });
            //            }
            //        }
            //    }
            //    else
            //    {
            //        return Json(new { estado = 1, existe = false, nuevo_bataclub = true });
            //    }                
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { estado = 0, existe = false, nuevo_bataclub = true , resultados = "Error al validar miembro BATACLUB" });
            //}           
        }

        [HttpPost]
        public ActionResult ValidarParticipacion()
        {
            try
            {
                if (Session["Tienda"] == null)
                {
                    return RedirectToAction("Login", "Control");
                }
                Premios ganador = new Premios();
                ganador = _datos.get_ganador_rulta(Session["Tienda"].ToString());
                if (ganador.tipo == 1)
                {
                    Session["PremioGanador"] = ganador;
                    return Json(new { estado = 1, w01 = ganador.indice, resultados = "" });
                }
                else
                {
                    return Json(new { estado = 0, w01 = "", resultados = "Lo sentimos. Se excedió el numero de ganadores en el día :'(" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { estado = 0, w01 = "", resultados = "Error al validar la participacion. Recargue la página por favor..." });
            }
        }
      
        /**/

        [HttpPost]
        public ActionResult ImprimirCodigo(GanadorRuleta ganador, string w01, string[] codigos, string afiliarse)
        {
            try
            {
                //string[] c = { "BCR200UJ037P000006" };
                //codigos = c;
                //codigos[0] = "BCR200UJ037P000006";
                DataTable dt_info_cupon = null;
                List<PremioRuleta> premios = new List<PremioRuleta>();
                for (int i = 0; i < codigos.Length; i++)
                {
                    dt_info_cupon = _datos.get_info_cupon_ruleta(codigos[i]);
                    PremioRuleta _pre = new PremioRuleta();
                    _pre.codigo_cupon = dt_info_cupon.Rows[0]["cod_cup_pre"].ToString();
                    _pre.nombre = dt_info_cupon.Rows[0]["nom_pre"].ToString();
                    _pre.descripcion = dt_info_cupon.Rows[0]["des_pre"].ToString();
                    _pre.prom_id = dt_info_cupon.Rows[0]["prom_id"].ToString();
                    premios.Add(_pre);
                }               

                return Json(new { estado = 1, premios = premios });
            }
            catch (Exception ex)
            {
                return Json(new { estado = 0 });
            }
        }



        public static string actualiza_cliente(string ruc, string nombres, string apepat, string _apemat,
                                                 string telefono, string email, string tda, ref string correo_envio, ref string _telef_envia)
        {
            string _valida = "";
            try
            {
                //ws_clientedniruc.Cons_ClienteSoapClient ws_cliente = new ws_clientedniruc.Cons_ClienteSoapClient();
                //_valida = ws_cliente.ws_update_cliente(_ruc, nombres, apepat, apemat, telefono, email, tda);
                //return "";
                //consultando en la ws de metricard

                #region<REGION BATACLUB BATA>
                BataClub.BataEcommerceSoapClient cliente_bataclub = new BataClub.BataEcommerceSoapClient();
                BataClub.ValidateAcceso header = new BataClub.ValidateAcceso();
                header.Username = "EA646294-11F4-4836-8C6E-F5D9B5F681FC";
                header.Password = "DB959DFE-E49A-4F9B-8CD5-97364EE31FBA";

                BataClub.Ent_Cliente_BataClub cliente = new BataClub.Ent_Cliente_BataClub();
                cliente.canal = "05";
                cliente.dni = ruc;
                cliente.primerNombre = nombres;
                cliente.apellidoPater = apepat;
                cliente.apellidoMater = _apemat;
                cliente.correo = email;
                cliente.telefono = telefono;
                cliente.tienda = tda;

                BataClub.Cliente_Parameter_Bataclub parameter = new BataClub.Cliente_Parameter_Bataclub();
                parameter.dni = ruc;
                parameter.dni_barra = "";

                var existe_cl = cliente_bataclub.ws_consultar_Cliente(header, parameter);

                var registra_cl = cliente_bataclub.ws_registrar_Cliente(header, cliente);


                //if (!existe_cl.existe_cliente)
                //{
                //    if (_email.Length != 0)
                //    {
                //        var respuesta3 = cliente_metri.EnviarCorreoBienvenidaCliente_DniString(_ruc);
                //        _correo_envio = "1";
                //    }
                //    else
                //    {
                //        _telef_envia = "1";
                //    }
                //}



                if (registra_cl != null)
                {
                    /*se inserto correctamente*/
                    if (registra_cl.codigo == "0")
                    {
                        if (!existe_cl.existe_cliente)
                        {
                            if (email.Length != 0)
                            {
                                correo_envio = "1";
                            }
                            else
                            {
                                _telef_envia = "1";
                            }
                        }
                        else
                        {
                            if (email.Length != 0)
                            {
                                correo_envio = "0";
                            }
                            else
                            {
                                _telef_envia = "0";
                            }
                        }
                    }
                    else
                    {
                        _valida = registra_cl.descripcion;
                    }

                }
                else
                {
                    _valida = "error de conexion de web service bata";
                }
                #endregion

                //ServiceMetricard.ServicioClienteClient cliente_metri = new ServiceMetricard.ServicioClienteClient();

                //bool existe_cliente = cliente_metri.EsCliente_DniString(_ruc);
                //insertando en la ws de metricard
                //ServiceMetricard.Cliente update_cliente_metri = new ServiceMetricard.Cliente { DNI_String = ruc, Nombres = nombres, Apellidos = apepat,ApellidoMaterno=_apemat, eMail = email, Celular = telefono,CodigoTienda= tda };
                //si no existe entonces insertamos

                //if (!existe_cliente)
                //{
                //    var respuesta1 = cliente_metri.IngresarDatosNuevoCliente(update_cliente_metri);
                //    if (!respuesta1.OperacionExitosa) _valida = respuesta1.Mensaje;//"Error de MetriCard";
                //    if (respuesta1.OperacionExitosa)
                //    {
                //        if (_email.Length!=0)
                //        { 
                //           var respuesta3 = cliente_metri.EnviarCorreoBienvenidaCliente_DniString(_ruc);
                //            _correo_envio = "1";
                //        }
                //        else
                //        {
                //            _telef_envia = "1";
                //        }

                //    }
                //}
                //else
                //{
                //    var respuesta2 = cliente_metri.ActualizarDatosCliente(update_cliente_metri);
                //    if (!respuesta2.OperacionExitosa) _valida = "Error de MetriCard";

                //    if (respuesta2.OperacionExitosa)
                //    {
                //        if (_email.Length != 0)
                //        {
                //            var respuesta3 = cliente_metri.EnviarCorreoBienvenidaCliente_DniString(_ruc);
                //            _correo_envio = "0";
                //        }
                //        else
                //        {
                //            _telef_envia = "0";
                //        }
                //    }

                //}


                //ServiceMetricard.ServicioClienteClient 


                //insertando en la ws de metricard
                //ServiceMetricard.Cliente update_cliente_metri = new ServiceMetricard.Cliente { DNI_String = ruc, Nombres = nombres, Apellidos = apepat + " " + apemat, eMail = email, Celular = telefono };





            }
            catch (Exception exc)
            {
                _valida = exc.Message;
            }
            return _valida;
        }

        /**/

    }
}