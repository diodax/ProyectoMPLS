using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Priority_Queue;

namespace ProyectoMPLS.Models.Topologia
{
    public class Dijkstra
    {
        /// <summary>
        /// Generar el árbol SPF de la topología en un proyecto, tomando el parámetro como punto de origen
        /// </summary>
        /// <param name="idRouterOrigen"></param>
        /// <param name="idProyecto"></param>
        /// <returns></returns>
        public static SimplePriorityQueue<Router> GenerarRutas(Router idRouterOrigen, int idProyecto)
        {
            idRouterOrigen.nMinDistancia = 0.0;
            SimplePriorityQueue<Router> routerQueue = new SimplePriorityQueue<Router>();
            routerQueue.Enqueue(idRouterOrigen, 1);

            while (routerQueue.Count > 0)
            {
                Router currentRouter = routerQueue.Dequeue();
                //Visita cada enlace adyacente al router u
                foreach (var enlace in currentRouter.listaEnlaces)
                {
                    LSR vecino = new LSR(enlace.idRouterB, idProyecto);
                    double nPesoBandwidth = enlace.nBandwidth;
                    double nDistanciaTotal = currentRouter.nMinDistancia + nPesoBandwidth;
                    if (nDistanciaTotal < vecino.nMinDistancia)
                    {
                        routerQueue.Remove(vecino);
                        vecino.nMinDistancia = nDistanciaTotal;
                        vecino.idRouterPrevio = currentRouter;
                        routerQueue.Enqueue(vecino, 1);
                    }
                }
            }
            return routerQueue;
        }

        /// <summary>
        /// Retorna el recorrido más cercano desde el nodo root, hasta el parámetro de destino
        /// </summary>
        /// <param name="idRouterDestino"></param>
        /// <returns></returns>
        public static List<Router> GetRutaMasCortaHasta(Router idRouterDestino)
        {
            List<Router> path = new List<Router>();

            for (Router r = idRouterDestino; r != null; r = r.idRouterPrevio)
            {
                path.Add(r);   
            }

            path.Reverse();
            return path;
        }
    }
}