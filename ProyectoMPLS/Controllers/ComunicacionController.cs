using ProyectoMPLS.Models.Comunicacion;
using ProyectoMPLS.Models.Topologia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoMPLS.Controllers
{
    public class ComunicacionController : Controller
    {
        // GET: Comunicacion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportFile()
        {
            List<Tabla> tablaDatos = new List<Tabla>();
            return View(tablaDatos);
        }

        [HttpPost]
        public ActionResult ImportFile(HttpPostedFileBase archivoCSV)
        {
            // Use the InputStream to get the actual stream sent.
            StreamReader csvreader = new StreamReader(archivoCSV.InputStream);
            List<Tabla> tablaDatos = new List<Tabla>();

            //Primera linea
            var line = csvreader.ReadLine();
            var values = line.Split(',');

            while (!csvreader.EndOfStream)
            {
                Tabla row = new Tabla();
                line = csvreader.ReadLine();
                values = line.Split(',');

                row.Hostname = values[0];
                row.OSPFRouterID = values[1];
                row.OSPFNeighborRouterID = values[2];
                row.OSPFNeighborIP = values[3];
                tablaDatos.Add(row);
            }
            Proyecto newModel = new Proyecto(1);
            newModel.GenerarTopologia(tablaDatos);

            return View(tablaDatos);
        }
    }
}