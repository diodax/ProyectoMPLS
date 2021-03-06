﻿using System;
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

        [Required]
        [Display(Name = "Descripción")]
        public string cDescripcion { get; set; }

        [Display(Name = "Color")]
        public string cColor { get; set; }

        //public List<SelectListItem> listaAfinidades { get; set; }

        public Afinidad() { }

        public Afinidad(int idProyecto)
        {
            this.idProyecto = idProyecto;
        }

        public Afinidad(int idProyecto, int idAfinidad)
        {
            Data.dsTopologiaTableAdapters.SelectAfinidadTableAdapter Adapter = new Data.dsTopologiaTableAdapters.SelectAfinidadTableAdapter();
            Data.dsTopologia.SelectAfinidadDataTable dt = Adapter.SelectAfinidad(idAfinidad, idProyecto);

            foreach (var dr in dt)
            {
                this.idProyecto = idProyecto;
                this.idAfinidad = idAfinidad;
                this.cDescripcion = dr.cDescripcion;
                if (!dr.IscColorNull())
                    this.cColor = dr.cColor;
            }
        }

        public static List<Afinidad> SelectListaAfinidades(int idProyecto)
        {
            List<Afinidad> listaAfinidades = new List<Afinidad>();
            Data.dsTopologiaTableAdapters.SelectListaAfinidadesTableAdapter Adapter = new Data.dsTopologiaTableAdapters.SelectListaAfinidadesTableAdapter();
            Data.dsTopologia.SelectListaAfinidadesDataTable dt = Adapter.SelectListaAfinidades(idProyecto);

            foreach (var dr in dt)
            {
                Afinidad temp = new Afinidad();
                temp.idProyecto = dr.idProyecto;
                temp.idAfinidad = dr.idAfinidad;
                temp.cDescripcion = dr.cDescripcion.Trim();

                if (!dr.IscColorNull())
                    temp.cColor = dr.cColor.Trim();

                listaAfinidades.Add(temp);
            }

            return listaAfinidades;
        }

        public static List<SelectListItem> ConvertDropdownListaAfinidades(List<Afinidad> listaAfinidades)
        {
            List<SelectListItem> dpAfinidades = new List<SelectListItem>();

            SelectListItem temp1 = new SelectListItem();
            temp1.Value = "0";
            temp1.Text = "--Seleccionar Afinidad--";
            dpAfinidades.Add(temp1);

            foreach (var item in listaAfinidades)
            {
                SelectListItem temp = new SelectListItem();
                temp.Value = item.idAfinidad.ToString();
                temp.Text = item.cDescripcion;
            }
            return dpAfinidades;
        }

        public void CrearAfinidad(int idProyecto, string cDescripcion, string cColor)
        {
            Data.dsTopologiaTableAdapters.SelectAfinidadTableAdapter Adapter = new Data.dsTopologiaTableAdapters.SelectAfinidadTableAdapter();
            Adapter.CrearActualizarAfinidad(idProyecto, 0, cDescripcion, cColor);
        }

        public void ActualizarAfinidad(int idProyecto, int idAfinidad, string cDescripcion, string cColor)
        {
            Data.dsTopologiaTableAdapters.SelectAfinidadTableAdapter Adapter = new Data.dsTopologiaTableAdapters.SelectAfinidadTableAdapter();
            Adapter.CrearActualizarAfinidad(idProyecto, idAfinidad, cDescripcion, cColor);
        }

        public static bool BorrarAfinidad(int idProyecto, int idAfinidad)
        {
            Data.dsTopologiaTableAdapters.SelectAfinidadTableAdapter Adapter = new Data.dsTopologiaTableAdapters.SelectAfinidadTableAdapter();
            bool bExito = (bool)Adapter.BorrarAfinidad(idProyecto, idAfinidad);
            return bExito;
        }
    }
}