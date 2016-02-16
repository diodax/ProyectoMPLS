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

        [Display(Name = "Nombre")]
        public string cNombre { get; set; }
        [Display(Name = "Restricción de BW")]
        public double nBandwidth { get; set; }

        [Display(Name = "Nodo de Origen")]
        public int nRouterOrigen { get; set; }
        [Display(Name = "Nodo Actual")]
        public int nRouterDestino { get; set; }

        [Display(Name = "Criterio de la Métrica")]
        public int nTipoMetrica { get; set; }

        public Proyecto proyectoActual { get; set; }
        public List<SelectListItem> listaNodos { get; set; }

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
                temp.Text = nodo.cHostname.ToString().Trim();
                listaNodos.Add(temp);
            }
        }
    }
}