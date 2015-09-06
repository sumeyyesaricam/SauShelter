using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SauShelter.Models;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Data.Entity.Validation;

namespace SauShelter.Controllers
{
    public class InsidersController : Controller
    {
        private SauShelterEntities db = new SauShelterEntities();
        private turkiyeEntities te = new turkiyeEntities();
       
        public ActionResult Login()
        {
            if (Session["user_id"] == null || Session["user_id"] == ("00000000-0000-0000-0000-000000000000"))
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<ActionResult> Login(Insider insider)
        {
            var insiders=db.Insider;
            Insider model = new Insider();
            foreach (var item in insiders)
                {
                    if (item.EMAIL == insider.EMAIL)
                    {
                        if (item.PASSWORD == insider.PASSWORD)
                        {
                            model.ID = item.ID;
                        }
                    }
                }
            var modelid = model.ID;
            var st = modelid.ToString();
            if (st == "00000000-0000-0000-0000-000000000000" || modelid == null)
            {
                ViewBag.Mesaj = "Girdiğiniz kullanıcı adı yada şifre hatalı.";
                return View();
            }
            else
            {
                Session["user_id"] = modelid;
                return RedirectToAction("Index", "Home");
            }
        }
             private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        // GET: Insiders/Create
             public ActionResult Register(string City, string townn, string dstrct)
        {
            var city = te.tbl_il;
            ViewBag.City = new SelectList(city, "il_id", "il_ad", City);
            var town = te.tbl_ilce;
            ViewBag.townn = new SelectList(town, "ilce_id", "ilce_ad", townn);
            var district = te.tbl_mahalle;
            ViewBag.dstrct = new SelectList(district, "mahalle_id", "mahalle_ad", dstrct);
            return View();
        }

        // POST: Insiders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model,string Acıklama, int? secimil, int? secimilce, int? secimMahalle, [Bind(Include = "ID,NAME,EMAIL,PHONENUMBER,PASSWORD,BIRTHDATE,GENDER,ADDRESSID,SURNAME,CONFIRMPASSWORD")] Insider insider)
        {
            var city = te.tbl_il;
            ViewBag.City = new SelectList(city, "il_id", "il_ad");
            var town = te.tbl_ilce;
            ViewBag.townn = new SelectList(town, "ilce_id", "ilce_ad");
            var district = te.tbl_mahalle;
            ViewBag.dstrct = new SelectList(district, "mahalle_id", "mahalle_ad");
            
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, };
                
                insider.ID = Guid.NewGuid();
                    Address adres = new Address();
                    if(secimil!=null || secimilce!=null || secimMahalle!=null)
                    {                        
                        adres.CITYID = secimil;
                        adres.DISTRICTID = secimMahalle;
                        adres.TOWNID = secimilce;
                        adres.EXPLANATION = Acıklama;
                        adres.ID = Guid.NewGuid();
                        insider.ADDRESSID = adres.ID;
                        db.Address.Add(adres);
                    }               
                db.Insider.Add(insider);               
                db.SaveChanges();
                Session["user_id"] = insider.ID;
                return RedirectToAction("Index", "Advert", new { insertid = insider.ID });
            }
            return View();
        }
        public ActionResult LogOff()
        {
            Session["user_id"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult GetirIlceler(string id)
        {
            var ilceler=te.tbl_ilce.Where(ilce=>ilce.il_id.ToString()==id)
                .Select(ilce => new IdAd
            {
                Id=ilce.ilce_id,
                Ad=ilce.ilce_ad
            }).ToList();
            return Json(ilceler, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetirMahalle(string id)
        {
            var mahalleler = te.tbl_mahalle.Where(mahalle => mahalle.semt_id.ToString() == id)
                .Select(mahalle => new IdAd
                {
                    Id = mahalle.mahalle_id,
                    Ad = mahalle.mahalle_ad
                }).ToList();
            return Json(mahalleler, JsonRequestBehavior.AllowGet);
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
