using CapaDato.Control;
using CapaDato.Menu;
using CapaEntidad.Control;
using CapaEntidad.Util;
using CapaPresentacion.Models.Control;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    [Authorize]
    public class ControlController : Controller
    {
        #region<Validacion de acceso al sistema>
        IAuthenticationManager Authentication
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
        // GET: Control
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = null)
        {
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            if (_usuario == null)
            {
                Authentication.SignOut();
                Session.Clear();
            }

            //ViewBag.returnUrl = returnUrl;

            LoginViewModel view = new LoginViewModel();
            view.returnUrl = returnUrl;
            //return View(new LoginViewModel());
            return View(view);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string _error_con = "";
            Boolean _acceso = IsValid(model.Usuario, model.Password, ref _error_con);

            string return_action = ""; string return_controller = "";

            if (_acceso)
            {
                if (returnUrl != null)
                {
                    if (returnUrl.Length > 0)
                    {
                        string[] controller_action = returnUrl.Split('|');
                        return_action = controller_action[0].ToString();
                        return_controller = controller_action[1].ToString();
                    }
                }


                Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
                var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, _usuario.usu_nombre), }, DefaultAuthenticationTypes.ApplicationCookie);
                Authentication.SignIn(new AuthenticationProperties
                {
                    IsPersistent = model.Recordar
                }, identity);


                if (return_action.Length == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    /*validamos las opciones del menu de acceso*/
                    var data = new Dat_Menu();
                    var items = data.navbarItems(_usuario.usu_id).ToList();
                    Session[Ent_Global._session_menu_user] = items;
                    return RedirectToAction(return_action, return_controller);
                    /*************************************/
                }

            }
            else
            {
                if (_error_con == "1")
                {
                    ModelState.AddModelError("", "El intento de conexión no fue correcto. Inténtelo de nuevo");
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("", "Usuario y/o Password con incorrectos.");
                    return View(model);
                }
            }
        }
        private bool IsValid(string usuario, string password, ref string _error_con)
        {
            bool _valida = false;
            Dat_Usuario _usuario = new Dat_Usuario();
            Ent_Usuario _data_user= _usuario.get_login(usuario);


            if (_data_user == null) _valida = false;             

            if (_data_user != null)
            {
                if (usuario.ToUpper() == _data_user.usu_login.ToUpper() && password ==_data_user.usu_contraseña)
                {
                    if (_data_user.usu_est_id == "A")
                    {
                        Session[Ent_Constantes.NameSessionUser] = _data_user;

                        _valida = true;
                    }
                    else
                    {
                        _valida = false;
                    }
                }
                else
                {
                    _valida = false;
                }
            }

            return _valida;
        }
        public ActionResult LogOff()
        {
            Authentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Control");
        }
        #endregion
    }
}