using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoMPLS.Controllers;
using ProyectoMPLS.Models.Topologia;

namespace ProyectoMPLS.Controllers
{
    public class WebApiController : ValuesController
    {
        // GET api/values
        // Just for testing
        public IEnumerable<Proyecto> Get()
        {
            string cUsuarioActual = "diodax".ToString();
            List<Proyecto> listaProyectos = new List<Proyecto>();
            listaProyectos = Proyecto.GetListaProyectos(cUsuarioActual);
            return listaProyectos;
        }
    }
}