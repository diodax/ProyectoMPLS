using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoMPLS.Models
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ModuleAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["Usuario"] != null)
                return true;

            return base.AuthorizeCore(httpContext);
        }



        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    if (filterContext.HttpContext.Session["Usuario"] == null)
        //    {
        //        filterContext.Result = new RedirectResult("~/Home/");
        //        return;
        //    }
        //    else
        //    {
        //        filterContext.Result = new RedirectResult("~/Home/Welcome");
        //        return;

        //        //Usuario Usr = new Usuario();
        //        //Usr.idPerfil = decimal.Parse(filterContext.HttpContext.Session["idPerfil"].ToString());
        //        //Usr.ControllerRoot = filterContext.HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString();

        //        //if (Usr.ValidarPerfilAcceso() == false)
        //        //{
        //        //    string ControllerRoot = filterContext.HttpContext.Session["ControllerRoot"].ToString();
        //        //    string ActionRoot = filterContext.HttpContext.Session["ActionRoot"].ToString();
        //        //    filterContext.Result = new RedirectResult("~/" + ControllerRoot + "/" + ActionRoot);
        //        //    return;
        //        //}
        //    }
        //}
    }
}