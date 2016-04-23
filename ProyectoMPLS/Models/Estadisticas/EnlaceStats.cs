using ProyectoMPLS.Models.Topologia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMPLS.Models.Estadisticas
{
    public class EnlaceStats
    {
        public class Elemento
        {
            public int idLSP { get; set; }
            public string cNombre { get; set; }
            public double nBWElemento { get; set; }

            public Elemento() { }
        }

        public int idProyecto { get; set; }
        public int idEnlace { get; set; }
        public double nBWReservadoTotal { get; set; }   //BW Total reservado del enlace
        public double nBWTotal { get; set; }            //BW Total disponible del enlace

        public List<Elemento> listaElementos { get; set; }

        public EnlaceStats() { }

        public EnlaceStats(int idProyecto, int idEnlace)
        {
            Data.dsEstadisticasTableAdapters.EstadisticasEnlaceTableAdapter Adapter = new Data.dsEstadisticasTableAdapters.EstadisticasEnlaceTableAdapter();
            Data.dsEstadisticas.EstadisticasEnlaceDataTable dt = Adapter.SelectEstadisticasEnlace(idProyecto, idEnlace);
            this.nBWReservadoTotal = 0;
            this.listaElementos = new List<Elemento>();

            if (dt.Count > 0)
            {
                this.idProyecto = dt[0].idProyecto;
                this.idEnlace = dt[0].idEnlace;
                this.nBWTotal = dt[0].nBandwidthTotal;
            }

            foreach(var dr in dt)
            {
                Elemento temp = new Elemento();
                temp.idLSP = dr.idLSP;
                temp.cNombre = new LSP(this.idProyecto, temp.idLSP).cNombre;
                temp.nBWElemento = dr.nBandwidthLSP;
                this.nBWReservadoTotal += dr.nBandwidthReservado;
                this.listaElementos.Add(temp);
            }
        }

        //
    } 
}