using ProyectoMPLS.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Helpers;

namespace ProyectoMPLS.Controllers
{
    public class UsuarioController : Controller
    {

        // GET: /Account/Registrar
        public ActionResult Registrar()
        {
            RegistroViewModel newModel = new RegistroViewModel();
            return View(newModel);
        }

        // POST: /Account/Registrar
        [HttpPost]
        public ActionResult Registrar(RegistroViewModel newUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Usuario newModel = new Usuario();
                    if (!Usuario.ExisteUsuario(newUser.cUserName))
                    {
                        newModel.InsertarUsuario(newUser);
                        FormsAuthentication.SetAuthCookie(newUser.cUserName, false);
                        Session["Usuario"] = newUser.cUserName;
                        return RedirectToAction("Welcome", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "El nombre de usuario ya existe.");
                    }     
                }
            }
            catch
            {
                Session["Usuario"] = null;
                return View(newUser);
            }

            return View(newUser);
        }

        // GET: /Account/Login
        public ActionResult Login()
        {
            LoginViewModel newModel = new LoginViewModel();
            return View(newModel);
        }

        // POST: /Account/Login
        [HttpPost]
        public ActionResult Login(LoginViewModel newUser, string returnUrl)
        {
            //Login Code
            try
            {
                if (ModelState.IsValid)
                {
                    Usuario newModel = new Usuario();
                    if (Usuario.EsLoginValido(newUser.cUserName, newUser.cPassword))
                    {
                        FormsAuthentication.SetAuthCookie(newUser.cUserName, false);
                        Session["Usuario"] = newUser.cUserName;


                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            //return RedirectToAction("Welcome", "Home");
                            return RedirectToAction("Index", "Topologia");
                        }
                        //return RedirectToAction("Index", "Topologia");

                    }
                    else
                    {
                        ModelState.AddModelError("", "El nombre de usuario o contraseña están incorrectos.");
                    }
                }
            }
            catch
            {
                Session["Usuario"] = null;
                return View(newUser);
            }

            return View(newUser);
        }

        // GET: /Account/Logout
        public ActionResult Logout()//LoginViewModel usuario)
        {
            Session["Usuario"] = null;
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Settings
        public ActionResult Settings(string cUsername)
        {
            try {
                string cUsuarioActual = Session["Usuario"].ToString();
                SettingsViewModel changedUser = new SettingsViewModel(cUsuarioActual);
                return View(changedUser);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST /Account/Settings
        [HttpPost]
        public ActionResult Settings(SettingsViewModel changedUser)
        {
            /*try
            {*/
                if (ModelState.IsValid)
                {
                    changedUser.ActualizarUsuario();
                    return RedirectToAction("Index", "Topologia");

                }
                else
                {
                    return View(changedUser);
                }
            /*}
            catch
            {
                return View(changedUser);
            }*/
        }
    }
}