using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoMPLS.Models.Topologia
{
    public class Afinidad
    {
        public int idAfinidad { get; set; }

        [Display(Name = "Identificador de Proyecto")]
        public int idProyecto { get; set; }

        [Display(Name = "Descripción")]
        public string cDescripcion { get; set; }

        [Display(Name = "Color")]
        public string cColor { get; set; }

        public Afinidad() { }

        public static List<Afinidad> SelectListaAfinidades(int idProyecto)
        {
            List<Afinidad> listaAfinidades = new List<Afinidad>();
            Data.dsTopologiaTableAdapters.AfinidadesTableAdapter Adapter = new Data.dsTopologiaTableAdapters.AfinidadesTableAdapter();
            Data.dsTopologia.AfinidadesDataTable dt = Adapter.SeleccionarListaAfinidades(idProyecto);

            foreach (var dr in dt)
            {
                Afinidad temp = new Afinidad();
                temp.idProyecto = dr.idProyecto;
                temp.idAfinidad = dr.idAfinidad;
                temp.cDescripcion = dr.cDescripcion;

                if (!dr.IscColorNull())
                    temp.cColor = dr.cColor.Trim();

                listaAfinidades.Add(temp);
            }

            return listaAfinidades;
        }
    }
}