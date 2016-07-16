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
        /// Generar y retornar el árbol SPF de la topología en un proyecto, tomando el parámetro como punto de origen
        /// </summary>
        /// <param name="idRouterOrigen"></param>
        /// <param name="idProyecto"></param>
        /// <param name="minBW"></param>
        /// <returns></returns>
        public static List<NodoDijkstra> GenerarRutas(NodoDijkstra idRouterOrigen, int idProyecto, double minBW, int nTipoMetrica = 2, int idAfinidad = 0)
        {
            idRouterOrigen.nMinDistancia = 0.0;
            SimplePriorityQueue<NodoDijkstra> routerQueue = new SimplePriorityQueue<NodoDijkstra>();
            routerQueue.Enqueue(idRouterOrigen, 1);

            //mantiene el registro de todos los nodos de la topologia por el que se pasa
            List<NodoDijkstra> routerList = new List<NodoDijkstra>();
            routerList.Add(idRouterOrigen);

            while (routerQueue.Count > 0)
            {
                NodoDijkstra currentRouter = routerQueue.Dequeue();
                //Visita cada enlace adyacente al router u
                foreach (var enlace in currentRouter.listaEnlacesDijkstra)
                {
                    int idRouterVecino = 0;
                    //Fix: Asegurandose de que se use el id del router adyacente en el enlace
                    if (enlace.idRouterB != currentRouter.idRouter)
                    {
                        idRouterVecino = enlace.idRouterB;
                        //enlace.target = enlace.targetB;
                    }
                    else
                    {
                        idRouterVecino = enlace.idRouterA;
                        //enlace.target = enlace.targetA;
                    }   
                    //NodoDijkstra vecino = new NodoDijkstra(idRouterVecino, idProyecto);

                    NodoDijkstra vecino = enlace.target;

                    double nPesoBandwidth = 0;
                    switch(nTipoMetrica)        //ignore var name, aqui va lo del tipo de peso
                    {
                        case 1: //Pesos Administrativos
                            nPesoBandwidth = enlace.nPesoAdministrativo;
                            break;
                        case 2: //Minima Cantidad de Saltos
                            nPesoBandwidth = 1;
                            break;
                        case 3: // 1/BW Reservado
                            nPesoBandwidth = 1.00 / (enlace.nBandwidth - enlace.nBandwidthDisponible);
                            break;
                        case 4: // 1/BW Disponible 
                            nPesoBandwidth = 1.00 / enlace.nBandwidthDisponible;
                            break;
                        default:
                            nPesoBandwidth = 1;
                            break;
                    }
                    
                                              
                    double nDistanciaTotal = currentRouter.nMinDistancia + nPesoBandwidth;
                    
                    //Aqui ocurre el filtro por afinidad
                    if (idAfinidad == 0)    //No afinidad definida
                    {
                        //En este if ocurre el filtro por BW disponible
                        if (nDistanciaTotal < vecino.nMinDistancia && minBW < enlace.nBandwidth) //Constraint check
                        {
                            if (routerQueue.Contains(vecino))
                                routerQueue.Remove(vecino);
                            vecino.nMinDistancia = nDistanciaTotal;
                            vecino.idRouterPrevio = currentRouter;
                            enlace.nBandwidthDisponible -= minBW; //reservar el BW en el enlace

                            routerQueue.Enqueue(vecino, 1);
                        }
                    }
                    else  //Afinidad definida
                    {
                        if (idAfinidad == enlace.idAfinidad)    //Afinidad check
                        {
                            //En este if ocurre el filtro por BW disponible
                            if (nDistanciaTotal < vecino.nMinDistancia && minBW < enlace.nBandwidth) //Constraint check
                            {
                                if (routerQueue.Contains(vecino))
                                    routerQueue.Remove(vecino);
                                vecino.nMinDistancia = nDistanciaTotal;
                                vecino.idRouterPrevio = currentRouter;
                                enlace.nBandwidthDisponible -= minBW; //reservar el BW en el enlace

                                routerQueue.Enqueue(vecino, 1);
                            }
                        }
                    }

                    

                    //Agrega el router (bueno, los 2) al registro
                    int indexTarget = routerList.FindIndex(n => n.idRouter == vecino.idRouter);
                    if (indexTarget != -1)
                        routerList[indexTarget] = vecino;
                    else
                        routerList.Add(vecino);
                    int indexSource = routerList.FindIndex(n => n.idRouter == currentRouter.idRouter);
                    if (indexSource != -1)
                        routerList[indexSource] = currentRouter;
                    else
                        routerList.Add(currentRouter);
                    
                }
            }
            return routerList;
        }

        /// <summary>
        /// Retorna el recorrido más cercano desde el nodo root, hasta el parámetro de destino
        /// </summary>
        /// <param name="idRouterDestino"></param>
        /// <returns></returns>
        public static List<NodoDijkstra> GetRutaMasCortaHasta(NodoDijkstra idRouterDestino)
        {
            List<NodoDijkstra> path = new List<NodoDijkstra>();
            for (NodoDijkstra r = idRouterDestino; r != null; r = r.idRouterPrevio)
            {
                path.Add(r);   
            }
            path.Reverse();
            return path;
        }
    }


}