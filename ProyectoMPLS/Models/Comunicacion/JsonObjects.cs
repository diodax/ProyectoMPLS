using ProyectoMPLS.Models.Topologia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMPLS.Models.Comunicacion
{
    public class RouterJson
    {
        public int key { get; set; }
        public string name { get; set; }
        public string source { get; set; }
        public string loopback_ip { get; set; }
        public string loc { get; set; }

        public RouterJson() { }

        public RouterJson(int idProyecto, int idRouter)
        {
            LSR temp = new LSR(idProyecto, idRouter);
            this.key = temp.idRouter;
            this.name = temp.cHostname;
            this.source = "/Content/Images/router.png";
            this.loopback_ip = temp.cRouterID;
            if (temp.cx != 0 && temp.cy != 0)
                this.loc = temp.cx + " " + temp.cy;
        }
    }

    public class EnlaceJson
    {
        public int from { get; set; }
        public int to { get; set; }
        public string toArrow { get; set; }
        public int idEnlace { get; set; }
        public string name { get; set; }
        public double total_bw { get; set; }
        public double weight { get; set; }
        public int afinity { get; set; }
        public double free_bw { get; set; }

        public EnlaceJson() { }

        public EnlaceJson(int idProyecto, int idEnlace)
        {
            Enlace temp = new Enlace(idEnlace, idProyecto);
            this.idEnlace = temp.idEnlace;
            this.name = temp.cNombre;
            this.from = temp.idRouterA;
            this.to = temp.idRouterB;
            this.toArrow = "";
            this.total_bw = temp.nBandwidth;
            this.weight = temp.nPesoAdministrativo;
            this.afinity = temp.idAfinidad;
            this.free_bw = temp.nBandwidthDisponible;
        }

        
    }
}


