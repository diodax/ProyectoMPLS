using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Priority_Queue;

namespace ProyectoMPLS.Models.Topologia
{
    //Fuente: http://stackoverflow.com/questions/17480022/java-find-shortest-path-between-2-points-in-a-distance-weighted-map

    public class Dijkstra
    {
        /// <summary>
        /// Generar el árbol SPF de la topología, tomando el parámetro como punto de origen
        /// </summary>
        /// <param name="idRouterOrigen"></param>
        public static SimplePriorityQueue<Router> GenerarRutas(Router idRouterOrigen)
        {
            idRouterOrigen.nMinDistancia = 0.0;
            SimplePriorityQueue<Router> routerQueue = new SimplePriorityQueue<Router>();
            routerQueue.Enqueue(idRouterOrigen, 1);

            while (routerQueue.Count > 0)
            {
                /*
                Vertex u = vertexQueue.poll();

                // Visit each edge exiting u
                for (Edge e : u.adjacencies)
                {
                    Vertex v = e.target;
                    double weight = e.weight;
                    double distanceThroughU = u.minDistance + weight;
                    if (distanceThroughU < v.minDistance) {
                        vertexQueue.remove(v);

                        v.minDistance = distanceThroughU ;
                        v.previous = u;
                        vertexQueue.add(v);
                    }
                }
                */
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

            /*
            for (Vertex vertex = target; vertex != null; vertex = vertex.previous)
            path.add(vertex);
            Collections.reverse(path);
            */

            return path;
        }
    }
}