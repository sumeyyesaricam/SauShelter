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
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public InsidersController()
        {
        }

        public InsidersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Insiders
        public ActionResult Index()
        {
            var insider = db.Insider.Include(i => i.Address);
            return View(insider.ToList());
        }

        // GET: Insiders/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insider insider = db.Insider.Find(id);
            if (insider == null)
            {
                return HttpNotFound();
            }
            return View(insider);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(Insider imodel, LoginViewModel model, string returnUrl)
        {
            bool kontrol = false;
            bool kontrol1 = false;
            foreach (var item in db.Insider)
            {
                if (item.EMAIL == imodel.EMAIL)
                {
                    kontrol = true;
                    imodel.ID = item.ID;
                    if (item.PASSWORD == imodel.PASSWORD)
                        kontrol1 = true;
                }
            }
            if (!kontrol)
            {
                ViewBag.EMesage = "Sistemimizde bu e-posta adresi ve şifre ile kayıtlı üye bulunmamaktadır.";
            }
            if (kontrol && !kontrol1)
            {
                ViewBag.PMesage = "Şifreniz yanlış";
            }
            if (kontrol && kontrol1)
            {
                imodel.GENDER = true;
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToAction("Index", "Advert", new { id = imodel.ID });
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return RedirectToAction("Index", "Advert", new { id = imodel.ID });
                }
            }
            return View(model);
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
        public ActionResult Create(string City,string townn,string dstrct)
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
        public async Task<ActionResult> Create(RegisterViewModel model,string Acıklama, int? secimil, int? secimilce, int? secimMahalle, [Bind(Include = "ID,NAME,EMAIL,PHONENUMBER,PASSWORD,BIRTHDATE,GENDER,ADDRESSID,SURNAME,CONFIRMPASSWORD")] Insider insider)
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
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
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
                return RedirectToAction("Index", "Advert", new { insertid = insider.ID });
            }
                AddErrors(result);
            }
            return View();
        }

        // GET: Insiders/Edit/5
        public ActionResult Edit(Guid? id, string City, string townn, string dstrct)
        {
            var city = te.tbl_il;
            ViewBag.City = new SelectList(city, "il_id", "il_ad", City);
            var town = te.tbl_ilce;
            ViewBag.townn = new SelectList(town, "ilce_id", "ilce_ad", townn);
            var district = te.tbl_mahalle;
            ViewBag.dstrct = new SelectList(district, "mahalle_id", "mahalle_ad", dstrct);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insider insider = db.Insider.Find(id);
            if (insider == null)
            {
                return HttpNotFound();
            }
            ViewBag.ADDRESSID = new SelectList(db.Address, "ID", "EXPLANATION", insider.ADDRESSID);
            return View(insider);
        }

        // POST: Insiders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string City,string townn,string dstrct,[Bind(Include = "ID,NAME,EMAIL,PHONENUMBER,PASSWORD,BIRTHDATE,GENDER,ADDRESSID,SURNAME,CONFIRMPASSWORD")] Insider insider)
        {
            var city = te.tbl_il;
            ViewBag.City = new SelectList(city, "il_id", "il_ad", City);
            var town = te.tbl_ilce;
            ViewBag.townn = new SelectList(town, "ilce_id", "ilce_ad", townn);
            var district = te.tbl_mahalle;
            ViewBag.dstrct = new SelectList(district, "mahalle_id", "mahalle_ad", dstrct);

            if (ModelState.IsValid)
            {
                db.Entry(insider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ADDRESSID = new SelectList(db.Address, "ID", "EXPLANATION", insider.ADDRESSID);
            return View(insider);
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
        // GET: Insiders/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insider insider = db.Insider.Find(id);
            if (insider == null)
            {
                return HttpNotFound();
            }
            return View(insider);
        }

        // POST: Insiders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Insider insider = db.Insider.Find(id);
            db.Insider.Remove(insider);
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
