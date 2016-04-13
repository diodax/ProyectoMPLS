using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMPLS.Models.Topologia
{
    public class LSPViewModel
    {
        public int idProyecto { get; set; }
        public int idLSP { get; set; }

        public string cNombre { get; set; }
        public string cRutaLSP { get; set; }

        public double nBandwidthUsado { get; set; }
        public double nPorcientoUtilizado { get; set; }

        public LSPViewModel() { }
    }
}