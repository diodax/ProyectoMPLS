using ProyectoMPLS.Models.Comunicacion;
using ProyectoMPLS.Models.Topologia;
using System;
using System.Collections.Generic;
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

        // GET: Topologia/CrearProyectoVacio
        public ActionResult CrearProyectoVacio()
        {
            string cUsuarioActual = Session["Usuario"].ToString();
            ProyectoVacioViewModel newModel = new ProyectoVacioViewModel();
            newModel.cUserName = cUsuarioActual;
            return View(newModel);
        }

        // POST: Topologia/CrearProyectoVacio
        [HttpPost]
        public ActionResult Create(ProyectoVacioViewModel newModel)
        {
            if (ModelState.IsValid)
            {
                newModel.InsertProyecto();
                return RedirectToAction("Index");
            }
            else
            {
                return View(newModel);
            }
        }

        // GET: Topologia/CrearProyectoRP
        public ActionResult CrearProyectoRP()
        {
            string cUsuarioActual = Session["Usuario"].ToString();
            ProyectoRaspberryViewModel newModel = new ProyectoRaspberryViewModel();
            newModel.cUserName = cUsuarioActual;
            return View(newModel);
        }

        // POST: Topologia/CrearProyectoRP
        [HttpPost]
        public ActionResult CrearProyectoRP(ProyectoRaspberryViewModel newModel)
        {
            if (ModelState.IsValid)
            {
                int idProyecto = newModel.InsertProyecto();
                return RedirectToAction("Index");
            }
            else
            {
                return View(newModel);
            }
        }

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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Topologia/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GetJsonTopologia(int idProyecto)
        {
            Proyecto temp = new Proyecto(idProyecto);
            return Json(new { routers = temp.listadoRouters, enlaces = temp.listadoEnlaces });
        }
    }
}
