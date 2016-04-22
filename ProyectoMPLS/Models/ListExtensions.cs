using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProyectoMPLS.Models.Comunicacion;
using ProyectoMPLS.Models.Topologia;

namespace ProyectoMPLS.Models
{
    public static class ListExtensions
    {
        /// <summary>
        /// Convierte una lista de Enlaces al modelo Json
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<EnlaceJson> toJson(this IEnumerable<Enlace> list)
        {
            List<EnlaceJson> listaEnlaces = new List<EnlaceJson>();
            foreach (var item in list.ToList())
            {
                EnlaceJson temp = new EnlaceJson();
                temp.idEnlace = item.idEnlace;
                temp.name = item.cNombre;
                temp.from = item.idRouterA;
                temp.to = item.idRouterB;
                temp.toArrow = "";
                temp.total_bw = item.nBandwidth;
                temp.weight = item.nPesoAdministrativo;
                temp.afinity = item.idAfinidad;
                temp.free_bw = item.nBandwidthDisponible;
                temp.id_proyecto = item.idProyecto;
                listaEnlaces.Add(temp);
            }
            return listaEnlaces;
        }

        /// <summary>
        /// Convierte una lista de Routers al modelo Json
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<RouterJson> toJson(this IEnumerable<Router> list)
        {
            List<RouterJson> listaRouters = new List<RouterJson>();
            foreach (var item in list.ToList())
            {
                RouterJson temp = new RouterJson();
                temp.key = item.idRouter;
                temp.name = item.cHostname;
                temp.source = "/Content/Images/router.png";
                temp.loopback_ip = item.cRouterID;
                if (item.cx != 0 && item.cy != 0)
                    temp.loc = item.cx + " " + item.cy;
                temp.id_proyecto = item.idProyecto;
                listaRouters.Add(temp);
            }
            return listaRouters;
        }

        /// <summary>
        /// Convierte desde el formato de Json al de modelo de Enlaces
        /// </summary>
        /// <param name="list"></param>
        /// <param name="idProyecto"></param>
        /// <returns></returns>
        public static List<Enlace> ToModeList(this List<EnlaceJson> list)
        {
            List<Enlace> listaEnlaces = new List<Enlace>();
            foreach (var item in list)
            {
                Enlace temp = new Enlace();
                temp.idEnlace = item.idEnlace;
                temp.idProyecto = item.id_proyecto;
                temp.cNombre = item.name;
                temp.idRouterA = item.from;
                temp.idRouterB = item.to;
                temp.nBandwidth = item.total_bw;
                temp.nPesoAdministrativo = item.weight;
                temp.idAfinidad = item.afinity;
                temp.nBandwidthDisponible = item.free_bw;
                listaEnlaces.Add(temp);
            }

            return listaEnlaces;
        }

        /// <summary>
        /// Convierte desde el formato de Json al de modelo de Routers
        /// </summary>
        /// <param name="list"></param>
        /// <param name="idProyecto"></param>
        /// <returns></returns>
        public static List<Router> ToModeList(this List<RouterJson> list)
        {
            List<Router> listaRouters = new List<Router>();
            foreach (var item in list)
            {
                LER temp = new LER();
                temp.idRouter = item.key;
                temp.idProyecto = item.id_proyecto;
                temp.cHostname = item.name;
                temp.cRouterID = item.loopback_ip;
                string[] coordenadas = item.loc != null ? item.loc.Split(' ') : new string[] { };
                if (coordenadas.Count() == 2)
                {
                    int result;
                    Int32.TryParse(coordenadas[0], out result);
                    temp.cx = result;

                    Int32.TryParse(coordenadas[1], out result);
                    temp.cy = result;
                }
                listaRouters.Add(temp);
            }

            return listaRouters;
        }

        /// <summary>
        /// Genera una lista de Enlaces en base a la lista de Nodos Dijkstra
        /// </summary>
        /// <param name="nodeList"></param>
        /// <returns></returns>
        public static List<EnlaceDijkstra> ToEnlaces(this List<NodoDijkstra> nodePathList, List<EnlaceDijkstra> linkPathList)
        {
            //Proyecto tempModel = new Proyecto(idProyecto);
            List<EnlaceDijkstra> listaEnlacesLSP = new List<EnlaceDijkstra>();

            //Genera una lista de Enlaces en base a la lista de Nodos Dijkstra
            for (int cont = 0; cont < nodePathList.Count; ++cont)
            {
                int curr_n = cont;
                int next_n = cont + 1;

                if (next_n < nodePathList.Count)
                {
                    int curr_routerid = nodePathList[curr_n].idRouter;
                    int next_routerid = nodePathList[next_n].idRouter;

                    EnlaceDijkstra temp = new EnlaceDijkstra();
                    temp = linkPathList.Find(
                                x => (x.idRouterA == curr_routerid && x.idRouterB == next_routerid) ||
                                    (x.idRouterB == curr_routerid && x.idRouterA == next_routerid));
                    if (temp != null)
                    {
                        listaEnlacesLSP.Add(temp);
                        //EnlaceDijkstra new_temp = new EnlaceDijkstra();
                        //new_temp = nodePathList[cont].listaEnlacesDijkstra.Find(x => x.idEnlace == temp.idEnlace);
                        //if (new_temp != null)
                        //    listaEnlacesLSP.Add(new_temp);
                    }
                        
                }
            }
            return listaEnlacesLSP;
        }

        /// <summary>
        /// Parsea los Hostnames del listado de nodos en un string, en orden
        /// </summary>
        /// <param name="nodePathList"></param>
        /// <returns></returns>
        public static string To_String(this List<NodoDijkstra> nodePathList)
        {
            string cRutaHostnames = "[";
            foreach (var router in nodePathList)
            {
                cRutaHostnames += router.cHostname + " > ";
            }
            if (cRutaHostnames.Length > 1)
            {
                cRutaHostnames = cRutaHostnames.Substring(0, cRutaHostnames.Length - 3);
                cRutaHostnames += "]";
            }
            else
            {
                cRutaHostnames = "";
            }
            return cRutaHostnames;
        }

        public static EnlaceDijkstra Clone(this EnlaceDijkstra enlace)
        {
            EnlaceDijkstra newModel = new EnlaceDijkstra();

            newModel.idEnlace = enlace.idEnlace;
            newModel.idProyecto = enlace.idProyecto;
            newModel.cNombre = enlace.cNombre;
            newModel.idAfinidad = enlace.idAfinidad;
            newModel.idRouterA = enlace.idRouterA;
            newModel.idRouterB = enlace.idRouterB;
            newModel.nBandwidth = enlace.nBandwidth;
            newModel.nBandwidthDisponible = enlace.nBandwidthDisponible;
            newModel.nPesoAdministrativo = enlace.nPesoAdministrativo;
            newModel.targetA = enlace.targetA;
            newModel.targetB = enlace.targetB;

            return newModel;
        }
    }
}