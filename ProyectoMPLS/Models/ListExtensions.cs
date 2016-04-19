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
                //temp.cx = item.loc;
                //temp.cy = item.loc;
                listaRouters.Add(temp);
            }

            return listaRouters;
        }
    }
}