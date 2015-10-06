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
            return View();
        }

        // GET: Topologia/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Topologia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Topologia/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Topologia/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
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
    }
}
