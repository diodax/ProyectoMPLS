using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoMPLS.Models.Comunicacion
{
    public class ProyectoRaspberryViewModel
    {
        public string cUserName { get; set; }
        [Display(Name ="Nombre del Proyecto")]
        [Required(ErrorMessage = "Nombre del proyecto requerido")]
        public string cFileName { get; set; }
        [Display(Name ="IP Raspberry Pi")]
        [Required(ErrorMessage = "Dirección IP del Raspberry Pi requerida")]
        public string cRaspberryIP { get; set; }
        [Display(Name = "Puerto")]
        public int nPuerto { get; set; }
        [Display(Name ="IP Router MPLS")]
        [Required(ErrorMessage = "Dirección IP de un Router MPLS requerida")]
        public string cRouterIP { get; set; }
        [Display(Name = "Community String")]
        [Required(ErrorMessage = "Community String de SMTP requerido")]
        [DataType(DataType.Password)]
        public string cCommunityString { get; set; }

        public ProyectoRaspberryViewModel() { }

        public int InsertProyecto()
        {
            Data.dsTopologiaTableAdapters.ProyectosTableAdapter Adapter = new Data.dsTopologiaTableAdapters.ProyectosTableAdapter();
            return (int)Adapter.CrearProyectoImportado(this.cUserName, this.cFileName, this.cRaspberryIP, this.nPuerto, this.cRouterIP, this.cCommunityString);
        }
    }

    


}