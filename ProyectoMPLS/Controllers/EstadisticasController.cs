using ProyectoMPLS.Models.Estadisticas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoMPLS.Controllers
{
    public class EstadisticasController : Controller
    {
        public ActionResult GetJsonEnlace(int idProyecto, int idEnlace)
        {
            EnlaceStats newModel = new EnlaceStats(idProyecto, idEnlace);
            return Json(newModel, JsonRequestBehavior.AllowGet);
        }

    }
}