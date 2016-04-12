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
        public int idProyecto { get; set; }

        public int idEnlace { get; set; }
        public string cNombre { get; set; }
        public int idRouterA { get; set; }
        public int idRouterB { get; set; }

        public double nBandwidth { get; set; }
        public double nPesoAdministrativo { get; set; }
        public int idAfinidad { get; set; }

        //Nuevos parametros
        public double nBandwidthDisponible { get; set; }

        //Coodernadas para la interfaz gráfica
        public int x1 { get; set; }
        public int y1 { get; set; }
        public int x2 { get; set; }
        public int y2 { get; set; }

        public Enlace() { }

        public Enlace(int idEnlace, int idProyecto)
        {
            //Data.dsTopologiaTableAdapters.ProyectosTableAdapter Adapter = new Data.dsTopologiaTableAdapters.ProyectosTableAdapter();
            //Data.dsTopologia.ProyectosDataTable dt = Adapter.SeleccionarListaProyectos(idProyecto, null);

            Data.dsTopologiaTableAdapters.EnlacesTableAdapter Adapter = new Data.dsTopologiaTableAdapters.EnlacesTableAdapter();
            Data.dsTopologia.EnlacesDataTable dt = Adapter.SelectEnlace(idProyecto, idEnlace);
            
            if (dt.Rows.Count > 0)
            {
                Data.dsTopologia.EnlacesRow dr = dt[0];
                this.idEnlace = idEnlace;
                this.idProyecto = idProyecto;
                if(!dr.IscNombreNull())
                    this.cNombre = dr.cNombre;
                if (!dr.IsidRouterANull())
                    this.idRouterA = dr.idRouterA;
                if (!dr.IsidRouterBNull())
                    this.idRouterB = dr.idRouterB;
                if (!dr.IsnBandwidthNull())
                    this.nBandwidth = dr.nBandwidth;
                if (!dr.IsnPesoAdministrativoNull())
                    this.nPesoAdministrativo = dr.nPesoAdministrativo;
                this.nBandwidthDisponible = nBandwidth;
            }
        }
    }
}

