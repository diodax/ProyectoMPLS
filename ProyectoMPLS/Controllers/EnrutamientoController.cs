using ProyectoMPLS.Models.Topologia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Priority_Queue;

namespace ProyectoMPLS.Controllers
{
    public class EnrutamientoController : Controller
    {
        #region LSP_Manual

        public ActionResult _InlineIndexLSPs(int idProyecto)
        {
            List<LSP> listaLSPs = new List<LSP>();
            listaLSPs = LSP.SelectListaLSP(idProyecto);
            return PartialView(listaLSPs);
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

        #endregion

        #region CSPF

        public ActionResult _CrearCSPF(int idProyecto)
        {
            CSPFViewModel newModel = new CSPFViewModel(idProyecto);
            return PartialView(newModel);
        }

        [HttpPost]
        public ActionResult _EjecutarDijkstra(CSPFViewModel newModel)
        {
            if (ModelState.IsValid)
            {
                //Inicializa el nodo que servira como punto de partida para el algoritmo
                NodoDijkstra RouterOrigen = new NodoDijkstra(newModel.nRouterOrigen, newModel.idProyecto);

                List<NodoDijkstra> routerQueue = new List<NodoDijkstra>();
                routerQueue = Dijkstra.GenerarRutas(RouterOrigen, newModel.idProyecto, newModel.nBandwidth);

                NodoDijkstra RouterDestino = routerQueue.FirstOrDefault(x => x.idRouter == newModel.nRouterDestino);

                List<NodoDijkstra> result = new List<NodoDijkstra>();
                result = Dijkstra.GetRutaMasCortaHasta(RouterDestino);
                return Json(new {path = result, success = true});
            }
            else
            {
                return Json(new {path = new List<NodoDijkstra>(), success = false});
            }
            
        }

        [HttpPost]
        public ActionResult _CrearCSPF(CSPFViewModel newModel)
        {
            if (ModelState.IsValid)
            {
                //Actualiza el LSP 
                return Json(new {success = true});
            }

            return PartialView(newModel);
        }

        #endregion

    }
}