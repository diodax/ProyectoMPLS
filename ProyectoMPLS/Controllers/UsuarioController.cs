using ProyectoMPLS.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
        public ActionResult Login(LoginViewModel newUser)
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
                        return RedirectToAction("Welcome", "Home");
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
    }
}