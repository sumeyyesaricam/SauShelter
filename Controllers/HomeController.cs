using PagedList;
using SauShelter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SauShelter.Controllers
{
    public class HomeController : Controller
    {
        SauShelterEntities ilan = new SauShelterEntities();
        turkiyeEntities te = new turkiyeEntities();
        public ActionResult Index(Guid? id, string currentFilter, string AdvertName, int? EnDusuk, int? EnYuksek, Guid? OdaSayisi, int? Sehir, int? Ilce, int page = 1, int pageSize=10)
        {
            if (AdvertName != null)
            {
                page = 1;
            }
            else
            {
                AdvertName = currentFilter;
            }

            ViewBag.CurrentFilter = AdvertName;

            foreach (var insider in ilan.Insider)
            {
                if (insider.EMAIL == User.Identity.Name)
                {
                        ViewBag.Eİlanlarım = "Emlak İlanlarım";
                        ViewBag.Dİlanlarım = "Diğer İlanlarım";
                        ViewBag.Kisi = insider.ID;
                }
            }
            
            List<Advert> liste = new List<Advert>();
            var oda = ilan.RoomCount;
            ViewBag.OdaSayisi = new SelectList(oda, "ID", "NAME", OdaSayisi);
            var city = te.tbl_il;
            ViewBag.City = new SelectList(city, "il_id", "il_ad");
            var town = te.tbl_ilce;
            ViewBag.townn = new SelectList(town, "ilce_id", "ilce_ad");
            List<Address> adrs=new List<Address>();
            
            foreach(var drs in ilan.Address)
            {
                adrs.Add(drs);
            }
            foreach (var item in ilan.Advert)
            {
                List<Address> adrs1 = new List<Address>();
                List<Address> adrs2 = new List<Address>();
                if (id == item.ATYPEID || id == null)
                {
                    if (AdvertName != null)
                    {
                            if (item.TITLE.Contains(AdvertName))
                                liste.Add(item);
                    }
                    else
                        liste.Add(item);
                    if (EnDusuk != null)
                    {
                        var sonuc = liste.Where(lst => lst.COST >= EnDusuk).ToList();
                        if (sonuc.Count != 0)
                                liste.Remove(item);
                    }
                    if (EnYuksek != null)
                    {
                        var sonuc = liste.Where(lst => lst.COST <= EnYuksek).ToList();
                        if (sonuc.Count != 0)
                                liste.Remove(item);
                    }
                    if (OdaSayisi != null)
                    {
                        var sonuc = liste.Where(lst => lst.ROOMCOUNTID != OdaSayisi).ToList();                        
                            if (sonuc.Count!=0)
                                liste.Remove(item);
                    }
                    if (Sehir != null)
                    {
                        var sonuc=adrs.Where(lst=>lst.CITYID == Sehir).ToList();
                        if (sonuc.Count != 0)
                        {
                            foreach(var ekle in sonuc)
                            {
                                adrs1.Add(ekle);  
                            }
                        }
                        if (adrs1.Count != 0)
                        {
                            int say = 0;
                            foreach (var bak in adrs1)
                            {
                              if (item.ADDRESSID == bak.ID )
                                    say++;
                            }
                            if (say == 0)
                                liste.Remove(item);
                        }
                        else
                            liste.Remove(item);               
                    }                    
                    if (Ilce != null)
                    {
                        var alis = adrs.Where(lst => lst.TOWNID == Ilce).ToList();
                        if (alis.Count != 0)
                        {
                            foreach (var ekle in alis)
                            {
                                adrs2.Add(ekle);
                            }
                        }
                        if (adrs2.Count != 0)
                        {
                            int say = 0;
                            foreach (var bak in adrs2)
                            {
                                if (item.ADDRESSID == bak.ID)
                                    say++;
                            }
                            if (say == 0)
                                liste.Remove(item);
                        }
                        else
                            liste.Remove(item); 
                    }
                    foreach(var adres in ilan.Address)
                    {
                      if(adres.ID==item.ADDRESSID)
                      {
                          foreach(var ill in city)
                          {
                              if (ill.il_id == adres.CITYID)
                                  ViewBag.Shr = ill.il_ad;
                          }
                      }
                    }
                }}
            
               foreach(var son in ilan.Advert)
               {
                   int ay=0;
                   DateTime ilkdeger = son.ADVERTDATE;
                   foreach(var trh in ilan.DeliveryTime)
                   {
                       if (son.TIMEID == trh.ID)
                           ay = Convert.ToInt32(trh.NAME);
                   }
                   DateTime ilkdğr=ilkdeger.AddMonths(ay);
                   DateTime sondeger = DateTime.Now;
                   System.TimeSpan zaman = ilkdğr - sondeger; 
                  double days = zaman.TotalDays;
                  System.TimeSpan zmn = sondeger - ilkdeger; 
                  double dys = zmn.TotalDays;
                  if (days < 0 || dys<0)
                   {
                       liste.Remove(son);
                   }
               }
               
                if (liste.Count() == 0)
                    ViewBag.Mesaj = "Aradığınız İlan Mevcut Değil.";
                PagedList<Advert> advrtlist = new PagedList<Advert>(liste, page, pageSize);
                return View(advrtlist);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}