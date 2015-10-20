using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMPLS.Models.Comunicacion
{
    public class Tabla
    {
        public string Hostname { get; set; }
        public string OSPFRouterID { get; set; }
        public string OSPFNeighborRouterID { get; set; }
        public string OSPFNeighborIP { get; set; }

        public Tabla() { }

        //constructor con lineas
    }
}