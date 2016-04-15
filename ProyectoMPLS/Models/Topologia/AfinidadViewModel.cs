using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoMPLS.Models.Topologia
{
    public class AfinidadViewModel
    {
        public int idProyecto { get; set; }
        public List<Afinidad> listaAfinidades { get; set; }

        public AfinidadViewModel () { }

        public AfinidadViewModel(int idProyecto)
        {
            this.idProyecto = idProyecto;
            this.listaAfinidades = new List<Afinidad>();
            this.listaAfinidades = Afinidad.SelectListaAfinidades(this.idProyecto);
        }
    }
}