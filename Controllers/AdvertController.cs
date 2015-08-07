using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SauShelter.Models;
using System.Text;
using System.Threading;
using System.Net.Mail;
using System.Web.Helpers;
using System.IO;
using System.Threading.Tasks;


namespace SauShelter.Controllers
{
    [Authorize]
    public class AdvertController : Controller
    {
        private SauShelterEntities db = new SauShelterEntities();
        private turkiyeEntities te = new turkiyeEntities();
        // GET: Advert
        int say = 0;
        public ActionResult Index(Guid? id)
        {
            var advert = db.Advert.Include(a => a.Address).Include(a => a.AdvertType).Include(a => a.ApartmentFloor).Include(a => a.DeliveryTime).Include(a => a.Heating).Include(a => a.Insider).Include(a => a.RoomCount).Include(a => a.Type);
            var ilan=advert.ToList().Where(i => i.OWNERID == id);
            if (ilan.Count() == 0)
                ViewBag.ilanmsj = "Size ait ilan bulunmamaktadır.";
            return View(ilan);
        }
        [AllowAnonymous]
        public ActionResult Message()
        {
            return View();
        }
        [AllowAnonymous]
          [HttpPost]
        public async Task<ActionResult> Message(string mail, string konu, string ileti)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress("sumeyyesaricam@hotmail.com"));  // replace with valid value 
            message.From = new MailAddress("sumeyye98762@gmail.com");  // replace with valid value
            message.Subject = "Your email subject";
            message.Body = ileti;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "sumeyye98762@gmail.com",  // replace with valid value
                    Password = "şifre"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index","Home");
        }
        public string Gonderildi()
        {
            return "İletiniz başarıyla gönderildi";
        }

        // GET: Advert/Details/5
        [AllowAnonymous]
        public ActionResult Details(Guid? id)
        {
            
           if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advert advert = db.Advert.Find(id);
            List<String> rsmlist = new List<string>();
            var images = Server.MapPath("/Images");
            DirectoryInfo di = new DirectoryInfo(images);
             FileInfo[] rgFiles = di.GetFiles();
             foreach (var rsm in rgFiles)
            {
                string name = rsm.Name;
                var isim=rsm.Name.Split(',');
                if(isim[0].ToString()==advert.ID.ToString())
                {
                    rsmlist.Add(name);
                }
            }
             ViewBag.liste = rsmlist;    

            foreach(var adrs in db.Address)
            {
                if(adrs.ID==advert.ADDRESSID)
                {
                    foreach (var drs in te.tbl_il)
                    {
                        if (adrs.CITYID == drs.il_id)
                            ViewBag.Sehir = drs.il_ad;
                    }
                    foreach (var drs in te.tbl_ilce)
                    {
                        if (adrs.TOWNID == drs.ilce_id)
                            ViewBag.İlce = drs.ilce_ad;
                    }
                    foreach (var drs in te.tbl_mahalle)
                    {
                        if (adrs.DISTRICTID == drs.mahalle_id)
                            ViewBag.Mahalle = drs.mahalle_ad;
                    }
                }
            }

            if (advert == null)
            {
                return HttpNotFound();
            }
            return View(advert);
        }

        // GET: Advert/Create
        public ActionResult Create()
        {
            var city = te.tbl_il;
            ViewBag.City = new SelectList(city, "il_id", "il_ad");
            var town = te.tbl_ilce;
            ViewBag.townn = new SelectList(town, "ilce_id", "ilce_ad");
            var district = te.tbl_mahalle;
            ViewBag.dstrct = new SelectList(district, "mahalle_id", "mahalle_ad");
            foreach (var insider in db.Insider)
            {
                if (insider.EMAIL == User.Identity.Name)
                          ViewBag.Kisi = insider.ID;
            }
            ViewBag.ADDRESSID = new SelectList(db.Address, "ID", "EXPLANATION");
            ViewBag.ATYPEID = new SelectList(db.AdvertType, "ID", "NAME");
            ViewBag.FLOORID = new SelectList(db.ApartmentFloor, "ID", "NAME");
            ViewBag.TIMEID = new SelectList(db.DeliveryTime, "ID", "NAME");
            ViewBag.HEATINGID = new SelectList(db.Heating, "ID", "NAME");
            ViewBag.OWNERID = new SelectList(db.Insider, "ID", "NAME");
            ViewBag.ROOMCOUNTID = new SelectList(db.RoomCount, "ID", "NAME");
            ViewBag.TYPID = new SelectList(db.Type, "ID", "NAME");
            foreach (var insider in db.Insider)
            {
                if (insider.EMAIL == User.Identity.Name)
                {
                    ViewBag.Telefon = insider.PHONENUMBER;
                    ViewBag.AID = insider.ID;
                }
            }
            return View();
        }

        // POST: Advert/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IEnumerable<HttpPostedFileBase> SecilenDosyalar, string Acıklama, int? secimil, int? secimilce, int? secimMahalle, [Bind(Include = "ID,ADVERTDATE,ROOMCOUNTID,FLOORID,HEATINGID,EXPLANATION,TITLE,BATHCOUNT,COST,PERSONCOUNT,ATYPEID,BALCONY,GARDEN,CONSTRUCTIONDATE,ADDRESSID,MONTHLYFEE,FULLYFURNISHED,SQUAREFEET,TYPID,OWNERID,TIMEID,OTHER")] Advert advert)
        {
            var city = te.tbl_il;
            ViewBag.City = new SelectList(city, "il_id", "il_ad");
            var town = te.tbl_ilce;
            ViewBag.townn = new SelectList(town, "ilce_id", "ilce_ad");
            var district = te.tbl_mahalle;
            ViewBag.dstrct = new SelectList(district, "mahalle_id", "mahalle_ad");
            Guid oid=Guid.NewGuid();
            Address adres = new Address();
            if (secimil != null || secimilce != null || secimMahalle != null)
            {
                adres.CITYID = secimil;
                adres.DISTRICTID = secimMahalle;
                adres.TOWNID = secimilce;
                adres.EXPLANATION = Acıklama;
                adres.ID = Guid.NewGuid();
                advert.ADDRESSID = adres.ID;
                db.Address.Add(adres);
            }
            
            if (ModelState.IsValid)
            {
                advert.ADVERTDATE =  DateTime.Today.AddDays(+1);
                advert.ID = Guid.NewGuid();
                foreach(var insider in db.Insider)
                {
                    if (insider.EMAIL == User.Identity.Name)
                    {
                        oid = insider.ID;
                        ViewBag.Telefon = insider.PHONENUMBER;
                        ViewBag.AID = insider.ID;
                    }
                }                
                advert.OWNERID = oid;
                db.Advert.Add(advert);
                db.SaveChanges();
                foreach (var file in SecilenDosyalar)
                {
                    say++;
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/" + advert.ID +","+say + ".jpg"));
                }
                return RedirectToAction("Index/"+ oid);
            }

            ViewBag.ADDRESSID = new SelectList(db.Address, "ID", "EXPLANATION", advert.ADDRESSID);
            ViewBag.ATYPEID = new SelectList(db.AdvertType, "ID", "NAME", advert.ATYPEID);
            ViewBag.FLOORID = new SelectList(db.ApartmentFloor, "ID", "NAME", advert.FLOORID);
            ViewBag.TIMEID = new SelectList(db.DeliveryTime, "ID", "NAME", advert.TIMEID);
            ViewBag.HEATINGID = new SelectList(db.Heating, "ID", "NAME", advert.HEATINGID);
            ViewBag.OWNERID = new SelectList(db.Insider, "ID", "NAME", advert.OWNERID);
            ViewBag.ROOMCOUNTID = new SelectList(db.RoomCount, "ID", "NAME", advert.ROOMCOUNTID);
            ViewBag.TYPID = new SelectList(db.Type, "ID", "NAME", advert.TYPID);
            return View(advert);
        }

        // GET: Advert/Edit/5

        public ActionResult Edit(Guid? id)
        {
            var city = te.tbl_il;
            ViewBag.City = new SelectList(city, "il_id", "il_ad");
            var town = te.tbl_ilce;
            ViewBag.townn = new SelectList(town, "ilce_id", "ilce_ad");
            var district = te.tbl_mahalle;
            ViewBag.dstrct = new SelectList(district, "mahalle_id", "mahalle_ad");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advert advert = db.Advert.Find(id);
             foreach (var adrs in db.Address)
            {
                if (adrs.ID == advert.ADDRESSID)
                {
                    foreach (var drs in te.tbl_il)
                    {
                        if (adrs.CITYID == drs.il_id)
                            ViewBag.Sehir = drs.il_ad;
                    }
                    foreach (var drs in te.tbl_ilce)
                    {
                        if (adrs.TOWNID == drs.ilce_id)
                            ViewBag.İlce = drs.ilce_ad;
                    }
                    foreach (var drs in te.tbl_mahalle)
                    {
                        if (adrs.DISTRICTID == drs.mahalle_id)
                            ViewBag.Mahalle = drs.mahalle_ad;
                    }
                }
            }
            if (advert == null)
            {
                return HttpNotFound();
            }
            ViewBag.ADDRESSID = new SelectList(db.Address, "ID", "EXPLANATION", advert.ADDRESSID);
            ViewBag.ATYPEID = new SelectList(db.AdvertType, "ID", "NAME", advert.ATYPEID);
            ViewBag.FLOORID = new SelectList(db.ApartmentFloor, "ID", "NAME", advert.FLOORID);
            ViewBag.TIMEID = new SelectList(db.DeliveryTime, "ID", "NAME", advert.TIMEID);
            ViewBag.HEATINGID = new SelectList(db.Heating, "ID", "NAME", advert.HEATINGID);
            ViewBag.OWNERID = new SelectList(db.Insider, "ID", "NAME", advert.OWNERID);
            ViewBag.ROOMCOUNTID = new SelectList(db.RoomCount, "ID", "NAME", advert.ROOMCOUNTID);
            ViewBag.TYPID = new SelectList(db.Type, "ID", "NAME", advert.TYPID);
            return View(advert);
        }

        // POST: Advert/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IEnumerable<HttpPostedFileBase> SecilenDosyalar, string Acıklama, int? secimil, int? secimilce, int? secimMahalle, [Bind(Include = "ID,ADVERTDATE,ROOMCOUNTID,FLOORID,HEATINGID,EXPLANATION,TITLE,BATHCOUNT,COST,PERSONCOUNT,ATYPEID,BALCONY,GARDEN,CONSTRUCTIONDATE,ADDRESSID,MONTHLYFEE,FULLYFURNISHED,SQUAREFEET,TYPID,OWNERID,TIMEID,OTHER")] Advert advert)
        {
            var city = te.tbl_il;
            ViewBag.City = new SelectList(city, "il_id", "il_ad");
            var town = te.tbl_ilce;
            ViewBag.townn = new SelectList(town, "ilce_id", "ilce_ad");
            var district = te.tbl_mahalle;
            ViewBag.dstrct = new SelectList(district, "mahalle_id", "mahalle_ad");

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
                Address adres = new Address();
                if (secimil != null || secimilce != null || secimMahalle != null)
                {
                    adres.CITYID = secimil;
                    adres.DISTRICTID = secimMahalle;
                    adres.TOWNID = secimilce;
                    adres.EXPLANATION = Acıklama;
                    adres.ID = Guid.NewGuid();
                    advert.ADDRESSID = adres.ID;
                    db.Address.Add(adres);
                }
                if(SecilenDosyalar!=null)
                {
                    var images = Server.MapPath("/Images");
                    DirectoryInfo di = new DirectoryInfo(images);
                    FileInfo[] rgFiles = di.GetFiles();
                    foreach (var rsm in rgFiles)
                    {
                        string name = rsm.Name;
                        var isim = rsm.Name.Split(',');
                        if (isim[0].ToString() == advert.ID.ToString())
                        {
                            System.IO.File.Delete(Server.MapPath("~/Images/" + name));
                        }
                    }
                   
                   foreach (var file in SecilenDosyalar)
                {
                    say++;
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/" + advert.ID + ","+say + ".jpg"));
                }
                   }
                              
                advert.OWNERID = oid;
                db.Entry(advert).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index/" + oid);
            }

            ViewBag.ADDRESSID = new SelectList(db.Address, "ID", "EXPLANATION", advert.ADDRESSID);
            ViewBag.ATYPEID = new SelectList(db.AdvertType, "ID", "NAME", advert.ATYPEID);
            ViewBag.FLOORID = new SelectList(db.ApartmentFloor, "ID", "NAME", advert.FLOORID);
            ViewBag.TIMEID = new SelectList(db.DeliveryTime, "ID", "NAME", advert.TIMEID);
            ViewBag.HEATINGID = new SelectList(db.Heating, "ID", "NAME", advert.HEATINGID);
            ViewBag.OWNERID = new SelectList(db.Insider, "ID", "NAME", advert.OWNERID);
            ViewBag.ROOMCOUNTID = new SelectList(db.RoomCount, "ID", "NAME", advert.ROOMCOUNTID);
            ViewBag.TYPID = new SelectList(db.Type, "ID", "NAME", advert.TYPID);
            return View(advert);
        }

               [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid? did)
        {
            Guid oid = Guid.NewGuid();
            Advert advert = db.Advert.Find(did);
            db.Advert.Remove(advert);

            var images = Server.MapPath("/Images");
            DirectoryInfo di = new DirectoryInfo(images);
            FileInfo[] rgFiles = di.GetFiles();
            foreach (var rsm in rgFiles)
            {
                string name = rsm.Name;
                var isim = rsm.Name.Split(',');
                if (isim[0].ToString() == advert.ID.ToString())
                {
                    System.IO.File.Delete(Server.MapPath("~/Images/" + name));
                }
            }
                        
            db.SaveChanges();
            return RedirectToAction("Index/" + oid);
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
