using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoMPLS.Models.Topologia
{

    /// <summary>
    /// Plantilla para la edición de un enlace
    /// </summary>
    public class EnlaceViewModel
    {
        public int idEnlace { get; set; }
        public int idProyecto { get; set; }

        [Display(Name = "Nombre")]
        public string cNombre { get; set; }

        [Display(Name = "Restricción de BW")]
        public double nBandwidth { get; set; }

        [Display(Name = "Peso Administrativo")]
        public double nPesoAdministrativo { get; set; }
        
        public int idRouterA { get; set; }
        public int idRouterB { get; set; }

        [Display(Name = "Nodo de Origen")]
        public string cNombreRouterA { get; set; }
        [Display(Name = "Nodo Destino")]
        public string cNombreRouterB { get; set; }

        [Display(Name = "Afinidad")]
        public int idAfinidad { get; set; }

        [Display(Name = "Criterio de la Métrica")]
        public int nTipoMetrica { get; set; }

        public List<SelectListItem> dpAfinidades { get; set; }

        //public Proyecto proyectoActual { get; set; }
        public Enlace enlaceActual { get; set; }
        public List<SelectListItem> listaNodos { get; set; }

        public EnlaceViewModel() { }

        /// <summary>
        /// Constructor para inicializar la plantilla con el ID de un proyecto específico
        /// </summary>
        /// <param name="idProyecto"></param>
        public EnlaceViewModel(int idEnlace, int idProyecto)
        {
            this.idEnlace = idEnlace;
            this.idProyecto = idProyecto;
            //this.proyectoActual = new Proyecto(idProyecto);
            this.enlaceActual = new Enlace(idEnlace, idProyecto);
            this.listaNodos = new List<SelectListItem>();

            Data.dsTopologiaTableAdapters.EnlacesTableAdapter Adapter = new Data.dsTopologiaTableAdapters.EnlacesTableAdapter();
            Data.dsTopologia.EnlacesDataTable dt = Adapter.SelectEnlace(idProyecto, idEnlace);

            if (dt.Rows.Count > 0)
            {
                Data.dsTopologia.EnlacesRow dr = dt[0];
                this.idEnlace = idEnlace;
                this.idProyecto = idProyecto;
                if (!dr.IscNombreNull())
                    this.cNombre = dr.cNombre;
                if (!dr.IsnBandwidthNull())
                    this.nBandwidth = dr.nBandwidth;
                if (!dr.IsnPesoAdministrativoNull())
                    this.nPesoAdministrativo = dr.nPesoAdministrativo;
                if (!dr.IsidRouterANull())
                    this.idRouterA = dr.idRouterA;
                if (!dr.IsidRouterBNull())
                    this.idRouterB = dr.idRouterB;
                if (!dr.IscAfinidadNull())
                    this.idAfinidad = dr.idAfinidad;

                //var idRouterA = this.idRouterA;
                //var idRouterB = this.idRouterB;

                Router routerA = new LSR(idProyecto, this.idRouterA);
                Router routerB = new LSR(idProyecto, this.idRouterB);

                this.cNombreRouterA = routerA.cHostname;
                this.cNombreRouterB = routerB.cHostname;
            }

            List<Afinidad> listaAfinidades = Afinidad.SelectListaAfinidades(this.idProyecto);
            this.dpAfinidades = Afinidad.ConvertDropdownListaAfinidades(listaAfinidades);

        }

        public void insertUpdateEnlace()
        {
            if (this.idAfinidad == 0)
            {
                Data.dsTopologiaTableAdapters.EnlacesTableAdapter Adapter = new Data.dsTopologiaTableAdapters.EnlacesTableAdapter();
                Adapter.InsertarActualizarEnlace(this.idProyecto, this.idEnlace, this.cNombre, this.idRouterA, this.idRouterB, (int)this.nBandwidth, (int)this.nPesoAdministrativo, null);
            }

            else
            {
                Data.dsTopologiaTableAdapters.EnlacesTableAdapter Adapter = new Data.dsTopologiaTableAdapters.EnlacesTableAdapter();
                Adapter.InsertarActualizarEnlace(this.idProyecto, this.idEnlace, this.cNombre, this.idRouterA, this.idRouterB, (int)this.nBandwidth, (int)this.nPesoAdministrativo, this.idAfinidad);
            }
            


        }
    }
}