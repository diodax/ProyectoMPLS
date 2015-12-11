using ProyectoMPLS.Models.Comunicacion;
using ProyectoMPLS.Models.Topologia;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoMPLS.Controllers
{
    public class TopologiaController : Controller
    {
        // GET: Topologia
        public ActionResult Index()
        {
            string cUsuarioActual = Session["Usuario"].ToString();
            List<Proyecto> listaProyectos = new List<Proyecto>();
            listaProyectos = Proyecto.GetListaProyectos(cUsuarioActual);
            return View(listaProyectos);
        }

        #region LSP

        public ActionResult _IndexLSPs(int idProyecto)
        {
            List<LSP> listaLSPs = new List<LSP>();
            listaLSPs = LSP.SelectListaLSP(idProyecto);
            return PartialView(listaLSPs);
        }

        public ActionResult _CrearLSP()
        {
            LSP newModel = new LSP();
            return PartialView();
        }

        public ActionResult _EditarLSP(LSP newModel)
        {
            return PartialView();
        }

        //TODO: Esto
        public ActionResult _GetListaEnlaces (string idRouter, double nBandwidth)
        {
            return Json(1);
        }


        #endregion

        #region CrearProyecto

        // GET: Topologia/CrearProyectoVacio
        public ActionResult _CrearProyectoVacio()
        {
            string cUsuarioActual = Session["Usuario"].ToString();
            ProyectoVacioViewModel newModel = new ProyectoVacioViewModel();
            newModel.cUserName = cUsuarioActual;
            return PartialView(newModel);
        }

        // POST: Topologia/CrearProyectoVacio
        [HttpPost]
        public ActionResult _CrearProyectoVacio(ProyectoVacioViewModel newModel)
        {
            if (ModelState.IsValid)
            {
                newModel.InsertProyecto();
                //return RedirectToAction("Index");
                return Json(new { success = true });
            }
            else
            {
                return PartialView(newModel);
            }
        }

        public ActionResult CrearProyectoArchivo()
        {
            string cUsuarioActual = Session["Usuario"].ToString();
            ProyectoArchivoViewModel newModel = new ProyectoArchivoViewModel();
            newModel.cUserName = cUsuarioActual;
            return PartialView(newModel);
        }

        [HttpPost]
        public ActionResult CrearProyectoArchivo(ProyectoArchivoViewModel newModel, HttpPostedFileBase topologiaCVS)
        {
            if (ModelState.IsValid)
            {
                int idProyecto = newModel.InsertProyecto();
                List<Tabla> tablaDatos = new List<Tabla>();
                StreamReader csvreader = new StreamReader(topologiaCVS.InputStream);    // Use the InputStream to get the actual stream sent.

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
                Proyecto newProyecto = new Proyecto(idProyecto);
                newProyecto.GenerarTopologia(tablaDatos);

                return Json(new { success = true });
            }
            else
            {
                return PartialView(newModel);
            }
        }


        // GET: Topologia/CrearProyectoRP
        public ActionResult CrearProyectoRP()
        {
            string cUsuarioActual = Session["Usuario"].ToString();
            ProyectoRaspberryViewModel newModel = new ProyectoRaspberryViewModel();
            newModel.cUserName = cUsuarioActual;

            //Just for testing purposes
            newModel.cRaspberryIP = "192.168.43.117"; //"148.101.128.39";  "64.32.76.83";
            newModel.nPuerto = 15000;   //22;
            newModel.cRouterIP = "192.168.2.1";
            newModel.cCommunityString = "public";
            newModel.cFileName = "test_v1";

            return PartialView(newModel);
        }

        // POST: Topologia/CrearProyectoRP
        [HttpPost]
        public ActionResult CrearProyectoRP(ProyectoRaspberryViewModel newModel)
        {
            if (ModelState.IsValid)
            {
                int idProyecto = newModel.InsertProyecto();
                string answer = ConexionSSH.EjecutarOSPFDiscovery(newModel.cRaspberryIP, newModel.nPuerto, newModel.cRouterIP, newModel.cCommunityString);
                //string answer = ""; //ConexionSSH.EjecutarOSPFDiscovery(newModel.cRaspberryIP, newModel.nPuerto, newModel.cRouterIP, newModel.cCommunityString);

                List<Tabla> tablaDatos = new List<Tabla>();
                
                #region response
                /*
                answer = @"Hostname; OSPFRouterID; OSPFNeighborRouterID; OSPFNeighborIP\n\n" +
                           @"R6; 6.6.6.6; 5.5.5.5, 4.4.4.4; 10.0.4.5, 10.0.4.14\n\n" +
                           @"R5; 5.5.5.5; 1.1.1.1, 6.6.6.6, 4.4.4.4; 10.0.4.1, 10.0.4.6, 10.0.4.10\n\n" +
                           @"R4; 4.4.4.4; 3.3.3.3, 5.5.5.5, 6.6.6.6; 10.0.3.1, 10.0.4.9, 10.0.4.13\n\n" +
                           @"R4; 4.4.4.4; 3.3.3.3, 5.5.5.5, 6.6.6.6; 10.0.3.1, 10.0.4.9, 10.0.4.13\n\n" +
                           @"R1; 1.1.1.1; 2.2.2.2, 5.5.5.5; 10.0.1.2, 10.0.4.2\n\n" + 
                           @"R3; 3.3.3.3; 2.2.2.2, 4.4.4.4; 10.0.2.1, 10.0.3.2\n\n" +                     
                           @"R3; 3.3.3.3; 2.2.2.2, 4.4.4.4; 10.0.2.1, 10.0.3.2\n\n" +                          
                           @"R3; 3.3.3.3; 2.2.2.2, 4.4.4.4; 10.0.2.1, 10.0.3.2\n\n" +                          
                           @"R3; 3.3.3.3; 2.2.2.2, 4.4.4.4; 10.0.2.1, 10.0.3.2\n\n" +                        
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" +                         
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" +                         
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" +                          
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"R2; 2.2.2.2; 1.1.1.1, 3.3.3.3; 10.0.1.1, 10.0.2.2\n\n" + 
                           @"end of file\n";*/
                #endregion

                if (answer != "")
                {
                    string[] lines = answer.Split(new string[] { "\n\n" }, StringSplitOptions.None);
                    // TODO: Parsear el archivo CSV

                    List<string> templist = new List<string>();
                    templist = lines.ToList();
                    templist.RemoveAt(0);
                    templist.RemoveAt(templist.Count - 1);

                    foreach (var line in templist)
                    {
                        Tabla row = new Tabla();
                        var values = line.Split(';');

                        row.Hostname = values[0].Trim();
                        row.OSPFRouterID = values[1].Trim();
                        row.OSPFNeighborRouterID = values[2].Trim();
                        row.OSPFNeighborIP = values[3].Trim();
                        tablaDatos.Add(row);
                    }
                    Proyecto newModelP = new Proyecto(idProyecto);
                    newModelP.GenerarTopologia(tablaDatos);
                }
                //return RedirectToAction("Index");
                //return RedirectToAction("Editar", new { idProyecto = idProyecto });

                string url = Url.Action("Editar", "Topologia", new { idProyecto = idProyecto });
                return Json(new { success = true, url = url });
            }
            else
            {
                return PartialView(newModel);
            }
        }

        #endregion 

        // GET: Topologia/Editar/5
        public ActionResult Editar(int idProyecto)
        {
            Proyecto newModel = new Proyecto(idProyecto);
            return View(newModel);
        }

        // POST: Topologia/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Topologia/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Topologia/Delete/5
        [HttpPost]
        public ActionResult Eliminar(int idProyecto)
        {
            try
            {
                // TODO: Add delete logic here
                Proyecto.EliminarProyecto(idProyecto);

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult GetJsonTopologia(int idProyecto)
        {
            Proyecto temp = new Proyecto(idProyecto);
            return Json(new { routers = temp.listadoRouters, enlaces = temp.listadoEnlaces });
        }
    }
}
