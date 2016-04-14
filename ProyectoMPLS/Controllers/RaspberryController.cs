using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using ProyectoMPLS.Controllers;
using ProyectoMPLS.Models.Accounts;
using ProyectoMPLS.Models.Comunicacion;
using ProyectoMPLS.Models.Topologia;

namespace ProyectoMPLS.Controllers
{
    public class RaspberryController : ApiController
    {
        // GET api/raspberry
        // Just for testing
        [System.Web.Http.AllowAnonymous]
        public IEnumerable<Proyecto> Get()
        {
            string cUsuarioActual = "diodax";
            List<Proyecto> listaProyectos = new List<Proyecto>();
            //listaProyectos = Proyecto.GetListaProyectos(cUsuarioActual);
            return listaProyectos;
        }

        // POST api/raspberry
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage Post([FromBody]HttpPostedFileBase fArchivoCSV, string cUsername, string cFileName)
        {
            try
            {
                //Llama al SP para crear el nuevo proyecto en la DB
                Data.dsTopologiaTableAdapters.ProyectosTableAdapter Adapter = new Data.dsTopologiaTableAdapters.ProyectosTableAdapter();
                int idProyecto = (int)Adapter.CrearProyectoLocal(cUsername, cFileName);

                //Proceso de parsing del archivo CSV
                List<Tabla> tablaDatos = new List<Tabla>();
                StreamReader csvreader = new StreamReader(fArchivoCSV.InputStream);    // Use the InputStream to get the actual stream sent.

                //Primera linea
                var line = csvreader.ReadLine();
                var values = line.Split(';');

                while (!csvreader.EndOfStream)
                {
                    Tabla row = new Tabla();
                    line = csvreader.ReadLine();
                    values = line.Split(';');

                    row.Hostname = values[0];
                    row.OSPFRouterID = values[1];
                    row.OSPFNeighborRouterID = values[2];
                    row.OSPFNeighborIP = values[3];
                    tablaDatos.Add(row);
                }

                //Creación del nuevo objeto de proyecto
                Proyecto newProyecto = new Proyecto(idProyecto);
                newProyecto.GenerarTopologia(tablaDatos);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }

    public class ApiLoginController : ApiController
    {
        // POST api/apilogin
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage Post(string cUsername, string cHashedPassword)
        {
            try
            {
                if (Usuario.EsLoginValido(cUsername, cHashedPassword))
                {
                    FormsAuthentication.SetAuthCookie(cUsername, false);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "El nombre de usuario o contraseña están incorrectos.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}