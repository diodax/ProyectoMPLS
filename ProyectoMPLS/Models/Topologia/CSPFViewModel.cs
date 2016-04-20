using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoMPLS.Models.Topologia
{
    /// <summary>
    /// Plantilla para la creación de un LSP usando el algoritmo CSPF
    /// </summary>
    public class CSPFViewModel
    {
        public int idProyecto { get; set; }
        public int idLSP { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string cNombre { get; set; }
        [Display(Name = "Restricción de BW")]
        public double nBandwidth { get; set; }
        [Display(Name = "Afinidad")]
        public int idAfinidad { get; set; }

        [Required]
        [Display(Name = "Nodo de Origen")]
        public int nRouterOrigen { get; set; }
        [Required]
        [Display(Name = "Nodo de Destino")]
        public int nRouterDestino { get; set; }

        [Display(Name = "Criterio de la Métrica")]
        public int nTipoMetrica { get; set; }

        public Proyecto proyectoActual { get; set; }
        public List<SelectListItem> listaNodos { get; set; }

        //SelectList con todos los nodos de la topologia. Estatico
        public List<SelectListItem> listaNodosOrigen { get; set; }
        //SelectList con los nodos de la topologia, excepto el seleccionado en listaNodosOrigen. Dinamico
        public List<SelectListItem> listaNodosDestino { get; set; }
        //SelectList con la lista de afinidades
        public List<SelectListItem> listaAfinidades { get; set; }

        public List<NodoDijkstra> calculatedPath { get; set; }
        public string cRutaHostnames { get; set; }
        public List<Enlace> listaEnlacesPath { get; set; } 

        /// <summary>
        /// Constructor para inicializar la plantilla con el ID de un proyecto específico
        /// </summary>
        /// <param name="idProyecto"></param>
        public CSPFViewModel(int idProyecto)
        {
            this.idProyecto = idProyecto;
            this.proyectoActual = new Proyecto(idProyecto);
            this.listaNodos = new List<SelectListItem>();

            foreach (var nodo in this.proyectoActual.listadoRouters)
            {
                SelectListItem temp = new SelectListItem();
                temp.Value = nodo.idRouter.ToString();
                string tempHostname = nodo.cHostname != null ? nodo.cHostname.Trim() : String.Empty;
                string tempIpAdress = nodo.cRouterID != null ? "[" + nodo.cRouterID.Trim() + "]" : String.Empty;

                temp.Text = tempHostname + " " + tempIpAdress;
                listaNodos.Add(temp);
            }

            List<Afinidad> listaAfinidades = Afinidad.SelectListaAfinidades(this.idProyecto);
            this.listaAfinidades = Afinidad.ConvertDropdownListaAfinidades(listaAfinidades);
        }
    }
}