using ProyectoMPLS.Models.Topologia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoMPLS.Controllers
{
    public class EnrutamientoController : Controller
    {
        // GET: Enrutamiento
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _IndexLSPs(int idProyecto)
        {
            List<LSP> listaLSPs = new List<LSP>();
            listaLSPs = LSP.SelectListaLSP(idProyecto);
            return PartialView(listaLSPs);
        }

        public ActionResult _CrearLSP(int idProyecto)
        {
            LSP newModel = new LSP(idProyecto);
            return PartialView(newModel);
        }

        public ActionResult _EditarLSP(int idProyecto, int idLSP)
        {
            LSP newModel = new LSP(idProyecto, idLSP);
            return PartialView(newModel);
        }
    }
}