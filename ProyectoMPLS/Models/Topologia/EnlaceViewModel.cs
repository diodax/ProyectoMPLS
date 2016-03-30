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

        [Display(Name = "Nodo de Origen")]
        public int nRouterOrigen { get; set; }
        [Display(Name = "Nodo Actual")]
        public int nRouterDestino { get; set; }

        [Display(Name = "Afinidad")]
        public int idAfinidad { get; set; }

        [Display(Name = "Criterio de la Métrica")]
        public int nTipoMetrica { get; set; }

        //public Proyecto proyectoActual { get; set; }
        public Enlace enlaceActual { get; set; }
        public List<SelectListItem> listaNodos { get; set; }

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

            /*foreach (var nodo in this.proyectoActual.listadoRouters)
            {
                SelectListItem temp = new SelectListItem();
                temp.Value = nodo.idRouter.ToString();
                temp.Text = nodo.cHostname.ToString().Trim();
                listaNodos.Add(temp);
            }*/
        }

        public void insertUpdateEnlace()
        {
            Data.dsTopologiaTableAdapters.EnlacesTableAdapter Adapter = new Data.dsTopologiaTableAdapters.EnlacesTableAdapter();
            Adapter.InsertarActualizarEnlace(this.idProyecto, this.idEnlace, this.cNombre, this.nRouterOrigen, this.nRouterDestino, (int)this.nBandwidth, (int)this.nPesoAdministrativo, this.idAfinidad);
        }
    }
}