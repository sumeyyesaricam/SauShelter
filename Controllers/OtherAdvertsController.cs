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
    [Authorize]
    public class OtherAdvertsController : Controller
    {
        private SauShelterEntities db = new SauShelterEntities();

        // GET: OtherAdverts
        [AllowAnonymous]
        public ActionResult HomeIndex(Guid? id, string AdvertName)
        {
            List<OtherAdvert> liste = new List<OtherAdvert>();
            var otherAdvert = db.OtherAdvert.Include(o => o.Insider).Include(o => o.OtherTypes);
            foreach (var item in db.OtherAdvert)
            {
                if (id == item.OTYPEID || id == null)
                {
                    liste.Add(item);
                }
                else
                    ViewBag.Mesaj = "Aradığınız İlan Mevcut Değil";
            }
            return View(liste);
        }
        public ActionResult Index(Guid? id)
        {
            List<OtherAdvert> liste = new List<OtherAdvert>();
            var otherAdvert = db.OtherAdvert.Include(o => o.Insider).Include(o => o.OtherTypes);
            foreach (var item in db.OtherAdvert)
            {
                if (id == item.OWNERID || id == null)
                {
                    liste.Add(item);
                }
                else
                    ViewBag.Mesaj = "Size Ait ilan bulunmamaktadır.";
            }
            return View(liste);
        }

        // GET: OtherAdverts/Details/5
        [AllowAnonymous]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtherAdvert otherAdvert = db.OtherAdvert.Find(id);
            if (otherAdvert == null)
            {
                return HttpNotFound();
            }
            return View(otherAdvert);
        }

        // GET: OtherAdverts/Create
        public ActionResult Create()
        {
            ViewBag.OWNERID = new SelectList(db.Insider, "ID", "NAME");
            ViewBag.OTYPEID = new SelectList(db.OtherTypes, "ID", "NAME");
            ViewBag.TYPEID = new SelectList(db.Type, "ID", "NAME");
            return View();
        }

        // POST: OtherAdverts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OTYPEID,ADVERTDATE,EXPLANATION,TITLE,IMAGE,COST,OWNERID")] OtherAdvert otherAdvert)
        {
            if (ModelState.IsValid)
            {
                otherAdvert.ID = Guid.NewGuid();
                db.OtherAdvert.Add(otherAdvert);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TYPEID = new SelectList(db.Type, "ID", "NAME", otherAdvert.TYPEID);
            ViewBag.OWNERID = new SelectList(db.Insider, "ID", "NAME", otherAdvert.OWNERID);
            ViewBag.OTYPEID = new SelectList(db.OtherTypes, "ID", "NAME", otherAdvert.OTYPEID);
            return View(otherAdvert);
        }

        // GET: OtherAdverts/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtherAdvert otherAdvert = db.OtherAdvert.Find(id);
            if (otherAdvert == null)
            {
                return HttpNotFound();
            }
            ViewBag.OWNERID = new SelectList(db.Insider, "ID", "NAME", otherAdvert.OWNERID);
            ViewBag.TYPEID = new SelectList(db.Type, "ID", "NAME", otherAdvert.TYPEID);
            ViewBag.OTYPEID = new SelectList(db.OtherTypes, "ID", "NAME", otherAdvert.OTYPEID);
            return View(otherAdvert);
        }

        // POST: OtherAdverts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OTYPEID,TYPEID,ADVERTDATE,EXPLANATION,TITLE,IMAGE,COST,OWNERID")] OtherAdvert otherAdvert)
        {
            if (ModelState.IsValid)
            {
                Guid oid = Guid.NewGuid();
                foreach (var insider in db.Insider)
                {
                    if (insider.EMAIL == User.Identity.Name)
                    {
                        oid = insider.ID;
                        ViewBag.Telefon = insider.PHONENUMBER;
                        ViewBag.AID = insider.ID;
                    }
                }
                db.Entry(otherAdvert).State = EntityState.Modified;
                otherAdvert.OWNERID = oid;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TYPEID = new SelectList(db.Type, "ID", "NAME", otherAdvert.TYPEID);
            ViewBag.OWNERID = new SelectList(db.Insider, "ID", "NAME", otherAdvert.OWNERID);
            ViewBag.OTYPEID = new SelectList(db.OtherTypes, "ID", "NAME", otherAdvert.OTYPEID);
            return View(otherAdvert);
        }

        // POST: OtherAdverts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid did)
        {
            OtherAdvert otherAdvert = db.OtherAdvert.Find(did);
            db.OtherAdvert.Remove(otherAdvert);
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
