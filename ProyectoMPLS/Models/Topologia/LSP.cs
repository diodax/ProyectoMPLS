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
        public string idLSP { get; set; }
        public string cNombre { get; set; }
    }

    public class Nodo
    {
        //
    }
}