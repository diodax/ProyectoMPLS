using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMPLS.Models.Topologia
{
    /// <summary>
    /// Label Switch Path: Representa el camino entre un nodo y otro en la red MPLS
    /// </summary>
    public class LSP
    {
        public int idLSP { get; set; }
        public string cNombre { get; set; }
        public double nBandwidth { get; set; }
        
        //Nuevos parametros
        public int idLEROrigen { get; set; }
        public int idLERDestino { get; set; }

        public int nSetupPriority { get; set; }
        public int nHoldPriority { get; set; }

        public List<Enlace> listaEnlaces { get; set; }
        public List<Router> listaNodos { get; set; }

    }

    public class Nodo
    {
        //
    }
}