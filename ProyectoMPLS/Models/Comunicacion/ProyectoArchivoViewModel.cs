using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoMPLS.Models.Comunicacion
{
    public class ProyectoArchivoViewModel
    {
        string cUserName { get; set; }
        [Display(Name = "Nombre del Proyecto")]
        [Required(ErrorMessage = "Nombre del proyecto requerido")]
        string cFileName { get; set; }
        [Display(Name = "Importar Archivo")]
        [Required(ErrorMessage ="Archivo de importación requerido")]
        public HttpPostedFileBase topologiaCVS { get; set; }
    }
}