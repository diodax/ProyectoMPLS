﻿using ProyectoMPLS.Models.Comunicacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoMPLS.Models.Topologia
{
    public class Proyecto
    {
        [Display(Name = "Código")]
        public int idProyecto { get; set; }
        [Display(Name = "Nombre de Usuario")]
        public string cUserName { get; set; }
        [Display(Name = "Nombre de Archivo")]
        public string cTitulo { get; set; }
        [Display(Name = "Fecha de Creación")]
        public DateTime dtFechaCreacion { get; set; }
        [Display(Name = "Última Modificación")]
        public DateTime dtFechaUltEdicion { get; set; }

        public List<Router> listadoRouters { get; set; }
        public List<Enlace> listadoEnlaces { get; set; }
        public List<LSP> listadoLSPs { get; set; }

        public List<SelectListItem> listadoDPEnlaces { get; set; }

        public Proyecto() { }

        /// <summary>
        /// Constructor para buscar un proyecto específico
        /// </summary>
        /// <param name="idProyecto"></param>
        public Proyecto(int idProyecto)
        {
            Data.dsTopologiaTableAdapters.ProyectosTableAdapter Adapter = new Data.dsTopologiaTableAdapters.ProyectosTableAdapter();
            Data.dsTopologia.ProyectosDataTable dt = Adapter.SeleccionarListaProyectos(idProyecto, null);

            if (dt.Rows.Count > 0)
            {
                Data.dsTopologia.ProyectosRow dr = dt[0];
                this.idProyecto = dr.idProyecto;
                this.cUserName = dr.cUserName.Trim();
                this.cTitulo = dr.cFileName.Trim();
                if (!dr.IsdtFechaCreacionNull())
                    this.dtFechaCreacion = dr.dtFechaCreacion;
                if (!dr.IsdtFechaUltEdicionNull())
                    this.dtFechaUltEdicion = dr.dtFechaUltEdicion;
            }

            this.listadoRouters = Proyecto.SelectListaRouters(this.idProyecto);
            this.listadoEnlaces = Proyecto.SelectListaEnlaces(this.idProyecto);
            this.listadoLSPs = LSP.SelectListaLSP(this.idProyecto);
            this.listadoDPEnlaces = Proyecto.ConvertDropdownEnlaces(this.listadoEnlaces);
        }

        /// <summary>
        /// Constructor a usar cuando ya se conoce la topologia de un proyecto especifico
        /// </summary>
        /// <param name="idProyecto"></param>
        /// <param name="listaRouters"></param>
        /// <param name="listaEnlaces"></param>
        public Proyecto(int idProyecto, List<Router> listaRouters, List<Enlace> listaEnlaces)
        {
            Data.dsTopologiaTableAdapters.ProyectosTableAdapter Adapter = new Data.dsTopologiaTableAdapters.ProyectosTableAdapter();
            Data.dsTopologia.ProyectosDataTable dt = Adapter.SeleccionarListaProyectos(idProyecto, null);

            if (dt.Rows.Count > 0)
            {
                Data.dsTopologia.ProyectosRow dr = dt[0];
                this.idProyecto = dr.idProyecto;
                this.cUserName = dr.cUserName.Trim();
                this.cTitulo = dr.cFileName.Trim();
                if (!dr.IsdtFechaCreacionNull())
                    this.dtFechaCreacion = dr.dtFechaCreacion;
                if (!dr.IsdtFechaUltEdicionNull())
                    this.dtFechaUltEdicion = dr.dtFechaUltEdicion;
            }
            this.listadoRouters = listaRouters;
            this.listadoEnlaces = listaEnlaces;
        }

        /// <summary>
        /// Genera la lista de proyectos asociados a un usuario
        /// </summary>
        /// <param name="cUserName"></param>
        /// <returns></returns>
        public static List<Proyecto> GetListaProyectos(string cUserName)
        {
            List<Proyecto> listaProyectos = new List<Proyecto>();

            Data.dsTopologiaTableAdapters.ProyectosTableAdapter Adapter = new Data.dsTopologiaTableAdapters.ProyectosTableAdapter();
            Data.dsTopologia.ProyectosDataTable dt = Adapter.SeleccionarListaProyectos(null, cUserName);

            foreach(var dr in dt)
            {
                Proyecto temp = new Proyecto();
                temp.idProyecto = dr.idProyecto;
                temp.cUserName = dr.cUserName.Trim();
                temp.cTitulo = dr.cFileName.Trim();
                if (!dr.IsdtFechaCreacionNull())
                    temp.dtFechaCreacion = dr.dtFechaCreacion;
                if (!dr.IsdtFechaUltEdicionNull())
                    temp.dtFechaUltEdicion = dr.dtFechaUltEdicion;
                listaProyectos.Add(temp);
            }

            return listaProyectos;
        }

        /// <summary>
        /// Genera una lista de routers y enlaces en base a los contenidos de la tabla CSV
        /// </summary>
        /// <param name="tablaDatos"></param>
        public void GenerarTopologia(List<Tabla> tablaDatos)
        {
            tablaDatos = tablaDatos.GroupBy(x => x.Hostname).Select(x => x.First()).ToList();
            List<Router> listaRouters = new List<Router>();
            List<Enlace> listaEnlaces = new List<Enlace>();
            Dictionary<string, int> routerIDs = new Dictionary<string, int>();
            
            for(int i = 0; i < tablaDatos.Count; ++i) 
            {
                LSR temp = new LSR();
                temp.idRouter = i + 1;
                temp.cHostname = tablaDatos[i].Hostname;
                temp.cRouterID = tablaDatos[i].OSPFRouterID.Trim();
                temp.listaEnlaces = new List<Enlace>();
                routerIDs.Add(temp.cRouterID, temp.idRouter);
                listaRouters.Add(temp);
            }

            foreach(var item in tablaDatos)
            {
                var values = item.OSPFNeighborRouterID.Split(',');

                foreach(var value in values)
                {
                    Enlace temp = new Enlace();
                    int thisID = routerIDs[item.OSPFRouterID.Trim()];
                    int otherID = routerIDs[value.Trim()];
                    

                    if (thisID < otherID)
                    {
                        temp.idRouterA = thisID;
                        temp.idRouterB = otherID;
                        listaRouters[thisID - 1].listaEnlaces.Add(temp);
                        listaEnlaces.Add(temp);
                    }
                    else if (thisID > otherID)
                    {
                        temp.idRouterA = otherID;
                        temp.idRouterB = thisID;
                        listaRouters[thisID - 1].listaEnlaces.Add(temp);
                        listaEnlaces.Add(temp);
                    }
                }
            }

            listaEnlaces = listaEnlaces.GroupBy(d => new { d.idRouterA, d.idRouterB })
                                       .Select(d => d.First())
                                       .ToList();

            for(int i = 0; i < listaEnlaces.Count; ++i)
            {
                listaEnlaces[i].idEnlace = i + 1;
            }

            this.listadoRouters = listaRouters;
            this.listadoEnlaces = listaEnlaces;
            this.InsertUpdateListaRouters();
            this.InsertUpdateListaEnlaces();

        }

        /// <summary>
        /// Inserta o actualiza la lista de routers a la base de datos
        /// </summary>
        public void InsertUpdateListaRouters()
        {
            Data.dsTopologiaTableAdapters.RoutersTableAdapter Adapter = new Data.dsTopologiaTableAdapters.RoutersTableAdapter();

            if (this.listadoRouters == null)
                this.listadoRouters = new List<Router>();

            foreach(var item in this.listadoRouters)
            {
                Adapter.InsertarActualizarRouter(this.idProyecto, item.idRouter, item.cHostname, item.cRouterID, item.cx, item.cy);
            }
        }

        /// <summary>
        /// Inserta o actualiza la lista de enlaces a la base de datos
        /// </summary>
        public void InsertUpdateListaEnlaces()
        {
            Data.dsTopologiaTableAdapters.EnlacesTableAdapter Adapter = new Data.dsTopologiaTableAdapters.EnlacesTableAdapter();

            if (this.listadoEnlaces == null)
                this.listadoEnlaces = new List<Enlace>();

            foreach(var item in this.listadoEnlaces)
            {
                Adapter.InsertarActualizarEnlace(this.idProyecto, item.idEnlace, item.cNombre, item.idRouterA, item.idRouterB, (int)item.nBandwidth, (int)item.nPesoAdministrativo, item.idAfinidad);
            }
        }

        /// <summary>
        /// Retorna el listado de routers en la topología de un proyecto
        /// </summary>
        /// <param name="idProyecto"></param>
        /// <returns></returns>
        public static List<Router> SelectListaRouters(int idProyecto)
        {
            List<Router> listaRouters = new List<Router>();

            Data.dsTopologiaTableAdapters.RoutersTableAdapter Adapter = new Data.dsTopologiaTableAdapters.RoutersTableAdapter();
            Data.dsTopologia.RoutersDataTable dt = Adapter.SelectRoutersProyecto(idProyecto);

            foreach (var dr in dt)
            {
                LSR temp = new LSR();
                temp.idRouter = dr.idRouter;
                temp.idProyecto = dr.idProyecto;
                if (!dr.IscHostnameNull())
                    temp.cHostname = dr.cHostname.Trim();
                if (!dr.IscRouterIDNull())
                    temp.cRouterID = dr.cRouterID.Trim();
                if (!dr.IscXNull())
                    temp.cx = dr.cX;
                if (!dr.IscYNull())
                    temp.cy = dr.cY;
                listaRouters.Add(temp);
            }

            return listaRouters;
        }

        /// <summary>
        /// Retorna el listado de enlaces en la topología de un proyecto
        /// </summary>
        /// <param name="idProyecto"></param>
        /// <returns></returns>
        public static List<Enlace> SelectListaEnlaces(int idProyecto)
        {
            List<Enlace> listaEnlaces = new List<Enlace>();

            Data.dsTopologiaTableAdapters.EnlacesTableAdapter Adapter = new Data.dsTopologiaTableAdapters.EnlacesTableAdapter();
            Data.dsTopologia.EnlacesDataTable dt = Adapter.SelectEnlacesProyecto(idProyecto);

            foreach (var dr in dt)
            {
                Enlace temp = new Enlace();
                temp.idEnlace = dr.idEnlace;
                temp.idProyecto = dr.idProyecto;
                if (!dr.IscNombreNull())
                    temp.cNombre = dr.cNombre.Trim();
                if (!dr.IsidRouterANull())
                    temp.idRouterA = dr.idRouterA;
                if (!dr.IsidRouterBNull())
                    temp.idRouterB = dr.idRouterB;
                if (!dr.IsnBandwidthNull())
                    temp.nBandwidth = dr.nBandwidth;
                if (!dr.IsnPesoAdministrativoNull())
                    temp.nPesoAdministrativo = dr.nPesoAdministrativo;
                if (!dr.IscAfinidadNull())
                    temp.idAfinidad = dr.idAfinidad;
                listaEnlaces.Add(temp);
            }

            return listaEnlaces;
        }

        public static List<SelectListItem> ConvertDropdownEnlaces(List<Enlace> lista)
        {
            List<SelectListItem> listaEnlaces = new List<SelectListItem>();
            foreach (var item in lista)
            {
                SelectListItem temp = new SelectListItem();
                temp.Value = item.idEnlace.ToString();
                string tempRouterA = new LER(item.idProyecto, item.idRouterA).cHostname;
                string tempRouterB = new LER(item.idProyecto, item.idRouterB).cHostname;
                temp.Text = "Enlace #" + item.idEnlace.ToString() + " [" + tempRouterA + " - " + tempRouterB + "]";
                listaEnlaces.Add(temp);
            }
            return listaEnlaces;
        }

        public static void EliminarProyecto(int idProyecto)
        {
            Data.dsUsuariosTableAdapters.Operaciones Adapter = new Data.dsUsuariosTableAdapters.Operaciones();
            Adapter.BorrarProyecto(idProyecto);
        }

        public static List<EnlaceDijkstra> SelectListaEnlacesRouter(int idProyecto, int idRouter)
        {
            List<EnlaceDijkstra> listaEnlaces = new List<EnlaceDijkstra>();

            Data.dsEnrutamientoTableAdapters.EnlacesRouterTableAdapter Adapter = new Data.dsEnrutamientoTableAdapters.EnlacesRouterTableAdapter();
            Data.dsEnrutamiento.EnlacesRouterDataTable dt = Adapter.SelectEnlacesRouter(idProyecto, idRouter);

            foreach (var dr in dt)
            {
                EnlaceDijkstra temp = new EnlaceDijkstra();
                temp.idEnlace = dr.idEnlace;
                temp.idProyecto = dr.idProyecto;
                if (!dr.IscNombreNull())
                    temp.cNombre = dr.cNombre.Trim();
                if (!dr.IsidRouterANull())
                    temp.idRouterA = dr.idRouterA;
                if (!dr.IsidRouterBNull())
                    temp.idRouterB = dr.idRouterB;
                if (!dr.IsnBandwidthNull())
                    temp.nBandwidth = dr.nBandwidth;
                if (!dr.IsnPesoAdministrativoNull())
                    temp.nPesoAdministrativo = dr.nPesoAdministrativo;
                if (!dr.IsidAfinidadNull())
                    temp.idAfinidad = dr.idAfinidad;
                temp.nBandwidthDisponible = dr.nBandwidth;

                //Solo para uso del algoritmo de Dijkstra
                temp.target = new NodoDijkstra();
                if (temp.idRouterA != idRouter)
                    temp.target = new NodoDijkstra(temp.idRouterA, idProyecto);
                else if (temp.idRouterB != idRouter)
                    temp.target = new NodoDijkstra(temp.idRouterB, idProyecto);

                listaEnlaces.Add(temp);
            }

            return listaEnlaces;
        }

        public static List<NodoDijkstra> SelectListaRoutersDijkstra(int idProyecto)
        {
            List<NodoDijkstra> listaRouters = new List<NodoDijkstra>();

            Data.dsTopologiaTableAdapters.RoutersTableAdapter Adapter = new Data.dsTopologiaTableAdapters.RoutersTableAdapter();
            Data.dsTopologia.RoutersDataTable dt = Adapter.SelectRoutersProyecto(idProyecto);

            foreach (var dr in dt)
            {
                NodoDijkstra temp = new NodoDijkstra();
                temp.idRouter = dr.idRouter;
                temp.idProyecto = dr.idProyecto;
                if (!dr.IscHostnameNull())
                    temp.cHostname = dr.cHostname.Trim();
                if (!dr.IscRouterIDNull())
                    temp.cRouterID = dr.cRouterID.Trim();
                if (!dr.IscXNull())
                    temp.cx = dr.cX;
                if (!dr.IscYNull())
                    temp.cy = dr.cY;
                listaRouters.Add(temp);
            }

            return listaRouters;
        }

        public static List<EnlaceDijkstra> SelectListaEnlacesDijkstra(int idProyecto)
        {
            List<EnlaceDijkstra> listaEnlaces = new List<EnlaceDijkstra>();

            Data.dsTopologiaTableAdapters.EnlacesTableAdapter Adapter = new Data.dsTopologiaTableAdapters.EnlacesTableAdapter();
            Data.dsTopologia.EnlacesDataTable dt = Adapter.SelectEnlacesProyecto(idProyecto);

            foreach (var dr in dt)
            {
                EnlaceDijkstra temp = new EnlaceDijkstra();
                temp.idEnlace = dr.idEnlace;
                temp.idProyecto = dr.idProyecto;
                if (!dr.IscNombreNull())
                    temp.cNombre = dr.cNombre.Trim();
                if (!dr.IsidRouterANull())
                    temp.idRouterA = dr.idRouterA;
                if (!dr.IsidRouterBNull())
                    temp.idRouterB = dr.idRouterB;
                if (!dr.IsnBandwidthNull())
                    temp.nBandwidth = dr.nBandwidth;
                if (!dr.IsnPesoAdministrativoNull())
                    temp.nPesoAdministrativo = dr.nPesoAdministrativo;
                if (!dr.IscAfinidadNull())
                    temp.idAfinidad = dr.idAfinidad;
                if (!dr.IsnBandwidthReservadoNull())
                    temp.nBandwidthDisponible = temp.nBandwidth - dr.nBandwidthReservado;
                listaEnlaces.Add(temp);
            }

            return listaEnlaces;
        }
    }
}