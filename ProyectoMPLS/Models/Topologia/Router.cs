using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMPLS.Models.Topologia
{
    /// <summary>
    /// Clase padre para las propiedades en común entre routers LSR y LER
    /// </summary>
    public abstract class Router
    {
        public int idRouter { get; set; }
        public string cHostname { get; set; }
        public string cRouterID { get; set; }
        public List<Enlace> listaEnlaces { get; set; }

        //Coordenadas para la interfaz gráfica
        public int cx { get; set; }
        public int cy { get; set; }
        //public decimal rx { get; set; }
        //public decimal ry { get; set; }

        

        public Router() { }

        public Router(int idRouter, int idProyecto)
        {
            //
        }
    }

    /// <summary>
    /// Representa un Router genérico utilizado como nodo por el Algoritmo de Dijkstra
    /// </summary>
    public class NodoDijkstra : Router
    {
        public NodoDijkstra idRouterPrevio { get; set; }
        public double nMinDistancia = Double.PositiveInfinity;
        public int CompareTo(NodoDijkstra other)
        {
            return this.nMinDistancia.CompareTo(other.nMinDistancia);
        }

        public NodoDijkstra() { }

        public NodoDijkstra(int idRouter, int idProyecto) { }
    }

    /// <summary>
    /// Label Switch Router: Representa un router localizado en el medio de una red MPLS
    /// </summary>
    public class LSR : Router
    {
        public LSR() { }

        public LSR(int idProyecto, int idRouter)
        {
            Data.dsTopologiaTableAdapters.RouterTableAdapter Adapter = new Data.dsTopologiaTableAdapters.RouterTableAdapter();
            Data.dsTopologia.RouterDataTable dt = Adapter.SelectRouter(idProyecto, idRouter);

            foreach (var dr in dt)
            {
                this.idRouter = dr.idRouter;
                if (!dr.IscHostnameNull())
                    this.cHostname = dr.cHostname.Trim();
                if (!dr.IscRouterIDNull())
                    this.cRouterID = dr.cRouterID.Trim();
            }
        }
    }

    /// <summary>
    /// Label Edge Router: Representa un router que opera en los bordes de una red MPLS
    /// </summary>
    public class LER : Router
    {
        public LER() { }

        public LER(int idProyecto, int idRouter)
        {
            Data.dsTopologiaTableAdapters.RouterTableAdapter Adapter = new Data.dsTopologiaTableAdapters.RouterTableAdapter();
            Data.dsTopologia.RouterDataTable dt = Adapter.SelectRouter(idProyecto, idRouter);

            foreach (var dr in dt)
            {
                this.idRouter = dr.idRouter;
                if (!dr.IscHostnameNull())
                    this.cHostname = dr.cHostname.Trim();
                if (!dr.IscRouterIDNull())
                    this.cRouterID = dr.cRouterID.Trim();
            }
        }
    }
}