using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SauShelter.Models;

namespace SauShelter.Controllers
{
    public class ShelterModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ShelterModels
        public ActionResult Index()
        {
            return View(db.ShelterModels.ToList());
        }

        // GET: ShelterModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShelterModel shelterModel = db.ShelterModels.Find(id);
            if (shelterModel == null)
            {
                return HttpNotFound();
            }
            return View(shelterModel);
        }

        // GET: ShelterModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShelterModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Born,Telephone,Email")] ShelterModel shelterModel)
        {
            if (ModelState.IsValid)
            {
                db.ShelterModels.Add(shelterModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shelterModel);
        }

        // GET: ShelterModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShelterModel shelterModel = db.ShelterModels.Find(id);
            if (shelterModel == null)
            {
                return HttpNotFound();
            }
            return View(shelterModel);
        }

        // POST: ShelterModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Born,Telephone,Email")] ShelterModel shelterModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shelterModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shelterModel);
        }

        // GET: ShelterModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShelterModel shelterModel = db.ShelterModels.Find(id);
            if (shelterModel == null)
            {
                return HttpNotFound();
            }
            return View(shelterModel);
        }

        // POST: ShelterModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShelterModel shelterModel = db.ShelterModels.Find(id);
            db.ShelterModels.Remove(shelterModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
