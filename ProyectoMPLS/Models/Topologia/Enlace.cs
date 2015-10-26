using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMPLS.Models.Topologia
{
    /// <summary>
    /// Enlace entre dos routers
    /// </summary>
    public class Enlace
    {
        public int idEnlace { get; set; }
        public string cNombre { get; set; }
        public int idRouterA { get; set; }
        public int idRouterB { get; set; }

        public double nBandwidth { get; set; }
        public double nPesoAdministrativo { get; set; } 
        public string cAfinidad { get; set; }

        //Coodernadas para la interfaz gráfica
        public int x1 { get; set; }
        public int y1 { get; set; }
        public int x2 { get; set; }
        public int y2 { get; set; }

    }
}