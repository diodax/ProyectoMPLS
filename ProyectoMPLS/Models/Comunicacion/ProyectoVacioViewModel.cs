using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoMPLS.Models.Comunicacion
{
    public class ProyectoVacioViewModel
    {
        public string cUserName { get; set; }
        [Display(Name = "Nombre del Proyecto")]
        [Required(ErrorMessage = "Nombre del proyecto requerido")]
        public string cFileName { get; set; }

        public ProyectoVacioViewModel() { }

        public void InsertProyecto()
        {
            Data.dsTopologiaTableAdapters.ProyectosTableAdapter Adapter = new Data.dsTopologiaTableAdapters.ProyectosTableAdapter();
            Adapter.CrearProyectoLocal(this.cUserName, this.cFileName);
        }
    }
}