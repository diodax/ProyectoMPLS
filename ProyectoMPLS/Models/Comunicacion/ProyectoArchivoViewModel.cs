using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoMPLS.Models.Comunicacion
{
    public class ProyectoArchivoViewModel
    {
        public string cUserName { get; set; }
        [Display(Name = "Nombre del Proyecto")]
        [Required(ErrorMessage = "Nombre del proyecto requerido")]
        public string cFileName { get; set; }
        [Display(Name = "Importar Archivo")]
        [Required(ErrorMessage ="Archivo de importación requerido")]
        public HttpPostedFileBase topologiaCVS { get; set; }

        public ProyectoArchivoViewModel() { }

        public int InsertProyecto()
        {
            Data.dsTopologiaTableAdapters.ProyectosTableAdapter Adapter = new Data.dsTopologiaTableAdapters.ProyectosTableAdapter();
            return (int)Adapter.CrearProyectoLocal(this.cUserName, this.cFileName);
        }
    }
}