using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMPLS.Models.Topologia
{
    public class CSPFViewModel
    {
        public int idProyecto { get; set; }

        public int nRouterOrigen { get; set; }
        public int nRouterDestino { get; set; }

        public CSPFViewModel() { }
    }
}