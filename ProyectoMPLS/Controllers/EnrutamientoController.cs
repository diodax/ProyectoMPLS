using ProyectoMPLS.Models.Topologia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Priority_Queue;
using ProyectoMPLS.Models;

namespace ProyectoMPLS.Controllers
{
    public class EnrutamientoController : Controller
    {
        #region LSP_Manual

        public ActionResult _InlineIndexLSPs(int idProyecto)
        {
            InlineLSPViewModel newModel = new InlineLSPViewModel(idProyecto);
            return PartialView(newModel);
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
        public ActionResult _CrearCSPF(CSPFViewModel newModel)
        {
            //if (!string.IsNullOrWhiteSpace(runDijkstra))
            if (newModel.saveDijkstra != "true")
            {
                //Ejecuta codigo del algoritmo
                if (ModelState.IsValid)
                {
                    //Inicio preparacion de la topologia
                    List<NodoDijkstra> listaNodos = Proyecto.SelectListaRoutersDijkstra(newModel.idProyecto);
                    List<EnlaceDijkstra> listaEnlaces = Proyecto.SelectListaEnlacesDijkstra(newModel.idProyecto);

                    foreach(var item in listaEnlaces)
                    {
                        //Pasando las referencias a los enlaces de la lista de nodos
                        item.target = new NodoDijkstra();
                        item.targetA = listaNodos.Find(x => x.idRouter == item.idRouterA);
                        item.targetB = listaNodos.Find(x => x.idRouter == item.idRouterB);
                    }

                    foreach(var node in listaNodos)
                    {
                        node.listaEnlacesDijkstra = new List<EnlaceDijkstra>();

                        foreach (var link in listaEnlaces)
                        {
                            if (link.idRouterA == node.idRouter)
                            {
                                EnlaceDijkstra temp = link.Clone();
                                temp.target = link.targetB;
                                node.listaEnlacesDijkstra.Add(temp);
                            }
                            else if (link.idRouterB == node.idRouter)
                            {
                                EnlaceDijkstra temp = link.Clone();
                                temp.target = link.targetA;
                                node.listaEnlacesDijkstra.Add(temp);
                            }
                                
                        }
                    }
                    //Fin preparacion de la topologia

                    //Jala el nodo que servira como punto de partida para el algoritmo
                    NodoDijkstra RouterOrigen = listaNodos.Find(x => x.idRouter == newModel.nRouterOrigen);
                    
                    List<NodoDijkstra> routerQueue = new List<NodoDijkstra>();
                    routerQueue = Dijkstra.GenerarRutas(RouterOrigen, newModel.idProyecto, newModel.nBandwidth);

                    NodoDijkstra RouterDestino = routerQueue.FirstOrDefault(x => x.idRouter == newModel.nRouterDestino);

                    List<NodoDijkstra> result = new List<NodoDijkstra>();
                    result = Dijkstra.GetRutaMasCortaHasta(RouterDestino);
                    List<EnlaceDijkstra> listaEnlacesLSP = result.ToEnlaces(listaEnlaces);
                    string literal = result.To_String();

                    //Remueve las referencias a los objetos_routers de los enlaces
                    //var query = someList.Select(x => { x.SomeProp = "foo"; return x; })
                    List<EnlaceDijkstra> query = listaEnlacesLSP.Select(
                        x => {
                                x.target = null;
                                x.targetA = null;
                                x.targetB = null;
                                return x;
                             }).ToList();


                    return Json(new { path = query, node_string = literal, success = true, operation = "run_algorithm" });
                }
                else
                {
                    return Json(new { path = new List<EnlaceDijkstra>(), node_string = "", success = false });
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //TODO: Completar esta funcion
                    //List<Enlace> listaEnlacesLSP = newModel.calculatedPath.ToEnlaces(newModel.idProyecto);

                    try
                    {
                        LSP newlink = new LSP();
                        newlink.idProyecto = newModel.idProyecto;
                        newlink.idLSP = newlink.idLSP;
                        newlink.cNombre = newModel.cNombre;
                        newlink.idRouterOrigen = newModel.nRouterOrigen;
                        newlink.idRouterDestino = newModel.nRouterDestino;
                        newlink.nBandwidth = newModel.nBandwidth;

                        int newEnlace = newlink.InsertUpdateLSPHeader();

                        LSP newThingModel = new LSP();

                        foreach(var item in newModel.listaEnlacesPath)
                        {
                            newThingModel.idProyecto = item.idProyecto;
                            newThingModel.idLSP = newEnlace;
                            newThingModel.DeleteDetalleLSP(item.idEnlace);
                            newThingModel.InsertDetalleLSP(item.idEnlace);

                            //newThingModel.InsertDetalleLSP(item.idEnlace);
                        }

                        
                        return Json(new { success = true, operation = "save" });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false });
                    }
                }
                return Json(new { success = false });
            }




            
        }

        #endregion

    }
}