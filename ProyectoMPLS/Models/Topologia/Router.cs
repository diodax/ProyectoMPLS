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
        public decimal rx { get; set; }
        public decimal ry { get; set; }

        //Parámetros a usar por el Algoritmo de Dijkstra
        public Router idRouterPrevio { get; set; }
        public double nMinDistancia = Double.PositiveInfinity;
        public int CompareTo(Router other)
        {
            return this.nMinDistancia.CompareTo(other.nMinDistancia);
        }
    }

    /// <summary>
    /// Label Switch Router: Representa un router localizado en el medio de una red MPLS
    /// </summary>
    public class LSR : Router
    {
        //
    }

    /// <summary>
    /// Label Edge Router: Representa un router que opera en los bordes de una red MPLS
    /// </summary>
    public class LER : Router
    {
        //
    }
}