using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoMPLS.Models.Topologia
{
    public class Proyecto
    {
        public int idProyecto { get; set; }
        public string cUserName { get; set; }
        public string cTitulo { get; set; }
        public DateTime dtFechaCreacion { get; set; }
        public DateTime dtFechaUltEdicion { get; set; }

        public Proyecto() { }

        /// <summary>
        /// Constructor para buscar un proyecto específico
        /// </summary>
        /// <param name="idProyecto"></param>
        public Proyecto(int idProyecto)
        {
            Data.dsTopologiaTableAdapters.ProyectosTableAdapter Adapter = new Data.dsTopologiaTableAdapters.ProyectosTableAdapter();
            Data.dsTopologia.ProyectosDataTable dt = Adapter.SeleccionarListaProyectos(idProyecto, null);

            if (dt.Rows.Count > 0)
            {
                Data.dsTopologia.ProyectosRow dr = dt[0];
                this.idProyecto = dr.idProyecto;
                this.cUserName = dr.cUserName.Trim();
                this.cTitulo = dr.cFileName.Trim();
                if (!dr.IsdtFechaCreacionNull())
                    this.dtFechaCreacion = dr.dtFechaCreacion;
                if (!dr.IsdtFechaUltEdicionNull())
                    this.dtFechaUltEdicion = dr.dtFechaUltEdicion;
            }
        }

        /// <summary>
        /// Genera la lista de proyectos asociados a un usuario
        /// </summary>
        /// <param name="cUserName"></param>
        /// <returns></returns>
        public static List<Proyecto> GetListaProyectos(string cUserName)
        {
            List<Proyecto> listaProyectos = new List<Proyecto>();

            Data.dsTopologiaTableAdapters.ProyectosTableAdapter Adapter = new Data.dsTopologiaTableAdapters.ProyectosTableAdapter();
            Data.dsTopologia.ProyectosDataTable dt = Adapter.SeleccionarListaProyectos(null, cUserName);

            foreach(var dr in dt)
            {
                Proyecto temp = new Proyecto();
                temp.idProyecto = dr.idProyecto;
                temp.cUserName = dr.cUserName.Trim();
                temp.cTitulo = dr.cFileName.Trim();
                if (!dr.IsdtFechaCreacionNull())
                    temp.dtFechaCreacion = dr.dtFechaCreacion;
                if (!dr.IsdtFechaUltEdicionNull())
                    temp.dtFechaUltEdicion = dr.dtFechaUltEdicion;
                listaProyectos.Add(temp);
            }

            return listaProyectos;
        }
    }
}